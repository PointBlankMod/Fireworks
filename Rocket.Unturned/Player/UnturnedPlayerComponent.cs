using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Rocket.Unturned.Player
{
    public class UnturnedPlayerComponent : MonoBehaviour
    {
        #region Properties
        public UnturnedPlayer Player { get; private set; }
        #endregion

        #region Mono Functions
        void Awake()
        {
            Player = UnturnedPlayer.FromPlayer(gameObject.GetComponent<SDG.Unturned.Player>());
            DontDestroyOnLoad(gameObject);
        }

        void OnEnable() => Load();
        void OnDisable() => Unload();
        #endregion

        #region Virtual Functions
        protected virtual void Load() { }
        protected virtual void Unload() { }
        #endregion
    }
}
