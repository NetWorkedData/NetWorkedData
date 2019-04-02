//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
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
    public partial class NWDAccount : NWDBasis<NWDAccount>
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
        public static string GetCurrentAccountReference()
        {
            return NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetCurrentAnonymousAccountReference()
        {
            return NWDAppConfiguration.SharedInstance().SelectedEnvironment().AnonymousPlayerAccountReference;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDAccounTest> GetTestsAccounts()
        {
            List<NWDAccounTest> rReturn = new List<NWDAccounTest>();
            string tValue = NWDAppConfiguration.SharedInstance().SelectedEnvironment().AccountsForTests;
            if (!string.IsNullOrEmpty(tValue))
            {
                string[] tValueArray = tValue.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tValueArrayLine in tValueArray)
                {
                    string[] tLineValue = tValueArrayLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                    if (tLineValue.Length == 2)
                    {
                        string tAccountKey = tLineValue[0];
                        string tText = tLineValue[1];
                        string[] tInfos = tText.Split(new string[] { NWDConstants.kFieldSeparatorC }, StringSplitOptions.RemoveEmptyEntries);
                        if (tInfos.Length == 3)
                        {
                            NWDAccounTest tAccount = new NWDAccounTest();
                            tAccount.InternalKey = tAccountKey;
                            tAccount.EmailHash = tInfos[0];
                            tAccount.PasswordHash = tInfos[1];
                            tAccount.Reference = tInfos[2];
                            rReturn.Add(tAccount);
                        }
                    }
                }
            }
            return rReturn;
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
