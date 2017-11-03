using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rocket.API
{
    public class ConsolePlayer : IRocketPlayer // Console peasants
    {
        public string Id => "Console";

        public string DisplayName => "Console";

        public bool IsAdmin => true;

        public int CompareTo(object obj) => Id.CompareTo(obj);
    }
}
