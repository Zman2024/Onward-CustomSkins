using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using BepInEx;

namespace CustomSkins
{
    public class SkinInfo
    {
        // The file name expected to be in CustomSkins that holds the texture data that
        // will replace the ObjectName's MeshRenderer.material.mainTexture
        public string FileName;

        // The name of the pickup that this object belongs to
        // eg: Pickup_MK18
        public string PickupName;

        // The name of the object that has the MeshRenderer
        // who's texture we want to replace
        public string ObjectName;

        // The WeaponName of the gun (real?)
        public WeaponName WeaponName;

        // The EquipmentType (if this is an equipment entry)
        public ClassLoadout.EquipmentType EquipmentType;

        // The raw bytes of the image loaded from the CustomSkins folder
        public byte[] TextureBytes;

        // This... i dont know what this is yet but i want it here for the future.
        // As of right now im using it to hold a reference to the mainTexture
        // that has it's texture replaced
        public Texture2D Texture;

        public SkinInfo() { }

        public SkinInfo(string fileName, string pickupName, string objectName, WeaponName weaponName = WeaponName.NULL, ClassLoadout.EquipmentType equipmentType = ClassLoadout.EquipmentType.NULL)
        {
            FileName = fileName;
            PickupName = pickupName;
            ObjectName = objectName;
            WeaponName = weaponName;
            EquipmentType = equipmentType;
        }

        public SkinInfo(string fileName, string pickupName, string objectName, ClassLoadout.EquipmentType equipmentType = ClassLoadout.EquipmentType.NULL, WeaponName weaponName = WeaponName.NULL)
        {
            FileName = fileName;
            PickupName = pickupName;
            ObjectName = objectName;
            WeaponName = weaponName;
            EquipmentType = equipmentType;
        }

    }

    public static class Translation
    {
        private static readonly List<SkinInfo> Translations = new List<SkinInfo>()
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
            new SkinInfo("552commando-body-texture", "Pickup_552Commando", "552_commando.001", WeaponName.Commando552),
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

            new SkinInfo("famas-body-texture", "Pickup_Famas", "polySurface2163", WeaponName.Famas),
            new SkinInfo("pkm-body-texture", "Pickup_PKM", "polySurface4195", WeaponName.PKM),
            new SkinInfo("svd-body-texture", "Pickup_SVD", "polySurface8415", WeaponName.SVD),
            new SkinInfo("aks74u-body-texture", "Pickup_AK74u", "aks74u_body", WeaponName.AK74u),

            new SkinInfo("g36c-body-texture", "Pickup_G36", "g36c_body", WeaponName.G36c),
            new SkinInfo("asval-body-texture", "Pickup_VAL", "m_gun", WeaponName.VAL),
            new SkinInfo("tar21-body-texture", "Pickup_TAR21", "sbg_AR21_body", WeaponName.TAR21),
            new SkinInfo("rpg7-body-texture", "Pickup_RPG7", "rpg_7_launcher_main", WeaponName.RPG7),

            new SkinInfo("sv98-body-texture", "Pickup_SV98", "sv98_body_main", WeaponName.SV98),
            new SkinInfo("g3a3-auto-body-texture", "Pickup_G3Auto", "g3a3_main_body", WeaponName.G3Auto),
            new SkinInfo("taser-body-texture", "Pickup_Taser", "taser_body", WeaponName.Taser),

            new SkinInfo("m18-smoke-white-body-texture", "Pickup_Smoke_Grenade", "m18_smoke_body", ClassLoadout.EquipmentType.GrenadeSmokeWhite),
            new SkinInfo("m18-smoke-green-body-texture", "Pickup_Smoke_Grenade", "polySurface7.001", ClassLoadout.EquipmentType.GrenadeSmokeGreen),
            new SkinInfo("m18-smoke-red-body-texture", "Pickup_Smoke_Grenade", "polySurface7.001", ClassLoadout.EquipmentType.GrenadeSmokeRed),
            new SkinInfo("flashbang-body-texture", "Pickup_GrenadeFlash", "flashbang", ClassLoadout.EquipmentType.GrenadeStun),

            new SkinInfo("frag-grenade-body-texture", "Pickup_Grenade", "grenade_body", ClassLoadout.EquipmentType.GrenadeFrag),
            new SkinInfo("syringe-body-texture", "Pickup_Syringe", "Main", ClassLoadout.EquipmentType.SyringeExtra),
            new SkinInfo("molotov-and-lighter-body-texture", "Pickup_Molotov", "molotov_bottle 1", ClassLoadout.EquipmentType.GrenadeMolotov),
            new SkinInfo("c4-and-detonator-body-texture", "Pickup_C4", "C4", ClassLoadout.EquipmentType.C4),
            
            new SkinInfo("knife-marsoc-body-texture", "Pickup_KnifeMarsoc", "Knife", WeaponName.NULL),
            new SkinInfo("knife-volk-body-texture", "Pickup_KnifeVolk", "Prefab_Knife01", WeaponName.NULL),

        };

        /// <summary>
        /// Returns the SkinInfo instance with the matching WeaponName (subject to becoming an array)
        /// </summary>
        public static SkinInfo FromWeaponName(WeaponName wpnName) => Translations.Find((info) => info.WeaponName == wpnName);

        /// <summary>
        /// Returns the SkinInfo instance with the matching PickupName (subject to becoming an array)
        /// </summary>
        public static SkinInfo FromPickupName(string pickupName) => Translations.Find((info) => info.PickupName == pickupName);

        /// <summary>
        /// Returns the SkinInfo instance with the matching FileName
        /// </summary>
        public static SkinInfo FromFileName(string fileName) => Translations.Find((info) => info.FileName.ToLower() == fileName.ToLower());

        /// <summary>
        /// Returns the SkinInfo instance with the matching ObjectName
        /// </summary>
        public static SkinInfo FromObjectName(string objName) => Translations.Find((info) => info.ObjectName == objName);

        /// <summary>
        /// Returns the SkinInfo instance with the matching EquipmentType
        /// </summary>
        public static SkinInfo FromEquipmentType(ClassLoadout.EquipmentType type) => Translations.Find((info) => info.EquipmentType == type);

        /// <summary>
        /// Adds a SkinInfo to the list of translated objects
        /// </summary>
        public static bool AddTranslation(SkinInfo info)
        {
            // Cant have null / whitespace only file names
            if (info.FileName.IsNullOrWhiteSpace()) return false;

            // Already has a SkinInfo with the same file name
            if (FromFileName(info.FileName) != null)
                return false;

            Translations.Add(info);
            return true;
        }

    }
}
