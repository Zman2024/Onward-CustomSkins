using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BepInEx;
using HarmonyLib;
using System.IO.Compression;
//using Ionic.Zip;

namespace CustomSkins
{
    [BepInPlugin("CustomSkins", "Custom Skins", Version)]
    public class Plugin : BaseUnityPlugin
    {
        public const string Version = "0.1.2";

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
                            /// none of these work because of a mono bug :(
                            //using(var zfile = ZipFile.OpenRead(fpathfull))
                            //{
                            //    foreach (var entry in zfile.Entries)
                            //    {
                            //        if (!entry.Name.EndsWith(".png") || !entry.Name.EndsWith(".jpg"))
                            //            continue;
                            //
                            //        string sname = Path.GetFileNameWithoutExtension(entry.Name);
                            //        var sinfo = Translation.FromFileName(sname);
                            //        if (sinfo == null)
                            //        {
                            //            Logger.LogWarning($"Skipping file {sname} in skinpack {Path.GetFileName(fpathfull)} (does not match known skin)");
                            //            continue;
                            //        }
                            //
                            //        if (CurrentSkins.Any((s) => s.FileName == sname))
                            //        {
                            //            Logger.LogWarning($"Skipping skin {sname} in skinpack {Path.GetFileName(fpathfull)} (skin is already replaced)");
                            //            continue;
                            //        }
                            //
                            //        using (var stream = entry.Open())
                            //        {
                            //            stream.Read(sinfo.TextureBytes, 0, (int)stream.Length);
                            //        }
                            //
                            //        CurrentSkins.Add(sinfo);
                            //    }
                            //}
                            /*
                            using (var zip = ZipFile.Read(fpathfull))
                            {
                                foreach (var file in zip)
                                {
                                    if (file.IsDirectory) continue;

                                    if (!file.FileName.EndsWith(".png") && !file.FileName.EndsWith(".jpg"))
                                    {
                                        // Unsupported format
                                        Logger.LogInfo($"Skipping file {file.FileName} from pack {Path.GetFileName(fpathfull)} (not png/jpg)");
                                        continue;
                                    }

                                    SkinInfo sinfo = Translation.FromFileName(Path.GetFileNameWithoutExtension(file.FileName));

                                    if (sinfo == null)
                                        continue;

                                    if (CurrentSkins.Any((s) => s.FileName == sinfo.FileName))
                                    {
                                        Logger.LogWarning($"Skipping {file.FileName} from pack {Path.GetFileName(fpathfull)} (skin already replaced)");
                                        continue;
                                    }

                                    using (var reader = file.OpenReader())
                                    {
                                        byte[] filebytes = new byte[reader.Length];
                                        reader.Read(filebytes, 0, filebytes.Length);
                                        sinfo.TextureBytes = filebytes;
                                    }

                                    CurrentSkins.Add(sinfo);
                                }
                            }
                            */

                            continue;
                        }
                        if (!fpathfull.EndsWith(".png") && !fpathfull.EndsWith(".jpg"))
                        {
                            // Unsupported format
                            Logger.LogInfo($"Skipping file {Path.GetFileName(fpathfull)} (not png/jpg or skin pack)");
                            continue;
                        }

                        string fnameNoExt = Path.GetFileNameWithoutExtension(fpathfull);
                        SkinInfo info = Translation.FromFileName(fnameNoExt);

                        if (info != null)
                        {
                            if (CurrentSkins.Any((tmp) => tmp.FileName == info.FileName))
                            {
                                Logger.LogWarning($"Skipping {fpathfull} (skin already replaced)");
                                continue;
                            }

                            // Load the image bytes into the SkinInfo's TextureBytes so it can be used
                            // by HarmonyPatches.PickupLibrary_Initialize later, and add the skin to the
                            // list of current skins

                            info.TextureBytes = File.ReadAllBytes(fpathfull);
                            CurrentSkins.Add(info);
                            if (info.WeaponName != WeaponName.NULL)
                                Logger.LogInfo($"Loaded texture {Path.GetFileName(files[x])} for {info.WeaponName}");
                            else
                                Logger.LogInfo($"Loaded texture {Path.GetFileName(files[x])} for {info.EquipmentType}");
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
