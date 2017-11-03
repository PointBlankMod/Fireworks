using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.Unturned.Events;
using PointBlank.API.Implements;
using PointBlank.API.Unturned.Server;
using UnityEngine;

namespace Rocket.Unturned.Player
{
    public sealed class UnturnedPlayerFeatures : UnturnedPlayerComponent
    {
        #region Variables
        public DateTime Joined = DateTime.Now;

        private bool _VanishMode = false;
        private bool _GodMode = false;
        private Vector3 pPosition;
        #endregion

        #region Properties
        public bool VanishMode
        {
            get => _VanishMode;
            set
            {
                if (value)
                {
                    if (!Player._Player.Metadata.ContainsKey("Vanish"))
                        Player._Player.Metadata.Add("Vanish", true);

                    UnturnedServer.Players.ForEach((ply) =>
                    {
                        if (!ply.PlayerList.Contains(Player._Player))
                            return;

                        ply.RemovePlayer(Player._Player);
                    });
                }
                else
                {
                    if (Player._Player.Metadata.ContainsKey("Vanish"))
                        Player._Player.Metadata.Remove("Vanish");

                    UnturnedServer.Players.ForEach((ply) =>
                    {
                        if (ply.PlayerList.Contains(Player._Player))
                            return;

                        ply.AddPlayer(Player._Player);
                    });
                }
            }
        }

        public bool GodMode
        {
            get => _GodMode;
            set
            {
                if (value)
                {
                    if (!Player._Player.Metadata.ContainsKey("GodMode"))
                        Player._Player.Metadata.Add("GodMode", true);
                }
                else
                {
                    if (Player._Player.Metadata.ContainsKey("GodMode"))
                        Player._Player.Metadata.Remove("GodMode");
                }
            }
        }
        #endregion

        #region Mono Functions
        void FixedUpdate()
        {
            if(pPosition != Player.Position)
            {
                UnturnedPlayerEvents.fireOnPlayerUpdatePosition(Player);
                pPosition = Player.Position;
            }
        }
        #endregion
    }
}
