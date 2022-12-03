using System;
using BepInEx.Configuration;

namespace CustomSkins
{
    internal static class Settings
    {
        public static ConfigEntry<bool> Enabled { get; set; }
        public static ConfigEntry<bool> LoadGlobal { get; set; }

        public static bool HasLoadedGlobal { get; set; } = false;

        private static class Definitions
        {
            public static ConfigDefinition LoadGlobal = new ConfigDefinition("CustomSkins", "LoadGlobal");
            public static ConfigDescription LoadGlobalDesc = new ConfigDescription("Applies custom skins gobally (overwrites original texture)");

            public static ConfigDefinition Enabled = new ConfigDefinition("CustomSkins", "Enabled");
            public static ConfigDescription EnabledDesc = new ConfigDescription("Determines whether or not to load the mod");
        }

        internal static void LoadConfig(ConfigFile cfg)
        {
            Enabled = cfg.Bind(Definitions.Enabled, true, Definitions.EnabledDesc);
            LoadGlobal = cfg.Bind(Definitions.LoadGlobal, true, Definitions.LoadGlobalDesc);
        }

    }
}
