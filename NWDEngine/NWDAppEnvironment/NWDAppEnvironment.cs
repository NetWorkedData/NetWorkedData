//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public enum NWDEnvironmentLogMode : short
    {
        NoLog = 0,
        LogInConsole = 1,
        LogInFile = 2,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public enum NWDEnvironmentType : short
    {
        Prod = 0,
        Preprod = 1,
        Dev = 2,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public partial class NWDAppEnvironmentRuntimeDefineEnum : NWEDataTypeEnumGeneric<NWDAppEnvironmentRuntimeDefineEnum>
    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAppEnvironmentRuntimeDefineEnum DefaultKey = Add(1, "DefaultKey");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public partial class NWDAppEnvironmentEditorDefineEnum : NWEDataTypeEnumGeneric<NWDAppEnvironmentEditorDefineEnum>
    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAppEnvironmentEditorDefineEnum DefaultKey = Add(1, "DefaultKey");
        public static NWDAppEnvironmentEditorDefineEnum DefaultKey2 = Add(2, "2");
        public static NWDAppEnvironmentEditorDefineEnum DefaultKey3 = Add(3, "3");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAppEnvironment
    {
        #region properties
        //-------------------------------------------------------------------------------------------------------------
        public bool Selected = false;
        public string Environment = NWDConstants.K_PRODUCTION_NAME;
        //-------------------------------------------------------------------------------------------------------------
        //public NWDAppEnvironmentPlayerStatut PlayerStatut = NWDAppEnvironmentPlayerStatut.Temporary;
        //-------------------------------------------------------------------------------------------------------------
        public string GetAccountReference()
        {
            return AccountReference;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetAccountReference(string sPlayerAccountReference)
        {
            AccountReference = sPlayerAccountReference;
            NWDAccountInfos.ResetCurrentData();
        }
        //-------------------------------------------------------------------------------------------------------------
        private string AccountReference = string.Empty;
        //private NWDAccountInfos PlayerInfos = null;

        public string RequesToken = string.Empty;
        public Dictionary<long, string> RuntimeDefineDictionary = new Dictionary<long, string>();
        // for debug anti-crack
#if UNITY_EDITOR
        public string PreviewRequesToken = string.Empty;
        public string LastPreviewRequesToken = string.Empty;
        public Dictionary<long, string> EditorDefineDictionary = new Dictionary<long, string>();
#endif
        private string WithSpecialSDKI;
        //-------------------------------------------------------------------------------------------------------------
        //public string AnonymousPlayerAccountReference = string.Empty;
        // reccord the first anonymous value to restaure old original account
        //public string AnonymousResetPassword = string.Empty;
        // reccord the secretKey to reset token
        //-------------------------------------------------------------------------------------------------------------
        public string DataSHAPassword = string.Empty;
        public string DataSHAVector = string.Empty;
        public string SaltStart = string.Empty;
        public string SaltEnd = string.Empty;
        public int IPBanTimer = 3600;
        public int IPBanMaxTentative = 3;
        public bool IPBanActive = true;
        public int LoadBalancingLimit = 50; // TODO : Rename LoadBalancingLimit
#if UNITY_EDITOR
        public NWDServerLanguage ServerLanguage = NWDServerLanguage.PHP;
        //public string SaltServer = string.Empty;
        public int RescueDelay = 3600;
        public int RescueLoginLength = 12;
        public int RescuePasswordLength = 24;

        //public string MailFrom = string.Empty;

        ////[Obsolete] // TODO : Remove
        ////public string MailReplyTo = string.Empty;
        //public string RescueEmail = "no-reply@my-web-site.com";

        //public bool MailBySMTP = false;
        //public bool MailSSL = true;
        //public string MailHost = "smtp.my-web-site.com";
        //public int MailPort = 465;
        //public bool MailAuth = true;
        //public string MailUserName = "no-reply@my-web-site.com";
        //public string MailPassword = "passwordFoMyLogin";

        //[Obsolete] // TODO : Remove
        //public string MailDomain = string.Empty;
        //[Obsolete] // TODO : Remove
        //public string MailAuthentication = "plain";
        //[Obsolete] // TODO : Remove
        //public string MailEnableStarttlsAuto = "true";
        //[Obsolete] // TODO : Remove
        //public string MailOpenSSLVerifyMode = "peer";

        //public string ServerHost = "localhost";
        //public string ServerUser = "user";
        //public string ServerPassword = string.Empty;
        //public string ServerBase = "myDatabase";
#endif
        public NWDEnvironmentLogMode LogMode = NWDEnvironmentLogMode.NoLog;
        //public bool LogInFileMode = true;
        //public string AdminKey = string.Empty;
        //public string AdminKeyHash = string.Empty;
        //public bool AdminInPlayer = false;

        public int SaltFrequency = 300;
        public string AddressPing = "8.8.8.8";
        //public string ServerHTTPS = "https://www.my-web-site.com/";
        //public bool AlwaysUseSSL = true;
        public bool AlwaysSecureData = false;
        //public string FacebookAppID = string.Empty;
        //public string FacebookAppSecret = string.Empty;
        //public string GoogleAppKey = string.Empty;
        //public string UnityAppKey = string.Empty;
        //public string TwitterAppKey = string.Empty;
        public int BuildTimestamp = 0;
        public int TokenHistoric = 6;
        public string AppName = "MyGameApp";
        public string PreProdTimeFormat = "_yyyy_MM_dd_HHmmss"; // TODO: rename
        public string AppProtocol = "MyGameApp://";
        public int EditorWebTimeOut = 600;
        public int WebTimeOut = 10;

        public float SpeedOfGameTime = 1.0F;
        //public string Version = "0.00.00";

        //public string AccountsForTests = string.Empty;

        public string BuildDate = string.Empty;

        public Color CartridgeColor = new Color(1.0F, 1.0F, 1.0F);

        public bool ThreadPoolForce = true;
        public NWDWritingMode WritingModeLocal = NWDWritingMode.QueuedMainThread;
        public NWDWritingMode WritingModeWebService = NWDWritingMode.QueuedMainThread;
        public NWDWritingMode WritingModeEditor = NWDWritingMode.QueuedMainThread;
        //-------------------------------------------------------------------------------------------------------------
        public string GetServerDNS()
        {
            //NWEBenchmark.Start();
            //string rReturn = ServerHTTPS;
            string rReturn = GetConfigurationServerHTTPS();
            NWDAccountInfos tAccountInfos = NWDAccountInfos.CurrentData();
            if (tAccountInfos != null)
            {
                if (tAccountInfos.Server == null)
                {
                    tAccountInfos.Server = new NWDReferenceType<NWDServerDomain>();
                }
                NWDServerDomain tServer = tAccountInfos.Server.GetReachableData();
                if (tServer != null)
                {
                    rReturn = tServer.ServerDNS;
                }
            }
            rReturn = NWDToolbox.CleanDNS(rReturn);
            //NWEBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetServerHTTPS()
        {
            //string rReturn = ServerHTTPS;
            //string rReturn = GetConfigurationServerHTTPS();
            //NWDAccountInfos tAccountInfos = NWDAccountInfos.CurrentData();
            //if (tAccountInfos != null)
            //{
            //    if (tAccountInfos.Server == null)
            //    {
            //        tAccountInfos.Server = new NWDReferenceType<NWDServerDomain>();
            //    }
            //    NWDServerDomain tServer = tAccountInfos.Server.GetReachableData();
            //    if (tServer != null)
            //    {
            //        rReturn = tServer.ServerDNS;
            //    }
            //}
            //if (AlwaysUseSSL == true)
            //{
                return "https://" + GetServerDNS();
            //}
            //else
            //{
            //    return "http://" + GetServerDNS();
            //}
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetConfigurationServerHTTPS()
        {
            //return "https://" + NWDToolbox.CleanDNS(ServerHTTPS);
            string rReturn = "https://localhost";
            foreach (NWDServerDomain tDomain in NWDBasisHelper.GetRawDatas<NWDServerDomain>())
            {
                if (tDomain.ValidInEnvironment(this))
                {
                    //if (AlwaysUseSSL == true)
                    //{
                        rReturn = "https://" + NWDToolbox.CleanDNS(tDomain.ServerDNS);
                    //}
                    //else
                    //{
                    //    rReturn = "http://" + NWDToolbox.CleanDNS(tDomain.ServerDNS);
                    //}
                    break;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDServerDomain> GetServerDNSList()
        {
            List<NWDServerDomain> rList = new List<NWDServerDomain>();
            foreach (NWDServerDomain tDomain in NWDBasisHelper.GetRawDatas<NWDServerDomain>())
            {
                if (tDomain.ValidInEnvironment(this))
                {
                    rList.Add(tDomain);
                }
            }
            return rList;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        #region constructor
        //-------------------------------------------------------------------------------------------------------------
        public NWDAppEnvironment()
        {
            //SetAccountReference(NWDToolbox.GenerateUniqueAccountID(this));
            FormatVerification();
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAppEnvironment(string sEnvironement, bool sSelected)
        {
            //NWEBenchmark.Start();
            this.Environment = sEnvironement;
            this.Selected = sSelected;
            FormatVerification();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAppEnvironment SelectedEnvironment()
        {
            return NWDAppConfiguration.SharedInstance().SelectedEnvironment();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SetEnvironment(NWDAppEnvironment sAppEnvironment)
        {
            if (NWDAppEnvironment.SelectedEnvironment() != sAppEnvironment)
            {
                foreach (NWDAppEnvironment tEnv in NWDAppConfiguration.SharedInstance().AllEnvironements())
                {
                    tEnv.Selected = false;
                }
                sAppEnvironment.Selected = true;
#if UNITY_EDITOR
                NWDAppEnvironmentChooser.Refresh();
#endif
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        #region instance methods
        //-------------------------------------------------------------------------------------------------------------
        public bool CurrentAccountIsCertified()
        {
            return NWDAccount.AccountIsCertified(GetAccountReference());
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CleanSecretKeyDevice()
        {
            WithSpecialSDKI = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string SecretKeyDevice()
        {
            string rReturn = "Hacker?";
            switch (NWDLauncher.CompileAs())
            {
                case NWDCompileType.Editor:
                    {
                        rReturn = SecretKeyDeviceEditor();
                    }
                    break;
                case NWDCompileType.PlayMode:
                    {
                        rReturn = SecretKeyDevicePlayer();
                    }
                    break;
                case NWDCompileType.Runtime:
                    {
                        rReturn = SecretKeyDevicePlayer();
                    }
                    break;
            }
            if (string.IsNullOrEmpty(WithSpecialSDKI) == false)
            {
                rReturn = WithSpecialSDKI;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string SecretKeyDeviceEditor()
        {
            string rReturn;
            rReturn = NWESecurityTools.GenerateSha(SystemInfo.deviceUniqueIdentifier + SaltStart);
#if UNITY_INCLUDE_TESTS
            if (NWDUnitTests.IsFakeDevice())
            {
                rReturn = NWDUnitTests.GetFakeDeviceEditor();
            }
#endif
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        const string kSecretKeyDevicePlayerKey = "kSecretKeyDevicePlayerKey_dad42928";
        public const int kSecretKeyDevicePlayerLength = 36;
        //-------------------------------------------------------------------------------------------------------------
        public string SecretKeyDevicePlayer()
        {
            string rReturn;
            if (NWDAppConfiguration.SharedInstance().AnonymousDeviceConnected == true)
            {
                rReturn = NWESecurityTools.GenerateSha(SystemInfo.deviceUniqueIdentifier + SaltEnd);
            }
            else
            {
                rReturn = NWEPrefsManager.ShareInstance().getString(kSecretKeyDevicePlayerKey, NWDToolbox.RandomStringUnix(kSecretKeyDevicePlayerLength));
            }
#if UNITY_INCLUDE_TESTS
            if (NWDUnitTests.IsFakeDevice())
            {
                rReturn = NWDUnitTests.GetFakeDevicePlayer();
            }
#endif
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SecretKeyDevicePlayerReset()
        {
            NWEPrefsManager.ShareInstance().set(kSecretKeyDevicePlayerKey, NWDToolbox.RandomStringUnix(kSecretKeyDevicePlayerLength));
#if UNITY_INCLUDE_TESTS
            if (NWDUnitTests.IsFakeDevice())
            {
                NWDUnitTests.FakeDevicePlayerReset();
            }
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        //public string AdminKeyHashGenerate()
        //{
        //    return NWESecurityTools.GenerateSha("455" + AdminKey + "gytf");
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void FormatVerification()
        {
            //NWEBenchmark.Start();
#if UNITY_EDITOR
            //NWEBenchmark.Start("eee");
            // Debug.Log ("VerifySecurity");
            // clean the salts
            DataSHAPassword = NWDToolbox.SaltCleaner(DataSHAPassword);
            DataSHAVector = NWDToolbox.SaltCleaner(DataSHAVector);
            SaltStart = NWDToolbox.SaltCleaner(SaltStart);
            SaltEnd = NWDToolbox.SaltCleaner(SaltEnd);
            //NWEBenchmark.Finish("eee");


            //SaltServer = NWDToolbox.SaltCleaner(SaltServer);
            // ServerPassword = NWDToolbox.SaltCleaner (ServerPassword);
            //AdminKey = NWDToolbox.SaltCleaner(AdminKey);
            //check salts are not mull
            if (DataSHAPassword == string.Empty)
            {
                DataSHAPassword = NWDToolbox.RandomString(16);
            }
            if (DataSHAVector == string.Empty)
            {
                DataSHAVector = NWDToolbox.RandomString(16);
            }
            if (SaltStart == string.Empty)
            {
                SaltStart = NWDToolbox.RandomString(16);
            }
            if (SaltEnd == string.Empty)
            {
                SaltEnd = NWDToolbox.RandomString(16);
            }
            //if (SaltServer == string.Empty)
            //{
            //    SaltServer = NWDToolbox.RandomString(16);
            //}
            if (SaltFrequency <= 400)
            {
                SaltFrequency = UnityEngine.Random.Range(400, 800);
            }
            //if (ServerPassword == string.Empty)
            //{
            //    ServerPassword = NWDToolbox.RandomString(16);
            //}
            //if (AdminKey == string.Empty)
            //{
            //    AdminKey = NWDToolbox.RandomString(16);
            //}
            if (TokenHistoric < 1 || TokenHistoric > 10)
            {
                TokenHistoric = 3;
            }
#endif
            //NWEBenchmark.Finish(true, Environment);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Absolutes the date time in game time. With additional year.
        /// </summary>
        /// <returns>The date time in game time.</returns>
        public DateTime AbsoluteDateTimeInGameTime()
        {
            float tSpeedOfGameTime = SpeedOfGameTime;
            DateTime tNow = DateTime.Now;
            DateTime tNowTwo = new DateTime(tNow.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            float tTimestamp = (float)tNow.Subtract(tNowTwo).TotalSeconds;
            if (tSpeedOfGameTime > 0 && tSpeedOfGameTime < 1000)
            {
                tTimestamp = tTimestamp * tSpeedOfGameTime;
            }
            DateTime tDateInGame = new DateTime(tNow.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            tDateInGame = tDateInGame.AddSeconds(tTimestamp);
            return tDateInGame;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Date the time in game time. Without additional year (loop in the same year… paradoxal seasons)
        /// </summary>
        /// <returns>The time in game time.</returns>
        public DateTime DateTimeInGameTime()
        {
            float tSpeedOfGameTime = SpeedOfGameTime;
            DateTime tNow = DateTime.Now;
            DateTime tNowTwo = new DateTime(tNow.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            float tTimestamp = (float)tNow.Subtract(tNowTwo).TotalSeconds;
            if (tSpeedOfGameTime > 0 && tSpeedOfGameTime < 1000)
            {
                tTimestamp = tTimestamp * tSpeedOfGameTime;
            }
            DateTime tDateInGame = new DateTime(tNow.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            tDateInGame = tDateInGame.AddSeconds(tTimestamp);
            DateTime tDateInGameResult = new DateTime(tNow.Year, tDateInGame.Month, tDateInGame.Day, tDateInGame.Hour, tDateInGame.Minute, tDateInGame.Second, DateTimeKind.Utc);
            return tDateInGameResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static float kSunRotationPerSeconds = 0.0014666667F; // ((360 /24)/60)/60 : 360° div by 24 hours, div by 60 minutes, div by 60 seconds : angle for one second
        //-------------------------------------------------------------------------------------------------------------
        public float RotationOfSunInGameTime()
        {
            //TODO Test this solution 
            DateTime tNow = DateTime.Now;
            int tSeconds = tNow.Hour * 3600 + tNow.Minute * 60 + tNow.Second;
            return kSunRotationPerSeconds * tSeconds * SpeedOfGameTime;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================