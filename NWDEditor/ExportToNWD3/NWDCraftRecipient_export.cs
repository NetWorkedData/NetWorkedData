using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDCraftRecipient : NWDBasis
    {
        #if UNITY_EDITOR
        public override List<NWDExportObject> ExportNWD3(ulong sProjectHub, ulong sProjectId)
        {
            // Create object
            var tExport = new {
                // Specific data
                ItemDescription = ItemDescription, //reference
                CraftOnlyMax = CraftOnlyMax,
                CraftUnUsedElements = CraftUnUsedElements,
                ActiveParticles = ActiveParticles,
                ActiveSound = ActiveSound,
                AddParticles = AddParticles,
                AddSound = AddSound,
                DisactiveParticles = DisactiveParticles,
                DisactiveSound = DisactiveSound,
                ItemFailedResult = ItemFailedResult, //references
                ItemGroup = ItemGroup, //reference
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
