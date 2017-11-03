using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API.Player;
using UnityEngine;

namespace Rocket.API
{
    public class RocketPlayer : PointBlankPlayer, IRocketPlayer
    {
        #region Variables
        private GameObject _GameObject = new GameObject();
        private string _Id;
        private string _DisplayName;
        #endregion

        #region Properties
        public override bool IsAdmin { get; set; }

        public override GameObject GameObject => _GameObject;

        public string Id => _Id;

        public string DisplayName => _DisplayName;
        #endregion

        public RocketPlayer(string Id, string DisplayName = null, bool IsAdmin = false)
        {
            _Id = Id;
            _DisplayName = (string.IsNullOrEmpty(DisplayName) ? Id : DisplayName);
            this.IsAdmin = IsAdmin;
        }

        #region Functions
        public int CompareTo(object obj) => Id.CompareTo(((IRocketPlayer)obj).Id);

        public override void SendMessage(object message, Color color) { }
        #endregion
    }
}
