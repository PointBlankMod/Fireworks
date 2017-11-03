using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fireworks.API;
using Rocket.API;
using PointBlank.API.Commands;
using PointBlank.API.Plugins;
using PointBlank.API.Interfaces;

namespace Fireworks.Framework
{
    public class CommandFramework : ILoadable
    {
        public void Load()
        {
            // Run the code
            foreach(PointBlankPlugin plugin in PointBlankPluginManager.LoadedPlugins)
            {
                if (!typeof(IRocketPlugin).IsAssignableFrom(plugin.GetType()))
                    continue;

                foreach (Type t in plugin.GetType().Assembly.GetTypes())
                    RegisterCommand(t);
            }
        }

        public void Unload()
        {
            foreach(PointBlankCommand command in PointBlankCommandManager.Commands)
                if (typeof(FireworksCommand).IsAssignableFrom(command.GetType()))
                    PointBlankCommandManager.UnloadCommand(command);
        }

        #region Functions
        public static void RegisterCommand(Type t)
        {
            if (!typeof(IRocketCommand).IsAssignableFrom(t))
                return;

            try
            {
                IRocketCommand command = (IRocketCommand)Activator.CreateInstance(t);
                FireworksCommand fc = new FireworksCommand(command);

                PointBlankCommandManager.LoadCommand(fc);
            }
            catch (Exception ex)
            {
                return;
            }
        }
        public static void RegisterCommand(IRocketCommand command)
        {
            try
            {
                FireworksCommand fc = new FireworksCommand(command);

                PointBlankCommandManager.LoadCommand(fc);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public static void UnregisterCommand(Type t)
        {
            if (!typeof(IRocketCommand).IsAssignableFrom(t))
                return;

            try
            {
                PointBlankCommand command = PointBlankCommandManager.Commands.FirstOrDefault(a => typeof(FireworksCommand).IsAssignableFrom(a.GetType()) && ((FireworksCommand)a)._RocketCommand.GetType() == t);

                if (command == null)
                    return;
                PointBlankCommandManager.UnloadCommand(command);
            }
            catch (Exception ex)
            {

            }
        }
        public static void UnregisterCommand(IRocketCommand command)
        {
            try
            {
                PointBlankCommand cmd = PointBlankCommandManager.Commands.FirstOrDefault(a => typeof(FireworksCommand).IsAssignableFrom(a.GetType()) && ((FireworksCommand)a)._RocketCommand == command);

                if (cmd == null)
                    return;
                PointBlankCommandManager.UnloadCommand(cmd);
            }
            catch (Exception ex)
            {
                return;
            }
        }
        #endregion
    }
}
