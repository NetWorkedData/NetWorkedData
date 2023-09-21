using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDStringsArrayType : NWEDataType
    {
        #if UNITY_EDITOR
        public string GetJsonConvert()
        {
            return "[" + GetReferences() + "]";
        }
        #endif
    }
}
