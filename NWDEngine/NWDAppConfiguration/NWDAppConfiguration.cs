//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

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
        public NWDAppEnvironment DevEnvironment = new NWDAppEnvironment(NWDConstants.K_DEVELOPMENT_NAME, true);
        public NWDAppEnvironment PreprodEnvironment = new NWDAppEnvironment(NWDConstants.K_PREPRODUCTION_NAME, false);
        public NWDAppEnvironment ProdEnvironment = new NWDAppEnvironment(NWDConstants.K_PRODUCTION_NAME, false);
        public Dictionary<string, string> IntegritySaltDictionary = new Dictionary<string, string>();
        public Dictionary<string, string> GenerateSaltDictionary = new Dictionary<string, string>();
        public string WebFolder = "NWDFolder";
        public string EditorPass = NWDToolbox.RandomString(UnityEngine.Random.Range(12, 24));
        public string AccountHashSalt = NWDToolbox.RandomString(UnityEngine.Random.Range(12, 24));
        public int WebBuild = 0;
        public bool RowDataIntegrity = true;
        public Dictionary<int, bool> WSList = new Dictionary<int, bool>();
        public Dictionary<int, string> TagList = new Dictionary<int, string>();
        public int TagNumberUser = 10;
        public int TagNumber = 20;
        public Dictionary<int, Dictionary<string, string[]>> kWebBuildkCSVAssemblyOrderArray = new Dictionary<int, Dictionary<string, string[]>>();
        public Dictionary<int, Dictionary<string, string[]>> kWebBuildkSLQAssemblyOrderArray = new Dictionary<int, Dictionary<string, string[]>>();
        public Dictionary<int, Dictionary<string, string>> kWebBuildkSLQAssemblyOrder = new Dictionary<int, Dictionary<string, string>>();
        public Dictionary<int, Dictionary<string, List<string>>> kWebBuildkSLQIntegrityOrder = new Dictionary<int, Dictionary<string, List<string>>>();
        public Dictionary<int, Dictionary<string, List<string>>> kWebBuildkSLQIntegrityServerOrder = new Dictionary<int, Dictionary<string, List<string>>>();
        public Dictionary<int, Dictionary<string, List<string>>> kWebBuildkDataAssemblyPropertiesList = new Dictionary<int, Dictionary<string, List<string>>>();
        public Dictionary<string, string> BundleName = new Dictionary<string, string>();
        public Dictionary<Type, int> kLastWebBuildClass = new Dictionary<Type, int>();
        public string ProjetcLanguage = "en";
        public bool PreloadDatas = true;
        //-------------------------------------------------------------------------------------------------------------
        #endregion

        #region shareInstance
        //-------------------------------------------------------------------------------------------------------------
        private static readonly NWDAppConfiguration kSharedInstance = new NWDAppConfiguration();
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAppConfiguration SharedInstance()
        {
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion

        #region constructor
        //-------------------------------------------------------------------------------------------------------------
        public const string kEnvironmentSelectedKey = "kEnvironmentSelectedKey";
        //-------------------------------------------------------------------------------------------------------------
        public NWDAppConfiguration()
        {
            EditorPass = NWDToolbox.RandomString(UnityEngine.Random.Range(24, 24));
            AccountHashSalt = NWDToolbox.RandomString(UnityEngine.Random.Range(24, 24));

            Type tType = this.GetType();
            var tMethodInfo = tType.GetMethod("RestaureConfigurations", BindingFlags.Instance | BindingFlags.Public);
            if (tMethodInfo != null)
            {
                tMethodInfo.Invoke(this, null);
            }
            else
            {
                this.ProdEnvironment.Selected = false;
                this.PreprodEnvironment.Selected = false;
                this.DevEnvironment.Selected = true;
            }

            // But in unity we bypass the restaure configuration to use the environment selected in the editor's preferences 
#if UNITY_EDITOR
            // reset all environement to false
            this.ProdEnvironment.Selected = false;
            this.PreprodEnvironment.Selected = false;
            this.DevEnvironment.Selected = false;
            // We restaured environement selected in the preference and override the configurations file
            int tEnvironmentSelected = EditorPrefs.GetInt(kEnvironmentSelectedKey);
            switch (tEnvironmentSelected)
            {
                case 1:
                    this.PreprodEnvironment.Selected = true;
                    break;
                case 2:
                    this.ProdEnvironment.Selected = true;
                    break;
                case 0:
                default:
                    this.DevEnvironment.Selected = true;
                    break;
            }
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion

        #region instance method
        //-------------------------------------------------------------------------------------------------------------
        public void SetSalt(string sKeyFirst, string sKeySecond, string sValue)
        {
            string sKey = sKeyFirst + sKeySecond;
            if (IntegritySaltDictionary.ContainsKey(sKey))
            {
                IntegritySaltDictionary[sKey] = sValue;
            }
            else
            {
                IntegritySaltDictionary.Add(sKey, sValue);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetSalt(string sKeyFirst, string sKeySecond, string sKeyValid)
        {
            string sKey = sKeyFirst + sKeySecond;
            string rReturn = "";
            if (IntegritySaltDictionary.ContainsKey(sKey))
            {
                rReturn = IntegritySaltDictionary[sKey];
            }
            if (rReturn == "")
            {
                rReturn = NWDToolbox.RandomString(UnityEngine.Random.Range(12, 24));
                IntegritySaltDictionary.Add(sKey, rReturn);
                SetSaltValid(sKeyFirst, sKeyValid, "");
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetSaltValid(string sKeyFirst, string sKeyValid, string sValue)
        {
            string sKey = sKeyFirst + sKeyValid;
            if (GenerateSaltDictionary.ContainsKey(sKey))
            {
                GenerateSaltDictionary[sKey] = sValue;
            }
            else
            {
                GenerateSaltDictionary.Add(sKey, sValue);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetSaltValid(string sKeyFirst, string sKeyValid)
        {
            string sKey = sKeyFirst + sKeyValid;
            string rReturn = "";
            if (GenerateSaltDictionary.ContainsKey(sKey))
            {
                rReturn = GenerateSaltDictionary[sKey];
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetAccountPass()
        {
            if (string.IsNullOrEmpty(NWDAppConfiguration.SharedInstance().AccountHashSalt))
            {
                return "";
            }
            else
            {
                string tDeviceUniqueID = SystemInfo.deviceUniqueIdentifier;
                //Debug.Log("GetAccountPass () : tDeviceUniqueID = " + tDeviceUniqueID);
                string tPass = NWDAccount.HashSum("w*€Ô`ds" + SystemInfo.deviceUniqueIdentifier + "-" +
                                                  NWDAppConfiguration.SharedInstance().AccountHashSalt + "dƒs@&g");
                //Debug.Log("GetAccountPass () : tPass = " + tPass);
                return tPass.Substring(0, 24);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetEditorPass()
        {
            if (string.IsNullOrEmpty(NWDAppConfiguration.SharedInstance().EditorPass))
            {
                return "";
            }
            else
            {
                return "Ôûw*€dñ" + NWDAppConfiguration.SharedInstance().EditorPass + "&s@g";
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAppEnvironment SelectedEnvironment()
        {
            NWDAppEnvironment rReturn = ProdEnvironment;
            if (DevEnvironment.Selected == true)
            {
                rReturn = DevEnvironment;
            }
            else if (PreprodEnvironment.Selected == true)
            {
                rReturn = PreprodEnvironment;
            }
            else
            {
                rReturn = ProdEnvironment;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        // Determine the default mode
        // sWritingMode = NWDAppConfiguration.WritingMode(sWritingMode);
        public static NWDWritingMode WritingMode(NWDWritingMode sWritingMode)
        {
            NWDWritingMode rWritingMode = sWritingMode;
            if (rWritingMode == NWDWritingMode.ByDefaultLocal)
            {
                rWritingMode = NWDAppEnvironment.SelectedEnvironment().WritingModeLocal;
            }
            else if (rWritingMode == NWDWritingMode.ByDefaultWebService)
            {
                rWritingMode = NWDAppEnvironment.SelectedEnvironment().WritingModeWebService;
            }
            else if (rWritingMode == NWDWritingMode.ByEditorDefault)
            {
                rWritingMode = NWDAppEnvironment.SelectedEnvironment().WritingModeEditor;
            }
            if (rWritingMode == NWDWritingMode.ByDefaultLocal
                || rWritingMode == NWDWritingMode.ByDefaultWebService
                || rWritingMode == NWDWritingMode.ByEditorDefault)
            {
                rWritingMode = NWDWritingMode.MainThread;
            }
            return rWritingMode;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool IsProdEnvironement()
        {
            return ProdEnvironment.Selected;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool IsPreprodEnvironement()
        {
            return PreprodEnvironment.Selected;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool IsDevEnvironement()
        {
            return DevEnvironment.Selected;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAppEnvironment[] AllEnvironements()
        {
            NWDAppEnvironment[] tEnvironnements = new NWDAppEnvironment[] {
                DevEnvironment,
                PreprodEnvironment,
                ProdEnvironment
            };
            return tEnvironnements;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string WebServiceFolder()
        {
            return WebFolder.TrimEnd('/').TrimStart('/') + "_" + WebBuild.ToString("0000");
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
    }
}
//=====================================================================================================================
