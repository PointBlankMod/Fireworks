using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.API.Collections;
using PointBlank.API.Plugins;
using PointBlank.API.Server;
using Rocket.Core.Plugins;

namespace Fireworks.API
{
    public class FireworksPlugin : PointBlankPlugin
    {
        #region Variables
        private RocketPlugin Instance;
        #endregion

        #region Properties
        public override TranslationList DefaultTranslations => Instance.DefaultTranslations;

        public override ConfigurationList DefaultConfigurations => new ConfigurationList();

        public override string Version => "0";
        #endregion

        public FireworksPlugin()
        {
            Instance = (RocketPlugin)this;
        }

        public override void Load() => Instance.LoadPlugin();

        public override void Unload() => Instance.UnloadPlugin();
    }
}
