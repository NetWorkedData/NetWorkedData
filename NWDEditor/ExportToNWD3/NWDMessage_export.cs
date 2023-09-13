using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDMessage : NWDBasis
    {
        #if UNITY_EDITOR
        public override List<NWDExportObject> ExportNWD3(ulong sProjectHub, ulong sProjectId)
        {
            // Create object
            var tExport = new {
                // Specific data
                Style = Style,
                Type = Type,
                Domain = Domain,
                Code = Code,
                Title = NWDExportObject.ProcessNewLocalizedString(sProjectHub, sProjectId, Title),
                Description = NWDExportObject.ProcessNewLocalizedString(sProjectHub, sProjectId, Description),
                Validation = NWDExportObject.ProcessNewLocalizedString(sProjectHub, sProjectId, Validation),
                Cancel = NWDExportObject.ProcessNewLocalizedString(sProjectHub, sProjectId, Cancel),
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
