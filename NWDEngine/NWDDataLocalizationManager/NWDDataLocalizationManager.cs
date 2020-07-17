//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#endif
//=====================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDataLocalizationManager
    {
        //-------------------------------------------------------------------------------------------------------------
        public const string kBaseDev = "BASE";
        public string LanguagesString = kBaseDev + ";de;en;es;fr;it;ja;ko;pt;ru;th";
        public Dictionary<string, string> LanguageDico = new Dictionary<string, string>(new StringIndexKeyComparer());
        //-------------------------------------------------------------------------------------------------------------
        static public string CheckLocalization(string sLanguage)
        {
            //NWDBenchmark.Start("????");
            //NWDBenchmark.Finish("????");

            //NWDBenchmark.Start();
            string rReturn = sLanguage;
            //NWDBenchmark.Start("1");
            NWDDataLocalizationManager tDataLocalizationManager = NWDAppConfiguration.SharedInstance().DataLocalizationManager;
            //NWDBenchmark.Finish("1");
            //NWDBenchmark.Start("2");
            if (tDataLocalizationManager.LanguagesString.Contains(sLanguage) == false)
            {
                rReturn = NWDAppConfiguration.SharedInstance().ProjetcLanguage;
            }
            //NWDBenchmark.Finish("2");
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public string SystemLanguageString ()
        {
            //Debug.Log("NWDDataLocalizationManager SystemLanguageString()");
            string tReturn = string.Empty;
            SystemLanguage tLocalLanguage = Application.systemLanguage;
            switch (tLocalLanguage)
            {
                //case SystemLanguage.Afrikaans:
                    //tReturn = "??";
                    //break;

                case SystemLanguage.Arabic:
                    tReturn = "ar";
                    break;

                //case SystemLanguage.Basque:
                    //tReturn = "??";
                    //break;

                //case SystemLanguage.Belarusian:
                    //tReturn = "??";
                    //break;

                //case SystemLanguage.Bulgarian:
                    //tReturn = "??";
                    //break;

                case SystemLanguage.Catalan:
                    tReturn = "ca";
                    break;

                case SystemLanguage.Chinese:
                    tReturn = "zh";
                    break;

                case SystemLanguage.Czech:
                    tReturn = "cs";
                    break;

                case SystemLanguage.Danish:
                    tReturn = "da";
                    break;

                case SystemLanguage.Dutch:
                    tReturn = "nl";
                    break;

                case SystemLanguage.English:
                    tReturn = "en";
                    break;

                //case SystemLanguage.Estonian:
                    //tReturn = "??";
                    //break;

                //case SystemLanguage.Faroese:
                    //tUserLanguage.Value = "??";
                    //break;

                case SystemLanguage.Finnish:
                    tReturn = "fi";
                    break;

                case SystemLanguage.French:
                    tReturn = "fr";
                    break;

                case SystemLanguage.German:
                    tReturn = "de";
                    break;

                case SystemLanguage.Greek:
                    tReturn = "el";
                    break;

                case SystemLanguage.Hebrew:
                    tReturn = "he";
                    break;

                case SystemLanguage.Hungarian:
                    tReturn = "hu";
                    break;

                //case SystemLanguage.Icelandic:
                    //tReturn = "??";
                    //break;

                case SystemLanguage.Indonesian:
                    tReturn = "id";
                    break;

                case SystemLanguage.Italian:
                    tReturn = "it";
                    break;

                case SystemLanguage.Japanese:
                    tReturn = "ja";
                    break;

                case SystemLanguage.Korean:
                    tReturn = "ko";
                    break;

                //case SystemLanguage.Latvian:
                    //tReturn = "??";
                    //break;

                //case SystemLanguage.Lithuanian:
                    //tReturn = "??";
                    //break;

                case SystemLanguage.Norwegian:
                    tReturn = "nb";
                    break;

                case SystemLanguage.Polish:
                    tReturn = "pl";
                    break;

                //case SystemLanguage.Portuguese:
                    //tReturn = "??";
                    //break;

                case SystemLanguage.Romanian:
                    tReturn = "ro";
                    break;

                case SystemLanguage.Russian:
                    tReturn = "ru";
                    break;

                //case SystemLanguage.SerboCroatian:
                    //tReturn = "??";
                    //break;

                case SystemLanguage.Slovak:
                    tReturn = "sk";
                    break;

                //case SystemLanguage.Slovenian:
                    //tReturn = "??";
                    //break;

                case SystemLanguage.Spanish:
                    tReturn = "es";
                    break;

                case SystemLanguage.Swedish:
                    tReturn = "sv";
                    break;

                case SystemLanguage.Thai:
                    tReturn = "th";
                    break;

                case SystemLanguage.Turkish:
                    tReturn = "tr";
                    break;

                case SystemLanguage.Ukrainian:
                    tReturn = "uk";
                    break;

                case SystemLanguage.Vietnamese:
                    tReturn = "vi";
                    break;

                case SystemLanguage.ChineseSimplified:
                    tReturn = "zhs";
                    break;

                case SystemLanguage.ChineseTraditional:
                    tReturn = "zht";
                    break;

                default:
                    tReturn = "en";
                    break;
            }
            return tReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDDataLocalizationManager()
        {
            // LanguageDico.Add ("Base Dev", kBaseDev);
            LanguageDico.Add("English (U.S.)", "en");
            //			LanguageDico.Add ("English (British)", "en_GB");	
            //			LanguageDico.Add ("English (Australian)", "en_AU");
            //			LanguageDico.Add ("English (Canadian)", "en_CA");	
            //			LanguageDico.Add ("English (Indian)", "en_IN");
            LanguageDico.Add("French", "fr");
            //			LanguageDico.Add ("French (Canadian)", "fr_CA");		
            LanguageDico.Add("Spanish", "es");
            //			LanguageDico.Add ("Spanish (Mexico)", "es_MX");		
            LanguageDico.Add("Portuguese", "pt");
            //			LanguageDico.Add ("Portuguese (Brazil)", "pt_BR");		
            LanguageDico.Add("Italian", "it");
            LanguageDico.Add("German", "de");
            LanguageDico.Add("Chinese (Simplified)", "zhs");
            LanguageDico.Add("Chinese (Traditional)", "zht");
            LanguageDico.Add("Dutch", "nl");
            LanguageDico.Add("Japanese", "ja");
            LanguageDico.Add("Korean", "ko");
            LanguageDico.Add("Vietnamese", "vi");
            LanguageDico.Add("Russian", "ru");
            LanguageDico.Add("Swedish", "sv");
            LanguageDico.Add("Danish", "da");
            LanguageDico.Add("Finnish", "fi");
            LanguageDico.Add("Norwegian (Bokmal)", "nb");
            //LanguageDico.Add ("Norwegian (Nynorsk)", "nn_NO");      
            LanguageDico.Add("Turkish", "tr");
            LanguageDico.Add("Greek", "el");
            LanguageDico.Add("Indonesian", "id");
            LanguageDico.Add("Malay", "ms");
            LanguageDico.Add("Thai", "th");
            LanguageDico.Add("Hindi", "hi");
            LanguageDico.Add("Hungarian", "hu");
            LanguageDico.Add("Polish", "pl");
            LanguageDico.Add("Czech", "cs");
            LanguageDico.Add("Slovak", "sk");
            LanguageDico.Add("Ukrainian", "uk");
            LanguageDico.Add("Croatian", "hr");
            LanguageDico.Add("Catalan", "ca");
            LanguageDico.Add("Romanian", "ro");
            LanguageDico.Add("Hebrew", "he");
            LanguageDico.Add("Arabic", "ar");
            //LanguageDico.Add("Arabic(Algeria)", "ar_DZ");
            //LanguageDico.Add("Arabic(Bahrain)", "ar_BH");
            //LanguageDico.Add("Arabic(Egypt)", "ar_EG");
            //LanguageDico.Add("Arabic(Iraq)", "ar_IQ");
            //LanguageDico.Add("Arabic(Jordan)", "ar_JO");
            //LanguageDico.Add("Arabic(Kuwait)", "ar_KW");
            //LanguageDico.Add("Arabic(Lebanon)", "ar_LB");
            //LanguageDico.Add("Arabic(Libya)", "ar_LY");
            //LanguageDico.Add("Arabic(Morocco)", "ar_MA");
            //LanguageDico.Add("Arabic(Oman)", "ar_OM");
            //LanguageDico.Add("Arabic(Qatar)", "ar_QA");
            //LanguageDico.Add("Arabic(Saudi Arabia)", "ar_SA");
            //LanguageDico.Add("Arabic(Sudan)", "ar_SD");
            //LanguageDico.Add("Arabic(Syria)", "ar_SY");
            //LanguageDico.Add("Arabic(Tunisia)", "ar_TN");
            //LanguageDico.Add("Arabic(Uae)", "ar_AE");
            //LanguageDico.Add("Arabic(Yemen)", "ar_YE");
        }
        //-------------------------------------------------------------------------------------------------------------
        public string ISOValue(string sValue)
        {
            if (sValue == "fr")
            {
                return "fr_FR";
            }
            else if (sValue == "en")
            {
                return "en_US";
            }
            else if (sValue == "es")
            {
                return "es_ES";
            }
            else if (sValue == "pt")
            {
                return "pt_PT";
            }
            else if (sValue == "it")
            {
                return "it_IT";
            }
            else if (sValue == "de")
            {
                return "de_DE";
            }
            else if (sValue == "zhs")
            {
                return "zh_CN";
            }
            else if (sValue == "zht")
            {
                return "zh_TW";
            }
            else if (sValue == "nl")
            {
                return "nl_NL";
            }
            else if (sValue == "ja")
            {
                return "ja_JP";
            }
            else if (sValue == "ko")
            {
                return "ko_KR";
            }
            else if (sValue == "vi")
            {
                return "vi_VN";
            }
            else if (sValue == "ru")
            {
                return "ru_RU";
            }
            else if (sValue == "sv")
            {
                return "sv_SE";
            }
            else if (sValue == "da")
            {
                return "da_DK";
            }
            else if (sValue == "fi")
            {
                return "fi_FI";
            }
            else if (sValue == "nb")
            {
                return "nb_NO";
            }
            else if (sValue == "tr")
            {
                return "tr_TR";
            }
            else if (sValue == "el")
            {
                return "el_GR";
            }
            else if (sValue == "id")
            {
                return "id_ID";
            }
            else if (sValue == "ms")
            {
                return "ms_My";
            }
            else if (sValue == "th")
            {
                return "th_TH";
            }
            else if (sValue == "hi")
            {
                return "hi_In";
            }
            else if (sValue == "hu")
            {
                return "hu_HU";
            }
            else if (sValue == "pl")
            {
                return "pl_PL";
            }
            else if (sValue == "cs")
            {
                return "cs_CZ";
            }
            else if (sValue == "sk")
            {
                return "sk_SK";
            }
            else if (sValue == "uk")
            {
                return "uk_UA";
            }
            else if (sValue == "hr")
            {
                return "hr_HR";
            }
            else if (sValue == "ca")
            {
                return "ca_ES";
            }
            else if (sValue == "ro")
            {
                return "ro_RO";
            }
            else if (sValue == "he")
            {
                return "he_IL";
            }
            else
            {
                return sValue;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================