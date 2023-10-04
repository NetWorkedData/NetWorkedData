using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDCraftBook : NWDBasis
    {
        #if UNITY_EDITOR
        public override List<NWDExportObject> ExportNWD3()
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
                Prefabs = NWDExportObject.ProcessNewAsset(tPrefabs, AssetType.Object),
                RecipeHashesArray = RecipeHashesArray?.GetJsonConvert(),
            };

            string tJson = JsonConvert.SerializeObject(tExport);
            tJson = GetJSonMergeWithBase(tJson);

            List<NWDExportObject> rReturn = new List<NWDExportObject>();
            string tClassName = MethodBase.GetCurrentMethod().DeclaringType.Name;
            NWDExportObject tObject = new NWDExportObject(Reference, InternalKey, InternalDescription, tJson, tClassName, false);
            //rReturn.Add(tObject);
            return rReturn;
        }
        #endif
    }
}
