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
            GUILayout.Label("Configuration for " + Environment + " environment", NWDConstants.kLabelTitleStyle);
            GUILayout.Label("AppName for server action " + Environment, NWDConstants.kLabelSubTitleStyle);
            AppName = EditorGUILayout.TextField("AppName", AppName);
            PreProdTimeFormat = EditorGUILayout.TextField("PreProdTimeFormat", PreProdTimeFormat);
            AppProtocol = EditorGUILayout.TextField("URL Scheme to use (xxx://)", AppProtocol);
           
            GUILayout.Label("Security of Datas" + Environment, NWDConstants.kLabelSubTitleStyle);
            DataSHAPassword = NWDToolbox.SaltCleaner(EditorGUILayout.TextField("SHA Password", DataSHAPassword));
            DataSHAVector = NWDToolbox.SaltCleaner(EditorGUILayout.TextField("SHA Vector", DataSHAVector));
           
            GUILayout.Label("Hash of Datas" + Environment, NWDConstants.kLabelSubTitleStyle);
            SaltStart = NWDToolbox.SaltCleaner(EditorGUILayout.TextField("Salt start", SaltStart));
            SaltEnd = NWDToolbox.SaltCleaner(EditorGUILayout.TextField("Salt end", SaltEnd));
            SaltServer = NWDToolbox.SaltCleaner(EditorGUILayout.TextField("Salt server", SaltServer));
            SaltFrequency = EditorGUILayout.IntField("Salt Frequency", SaltFrequency);

            GUILayout.Label("Network Ping tester " + Environment, NWDConstants.kLabelSubTitleStyle);
            AddressPing = EditorGUILayout.TextField("Address Ping (8.8.8.8)", AddressPing);

            GUILayout.Label("Server Params for " + Environment, NWDConstants.kLabelSubTitleStyle);
            ServerHTTPS = EditorGUILayout.TextField("Server (https://…)", ServerHTTPS);
            AllwaysSecureData = EditorGUILayout.ToggleLeft("Allways Secure Data", AllwaysSecureData);
            LogMode = EditorGUILayout.ToggleLeft("LogMode", LogMode);
            ServerHost = EditorGUILayout.TextField("MySQL Host", ServerHost);
            ServerUser = EditorGUILayout.TextField("MySQL user", ServerUser);
            ServerPassword = EditorGUILayout.TextField("MySQL password", ServerPassword);
            ServerBase = EditorGUILayout.TextField("MySQL base", ServerBase);
            WebTimeOut = EditorGUILayout.IntField("TimeOut request", WebTimeOut);
            EditorWebTimeOut = EditorGUILayout.IntField("Editor TimeOut request", EditorWebTimeOut);

            GUILayout.Label("SFTP for " + Environment, NWDConstants.kLabelSubTitleStyle);
            SFTPHost = EditorGUILayout.TextField("SFTP Host", SFTPHost);
            SFTPPort = EditorGUILayout.IntField("SFTP Port", SFTPPort);
            SFTPFolder = EditorGUILayout.TextField("SFTP Folder", SFTPFolder);
            SFTPUser = EditorGUILayout.TextField("SFTP User ", SFTPUser);
            SFTPPassword = EditorGUILayout.TextField("SFTP Password ", SFTPPassword);

            GUILayout.Label("Email to send forgotten code " + Environment, NWDConstants.kLabelSubTitleStyle);
            RescueEmail = EditorGUILayout.TextField("RescueEmail", RescueEmail);
            GUILayout.Label("Mail Params for " + Environment, NWDConstants.kLabelSubTitleStyle);
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

            GUILayout.Label("Social Params for " + Environment, NWDConstants.kLabelSubTitleStyle);
            FacebookAppID = EditorGUILayout.TextField("FacebookAppID", FacebookAppID);
            FacebookAppSecret = EditorGUILayout.TextField("FacebookAppSecret", FacebookAppSecret);
            GoogleAppKey = EditorGUILayout.TextField("GoogleAppKey", GoogleAppKey);
            UnityAppKey = EditorGUILayout.TextField("UnityAppKey", UnityAppKey);
            TwitterAppKey = EditorGUILayout.TextField("TwitterAppKey", TwitterAppKey);

            GUILayout.Label("Admin Key for " + Environment, NWDConstants.kLabelSubTitleStyle);
            AdminKey = EditorGUILayout.TextField("AdminKey", AdminKey);

            GUILayout.Label("Token Historic limit for " + Environment, NWDConstants.kLabelSubTitleStyle);
            TokenHistoric = EditorGUILayout.IntSlider("Token number", TokenHistoric, 1, 10);

            GUILayout.Label("Options for game in " + Environment, EditorStyles.boldLabel);
            SpeedOfGameTime = EditorGUILayout.FloatField("Speed Of GameTime", SpeedOfGameTime);

            GUILayout.Label("Version for " + Environment, EditorStyles.boldLabel);
            EditorGUILayout.LabelField("version", NWDVersion.GetMaxVersionStringForEnvironemt(this), EditorStyles.boldLabel);

            GUILayout.Label("SQL Thread Activation " + Environment, NWDConstants.kLabelSubTitleStyle);
            ThreadPoolForce = EditorGUILayout.Toggle("SQL Thread", ThreadPoolForce);
            WritingModeLocal = (NWDWritingMode)EditorGUILayout.EnumPopup("Writing Local", (NWDWritingModeConfig)WritingModeLocal);
            WritingModeWebService = (NWDWritingMode)EditorGUILayout.EnumPopup("Writing WebService", (NWDWritingModeConfig)WritingModeWebService);
            WritingModeEditor = (NWDWritingMode)EditorGUILayout.EnumPopup("Writing Editor", (NWDWritingModeConfig)WritingModeEditor);

            GUILayout.Label("Last Build infos " + Environment, NWDConstants.kLabelSubTitleStyle);
            EditorGUILayout.LabelField("Build date", this.BuildDate, EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Build Timestamp", this.BuildTimestamp.ToString(), EditorStyles.boldLabel);
            DateTime tDate = BTBDateHelper.ConvertFromTimestamp(this.BuildTimestamp);
            EditorGUILayout.LabelField("Build Timestamp string ", tDate.ToString("yyyy/MM/dd HH:mm:ss"), EditorStyles.boldLabel);
            if (GUILayout.Button("Reset Build Timestamp"))
            {
                BuildTimestamp = 0;
            }
            CartridgeColor = EditorGUILayout.ColorField("Cartridge Color", CartridgeColor);
            FormatVerification();
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif