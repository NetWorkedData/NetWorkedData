using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDReferenceType<K> : NWDReferenceSimple where K : NWDBasis, new()
    {
        #if UNITY_EDITOR
        public object GetJsonConvert()
        {
            return "[" + GetReference() + "]";
        }
        #endif
    }
}
