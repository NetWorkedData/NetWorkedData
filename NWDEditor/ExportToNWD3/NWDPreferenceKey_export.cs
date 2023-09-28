using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDPreferenceKey : NWDBasis
    {
        #if UNITY_EDITOR
        public override List<NWDExportObject> ExportNWD3()
        {
            // Create object
            var tExport = new {
                // Specific data
                Title = NWDExportObject.ProcessNewLocalizedString(Title),
                Description = NWDExportObject.ProcessNewLocalizedString(Description),
                Domain = Domain,
                Default = Default, //multitype
                NotifyChange = NotifyChange,
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
