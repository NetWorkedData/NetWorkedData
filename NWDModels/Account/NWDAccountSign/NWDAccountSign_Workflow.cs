//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:16
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountSign : NWDBasisAccountDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountSign() { }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountSign(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData) { }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() { }
        //=============================================================================================================
        // PUBLIC METHOD
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonTrashMe()
        {
            SignStatus = NWDAccountSignAction.TryToDissociate;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Register()
        {
            SignStatus = NWDAccountSignAction.TryToAssociate;
            UpdateData();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Unregister()
        {
            SignStatus = NWDAccountSignAction.TryToDissociate;
            UpdateData();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RegisterDelete()
        {
            SignType = NWDAccountSignType.None;
            SignHash = string.Empty;
            RescueHash = string.Empty;
            LoginHash = string.Empty;
            Unregister();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RegisterDeviceEditor()
        {
            SignType = NWDAccountSignType.DeviceID;
            SignHash = SignDeviceEditor();
            RescueHash = string.Empty;
            LoginHash = string.Empty;
            Tag = NWDBasisTag.TagDeviceCreated;
#if UNITY_EDITOR
            NWDAppEnvironment tEnv = null;
            if (DevSync < 0)
            {
                tEnv = NWDAppConfiguration.SharedInstance().PreprodEnvironment;
            }
            else
            {
                tEnv = NWDAppConfiguration.SharedInstance().DevEnvironment;
            }
            SignHash = tEnv.SecretKeyDeviceEditor();
            SetInternalKey();
            InternalDescription = "Editor Device ID <" + tEnv.Environment + "> " + SignHash;
#endif
            Register();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RegisterDevicePlayer()
        {
            SignType = NWDAccountSignType.DeviceID;
            SignHash = SignDevicePlayer();
            RescueHash = string.Empty;
            LoginHash = string.Empty;
            Tag = NWDBasisTag.TagDeviceCreated;
#if UNITY_EDITOR
            SetInternalKey();
            InternalDescription = "Player Device ID : " + SignHash;
#endif
            Register();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RegisterDevice()
        {
            SignType = NWDAccountSignType.DeviceID;
            SignHash = NWDAppEnvironment.SelectedEnvironment().SecretKeyDevice();
            RescueHash = string.Empty;
            LoginHash = string.Empty;
            Tag = NWDBasisTag.TagDeviceCreated;
#if UNITY_EDITOR
            SetInternalKey();
            InternalDescription = "Device ID : " + SignHash;
#endif
            Register();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RegisterSocialNetwork(string sSocialID, NWDAccountSignType sSocialType)
        {
            if (string.IsNullOrEmpty(sSocialID))
            {
                // Not possible
            }
            else
            {
                SignType = sSocialType;
                SignHash = GetSignSocialHash(sSocialID);
                RescueHash = string.Empty;
                LoginHash = string.Empty;
                Tag = NWDBasisTag.TagUserCreated;
#if UNITY_EDITOR
                SetInternalKey();
                InternalDescription = "" + sSocialType + " ID : " + SignHash;
#endif
                Register();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RegisterEmailLoginPassword(string sEmail, string sLogin, string sPassword)
        {
            if (string.IsNullOrEmpty(sEmail) || string.IsNullOrEmpty(sPassword) || string.IsNullOrEmpty(sLogin))
            {
                // Not possible
            }
            else
            {
                SignType = NWDAccountSignType.LoginPasswordEmail;
                SignHash = GetSignLoginPasswordHash(sLogin, sPassword);
                RescueHash = GetRescueEmailHash(sEmail);
                LoginHash = GetLoginHash(sLogin);
                Tag = NWDBasisTag.TagUserCreated;
#if UNITY_EDITOR
                SetInternalKey();
                InternalDescription = "Login Password Email : " + sLogin + "/" + sPassword + " / " + sEmail;
#endif
                Register();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RegisterEmailPassword(string sEmail, string sPassword)
        {
            if (string.IsNullOrEmpty(sEmail) || string.IsNullOrEmpty(sPassword))
            {
                // Not possible
            }
            else
            {
                SignType = NWDAccountSignType.EmailPassword;
                SignHash = GetSignEmailPasswordHash(sEmail, sPassword);
                RescueHash = GetRescueEmailHash(sEmail);
                LoginHash = string.Empty;
                Tag = NWDBasisTag.TagUserCreated;
#if UNITY_EDITOR
                SetInternalKey();
                InternalDescription = "Email / Password: " + sEmail + " / " + sPassword;
#endif
                Register();
            }
        }
        //=============================================================================================================
        // PUBLIC STATIC METHOD
        //-------------------------------------------------------------------------------------------------------------
        public static string SignDeviceEditor()
        {
            return NWDAppEnvironment.SelectedEnvironment().SecretKeyDeviceEditor();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string SignDevicePlayer()
        {
            return NWDAppEnvironment.SelectedEnvironment().SecretKeyDevicePlayer();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string SignDevice()
        {
            return NWDAppEnvironment.SelectedEnvironment().SecretKeyDevice();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void CreateAndRegisterSocialNetwork(string sSocialID, NWDAccountSignType sSocialType, NWEOperationBlock sSuccessBlock = null, NWEOperationBlock sErrorBlock = null)
        {
            // Generate Hash with email and password
            string tSignHash = NWDAccountSign.GetSignSocialHash(sSocialID);
            bool tResult = NWDAccountSign.CheckReacheableAccountSign(tSignHash, sSocialType);

            if (!tResult)
            {
                NWDAccountSign tSign = NWDBasisHelper.NewData<NWDAccountSign>();
                tSign.RegisterSocialNetwork(sSocialID, sSocialType);

                // Sync NWDAccountSign
                NWDBasisHelper.SynchronizationFromWebService<NWDAccountSign>(sSuccessBlock, sErrorBlock);
            }
            else
            {
                sErrorBlock?.Invoke(null);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void CreateAndRegisterLoginPasswordEmail(string sLogin, string sPassword, string sEmail, NWEOperationBlock sSuccessBlock = null, NWEOperationBlock sErrorBlock = null)
        {
            // Generate Hash with email and password
            string tSignHash = NWDAccountSign.GetSignLoginPasswordHash(sLogin, sPassword);
            bool tResult = NWDAccountSign.CheckReacheableAccountSign(tSignHash, NWDAccountSignType.LoginPasswordEmail);

            if (!tResult)
            {
                NWDAccountSign tSign = NWDBasisHelper.NewData<NWDAccountSign>();
                tSign.RegisterEmailLoginPassword(sEmail, sLogin, sPassword);

                // Sync NWDAccountSign
                NWDBasisHelper.SynchronizationFromWebService<NWDAccountSign>(sSuccessBlock, sErrorBlock);
            }
            else
            {
                sErrorBlock?.Invoke(null);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void CreateAndRegisterEmailPassword(string sEmail, string sPassword, NWEOperationBlock sSuccessBlock = null, NWEOperationBlock sErrorBlock = null)
        {
            // Generate Hash with email and password
            string tSignHash = NWDAccountSign.GetSignEmailPasswordHash(sEmail, sPassword);
            bool tResult = NWDAccountSign.CheckReacheableAccountSign(tSignHash, NWDAccountSignType.EmailPassword);

            if (!tResult)
            {
                NWDAccountSign tSign = NWDBasisHelper.NewData<NWDAccountSign>();
                tSign.RegisterEmailPassword(sEmail, sPassword);

                // Sync NWDAccountSign
                NWDBasisHelper.SynchronizationFromWebService<NWDAccountSign>(sSuccessBlock, sErrorBlock);
            }
            else
            {
                sErrorBlock?.Invoke(null);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetSignSocialHash(string sSocialID)
        {
            string rReturn = null;
            if (string.IsNullOrEmpty(sSocialID))
            {
                rReturn = string.Empty;
            }
            else
            {
                rReturn = NWESecurityTools.GenerateSha(sSocialID + NWDAppEnvironment.SelectedEnvironment().SaltEnd);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetSignLoginPasswordHash(string sLogin, string sPassword)
        {
            string rReturn = null;
            if (string.IsNullOrEmpty(sLogin) || string.IsNullOrEmpty(sPassword))
            {
                rReturn = string.Empty;
            }
            else
            {
                rReturn = NWESecurityTools.GenerateSha(sLogin + sPassword + NWDAppEnvironment.SelectedEnvironment().SaltEnd);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetSignEmailPasswordHash(string sEmail, string sPassword)
        {
            string rReturn = null;
            if (string.IsNullOrEmpty(sEmail) || string.IsNullOrEmpty(sPassword))
            {
                rReturn = string.Empty;
            }
            else
            {
                rReturn = NWESecurityTools.GenerateSha(sEmail.ToLower() + sPassword + NWDAppEnvironment.SelectedEnvironment().SaltEnd);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetRescueEmailHash(string sEmail)
        {
            string rReturn = null;
            if (string.IsNullOrEmpty(sEmail))
            {
                rReturn = string.Empty;
            }
            else
            {
                rReturn = NWESecurityTools.GenerateSha(sEmail.ToLower() + NWDAppEnvironment.SelectedEnvironment().SaltStart);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetLoginHash(string sLogin)
        {
            string rReturn = null;
            if (string.IsNullOrEmpty(sLogin))
            {
                rReturn = string.Empty;
            }
            else
            {
                rReturn = NWESecurityTools.GenerateSha(sLogin.ToUpper() + NWDAppEnvironment.SelectedEnvironment().SaltStart);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAccountSign[] GetCorporateDatasAssociated(string sAccountReference = null)
        {
            List<NWDAccountSign> tSignList = NWDBasisHelper.GetCorporateDatasList<NWDAccountSign>(sAccountReference);
            List<NWDAccountSign> rReturn = new List<NWDAccountSign>();
            foreach (NWDAccountSign tSign in tSignList)
            {
                if (tSign.SignStatus == NWDAccountSignAction.Associated)
                {
                    rReturn.Add(tSign);
                }
            }
            return rReturn.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAccountSign[] GetReachableDatasAssociated()
        {
            List<NWDAccountSign> tSignList = NWDBasisHelper.GetReachableDatasList<NWDAccountSign>();
            List<NWDAccountSign> rReturn = new List<NWDAccountSign>();
            foreach (NWDAccountSign tSign in tSignList)
            {
                if (tSign.SignStatus == NWDAccountSignAction.Associated)
                {
                    rReturn.Add(tSign);
                }
            }
            return rReturn.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAccountSign[] GetReachableDatasBySignType(NWDAccountSignType tSignType)
        {
            List<NWDAccountSign> tSignList = NWDBasisHelper.GetReachableDatasList<NWDAccountSign>();
            List<NWDAccountSign> rReturn = new List<NWDAccountSign>();
            foreach (NWDAccountSign tSign in tSignList)
            {
                if (tSign.SignType == tSignType)
                {
                    rReturn.Add(tSign);
                }
            }
            return rReturn.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool CheckReacheableAccountSign(string sSignHash, NWDAccountSignType sAccountType)
        {
            bool rResult = false;
            NWDAccountSign[] tSigns = NWDBasisHelper.GetReachableDatas<NWDAccountSign>();
            foreach (NWDAccountSign k in tSigns)
            {
                if (k.SignType == sAccountType)
                {
                    if (k.SignHash.Equals(sSignHash) || sSignHash.Equals(""))
                    {
                        rResult = true;
                        break;
                    }
                }
            }
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAccountSign GetReacheableAccountSign(string sSignHash, NWDAccountSignType sAccountType)
        {
            NWDAccountSign rResult = null;
            NWDAccountSign[] tSigns = NWDBasisHelper.GetReachableDatas<NWDAccountSign>();
            foreach (NWDAccountSign k in tSigns)
            {
                if (k.SignType == sAccountType)
                {
                    if (k.SignHash.Equals(sSignHash) || sSignHash.Equals(""))
                    {
                        rResult = k;
                        break;
                    }
                }
            }
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAccountSign GetFirstReacheableAccountSign(NWDAccountSignType sAccountType)
        {
            NWDAccountSign[] tSigns = NWDBasisHelper.GetReachableDatas<NWDAccountSign>();
            foreach (NWDAccountSign k in tSigns)
            {
                if (k.SignType == sAccountType)
                {
                    return k;
                }
            }

            return null;
        }
        //-------------------------------------------------------------------------------------------------------------
        void SetInternalKey()
        {
            NWDAccount tAccount = NWDBasisHelper.GetRawDataByReference<NWDAccount>(Account.GetReference());
            if (tAccount != null && !string.IsNullOrEmpty(tAccount.InternalKey))
            {
                InternalKey = tAccount.InternalKey + " (" + SignStatus.ToString() + ")";
            }
            else
            {
                InternalKey = "(" + SignStatus.ToString() + ")";
            }
            Tag = NWDBasisTag.TagAdminCreated;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================