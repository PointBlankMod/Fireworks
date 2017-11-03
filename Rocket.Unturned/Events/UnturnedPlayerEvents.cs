using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.Core.Extensions;
using Rocket.Unturned.Enumerations;
using Rocket.Unturned.Player;
using UnityEngine;
using SDG.Unturned;
using Steamworks;

namespace Rocket.Unturned.Events
{
    public sealed class UnturnedPlayerEvents : UnturnedPlayerComponent
    {
        #region Enums
        public enum PlayerGesture { None, InventoryOpen, InventoryClose, Pickup, PunchLeft, PunchRight, SurrenderStart, SurrenderStop, Point, Wave, Salute, Arrest_Start, Arrest_Stop, Rest_Start, Rest_Stop, Facepalm }
        public enum Wearables { Hat = 0, Mask = 1, Vest = 2, Pants = 3, Shirt = 4, Glasses = 5, Backpack = 6 };
        #endregion

        #region Handlers
        public delegate void PlayerUpdatePosition(UnturnedPlayer player, Vector3 position);
        public delegate void PlayerUpdateBleeding(UnturnedPlayer player, bool bleeding);
        public delegate void PlayerUpdateBroken(UnturnedPlayer player, bool broken);
        public delegate void PlayerDeath(UnturnedPlayer player, EDeathCause cause, ELimb limb, CSteamID murderer);
        public delegate void PlayerDead(UnturnedPlayer player, Vector3 position);
        public delegate void PlayerUpdateLife(UnturnedPlayer player, byte life);
        public delegate void PlayerUpdateFood(UnturnedPlayer player, byte food);
        public delegate void PlayerUpdateHealth(UnturnedPlayer player, byte health);
        public delegate void PlayerUpdateVirus(UnturnedPlayer player, byte virus);
        public delegate void PlayerUpdateWater(UnturnedPlayer player, byte water);
        public delegate void PlayerUpdateGesture(UnturnedPlayer player, PlayerGesture gesture);
        public delegate void PlayerUpdateStance(UnturnedPlayer player, byte stance);
        public delegate void PlayerRevive(UnturnedPlayer player, Vector3 position, byte angle);
        public delegate void PlayerUpdateStat(UnturnedPlayer player, EPlayerStat stat);
        public delegate void PlayerUpdateExperience(UnturnedPlayer player, uint experience);
        public delegate void PlayerUpdateStamina(UnturnedPlayer player, byte stamina);
        public delegate void PlayerInventoryUpdated(UnturnedPlayer player, InventoryGroup inventoryGroup, byte inventoryIndex, ItemJar P);
        public delegate void PlayerInventoryResized(UnturnedPlayer player, InventoryGroup inventoryGroup, byte O, byte U);
        public delegate void PlayerInventoryRemoved(UnturnedPlayer player, InventoryGroup inventoryGroup, byte inventoryIndex, ItemJar P);
        public delegate void PlayerInventoryAdded(UnturnedPlayer player, InventoryGroup inventoryGroup, byte inventoryIndex, ItemJar P);
        public delegate void PlayerChatted(UnturnedPlayer player, ref Color color, string message, EChatMode chatMode, ref bool cancel);
        public delegate void PlayerWear(UnturnedPlayer player, Wearables wear, ushort id, byte? quality);
        #endregion

        #region Events
        public static event PlayerUpdatePosition OnPlayerUpdatePosition;

        public static event PlayerUpdateBleeding OnPlayerUpdateBleeding;
        public event PlayerUpdateBleeding OnUpdateBleeding;

        public static event PlayerUpdateBroken OnPlayerUpdateBroken;
        public event PlayerUpdateBroken OnUpdateBroken;

        public static event PlayerDeath OnPlayerDeath;
        public event PlayerDeath OnDeath;

        public static event PlayerDead OnPlayerDead;
        public event PlayerDead OnDead;

        public static event PlayerUpdateLife OnPlayerUpdateLife;
        public event PlayerUpdateLife OnUpdateLife;

        public static event PlayerUpdateFood OnPlayerUpdateFood;
        public event PlayerUpdateFood OnUpdateFood;

        public static event PlayerUpdateHealth OnPlayerUpdateHealth;
        public event PlayerUpdateHealth OnUpdateHealth;

        public static event PlayerUpdateVirus OnPlayerUpdateVirus;
        public event PlayerUpdateVirus OnUpdateVirus;

        public static event PlayerUpdateWater OnPlayerUpdateWater;
        public event PlayerUpdateWater OnUpdateWater;

        public static event PlayerUpdateGesture OnPlayerUpdateGesture;
        public event PlayerUpdateGesture OnUpdateGesture;

        public static event PlayerUpdateStance OnPlayerUpdateStance;
        public event PlayerUpdateStance OnUpdateStance;

        public static event PlayerRevive OnPlayerRevive;
        public event PlayerRevive OnRevive;

        public static event PlayerUpdateStat OnPlayerUpdateStat;
        public event PlayerUpdateStat OnUpdateStat;

        public static event PlayerUpdateExperience OnPlayerUpdateExperience;
        public event PlayerUpdateExperience OnUpdateExperience;

        public static event PlayerUpdateStamina OnPlayerUpdateStamina;
        public event PlayerUpdateStamina OnUpdateStamina;

        public static event PlayerInventoryUpdated OnPlayerInventoryUpdated;
        public event PlayerInventoryUpdated OnInventoryUpdated;

        public static event PlayerInventoryResized OnPlayerInventoryResized;
        public event PlayerInventoryResized OnInventoryResized;

        public static event PlayerInventoryRemoved OnPlayerInventoryRemoved;
        public event PlayerInventoryRemoved OnInventoryRemoved;

        public static event PlayerInventoryAdded OnPlayerInventoryAdded;
        public event PlayerInventoryAdded OnInventoryAdded;

        public static event PlayerChatted OnPlayerChatted;

        public static event PlayerWear OnPlayerWear;
        #endregion

        protected override void Load()
        {
            Player.Player.life.onStaminaUpdated += onUpdateStamina;
            Player.Player.inventory.onInventoryAdded += onInventoryAdded;
            Player.Player.inventory.onInventoryRemoved += onInventoryRemoved;
            Player.Player.inventory.onInventoryResized += onInventoryResized;
            Player.Player.inventory.onInventoryUpdated += onInventoryUpdated;

            SteamChannel.onTriggerSend += TriggerSend;
        }

        #region Event Functions
        private static void TriggerSend(SteamPlayer s, string W, ESteamCall X, ESteamPacket l, params object[] R)
        {
            try
            {
                if (s == null || s.player == null || s.playerID.steamID == CSteamID.Nil || s.player.transform == null || R == null) return;
                UnturnedPlayerEvents instance = s.player.transform.GetComponent<UnturnedPlayerEvents>();
                UnturnedPlayer rp = UnturnedPlayer.FromSteamPlayer(s);

                if (W.StartsWith("tellWear"))
                {
                    OnPlayerWear.TryInvoke(rp, Enum.Parse(typeof(Wearables), W.Replace("tellWear", "")), (ushort)R[0], R.Count() > 1 ? (byte?)R[1] : null);
                }
                switch (W)
                {
                    case "tellBleeding":
                        OnPlayerUpdateBleeding.TryInvoke(rp, (bool)R[0]);
                        instance.OnUpdateBleeding.TryInvoke(rp, (bool)R[0]);
                        break;
                    case "tellBroken":
                        OnPlayerUpdateBroken.TryInvoke(rp, (bool)R[0]);
                        instance.OnUpdateBroken.TryInvoke(rp, (bool)R[0]);
                        break;
                    case "tellLife":
                        OnPlayerUpdateLife.TryInvoke(rp, (byte)R[0]);
                        instance.OnUpdateLife.TryInvoke(rp, (byte)R[0]);
                        break;
                    case "tellFood":
                        OnPlayerUpdateFood.TryInvoke(rp, (byte)R[0]);
                        instance.OnUpdateFood.TryInvoke(rp, (byte)R[0]);
                        break;
                    case "tellHealth":
                        OnPlayerUpdateHealth.TryInvoke(rp, (byte)R[0]);
                        instance.OnUpdateHealth.TryInvoke(rp, (byte)R[0]);
                        break;
                    case "tellVirus":
                        OnPlayerUpdateVirus.TryInvoke(rp, (byte)R[0]);
                        instance.OnUpdateVirus.TryInvoke(rp, (byte)R[0]);
                        break;
                    case "tellWater":
                        OnPlayerUpdateWater.TryInvoke(rp, (byte)R[0]);
                        instance.OnUpdateWater.TryInvoke(rp, (byte)R[0]);
                        break;
                    case "tellStance":
                        OnPlayerUpdateStance.TryInvoke(rp, (byte)R[0]);
                        instance.OnUpdateStance.TryInvoke(rp, (byte)R[0]);
                        break;
                    case "tellGesture":
                        OnPlayerUpdateGesture.TryInvoke(rp, (PlayerGesture)Enum.Parse(typeof(PlayerGesture), R[0].ToString()));
                        instance.OnUpdateGesture.TryInvoke(rp, (PlayerGesture)Enum.Parse(typeof(PlayerGesture), R[0].ToString()));
                        break;
                    case "tellStat":
                        OnPlayerUpdateStat.TryInvoke(rp, (EPlayerStat)(byte)R[0]);
                        instance.OnUpdateStat.TryInvoke(rp, (EPlayerStat)(byte)R[0]);
                        break;
                    case "tellExperience":
                        OnPlayerUpdateExperience.TryInvoke(rp, (uint)R[0]);
                        instance.OnUpdateExperience.TryInvoke(rp, (uint)R[0]);
                        break;
                    case "tellRevive":
                        OnPlayerRevive.TryInvoke(rp, (Vector3)R[0], (byte)R[1]);
                        instance.OnRevive.TryInvoke(rp, (Vector3)R[0], (byte)R[1]);
                        break;
                    case "tellDead":
                        OnPlayerDead.TryInvoke(rp, (Vector3)R[0]);
                        instance.OnDead.TryInvoke(rp, (Vector3)R[0]);
                        break;
                    case "tellDeath":
                        OnPlayerDeath.TryInvoke(rp, (EDeathCause)(byte)R[0], (ELimb)(byte)R[1], new CSteamID(ulong.Parse(R[2].ToString())));
                        instance.OnDeath.TryInvoke(rp, (EDeathCause)(byte)R[0], (ELimb)(byte)R[1], new CSteamID(ulong.Parse(R[2].ToString())));
                        break;
                    default:
                        break;
                }
                return;
            }
            catch (Exception ex)
            {
                Core.Logging.Logger.LogException(ex, "Failed to receive packet \"" + W + "\"");
            }
        }
        public static Color firePlayerChatted(UnturnedPlayer player, EChatMode chatMode, Color color, string msg, ref bool cancel)
        {
            OnPlayerChatted?.Invoke(player, ref color, msg, chatMode, ref cancel);

            return color;
        }
        public static void fireOnPlayerUpdatePosition(UnturnedPlayer player) => OnPlayerUpdatePosition.TryInvoke(player, player.Position);

        private void onUpdateStamina(byte stamina)
        {
            OnPlayerUpdateStamina.TryInvoke(Player, stamina);
            OnUpdateStamina.TryInvoke(Player, stamina);
        }
        private void onInventoryUpdated(byte E, byte O, ItemJar P)
        {
            OnPlayerInventoryUpdated.TryInvoke(Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), O, P);
            OnInventoryUpdated.TryInvoke(Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), O, P);
        }
        private void onInventoryResized(byte E, byte M, byte U)
        {
            OnPlayerInventoryResized.TryInvoke(Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), M, U);
            OnInventoryResized.TryInvoke(Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), M, U);
        }
        private void onInventoryRemoved(byte E, byte y, ItemJar f)
        {
            OnPlayerInventoryRemoved.TryInvoke(Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), y, f);
            OnInventoryRemoved.TryInvoke(Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), y, f);
        }
        private void onInventoryAdded(byte E, byte u, ItemJar J)
        {
            OnPlayerInventoryAdded.TryInvoke(Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), u, J);
            OnInventoryAdded.TryInvoke(Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), u, J);
        }
        #endregion
    }
}
