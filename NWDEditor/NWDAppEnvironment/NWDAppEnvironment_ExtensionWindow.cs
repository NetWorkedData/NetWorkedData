//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
#if UNITY_EDITOR
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
        public void DrawInEditor(EditorWindow sEditorWindow)
        {
            //BTBBenchmark.Start();
            //float tMinWidht = 270.0F;
            //float tScrollMarge = 20.0f;
            NWDGUILayout.Section("Configuration for " + Environment + " environment");
            NWDGUILayout.Informations(Environment);
            NWDGUILayout.SubSection("AppName for server action " + Environment);
            AppName = EditorGUILayout.TextField("AppName", AppName);
            PreProdTimeFormat = EditorGUILayout.TextField("PreProdTimeFormat", PreProdTimeFormat);
            AppProtocol = EditorGUILayout.TextField("URL Scheme to use (xxx://)", AppProtocol);

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
            ServerHost = EditorGUILayout.TextField("MySQL Host", ServerHost);
            ServerUser = EditorGUILayout.TextField("MySQL user", ServerUser);
            ServerPassword = EditorGUILayout.TextField("MySQL password", ServerPassword);
            ServerBase = EditorGUILayout.TextField("MySQL base", ServerBase);
            WebTimeOut = EditorGUILayout.IntField("TimeOut request", WebTimeOut);
            EditorWebTimeOut = EditorGUILayout.IntField("Editor TimeOut request", EditorWebTimeOut);

            NWDGUILayout.SubSection("SFTP for " + Environment);
            SFTPHost = EditorGUILayout.TextField("SFTP Host", SFTPHost);
            SFTPPort = EditorGUILayout.IntField("SFTP Port", SFTPPort);
            SFTPFolder = EditorGUILayout.TextField("SFTP Folder", SFTPFolder);
            SFTPUser = EditorGUILayout.TextField("SFTP User ", SFTPUser);
            SFTPPassword = EditorGUILayout.TextField("SFTP Password ", SFTPPassword);

            NWDGUILayout.SubSection("Email to send forgotten code " + Environment);
            RescueEmail = EditorGUILayout.TextField("RescueEmail", RescueEmail);
            NWDGUILayout.SubSection("Mail Params for " + Environment);
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

            NWDGUILayout.SubSection("Social Params for " + Environment);
            FacebookAppID = EditorGUILayout.TextField("FacebookAppID", FacebookAppID);
            FacebookAppSecret = EditorGUILayout.TextField("FacebookAppSecret", FacebookAppSecret);
            GoogleAppKey = EditorGUILayout.TextField("GoogleAppKey", GoogleAppKey);
            UnityAppKey = EditorGUILayout.TextField("UnityAppKey", UnityAppKey);
            TwitterAppKey = EditorGUILayout.TextField("TwitterAppKey", TwitterAppKey);

            NWDGUILayout.SubSection("Admin Key for " + Environment);
            AdminKey = EditorGUILayout.TextField("AdminKey", AdminKey);
            AdminInPlayer = EditorGUILayout.Toggle("Admin In Player", AdminInPlayer);

            NWDGUILayout.SubSection("Token Historic limit for " + Environment);
            TokenHistoric = EditorGUILayout.IntSlider("Token number", TokenHistoric, 1, 10);

            //GUILayout.Label("Options for game in " + Environment, EditorStyles.boldLabel);
            SpeedOfGameTime = EditorGUILayout.FloatField("Speed Of Game Time", SpeedOfGameTime);

            //GUILayout.Label("Version for " + Environment, EditorStyles.boldLabel);
            EditorGUILayout.LabelField("version", NWDVersion.GetMaxVersionStringForEnvironemt(this), EditorStyles.boldLabel);

            NWDGUILayout.SubSection("SQL Thread Activation " + Environment);
            ThreadPoolForce = EditorGUILayout.Toggle("SQL Thread", ThreadPoolForce);
            WritingModeLocal = (NWDWritingMode)EditorGUILayout.EnumPopup("Writing Local", (NWDWritingModeConfig)WritingModeLocal);
            WritingModeWebService = (NWDWritingMode)EditorGUILayout.EnumPopup("Writing WebService", (NWDWritingModeConfig)WritingModeWebService);
            WritingModeEditor = (NWDWritingMode)EditorGUILayout.EnumPopup("Writing Editor", (NWDWritingModeConfig)WritingModeEditor);

            NWDGUILayout.SubSection("Last Build infos " + Environment);
            EditorGUILayout.LabelField("Build date", this.BuildDate, EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Build Timestamp", this.BuildTimestamp.ToString(), EditorStyles.boldLabel);
            DateTime tDate = BTBDateHelper.ConvertFromTimestamp(this.BuildTimestamp);
            EditorGUILayout.LabelField("Build Timestamp string ", tDate.ToString("yyyy/MM/dd HH:mm:ss"), EditorStyles.boldLabel);
            CartridgeColor = EditorGUILayout.ColorField("Cartridge Color", CartridgeColor);
            if (GUILayout.Button("Reset Build Timestamp"))
            {
                BuildTimestamp = 0;
            }
            FormatVerification();
            NWDGUILayout.LittleSpace();
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif