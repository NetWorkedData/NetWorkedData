//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using BasicToolBox;
using System;
using UnityEditor;
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
        public void DrawInEditor(EditorWindow sEditorWindow, bool sAutoSelect = false)
        {
            //BTBBenchmark.Start();
            float tMinWidht = 270.0F;
            float tScrollMarge = 20.0f;
            int tColum = 1;
            if (sEditorWindow.position.width - tScrollMarge >= tMinWidht * 2)
            {
                tColum = 2;
            }


            EditorGUILayout.HelpBox("Project configuration " + Environment + " for connection with server", MessageType.None);
            if (tColum > 1)
            {
                EditorGUILayout.BeginHorizontal();
            }

            EditorGUILayout.BeginVertical(GUILayout.MinWidth(tMinWidht));

            EditorGUILayout.TextField("AppName for server action " + Environment, EditorStyles.boldLabel);
            AppName = EditorGUILayout.TextField("AppName", AppName);
            PreProdTimeFormat = EditorGUILayout.TextField("PreProdTimeFormat", PreProdTimeFormat);
            AppProtocol = EditorGUILayout.TextField("URL Scheme to use (xxx://)", AppProtocol);
            EditorGUILayout.TextField("Security of Datas" + Environment, EditorStyles.boldLabel);
            DataSHAPassword = NWDToolbox.SaltCleaner(EditorGUILayout.TextField("SHA Password", DataSHAPassword));
            DataSHAVector = NWDToolbox.SaltCleaner(EditorGUILayout.TextField("SHA Vector", DataSHAVector));
            EditorGUILayout.TextField("Hash of Datas" + Environment, EditorStyles.boldLabel);
            SaltStart = NWDToolbox.SaltCleaner(EditorGUILayout.TextField("Salt start", SaltStart));
            SaltEnd = NWDToolbox.SaltCleaner(EditorGUILayout.TextField("Salt end", SaltEnd));
            SaltServer = NWDToolbox.SaltCleaner(EditorGUILayout.TextField("Salt server", SaltServer));
            SaltFrequency = EditorGUILayout.IntField("Salt Frequency", SaltFrequency);
            EditorGUILayout.TextField("Network Ping tester " + Environment, EditorStyles.boldLabel);
            AddressPing = EditorGUILayout.TextField("Address Ping (8.8.8.8)", AddressPing);
            EditorGUILayout.TextField("Server Params for " + Environment, EditorStyles.boldLabel);
            ServerHTTPS = EditorGUILayout.TextField("Server (https://…)", ServerHTTPS);
            AllwaysSecureData = EditorGUILayout.ToggleLeft("Allways Secure Data", AllwaysSecureData);
            LogMode = EditorGUILayout.ToggleLeft("LogMode", LogMode);
            ServerHost = EditorGUILayout.TextField("MySQL Host", ServerHost);
            ServerUser = EditorGUILayout.TextField("MySQL user", ServerUser);
            ServerPassword = EditorGUILayout.TextField("MySQL password", ServerPassword);
            ServerBase = EditorGUILayout.TextField("MySQL base", ServerBase);
            WebTimeOut = EditorGUILayout.IntField("TimeOut request", WebTimeOut);
            EditorWebTimeOut = EditorGUILayout.IntField("Editor TimeOut request", EditorWebTimeOut);

            EditorGUILayout.TextField("SFTP for " + Environment, EditorStyles.boldLabel);
            SFTPHost = EditorGUILayout.TextField("SFTP Host", SFTPHost);
            SFTPPort = EditorGUILayout.IntField("SFTP Port", SFTPPort);
            SFTPFolder = EditorGUILayout.TextField("SFTP Folder", SFTPFolder);
            SFTPUser = EditorGUILayout.TextField("SFTP User ", SFTPUser);
            SFTPPassword = EditorGUILayout.TextField("SFTP Password ", SFTPPassword);


            EditorGUILayout.TextField("Mail Params for " + Environment, EditorStyles.boldLabel);
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
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical(GUILayout.MinWidth(tMinWidht));
            EditorGUILayout.TextField("Social Params for " + Environment, EditorStyles.boldLabel);
            FacebookAppID = EditorGUILayout.TextField("FacebookAppID", FacebookAppID);
            FacebookAppSecret = EditorGUILayout.TextField("FacebookAppSecret", FacebookAppSecret);
            GoogleAppKey = EditorGUILayout.TextField("GoogleAppKey", GoogleAppKey);
            UnityAppKey = EditorGUILayout.TextField("UnityAppKey", UnityAppKey);
            TwitterAppKey = EditorGUILayout.TextField("TwitterAppKey", TwitterAppKey);
            EditorGUILayout.TextField("Email to send forgotten code " + Environment, EditorStyles.boldLabel);
            RescueEmail = EditorGUILayout.TextField("RescueEmail", RescueEmail);
            EditorGUILayout.TextField("Admin Key for " + Environment, EditorStyles.boldLabel);
            AdminKey = EditorGUILayout.TextField("AdminKey", AdminKey);
            EditorGUILayout.TextField("Token Historic limit for " + Environment, EditorStyles.boldLabel);
            TokenHistoric = EditorGUILayout.IntSlider("Token number", TokenHistoric, 1, 10);
            EditorGUILayout.TextField("Options for game in " + Environment, EditorStyles.boldLabel);
            SpeedOfGameTime = EditorGUILayout.FloatField("Speed Of GameTime", SpeedOfGameTime);
            EditorGUILayout.TextField("Version for " + Environment, EditorStyles.boldLabel);
            EditorGUILayout.LabelField("version", NWDVersion.GetMaxVersionStringForEnvironemt(this), EditorStyles.boldLabel);
            EditorGUILayout.TextField("SQL Thread Activation " + Environment, EditorStyles.boldLabel);
            ThreadPoolForce = EditorGUILayout.Toggle("SQL Thread", ThreadPoolForce);
            WritingModeLocal = (NWDWritingMode)EditorGUILayout.EnumPopup("Writing Local", (NWDWritingModeConfig)WritingModeLocal);
            WritingModeWebService = (NWDWritingMode)EditorGUILayout.EnumPopup("Writing WebService", (NWDWritingModeConfig)WritingModeWebService);
            WritingModeEditor = (NWDWritingMode)EditorGUILayout.EnumPopup("Writing Editor", (NWDWritingModeConfig)WritingModeEditor);
            EditorGUILayout.TextField("Last Build infos " + Environment, EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Build date", this.BuildDate, EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Build Timestamp", this.BuildTimestamp.ToString(), EditorStyles.boldLabel);
            DateTime tDate = BTBDateHelper.ConvertFromTimestamp(this.BuildTimestamp);
            EditorGUILayout.LabelField("Build Timestamp string ", tDate.ToString("yyyy/MM/dd HH:mm:ss"), EditorStyles.boldLabel);
            if (GUILayout.Button("Reset Build Timestamp"))
            {
                BuildTimestamp = 0;
            }
            CartridgeColor = EditorGUILayout.ColorField("Cartridge Color", CartridgeColor);
            EditorGUILayout.EndVertical();
            if (tColum > 1)
            {
                EditorGUILayout.EndHorizontal();
            }
            // DATABASE PARAMS
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Databases", MessageType.None);
            string tDatabasePathEditor = NWDDataManager.SharedInstance().DatabasePathEditor + "/" + NWDDataManager.SharedInstance().DatabaseNameEditor;

            //string tDatabasePathAccount = NWDDataManager.SharedInstance().DatabasePathAccount + "/" + NWDDataManager.SharedInstance().DatabaseNameAccount;
            string tDatabasePathAccount = "/" + NWDDataManager.SharedInstance().DatabaseNameAccount;

            EditorGUILayout.LabelField("Databases Editor config for all environements", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Editor path ", tDatabasePathEditor);
            if (GUILayout.Button("Editor Database File"))
            {
                EditorUtility.RevealInFinder(tDatabasePathEditor);
            }
            EditorGUILayout.LabelField("EditorPass", NWDAppConfiguration.SharedInstance().EditorPass);
            EditorGUILayout.LabelField("EditorPassA", NWDAppConfiguration.SharedInstance().EditorPassA);
            EditorGUILayout.LabelField("EditorPassAB", NWDAppConfiguration.SharedInstance().EditorPassB);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Editor Pass Result", NWDAppConfiguration.SharedInstance().GetEditorPass());
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.LabelField("Databases Accountconfig for all environements (by device)", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Account path ", tDatabasePathAccount);
            if (GUILayout.Button("Account Database File"))
            {
                EditorUtility.RevealInFinder(tDatabasePathAccount);
            }
            EditorGUILayout.LabelField("AccountHashSalt", NWDAppConfiguration.SharedInstance().AccountHashSalt);
            EditorGUILayout.LabelField("AccountHashSaltA", NWDAppConfiguration.SharedInstance().AccountHashSaltA);
            EditorGUILayout.LabelField("AccountHashSaltB", NWDAppConfiguration.SharedInstance().AccountHashSaltB);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Account Pass Result", NWDAppConfiguration.SharedInstance().GetAccountPass());
            EditorGUI.EndDisabledGroup();
            // WEBSERVICES PARAMS
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Webservices", MessageType.None);
            EditorGUILayout.LabelField("Webservices config for all environements", EditorStyles.boldLabel);
            if (tColum > 1)
            {
                EditorGUILayout.BeginHorizontal();
            }
            EditorGUILayout.BeginVertical(GUILayout.MinWidth(tMinWidht));

            Dictionary<int, bool> tWSList = new Dictionary<int, bool>();
            tWSList.Add(0, true);
            foreach (KeyValuePair<int, bool> tWS in NWDAppConfiguration.SharedInstance().WSList)
            {
                if (tWSList.ContainsKey(tWS.Key) == false)
                {
                    tWSList.Add(tWS.Key, tWS.Value);
                }
            }
            List<int> tWSListUsable = new List<int>();
            List<string> tWSListUsableString = new List<string>();
            foreach (KeyValuePair<int, bool> tWS in tWSList)
            {
                if (tWS.Value == true)
                {
                    tWSListUsable.Add(tWS.Key);
                    tWSListUsableString.Add(tWS.Key.ToString());
                }
            }
            NWDAppConfiguration.SharedInstance().WebFolder = EditorGUILayout.TextField("WebService Folder", NWDAppConfiguration.SharedInstance().WebFolder);
            NWDAppConfiguration.SharedInstance().RowDataIntegrity = EditorGUILayout.Toggle("Active Row Integrity", NWDAppConfiguration.SharedInstance().RowDataIntegrity);
            NWDAppConfiguration.SharedInstance().PreloadDatas = EditorGUILayout.Toggle("Preload Datas", NWDAppConfiguration.SharedInstance().PreloadDatas);

            //EditorGUILayout.LabelField("WebService active", NWDAppConfiguration.SharedInstance().WebBuild.ToString());
            //NWDAppConfiguration.SharedInstance().WebBuild = EditorGUILayout.IntField("WebService active", NWDAppConfiguration.SharedInstance().WebBuild);

            int tIndexWS = tWSListUsable.IndexOf(NWDAppConfiguration.SharedInstance().WebBuild);
            tIndexWS = EditorGUILayout.Popup("WebService active", tIndexWS, tWSListUsableString.ToArray());
            NWDAppConfiguration.SharedInstance().WebBuild = tWSListUsable[tIndexWS];
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical(GUILayout.MinWidth(tMinWidht));
            foreach (KeyValuePair<int, bool> tWS in tWSList)
            {
                EditorGUI.BeginDisabledGroup(tWS.Key == 0);
                NWDDatas tDatasToTest = NWDDatas.FindTypeInfos(typeof(NWDParameter));
                if (tDatasToTest.WebModelSQLOrder.ContainsKey(tWS.Key) == false)
                {
                    bool tV = EditorGUILayout.Toggle("(WebService " + tWS.Key.ToString() + " unused)", tWS.Value);
                    NWDAppConfiguration.SharedInstance().WSList[tWS.Key] = tV;
                }
                else
                {
                    bool tV = EditorGUILayout.Toggle("WebService " + tWS.Key.ToString() + " in config", tWS.Value);
                    NWDAppConfiguration.SharedInstance().WSList[tWS.Key] = tV;
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndVertical();
            if (tColum > 1)
            {
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Tags", MessageType.None);

            EditorGUILayout.LabelField("Tag managment (all environements)", EditorStyles.boldLabel);
            if (tColum > 1)
            {
                EditorGUILayout.BeginHorizontal();
            }
            EditorGUILayout.BeginVertical(GUILayout.MinWidth(tMinWidht));

            NWDAppConfiguration.SharedInstance().TagList[-1] = "No Tag";

            Dictionary<int, string> tTagList = new Dictionary<int, string>(NWDAppConfiguration.SharedInstance().TagList);
            for (int tI = -1; tI <= NWDAppConfiguration.SharedInstance().TagNumber; tI++)
            {
                if (NWDAppConfiguration.SharedInstance().TagList.ContainsKey(tI) == false)
                {
                    NWDAppConfiguration.SharedInstance().TagList.Add(tI, "tag " + tI.ToString());
                }
                EditorGUI.BeginDisabledGroup(tI < 0 || tI > NWDAppConfiguration.SharedInstance().TagNumberUser);
                string tV = EditorGUILayout.TextField("tag " + tI.ToString(), NWDAppConfiguration.SharedInstance().TagList[tI]);
                tTagList[tI] = tV.Replace("\"", "`");
                EditorGUI.EndDisabledGroup();
            }
            NWDAppConfiguration.SharedInstance().TagList = tTagList;
            EditorGUILayout.EndVertical();
            if (tColum > 1)
            {
                EditorGUILayout.EndHorizontal();
            }
            FormatVerification();
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif