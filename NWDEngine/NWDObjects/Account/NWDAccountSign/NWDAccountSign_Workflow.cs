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

//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================

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
        // Declare your static properties and private here
        //-------------------------------------------------------------------------------------------------------------
        static public void ClassMethodExample()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public void InstanceMethodExample()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountSign()
        {
            //Debug.Log("NWDAccountSign Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountSign(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDAccountSign Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance Initialization
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Register()
        {
            SignAction = NWDAccountSignAction.TryToAssociate;
            UpdateData();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Unregister()
        {
            SignAction = NWDAccountSignAction.TryToDissociate;
            UpdateData();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RegisterDeviceEditor()
        {
            SignType = NWDAccountSignType.DeviceID;
            SignHash = SignDeviceEditor();
            RescueHash = string.Empty;
#if UNITY_EDITOR
            NWDAccount tAccount = NWDAccount.GetRawDataByReference(Account.GetReference());
            if (tAccount != null)
            {
                InternalKey = tAccount.InternalKey;
            }
            InternalDescription = "Editor Device ID : " + SignHash;
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
                if (tSign.SignAction == NWDAccountSignAction.Associated)
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
                if (tSign.SignAction == NWDAccountSignAction.Associated)
                {
                    rReturn.Add(tSign);
                }
            }
            return rReturn.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static NWDAccountSign[] GetCorporateDatasBySignType(NWDAccountSignType tSignType, string sAccountReference = null)
        //{
        //    List<NWDAccountSign> tSignList = GetCorporateDatasList(sAccountReference);
        //    List<NWDAccountSign> rReturn = new List<NWDAccountSign>();
        //    foreach (NWDAccountSign tSign in tSignList)
        //    {
        //        if (tSign.SignType == tSignType)
        //        {
        //            rReturn.Add(tSign);
        //        }
        //    }
        //    return rReturn.ToArray();
        //}
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
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region NetWorkedData addons methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just after loaded from database.
        /// </summary>
        public override void AddonLoadedMe()
        {
            // do something when object was loaded
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before unload from memory.
        /// </summary>
        public override void AddonUnloadMe()
        {
            // do something when object will be unload
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before insert.
        /// </summary>
        public override void AddonInsertMe()
        {
            // do something when object will be inserted
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before update.
        /// </summary>
        public override void AddonUpdateMe()
        {
            // do something when object will be updated
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method when updated.
        /// </summary>
        public override void AddonUpdatedMe()
        {
            // do something when object finish to be updated
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method when updated me from Web.
        /// </summary>
        //public override void AddonUpdatedMeFromWeb()
        //{
        //    // do something when object finish to be updated from CSV from WebService response
        //    // TODO verif if method is call in good place in good timing
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before dupplicate.
        /// </summary>
        public override void AddonDuplicateMe()
        {
            // do something when object will be dupplicate
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before enable.
        /// </summary>
        public override void AddonEnableMe()
        {
            // do something when object will be enabled
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before disable.
        /// </summary>
        public override void AddonDisableMe()
        {
            // do something when object will be disabled
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before put in trash.
        /// </summary>
        public override void AddonTrashMe()
        {
            // do something when object will be put in trash
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before remove from trash.
        /// </summary>
        public override void AddonUnTrashMe()
        {
            // do something when object will be remove from trash
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons the delete me.
        /// </summary>
        public override void AddonDeleteMe()
        {
            // do something when object will be delete from local base
        }
        //-------------------------------------------------------------------------------------------------------------
        //public override bool AddonSyncForce()
        //{
        //    return false;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonWebversionUpgradeMe(int sOldWebversion, int sNewWebVersion)
        {
            // do something when object will be web service upgrade
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================