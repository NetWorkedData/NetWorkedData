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
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;



#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDRequestToken : NWDBasisAccountRestricted
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDRequestToken()
        {
            //Debug.Log("NWDRequestToken Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDRequestToken(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDRequestToken Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        //static NWDRequestToken kCurrent = null;
        //-------------------------------------------------------------------------------------------------------------
        //public static NWDRequestToken GetRequestTokenOrCreate()
        //{
        //    if (kCurrent != null)
        //    {
        //        if (kCurrent.Account.GetValue() != NWDAccount.CurrentReference())
        //        {
        //            kCurrent = null;
        //        }
        //    }
        //    if (kCurrent == null)
        //    {
        //        NWDRequestToken tResquestToken = NWDBasisHelper.GetCorporateFirstData<NWDRequestToken>(NWDAccount.CurrentReference(), null);
        //        if (tResquestToken == null)
        //        {
        //            NWDAppEnvironment tAppEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
        //            tResquestToken = NWDBasisHelper.NewData<NWDRequestToken>();
        //            tResquestToken.Account.SetValue( NWDAccount.CurrentReference());
        //            tResquestToken.Tag = NWDBasisTag.TagUserCreated;
        //            tResquestToken.SaveData();
        //        }
        //        kCurrent = tResquestToken;
        //    }
        //    return kCurrent;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //static public string GetToken()
        //{
        //    NWDRequestToken tToken = GetRequestTokenOrCreate();
        //    return tToken.Token;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //static public void SetToken(string sNewToken)
        //{
        //    NWDRequestToken tToken = GetRequestTokenOrCreate();
        //    tToken.Token = sNewToken;
        //    tToken.SaveData();
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
