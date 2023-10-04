using System.Collections.Generic;
using System.Reflection;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDLocalization : NWDBasis
    {
        #if UNITY_EDITOR
        public override List<NWDExportObject> ExportNWD3()
        {
            object tText = null;
            try
            {
                tText = GetText(TextValue, out _);
            }
            catch
            {
                tText = new
                {
                    Value = new Dictionary<NWDLanguageEnum, string>()
                };
            }

            var tExport = new {
                // Specific
                Text = tText,
                Key = KeyValue,
                Context = TextValue.GetBaseString(),
                NeedToBeTranslated = true,
            };

            string tJson = JsonConvert.SerializeObject(tExport);
            tJson = GetJSonMergeWithBase(tJson);

            List<NWDExportObject> rReturn = new List<NWDExportObject>();
            string tClassName = MethodBase.GetCurrentMethod().DeclaringType.Name;
            NWDExportObject tObject = new NWDExportObject(Reference, InternalKey, InternalDescription, tJson, "NWDStringLocalization", false);
            rReturn.Add(tObject);
            return rReturn;
        }

        static public object GetText(NWDLocalizableType textValue, out string sTitle)
        {
            Dictionary<string, string> tDico = textValue.GetDictionary();
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

            if (!tNewDico.ContainsKey(NWDLanguageEnum.French))
            {
                string tBase = textValue.GetBaseString();
                if (!string.IsNullOrEmpty(tBase))
                {
                    tNewDico.Add(NWDLanguageEnum.French, tBase);
                }
            }

            if (tNewDico.Count == 0)
            {
                throw new System.Exception();
            }

            if (!tNewDico.TryGetValue(NWDLanguageEnum.French, out sTitle))
            {
                sTitle = "No title";
            }

            var tExport = new {
                Value = tNewDico,
            };

            return tExport;
        }
        #endif
    }
}
