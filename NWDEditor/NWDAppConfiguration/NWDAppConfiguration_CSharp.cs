//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using BasicToolBox;
#if UNITY_EDITOR
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAppConfiguration
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Generates the C sharp file for all environments' restauration.
        /// </summary>
        /// <param name="sEnvironment">S environment.</param>
        public void GenerateCSharpFile(NWDAppEnvironment sEnvironment)
        {
            BTBBenchmark.Start();
            DateTime tTime = DateTime.UtcNow;
            string tDateTimeString = NWDToolbox.DateTimeYYYYMMdd(tTime);
            string tYearString = NWDToolbox.DateTimeYYYY(tTime);
            string tConstantsFile = string.Empty;
            tConstantsFile += string.Empty +
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
            "\t\tpublic override bool RestaureConfigurations ()\n" +
            "\t\t{\n" +
            "\t\t\tWebFolder = \"" + WebFolder + "\";\n" +
            "\t\t\tWebBuild = " + WebBuild + ";\n" +
            "\t\t\tDatabasePrefix = \"" + DatabasePrefix + "\";\n" +
            "\t\t\tEditorPass = \"" + EditorPass + "\";\n" +
            "\t\t\tEditorPassA = \"" + EditorPassA + "\";\n" +
            "\t\t\tEditorPassB = \"" + EditorPassB + "\";\n" +
            "\t\t\tAccountHashSalt = \"" + AccountHashSalt + "\";\n" +
            "\t\t\tAccountHashSaltA = \"" + AccountHashSaltA + "\";\n" +
            "\t\t\tAccountHashSaltB = \"" + AccountHashSaltB + "\";\n" +
            "\t\t\tRowDataIntegrity = " + RowDataIntegrity.ToString().ToLower() + ";\n" +
            "\t\t\tPreloadDatas = " + PreloadDatas.ToString().ToLower() + ";\n" +
            "\t\t\t//Debug.Log(\"NWDAppConfiguration Restaure Config\");\n" +
            "\t\t\t//Salts regenerate (Calgon© is back :-p )\n";
            foreach (KeyValuePair<string, string> tEntry in IntegritySaltDictionary.OrderBy(x => x.Key))
            {
                tConstantsFile += "\t\t\tIntegritySaltDictionary[\"" + tEntry.Key + "\"]=\"" + tEntry.Value.Replace("\"", "\\\"") + "\";\n";
            }
            tConstantsFile += "//Salts Validity\n";
            foreach (KeyValuePair<string, string> tEntry in GenerateSaltDictionary.OrderBy(x => x.Key))
            {
                tConstantsFile += "\t\t\tGenerateSaltDictionary[\"" + tEntry.Key + "\"]=\"" + tEntry.Value.Replace("\"", "\\\"") + "\";\n";
            }
            tConstantsFile += "\t\t\t//Language regenerate (MultiPass?!)\n";
            tConstantsFile += "\t\t\tProjetcLanguage = \"" + ProjetcLanguage + "\";\n";
            foreach (KeyValuePair<string, string> tEntry in BundleName.OrderBy(x => x.Key))
            {
                tConstantsFile += "\t\t\tBundleName[\"" + tEntry.Key + "\"]=\"" + tEntry.Value.Replace("\"", "\\\"") + "\";\n";
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
            foreach (KeyValuePair<int, bool> tWS in WSList.OrderBy(x => x.Key))
            {
                if (tWS.Value == true)
                {
                    tConstantsFile += "\t\t\t WSList.Add(" + tWS.Key + "," + tWS.Value.ToString().ToLower() + ");\n";
                }
                else
                {
                    string tWebServiceFolder = NWDAppConfiguration.SharedInstance().OldWebServiceFolder(tWS.Key);
                    string tOwnerServerFolderPath = NWDToolbox.FindOwnerServerFolder();
                    if (AssetDatabase.IsValidFolder(tOwnerServerFolderPath + "/" + tWebServiceFolder) == true)
                    {
                        AssetDatabase.DeleteAsset(tOwnerServerFolderPath + "/" + tWebServiceFolder);
                    }
                }
            }
            tConstantsFile += "\t\t\t TagList = new Dictionary<int, string>();\n";
            for (int tI = -1; tI <= NWDAppConfiguration.SharedInstance().TagNumberUser; tI++)
            {
                if (TagList.ContainsKey(tI) == true)
                {
                    tConstantsFile += "\t\t\t TagList.Add(" + tI + ",\"" + TagList[tI] + "\");\n";
                }
            }
            int tTag = TagNumberUser + 1;
            tConstantsFile += "\t\t\t TagList.Add(" + (tTag++) + ",\"Internal Created\");\n"; // 11
            tConstantsFile += "\t\t\t TagList.Add(" + (tTag++) + ",\"(Reserved)\");\n"; // 12
            tConstantsFile += "\t\t\t TagList.Add(" + (tTag++) + ",\"(Reserved)\");\n"; // 13
            tConstantsFile += "\t\t\t TagList.Add(" + (tTag++) + ",\"(Reserved)\");\n"; // 14
            tConstantsFile += "\t\t\t TagList.Add(" + (tTag++) + ",\"Test for Preprod\");\n"; // 15 
            tConstantsFile += "\t\t\t TagList.Add(" + (tTag++) + ",\"Test for Dev\");\n"; // 16
            tConstantsFile += "\t\t\t TagList.Add(" + (tTag++) + ",\"Admin Created\");\n"; // 17
            tConstantsFile += "\t\t\t TagList.Add(" + (tTag++) + ",\"Device Created\");\n"; // 18
            tConstantsFile += "\t\t\t TagList.Add(" + (tTag++) + ",\"Server Created\");\n"; // 19
            tConstantsFile += "\t\t\t TagList.Add(" + (tTag++) + ",\"User Created\");\n"; // 20
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
            // PRODUCTION
            "\t\t\t// Prod environment\n" +
            "\t\t\tthis.ProdEnvironment.Environment = \"" + this.ProdEnvironment.Environment.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.DataSHAPassword = \"" + this.ProdEnvironment.DataSHAPassword.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.DataSHAVector = \"" + this.ProdEnvironment.DataSHAVector.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.SaltStart = \"" + this.ProdEnvironment.SaltStart.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.SaltEnd = \"" + this.ProdEnvironment.SaltEnd.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.WebTimeOut = " + this.ProdEnvironment.WebTimeOut.ToString() + ";\n" +
            "\t\t\tthis.ProdEnvironment.EditorWebTimeOut = " + this.ProdEnvironment.EditorWebTimeOut.ToString() + ";\n" +
            "\t\t\tthis.ProdEnvironment.SaltFrequency = " + this.ProdEnvironment.SaltFrequency.ToString() + ";\n" +
            "\t\t\tthis.ProdEnvironment.AddressPing = \"" + this.ProdEnvironment.AddressPing.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.ServerHTTPS = \"" + this.ProdEnvironment.ServerHTTPS.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.AllwaysSecureData = " + this.ProdEnvironment.AllwaysSecureData.ToString().ToLower() + ";\n" +
            "\t\t\tthis.ProdEnvironment.BuildDate = \"" + this.ProdEnvironment.BuildDate.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.FacebookAppID = \"" + this.ProdEnvironment.FacebookAppID.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.FacebookAppSecret = \"" + this.ProdEnvironment.FacebookAppSecret.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.GoogleAppKey = \"" + this.ProdEnvironment.GoogleAppKey.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.UnityAppKey = \"" + this.ProdEnvironment.UnityAppKey.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.TwitterAppKey = \"" + this.ProdEnvironment.TwitterAppKey.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.AppName = \"" + this.ProdEnvironment.AppName.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.PreProdTimeFormat = \"" + this.ProdEnvironment.PreProdTimeFormat.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.AppProtocol = \"" + this.ProdEnvironment.AppProtocol.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.SpeedOfGameTime = " + this.ProdEnvironment.SpeedOfGameTime.ToString() + "F;\n" +
            "\t\t\tthis.ProdEnvironment.BuildTimestamp = " + this.ProdEnvironment.BuildTimestamp.ToString() + ";\n" +
            "\t\t\tthis.ProdEnvironment.ThreadPoolForce = " + this.ProdEnvironment.ThreadPoolForce.ToString().ToLower() + ";\n" +
            "\t\t\tthis.ProdEnvironment.WritingModeLocal = NWDWritingMode." + this.ProdEnvironment.WritingModeLocal.ToString() + ";\n" +
            "\t\t\tthis.ProdEnvironment.WritingModeWebService = NWDWritingMode." + this.ProdEnvironment.WritingModeWebService.ToString() + ";\n" +
            "\t\t\tthis.ProdEnvironment.WritingModeEditor = NWDWritingMode." + this.ProdEnvironment.WritingModeEditor.ToString() + ";\n" +
            "\t\t\tthis.ProdEnvironment.CartridgeColor = new Color(" + NWDToolbox.FloatToString(this.ProdEnvironment.CartridgeColor.r) + "F," +
                                                                NWDToolbox.FloatToString(this.ProdEnvironment.CartridgeColor.g) + "F," +
                                                                NWDToolbox.FloatToString(this.ProdEnvironment.CartridgeColor.b) + "F," +
                                                                NWDToolbox.FloatToString(this.ProdEnvironment.CartridgeColor.a) + "F);\n" +
            "\t\t\t#if UNITY_EDITOR\n" +
            "\t\t\tthis.ProdEnvironment.LogMode = " + this.ProdEnvironment.LogMode.ToString().ToLower() + ";\n" +
            "\t\t\tthis.ProdEnvironment.SFTPHost = \"" + this.ProdEnvironment.SFTPHost.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.SFTPPort = " + this.ProdEnvironment.SFTPPort.ToString() + ";\n" +
            "\t\t\tthis.ProdEnvironment.SFTPFolder = \"" + this.ProdEnvironment.SFTPFolder.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.SFTPUser = \"" + this.ProdEnvironment.SFTPUser.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.SFTPPassword = \"" + this.ProdEnvironment.SFTPPassword.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.SaltServer = \"" + this.ProdEnvironment.SaltServer.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.ServerHost = \"" + this.ProdEnvironment.ServerHost.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.ServerUser = \"" + this.ProdEnvironment.ServerUser.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.ServerPassword = \"" + this.ProdEnvironment.ServerPassword.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.ServerBase = \"" + this.ProdEnvironment.ServerBase.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.AdminKey = \"" + this.ProdEnvironment.AdminKey.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.RescueEmail = \"" + this.ProdEnvironment.RescueEmail.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.ProdEnvironment.TokenHistoric = " + this.ProdEnvironment.TokenHistoric.ToString() + ";\n" +
            "\t\t\tthis.ProdEnvironment.MailHost =  \"" + this.ProdEnvironment.MailHost + " \";\n" +
            "\t\t\tthis.ProdEnvironment.MailPort = " + this.ProdEnvironment.MailPort.ToString() + ";\n" +
            "\t\t\tthis.ProdEnvironment.MailUserName =  \"" + this.ProdEnvironment.MailUserName + " \";\n" +
            "\t\t\tthis.ProdEnvironment.MailPassword =  \"" + this.ProdEnvironment.MailPassword + " \";\n" +
            "\t\t\tthis.ProdEnvironment.MailDomain =  \"" + this.ProdEnvironment.MailDomain + " \";\n" +
            "\t\t\tthis.ProdEnvironment.MailAuthentication =  \"" + this.ProdEnvironment.MailAuthentication + " \";\n" +
            "\t\t\tthis.ProdEnvironment.MailEnableStarttlsAuto =  \"" + this.ProdEnvironment.MailEnableStarttlsAuto + " \";\n" +
            "\t\t\tthis.ProdEnvironment.MailOpenSSLVerifyMode =  \"" + this.ProdEnvironment.MailOpenSSLVerifyMode + " \";\n" +
            "\t\t\tthis.ProdEnvironment.MailOpenSSLVerifyMode =  \"" + this.ProdEnvironment.MailOpenSSLVerifyMode + " \";\n" +
            "\t\t\tthis.ProdEnvironment.MailFrom =  \"" + this.ProdEnvironment.MailFrom + " \";\n" +
            "\t\t\tthis.ProdEnvironment.MailReplyTo =  \"" + this.ProdEnvironment.MailReplyTo + " \";\n" +
            "\t\t\t#endif\n" +
            "\t\t\tthis.ProdEnvironment.LoadPreferences ();\n" +
            "\t\t\tthis.ProdEnvironment.FormatVerification ();\n" +
            // PREPRODUCTION
            "\t\t\t// Preprod environment\n" +
            "\t\t\tthis.PreprodEnvironment.Environment = \"" + this.PreprodEnvironment.Environment.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.DataSHAPassword = \"" + this.PreprodEnvironment.DataSHAPassword.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.DataSHAVector = \"" + this.PreprodEnvironment.DataSHAVector.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.SaltStart = \"" + this.PreprodEnvironment.SaltStart.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.SaltEnd = \"" + this.PreprodEnvironment.SaltEnd.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.WebTimeOut = " + this.PreprodEnvironment.WebTimeOut.ToString() + ";\n" +
            "\t\t\tthis.PreprodEnvironment.EditorWebTimeOut = " + this.PreprodEnvironment.EditorWebTimeOut.ToString() + ";\n" +
            "\t\t\tthis.PreprodEnvironment.SaltFrequency = " + this.PreprodEnvironment.SaltFrequency.ToString() + ";\n" +
            "\t\t\tthis.PreprodEnvironment.AddressPing = \"" + this.PreprodEnvironment.AddressPing.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.ServerHTTPS = \"" + this.PreprodEnvironment.ServerHTTPS.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.AllwaysSecureData = " + this.PreprodEnvironment.AllwaysSecureData.ToString().ToLower() + ";\n" +
            "\t\t\tthis.PreprodEnvironment.BuildDate = \"" + this.PreprodEnvironment.BuildDate.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.FacebookAppID = \"" + this.PreprodEnvironment.FacebookAppID.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.FacebookAppSecret = \"" + this.PreprodEnvironment.FacebookAppSecret.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.GoogleAppKey = \"" + this.PreprodEnvironment.GoogleAppKey.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.UnityAppKey = \"" + this.PreprodEnvironment.UnityAppKey.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.TwitterAppKey = \"" + this.PreprodEnvironment.TwitterAppKey.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.AppName = \"" + this.PreprodEnvironment.AppName.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.PreProdTimeFormat = \"" + this.PreprodEnvironment.PreProdTimeFormat.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.AppProtocol = \"" + this.PreprodEnvironment.AppProtocol.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.SpeedOfGameTime = " + this.PreprodEnvironment.SpeedOfGameTime.ToString() + "F;\n" +
            "\t\t\tthis.PreprodEnvironment.BuildTimestamp = " + this.PreprodEnvironment.BuildTimestamp.ToString() + ";\n" +
            "\t\t\tthis.PreprodEnvironment.ThreadPoolForce = " + this.PreprodEnvironment.ThreadPoolForce.ToString().ToLower() + ";\n" +
            "\t\t\tthis.PreprodEnvironment.WritingModeLocal = NWDWritingMode." + this.PreprodEnvironment.WritingModeLocal.ToString() + ";\n" +
            "\t\t\tthis.PreprodEnvironment.WritingModeWebService = NWDWritingMode." + this.PreprodEnvironment.WritingModeWebService.ToString() + ";\n" +
            "\t\t\tthis.PreprodEnvironment.WritingModeEditor = NWDWritingMode." + this.PreprodEnvironment.WritingModeEditor.ToString() + ";\n" +
            "\t\t\tthis.PreprodEnvironment.CartridgeColor = new Color(" + NWDToolbox.FloatToString(this.PreprodEnvironment.CartridgeColor.r) + "F," +
                                                                NWDToolbox.FloatToString(this.PreprodEnvironment.CartridgeColor.g) + "F," +
                                                                NWDToolbox.FloatToString(this.PreprodEnvironment.CartridgeColor.b) + "F," +
                                                                NWDToolbox.FloatToString(this.PreprodEnvironment.CartridgeColor.a) + "F);\n" +
            "\t\t\t#if UNITY_EDITOR\n" +
            "\t\t\tthis.PreprodEnvironment.LogMode = " + this.PreprodEnvironment.LogMode.ToString().ToLower() + ";\n" +
            "\t\t\tthis.PreprodEnvironment.SFTPHost = \"" + this.PreprodEnvironment.SFTPHost.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.SFTPPort = " + this.PreprodEnvironment.SFTPPort.ToString() + ";\n" +
            "\t\t\tthis.PreprodEnvironment.SFTPFolder = \"" + this.PreprodEnvironment.SFTPFolder.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.SFTPUser = \"" + this.PreprodEnvironment.SFTPUser.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.SFTPPassword = \"" + this.PreprodEnvironment.SFTPPassword.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.SaltServer = \"" + this.PreprodEnvironment.SaltServer.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.ServerHost = \"" + this.PreprodEnvironment.ServerHost.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.ServerUser = \"" + this.PreprodEnvironment.ServerUser.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.ServerPassword = \"" + this.PreprodEnvironment.ServerPassword.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.ServerBase = \"" + this.PreprodEnvironment.ServerBase.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.AdminKey = \"" + this.PreprodEnvironment.AdminKey.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.RescueEmail = \"" + this.PreprodEnvironment.RescueEmail.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.PreprodEnvironment.TokenHistoric = " + this.PreprodEnvironment.TokenHistoric.ToString() + ";\n" +
            "\t\t\tthis.PreprodEnvironment.MailHost =  \"" + this.PreprodEnvironment.MailHost + " \";\n" +
            "\t\t\tthis.PreprodEnvironment.MailPort = " + this.PreprodEnvironment.MailPort.ToString() + ";\n" +
            "\t\t\tthis.PreprodEnvironment.MailUserName =  \"" + this.PreprodEnvironment.MailUserName + " \";\n" +
            "\t\t\tthis.PreprodEnvironment.MailPassword =  \"" + this.PreprodEnvironment.MailPassword + " \";\n" +
            "\t\t\tthis.PreprodEnvironment.MailDomain =  \"" + this.PreprodEnvironment.MailDomain + " \";\n" +
            "\t\t\tthis.PreprodEnvironment.MailAuthentication =  \"" + this.PreprodEnvironment.MailAuthentication + " \";\n" +
            "\t\t\tthis.PreprodEnvironment.MailEnableStarttlsAuto =  \"" + this.PreprodEnvironment.MailEnableStarttlsAuto + " \";\n" +
            "\t\t\tthis.PreprodEnvironment.MailOpenSSLVerifyMode =  \"" + this.PreprodEnvironment.MailOpenSSLVerifyMode + " \";\n" +
            "\t\t\tthis.PreprodEnvironment.MailFrom =  \"" + this.PreprodEnvironment.MailFrom + " \";\n" +
            "\t\t\tthis.PreprodEnvironment.MailReplyTo =  \"" + this.PreprodEnvironment.MailReplyTo + " \";\n" +
            "\t\t\t#endif\n" +
            "\t\t\tthis.PreprodEnvironment.LoadPreferences ();\n" +
            "\t\t\tthis.PreprodEnvironment.FormatVerification ();\n" +
            // DEVELOPMENT
            "\t\t\t// Dev environment\n" +
            "\t\t\tthis.DevEnvironment.Environment = \"" + this.DevEnvironment.Environment.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.DataSHAPassword = \"" + this.DevEnvironment.DataSHAPassword.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.DataSHAVector = \"" + this.DevEnvironment.DataSHAVector.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.SaltStart = \"" + this.DevEnvironment.SaltStart.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.SaltEnd = \"" + this.DevEnvironment.SaltEnd.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.WebTimeOut = " + this.DevEnvironment.WebTimeOut.ToString() + ";\n" +
            "\t\t\tthis.DevEnvironment.EditorWebTimeOut = " + this.DevEnvironment.EditorWebTimeOut.ToString() + ";\n" +
            "\t\t\tthis.DevEnvironment.SaltFrequency = " + this.DevEnvironment.SaltFrequency.ToString() + ";\n" +
            "\t\t\tthis.DevEnvironment.AddressPing = \"" + this.DevEnvironment.AddressPing.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.ServerHTTPS = \"" + this.DevEnvironment.ServerHTTPS.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.AllwaysSecureData = " + this.DevEnvironment.AllwaysSecureData.ToString().ToLower() + ";\n" +
            "\t\t\tthis.DevEnvironment.BuildDate = \"" + this.DevEnvironment.BuildDate.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.FacebookAppID = \"" + this.DevEnvironment.FacebookAppID.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.FacebookAppSecret = \"" + this.DevEnvironment.FacebookAppSecret.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.GoogleAppKey = \"" + this.DevEnvironment.GoogleAppKey.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.UnityAppKey = \"" + this.DevEnvironment.UnityAppKey.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.TwitterAppKey = \"" + this.DevEnvironment.TwitterAppKey.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.AppName = \"" + this.DevEnvironment.AppName.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.PreProdTimeFormat = \"" + this.DevEnvironment.PreProdTimeFormat.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.AppProtocol = \"" + this.DevEnvironment.AppProtocol.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.SpeedOfGameTime = " + this.DevEnvironment.SpeedOfGameTime.ToString() + "F;\n" +
            "\t\t\tthis.DevEnvironment.BuildTimestamp = " + this.DevEnvironment.BuildTimestamp.ToString() + ";\n" +
            "\t\t\tthis.DevEnvironment.ThreadPoolForce = " + this.DevEnvironment.ThreadPoolForce.ToString().ToLower() + ";\n" +
            "\t\t\tthis.DevEnvironment.WritingModeLocal = NWDWritingMode." + this.DevEnvironment.WritingModeLocal.ToString() + ";\n" +
            "\t\t\tthis.DevEnvironment.WritingModeWebService = NWDWritingMode." + this.DevEnvironment.WritingModeWebService.ToString() + ";\n" +
            "\t\t\tthis.DevEnvironment.WritingModeEditor = NWDWritingMode." + this.DevEnvironment.WritingModeEditor.ToString() + ";\n" +
            "\t\t\tthis.DevEnvironment.CartridgeColor = new Color(" + NWDToolbox.FloatToString(this.DevEnvironment.CartridgeColor.r) + "F," +
                                                                NWDToolbox.FloatToString(this.DevEnvironment.CartridgeColor.g) + "F," +
                                                                NWDToolbox.FloatToString(this.DevEnvironment.CartridgeColor.b) + "F," +
                                                                NWDToolbox.FloatToString(this.DevEnvironment.CartridgeColor.a) + "F);\n" +
            "#if UNITY_EDITOR\n" +
            "\t\t\tthis.DevEnvironment.LogMode = " + this.DevEnvironment.LogMode.ToString().ToLower() + ";\n" +
            "\t\t\tthis.DevEnvironment.SFTPHost = \"" + this.DevEnvironment.SFTPHost.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.SFTPPort = " + this.DevEnvironment.SFTPPort.ToString() + ";\n" +
            "\t\t\tthis.DevEnvironment.SFTPFolder = \"" + this.DevEnvironment.SFTPFolder.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.SFTPUser = \"" + this.DevEnvironment.SFTPUser.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.SFTPPassword = \"" + this.DevEnvironment.SFTPPassword.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.SaltServer = \"" + this.DevEnvironment.SaltServer.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.ServerHost = \"" + this.DevEnvironment.ServerHost.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.ServerUser = \"" + this.DevEnvironment.ServerUser.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.ServerPassword = \"" + this.DevEnvironment.ServerPassword.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.ServerBase = \"" + this.DevEnvironment.ServerBase.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.AdminKey = \"" + this.DevEnvironment.AdminKey.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.RescueEmail = \"" + this.DevEnvironment.RescueEmail.Replace("\"", "\\\"") + "\";\n" +
            "\t\t\tthis.DevEnvironment.TokenHistoric = " + this.DevEnvironment.TokenHistoric.ToString() + ";\n" +
            "\t\t\tthis.DevEnvironment.MailHost =  \"" + this.DevEnvironment.MailHost + " \";\n" +
            "\t\t\tthis.DevEnvironment.MailPort = " + this.DevEnvironment.MailPort.ToString() + ";\n" +
            "\t\t\tthis.DevEnvironment.MailUserName =  \"" + this.DevEnvironment.MailUserName + " \";\n" +
            "\t\t\tthis.DevEnvironment.MailPassword =  \"" + this.DevEnvironment.MailPassword + " \";\n" +
            "\t\t\tthis.DevEnvironment.MailDomain =  \"" + this.DevEnvironment.MailDomain + " \";\n" +
            "\t\t\tthis.DevEnvironment.MailAuthentication =  \"" + this.DevEnvironment.MailAuthentication + " \";\n" +
            "\t\t\tthis.DevEnvironment.MailEnableStarttlsAuto =  \"" + this.DevEnvironment.MailEnableStarttlsAuto + " \";\n" +
            "\t\t\tthis.DevEnvironment.MailOpenSSLVerifyMode =  \"" + this.DevEnvironment.MailOpenSSLVerifyMode + " \";\n" +
            "\t\t\tthis.DevEnvironment.MailFrom =  \"" + this.DevEnvironment.MailFrom + " \";\n" +
            "\t\t\tthis.DevEnvironment.MailReplyTo =  \"" + this.DevEnvironment.MailReplyTo + " \";\n" +
            "#endif\n" +
            "\t\t\tthis.DevEnvironment.LoadPreferences ();\n" +
            "\t\t\tthis.DevEnvironment.FormatVerification ();\n" +
            "\n" +
            "\t\t\t// Restaure languages \n" +
            "\t\t\tthis.DataLocalizationManager.LanguagesString = \"" + this.DataLocalizationManager.LanguagesString + "\";\n" +
            "\t\t\tRestaureStepTwo();\n" +
            "\t\t\tRestaureStepThree();\n" +
            "\t\t\tRestaureStepFour();\n" +
            "\t\t\tRestaureStepFive();\n" +
            "\t\t\tRestaureStepSix();\n" +
            "\t\t\tRestaureStepSeven();\n" +
            "\t\t\tRestaureStepHeight();\n" +
            "\t\t\treturn true;\n" +
            "\t\t}\n" +
            "\t//-------------------------------------------------------------------------------------------------------------\n";
            tConstantsFile += "\t\t public void RestaureStepTwo() \n" +
            "\t\t{\n";
            foreach (KeyValuePair<int, Dictionary<string, string[]>> tKeyValue in kWebBuildkCSVAssemblyOrderArray.OrderBy(x => x.Key))
            {
                if (WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (WSList[tKeyValue.Key] == true)
                    {
                        tConstantsFile += "\t\t\t kWebBuildkCSVAssemblyOrderArray.Add (" + tKeyValue.Key + ",new Dictionary<string, string[]>());\n";
                        foreach (KeyValuePair<string, string[]> tSubValue in tKeyValue.Value.OrderBy(x => x.Key))
                        {
                            tConstantsFile += "\t\t\t kWebBuildkCSVAssemblyOrderArray[" + tKeyValue.Key + "].Add(\"" + tSubValue.Key + "\", new string[]{\"" + string.Join("\", \"", tSubValue.Value) + "\"});\n";
                        }
                    }
                }
            }
            tConstantsFile += "\t\t}\n" +
            "\t//-------------------------------------------------------------------------------------------------------------\n" +
            "\t\t public void RestaureStepThree() \n" +
            "\t\t{\n";
            foreach (KeyValuePair<int, Dictionary<string, string[]>> tKeyValue in kWebBuildkSLQAssemblyOrderArray.OrderBy(x => x.Key))
            {
                if (WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (WSList[tKeyValue.Key] == true)
                    {
                        tConstantsFile += "\t\t\t kWebBuildkSLQAssemblyOrderArray.Add (" + tKeyValue.Key + ",new Dictionary<string, string[]>());\n";
                        foreach (KeyValuePair<string, string[]> tSubValue in tKeyValue.Value.OrderBy(x => x.Key))
                        {
                            tConstantsFile += "\t\t\t kWebBuildkSLQAssemblyOrderArray[" + tKeyValue.Key + "].Add(\"" + tSubValue.Key + "\", new string[]{\"" + string.Join("\", \"", tSubValue.Value) + "\"});\n";
                        }
                    }
                }
            }
            tConstantsFile += "\t\t}\n" +
            "\t//-------------------------------------------------------------------------------------------------------------\n" +
            "\t\t public void RestaureStepFour() \n" +
            "\t\t{\n";
            foreach (KeyValuePair<int, Dictionary<string, string>> tKeyValue in kWebBuildkSLQAssemblyOrder.OrderBy(x => x.Key))
            {
                if (WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (WSList[tKeyValue.Key] == true)
                    {
                        tConstantsFile += "\t\t\t kWebBuildkSLQAssemblyOrder.Add (" + tKeyValue.Key + ",new Dictionary<string, string>());\n";
                        foreach (KeyValuePair<string, string> tSubValue in tKeyValue.Value.OrderBy(x => x.Key))
                        {
                            tConstantsFile += "\t\t\t kWebBuildkSLQAssemblyOrder[" + tKeyValue.Key + "].Add(\"" + tSubValue.Key + "\", \"" + tSubValue.Value.Replace("\"", "\\\"") + "\");\n";
                        }
                    }
                }
            }
            tConstantsFile += "\t\t}\n" +
            "\t//-------------------------------------------------------------------------------------------------------------\n" +
            "\t\t public void RestaureStepFive() \n" +
            "\t\t{\n";
            foreach (KeyValuePair<int, Dictionary<string, List<string>>> tKeyValue in kWebBuildkSLQIntegrityOrder.OrderBy(x => x.Key))
            {
                if (WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (WSList[tKeyValue.Key] == true)
                    {
                        tConstantsFile += "\t\t\t kWebBuildkSLQIntegrityOrder.Add (" + tKeyValue.Key + ",new Dictionary<string, List<string>>());\n";
                        foreach (KeyValuePair<string, List<string>> tSubValue in tKeyValue.Value.OrderBy(x => x.Key))
                        {
                            tConstantsFile += "\t\t\t kWebBuildkSLQIntegrityOrder[" + tKeyValue.Key + "].Add(\"" + tSubValue.Key + "\", new List<string>(){\"" + string.Join("\", \"", tSubValue.Value.ToArray()) + "\"});\n";
                        }
                    }
                }
            }
            tConstantsFile += "\t\t}\n" +
            "\t//-------------------------------------------------------------------------------------------------------------\n" +
            "\t\t public void RestaureStepSix() \n" +
            "\t\t{\n" +
            "\t\t\t\t#if UNITY_EDITOR\n" +
            "";
            foreach (KeyValuePair<int, Dictionary<string, List<string>>> tKeyValue in kWebBuildkSLQIntegrityServerOrder.OrderBy(x => x.Key))
            {
                if (WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (WSList[tKeyValue.Key] == true)
                    {
                        tConstantsFile += "\t\t\t kWebBuildkSLQIntegrityServerOrder.Add (" + tKeyValue.Key + ",new Dictionary<string, List<string>>());\n";
                        foreach (KeyValuePair<string, List<string>> tSubValue in tKeyValue.Value.OrderBy(x => x.Key))
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
            "\t\t{\n";
            foreach (KeyValuePair<int, Dictionary<string, List<string>>> tKeyValue in kWebBuildkDataAssemblyPropertiesList.OrderBy(x => x.Key))
            {
                if (WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (WSList[tKeyValue.Key] == true)
                    {
                        tConstantsFile += "\t\t\t kWebBuildkDataAssemblyPropertiesList.Add (" + tKeyValue.Key + ",new Dictionary<string, List<string>>());\n";
                        foreach (KeyValuePair<string, List<string>> tSubValue in tKeyValue.Value.OrderBy(x => x.Key))
                        {
                            tConstantsFile += "\t\t\t kWebBuildkDataAssemblyPropertiesList[" + tKeyValue.Key + "].Add(\"" + tSubValue.Key + "\", new List<string>(){\"" + string.Join("\", \"", tSubValue.Value.ToArray()) + "\"});\n";
                        }
                    }
                }
            }
            tConstantsFile += "\t\t}\n" +
            "\t//-------------------------------------------------------------------------------------------------------------\n" +
            "\t\t public void RestaureStepHeight() \n" +
            "\t\t{\n";
            Dictionary<string, int> tResult = new Dictionary<string, int>();
            foreach (KeyValuePair<int, Dictionary<string, string>> tKeyValue in kWebBuildkSLQAssemblyOrder.OrderBy(x => x.Key))
            {
                if (WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (WSList[tKeyValue.Key] == true)
                    {
                        foreach (KeyValuePair<string, string> tSubKeyValue in tKeyValue.Value.OrderBy(x => x.Key))
                        {
                            if (tResult.ContainsKey(tSubKeyValue.Key))
                            {
                                if (tResult[tSubKeyValue.Key] < tKeyValue.Key)
                                {
                                    tResult[tSubKeyValue.Key] = tKeyValue.Key;
                                }
                            }
                            else
                            {
                                tResult.Add(tSubKeyValue.Key, tKeyValue.Key);
                            }
                        }
                    }
                }
            }
            List<string> tTypeNameValidList = new List<string>();
            foreach (Type tType in NWDTypeLauncher.AllTypes)
            {
                tTypeNameValidList.Add(tType.Name);
            }
            foreach (KeyValuePair<string, int> tKeyValue in tResult.OrderBy(x => x.Key))
            {
                if (tTypeNameValidList.Contains(tKeyValue.Key) == true)
                {
                    tConstantsFile += "\t\t\t kLastWebBuildClass.Add (typeof(" + tKeyValue.Key + ")," + tKeyValue.Value + ");\n";
                }
            }
            tConstantsFile += "\t\t}\n" +
            "\t//-------------------------------------------------------------------------------------------------------------\n";
            tConstantsFile += "\t}\n" +
            "//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++\n" +
            "}\n" +
            "//=====================================================================================================================\n";
            // force to import this file by Unity3D
            string tOwnerConfigurationFolderPath = NWDToolbox.FindOwnerConfigurationFolder();
            //tFolderPath = "Assets";
            string tPath = tOwnerConfigurationFolderPath + "/NWDConfigurations.cs";

            tConstantsFile = NWDToolbox.CSharpFormat(tConstantsFile);
            File.WriteAllText(tPath, tConstantsFile);
            // force to import this file by Unity3D
            AssetDatabase.ImportAsset(tPath);
            AssetDatabase.Refresh();
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif