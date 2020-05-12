//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:15
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

//=====================================================================================================================
using System;
using UnityEngine;

namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDAccountStatut
    {
        Temporary = 0,
        Certified = 1,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDAccountEnvironment : int
    {
        InGame = 0, // player state (Prod)
        Dev = 1,    // dev state
        Preprod = 2, // preprod state
        Prod = 3, //NEVER COPY ACCOUNT IN PROD !!!!
        None = 9,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccount : NWDBasisAccountRestricted
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccount() { }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccount(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData) { }
        //-------------------------------------------------------------------------------------------------------------
        public static bool AccountIsCertified(string sReference)
        {
            bool rReturn = true;
            if (sReference.EndsWith(NWDAccount.K_ACCOUNT_TEMPORARY_SUFFIXE) || sReference.EndsWith(NWDAccount.K_ACCOUNT_NEW_SUFFIXE))
            {
                rReturn = false;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void Check()
        {
            Account.SetValue(Reference);
            if (Reference.Contains(NWDAccount.K_ACCOUNT_FROM_EDITOR))
            {
                if (DevSync >= 0)
                {
                    InternalDescription = "Create in editor environment " + NWDAppConfiguration.SharedInstance().DevEnvironment.Environment;
                }
                if (PreprodSync >= 0)
                {
                    InternalDescription = "Create in editor environment " + NWDAppConfiguration.SharedInstance().PreprodEnvironment.Environment;
                }
                if (ProdSync >= 0)
                {
                    InternalDescription = "WHAT! Create in editor environment " + NWDAppConfiguration.SharedInstance().ProdEnvironment.Environment;
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonInsertedMe()
        {
            base.AddonInsertedMe();
            Check();
            UpdateDataIfModified();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            base.AddonUpdateMe();
            Check();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// New reference. If account dependent this UUID of Player Account is integrate in Reference generation
        /// </summary>
        /// <returns>The reference.</returns>
        public override string NewReference()
        {
            string rReturn = string.Empty;
#if UNITY_EDITOR
            string tRangeServer = NWDServerDatas.RangeEditor;
            // not necessary  beacuse editor account must be exceptional
            NWDServerDatabaseAuthentication[] tServerDatas = NWDServerDatas.GetAllConfigurationServerDatabase(NWDAppEnvironment.SelectedEnvironment());
            if (tServerDatas.Length > 1)
            {
                tServerDatas.ShuffleList();
            }
            if (tServerDatas.Length > 1)
            {
                NWDServerDatabaseAuthentication tServerData = tServerDatas[0];
                tRangeServer = UnityEngine.Random.Range(tServerData.RangeMin, tServerData.RangeMax).ToString();
            }
            bool tValid = false;
            while (tValid == false)
            {
                string tReferenceMiddle = NWDToolbox.AplhaNumericToNumeric(NWESecurityTools.GenerateSha("5r5" + SystemInfo.deviceUniqueIdentifier + "7z4").ToUpper().Substring(0, 9));

                rReturn = K_ACCOUNT_PREFIX_TRIGRAM + NWEConstants.K_MINUS + tRangeServer + NWEConstants.K_MINUS + tReferenceMiddle + NWEConstants.K_MINUS + UnityEngine.Random.Range(100000, 999999).ToString() + K_ACCOUNT_FROM_EDITOR;
                tValid = TestReference(rReturn);
            }
#endif
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            UseInEnvironment = NWDAccountEnvironment.InGame;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool AccountCanSignOut()
        {
            bool rReturn = true;
            foreach (NWDAccountSign tSign in NWDAccountSign.GetReachableDatasAssociated())
            {
                if (tSign.SignHash == NWDAppEnvironment.SelectedEnvironment().SecretKeyDevice())
                {
                    rReturn = false;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUnTrashMe()
        {
            AccountUnTrash(Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void AccountUnTrash(string sAccountReference)
        {
            NWEBenchmark.Start();
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeAccountDependantList)
            {
                if (tType != typeof(NWDAccount))
                {
                    foreach (NWDBasis tObject in NWDBasisHelper.FindTypeInfos(tType).Datas)
                    {
                        if (tObject.IsReacheableByAccount(sAccountReference))
                        {
                            if (tObject.UnTrashData())
                            {
                                Debug.Log("Data ref " + tObject.Reference + " put remove from trash");
                            }
                        }
                    }
                }
            }
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonTrashMe()
        {
            AccountTrash(Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void AccountTrash(string sAccountReference)
        {
            NWEBenchmark.Start();
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeAccountDependantList)
            {
                if (tType != typeof(NWDAccount))
                {
                    foreach (NWDBasis tObject in NWDBasisHelper.FindTypeInfos(tType).Datas)
                    {
                        if (tObject.IsReacheableByAccount(sAccountReference))
                        {
                            if (tObject.TrashData())
                            {
                                Debug.Log("Data ref " + tObject.Reference + " put in trash, ready to be sync and delete!");
                            }
                        }
                    }
                }
            }
            NWEBenchmark.Finish();
        }

        //-------------------------------------------------------------------------------------------------------------
        public static string GetUniqueReference(string UUID, Type sType)
        {
            string rReturn = UUID;
            rReturn = rReturn.TrimEnd(K_ACCOUNT_CERTIFIED_SUFFIXE.ToCharArray(0, 1));
            rReturn = rReturn.TrimEnd(K_ACCOUNT_FROM_EDITOR.ToCharArray(0, 1));
            //rReturn = rReturn.Replace(K_ACCOUNT_PREFIX_TRIGRAM + "-", "");
            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(sType);
            rReturn = rReturn.Replace(K_ACCOUNT_PREFIX_TRIGRAM, tHelper.ClassTrigramme);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetUniqueReferenceFromCurrentAccount<T>()
        {
            return GetUniqueReference(NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference, typeof(T));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
