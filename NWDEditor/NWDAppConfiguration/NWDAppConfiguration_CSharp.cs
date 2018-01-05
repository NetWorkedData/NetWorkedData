//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public partial class NWDAppConfiguration
	{
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Generates the C sharp file for all environments' restauration.
		/// </summary>
		/// <param name="sEnvironment">S environment.</param>
		public void GenerateCSharpFile (NWDAppEnvironment sEnvironment)
		{
			string tEngineRootFolder = "Assets";
			DateTime tTime = DateTime.UtcNow;
			string tDateTimeString = tTime.ToString ("yyyy-MM-dd");
			string tYearString = tTime.ToString ("yyyy");

			string tConstantsFile = "";
			tConstantsFile += "" +
			"//NWD Autogenerate File at " + tDateTimeString + "\n" +
			"//Copyright NetWorkedDatas ideMobi " + tYearString + "\n" +
			"//Created by Jean-François CONTART\n" +
			"//-------------------- \n" +
			"using System.Collections;\n" +
			"using System.Collections.Generic;\n" +
			"using UnityEngine;\n" +
			"//=====================================================================================================================\n" +
			"namespace NetWorkedData\n" +
			"{\n" +
			"//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\n" +
			"\tpublic partial class NWDAppConfiguration\n" +
			"\t{\n" +
			"\t//-------------------------------------------------------------------------------------------------------------\n" +
			"\t\tpublic void RestaureConfigurations ()\n" +
			"\t\t{\n" +
			"\t\t\t//Debug.Log(\"NWDAppConfiguration Restaure Config\");\n" +
			"\t\t\t//Salts regenerate (Calgon© is back :-p )\n";
			foreach (KeyValuePair<string, string> tEntry in IntegritySaltDictionary) {
				tConstantsFile += "\t\t\tIntegritySaltDictionary[\"" + tEntry.Key + "\"]=\"" + tEntry.Value.Replace ("\"", "\\\"") + "\";\n";
		
			}
			tConstantsFile += "//Salts Validity\n";
			foreach (KeyValuePair<string, string> tEntry in GenerateSaltDictionary) {
				tConstantsFile += "\t\t\tGenerateSaltDictionary[\"" + tEntry.Key + "\"]=\"" + tEntry.Value.Replace ("\"", "\\\"") + "\";\n";
			}
			tConstantsFile += "//Environments restaure\n";
			// Select the build environnement
			if (sEnvironment == NWDAppConfiguration.SharedInstance.DevEnvironment) {
				tConstantsFile += "" +
				"\t\t\tthis.ProdEnvironment.Selected = false;\n" +
				"\t\t\tthis.PreprodEnvironment.Selected = false;\n" +
				"\t\t\tthis.DevEnvironment.Selected = true;\n";
			} else if (sEnvironment == NWDAppConfiguration.SharedInstance.PreprodEnvironment) {
				tConstantsFile += "" +
					"\t\t\tthis.ProdEnvironment.Selected = false;\n" +
					"\t\t\tthis.PreprodEnvironment.Selected = true;\n" +
					"\t\t\tthis.DevEnvironment.Selected = false;\n";
			} else if (sEnvironment == NWDAppConfiguration.SharedInstance.ProdEnvironment) {
				tConstantsFile += "" +
					"\t\t\tthis.ProdEnvironment.Selected = true;\n" +
					"\t\t\tthis.PreprodEnvironment.Selected = false;\n" +
					"\t\t\tthis.DevEnvironment.Selected = false;\n";
			}
			
			tConstantsFile += "" +
			"\t\t\t// Prod environment\n" +
			"\t\t\tthis.ProdEnvironment.Environment = \"" + this.ProdEnvironment.Environment.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.ProdEnvironment.DataSHAPassword = \"" + this.ProdEnvironment.DataSHAPassword.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.ProdEnvironment.DataSHAVector = \"" + this.ProdEnvironment.DataSHAVector.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.ProdEnvironment.SaltStart = \"" + this.ProdEnvironment.SaltStart.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.ProdEnvironment.SaltEnd = \"" + this.ProdEnvironment.SaltEnd.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.ProdEnvironment.SaltFrequency = " + this.ProdEnvironment.SaltFrequency.ToString () + ";\n" +
			"\t\t\tthis.ProdEnvironment.ServerHTTPS = \"" + this.ProdEnvironment.ServerHTTPS.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\t#if UNITY_EDITOR\n" +
			"\t\t\tthis.ProdEnvironment.ServerHost = \"" + this.ProdEnvironment.ServerHost.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.ProdEnvironment.ServerUser = \"" + this.ProdEnvironment.ServerUser.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.ProdEnvironment.ServerPassword = \"" + this.ProdEnvironment.ServerPassword.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.ProdEnvironment.ServerBase = \"" + this.ProdEnvironment.ServerBase.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.ProdEnvironment.AdminKey = \"" + this.ProdEnvironment.AdminKey.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.ProdEnvironment.RescueEmail = \"" + this.ProdEnvironment.RescueEmail.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.ProdEnvironment.TokenHistoric = " + this.ProdEnvironment.TokenHistoric.ToString () + ";\n" +
			"\t\t\t#endif\n" +
			"\t\t\tthis.ProdEnvironment.FacebookAppID = \"" + this.ProdEnvironment.FacebookAppID.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.ProdEnvironment.FacebookAppSecret = \"" + this.ProdEnvironment.FacebookAppSecret.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.ProdEnvironment.GoogleAppKey = \"" + this.ProdEnvironment.GoogleAppKey.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.ProdEnvironment.UnityAppKey = \"" + this.ProdEnvironment.UnityAppKey.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.ProdEnvironment.TwitterAppKey = \"" + this.ProdEnvironment.TwitterAppKey.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.ProdEnvironment.AppName = \"" + this.ProdEnvironment.AppName.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.ProdEnvironment.BuildTimestamp = " + NWDToolbox.Timestamp ().ToString () + ";\n" +
//			"\t\t\tthis.ProdEnvironment.Version = \"" + this.ProdEnvironment.Version.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.ProdEnvironment.LoadPreferences ();\n" +
			"\t\t\tthis.ProdEnvironment.FormatVerification ();\n" +
			"\t\t\t// Preprod environment\n" +
			"\t\t\tthis.PreprodEnvironment.Environment = \"" + this.PreprodEnvironment.Environment.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.PreprodEnvironment.DataSHAPassword = \"" + this.PreprodEnvironment.DataSHAPassword.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.PreprodEnvironment.DataSHAVector = \"" + this.PreprodEnvironment.DataSHAVector.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.PreprodEnvironment.SaltStart = \"" + this.PreprodEnvironment.SaltStart.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.PreprodEnvironment.SaltEnd = \"" + this.PreprodEnvironment.SaltEnd.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.PreprodEnvironment.SaltFrequency = " + this.PreprodEnvironment.SaltFrequency.ToString () + ";\n" +
			"\t\t\tthis.PreprodEnvironment.ServerHTTPS = \"" + this.PreprodEnvironment.ServerHTTPS.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\t#if UNITY_EDITOR\n" +
			"\t\t\tthis.PreprodEnvironment.ServerHost = \"" + this.PreprodEnvironment.ServerHost.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.PreprodEnvironment.ServerUser = \"" + this.PreprodEnvironment.ServerUser.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.PreprodEnvironment.ServerPassword = \"" + this.PreprodEnvironment.ServerPassword.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.PreprodEnvironment.ServerBase = \"" + this.PreprodEnvironment.ServerBase.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.PreprodEnvironment.AdminKey = \"" + this.PreprodEnvironment.AdminKey.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.PreprodEnvironment.RescueEmail = \"" + this.PreprodEnvironment.RescueEmail.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.PreprodEnvironment.TokenHistoric = " + this.PreprodEnvironment.TokenHistoric.ToString () + ";\n" +
			"\t\t\t#endif\n" +
			"\t\t\tthis.PreprodEnvironment.FacebookAppID = \"" + this.PreprodEnvironment.FacebookAppID.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.PreprodEnvironment.FacebookAppSecret = \"" + this.PreprodEnvironment.FacebookAppSecret.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.PreprodEnvironment.GoogleAppKey = \"" + this.PreprodEnvironment.GoogleAppKey.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.PreprodEnvironment.UnityAppKey = \"" + this.PreprodEnvironment.UnityAppKey.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.PreprodEnvironment.TwitterAppKey = \"" + this.PreprodEnvironment.TwitterAppKey.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.PreprodEnvironment.AppName = \"" + this.PreprodEnvironment.AppName.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.PreprodEnvironment.BuildTimestamp = " + NWDToolbox.Timestamp ().ToString () + ";\n" +
//			"\t\t\tthis.PreprodEnvironment.Version = \"" + this.PreprodEnvironment.Version.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.PreprodEnvironment.LoadPreferences ();\n" +
			"\t\t\tthis.PreprodEnvironment.FormatVerification ();\n" +
			"#if UNITY_EDITOR\n" +
			"\t\t\t// Dev environment\n" +
			"\t\t\tthis.DevEnvironment.Environment = \"" + this.DevEnvironment.Environment.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.DevEnvironment.DataSHAPassword = \"" + this.DevEnvironment.DataSHAPassword.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.DevEnvironment.DataSHAVector = \"" + this.DevEnvironment.DataSHAVector.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.DevEnvironment.SaltStart = \"" + this.DevEnvironment.SaltStart.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.DevEnvironment.SaltEnd = \"" + this.DevEnvironment.SaltEnd.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.DevEnvironment.SaltFrequency = " + this.DevEnvironment.SaltFrequency.ToString () + ";\n" +
			"\t\t\tthis.DevEnvironment.ServerHTTPS = \"" + this.DevEnvironment.ServerHTTPS.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.DevEnvironment.ServerHost = \"" + this.DevEnvironment.ServerHost.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.DevEnvironment.ServerUser = \"" + this.DevEnvironment.ServerUser.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.DevEnvironment.ServerPassword = \"" + this.DevEnvironment.ServerPassword.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.DevEnvironment.ServerBase = \"" + this.DevEnvironment.ServerBase.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.DevEnvironment.FacebookAppID = \"" + this.DevEnvironment.FacebookAppID.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.DevEnvironment.FacebookAppSecret = \"" + this.DevEnvironment.FacebookAppSecret.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.DevEnvironment.GoogleAppKey = \"" + this.DevEnvironment.GoogleAppKey.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.DevEnvironment.UnityAppKey = \"" + this.DevEnvironment.UnityAppKey.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.DevEnvironment.TwitterAppKey = \"" + this.DevEnvironment.TwitterAppKey.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.DevEnvironment.AdminKey = \"" + this.DevEnvironment.AdminKey.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.DevEnvironment.AppName = \"" + this.DevEnvironment.AppName.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.DevEnvironment.RescueEmail = \"" + this.DevEnvironment.RescueEmail.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.DevEnvironment.BuildTimestamp = " + NWDToolbox.Timestamp ().ToString () + ";\n" +
			"\t\t\tthis.DevEnvironment.TokenHistoric = " + this.DevEnvironment.TokenHistoric.ToString () + ";\n" +
//			"\t\t\tthis.DevEnvironment.Version = \"" + this.DevEnvironment.Version.Replace ("\"", "\\\"") + "\";\n" +
			"\t\t\tthis.DevEnvironment.LoadPreferences ();\n" +
			"\t\t\tthis.DevEnvironment.FormatVerification ();\n" +
			"#endif\n" +
			"\n" +
			"\t\t\t// Restaure languages \n" +
			"\t\t\tthis.DataLocalizationManager.LanguagesString = \"" + this.DataLocalizationManager.LanguagesString + "\";\n" +
			"\t\t}\n" +
			"\t//-------------------------------------------------------------------------------------------------------------\n" +
			"\t}\n" +
			"//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\n" +
			"}\n" +
			"//=====================================================================================================================\n";
			
            // File.WriteAllText(tEngineRootFolder + "/NWDConfigurations.cs", tConstantsFile);
			// force to import this file by Unity3D
			// AssetDatabase.ImportAsset (tEngineRootFolder + "/NWDConfigurations.cs");

            string tPath = NWDFindPackage.PathOfPackage("/NWDConfigurations.cs");
            File.WriteAllText(tPath, tConstantsFile);
            // force to import this file by Unity3D
            AssetDatabase.ImportAsset(tPath);
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================