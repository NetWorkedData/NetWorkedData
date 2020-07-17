//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#endif
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
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAppConfiguration : NWDApp
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string Version = "0.79.00-cluster";
        //-------------------------------------------------------------------------------------------------------------
        #region properties
        //-------------------------------------------------------------------------------------------------------------
        public NWDDataLocalizationManager DataLocalizationManager = new NWDDataLocalizationManager();
        // [NWDAlias(NWD.K_DevEnvironment)]
        public NWDAppEnvironment DevEnvironment
        {
            set; get;
        }
        // [NWDAlias(NWD.K_PreprodEnvironment)]
        public NWDAppEnvironment PreprodEnvironment
        {
            set; get;
        }
        // [NWDAlias(NWD.K_ProdEnvironment)]
        public NWDAppEnvironment ProdEnvironment
        {
            set; get;
        }
        public string CompileOn = "Mac Windows Linux";
        public string BuilderUser = "User";
        //public Dictionary<string, string> IntegritySaltDictionary = new Dictionary<string, string>();
        //public Dictionary<string, string> GenerateSaltDictionary = new Dictionary<string, string>();
        public string WebFolder = "NWDFolder";
        public string TablePrefixe = string.Empty;
        public string DatabasePrefix = "NWD000000000";
        public string EditorPass;// = NWDToolbox.RandomStringCypher(UnityEngine.Random.Range(24, 36));
        public string EditorPassA;// = NWDToolbox.RandomStringCypher(UnityEngine.Random.Range(12, 18));
        public string EditorPassB;// = NWDToolbox.RandomStringCypher(UnityEngine.Random.Range(12, 18));
        public string AccountHashSalt;// = NWDToolbox.RandomStringCypher(UnityEngine.Random.Range(24, 36));
        public string AccountHashSaltA;// = NWDToolbox.RandomStringCypher(UnityEngine.Random.Range(12, 18));
        public string AccountHashSaltB;// = NWDToolbox.RandomStringCypher(UnityEngine.Random.Range(12, 18));
        public int WebBuild = 0;
        public int WebBuildMax = 0;
        public bool RowDataIntegrity = true;
        public Dictionary<int, bool> WSList = new Dictionary<int, bool>();
        public Dictionary<int, string> TagList = new Dictionary<int, string>();
        public int TagNumberUser = 10;
        public int TagNumber = 21;
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
        public bool BundleDatas = true;
        public bool AutoDeleteTrashDatas = true;
        public bool LauncherBenchmark = true;
        public int LauncherFaster = 10;
        //public bool PreloadDatasInEditor = true;
        //public bool AnonymousPlayerIsLocal = true;
        public bool AnonymousDeviceConnected = true;

        public string SlackWebhookURL = "";

        public bool OverrideCacheMethodEverywhere = false;
        public bool OverrideCacheMethod = false;
        public bool OverrideCacheMethodInPlayMode = false;

        public bool PurgeOldAccountDatabase = false;

        public bool SurProtected = false; //TODO:  rename OverProtected
        public int PinCodeLenghtMin = 4; //TODO:  rename PinCodeMinLength
        public int PinCodeLenghtMax = 8; //TODO:  rename PinCodeMaxLength
        public int ProtectionTentativeMax = 6; //TODO:  rename maximum attempt

        [Obsolete]
        public bool EditorTableCommun = true; // 

        //public bool ShowCompile = true; //TODO param in config editor extension...
        //public Color TintColor;

        public int LauncherClassEditorStep = 0;
        public int LauncherClassAccountStep = 0;
        //-------------------------------------------------------------------------------------------------------------
        public bool NeverNullDataType = true;
        //-------------------------------------------------------------------------------------------------------------
        #endregion

        #region shareInstance
        //-------------------------------------------------------------------------------------------------------------
        private static NWDAppConfiguration kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAppConfiguration SharedInstance()
        {
            if (kSharedInstance == null)
            {
                kSharedInstance = new NWDAppConfiguration();
            }
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
            Install();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public void ResetTintColor()
        //{
        //    TintColor = NWDToolbox.Color255(25, 20, 34, 255);
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void Install()
        {
            //NWDBenchmark.Start();
            DevEnvironment = new NWDAppEnvironment(NWDConstants.K_DEVELOPMENT_NAME, false);
            PreprodEnvironment = new NWDAppEnvironment(NWDConstants.K_PREPRODUCTION_NAME, false);
            ProdEnvironment = new NWDAppEnvironment(NWDConstants.K_PRODUCTION_NAME, false);
            //ResetTintColor();
            // REMOVED : Change to remove invoke!
            Type tType = this.GetType();
            //var tMethodInfo = tType.GetMethod("RestaureConfigurations", BindingFlags.Instance | BindingFlags.Public);
            //MethodInfo tMethodInfo = NWDAliasMethod.GetMethodPublicInstance(tType, NWDConstants.M_RestaureConfigurations);
            //if (tMethodInfo != null)
            //{
            //    tMethodInfo.Invoke(this, null);
            //}
            //else
            //{
            //    this.ProdEnvironment.Selected = false;
            //    this.PreprodEnvironment.Selected = false;
            //    this.DevEnvironment.Selected = true;
            //}

            if (RestaureConfigurations() == false)
            {
                EditorPass = NWDToolbox.RandomStringCypher(UnityEngine.Random.Range(24, 36));
                EditorPassA = NWDToolbox.RandomStringCypher(UnityEngine.Random.Range(12, 18));
                EditorPassB = NWDToolbox.RandomStringCypher(UnityEngine.Random.Range(12, 18));
                AccountHashSalt = NWDToolbox.RandomStringCypher(UnityEngine.Random.Range(24, 36));
                AccountHashSaltA = NWDToolbox.RandomStringCypher(UnityEngine.Random.Range(12, 18));
                AccountHashSaltB = NWDToolbox.RandomStringCypher(UnityEngine.Random.Range(12, 18));

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
            int tEnvironmentSelected = NWDProjectPrefs.GetInt(kEnvironmentSelectedKey);
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
            if (TagList.ContainsKey(-1) == false)
            {
                TagList.Add(-1, "No Tag");
            }
            if (TagList.ContainsKey(0) == false)
            {
                TagList.Add(0, "tag 0");
            }
            if (TagList.ContainsKey(1) == false)
            {
                TagList.Add(1, "tag 1");
            }
            if (TagList.ContainsKey(2) == false)
            {
                TagList.Add(2, "tag 2");
            }
            if (TagList.ContainsKey(3) == false)
            {
                TagList.Add(3, "tag 3");
            }
            if (TagList.ContainsKey(4) == false)
            {
                TagList.Add(4, "tag 4");
            }
            if (TagList.ContainsKey(5) == false)
            {
                TagList.Add(5, "tag 5");
            }
            if (TagList.ContainsKey(6) == false)
            {
                TagList.Add(6, "tag 6");
            }
            if (TagList.ContainsKey(7) == false)
            {
                TagList.Add(7, "tag 7");
            }
            if (TagList.ContainsKey(8) == false)
            {
                TagList.Add(8, "tag 8");
            }
            if (TagList.ContainsKey(9) == false)
            {
                TagList.Add(9, "tag 9");
            }
            if (TagList.ContainsKey(10) == false)
            {
                TagList.Add(10, "tag 10");
            }
            if (TagList.ContainsKey(11) == false)
            {
                TagList.Add(11, "Internal Created");
            }
            if (TagList.ContainsKey(12) == false)
            {
                TagList.Add(12, "(Reserved)");
            }
            if (TagList.ContainsKey(13) == false)
            {
                TagList.Add(13, "(Reserved)");
            }
            if (TagList.ContainsKey(14) == false)
            {
                TagList.Add(14, "(Reserved)");
            }
            if (TagList.ContainsKey(15) == false)
            {
                TagList.Add(15, "Test for Preprod");
            }
            if (TagList.ContainsKey(16) == false)
            {
                TagList.Add(16, "Test for Dev");
            }
            if (TagList.ContainsKey(17) == false)
            {
                TagList.Add(17, "Admin Created");
            }
            if (TagList.ContainsKey(18) == false)
            {
                TagList.Add(18, "Device Created");
            }
            if (TagList.ContainsKey(19) == false)
            {
                TagList.Add(19, "Server Created");
            }
            if (TagList.ContainsKey(20) == false)
            {
                TagList.Add(20, "User Created");
            }
            if (TagList.ContainsKey(21) == false)
            {
                TagList.Add(21, "TO DELETE");
            }

            if (TagList.ContainsKey(22) == false)
            {
                TagList.Add(22, "UnitTest To Delete");
            }
            if (TagList.ContainsKey(23) == false)
            {
                TagList.Add(23, "UnitTest Not Delete");
            }
#endif
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion

        #region instance method
        //-------------------------------------------------------------------------------------------------------------
        //public void SetSalt(string sKeyFirst, string sKeySecond, string sValue)
        //{
        //    string sKey = sKeyFirst + sKeySecond;
        //    if (IntegritySaltDictionary.ContainsKey(sKey))
        //    {
        //        IntegritySaltDictionary[sKey] = sValue;
        //    }
        //    else
        //    {
        //        IntegritySaltDictionary.Add(sKey, sValue);
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public string GetSalt(string sKeyFirst, string sKeySecond, string sKeyValid)
        //{
        //    string sKey = sKeyFirst + sKeySecond;
        //    string rReturn = string.Empty;
        //    if (IntegritySaltDictionary.ContainsKey(sKey))
        //    {
        //        rReturn = IntegritySaltDictionary[sKey];
        //    }
        //    if (string.IsNullOrEmpty(rReturn))
        //    {
        //        Debug.Log("Generate Salt for " + sKey);
        //        rReturn = NWDToolbox.RandomString(UnityEngine.Random.Range(12, 24));
        //        IntegritySaltDictionary.Add(sKey, rReturn);
        //        SetSaltValid(sKeyFirst, sKeyValid, string.Empty);
        //    }
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public void SetSaltValid(string sKeyFirst, string sKeyValid, string sValue)
        //{
        //    string sKey = sKeyFirst + sKeyValid;
        //    if (GenerateSaltDictionary.ContainsKey(sKey))
        //    {
        //        GenerateSaltDictionary[sKey] = sValue;
        //    }
        //    else
        //    {
        //        GenerateSaltDictionary.Add(sKey, sValue);
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public string GetSaltValid(string sKeyFirst, string sKeyValid)
        //{
        //    string sKey = sKeyFirst + sKeyValid;
        //    string rReturn = string.Empty;
        //    if (GenerateSaltDictionary.ContainsKey(sKey))
        //    {
        //        rReturn = GenerateSaltDictionary[sKey];
        //    }
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public string GetAccountPass(string sPinCode)
        {
            string tDeviceUniqueID = sPinCode + SystemInfo.deviceUniqueIdentifier;
            string tPass = tDeviceUniqueID;
            if (string.IsNullOrEmpty(NWDAppConfiguration.SharedInstance().AccountHashSalt))
            {
                tPass = NWESecurityTools.GenerateSha(tPass);
            }
            else
            {
                tPass = NWDAppConfiguration.SharedInstance().AccountHashSaltA.Substring(0, 8) +
                    NWESecurityTools.GenerateSha(tPass + NWDAppConfiguration.SharedInstance().AccountHashSalt) +
                    NWDAppConfiguration.SharedInstance().AccountHashSaltB.Substring(2, 8);
            }
            return tPass;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetEditorPass()
        {
            string tPass = string.Empty;
            if (string.IsNullOrEmpty(NWDAppConfiguration.SharedInstance().EditorPass))
            {
                tPass = string.Empty;
            }
            else
            {
                tPass = NWDAppConfiguration.SharedInstance().EditorPassA.Substring(1, 8) +
                    NWDAppConfiguration.SharedInstance().EditorPass +
                    NWDAppConfiguration.SharedInstance().EditorPassB.Substring(3, 8);
            }
            return tPass;
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
        public void ServerEnvironmentCheck()
        {
#if UNITY_EDITOR
            NWDBasisHelper tServerHelper = NWDBasisHelper.FindTypeInfos(typeof(NWDServer));
            NWDBasisHelper tServerClusterHelper = NWDBasisHelper.FindTypeInfos(typeof(NWDCluster));
            NWDBasisHelper tServerDatasHelper = NWDBasisHelper.FindTypeInfos(typeof(NWDServerDatas));
            NWDBasisHelper tServerServicesHelper = NWDBasisHelper.FindTypeInfos(typeof(NWDServerServices));
            NWDBasisHelper tServerDomainsHelper = NWDBasisHelper.FindTypeInfos(typeof(NWDServerDomain));

            if (tServerHelper.IsLoaded() == false)
            {
                tServerHelper.LoadFromDatabaseByBundle(NWDBundle.ALL, false);
            }
            if (tServerDatasHelper.IsLoaded() == false)
            {
                tServerDatasHelper.LoadFromDatabaseByBundle(NWDBundle.ALL, false);
            }
            if (tServerServicesHelper.IsLoaded() == false)
            {
                tServerServicesHelper.LoadFromDatabaseByBundle(NWDBundle.ALL, false);
            }
            if (tServerClusterHelper.IsLoaded() == false)
            {
                tServerClusterHelper.LoadFromDatabaseByBundle(NWDBundle.ALL, false);
            }
            if (tServerClusterHelper.IsLoaded() == false)
            {
                tServerDomainsHelper.LoadFromDatabaseByBundle(NWDBundle.ALL, false);
            }

            DevServerServicesState = false;
            DevServerDataState = false;
            DevServerDomainState = false;
            PreprodServerServiceState = false;
            PreprodServerDataState = false;
            PreprodServerDomainState = false;
            ProdServerServiceState = false;
            ProdServerDataState = false;
            ProdServerDomainState = false;

            foreach (NWDServerDomain tServerDomain in tServerDomainsHelper.Datas)
            {
                if (tServerDomain.Dev == true)
                {
                    DevServerDomainState = true;
                }
                if (tServerDomain.Preprod == true)
                {
                    PreprodServerDomainState = true;
                }
                if (tServerDomain.Prod == true)
                {
                    ProdServerDomainState = true;
                }
            }

            foreach (NWDServerServices tServerDomain in tServerServicesHelper.Datas)
            {
                if (tServerDomain.Dev == true)
                {
                    DevServerServicesState = true;
                }
                if (tServerDomain.Preprod == true)
                {
                    PreprodServerServiceState = true;
                }
                if (tServerDomain.Prod == true)
                {
                    ProdServerServiceState = true;
                }
            }

            foreach (NWDServerDatas tServerData in tServerDatasHelper.Datas)
            {
                if (tServerData.Dev == true)
                {
                    DevServerDataState = true;
                }
                if (tServerData.Preprod == true)
                {
                    PreprodServerDataState = true;
                }
                if (tServerData.Prod == true)
                {
                    ProdServerDataState = true;
                }
            }
            NWDAppEnvironmentSync.Refresh();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        private bool DevServerServicesState = false;
        private bool DevServerDataState = false;
        private bool DevServerDomainState = false;
        //-------------------------------------------------------------------------------------------------------------
        public bool DevServerIsActive()
        {
            return DevServerServicesState && DevServerDataState && DevServerDomainState;
        }
        //-------------------------------------------------------------------------------------------------------------
        private bool PreprodServerServiceState = false;
        private bool PreprodServerDataState = false;
        private bool PreprodServerDomainState = false;
        //-------------------------------------------------------------------------------------------------------------
        public bool PreprodServerIsActive()
        {
            return PreprodServerServiceState && PreprodServerDataState && PreprodServerDomainState;
        }
        //-------------------------------------------------------------------------------------------------------------
        private bool ProdServerServiceState = false;
        private bool ProdServerDataState = false;
        private bool ProdServerDomainState = false;
        //-------------------------------------------------------------------------------------------------------------
        public bool ProdServerIsActive()
        {
            return ProdServerServiceState && ProdServerDataState && ProdServerDomainState;
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
        //public bool AdminInPLayer()
        //{
        //    bool rReturn = false;
        //    NWDAppEnvironment tEnvironment = SelectedEnvironment();
        //    if (tEnvironment.AdminInPlayer == true)
        //    {
        //        if (string.IsNullOrEmpty(tEnvironment.AdminKey) == false)
        //        {
        //            if (string.IsNullOrEmpty(tEnvironment.AdminKeyHash) == false)
        //            {
        //                if (tEnvironment.AdminKeyHash == tEnvironment.AdminKeyHashGenerate())
        //                {
        //                    //NWDAccountInfos tAccountInfos = NWDAccountInfos.GetAccountInfosOrCreate();
        //                    //if (tAccountInfos.Tag == NWDBasisTag.TagAdminCreated)
        //                    //{
        //                        foreach (NWDAccounTest tAccount in NWDAccount.SelectDatasForTests())
        //                        {
        //                            if (tAccount.Reference == tEnvironment.PlayerAccountReference)
        //                            {
        //                                rReturn = true;
        //                            }
        //                        }
        //                    //}
        //                }
        //            }
        //        }
        //    }
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public NWDAppEnvironment[] AllEnvironements()
        {
            NWDAppEnvironment[] tEnvironnements = {
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
        public string OldWebServiceFolder(int sWebService)
        {
            return WebFolder.TrimEnd('/').TrimStart('/') + "_" + sWebService.ToString("0000");
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
