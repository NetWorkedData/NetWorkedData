// =====================================================================================================================
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
// =====================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountSign : NWDBasis<NWDAccountSign>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountSign()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountSign(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
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
        public void RegisterDeviceEditor()
        {
            SignType = NWDAccountSignType.DeviceID;
            SignHash = SignDeviceEditor();
            RescueHash = string.Empty;
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
            NWDAccount tAccount = NWDAccount.GetRawDataByReference(Account.GetReference());
            if (tAccount != null)
            {
                InternalKey = tAccount.InternalKey;
            }
            InternalDescription = "Editor Device ID <" + tEnv.Environment + "> " + SignHash;
#endif
            Register();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string SignDeviceEditor()
        {
            return NWDAppEnvironment.SelectedEnvironment().SecretKeyDeviceEditor();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RegisterDevicePlayer()
        {
            SignType = NWDAccountSignType.DeviceID;
            SignHash = NWDAppEnvironment.SelectedEnvironment().SecretKeyDevicePlayer();
            RescueHash = string.Empty;
#if UNITY_EDITOR
            NWDAccount tAccount = NWDAccount.GetRawDataByReference(Account.GetReference());
            if (tAccount != null)
            {
                InternalKey = tAccount.InternalKey;
            }
            InternalDescription = "Player Device ID : " + SignHash;
#endif
            Register();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string SignDevicePlayer()
        {
            return NWDAppEnvironment.SelectedEnvironment().SecretKeyDevicePlayer();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RegisterDevice()
        {
            SignType = NWDAccountSignType.DeviceID;
            SignHash = NWDAppEnvironment.SelectedEnvironment().SecretKeyDevice();
            RescueHash = string.Empty;
#if UNITY_EDITOR
            NWDAccount tAccount = NWDAccount.GetRawDataByReference(Account.GetReference());
            if (tAccount != null)
            {
                InternalKey = tAccount.InternalKey;
            }
            InternalDescription = "Device ID : " + SignHash;
#endif
            Register();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string SignDevice()
        {
            return NWDAppEnvironment.SelectedEnvironment().SecretKeyDevice();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RegisterSocialFacebook(string sSocialToken)
        {
            SignType = NWDAccountSignType.Facebook;
            SignHash = SignSocialDevice(sSocialToken);
            RescueHash = string.Empty;
#if UNITY_EDITOR
            NWDAccount tAccount = NWDAccount.GetRawDataByReference(Account.GetReference());
            if (tAccount != null)
            {
                InternalKey = tAccount.InternalKey;
            }
            InternalDescription = "facebook ID : " + SignHash;
#endif
            Register();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RegisterSocialGoogle(string sSocialToken)
        {
            SignType = NWDAccountSignType.Google;
            SignHash = SignSocialDevice(sSocialToken);
            RescueHash = string.Empty;
#if UNITY_EDITOR
            NWDAccount tAccount = NWDAccount.GetRawDataByReference(Account.GetReference());
            if (tAccount != null)
            {
                InternalKey = tAccount.InternalKey;
            }
            InternalDescription = "google ID : " + SignHash;
#endif
            Register();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string SignSocialDevice(string sSocialToken)
        {
            string rReturn = null;
            if (string.IsNullOrEmpty(sSocialToken))
            {
                rReturn = string.Empty;
            }
            else
            {
                rReturn = BTBSecurityTools.GenerateSha(sSocialToken + NWDAppEnvironment.SelectedEnvironment().SaltEnd);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RegisterEmailPassword(string sEmail, string sPassword)
        {
            if (string.IsNullOrEmpty(sEmail) || string.IsNullOrEmpty(sPassword))
            {
                // not possible
            }
            else
            {
                SignType = NWDAccountSignType.LoginPassword;
                SignHash = SignLoginPassword(sEmail, sPassword);
                RescueHash = RescueEmailHash(sEmail);
#if UNITY_EDITOR
                NWDAccount tAccount = NWDAccount.GetRawDataByReference(Account.GetReference());
                if (tAccount != null)
                {
                    InternalKey = tAccount.InternalKey;
                }
                InternalDescription = "Login Password : " + sEmail + " / " + sPassword;
#endif
                Register();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void CreateAndRegisterEmail(string sEmail, string sPassword, BTBOperationBlock sSuccessBlock = null, BTBOperationBlock sErrorBlock = null)
        {
            NWDAccountSign tSign = NewData();
            tSign.RegisterEmailPassword(sEmail, sPassword);

            /*BTBOperationBlock tSuccess = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bResult)
            {
                if (synchronizeBlockDelegate != null)
                {
                    NWDOperationResult tResult = bResult as NWDOperationResult;
                    synchronizeBlockDelegate(false, tResult);
                }
            };
            BTBOperationBlock tFailed = delegate (BTBOperation bOperation, float bProgress, BTBOperationResult bResult)
            {
                if (synchronizeBlockDelegate != null)
                {
                    NWDOperationResult tResult = bResult as NWDOperationResult;
                    synchronizeBlockDelegate(true, tResult);
                }
            };*/

            // Sync NWDAccountSign
            SynchronizationFromWebService(sSuccessBlock, sErrorBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string SignLoginPassword(string sLogin, string sPassword)
        {
            string rReturn = null;
            if (string.IsNullOrEmpty(sLogin) || string.IsNullOrEmpty(sPassword))
            {
                rReturn = string.Empty;
            }
            else
            {
                rReturn = BTBSecurityTools.GenerateSha(sLogin + sPassword + NWDAppEnvironment.SelectedEnvironment().SaltEnd);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string RescueEmailHash(string sEmail)
        {
            string rReturn = null;
            if (string.IsNullOrEmpty(sEmail))
            {
                rReturn = string.Empty;
            }
            else
            {
                rReturn = BTBSecurityTools.GenerateSha(sEmail + NWDAppEnvironment.SelectedEnvironment().SaltStart);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RegisterDelete()
        {
            SignType = NWDAccountSignType.None;
            SignHash = string.Empty;
            RescueHash = string.Empty;
            Unregister();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAccountSign[] GetCorporateDatasAssociated(string sAccountReference = null)
        {
            List<NWDAccountSign> tSignList = GetCorporateDatasList(sAccountReference);
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
            List<NWDAccountSign> tSignList = GetReachableDatasList();
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
            List<NWDAccountSign> tSignList = GetReachableDatasList();
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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================