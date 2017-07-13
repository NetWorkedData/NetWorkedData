using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NetWorkedData
{
	public partial class NWDDataLocalizationManager
	{
		public const string kBaseDev = "BASE";
		public string LanguagesString = kBaseDev + ";de;en;es;fr;it;ja;ko;pt;ru;th";
		public Dictionary<string,string> LanguageDico = new Dictionary<string,string> ();

		public NWDDataLocalizationManager ()
		{
			// LanguageDico.Add ("Base Dev", kBaseDev);
			LanguageDico.Add ("English (U.S.)", "en");		
//			LanguageDico.Add ("English (British)", "en-GB");	
//			LanguageDico.Add ("English (Australian)", "en-AU");
//			LanguageDico.Add ("English (Canadian)", "en-CA");	
//			LanguageDico.Add ("English (Indian)", "en-IN");
			LanguageDico.Add ("French", "fr");	
//			LanguageDico.Add ("French (Canadian)", "fr-CA");		
			LanguageDico.Add ("Spanish", "es");		
//			LanguageDico.Add ("Spanish (Mexico)", "es-MX");		
			LanguageDico.Add ("Portuguese", "pt");		
//			LanguageDico.Add ("Portuguese (Brazil)", "pt-BR");		
			LanguageDico.Add ("Italian", "it");		
			LanguageDico.Add ("German", "de");	
			LanguageDico.Add ("Chinese (Simplified)", "zhs");		
			LanguageDico.Add ("Chinese (Traditional)", "zht");
//			LanguageDico.Add ("Chinese (Simplified)", "zh-Hans");		
//			LanguageDico.Add ("Chinese (Traditional)", "zh-Hant");		
//			LanguageDico.Add ("Chinese (Hong Kong)", "zh-HK");		
			LanguageDico.Add ("Dutch", "nl");		
			LanguageDico.Add ("Japanese", "ja");		
			LanguageDico.Add ("Korean", "ko");		
			LanguageDico.Add ("Vietnamese", "vi");		
			LanguageDico.Add ("Russian", "ru");		
			LanguageDico.Add ("Swedish", "sv");		
			LanguageDico.Add ("Danish", "da");		
			LanguageDico.Add ("Finnish", "fi");		
			LanguageDico.Add ("Norwegian (Bokmal)", "nb");		
			LanguageDico.Add ("Turkish", "tr");		
			LanguageDico.Add ("Greek", "el");		
			LanguageDico.Add ("Indonesian", "id");		
			LanguageDico.Add ("Malay", "ms");		
			LanguageDico.Add ("Thai", "th");		
			LanguageDico.Add ("Hindi", "hi");		
			LanguageDico.Add ("Hungarian", "hu");		
			LanguageDico.Add ("Polish", "pl");		
			LanguageDico.Add ("Czech", "cs");		
			LanguageDico.Add ("Slovak", "sk");		
			LanguageDico.Add ("Ukrainian", "uk");		
			LanguageDico.Add ("Croatian", "hr");		
			LanguageDico.Add ("Catalan", "ca");		
			LanguageDico.Add ("Romanian", "ro");		
			LanguageDico.Add ("Hebrew", "he");		
			LanguageDico.Add ("Arabic", "ar");
		}
	}
}