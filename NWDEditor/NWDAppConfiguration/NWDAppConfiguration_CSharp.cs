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
        public void GenerateCSharpFile(NWDAppEnvironment sEnvironment)
        {
            Debug.LogWarning("GenerateCSharpFile !!!");
            //string tEngineRootFolder = "Assets";
            DateTime tTime = DateTime.UtcNow;
            string tDateTimeString = tTime.ToString("yyyy-MM-dd");
            string tYearString = tTime.ToString("yyyy");
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
            "\t\t\tWebFolder = \"" + WebFolder + "\";\n" +
            "\t\t\tWebBuild = " + WebBuild + ";\n" +
            "\t\t\tRowDataIntegrity = " + RowDataIntegrity.ToString().ToLower() + ";\n" +
            "\t\t\t//Debug.Log(\"NWDAppConfiguration Restaure Config\");\n" +
            "\t\t\t//Salts regenerate (Calgon© is back :-p )\n";
            foreach (KeyValuePair<string, string> tEntry in IntegritySaltDictionary)
            {
                tConstantsFile += "\t\t\tIntegritySaltDictionary[\"" + tEntry.Key + "\"]=\"" + tEntry.Value.Replace("\"", "\\\"") + "\";\n";
            }
            tConstantsFile += "//Salts Validity\n";
            foreach (KeyValuePair<string, string> tEntry in GenerateSaltDictionary)
            {
                tConstantsFile += "\t\t\tGenerateSaltDictionary[\"" + tEntry.Key + "\"]=\"" + tEntry.Value.Replace("\"", "\\\"") + "\";\n";
            }
            tConstantsFile += "\t\t\t//WebService regenerate (Apache or Siou)\n";

            tConstantsFile += "\t\t\t kWebBuildkCSVAssemblyOrderArray = new Dictionary<int, Dictionary<string, string[]>>();\n";
            tConstantsFile += "\t\t\t kWebBuildkSLQAssemblyOrderArray = new Dictionary<int, Dictionary<string, string[]>>();\n";
            tConstantsFile += "\t\t\t kWebBuildkSLQAssemblyOrder = new Dictionary<int, Dictionary<string, string>>();\n";
            tConstantsFile += "\t\t\t kWebBuildkSLQIntegrityOrder = new Dictionary<int, Dictionary<string, List<string>>>();\n";
            tConstantsFile += "\t\t\t kWebBuildkSLQIntegrityServerOrder = new Dictionary<int, Dictionary<string, List<string>>>();\n";
            tConstantsFile += "\t\t\t kWebBuildkDataAssemblyPropertiesList = new Dictionary<int, Dictionary<string, List<string>>>();\n";

            tConstantsFile += "\t\t\t//WebService regenerate all version of webservice\n";

            tConstantsFile += "\t\t\t WSList = new Dictionary<int, bool>();\n";
            foreach (KeyValuePair<int, bool> tWS in WSList)
            {
                if (tWS.Value == true)
                {
                    tConstantsFile += "\t\t\t WSList.Add(" + tWS.Key + "," + tWS.Value.ToString().ToLower() + ");\n";
                }
                else
                {
                    // TODO remove the web folder 
                    string tWebServiceFolder = NWDAppConfiguration.SharedInstance().WebServiceFolder();
                    if (AssetDatabase.IsValidFolder("Assets/NetWorkedDataServer/"+tWebServiceFolder) == true)
                    {
                        //AssetDatabase.DeleteAsset("Assets/NetWorkedDataServer/" + tWebServiceFolder);

                    }
                }
            }


            tConstantsFile += "\t\t\t TagList = new Dictionary<int, string>();\n";
            // bug in writing order
            //foreach (KeyValuePair<int, string> tTag in TagList)
            //{
            //    tConstantsFile += "\t\t\t TagList.Add(" + tTag.Key + ",\"" + tTag.Value + "\");\n";
            //}
            // write in good order
            for (int tI = -1; tI <= NWDAppConfiguration.SharedInstance().TagNumber; tI++)
            {
                if (TagList.ContainsKey(tI) == true)
                {
                    tConstantsFile += "\t\t\t TagList.Add(" + tI + ",\"" + TagList[tI] + "\");\n";
                }
            }

            tConstantsFile += "//Environments restaure\n";
            // Select the build environnement
            if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            {
                tConstantsFile += "" +
                "\t\t\tthis.ProdEnvironment.Selected = false;\n" +
                "\t\t\tthis.PreprodEnvironment.Selected = false;\n" +
                "\t\t\tthis.DevEnvironment.Selected = true;\n" +
                "\t\t\t// For tests\n" +
                "\t\t\tthis.ProdEnvironment.AccountsForTests = \"\";\n" +
                "\t\t\tthis.PreprodEnvironment.AccountsForTests = \"\";\n" +
                "\t\t\tthis.DevEnvironment.AccountsForTests = \"" + NWDAccount.GetAccountsForConfig(NWDAccountEnvironment.Dev) + "\";\n" +
                "";
            }
            else if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {
                tConstantsFile += "" +
                "\t\t\tthis.ProdEnvironment.Selected = false;\n" +
                "\t\t\tthis.PreprodEnvironment.Selected = true;\n" +
                "\t\t\tthis.DevEnvironment.Selected = false;\n" +
                "\t\t\t// For tests\n" +
                "\t\t\tthis.ProdEnvironment.AccountsForTests = \"\";\n" +
                "\t\t\tthis.PreprodEnvironment.AccountsForTests = \"" + NWDAccount.GetAccountsForConfig(NWDAccountEnvironment.Preprod) + "\";\n" +
                "\t\t\tthis.DevEnvironment.AccountsForTests = \"\";\n" +
                "";
            }
            else if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                tConstantsFile += "" +
                "\t\t\tthis.ProdEnvironment.Selected = true;\n" +
                "\t\t\tthis.PreprodEnvironment.Selected = false;\n" +
                "\t\t\tthis.DevEnvironment.Selected = false;\n" +
                "\t\t\t// For tests\n" +
                "\t\t\tthis.ProdEnvironment.AccountsForTests = \"\";\n" +
                "\t\t\tthis.PreprodEnvironment.AccountsForTests = \"\";\n" +
                "\t\t\tthis.DevEnvironment.AccountsForTests = \"\";\n" +
                "";
            }

            tConstantsFile += "" +
            "\t\t\t// Prod environment\n" +
            "\t\t\tthis.ProdEnvironment.Environment = \"" + this.ProdEnvironment.Environment.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.DataSHAPassword = \"" + this.ProdEnvironment.DataSHAPassword.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.DataSHAVector = \"" + this.ProdEnvironment.DataSHAVector.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.SaltStart = \"" + this.ProdEnvironment.SaltStart.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.SaltEnd = \"" + this.ProdEnvironment.SaltEnd.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.SaltFrequency = " + this.ProdEnvironment.SaltFrequency.ToString() + ";\n" +
            "\t\t\tthis.ProdEnvironment.ServerHTTPS = \"" + this.ProdEnvironment.ServerHTTPS.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\t#if UNITY_EDITOR\n" +
            "\t\t\tthis.ProdEnvironment.SaltServer = \"" + this.ProdEnvironment.SaltServer.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.ServerHost = \"" + this.ProdEnvironment.ServerHost.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.ServerUser = \"" + this.ProdEnvironment.ServerUser.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.ServerPassword = \"" + this.ProdEnvironment.ServerPassword.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.ServerBase = \"" + this.ProdEnvironment.ServerBase.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.AdminKey = \"" + this.ProdEnvironment.AdminKey.Replace("\"", "\\\"") + "\";\n" +
            //"\t\t\tthis.ProdEnvironment.RescueEmail = \"" + this.ProdEnvironment.RescueEmail.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.TokenHistoric = " + this.ProdEnvironment.TokenHistoric.ToString() + ";\n" +
            "\t\t\t#endif\n" +
            "\t\t\tthis.ProdEnvironment.FacebookAppID = \"" + this.ProdEnvironment.FacebookAppID.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.FacebookAppSecret = \"" + this.ProdEnvironment.FacebookAppSecret.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.GoogleAppKey = \"" + this.ProdEnvironment.GoogleAppKey.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.UnityAppKey = \"" + this.ProdEnvironment.UnityAppKey.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.TwitterAppKey = \"" + this.ProdEnvironment.TwitterAppKey.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.AppName = \"" + this.ProdEnvironment.AppName.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.SpeedOfGameTime = " + this.ProdEnvironment.SpeedOfGameTime.ToString() + "F;\n" +
            "\t\t\tthis.ProdEnvironment.BuildTimestamp = " + this.ProdEnvironment.BuildTimestamp.ToString() + ";\n" +
            //"\t\t\tthis.ProdEnvironment.Version = \"" + this.ProdEnvironment.Version.Replace ("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.LoadPreferences ();\n" +
            "\t\t\tthis.ProdEnvironment.FormatVerification ();\n" +
            "\t\t\t// Preprod environment\n" +
            "\t\t\tthis.PreprodEnvironment.Environment = \"" + this.PreprodEnvironment.Environment.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.DataSHAPassword = \"" + this.PreprodEnvironment.DataSHAPassword.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.DataSHAVector = \"" + this.PreprodEnvironment.DataSHAVector.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.SaltStart = \"" + this.PreprodEnvironment.SaltStart.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.SaltEnd = \"" + this.PreprodEnvironment.SaltEnd.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.SaltFrequency = " + this.PreprodEnvironment.SaltFrequency.ToString() + ";\n" +
            "\t\t\tthis.PreprodEnvironment.ServerHTTPS = \"" + this.PreprodEnvironment.ServerHTTPS.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\t#if UNITY_EDITOR\n" +
            "\t\t\tthis.PreprodEnvironment.SaltServer = \"" + this.PreprodEnvironment.SaltServer.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.ServerHost = \"" + this.PreprodEnvironment.ServerHost.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.ServerUser = \"" + this.PreprodEnvironment.ServerUser.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.ServerPassword = \"" + this.PreprodEnvironment.ServerPassword.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.ServerBase = \"" + this.PreprodEnvironment.ServerBase.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.AdminKey = \"" + this.PreprodEnvironment.AdminKey.Replace("\"", "\\\"") + "\";\n" +
            //"\t\t\tthis.PreprodEnvironment.RescueEmail = \"" + this.PreprodEnvironment.RescueEmail.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.TokenHistoric = " + this.PreprodEnvironment.TokenHistoric.ToString() + ";\n" +
            "\t\t\t#endif\n" +
            "\t\t\tthis.PreprodEnvironment.FacebookAppID = \"" + this.PreprodEnvironment.FacebookAppID.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.FacebookAppSecret = \"" + this.PreprodEnvironment.FacebookAppSecret.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.GoogleAppKey = \"" + this.PreprodEnvironment.GoogleAppKey.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.UnityAppKey = \"" + this.PreprodEnvironment.UnityAppKey.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.TwitterAppKey = \"" + this.PreprodEnvironment.TwitterAppKey.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.AppName = \"" + this.PreprodEnvironment.AppName.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.SpeedOfGameTime = " + this.PreprodEnvironment.SpeedOfGameTime.ToString() + "F;\n" +
            "\t\t\tthis.PreprodEnvironment.BuildTimestamp = " + this.PreprodEnvironment.BuildTimestamp.ToString() + ";\n" +
            //"\t\t\tthis.PreprodEnvironment.Version = \"" + this.PreprodEnvironment.Version.Replace ("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.LoadPreferences ();\n" +
            "\t\t\tthis.PreprodEnvironment.FormatVerification ();\n" +
            "#if UNITY_EDITOR\n" +
            "\t\t\t// Dev environment\n" +
            "\t\t\tthis.DevEnvironment.Environment = \"" + this.DevEnvironment.Environment.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.DataSHAPassword = \"" + this.DevEnvironment.DataSHAPassword.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.DataSHAVector = \"" + this.DevEnvironment.DataSHAVector.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.SaltStart = \"" + this.DevEnvironment.SaltStart.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.SaltEnd = \"" + this.DevEnvironment.SaltEnd.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.SaltServer = \"" + this.DevEnvironment.SaltServer.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.SaltFrequency = " + this.DevEnvironment.SaltFrequency.ToString() + ";\n" +
            "\t\t\tthis.DevEnvironment.ServerHTTPS = \"" + this.DevEnvironment.ServerHTTPS.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.ServerHost = \"" + this.DevEnvironment.ServerHost.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.ServerUser = \"" + this.DevEnvironment.ServerUser.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.ServerPassword = \"" + this.DevEnvironment.ServerPassword.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.ServerBase = \"" + this.DevEnvironment.ServerBase.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.FacebookAppID = \"" + this.DevEnvironment.FacebookAppID.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.FacebookAppSecret = \"" + this.DevEnvironment.FacebookAppSecret.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.GoogleAppKey = \"" + this.DevEnvironment.GoogleAppKey.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.UnityAppKey = \"" + this.DevEnvironment.UnityAppKey.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.TwitterAppKey = \"" + this.DevEnvironment.TwitterAppKey.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.AdminKey = \"" + this.DevEnvironment.AdminKey.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.AppName = \"" + this.DevEnvironment.AppName.Replace("\"", "\\\"") + "\";\n" +
            // "\t\t\tthis.DevEnvironment.RescueEmail = \"" + this.DevEnvironment.RescueEmail.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.TokenHistoric = " + this.DevEnvironment.TokenHistoric.ToString() + ";\n" +
            "\t\t\tthis.DevEnvironment.SpeedOfGameTime = " + this.DevEnvironment.SpeedOfGameTime.ToString() + "F;\n" +
            "\t\t\tthis.DevEnvironment.BuildTimestamp = " + this.DevEnvironment.BuildTimestamp.ToString() + ";\n" +
            //"\t\t\tthis.DevEnvironment.Version = \"" + this.DevEnvironment.Version.Replace ("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.LoadPreferences ();\n" +
            "\t\t\tthis.DevEnvironment.FormatVerification ();\n" +
            "#endif\n" +
            "\n" +
            "\t\t\t// Restaure languages \n" +
            "\t\t\tthis.DataLocalizationManager.LanguagesString = \"" + this.DataLocalizationManager.LanguagesString + "\";\n" +
            "\t\t\tRestaureStepTwo();\n" +
            "\t\t\tRestaureStepThree();\n" +
            "\t\t\tRestaureStepFour();\n" +
            "\t\t\tRestaureStepFive();\n" +
            "\t\t\tRestaureStepSix();\n" +
            "\t\t\tRestaureStepSeven();\n" +
            "\t\t}\n" +
            "\t//-------------------------------------------------------------------------------------------------------------\n";

            foreach (KeyValuePair<int, bool> tWS in WSList)
            {

            }

            tConstantsFile += "\t\t public void RestaureStepTwo() \n" +
                "\t\t\t{\n";
            foreach (KeyValuePair<int, Dictionary<string, string[]>> tKeyValue in kWebBuildkCSVAssemblyOrderArray)
            {
                if (WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (WSList[tKeyValue.Key] == true)
                    {
                        tConstantsFile += "\t\t\t kWebBuildkCSVAssemblyOrderArray.Add (" + tKeyValue.Key + ",new Dictionary<string, string[]>());\n";
                        foreach (KeyValuePair<string, string[]> tSubValue in tKeyValue.Value)
                        {
                            tConstantsFile += "\t\t\t kWebBuildkCSVAssemblyOrderArray[" + tKeyValue.Key + "].Add(\"" + tSubValue.Key + "\", new string[]{\"" + string.Join("\", \"", tSubValue.Value) + "\"});\n";
                        }
                    }
                }
            }
            tConstantsFile += "\t\t}\n" +
            "\t//-------------------------------------------------------------------------------------------------------------\n" +
            "\t\t public void RestaureStepThree() \n" +
            "\t\t\t{\n";

            foreach (KeyValuePair<int, Dictionary<string, string[]>> tKeyValue in kWebBuildkSLQAssemblyOrderArray)
            {
                if (WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (WSList[tKeyValue.Key] == true)
                    {
                        tConstantsFile += "\t\t\t kWebBuildkSLQAssemblyOrderArray.Add (" + tKeyValue.Key + ",new Dictionary<string, string[]>());\n";
                        foreach (KeyValuePair<string, string[]> tSubValue in tKeyValue.Value)
                        {
                            tConstantsFile += "\t\t\t kWebBuildkSLQAssemblyOrderArray[" + tKeyValue.Key + "].Add(\"" + tSubValue.Key + "\", new string[]{\"" + string.Join("\", \"", tSubValue.Value) + "\"});\n";
                        }
                    }
                }
            }
            tConstantsFile += "\t\t}\n" +
            "\t//-------------------------------------------------------------------------------------------------------------\n" +
            "\t\t public void RestaureStepFour() \n" +
            "\t\t\t{\n";

            foreach (KeyValuePair<int, Dictionary<string, string>> tKeyValue in kWebBuildkSLQAssemblyOrder)
            {
                if (WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (WSList[tKeyValue.Key] == true)
                    {
                        tConstantsFile += "\t\t\t kWebBuildkSLQAssemblyOrder.Add (" + tKeyValue.Key + ",new Dictionary<string, string>());\n";
                        foreach (KeyValuePair<string, string> tSubValue in tKeyValue.Value)
                        {
                            tConstantsFile += "\t\t\t kWebBuildkSLQAssemblyOrder[" + tKeyValue.Key + "].Add(\"" + tSubValue.Key + "\", \"" + tSubValue.Value + "\");\n";
                        }
                    }
                }
            }
            tConstantsFile += "\t\t}\n" +
            "\t//-------------------------------------------------------------------------------------------------------------\n" +
            "\t\t public void RestaureStepFive() \n" +
            "\t\t\t{\n";
            foreach (KeyValuePair<int, Dictionary<string, List<string>>> tKeyValue in kWebBuildkSLQIntegrityOrder)
            {
                if (WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (WSList[tKeyValue.Key] == true)
                    {
                        tConstantsFile += "\t\t\t kWebBuildkSLQIntegrityOrder.Add (" + tKeyValue.Key + ",new Dictionary<string, List<string>>());\n";
                        foreach (KeyValuePair<string, List<string>> tSubValue in tKeyValue.Value)
                        {
                            tConstantsFile += "\t\t\t kWebBuildkSLQIntegrityOrder[" + tKeyValue.Key + "].Add(\"" + tSubValue.Key + "\", new List<string>(){\"" + string.Join("\", \"", tSubValue.Value.ToArray()) + "\"});\n";
                        }
                    }
                }
            }
            tConstantsFile += "\t\t}\n" +
            "\t//-------------------------------------------------------------------------------------------------------------\n" +
            "\t\t public void RestaureStepSix() \n" +
            "\t\t\t{\n" +
            "\t\t\t\t#if UNITY_EDITOR\n" +
            "";
            foreach (KeyValuePair<int, Dictionary<string, List<string>>> tKeyValue in kWebBuildkSLQIntegrityServerOrder)
            {
                if (WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (WSList[tKeyValue.Key] == true)
                    {
                        tConstantsFile += "\t\t\t kWebBuildkSLQIntegrityServerOrder.Add (" + tKeyValue.Key + ",new Dictionary<string, List<string>>());\n";
                        foreach (KeyValuePair<string, List<string>> tSubValue in tKeyValue.Value)
                        {
                            tConstantsFile += "\t\t\t kWebBuildkSLQIntegrityServerOrder[" + tKeyValue.Key + "].Add(\"" + tSubValue.Key + "\", new List<string>(){\"" + string.Join("\", \"", tSubValue.Value.ToArray()) + "\"});\n";
                        }
                    }
                }
            }
            tConstantsFile += "" +
            "\t\t\t\t#endif\n" +
            "\t\t}\n" +
            "\t//-------------------------------------------------------------------------------------------------------------\n" +
            "\t\t public void RestaureStepSeven() \n" +
            "\t\t\t{\n";
            foreach (KeyValuePair<int, Dictionary<string, List<string>>> tKeyValue in kWebBuildkDataAssemblyPropertiesList)
            {
                if (WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (WSList[tKeyValue.Key] == true)
                    {
                        tConstantsFile += "\t\t\t kWebBuildkDataAssemblyPropertiesList.Add (" + tKeyValue.Key + ",new Dictionary<string, List<string>>());\n";
                        foreach (KeyValuePair<string, List<string>> tSubValue in tKeyValue.Value)
                        {
                            tConstantsFile += "\t\t\t kWebBuildkDataAssemblyPropertiesList[" + tKeyValue.Key + "].Add(\"" + tSubValue.Key + "\", new List<string>(){\"" + string.Join("\", \"", tSubValue.Value.ToArray()) + "\"});\n";
                        }
                    }
                }
            }
            tConstantsFile += "\t\t}\n" +
            "\t//-------------------------------------------------------------------------------------------------------------\n";
            tConstantsFile += "\t}\n" +
            "//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\n" +
            "}\n" +
            "//=====================================================================================================================\n";


            // File.WriteAllText(tEngineRootFolder + "/NWDConfigurations.cs", tConstantsFile);
            // force to import this file by Unity3D
            // AssetDatabase.ImportAsset (tEngineRootFolder + "/NWDConfigurations.cs");

            //string tPath = NWDFindPackage.PathOfPackage("/NWDConfigurations.cs");
            string tPath = "Assets/NWDConfigurations.cs";
            File.WriteAllText(tPath, tConstantsFile);
            // force to import this file by Unity3D
            AssetDatabase.ImportAsset(tPath);
            AssetDatabase.Refresh();
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================