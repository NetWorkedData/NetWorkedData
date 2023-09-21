using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDVersionType : NWEDataType
    {
        #if UNITY_EDITOR
        public string GetJsonConvert()
        {
            GetIntVersion();
            var tExport = new {
                Major = MajorIndex,
                Minor = MinorIndex,
                Build = BuildIndex,
            };
            return JsonConvert.SerializeObject(tExport);
        }
        #endif
    }
}
