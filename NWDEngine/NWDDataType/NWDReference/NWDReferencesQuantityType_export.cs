using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDReferencesQuantityType<K> : NWDReferenceMultiple where K : NWDBasis, new()
    {
        #if UNITY_EDITOR
        public object GetJsonConvert()
        {
            Dictionary<string, int> tDico = GetReferenceAndQuantity();
            string tReferenceQty = "";
            foreach(var k in tDico)
            {
                if (tReferenceQty.Length > 0) tReferenceQty += ",";
                tReferenceQty += $"\"{k.Key}\":{k.Value}";
            }

            var tExport = new {
                ReferenceQuantity = tReferenceQty,
            };
            return tExport;
        }
        #endif
    }
}
