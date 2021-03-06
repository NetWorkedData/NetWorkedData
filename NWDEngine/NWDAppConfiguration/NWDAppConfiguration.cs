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
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEditor;
using NetWorkedData.NWDEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
#if UNITY_EDITOR
    public enum NWDAppInstallation : int
    {
        FirstInstallation,
        FormInstallation,
        Installed,
    }
#endif
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public class NWDAppConfigurationRestaure : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDAppConfigurationRestaure()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAppConfiguration : NWDApp
    {
        //-------------------------------------------------------------------------------------------------------------
        #region properties
        //-------------------------------------------------------------------------------------------------------------
        public NWDDataLocalizationManager DataLocalizationManager = new NWDDataLocalizationManager();
        public NWDAppEnvironment DevEnvironment
        {
            set; get;
        }
        public NWDAppEnvironment PreprodEnvironment
        {
            set; get;
        }
        public NWDAppEnvironment ProdEnvironment
        {
            set; get;
        }
#if UNITY_EDITOR
        public NWDAppInstallation Installed = NWDAppInstallation.FirstInstallation;
#endif
        public string CompileOn = "Mac Windows Linux";
        public string BuilderUser = "User";
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
        //public bool LauncherBenchmark = false;
        public int LauncherFaster = 10;
        public bool AnonymousDeviceConnected = true;

        public string SlackWebhookURLNotification = "";
        public string SlackWebhookURLSync = "";
        public string SlackWebhookURLUpgrade = "";

        public bool OverrideCacheMethodEverywhere = false;
        public bool OverrideCacheMethod = false;
        public bool OverrideCacheMethodInPlayMode = false;

        public bool PurgeOldAccountDatabase = false;

        public bool SurProtected = false; //TODO:  rename OverProtected
        public int PinCodeLenghtMin = 4; //TODO:  rename PinCodeMinLength
        public int PinCodeLenghtMax = 8; //TODO:  rename PinCodeMaxLength
        public int ProtectionTentativeMax = 6; //TODO:  rename maximum attempt

        public int LauncherClassEditorStep = 0;
        public int LauncherClassAccountStep = 0;
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
        public void Install()
        {
            NWDBenchmark.Start();
            DevEnvironment = new NWDAppEnvironment(NWDConstants.K_DEVELOPMENT_NAME, false);
            PreprodEnvironment = new NWDAppEnvironment(NWDConstants.K_PREPRODUCTION_NAME, false);
            ProdEnvironment = new NWDAppEnvironment(NWDConstants.K_PRODUCTION_NAME, false);
            Type tType = this.GetType();
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
            //Debug.Log("ServerEnvironmentCheck()");

            NWDProjectCredentialsManagerContent.SharedInstance().Load();

            NWDBasisHelper tServerClusterHelper = NWDBasisHelper.FindTypeInfos(typeof(NWDCluster));
            NWDBasisHelper tServerHelper = NWDBasisHelper.FindTypeInfos(typeof(NWDServer));
            NWDBasisHelper tServerDatasHelper = NWDBasisHelper.FindTypeInfos(typeof(NWDServerDatas));
            NWDBasisHelper tServerServicesHelper = NWDBasisHelper.FindTypeInfos(typeof(NWDServerServices));
            NWDBasisHelper tServerDomainsHelper = NWDBasisHelper.FindTypeInfos(typeof(NWDServerDomain));

            if (tServerClusterHelper.IsLoaded() == false)
            {
                tServerDomainsHelper.LoadFromDatabaseByBundle(NWDBundle.ALL, false);
            }
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
            if (tServerDomainsHelper.IsLoaded() == false)
            {
                tServerDomainsHelper.LoadFromDatabaseByBundle(NWDBundle.ALL, false);
            }

            DevCluster = false;
            DevServerServicesState = false;
            DevServerDataState = false;
            DevServerDomainState = false;

            PreprodCluster = false;
            PreprodServerServiceState = false;
            PreprodServerDataState = false;
            PreprodServerDomainState = false;

            ProdCluster = false;
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
                if (tServerDomain.Secure_Password.Decrypt() != null)
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
                else
                {
                    if (tServerDomain.Dev == true)
                    {
                        DevServerServicesState = false;
                    }
                    if (tServerDomain.Preprod == true)
                    {
                        PreprodServerServiceState = false;
                    }
                    if (tServerDomain.Prod == true)
                    {
                        ProdServerServiceState = false;
                    }
                    break;
                }
            }

            foreach (NWDServerDatas tServerData in tServerDatasHelper.Datas)
            {
                if (tServerData.MySQLSecurePassword.Decrypt() != null)
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
                else
                {
                    if (tServerData.Dev == true)
                    {
                        DevServerDataState = false;
                    }
                    if (tServerData.Preprod == true)
                    {
                        PreprodServerDataState = false;
                    }
                    if (tServerData.Prod == true)
                    {
                        ProdServerDataState = false;
                    }
                    break;
                }
            }

            foreach (NWDCluster tCluster in tServerClusterHelper.Datas)
            {
                if (tCluster.AdminKey == null)
                {
                    tCluster.AdminKey = new NWDSecurePassword();
                }
                if (tCluster.SaltServer == null)
                {
                    tCluster.SaltServer = new NWDSecurePassword();
                }
                if (string.IsNullOrEmpty(tCluster.AdminKey.Decrypt()) == false )
                {
                    if (tCluster.Dev == true)
                    {
                        DevCluster = true;
                    }
                    if (tCluster.Preprod == true)
                    {
                        PreprodCluster = true;
                    }
                    if (tCluster.Prod == true)
                    {
                        ProdCluster = true;
                    }
                }
                if (string.IsNullOrEmpty(tCluster.SaltServer.Decrypt()) == true)
                {
                    if (tCluster.Dev == true)
                    {
                        DevServerServicesState = false;
                    }
                    if (tCluster.Preprod == true)
                    {
                        PreprodServerServiceState = false;
                    }
                    if (tCluster.Prod == true)
                    {
                        ProdServerServiceState = false;
                    }
                }
            }
            NWDAppEnvironmentSync.Refresh();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        private bool DevCluster = false;
        private bool DevServerServicesState = false;
        private bool DevServerDataState = false;
        private bool DevServerDomainState = false;
        //-------------------------------------------------------------------------------------------------------------
        public bool DevServerIsActive()
        {
            return DevServerServicesState && DevServerDataState && DevServerDomainState && DevCluster;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool DevServerSyncActive()
        {
            return DevServerDomainState && DevCluster;
        }
        //-------------------------------------------------------------------------------------------------------------
        private bool PreprodCluster = false;
        private bool PreprodServerServiceState = false;
        private bool PreprodServerDataState = false;
        private bool PreprodServerDomainState = false;
        //-------------------------------------------------------------------------------------------------------------
        public bool PreprodServerIsActive()
        {
            return PreprodServerServiceState && PreprodServerDataState && PreprodServerDomainState && PreprodCluster;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool PreprodServerSyncActive()
        {
            return PreprodServerDomainState && PreprodCluster;
        }
        //-------------------------------------------------------------------------------------------------------------
        private bool ProdCluster = false;
        private bool ProdServerServiceState = false;
        private bool ProdServerDataState = false;
        private bool ProdServerDomainState = false;
        //-------------------------------------------------------------------------------------------------------------
        public bool ProdServerIsActive()
        {
            return ProdServerServiceState && ProdServerDataState && ProdServerDomainState && ProdCluster;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ProdServerSyncActive()
        {
            return ProdServerDomainState && ProdCluster;
        }
        //-------------------------------------------------------------------------------------------------------------
        // Determine the default mode
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
            //return WebFolder.TrimEnd('/').TrimStart('/') + "_" + WebBuild.ToString("0000");
            return OldWebServiceFolder(WebBuild);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string WebServiceFolderMax()
        {
            //return WebFolder.TrimEnd('/').TrimStart('/') + "_" + WebBuildMax.ToString("0000");
            return OldWebServiceFolder(WebBuildMax);
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
