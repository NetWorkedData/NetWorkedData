﻿//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:33
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
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
using SQLite.Attribute;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDRequestToken : NWDBasis
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
                if (kCurrent.UUIDHash.GetReference() != NWDAccount.CurrentReference())
                {
                    kCurrent = null;
                }
            }
            if (kCurrent == null)
            {
                NWDRequestToken tResquestToken = NWDBasisHelper.GetCorporateFirstData<NWDRequestToken>(NWDAccount.CurrentReference(), null);
                if (tResquestToken == null)
                {
                    NWDAppEnvironment tAppEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
                    tResquestToken = NWDBasisHelper.NewData<NWDRequestToken>();
                    tResquestToken.UUIDHash.SetReference(NWDAccount.CurrentReference());
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
