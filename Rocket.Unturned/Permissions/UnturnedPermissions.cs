using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SDG.Unturned;
using Steamworks;

namespace Rocket.Unturned.Permissions
{
    public class UnturnedPermissions
    {
        #region Handlers
        public delegate void JoinRequested(CSteamID player, ref ESteamRejection? rejectionReason);
        #endregion

        #region Events
        public static event JoinRequested OnJoinRequested;
        #endregion

        #region Functions
        public static void RunJoinRequested(CSteamID player, ref ESteamRejection? rejectionReason) => OnJoinRequested?.Invoke(player, ref rejectionReason);
        #endregion
    }
}
