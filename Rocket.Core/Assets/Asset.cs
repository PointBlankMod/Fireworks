using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rocket.API;

namespace Rocket.Core.Assets
{
    public class Asset<T> : IAsset<T> where T : class
    {
        #region Variables
        protected T _Instance = null;
        #endregion

        #region Properties
        public T Instance
        {
            get
            {
                if (_Instance == null)
                    Load();
                return _Instance;
            }
            set
            {
                if (value == null)
                    return;

                _Instance = value;
                Save();
            }
        }
        #endregion

        #region Functions
        public virtual void Load(AssetLoaded<T> callback = null)
        {
            if (callback == null)
                return;

            callback(this);
        }

        public virtual T Save() => _Instance;

        public void Unload(AssetUnloaded<T> callback = null)
        {
            if (callback == null)
                return;

            callback(this);
        }
        #endregion
    }
}
