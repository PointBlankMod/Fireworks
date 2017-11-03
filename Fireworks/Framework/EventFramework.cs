using System;
using Fireworks.API;
using PointBlank.API;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using PointBlank.API.Plugins;
using PointBlank.API.Interfaces;
using PointBlank.API.Unturned.Server;
using SDG.Unturned;
using UnityEngine;
using Rocket.API;
using Rocket.Unturned;
using Rocket.API.Extensions;
using Rocket.Core;
using Rocket.Core.Commands;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using Rocket.Unturned.Permissions;
using Steamworks;
using PBPlayer = PointBlank.API.Unturned.Player.UnturnedPlayer;

namespace Fireworks.Framework
{
    internal class EventFramework : ILoadable
    {
        public void Load()
        {
            ChatManager.onChatted += handleChat;
            ServerEvents.OnServerInitialized += OnServerInitialized;
            ServerEvents.OnServerShutdown += ServerEvents_OnServerShutdown;
            ServerEvents.OnPlayerConnected += ServerEvents_OnPlayerConnected;
            ServerEvents.OnPlayerDisconnected += ServerEvents_OnPlayerDisconnected;
            ServerEvents.OnPlayerPreConnect += ServerEvents_OnPlayerPreConnect;
            PointBlankCommandEvents.OnCommandExecuted += PointBlankCommandEvents_OnCommandExecuted;
            PointBlankPluginEvents.OnPluginsLoaded += PointBlankPluginEvents_OnPluginsLoaded;
        }

        public void Unload()
        {
            ChatManager.onChatted -= handleChat;
            ServerEvents.OnServerInitialized -= OnServerInitialized;
            ServerEvents.OnServerShutdown -= ServerEvents_OnServerShutdown;
            ServerEvents.OnPlayerConnected -= ServerEvents_OnPlayerConnected;
            ServerEvents.OnPlayerDisconnected -= ServerEvents_OnPlayerDisconnected;
            ServerEvents.OnPlayerPreConnect -= ServerEvents_OnPlayerPreConnect;
            PointBlankCommandEvents.OnCommandExecuted -= PointBlankCommandEvents_OnCommandExecuted;
            PointBlankPluginEvents.OnPluginsLoaded -= PointBlankPluginEvents_OnPluginsLoaded;
        }

        #region Event Functions
        private void handleChat(SteamPlayer steamPlayer, EChatMode chatMode, ref Color incomingColor, string message, ref bool cancel)
        {
            cancel = false;
            Color color = incomingColor;
            try
            {
                UnturnedPlayer player = UnturnedPlayer.FromSteamPlayer(steamPlayer);
                color = UnturnedPlayerEvents.firePlayerChatted(player, chatMode, player.Color, message, ref cancel);
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Error in handling chat!", ex);
            }
            cancel = !cancel;
            incomingColor = color;
        }

        private void OnServerInitialized() => U.Instance.RunInit();
        private void ServerEvents_OnServerShutdown()
        {
            if (UnturnedEvents.Instance == null)
                return;

            UnturnedEvents.Instance.RunShutdown();
        }
        private void ServerEvents_OnPlayerConnected(PBPlayer player)
        {
            player.GameObject.TryAddComponent<UnturnedPlayerEvents>();
            player.GameObject.TryAddComponent<UnturnedPlayerFeatures>();

            UnturnedPlayer ply = new UnturnedPlayer(player);
            UnturnedEvents.Instance.RunPlayerBeforeConnected(ply);
            UnturnedEvents.Instance.RunPlayerConnected(ply);
        }
        private void ServerEvents_OnPlayerDisconnected(PBPlayer player) => UnturnedEvents.Instance.RunPlayerDisconnected(new UnturnedPlayer(player));
        private void ServerEvents_OnPlayerPreConnect(CSteamID steamID, ref ESteamRejection? reject) => UnturnedPermissions.RunJoinRequested(steamID, ref reject);
        private void PointBlankCommandEvents_OnCommandExecuted(PointBlankCommand command, string[] args, PointBlankPlayer executor, ref bool allowExecute)
        {
            if (!typeof(FireworksCommand).IsAssignableFrom(command.GetType()))
                return;
            bool cancel = false;
            IRocketPlayer rocketPlayer = (PointBlankPlayer.IsServer(executor) ? new ConsolePlayer() : (IRocketPlayer)new UnturnedPlayer((PBPlayer)executor));

            RocketCommandManager.Instance.RunExecuteCommand(rocketPlayer, ((FireworksCommand)command)._RocketCommand, ref cancel);
            allowExecute = !cancel;
        }
        private void PointBlankPluginEvents_OnPluginsLoaded() => R.Plugins.RunPluginsLoaded();
        #endregion
    }
}
