//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
//#define NWD_LOG
//#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEngine;
using System;
using System.Collections.Generic;
//=====================================================================================================================
using UnityEditor;
using NetWorkedData.NWDEditor;
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
        public void DrawInEditor()
        {
            NWDBenchmark.Start();
            NWDGUILayout.Section("Configuration for " + Environment + " environment");
            NWDGUILayout.SubSection("Cluster data used for " + Environment);
            NWDCluster tCluster = NWDCluster.SelectClusterforEnvironment(this, false);
            string tClusterSelection = "No selected cluster";
            if (tCluster != null)
            {
                tClusterSelection = tCluster.InternalKey;
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Cluster used", tClusterSelection);
                if (GUILayout.Button("Cluster edition"))
                {
                    NWDDataInspector.InspectNetWorkedData(tCluster);
                }
                GUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.LabelField("Cluster used", tClusterSelection);
            }
            NWDGUILayout.SubSection("App identity " + Environment);
            AppName = EditorGUILayout.TextField("AppName", AppName);
            PreProdTimeFormat = EditorGUILayout.TextField("Preprod time format", PreProdTimeFormat);
            //AppProtocol = EditorGUILayout.TextField("URL scheme (xxx://)", AppProtocol);
            EditorGUILayout.LabelField("Version", NWDVersion.GetMaxVersionStringForEnvironemt(this), EditorStyles.boldLabel);

            NWDGUILayout.SubSection("IP Ban " + Environment);
            IPBanActive = EditorGUILayout.Toggle("IPBan active", IPBanActive);
            EditorGUI.BeginDisabledGroup(!IPBanActive);
            IPBanMaxTentative = EditorGUILayout.IntField("Max tentative", IPBanMaxTentative);
            IPBanTimer = EditorGUILayout.IntField("Timer", IPBanTimer);
            EditorGUI.EndDisabledGroup();

            NWDGUILayout.SubSection("Security of datas" + Environment);
            DataSHAPassword = NWDToolbox.SaltCleaner(EditorGUILayout.TextField("SHA password", DataSHAPassword));
            DataSHAVector = NWDToolbox.SaltCleaner(EditorGUILayout.TextField("SHA vector", DataSHAVector));

            NWDGUILayout.SubSection("Hash of datas" + Environment);
            SaltStart = NWDToolbox.SaltCleaner(EditorGUILayout.TextField("Salt start", SaltStart));
            SaltEnd = NWDToolbox.SaltCleaner(EditorGUILayout.TextField("Salt end", SaltEnd));
            SaltFrequency = EditorGUILayout.IntField("Salt frequency", SaltFrequency);

            NWDGUILayout.SubSection("Log mode " + Environment);
            LogMode = (NWDEnvironmentLogMode)EditorGUILayout.EnumPopup("Log mode", LogMode);

            NWDGUILayout.SubSection("Network ping tester " + Environment);
            AddressPing = EditorGUILayout.TextField("Address ping (8.8.8.8)", AddressPing);

            NWDGUILayout.SubSection("Server params for " + Environment);
            AlwaysSecureData = EditorGUILayout.Toggle("Always secure data", AlwaysSecureData);
            ServerLanguage = (NWDServerLanguage)EditorGUILayout.EnumPopup("Server language", ServerLanguage);
            WebTimeOut = EditorGUILayout.IntField("Timeout request", WebTimeOut);
            EditorWebTimeOut = EditorGUILayout.IntField("Editor timeout request", EditorWebTimeOut);
            LoadBalancingLimit = EditorGUILayout.IntField("Balance load", LoadBalancingLimit);
            NWDGUILayout.SubSection("Email to send forgotten code " + Environment);
            RescueDelay = EditorGUILayout.IntField("Rescue delay", RescueDelay);
            RescueLoginLength = EditorGUILayout.IntField("Rescue login length", RescueLoginLength);
            RescuePasswordLength = EditorGUILayout.IntField("Rescue password length", RescuePasswordLength);

            NWDGUILayout.SubSection("Token historic limit for " + Environment);
            TokenHistoric = EditorGUILayout.IntSlider("Token number", TokenHistoric, 1, 10);

            NWDGUILayout.SubSection("Time multiplicator for " + Environment);
            SpeedOfGameTime = EditorGUILayout.FloatField("Speed of game time", SpeedOfGameTime);

            NWDGUILayout.SubSection("SQL thread Activation " + Environment);
            ThreadPoolForce = EditorGUILayout.Toggle("SQL Thread", ThreadPoolForce);
            WritingModeLocal = (NWDWritingMode)EditorGUILayout.EnumPopup("Writing local", (NWDWritingModeConfig)WritingModeLocal);
            WritingModeWebService = (NWDWritingMode)EditorGUILayout.EnumPopup("Writing webservice", (NWDWritingModeConfig)WritingModeWebService);
            WritingModeEditor = (NWDWritingMode)EditorGUILayout.EnumPopup("Writing editor", (NWDWritingModeConfig)WritingModeEditor);
            NWDGUILayout.SubSection("Last build infos " + Environment);
            EditorGUILayout.LabelField("Build date", this.BuildDate, EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Build timestamp", this.BuildTimestamp.ToString(), EditorStyles.boldLabel);
            DateTime tDate = NWEDateHelper.ConvertFromTimestamp(this.BuildTimestamp);
            EditorGUILayout.LabelField("Build timestamp string ", tDate.ToString("yyyy/MM/dd HH:mm:ss"), EditorStyles.boldLabel);
            CartridgeColor = EditorGUILayout.ColorField("Cartridge color", CartridgeColor);
            if (GUILayout.Button("Reset build timestamp"))
            {
                BuildTimestamp = 0;
            }
            NWDGUILayout.SubSection("Editor define " + Environment);
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
            NWDGUILayout.SubSection("Runtime define " + Environment);
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
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
