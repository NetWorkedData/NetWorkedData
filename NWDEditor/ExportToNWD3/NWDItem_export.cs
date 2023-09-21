using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace NetWorkedData
{
    public partial class NWDItem : NWDBasis
    {
        #if UNITY_EDITOR
        public override List<NWDExportObject> ExportNWD3(ulong sProjectHub, ulong sProjectId)
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
                Name = NWDExportObject.ProcessNewLocalizedString(sProjectHub, sProjectId, Name),
                PluralName = NWDExportObject.ProcessNewLocalizedString(sProjectHub, sProjectId, PluralName),
                SubName = NWDExportObject.ProcessNewLocalizedString(sProjectHub, sProjectId, SubName),
                Description = NWDExportObject.ProcessNewLocalizedString(sProjectHub, sProjectId, Description),
                Rarity = Rarity,
                HiddenInGame = HiddenInGame,
                Uncountable = Uncountable,
                //Stackable = 0, //New Data NWD3
                Usable = Usable,
                ItemExtensionQuantity = ItemExtensionQuantity?.GetJsonConvert(),
                Sprites = NWDExportObject.ProcessNewAsset(sProjectHub, sProjectId, tSprites),
                Textures = NWDExportObject.ProcessNewAsset(sProjectHub, sProjectId, tTextures),
                Colors = NWDExportObject.ProcessNewColor(tColors),
                Prefabs = NWDExportObject.ProcessNewAsset(sProjectHub, sProjectId, tPrefabs),
                EffectPrefab = NWDExportObject.ProcessNewAsset(sProjectHub, sProjectId, EffectPrefab),
                Categories = CategoryList?.GetJsonConvert(),
                JSON = JSON,
                KeysValues = KeysValues,
            };
            
            string tJson = JsonConvert.SerializeObject(tExport);
            tJson = GetJSonMergeWithBase(tJson);

            List<NWDExportObject> rReturn = new List<NWDExportObject>();
            NWDExportObject tObject = new NWDExportObject(sProjectHub, sProjectId, Reference, InternalKey, InternalDescription, tJson, nameof(NWDItem), false);
            rReturn.Add(tObject);
            return rReturn;
        }
        #endif
    }
}
