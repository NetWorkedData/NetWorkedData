using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDCraftRecipient : NWDBasis
    {
        #if UNITY_EDITOR
        public override List<NWDExportObject> ExportNWD3()
        {
            List<NWDAssetType> tPrefabs = new()
            {
                ActiveParticles,
                ActiveSound,
                AddParticles,
                AddSound,
                DisactiveParticles,
                DisactiveSound
            };

            // Create object
            var tExport = new {
                // Specific data
                ItemDescription = ItemDescription?.GetJsonConvert(),
                CraftOnlyMax = CraftOnlyMax,
                CraftUnUsedElements = CraftUnUsedElements,
                Prefabs = NWDExportObject.ProcessNewAsset(tPrefabs),
                ItemFailedResult = ItemFailedResult?.GetJsonConvert(),
                ItemGroup = ItemGroup?.GetJsonConvert(),
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
