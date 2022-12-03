using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;

namespace CustomSkins
{

    [HarmonyPatch]
    public static class HarmonyPatches
    {
        public static BepInEx.Logging.ManualLogSource Logger;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PickupLibrary), nameof(PickupLibrary.Initialize))]
        public static void PickupLibrary_Initialize(PickupLibrary __instance)
        {
            // Dont do this more than once because there's no reason to
            // The changes to the textures stay for the lifetime of the game
            if (!Settings.LoadGlobal.Value || Settings.HasLoadedGlobal)
                return;

            try
            {
                // Go through all weapons (may be faster to go through all Plugins.CurrentSkins but ah who cares)
                foreach (var winfo in __instance.Weapons)
                {
                    // Make sure the pickup reference has a Pickup_Gun on it
                    if (winfo.PickupRef && winfo.PickupRef.IsPickupGunInstance)
                    {
                        Pickup_Gun pickup = winfo.PickupRef.LoadRef().GetComponent<Pickup_Gun>();

                        // Find the skin info for this weapon (may be null)
                        SkinInfo info = Plugin.CurrentSkins.Find((match) => match.WeaponName == winfo.weapon);

                        // Make sure there is a skin for this weapon and the texture bytes were loaded
                        if (info == null || info.TextureBytes == null || info.TextureBytes.Length == 0) continue;

                        // Get all mesh renderers for the weapon model
                        MeshRenderer[] meshRenderers = pickup.WeaponModel.GetComponentsInChildren<MeshRenderer>();

                        // Search for the mesh renderer we need to override using the SkinInfo.ObjectName
                        for (int x = 0; x < meshRenderers.Length; x++)
                        {
                            MeshRenderer renderer = meshRenderers[x];

                            // Check the renderer's name against the SkinInfo.ObjectName
                            // I shouldn't need the (Clone) but just in case
                            if (renderer.name == info.ObjectName || renderer.name == (info.ObjectName + "(Clone)"))
                            {
                                // Cast to Texture2D so we can use LoadImage
                                Texture2D tex = renderer.material.mainTexture as Texture2D;
                                info.Texture = tex;

                                // Replace the texture with the image loaded from CustomSkins
                                if (tex.LoadImage(info.TextureBytes))
                                {
                                    Logger.LogInfo($"Replaced texture for {info.WeaponName}");
                                }
                                else // RIP bozo it didnt work
                                {
                                    Logger.LogWarning($"Could not replace texture for {info.WeaponName} (bad format?)");
                                }

                                break;
                            }
                        }
                    }
                }

                // No need to run this more than once (for now)
                Settings.HasLoadedGlobal = true;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

    }
}
