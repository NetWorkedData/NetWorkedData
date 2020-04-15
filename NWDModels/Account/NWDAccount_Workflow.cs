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
    public enum NWDAccountEnvironment : int
    {
        InGame = 0, // player state (Prod)
        Dev = 1,    // dev state
        Preprod = 2, // preprod state
        Prod = 3, //NEVER COPY ACCOUNT IN PROD !!!!
        None = 9,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccount : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccount() { }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccount(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData) { }
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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
