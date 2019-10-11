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


using System;
using System.Collections.Generic;
using UnityEngine;
//using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDAccounTest
    {
        public string Reference;
        public string InternalKey;
        public string EmailHash;
        public string PasswordHash;
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
    public partial class NWDAccount : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccount()
        {
            //Debug.Log("NWDAccount Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccount(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDAccount Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            UseInEnvironment = NWDAccountEnvironment.InGame;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
