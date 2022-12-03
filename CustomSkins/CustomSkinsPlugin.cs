using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using HarmonyLib;
using BepInEx.Configuration;
using BepInEx.Harmony;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections;

namespace CustomSkins
{
    internal static class Settings
    {
        public static ConfigEntry<bool> LoadGlobal { get; set; }
        public static bool HasLoadedGlobal { get; set; } = false;

        private static class Definitions
        {
            public static ConfigDefinition LoadGlobal = new ConfigDefinition("CustomSkins", "LoadGlobal");
            public static ConfigDescription LoadGlobalDesc = new ConfigDescription("Applyies cutom skins gobally (overwrites original texture)");
        }

        internal static void LoadConfig(ConfigFile cfg)
        {
            LoadGlobal = cfg.Bind(Definitions.LoadGlobal, false, Definitions.LoadGlobalDesc);
        }

    }

    public class SkinInfo
    {
        public string FileName;
        public string PickupName;
        public string ObjectName;
        public WeaponName WeaponName;

        public byte[] TextureBytes;
        public Texture2D Texture;

        public SkinInfo() { }

        public SkinInfo(string fileName, string pickupName, string objectName, WeaponName weaponName)
        {
            FileName = fileName;
            PickupName = pickupName;
            ObjectName = objectName;
            WeaponName = weaponName;
        }

    }

    public static class Translation
    {
        public static readonly List<SkinInfo> Translations = new List<SkinInfo>()
        {
            new SkinInfo("mk18-body-texture", "Pickup_MK18", "WPN_MK18", WeaponName.MK18),
            new SkinInfo("glock17-body-texture", "Pickup_G17", "glock_17_body", WeaponName.G17),
            new SkinInfo("ak12-body-texture", "Pickup_AK12", "AK12_base", WeaponName.AK12),
            new SkinInfo("g3a3-body-texture", "Pickup_G3", "g3a3_main_body", WeaponName.G3),

            new SkinInfo("m16a4-body-texture", "Pickup_M16A4", "m16a4_base", WeaponName.M16A4),
            new SkinInfo("m1911-body-texture", "Pickup_M1911", "m_gun", WeaponName.M1911),
            new SkinInfo("mk16-body-texture", "Pickup_Mk16", "AssaultRifle", WeaponName.MK16),
            new SkinInfo("aug-body-texture", "Pickup_AUG", "Model", WeaponName.AUG),

            new SkinInfo("ak5c-body-texture", "Pickup_AK5C", "m_gun", WeaponName.AK5C),
            new SkinInfo("m9-body-texture", "Pickup_M9", "m9_body", WeaponName.M9),
            new SkinInfo("m1014-body-texture", "Pickup_M1014", "m1014_body", WeaponName.M1014),
            new SkinInfo("mp5-body-texture", "Pickup_MP5", "WPN_MP5", WeaponName.MP5),

            new SkinInfo("p90-body-texture", "Pickup_P90", "m_gun", WeaponName.P90),
            new SkinInfo("commando552-body-texture", "Pickup_552Commando", "552_commando.001", WeaponName.Commando552),
            new SkinInfo("l86a2-lsw-body-texture", "Pickup_SA80", "SA80_LSW_Body", WeaponName.SA80),
            new SkinInfo("m249-body-texture", "Pickup_M249", "M249_main", WeaponName.M249),

            new SkinInfo("m40a5-body-texture", "Pickup_M40A5", "polySurface8414", WeaponName.M40A5),
            new SkinInfo("mk17-body-texture", "Pickup_MK17", "AssaultRifle", WeaponName.MK17),
            new SkinInfo("m39emr-body-texture", "Pickup_M39EMR", "polySurface8270", WeaponName.M39EMR),
            new SkinInfo("tt30-body-texture", "Pickup_TT", "TT_frame", WeaponName.TT),

            new SkinInfo("sks-body-texture", "Pickup_SKS", "stock_base", WeaponName.SKS),
            new SkinInfo("akm-body-texture", "Pickup_AKM", "akm 1", WeaponName.AKM),

            new SkinInfo("makarov-body-texture", "Pickup_Makarov", "makarov_body", WeaponName.Makarov),
            new SkinInfo("fiveseven-body-texture", "Pickup_FiveSeven", "fn_five_seven_body", WeaponName.FiveSeven),
            new SkinInfo("flaregun-body-texture", "Pickup_FlareGun", "flare_gun_base", WeaponName.FlareGun),
            new SkinInfo("spas12-body-texture", "Pickup_SPAS12", "m_shot12", WeaponName.SPAS12),

            new SkinInfo("svd-body-texture", "Pickup_SVD", "polySurface8415", WeaponName.SVD), // may not work
            new SkinInfo("aks74u-body-texture", "Pickup_AK74u", "aks74u_body", WeaponName.AK74u),

            new SkinInfo("g36c-body-texture", "Pickup_G36", "g36c_body", WeaponName.G36c),
            new SkinInfo("asval-body-texture", "Pickup_VAL", "m_gun", WeaponName.VAL),
            new SkinInfo("tar21-body-texture", "Pickup_TAR21", "sbg_AR21_body", WeaponName.TAR21),
            new SkinInfo("rpg7-body-texture", "Pickup_RPG7", "rpg_7_launcher_main", WeaponName.RPG7),

            new SkinInfo("sv98-body-texture", "Pickup_SV98", "sv98_body_main", WeaponName.SV98),
            new SkinInfo("g3a3-auto-body-texture", "Pickup_G3Auto", "g3a3_main_body", WeaponName.G3Auto),
            new SkinInfo("taser-body-texture", "Pickup_Taser", "taser_body", WeaponName.Taser),

            // need pkm and famas

        };

        public static SkinInfo FromWeaponName(WeaponName wpnName)
        {
            return Translations.Find((bundle) => bundle.WeaponName == wpnName);
        }

        public static SkinInfo FromFileName(string fileName)
        {
            return Translations.Find((bundle) => bundle.FileName == fileName);
        }

        public static SkinInfo FromPickupName(string pickupName)
        {
            return Translations.Find((bundle) => bundle.PickupName == pickupName);
        }

    }

    [BepInPlugin("Zman2024-CustomSkins", "Custom Skins", "0.1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        public static readonly string CustomSkinsFolder = Path.Combine(Directory.GetCurrentDirectory(), "CustomSkins");

        public static List<SkinInfo> CurrentSkins = new List<SkinInfo>();

        public void Start()
        {
            try
            {
                if (!Directory.Exists(CustomSkinsFolder))
                {
                    Directory.CreateDirectory(CustomSkinsFolder);
                }
                else
                {
                    Logger.LogInfo("Loading textures");

                    string[] files = Directory.GetFiles(CustomSkinsFolder);
                    for (int x = 0; x < files.Length; x++)
                    {
                        string fpathfull = files[x].ToLower();
                        if (fpathfull.EndsWith(".zip"))
                        {
                            // TODO: make this work so people can create texture packs
                            continue;
                        }

                        string fnameNoExt = Path.GetFileNameWithoutExtension(fpathfull);
                        SkinInfo info = Translation.FromFileName(fnameNoExt);

                        if (info != null)
                        {
                            info.TextureBytes = File.ReadAllBytes(fpathfull);
                            CurrentSkins.Add(info);
                            Logger.LogInfo($"Loaded texture {Path.GetFileName(files[x])} for {info.WeaponName}");
                        }

                    }

                    Logger.LogInfo("Applying harmony patches...");
                    var harmony = Harmony.CreateAndPatchAll(typeof(HarmonyPatches));
                }

                Logger.LogInfo("Done");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return;
            }
        }
        
        public void Awake()
        {
            HarmonyPatches.Logger = Logger;
            Settings.LoadConfig(Config);
        }

    }
}
