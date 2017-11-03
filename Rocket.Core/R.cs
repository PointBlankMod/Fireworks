using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.API;
using Rocket.Core.Commands;
using Rocket.Core.Permissions;
using Rocket.Core.Plugins;

namespace Rocket.Core
{
    public class R
    {
        #region Variables
        public static R Instance = new R();
        public static RocketCommandManager Commands = new RocketCommandManager();
        public static IRocketPermissionsProvider Permissions = new RocketPermissionsManager();
        public static RocketPluginManager Plugins = new RocketPluginManager();
        #endregion
    }
}
