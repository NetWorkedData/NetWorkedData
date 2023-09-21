using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDReferencesListType<K> : NWDReferenceMultiple where K : NWDBasis, new()
    {
        #if UNITY_EDITOR
        public object GetJsonConvert()
        {
            string rReferences = "";
            List<K> tReferencesList = GetRawDatasList();
            if (tReferencesList != null)
            {
                foreach(var k in tReferencesList)
                {
                    if (rReferences.Length > 0) rReferences += ",";
                    rReferences += k.Reference;
                }
            }
            return "[" + rReferences + "]";
        }
        #endif
    }
}
