using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using HarmonyLib;

namespace CustomSkins
{
    [BepInPlugin("CustomSkins", "Custom Skins", "0.1.0")]
    public class Plugin : BaseUnityPlugin
    {
        // The folder that holds the custom textures (Onward/CustomSkins)
        public static readonly string CustomSkinsFolder = Path.Combine(Directory.GetCurrentDirectory(), "CustomSkins");

        // The skins that were found in Onward/CustomSkins
        public static List<SkinInfo> CurrentSkins = new List<SkinInfo>();

        // The harmony instace for this mod
        private static Harmony HarmonyInstance { get; set; }

        public void Start()
        {
            try
            {
                if (!Settings.Enabled.Value)
                {
                    Logger.LogInfo("Not enabling custom skins (config/CustomSkins.cfg -> Enabled is false)");
                    return;
                }

                // Make sure the CustomSkins directory exists, if it doesn't
                // there's no point to even loading the mod
                if (!Directory.Exists(CustomSkinsFolder))
                {
                    // Create the directory
                    Directory.CreateDirectory(CustomSkinsFolder);
                }
                else
                {
                    Logger.LogInfo("Loading textures");

                    string[] files = Directory.GetFiles(CustomSkinsFolder);

                    // Load all skins from the CustomSkins folder
                    for (int x = 0; x < files.Length; x++)
                    {
                        string fpathfull = files[x].ToLower();

                        if (fpathfull.EndsWith(".zip"))
                        {
                            // TODO: make this work so people can create texture packs
                            continue;
                        }
                        if (!fpathfull.EndsWith(".png") && !fpathfull.EndsWith(".jpg"))
                        {
                            // Unsupported format
                            continue;
                        }

                        string fnameNoExt = Path.GetFileNameWithoutExtension(fpathfull);
                        SkinInfo info = Translation.FromFileName(fnameNoExt);

                        if (info != null)
                        {
                            // Load the image bytes into the SkinInfo's TextureBytes so it can be used
                            // by HarmonyPatches.PickupLibrary_Initialize later, and add the skin to the
                            // list of current skins
                            info.TextureBytes = File.ReadAllBytes(fpathfull);
                            CurrentSkins.Add(info);
                            Logger.LogInfo($"Loaded texture {Path.GetFileName(files[x])} for {info.WeaponName}");
                        }

                    }

                    // No need to apply patches if there are no custom skins
                    if (CurrentSkins.Count == 0)
                    {
                        Logger.LogInfo("No custom skins were loaded, not applying harmony patches");
                        return;
                    }

                    Logger.LogInfo("Applying harmony patches...");
                    HarmonyInstance = Harmony.CreateAndPatchAll(typeof(HarmonyPatches));
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
