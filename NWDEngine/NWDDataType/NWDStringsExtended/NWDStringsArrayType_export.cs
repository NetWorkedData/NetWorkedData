using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDStringsArrayType : NWEDataType
    {
        #if UNITY_EDITOR
        public object GetJsonConvert()
        {
            return GetReferences();
        }
        #endif
    }
}
