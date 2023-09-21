using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDLocalization : NWDBasis
    {
        #if UNITY_EDITOR
        public override List<NWDExportObject> ExportNWD3(ulong sProjectHub, ulong sProjectId)
        {
            var tExport = new {
                // Specific
                Text = GetText(),
                Key = KeyValue,
                Context = TextValue.GetBaseString(),
                NeedToBeTranslated = true,
            };

            string tJson = JsonConvert.SerializeObject(tExport);
            tJson = GetJSonMergeWithBase(tJson);

            List<NWDExportObject> rReturn = new List<NWDExportObject>();
            NWDExportObject tObject = new NWDExportObject(sProjectHub, sProjectId, Reference, InternalKey, InternalDescription, tJson, nameof(NWDItem), false);
            rReturn.Add(tObject);
            return rReturn;
        }

        public object GetText()
        {
            Dictionary<string, string> tDico = TextValue.GetDictionary();
            Dictionary<NWDLanguageEnum, string> tNewDico = new Dictionary<NWDLanguageEnum, string>();
            foreach(var k in tDico)
            {
                if(k.Key.Equals("fr"))
                {
                    tNewDico.Add(NWDLanguageEnum.French, k.Value);
                }
                else if(k.Key.Equals("en"))
                {
                    tNewDico.Add(NWDLanguageEnum.English, k.Value);
                }
            }

            var tExport = new {
                Value = tNewDico,
            };
            return tExport;
        }
        #endif
    }
}
