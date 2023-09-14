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
                ItemDescription = NWDExportObject.ProcessNewArray(ItemDescription?.GetReference()),
                OrderIsImportant = OrderIsImportant,
                RecipientGroup = NWDExportObject.ProcessNewArray(RecipientGroup?.GetReference()),
                ItemGroupIngredient = ItemGroupIngredient, //reference array
                ItemResult = NWDExportObject.ProcessNewArray(ItemResult?.GetReferenceAndQuantity()),
                AdditionalReward = NWDExportObject.ProcessNewArray(AdditionalReward?.GetValue()),
                Prefabs = NWDExportObject.ProcessNewAsset(sProjectHub, sProjectId, tPrefabs),
                RecipeHashesArray = RecipeHashesArray, //string array
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
