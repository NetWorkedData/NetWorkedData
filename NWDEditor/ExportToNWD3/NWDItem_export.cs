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
            List<NWDAssetType> tSprites = new List<NWDAssetType>();
            tSprites.Add(PrimarySprite);
            tSprites.Add(SecondarySprite);
            tSprites.Add(TertiarySprite);

            List<NWDAssetType> tTextures = new List<NWDAssetType>();
            tSprites.Add(PrimaryTexture);
            tSprites.Add(SecondaryTexture);
            tSprites.Add(TertiaryTexture);

            List<NWDColorType> tColors = new List<NWDColorType>();
            tColors.Add(PrimaryColor);
            tColors.Add(SecondaryColor);
            tColors.Add(TertiaryColor);

            List<NWDAssetType> tPrefabs = new List<NWDAssetType>();
            tPrefabs.Add(PrimaryPrefab);
            tPrefabs.Add(SecondaryPrefab);
            tPrefabs.Add(TertiaryPrefab);

            // Create object
            var tExport = new {
                // Specific data
                Name = NWDExportObject.ProcessNewLocalizedString(sProjectHub, sProjectId, Name),
                PluralName = NWDExportObject.ProcessNewLocalizedString(sProjectHub, sProjectId, PluralName),
                SubName = NWDExportObject.ProcessNewLocalizedString(sProjectHub, sProjectId, SubName),
                Description = NWDExportObject.ProcessNewLocalizedString(sProjectHub, sProjectId, Description),
                FirstAcquisitionNotification = FirstAcquisitionNotification,
                AddItemNotification = AddItemNotification,
                RemoveItemNotification = RemoveItemNotification,
                NoMoreItemNotification = NoMoreItemNotification,
                Rarity = Rarity,
                HiddenInGame = HiddenInGame,
                Uncountable = Uncountable,
                Usable = Usable,
                ItemExtensionQuantity = ItemExtensionQuantity, //Reference Quantity Type
                ParameterList = ParameterList,
                Sprites = NWDExportObject.ProcessNewAsset(sProjectHub, sProjectId, tSprites),
                Textures = NWDExportObject.ProcessNewAsset(sProjectHub, sProjectId, tTextures),
                Colors = NWDExportObject.ProcessNewColor(tColors),
                Prefabs = NWDExportObject.ProcessNewAsset(sProjectHub, sProjectId, tPrefabs),
                EffectPrefab = NWDExportObject.ProcessNewAsset(sProjectHub, sProjectId, EffectPrefab),
                //Categories = NWDExportObject.ProcessNewArray(CategoryList.Value.ToString()), //references
                JSON = JSON,
                KeysValues = KeysValues,
                // Missing Data NWD3
                //ItemGroupList = ItemGroupList, //references
                //CraftBookAttachment = CraftBookAttachment, //reference
                //FamilyList = FamilyList, //references
                //KeywordList = KeywordList, //references
                // Obsolete Data
                //DelayBeforeUse = DelayBeforeUse,
                //DurationOfUse = DurationOfUse,
                //DelayBeforeReUse = DelayBeforeReUse
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
