using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PointBlank.API;
using PointBlank.API.Plugins;
using PointBlank.API.Server;
using Rocket.API;
using Rocket.API.Collections;
using Rocket.Core.Assets;
using Rocket.API.Extensions;
using UnityEngine;
using Fireworks.API;
using Conf = PointBlank.API.Collections.ConfigurationList;

namespace Rocket.Core.Plugins
{
    public class RocketPlugin : FireworksPlugin, IRocketPlugin
    {
        #region Events
        public delegate void PluginUnloading(IRocketPlugin plugin);
        public static event PluginUnloading OnPluginUnloading;

        public delegate void PluginLoading(IRocketPlugin plugin, ref bool cancel);
        public static event PluginLoading OnPluginLoading;

        public delegate void ExecuteDependencyCodeDelegate(IRocketPlugin plugin);
        #endregion

        #region Variables
        private ConnectedAsset<TranslationList> _ConnectedTranslations;
        private PluginState _State = PluginState.Unloaded;
        #endregion

        #region Properties
        public IAsset<TranslationList> Translations
        {
            get
            {
                if (_ConnectedTranslations == null)
                    _ConnectedTranslations = new ConnectedAsset<TranslationList>(DefaultTranslations);
                return _ConnectedTranslations;
            }
        }
        public virtual TranslationList DefaultTranslations => new TranslationList();

        public PluginState State => _State;

        public Assembly Assembly { get; private set; }
        public string Directory { get; private set; }
        public string Name { get; private set; }
        #endregion

        public RocketPlugin()
        {
            // Set the variables
            Assembly = GetType().Assembly;
            Name = Assembly.GetName().Name;
            Directory = PointBlankServer.DataPath;

            System.IO.Directory.CreateDirectory(PointBlankServer.PluginsPath + "/" + Name);
        }

        #region Static Functions
        public static bool IsDependencyLoaded(string plugin) => PointBlankPluginManager.LoadedPlugins.Count(a => a.name == plugin) > 0;
        
        public static void ExecuteDependencyCode(string plugin, ExecuteDependencyCodeDelegate a)
        {
            IRocketPlugin p = (IRocketPlugin)PointBlankPluginManager.LoadedPlugins.FirstOrDefault(b => b.name == plugin && typeof(IRocketPlugin).IsAssignableFrom(b.GetType()));

            if (p != null)
                a(p);
        }
        #endregion

        #region Virtual Functions
        protected virtual void Load() { }
        protected virtual void Unload() { }

        public virtual void LoadPlugin()
        {
            PointBlankLogging.Log("Loading rocket plugin: " + Name);

            try
            {
                Load();
            }
            catch (Exception ex)
            {
                PointBlankLogging.LogError("Failed to load rocket plugin " + Name + ", unloading...", ex);
                try
                {
                    PointBlankLogging.LogError("Failed to unload rocket plugin " + Name, ex);
                    UnloadPlugin(PluginState.Failure);
                    return;
                }
                catch (Exception ex1)
                {
                    PointBlankLogging.LogError("Failed to unload rocket plugin " + Name, ex1);
                }
            }

            bool cancel = false;
            OnPluginLoading?.Invoke(this, ref cancel);

            if (cancel)
                return;

            _State = PluginState.Loaded;
        }
        public virtual void UnloadPlugin(PluginState state = PluginState.Unloaded)
        {
            PointBlankLogging.Log("Unloading rocket plugin " + Name);
            OnPluginUnloading?.Invoke(this);

            Unload();
            _State = state;
        }
        #endregion

        #region Functions
        public void ReloadPlugin()
        {
            UnloadPlugin();
            LoadPlugin();
        }

        public T TryAddComponent<T>() where T : Component => gameObject.TryAddComponent<T>();
        public void TryRemoveComponent<T>() where T : Component => gameObject.TryRemoveComponent<T>();
        #endregion
    }

    public class RocketPlugin<RocketPluginConfiguration> : RocketPlugin, IRocketPlugin<RocketPluginConfiguration> where RocketPluginConfiguration : class, IRocketPluginConfiguration
    {
        #region Variables
        private IAsset<RocketPluginConfiguration> _Configuration;
        #endregion

        #region Properties
        public IAsset<RocketPluginConfiguration> Configuration => _Configuration;
        #endregion

        public RocketPlugin() : base() =>
            _Configuration = new ClassToListAsset<RocketPluginConfiguration, Conf>(Activator.CreateInstance<RocketPluginConfiguration>(), Configurations);

        #region Functions
        public override void LoadPlugin() =>
            _Configuration.Load((IAsset<RocketPluginConfiguration> asset) => { base.LoadPlugin(); });
        #endregion
    }
}
