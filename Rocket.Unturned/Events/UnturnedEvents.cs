using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.API;
using Rocket.Unturned.Player;
using UnityEngine;

namespace Rocket.Unturned.Events
{
    public sealed class UnturnedEvents : IRocketImplementationEvents
    {
        #region Properties
        public static UnturnedEvents Instance { get; private set; }
        #endregion

        #region Handlers
        public delegate void PlayerDisconnected(UnturnedPlayer player);
        public delegate void PlayerConnected(UnturnedPlayer player);
        #endregion

        #region Events
        public event PlayerConnected OnPlayerConnected;
        public event PlayerConnected OnBeforePlayerConnected;
        public event ImplementationShutdown OnShutdown;
        public event PlayerDisconnected OnPlayerDisconnected;
        #endregion

        public UnturnedEvents() => Instance = this;

        #region Functions
        public void RunPlayerConnected(UnturnedPlayer player) => OnPlayerConnected?.Invoke(player);
        public void RunPlayerBeforeConnected(UnturnedPlayer player) => OnBeforePlayerConnected?.Invoke(player);
        public void RunShutdown() => OnShutdown?.Invoke();
        public void RunPlayerDisconnected(UnturnedPlayer player) => OnPlayerDisconnected?.Invoke(player);
        #endregion
    }
}
