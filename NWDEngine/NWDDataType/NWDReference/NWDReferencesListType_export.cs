using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDReferencesListType<K> : NWDReferenceMultiple where K : NWDBasis, new()
    {
        #if UNITY_EDITOR
        public object GetJsonConvert()
        {
            List<K> tList = GetRawDatasList();
            List<long> tReferencesList = new List<long>();
            if (tList != null)
            {
                foreach(var k in tList)
                {
                    tReferencesList.Add(NWDToolbox.NumericCleaner(k.Reference));
                }
            }
            return tReferencesList;
        }
        #endif
    }
}
