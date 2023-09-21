using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDVector2 : NWEDataType
    {
        #if UNITY_EDITOR
        public object GetJsonConvert()
        {
            var tExport = new {
                X = GetVector().x,
                Y = GetVector().y,
            };
            return tExport;
        }
        #endif
    }
}
