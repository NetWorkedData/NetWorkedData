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
                FirstAcquisitionNotification = FirstAcquisitionNotification,
                AddItemNotification = AddItemNotification,
                RemoveItemNotification = RemoveItemNotification,
                NoMoreItemNotification = NoMoreItemNotification,
                Rarity = Rarity,
                HiddenInGame = HiddenInGame,
                Uncountable = Uncountable,
                Usable = Usable,
                ItemExtensionQuantity = NWDExportObject.ProcessNewArray(ItemExtensionQuantity?.GetReferenceAndQuantity()),
                Sprites = NWDExportObject.ProcessNewAsset(sProjectHub, sProjectId, tSprites),
                Textures = NWDExportObject.ProcessNewAsset(sProjectHub, sProjectId, tTextures),
                Colors = NWDExportObject.ProcessNewColor(tColors),
                Prefabs = NWDExportObject.ProcessNewAsset(sProjectHub, sProjectId, tPrefabs),
                EffectPrefab = NWDExportObject.ProcessNewAsset(sProjectHub, sProjectId, EffectPrefab),
                Categories = NWDExportObject.ProcessNewArray(CategoryList?.GetValue()),
                JSON = JSON,
                KeysValues = KeysValues,
                
                // Missing Data NWD3
                //ParameterList = ParameterList, //references
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
