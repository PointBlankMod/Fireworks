using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.API;
using Rocket.Core.Steam;
using Rocket.Unturned.Events;
using Rocket.Unturned.Skills;
using SDG.Unturned;
using UnityEngine;
using Steamworks;
using PBPlayer = PointBlank.API.Unturned.Player.UnturnedPlayer;

namespace Rocket.Unturned.Player
{
    public class UnturnedPlayer : IRocketPlayer
    {
        #region Variables
        public PBPlayer _Player;
        #endregion

        #region Properties
        public string Id => _Player.SteamID.ToString();

        public string DisplayName => _Player.CharacterName;

        public bool IsAdmin => _Player.IsAdmin;

        public Profile SteamProfile => new Profile(_Player.SteamID.m_SteamID);

        public SDG.Unturned.Player Player => _Player.Player;

        public CSteamID CSteamID => _Player.SteamID;

        public Color Color => _Player.GetColor();

        public float Ping => Player.channel.owner.ping;

        public UnturnedPlayerFeatures Features => GetComponent<UnturnedPlayerFeatures>();

        public UnturnedPlayerEvents Events => GetComponent<UnturnedPlayerEvents>();

        public string IP => _Player.IP;

        public PlayerInventory Inventory => _Player.Inventory;

        public CSteamID SteamGroupID => Player.channel.owner.playerID.group;

        public bool VanishMode
        {
            get => Features.VanishMode;
            set => Features.VanishMode = value;
        }

        public bool GodMode
        {
            get => Features.GodMode;
            set => Features.GodMode = value;
        }

        public Vector3 Position => Player.transform.position;

        public EPlayerStance Stance => Player.stance.stance;

        public float Rotation => Player.transform.rotation.eulerAngles.y;

        public byte Stamina => Player.life.stamina;

        public string CharacterName => Player.channel.owner.playerID.characterName;

        public string SteamName => Player.channel.owner.playerID.playerName;

        public byte Infection
        {
            get => Player.life.virus;
            set
            {
                Player.life.askDisinfect(100);
                Player.life.askInfect(value);
            }
        }

        public uint Experience
        {
            get => Player.skills.experience;
            set
            {
                Player.skills.channel.send("tellExperience", ESteamCall.SERVER, ESteamPacket.UPDATE_RELIABLE_BUFFER, value);
                Player.skills.channel.send("tellExperience", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, value);
            }
        }

        public int Reputation
        {
            get => Player.skills.reputation;
            set => Player.skills.askRep(value);
        }

        public byte Health => Player.life.health;

        public byte Hunger
        {
            get => Player.life.food;
            set
            {
                Player.life.askEat(100);
                Player.life.askStarve(value);
            }
        }

        public byte Thirst
        {
            get => Player.life.water;

            set
            {
                Player.life.askDrink(100);
                Player.life.askDehydrate(value);
            }
        }

        public bool Broken
        {
            get => Player.life.isBroken;
            set
            {
                Player.life.tellBroken(Provider.server, value);
                Player.life.channel.send("tellBroken", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[] { value });
            }
        }

        public bool Bleeding
        {
            get => Player.life.isBleeding;
            set
            {
                Player.life.tellBleeding(Provider.server, value);
                Player.life.channel.send("tellBleeding", ESteamCall.OWNER, ESteamPacket.UPDATE_RELIABLE_BUFFER, new object[] { value });
            }
        }

        public bool Dead => Player.life.isDead;

        public bool IsPro => Player.channel.owner.isPro;

        public InteractableVehicle CurrentVehicle => Player.movement.getVehicle();

        public bool IsInVehicle => CurrentVehicle != null;
        #endregion

        public UnturnedPlayer() { }
        public UnturnedPlayer(PBPlayer player) => _Player = player;

        #region Static Functions
        public static UnturnedPlayer FromName(string name)
        {
            if (!PBPlayer.TryGetPlayer(name, out PBPlayer player))
                return null;

            UnturnedPlayer ply = new UnturnedPlayer();
            ply._Player = player;

            return ply;
        }

        public static UnturnedPlayer FromCSteamID(CSteamID steamID)
        {
            UnturnedPlayer ply = new UnturnedPlayer();
            ply._Player = PBPlayer.Get(steamID);
            if (ply._Player == null)
                return null;

            return ply;
        }

        public static UnturnedPlayer FromPlayer(SDG.Unturned.Player player)
        {
            UnturnedPlayer ply = new UnturnedPlayer();
            ply._Player = PBPlayer.Get(player);
            if (ply._Player == null)
                return null;

            return ply;
        }

        public static UnturnedPlayer FromSteamPlayer(SteamPlayer player)
        {
            UnturnedPlayer ply = new UnturnedPlayer();
            ply._Player = PBPlayer.Get(player);
            if (ply._Player == null)
                return null;

            return ply;
        }
        #endregion

        #region Functions
        public int CompareTo(object obj) => Id.CompareTo(obj);

        public bool Equals(UnturnedPlayer p)
        {
            if (p == null)
                return false;

            return (CSteamID.ToString() == p.CSteamID.ToString());
        }

        public T GetComponent<T>() => Player.GetComponent<T>();

        public override string ToString() => CSteamID.ToString();

        public void TriggerEffect(ushort effectID) => 
            EffectManager.instance.channel.send("tellEffectPoint", CSteamID, ESteamPacket.UPDATE_UNRELIABLE_BUFFER, new object[] { effectID, Player.transform.position });

        public void MaxSkills()
        {
            foreach (Skill skill in (Player.skills.skills.SelectMany((Skill[] skills) => skills)))
                skill.level = skill.max;
        }

        public string SteamGroupName()
        {
            FriendsGroupID_t id;
            id.m_FriendsGroupID = (short)SteamGroupID.m_SteamID;
            return SteamFriends.GetFriendsGroupName(id);
        }

        public int SteamGroupMembersCount()
        {
            FriendsGroupID_t id;
            id.m_FriendsGroupID = (short)SteamGroupID.m_SteamID;
            return SteamFriends.GetFriendsGroupMembersCount(id);
        }

        public SteamPlayer SteamPlayer() => _Player.SteamPlayer;

        public bool GiveItem(ushort itemId, byte amount) => ItemTool.tryForceGiveItem(Player, itemId, amount);
        public bool GiveItem(Item item) => Player.inventory.tryAddItem(item, false);

        public bool GiveVehicle(ushort vehicleId) => VehicleTool.giveVehicle(Player, vehicleId);

        public void Kick(string reason) => Provider.kick(CSteamID, reason);

        public void Ban(string reason, uint duration) => Provider.ban(CSteamID, reason, duration);

        public void Admin(bool admin) => Admin(admin, null);
        public void Admin(bool admin, UnturnedPlayer issuer)
        {
            if (admin)
            {
                if (issuer == null)
                    SteamAdminlist.admin(CSteamID, new CSteamID(0));
                else
                    SteamAdminlist.admin(CSteamID, issuer.CSteamID);
            }
            else
            {
                SteamAdminlist.unadmin(Player.channel.owner.playerID.steamID);
            }
        }

        public void Teleport(UnturnedPlayer target)
        {
            Vector3 d1 = target.Player.transform.position;
            Vector3 vector31 = target.Player.transform.rotation.eulerAngles;
            Teleport(d1, MeasurementTool.angleToByte(vector31.y));
        }
        public void Teleport(Vector3 position, float rotation) =>
            Player.channel.send("askTeleport", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, position, MeasurementTool.angleToByte(rotation));
        public bool Teleport(string nodeName)
        {
            Node node = LevelNodes.nodes.Where(n => n.type == ENodeType.LOCATION && ((LocationNode)n).name.ToLower().Contains(nodeName)).FirstOrDefault();
            if (node != null)
            {
                Vector3 c = node.point + new Vector3(0f, 0.5f, 0f);
                Player.sendTeleport(c, MeasurementTool.angleToByte(Rotation));
                return true;
            }
            return false;
        }

        public void Heal(byte amount) => Heal(amount, null, null);
        public void Heal(byte amount, bool? bleeding, bool? broken) =>
            Player.life.askHeal(amount, bleeding != null ? bleeding.Value : Player.life.isBleeding, broken != null ? broken.Value : Player.life.isBroken);

        public void Suicide() => Player.life.askSuicide(Player.channel.owner.playerID.steamID);

        public EPlayerKill Damage(byte amount, Vector3 direction, EDeathCause cause, ELimb limb, CSteamID damageDealer)
        {
            EPlayerKill playerKill;
            Player.life.askDamage(amount, direction, cause, limb, damageDealer, out playerKill);
            return playerKill;
        }

        public void SetSkillLevel(UnturnedSkill skill, byte level)
        {
            GetSkill(skill).level = level;
            Player.skills.askSkills(CSteamID);
        }

        public byte GetSkillLevel(UnturnedSkill skill) => GetSkill(skill).level;

        public Skill GetSkill(UnturnedSkill skill)
        {
            var skills = Player.skills;
            return skills.skills[skill.Speciality][skill.Skill];
        }
        #endregion
    }
}
