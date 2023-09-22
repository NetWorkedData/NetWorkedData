using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDReferencesQuantityType<K> : NWDReferenceMultiple where K : NWDBasis, new()
    {
        #if UNITY_EDITOR
        public object GetJsonConvert()
        {
            var tExport = new {
                ReferenceQuantity = GetReferenceAndQuantity(),
            };
            return tExport;
        }
        #endif
    }
}
