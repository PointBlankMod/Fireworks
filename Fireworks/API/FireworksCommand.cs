using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Commands;
using PointBlank.API.Player;
using Rocket.API;
using Rocket.Unturned.Player;
using PBPlayer = PointBlank.API.Unturned.Player.UnturnedPlayer;

namespace Fireworks.API
{
    public class FireworksCommand : PointBlankCommand
    {
        #region Variables
        public IRocketCommand _RocketCommand;

        private List<string> _Commands = new List<string>();
        #endregion

        #region Properties
        public override string Name => _RocketCommand.Name;

        public override EAllowedCaller AllowedCaller
        {
            get
            {
                switch (_RocketCommand.AllowedCaller)
                {
                    case Rocket.API.AllowedCaller.Console:
                        return EAllowedCaller.SERVER;
                    case Rocket.API.AllowedCaller.Player:
                        return EAllowedCaller.PLAYER;
                    default:
                        return EAllowedCaller.BOTH;
                }
            }
        }

        public override string[] DefaultCommands => _Commands.ToArray();

        public override string Help => _RocketCommand.Help;

        public override string Usage => Commands[0] + _RocketCommand.Syntax;

        public override string DefaultPermission => (_RocketCommand.Permissions.Count > 0 ? _RocketCommand.Permissions[0] : "");
        #endregion

        public FireworksCommand(IRocketCommand command)
        {
            // Set the variables
            _RocketCommand = command;

            // Run the code
            _Commands.Add(_RocketCommand.Name);
            _RocketCommand.Aliases.ForEach(a => _Commands.Add(a));
        }

        #region Functions
        public override void Execute(PointBlankPlayer executor, string[] args) =>
            _RocketCommand.Execute((PointBlankPlayer.IsServer(executor) ? new ConsolePlayer() : (IRocketPlayer)(new UnturnedPlayer((PBPlayer)executor))), args);
        #endregion
    }
}
