using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rocket.API
{
    #region Delegates
    public delegate void AssetLoaded<T>(IAsset<T> asset) where T : class;
    public delegate void AssetUnloaded<T>(IAsset<T> asset) where T : class;
    #endregion

    public interface IAsset<T> where T : class
    {
        T Instance { get; set; }

        T Save();
        void Load(AssetLoaded<T> callback = null);
        void Unload(AssetUnloaded<T> callback = null);
    }
}
