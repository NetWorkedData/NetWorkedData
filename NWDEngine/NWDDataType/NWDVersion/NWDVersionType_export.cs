using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDVersionType : NWEDataType
    {
        #if UNITY_EDITOR
        public object GetJsonConvert()
        {
            GetIntVersion();
            var tExport = new {
                Major = MajorIndex,
                Minor = MinorIndex,
                Build = BuildIndex,
            };
            return tExport;
        }
        #endif
    }
}
