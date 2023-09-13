using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDCraftBook : NWDBasis
    {
        #if UNITY_EDITOR
        public override List<NWDExportObject> ExportNWD3(ulong sProjectHub, ulong sProjectId)
        {
            // Create object
            var tExport = new {
                // Specific data
                ItemDescription = ItemDescription, //reference
                OrderIsImportant = OrderIsImportant,
                RecipientGroup = RecipientGroup, //reference
                ItemGroupIngredient = ItemGroupIngredient,
                ItemResult = ItemResult, //references
                AdditionalReward = AdditionalReward, //reference
                SuccessParticles = SuccessParticles,
                SuccessSound = SuccessSound,
                FailParticles = FailParticles,
                FailSound = FailSound,
                RecipeHashesArray = RecipeHashesArray,
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
