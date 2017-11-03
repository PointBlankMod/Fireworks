using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Rocket.API;
using PointBlank.API.Plugins;
using UnityEngine;

namespace Rocket.Core.Plugins
{
    public sealed class RocketPluginManager : MonoBehaviour
    {
        public delegate void PluginsLoaded();
        public event PluginsLoaded OnPluginsLoaded;
        public void RunPluginsLoaded() => OnPluginsLoaded?.Invoke();

        public List<IRocketPlugin> GetPlugins() =>
            PointBlankPluginManager.LoadedPlugins.Where(a => typeof(IRocketPlugin).IsAssignableFrom(a.GetType())).Select(a => (IRocketPlugin)a).ToList();

        public IRocketPlugin GetPlugin(Assembly assembly) =>
            (IRocketPlugin)PointBlankPluginManager.LoadedPlugins.FirstOrDefault(a => typeof(IRocketPlugin).IsAssignableFrom(a.GetType()) && a.GetType().Assembly.FullName == assembly.FullName);

        public IRocketPlugin GetPlugin(string name) =>
            (IRocketPlugin)PointBlankPluginManager.LoadedPlugins.FirstOrDefault(a => typeof(IRocketPlugin).IsAssignableFrom(a.GetType()) && a.GetType().Name == name);

        public Type GetMainTypeFromAssembly(Assembly assembly) =>
            PointBlankPluginManager.LoadedPlugins.FirstOrDefault(a => typeof(IRocketPlugin).IsAssignableFrom(a.GetType()) && a.GetType().Assembly.FullName == assembly.FullName).GetType();

        public static Dictionary<string, string> GetAssembliesFromDirectory(string directory, string extension = "*.dll")
        {
            Dictionary<string, string> l = new Dictionary<string, string>();
            IEnumerable<FileInfo> libraries = new DirectoryInfo(directory).GetFiles(extension, SearchOption.AllDirectories);
            foreach (FileInfo library in libraries)
            {
                try
                {
                    AssemblyName name = AssemblyName.GetAssemblyName(library.FullName);
                    l.Add(name.FullName, library.FullName);
                }
                catch { }
            }
            return l;
        }

        public static List<Assembly> LoadAssembliesFromDirectory(string directory, string extension = "*.dll") => new List<Assembly>(); // Yeah not happening
    }
}
