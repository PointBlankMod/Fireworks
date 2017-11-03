using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fireworks.Framework;
using SDG.Framework.Modules;
using PointBlank.API;
using PointBlank.API.Interfaces;
using PointBlank.API.Server;
using UnityEngine;

namespace Fireworks
{
    public class Loader : IModuleNexus
    {
        #region Variables
        public static Loader Instance;
        #endregion

        #region Properties
        public static Dictionary<Type, ILoadable> Modules { get; private set; }
        #endregion

        public Loader() => Instance = this;

        public void initialize()
        {
            PointBlankLogging.LogImportant("Welcome to Project Fireworks!");
            PointBlankLogging.Log("PointBlank's experience, Rocket's support");

            if (!Directory.Exists(PointBlankServer.ModLoaderPath))
                Directory.CreateDirectory(PointBlankServer.ModLoaderPath);
            Directory.SetCurrentDirectory(PointBlankServer.ModLoaderPath);
            // Setup the variables
            Modules = new Dictionary<Type, ILoadable>()
            {
                { typeof(EventFramework), new EventFramework() },
                { typeof(CommandFramework), new CommandFramework() },
            };

            // Run the code
            foreach (Type t in Modules.Keys)
            {
                PointBlankLogging.Log("Starting fireworks framework: " + t.Name);
                Modules[t].Load();
            }
        }

        public void shutdown()
        {
            foreach (Type t in Modules.Keys)
            {
                PointBlankLogging.Log("Stopping fireworks framework: " + t.Name);
                Modules[t].Unload();
            }
        }
    }
}
