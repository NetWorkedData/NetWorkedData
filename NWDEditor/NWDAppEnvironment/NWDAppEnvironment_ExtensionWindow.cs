//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:19
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using UnityEngine;
//using BasicToolBox;
using System;
using UnityEditor;
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAppEnvironment
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string MenuName()
        {
            return NWDConstants.K_APP_ENVIRONMENT_MENU_NAME;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawInEditor(EditorWindow sEditorWindow)
        {
            //NWEBenchmark.Start();
            NWDGUILayout.Section("Configuration for " + Environment + " environment");
            //NWDGUILayout.Informations(Environment);
            NWDGUILayout.SubSection("App identity " + Environment);
            AppName = EditorGUILayout.TextField("AppName", AppName);
            PreProdTimeFormat = EditorGUILayout.TextField("Preprod Time Format", PreProdTimeFormat);
            AppProtocol = EditorGUILayout.TextField("URL Scheme to use (xxx://)", AppProtocol);

            NWDGUILayout.SubSection("IP Ban " + Environment);
            IPBanActive = EditorGUILayout.Toggle("IP Ban Active", IPBanActive);
            EditorGUI.BeginDisabledGroup(!IPBanActive);
            IPBanMaxTentative = EditorGUILayout.IntField("Max Tentative", IPBanMaxTentative);
            IPBanTimer = EditorGUILayout.IntField("Timer", IPBanTimer);
            EditorGUI.EndDisabledGroup();

            NWDGUILayout.SubSection("Security of Datas" + Environment);
            DataSHAPassword = NWDToolbox.SaltCleaner(EditorGUILayout.TextField("SHA Password", DataSHAPassword));
            DataSHAVector = NWDToolbox.SaltCleaner(EditorGUILayout.TextField("SHA Vector", DataSHAVector));

            NWDGUILayout.SubSection("Hash of Datas" + Environment);
            SaltStart = NWDToolbox.SaltCleaner(EditorGUILayout.TextField("Salt start", SaltStart));
            SaltEnd = NWDToolbox.SaltCleaner(EditorGUILayout.TextField("Salt end", SaltEnd));
            SaltServer = NWDToolbox.SaltCleaner(EditorGUILayout.TextField("Salt server", SaltServer));
            SaltFrequency = EditorGUILayout.IntField("Salt Frequency", SaltFrequency);

            NWDGUILayout.SubSection("Network Ping tester " + Environment);
            AddressPing = EditorGUILayout.TextField("Address Ping (8.8.8.8)", AddressPing);

            NWDGUILayout.SubSection("Server Params for " + Environment);
            ServerHTTPS = EditorGUILayout.TextField("Server (https://…)", ServerHTTPS);
            AllwaysSecureData = EditorGUILayout.Toggle("Allways Secure Data", AllwaysSecureData);
            LogMode = EditorGUILayout.Toggle("LogMode", LogMode);
            EditorGUI.BeginDisabledGroup(true);
            ServerLanguage = (NWDServerLanguage)EditorGUILayout.EnumPopup("Server Language", ServerLanguage);
            EditorGUI.EndDisabledGroup();
            WebTimeOut = EditorGUILayout.IntField("TimeOut request", WebTimeOut);
            EditorWebTimeOut = EditorGUILayout.IntField("Editor TimeOut request", EditorWebTimeOut);
            SFTPBalanceLoad = EditorGUILayout.IntField("BalanceLoad", SFTPBalanceLoad);

            NWDGUILayout.SubSection("MySQL for " + Environment);
            ServerHost = EditorGUILayout.TextField("MySQL Host", ServerHost);
            ServerUser = EditorGUILayout.TextField("MySQL user", ServerUser);
            ServerPassword = EditorGUILayout.TextField("MySQL password", ServerPassword);
            ServerBase = EditorGUILayout.TextField("MySQL base", ServerBase);

            NWDGUILayout.SubSection("SFTP for " + Environment);
            SFTPHost = EditorGUILayout.TextField("SFTP Host", SFTPHost);
            SFTPPort = EditorGUILayout.IntField("SFTP Port", SFTPPort);
            SFTPFolder = EditorGUILayout.TextField("SFTP Folder", SFTPFolder);
            SFTPUser = EditorGUILayout.TextField("SFTP User ", SFTPUser);
            SFTPPassword = EditorGUILayout.TextField("SFTP Password ", SFTPPassword);

            NWDGUILayout.SubSection("Email to send forgotten code " + Environment);
            RescueEmail = EditorGUILayout.TextField("RescueEmail", RescueEmail);
            RescueDelay = EditorGUILayout.IntField("Rescue delay", RescueDelay);
            RescueLoginLength = EditorGUILayout.IntField("Rescue Login length", RescueLoginLength);
            RescuePasswordLength = EditorGUILayout.IntField("Rescue Password length", RescuePasswordLength);
            NWDGUILayout.SubSection("Mail Params for " + Environment);

            MailBySMTP = EditorGUILayout.Toggle("Mail By SMTP", MailBySMTP);
            EditorGUI.BeginDisabledGroup(!MailBySMTP);
            MailHost = EditorGUILayout.TextField("Mail Host", MailHost);
            MailPort = EditorGUILayout.IntField("Mail Port", MailPort);
            MailDomain = EditorGUILayout.TextField("Mail Domain", MailDomain);
            MailFrom = EditorGUILayout.TextField("Mail From ", MailFrom);
            MailReplyTo = EditorGUILayout.TextField("Mail Reply to", MailReplyTo);
            MailUserName = EditorGUILayout.TextField("Mail User Name", MailUserName);
            MailPassword = EditorGUILayout.TextField("Mail Password", MailPassword);
            MailAuthentication = EditorGUILayout.TextField("Mail Authentication", MailAuthentication);
            MailEnableStarttlsAuto = EditorGUILayout.TextField("Mail Enable Starttls Auto", MailEnableStarttlsAuto);
            MailOpenSSLVerifyMode = EditorGUILayout.TextField("Mail Open SSL Verify Mode", MailOpenSSLVerifyMode);
            EditorGUI.EndDisabledGroup();
            //NWDGUILayout.SubSection("Social Params for " + Environment);
            //FacebookAppID = EditorGUILayout.TextField("FacebookAppID", FacebookAppID);
            //FacebookAppSecret = EditorGUILayout.TextField("FacebookAppSecret", FacebookAppSecret);
            //GoogleAppKey = EditorGUILayout.TextField("GoogleAppKey", GoogleAppKey);
            //UnityAppKey = EditorGUILayout.TextField("UnityAppKey", UnityAppKey);
            //TwitterAppKey = EditorGUILayout.TextField("TwitterAppKey", TwitterAppKey);

            NWDGUILayout.SubSection("Admin Key for " + Environment);
            AdminKey = EditorGUILayout.TextField("AdminKey", AdminKey);
            AdminInPlayer = EditorGUILayout.Toggle("Admin In Player", AdminInPlayer);

            NWDGUILayout.SubSection("Token Historic limit for " + Environment);
            TokenHistoric = EditorGUILayout.IntSlider("Token number", TokenHistoric, 1, 10);

            SpeedOfGameTime = EditorGUILayout.FloatField("Speed Of Game Time", SpeedOfGameTime);

            EditorGUILayout.LabelField("version", NWDVersion.GetMaxVersionStringForEnvironemt(this), EditorStyles.boldLabel);

            NWDGUILayout.SubSection("SQL Thread Activation " + Environment);
            ThreadPoolForce = EditorGUILayout.Toggle("SQL Thread", ThreadPoolForce);
            WritingModeLocal = (NWDWritingMode)EditorGUILayout.EnumPopup("Writing Local", (NWDWritingModeConfig)WritingModeLocal);
            WritingModeWebService = (NWDWritingMode)EditorGUILayout.EnumPopup("Writing WebService", (NWDWritingModeConfig)WritingModeWebService);
            WritingModeEditor = (NWDWritingMode)EditorGUILayout.EnumPopup("Writing Editor", (NWDWritingModeConfig)WritingModeEditor);

            NWDGUILayout.SubSection("Last Build infos " + Environment);
            EditorGUILayout.LabelField("Build date", this.BuildDate, EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Build Timestamp", this.BuildTimestamp.ToString(), EditorStyles.boldLabel);
            DateTime tDate = NWEDateHelper.ConvertFromTimestamp(this.BuildTimestamp);
            EditorGUILayout.LabelField("Build Timestamp string ", tDate.ToString("yyyy/MM/dd HH:mm:ss"), EditorStyles.boldLabel);
            CartridgeColor = EditorGUILayout.ColorField("Cartridge Color", CartridgeColor);
            if (GUILayout.Button("Reset Build Timestamp"))
            {
                BuildTimestamp = 0;
            }

            NWDGUILayout.SubSection("Editor Define " + Environment);
            Dictionary<long, string> tEditorExtensionDictionary = new Dictionary<long, string>(EditorDefineDictionary);
            Dictionary<long, string> tEditorExtensionDictionaryNext = new Dictionary<long, string>();
            if (tEditorExtensionDictionary.ContainsKey(0) == false)
            {
                tEditorExtensionDictionary.Add(0, string.Empty);
            }
            foreach (KeyValuePair<long, string> tKeyValue in tEditorExtensionDictionary)
            {
                GUILayout.BeginHorizontal();
                NWDAppEnvironmentEditorDefineEnum tKey = NWDAppEnvironmentEditorDefineEnum.GetForValue(tKeyValue.Key);
                EditorGUILayout.LabelField(string.Empty, string.Empty);
                Rect tRect = GUILayoutUtility.GetLastRect();
                NWEDataTypeEnum tNext = tKey.ControlField(tRect, "Key", false) as NWEDataTypeEnum;
                EditorGUI.BeginDisabledGroup(tKeyValue.Key == 0);
                string tNextText = EditorGUILayout.TextField("Value", tKeyValue.Value);
                EditorGUI.EndDisabledGroup();
                if (tEditorExtensionDictionaryNext.ContainsKey(tNext.ToLong()) == false)
                {
                    tEditorExtensionDictionaryNext.Add(tNext.ToLong(), tNextText);
                }
                GUILayout.EndHorizontal();
            }
            if (tEditorExtensionDictionaryNext.ContainsKey(0) == true)
            {
                tEditorExtensionDictionaryNext.Remove(0);
            }

            EditorDefineDictionary = tEditorExtensionDictionaryNext;

            NWDGUILayout.SubSection("Runtime Define " + Environment);

            Dictionary<long, string> tRuntimeExtensionDictionary = new Dictionary<long, string>(RuntimeDefineDictionary);
            Dictionary<long, string> tRuntimeExtensionDictionaryNext = new Dictionary<long, string>();
            if (tRuntimeExtensionDictionary.ContainsKey(0) == false)
            {
                tRuntimeExtensionDictionary.Add(0, string.Empty);
            }
            foreach (KeyValuePair<long, string> tKeyValue in tRuntimeExtensionDictionary)
            {
                GUILayout.BeginHorizontal();
                NWDAppEnvironmentRuntimeDefineEnum tKey = NWDAppEnvironmentRuntimeDefineEnum.GetForValue(tKeyValue.Key);
                EditorGUILayout.LabelField(string.Empty, string.Empty);
                Rect tRect = GUILayoutUtility.GetLastRect();
                NWEDataTypeEnum tNext = tKey.ControlField(tRect, "Key", false) as NWEDataTypeEnum;
                EditorGUI.BeginDisabledGroup(tKeyValue.Key == 0);
                string tNextText = EditorGUILayout.TextField("Value", tKeyValue.Value);
                EditorGUI.EndDisabledGroup();
                if (tRuntimeExtensionDictionaryNext.ContainsKey(tNext.ToLong()) == false)
                {
                    tRuntimeExtensionDictionaryNext.Add(tNext.ToLong(), tNextText);
                }
                GUILayout.EndHorizontal();
            }
            if (tRuntimeExtensionDictionaryNext.ContainsKey(0) == true)
            {
                tRuntimeExtensionDictionaryNext.Remove(0);
            }

            RuntimeDefineDictionary = tRuntimeExtensionDictionaryNext;

            FormatVerification();

            NWDGUILayout.LittleSpace();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif