using System.Collections.Generic;
using Rocket.API;
using Rocket.API.Serialisation;
using Rocket.Unturned.Player;
using PointBlank.API.Groups;
using PointBlank.API.Permissions;
using UnityEngine;

namespace Rocket.Core.Permissions
{
    public sealed class RocketPermissionsManager : MonoBehaviour, IRocketPermissionsProvider
    {
        public void Reload()
        {
            PointBlankGroupManager.Reload();
        }

        public bool HasPermission(IRocketPlayer player, List<string> permissions) => ((UnturnedPlayer)player)._Player.HasPermissions(permissions.ToArray());

        public List<RocketPermissionsGroup> GetGroups(IRocketPlayer player, bool includeParentGroups)
        {
            List<RocketPermissionsGroup> groups = new List<RocketPermissionsGroup>();

            foreach(PointBlankGroup group in ((UnturnedPlayer)player)._Player.Groups)
                groups.Add(new RocketPermissionsGroup(group));
            return groups;
        }

        public List<Permission> GetPermissions(IRocketPlayer player)
        {
            List<Permission> permissions = new List<Permission>();

            foreach(PointBlankPermission permission in ((UnturnedPlayer)player)._Player.Permissions)
                permissions.Add(new Permission(permission.Permission, (uint)permission.Cooldown));
            return permissions;
        }

        public List<Permission> GetPermissions(IRocketPlayer player, List<string> requestedPermissions)
        {
            List<Permission> permissions = new List<Permission>();

            foreach(string permission in requestedPermissions)
                permissions.Add(new Permission(permission));
            return permissions;
        }

        public RocketPermissionsProviderResult AddPlayerToGroup(string groupId, IRocketPlayer player)
        {
            PointBlankGroup group = PointBlankGroup.Find(groupId);

            if (group == null)
                return RocketPermissionsProviderResult.GroupNotFound;
            ((UnturnedPlayer)player)._Player.AddGroup(group);
            return RocketPermissionsProviderResult.Success;
        }

        public RocketPermissionsProviderResult RemovePlayerFromGroup(string groupId, IRocketPlayer player)
        {
            PointBlankGroup group = PointBlankGroup.Find(groupId);

            if (group == null)
                return RocketPermissionsProviderResult.GroupNotFound;
            ((UnturnedPlayer)player)._Player.RemoveGroup(group);
            return RocketPermissionsProviderResult.Success;
        }

        public RocketPermissionsGroup GetGroup(string groupId)
        {
            PointBlankGroup group = PointBlankGroup.Find(groupId);

            if (group == null)
                return null;
            return new RocketPermissionsGroup(group);
        }

        public RocketPermissionsProviderResult SaveGroup(RocketPermissionsGroup group)
        {
            // Yea fucking sure
            return RocketPermissionsProviderResult.Success;
        }

        public RocketPermissionsProviderResult AddGroup(RocketPermissionsGroup group)
        {
            PointBlankGroupManager.AddGroup(group.Id, group.DisplayName, false, Color.white);

            return RocketPermissionsProviderResult.Success;
        }

        public RocketPermissionsProviderResult DeleteGroup(RocketPermissionsGroup group)
        {
            PointBlankGroupManager.RemoveGroup(group.Id);

            return RocketPermissionsProviderResult.Success;
        }

        public RocketPermissionsProviderResult DeleteGroup(string groupId)
        {
            PointBlankGroupManager.RemoveGroup(groupId);

            return RocketPermissionsProviderResult.Success;
        }
    }
}
