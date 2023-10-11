using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDReferencesQuantityType<K> : NWDReferenceMultiple where K : NWDBasis, new()
    {
        #if UNITY_EDITOR
        public object GetJsonConvert()
        {
            var tExport = new {
                ReferenceQuantity = GetReferenceAndQuantity().ToDictionary(x => NWDToolbox.NumericCleaner(x.Key), y => (long)y.Value),
            };
            return tExport;
        }
        #endif
    }
}
