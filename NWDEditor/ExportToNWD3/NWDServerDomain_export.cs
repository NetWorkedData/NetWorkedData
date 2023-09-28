using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDServerDomain : NWDBasis
    {
        #if UNITY_EDITOR
        public override List<NWDExportObject> ExportNWD3()
        {
            // Create object
            var tExport = new {
                // Specific data
                ServerDNS = ServerDNS,
                BalanceLoad = BalanceLoad,
                Dev = Dev,
                Preprod = Preprod,
                Prod = Prod,
                Information = Information,
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
