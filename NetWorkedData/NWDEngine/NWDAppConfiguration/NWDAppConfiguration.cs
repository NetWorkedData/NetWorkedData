using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	public partial class NWDAppConfiguration
	{
		#region properties
		//-------------------------------------------------------------------------------------------------------------
		public NWDDataLocalizationManager DataLocalizationManager = new NWDDataLocalizationManager();
		public NWDAppEnvironment DevEnvironment = new NWDAppEnvironment (NWDConstants.K_DEVELOPMENT_NAME, true);
		public NWDAppEnvironment PreprodEnvironment = new NWDAppEnvironment (NWDConstants.K_PREPRODUCTION_NAME, false);
		public NWDAppEnvironment ProdEnvironment = new NWDAppEnvironment (NWDConstants.K_PREPRODUCTION_NAME, false);
		public Dictionary<string,string> IntegritySaltDictionary = new Dictionary<string,string> ();
		public Dictionary<string,string> GenerateSaltDictionary = new Dictionary<string,string> ();
		//-------------------------------------------------------------------------------------------------------------
		#endregion

		#region shareInstance
		//-------------------------------------------------------------------------------------------------------------
		private static readonly NWDAppConfiguration kSharedInstance = new NWDAppConfiguration ();
		//-------------------------------------------------------------------------------------------------------------
		public static NWDAppConfiguration SharedInstance {
			get {
				return kSharedInstance; 
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		#endregion

		#region constructor
		//-------------------------------------------------------------------------------------------------------------
		public const string kEnvironmentSelectedKey = "kEnvironmentSelectedKey";
		//-------------------------------------------------------------------------------------------------------------
		public NWDAppConfiguration ()
		{
			Type tType = this.GetType ();
			var tMethodInfo = tType.GetMethod ("RestaureConfigurations", BindingFlags.Instance | BindingFlags.Public);
			if (tMethodInfo != null) {
				tMethodInfo.Invoke (this, null);
			} 
			else 
			{
				this.ProdEnvironment.Selected = false;
				this.PreprodEnvironment.Selected = false;
				this.DevEnvironment.Selected = true;
				Debug.Log (NWDConstants.kAlertSaltError);
			}
			// But in unity we bypass the restaure configuration to use the environment selected in the editor's preferences 
			#if UNITY_EDITOR
			// reset all environement to false
			this.ProdEnvironment.Selected = false;
			this.PreprodEnvironment.Selected = false;
			this.DevEnvironment.Selected = false;
			// We restaured environement selected in the preference and override the configurations file
			int tEnvironmentSelected = EditorPrefs.GetInt (kEnvironmentSelectedKey);
			switch (tEnvironmentSelected)
			{
			case 1 : this.PreprodEnvironment.Selected = true;
				break;
			case 2 : this.ProdEnvironment.Selected = true;
				break;
			case 0 :
			default : this.DevEnvironment.Selected = true;
				break;
			}
			#endif
		}
		//-------------------------------------------------------------------------------------------------------------
		#endregion

		#region instance method
		//-------------------------------------------------------------------------------------------------------------
		public void SetSalt (string sKeyFirst, string sKeySecond, string sValue)
		{
			string sKey = sKeyFirst + sKeySecond;
			if (IntegritySaltDictionary.ContainsKey (sKey)) {
				IntegritySaltDictionary [sKey] = sValue;
			} else {
				IntegritySaltDictionary.Add (sKey, sValue);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public string GetSalt (string sKeyFirst, string sKeySecond, string sKeyValid)
		{
			string sKey = sKeyFirst + sKeySecond;
			string rReturn = "";
			if (IntegritySaltDictionary.ContainsKey (sKey)) {
				rReturn = IntegritySaltDictionary [sKey];
			}
			if (rReturn == "") 
			{
				rReturn = NWDToolbox.RandomString (UnityEngine.Random.Range (12, 24));
				IntegritySaltDictionary.Add (sKey, rReturn);
				SetSaltValid (sKeyFirst, sKeyValid , "");
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetSaltValid (string sKeyFirst, string sKeyValid, string sValue)
		{
			string sKey = sKeyFirst + sKeyValid;
			if (GenerateSaltDictionary.ContainsKey (sKey)) {
				GenerateSaltDictionary [sKey] = sValue;
			} else {
				GenerateSaltDictionary.Add (sKey, sValue);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public string GetSaltValid (string sKeyFirst, string sKeyValid)
		{
			string sKey = sKeyFirst + sKeyValid;
			string rReturn = "";
			if (GenerateSaltDictionary.ContainsKey (sKey)) {
				rReturn = GenerateSaltDictionary [sKey];
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDAppEnvironment SelectedEnvironment ()
		{
			NWDAppEnvironment rReturn = ProdEnvironment;
			if (DevEnvironment.Selected == true) {
				rReturn = DevEnvironment;
			} else if (PreprodEnvironment.Selected == true) {
				rReturn = PreprodEnvironment;
			} else {
				rReturn = ProdEnvironment;
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public bool IsProdEnvironement ()
		{
			return ProdEnvironment.Selected;
		}
		//-------------------------------------------------------------------------------------------------------------
		public bool IsPreprodEnvironement ()
		{
			return PreprodEnvironment.Selected;
		}
		//-------------------------------------------------------------------------------------------------------------
		public bool IsDevEnvironement ()
		{
			return DevEnvironment.Selected;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDAppEnvironment[] AllEnvironements ()
		{
			NWDAppEnvironment[] tEnvironnements = new NWDAppEnvironment[] {
				DevEnvironment,
				PreprodEnvironment,
				ProdEnvironment
			};
			return tEnvironnements;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endregion
	}
}
//=====================================================================================================================
