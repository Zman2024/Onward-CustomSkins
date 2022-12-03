using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;
using UnityEngine.UI;
using MonoMod;
using System.Reflection;
using Onward;
using Onward.UI;

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
            try
            {
                if (Settings.LoadGlobal.Value && !Settings.HasLoadedGlobal)
                {
                    foreach (var winfo in __instance.Weapons)
                    {
                        if (winfo.PickupRef && winfo.PickupRef.IsPickupGunInstance)
                        {
                            Pickup_Gun pickup = winfo.PickupRef.LoadRef().GetComponent<Pickup_Gun>();

                            SkinInfo info = Plugin.CurrentSkins.Find((match) => match.WeaponName == winfo.weapon);
                            if (info != null && info.TextureBytes != null)
                            {
                                MeshRenderer[] meshRenderers = pickup.WeaponModel.GetComponentsInChildren<MeshRenderer>();
                                for (int x = 0; x < meshRenderers.Length; x++)
                                {
                                    var renderer = meshRenderers[x];
                                    if (renderer.name == info.ObjectName || renderer.name == (info.ObjectName + "(Clone)"))
                                    {
                                        Logger.LogInfo($"Replacing texture on renderer: {renderer.name} with sortingLayerName: {renderer.sortingLayerName}");
                                        var tex = renderer.material.mainTexture as Texture2D;
                                        tex.LoadImage(info.TextureBytes);
                                        info.Texture = tex;
                                        Logger.LogInfo($"Replaced texture for {info.WeaponName}");
                                        break;
                                    }
                                }
                            }

                        }
                    }
                    Settings.HasLoadedGlobal = true;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
            }
        }

    }
}
