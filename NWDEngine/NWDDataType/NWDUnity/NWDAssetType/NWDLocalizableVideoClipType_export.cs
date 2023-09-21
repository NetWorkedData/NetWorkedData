using System.Collections.Generic;
using Newtonsoft.Json;

namespace NetWorkedData
{
    public partial class NWDLocalizableVideoClipType : NWDLocalizableType
    {
        #if UNITY_EDITOR
        public object GetJsonConvert()
        {
            var tExport = new {
                // Specific
                Video = GetVideo(),
                Key = "",
                Context = GetBaseVideo(),
                NeedToBeTranslated = true,
            };

            return tExport;
        }

        public object GetVideo()
        {
            Dictionary<string, string> tDico = GetDictionary();
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

        public string GetBaseVideo()
        {
            Dictionary<string, string> tDico = GetDictionary();
            string rReference = "";
            foreach(var k in tDico)
            {
                if(k.Key.Equals("base"))
                {
                    rReference = $"{k.Value}";
                    break;
                }
            }

            return rReference;
        }
        #endif
    }
}
