using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.API;
using Rocket.Core.Serialization;
using Rocket.Unturned.Player;
using Fireworks;
using Fireworks.Framework;
using Fireworks.API;
using PointBlank.API.Commands;
using SDG.Unturned;
using PBPlayer = PointBlank.API.Unturned.Player.UnturnedPlayer;

namespace Rocket.Core.Commands
{
    public class RocketCommandManager
    {
        #region Handlers
        public delegate void ExecuteCommand(IRocketPlayer player, IRocketCommand command, ref bool cancel);
        #endregion

        #region Events
        public event ExecuteCommand OnExecuteCommand;
        #endregion

        #region Variables
        public static RocketCommandManager Instance;
        #endregion

        public RocketCommandManager() => Instance = this;

        #region Functions
        public void RunExecuteCommand(IRocketPlayer player, IRocketCommand command, ref bool cancel) => OnExecuteCommand?.Invoke(player, command, ref cancel);

        public IRocketCommand GetCommand(string command) =>
            ((FireworksCommand)PointBlankCommandManager.Commands.FirstOrDefault(a => a.Commands.Count(b => b.ToLower() == command.ToLower()) > 0 && typeof(IRocketCommand).IsAssignableFrom(a.GetType())))._RocketCommand;

        public bool Register(IRocketCommand command)
        {
            Register(command, null);
            return true;
        }
        public void Register(IRocketCommand command, string alias)
        {
            Register(command, alias, CommandPriority.Normal);
        }
        public void Register(IRocketCommand command, string alias, CommandPriority priority) =>
            CommandFramework.RegisterCommand(command);

        public void DeregisterFromAssembly(Assembly assembly)
        {
            foreach (Type t in assembly.GetTypes())
                CommandFramework.UnregisterCommand(t);
        }

        public double GetCooldown(IRocketPlayer player, IRocketCommand command) => 0.0; // TODO: Make this functional

        public void SetCooldown(IRocketPlayer player, IRocketCommand command)
        {
            // Nothing yet
        }

        public bool Execute(IRocketPlayer player, string command)
        {
            if (player is ConsolePlayer)
                CommandWindow.input.onInputText(command);
            else
                PBPlayer.Get(((UnturnedPlayer)player).CSteamID).Sudo(command);

            return true;
        }

        public void RegisterFromAssembly(Assembly assembly)
        {
            foreach (Type t in assembly.GetTypes())
                CommandFramework.RegisterCommand(t);
        }
        #endregion
    }
}
