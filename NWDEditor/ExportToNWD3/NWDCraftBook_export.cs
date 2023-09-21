using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDCraftBook : NWDBasis
    {
        #if UNITY_EDITOR
        public override List<NWDExportObject> ExportNWD3(ulong sProjectHub, ulong sProjectId)
        {
            List<NWDAssetType> tPrefabs = new()
            {
                SuccessParticles,
                SuccessSound,
                FailParticles,
                FailSound
            };

            // Create object
            var tExport = new {
                // Specific data
                ItemDescription = ItemDescription?.GetJsonConvert(),
                OrderIsImportant = OrderIsImportant,
                RecipientGroup = RecipientGroup?.GetJsonConvert(),
                ItemGroupIngredient = ItemGroupIngredient?.GetJsonConvert(),
                ItemResult = ItemResult?.GetJsonConvert(),
                AdditionalReward = AdditionalReward?.GetJsonConvert(),
                Prefabs = NWDExportObject.ProcessNewAsset(sProjectHub, sProjectId, tPrefabs),
                RecipeHashesArray = RecipeHashesArray?.GetJsonConvert(),
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
