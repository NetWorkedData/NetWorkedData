using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDCategory : NWDBasis
    {
        #if UNITY_EDITOR
        public override List<NWDExportObject> ExportNWD3(ulong sProjectHub, ulong sProjectId)
        {
            // Create object
            var tExport = new {
                // Specific data
                Name = NWDExportObject.ProcessNewLocalizedString(sProjectHub, sProjectId, Name),
                ItemDescription = NWDExportObject.ProcessNewArray(ItemDescription?.GetReference()),
                ParentCategoryList = NWDExportObject.ProcessNewArray(ParentCategoryList?.GetValue()),
                ChildrenCategoryList = NWDExportObject.ProcessNewArray(ChildrenCategoryList?.GetValue()),
                CascadeCategoryList = NWDExportObject.ProcessNewArray(CascadeCategoryList?.GetValue()),
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
