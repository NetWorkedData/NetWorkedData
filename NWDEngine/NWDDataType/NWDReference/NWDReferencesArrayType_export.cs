using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDReferencesArrayType<K> : NWDReferenceMultiple where K : NWDBasis, new()
    {
        #if UNITY_EDITOR
        public object GetJsonConvert()
        {
            return "[" + GetReferences() + "]";
        }
        #endif
    }
}
