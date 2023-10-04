using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;
using UnityEngine;

namespace NetWorkedData
{
    public partial class NWDItem : NWDBasis
    {
        #if UNITY_EDITOR
        public override List<NWDExportObject> ExportNWD3()
        {
            List<NWDAssetType> tSprites = new()
            {
                PrimarySprite,
                SecondarySprite,
                TertiarySprite
            };

            List<NWDAssetType> tTextures = new()
            {
                PrimaryTexture,
                SecondaryTexture,
                TertiaryTexture
            };

            List<NWDColorType> tColors = new()
            {
                PrimaryColor,
                SecondaryColor,
                TertiaryColor
            };

            List<NWDAssetType> tPrefabs = new()
            {
                PrimaryPrefab,
                SecondaryPrefab,
                TertiaryPrefab
            };

            // Create object
            var tExport = new {
                // Specific data
                Name = NWDExportObject.ProcessNewLocalizedString(Name),
                PluralName = NWDExportObject.ProcessNewLocalizedString(PluralName),
                SubName = NWDExportObject.ProcessNewLocalizedString(SubName),
                Description = NWDExportObject.ProcessNewLocalizedString(Description),
                Rarity = Rarity,
                HiddenInGame = HiddenInGame,
                Uncountable = Uncountable,
                //Stackable = 0, //New Data NWD3
                Usable = Usable,
                ItemExtensionQuantity = ItemExtensionQuantity?.GetJsonConvert(),
                Sprites = NWDExportObject.ProcessNewAsset(tSprites, AssetType.Object),
                Textures = NWDExportObject.ProcessNewAsset(tTextures, AssetType.Object),
                Colors = NWDExportObject.ProcessNewColor(tColors),
                Prefabs = NWDExportObject.ProcessNewAsset(tPrefabs, AssetType.Object),
                EffectPrefab = NWDExportObject.ProcessNewAsset(EffectPrefab),
                Categories = CategoryList?.GetJsonConvert(),
                JSON = JSON,
                KeysValues = KeysValues,
            };
            
            string tJson = JsonConvert.SerializeObject(tExport);
            tJson = GetJSonMergeWithBase(tJson);

            List<NWDExportObject> rReturn = new List<NWDExportObject>();
            string tClassName = MethodBase.GetCurrentMethod().DeclaringType.Name;
            NWDExportObject tObject = new NWDExportObject(Reference, InternalKey, InternalDescription, tJson, tClassName, false);
            rReturn.Add(tObject);
            return rReturn;
        }
        #endif
    }
}
