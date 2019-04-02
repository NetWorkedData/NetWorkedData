//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

using SQLite4Unity3d;

using BasicToolBox;
using SQLite.Attribute;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDRequestToken : NWDBasis<NWDRequestToken>
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
        static NWDRequestToken kCurrent = null;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDRequestToken GetRequestTokenOrCreate()
        {
            if (kCurrent != null)
            {
                if (kCurrent.UUIDHash.GetReference() != NWDAccount.GetCurrentAccountReference())
                {
                    kCurrent = null;
                }
            }
            if (kCurrent == null)
            {
                NWDRequestToken tResquestToken = GetFirstData(NWDAccount.GetCurrentAccountReference(), null);
                if (tResquestToken == null)
                {
                    NWDAppEnvironment tAppEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
                    tResquestToken = NewData();
                    tResquestToken.UUIDHash.SetReference(NWDAccount.GetCurrentAccountReference());
                    tResquestToken.Tag = NWDBasisTag.TagUserCreated;
                    tResquestToken.SaveData();
                }
                kCurrent = tResquestToken;
            }
            return kCurrent;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public string GetToken()
        {
            NWDRequestToken tToken = GetRequestTokenOrCreate();
            return tToken.Token;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void SetToken(string sNewToken)
        {
            NWDRequestToken tToken = GetRequestTokenOrCreate();
            tToken.Token = sNewToken;
            tToken.SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
