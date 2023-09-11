using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDItem : NWDBasis
    {
        #if UNITY_EDITOR
        public override List<NWDExportObject> ExportNWD3(ulong sProjectHub, ulong sProjectId)
        {
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
                DelayBeforeUse = DelayBeforeUse,
                DurationOfUse = DurationOfUse,
                DelayBeforeReUse = DelayBeforeReUse,
                ItemExtensionQuantity = ItemExtensionQuantity, //references
                ParameterList = ParameterList,
                PrimarySprite = NWDExportObject.ProcessNewAsset(sProjectHub, sProjectId, PrimarySprite),
                SecondarySprite = NWDExportObject.ProcessNewAsset(sProjectHub, sProjectId, SecondarySprite),
                TertiarySprite = NWDExportObject.ProcessNewAsset(sProjectHub, sProjectId, TertiarySprite),
                PrimaryTexture = NWDExportObject.ProcessNewAsset(sProjectHub, sProjectId, PrimaryTexture),
                SecondaryTexture = NWDExportObject.ProcessNewAsset(sProjectHub, sProjectId, SecondaryTexture),
                TertiaryTexture = NWDExportObject.ProcessNewAsset(sProjectHub, sProjectId, TertiaryTexture),
                PrimaryColor = PrimaryColor,
                SecondaryColor = SecondaryColor,
                TertiaryColor = TertiaryColor,
                PrimaryPrefab = PrimaryPrefab,
                SecondaryPrefab = SecondaryPrefab,
                TertiaryPrefab = TertiaryPrefab,
                EffectPrefab = EffectPrefab,
                JSON = JSON,
                KeysValues = KeysValues,
                ItemGroupList = ItemGroupList, //references
                CraftBookAttachment = CraftBookAttachment, //reference
                CategoryList = CategoryList, //references
                FamilyList = FamilyList, //references
                KeywordList = KeywordList, //references
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
