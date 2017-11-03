using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.API;

namespace Rocket.Core.Assets
{
    public class ConnectedAsset<T> : Asset<T> where T : class
    {
        public ConnectedAsset(T connection)
        {
            // Set the variables
            _Instance = connection;
        }
    }
}
