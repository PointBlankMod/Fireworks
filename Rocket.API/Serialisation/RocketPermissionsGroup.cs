using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using PointBlank.API.Groups;
using PointBlank.API.Permissions;

namespace Rocket.API.Serialisation
{
    [Serializable]
    public class RocketPermissionsGroup
    {
        public RocketPermissionsGroup()
        {
        }

        public RocketPermissionsGroup(PointBlankGroup group)
        {
            Id = group.ID;
            DisplayName = group.Name;
            Members = new List<string>();
            Permissions = new List<Permission>();
            ParentGroup = group.Inherits[0].ID;
            Color = group.Color.ToString();
            Priority = 100;

            foreach(PointBlankPermission permission in group.Permissions)
                Permissions.Add(new Permission(permission.Permission, (uint)permission.Cooldown));
        }

        public RocketPermissionsGroup(string id, string displayName, string parentGroup, List<string> members, List<Permission> permissions, string color = null, short priority = 100)
        {
            Id = id;
            DisplayName = displayName;
            Members = members;
            Permissions = permissions;
            ParentGroup = parentGroup;
            Color = color;
            Priority = priority;
        }

        [XmlElement("Id")]
        public string Id;

        [XmlElement("DisplayName")]
        public string DisplayName;

        [XmlElement("Prefix")]
        public string Prefix = "";

        [XmlElement("Suffix")]
        public string Suffix = "";

        [XmlElement("Color")]
        public string Color = "white";

        [XmlArray("Members")]
        [XmlArrayItem(ElementName = "Member")]
        public List<string> Members;

        [XmlArray("Commands")]
        [XmlArrayItem(ElementName = "Command")]
        public List<Permission> OldPermissions;

        public bool ShouldSerializeOldPermissions()
        {
            return OldPermissions != null && OldPermissions.Count != 0;
        }

        [XmlArray("Permissions")]
        [XmlArrayItem(ElementName = "Permission")]
        private List<Permission> permissions;
        public List<Permission> Permissions
        {
            get
            {
                if (OldPermissions != null)
                {
                    if (permissions == null) permissions = new List<Permission>();
                    permissions.AddRange(OldPermissions);
                    OldPermissions = null;
                }
                return permissions;
            }
            set
            {
                permissions = value;
            }
        }


        [XmlElement("ParentGroup")]
        public string ParentGroup;

        [XmlElement("Priority")]
        public short Priority = 100;
    }
}
