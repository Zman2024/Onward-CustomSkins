# Onward Custom Skins Mod 
An early development custom skins mod for Onward 1.7.7

## How to install

1. [Install BepInEx](https://github.com/BepInEx/BepInEx/wiki/Installation) to Onward (ignore if you already have it)

2. Either [download the latest release](https://github.com/Zman2024/Onward-CustomSkins/releases/latest) or build from source

3. Put `CustomSkins.dll` into `Onward/BepInEx/plugins`

4. Boom, installed

## How to get the original textures to use as a reference
I recommend using [Asset Studio GUI](https://github.com/Perfare/AssetStudio). Open asset studio, click "File" in the top left corner 
and then click "Load file". Next navigate to your onward install and go to the `Onward_Data` subfolder, then open the `resources.assets`
and wait for asset studio to finish loading the assets (this process will use about 3GB of ram btw). Go to the `Asset List` tab and search 
for the weapon texture you are looking for by using the list below. Once you have found selected texture(s) you want to export, right click it
and click "Export selected assets" then chose the folder you want the textures to be exported to. Asset studio will create a subfolder 
called "Texture2D" and in it will be all the the textures you exported. Now for the names of all the textures (they are a bit difficult to search for
so here's a list of them)

<details>
<summary>Click here to show texture names</summary>

```
 Gun                 Asset Name
MK18:         WPNT_MK18_AlbedoTransparency
Glock17:      glock_17_dif
AK12:         ak12_dif
G3A3:         g3a3_rifle_dif
M16A4:        m16a4_dif
M1911:        T_1911_ALB
MK16:         Rifle_Sand_D
AUG:          aug_diffuse
AK5C:         T_KA5C_ALB
M9:           m9_diffuse
M1014:        m1014_diffuse
MP5:          WPNT_MP5_AlbedoTransparency
P90:          T_SMG90_Albedo
552Commando:  552_commando_diff
M249:         m249_body_dif
M40A5:        m40a5_body_dif
MK17:         Rifle_Black_D
M39EMR:       m39_dif
TT30:         TT_COL
SKS:          sks_dif
AKM:          akm_diff
Makarov:      makarov_dif
FiveSeven:    fn_five_seven_black_dif
Flaregun:     flare_gun_dif
SPAS-12:      T_Shot12_ALB
Famas:        famas_body_dif
PKM:          pkm_body_dif
SVD:          svd_body_dif
AKS74u:       ak74u_dif
G36C:         g36c_body_diffuse
AS-VAL:       T_KA_Val_Black_ALB
RPG7:         rpg_7_dif
SV98:         sv98_body_dif
G3A3Auto:     g3a3_rifle_dif
```
</details>

## How to make a texture that works with this mod

All you need to do is export your texture to a png (or jpg, but png is better) and rename it with the 
following format in correspondence with the gun the texture belongs to: `(gun-name)-body-texture.png`
so for example if i had a mk18 texture it would need to be named `mk18-body-texture.png` and it would go
into the `Onward/CustomSkins` folder so it can be loaded by the mod. 

Here is a list of all the file names that are recognized by the mod: 
<details>
<summary>Click here to view file names</summary>

```
mk18-body-texture
glock17-body-texture
ak12-body-texture
g3a3-body-texture
m16a4-body-texture
m1911-body-texture
mk16-body-texture
aug-body-texture
ak5c-body-texture
m9-body-texture
m1014-body-texture
mp5-body-texture
p90-body-texture
552commando-body-texture
l86a2-lsw-body-texture
m249-body-texture
m40a5-body-texture
mk17-body-texture
m39emr-body-texture
tt30-body-texture
sks-body-texture
akm-body-texture
makarov-body-texture
fiveseven-body-texture
flaregun-body-texture
spas12-body-texture
famas-body-texture
pkm-body-texture
svd-body-texture
aks74u-body-texture
g36c-body-texture
asval-body-texture
tar21-body-texture
rpg7-body-texture
sv98-body-texture
g3a3-auto-body-texture
```
</details>

## Configuration

For the time being there are only 2 config variables (located at `Onward/BepInEx/config/CustomSkins.cfg`):

* `Enabled` which determines if the mod should load custom textures

* `LoadGlobal` which determines if textures should be globally replaced
For now all textures are globally replaced so if this is false no textures custom will be loaded
