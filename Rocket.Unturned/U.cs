using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.API;
using Rocket.Unturned.Events;
using SDG.Unturned;

namespace Rocket.Unturned
{
    public class U : IRocketImplementation
    {
        #region Variables
        public static UnturnedEvents Events = new UnturnedEvents();
        public static U Instance = new U();
        #endregion

        #region Properties
        public IRocketImplementationEvents ImplementationEvents => Events;

        public string InstanceId => Dedicator.serverID;

        public event RocketImplementationInitialized OnRocketImplementationInitialized;
        #endregion

        #region Functions
        public void RunInit() => OnRocketImplementationInitialized?.Invoke();

        public void Reload() { }
        public void Shutdown() { }
        #endregion
    }
}
