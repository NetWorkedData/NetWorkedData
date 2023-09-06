using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace NetWorkedData
{
    public partial class NWDItem : NWDBasis
    {
        public override List<NWDExportObject> ExportNWD3(ulong sProjectHub, ulong sProjectId)
        {
            // Create object for Language!
            var tExport = new {
                Name = NWDExportObject.ProcessNewLocalizedString(sProjectHub, sProjectId, Name),
                PluralName = NWDExportObject.ProcessNewLocalizedString(sProjectHub, sProjectId, PluralName),
                SubName = NWDExportObject.ProcessNewLocalizedString(sProjectHub, sProjectId, SubName),
                Description = NWDExportObject.ProcessNewLocalizedString(sProjectHub, sProjectId, Description),
                FirstAcquisitionNotification = 0,
                AddItemNotification = 0,
                RemoveItemNotification = 0,
                NoMoreItemNotification = 0,
                Rarity = 0,
                HiddenInGame = 0,
                Uncountable = 0,
                Usable = 1,
                DelayBeforeUse = 0,
                DurationOfUse = 0,
                DelayBeforeReUse = 0,
                ItemExtensionQuantity = "",
                ParameterList = "",
                PrimarySprite = "**Assets/Babaoo_Graph/Resources/Pictograms/Simple Inhibition.png**",
                SecondarySprite = "",
                TertiarySprite = "",
                PrimaryTexture = "",
                SecondaryTexture = "",
                TertiaryTexture = "",
                PrimaryColor = "2753FAFF",
                SecondaryColor = "000000FF",
                TertiaryColor = "000000FF",
                PrimaryPrefab = "",
                SecondaryPrefab = "",
                TertiaryPrefab = "",
                EffectPrefab = "",
                JSON = "",
                KeysValues = "",
                ItemGroupList = "",
                RangeAccess = 0,
                ID = 9,
                Reference = "62860253-481690",
                CheckList = 0,
                WebModel = 10,
                InternalKey = "Pictogram - Simple Inhibition",
                InternalDescription = "",
                Preview = "",
                AC = 1,
                DC = 1651497053,
                DM = 1673434260,
                DD = 0,
                XX = 0,
                Integrity = "",
                DS = 1673434260,
                DevSync = 1692018323,
                PreprodSync = 0,
                ProdSync = 0,
                Tag = 0,
                ServerHash = "",
                ServerLog = "",
                InError = 0,
                CraftBookAttachment = "",
                CategoryList = "",
                FamilyList = "",
                KeywordList = ""
            };

            //string tJsonUnity = JsonUtility.ToJson(tExport);
            string tJson = JsonConvert.SerializeObject(tExport);

            // Create object for Asset!
            // no asset here
            // Create root object

            List<NWDExportObject> rReturn = new List<NWDExportObject>();
            NWDExportObject tObject = new NWDExportObject(sProjectHub, sProjectId, Reference, InternalKey, InternalDescription, tJson, nameof(NWDItem), false);
            rReturn.Add(tObject);
            return rReturn;
        }
    }
}
