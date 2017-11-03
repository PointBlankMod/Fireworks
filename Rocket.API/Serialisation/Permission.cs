using System;
using System.Xml.Serialization;

namespace Rocket.API.Serialisation
{
    [Serializable]
    public class Permission
    {
        [XmlAttribute]
        public uint Cooldown = 0;

        [XmlText]
        public string Name = "";

        public Permission() { }

        public Permission(string name, uint cooldown = 0)
        {
            Name = name;
            Cooldown = cooldown;
        }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is Permission))
                return false;

            return ((Permission)obj).Name == Name;
        }
    }
}
