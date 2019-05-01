// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:14
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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
    public partial class NWDAccount : NWDBasis<NWDAccount>
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string CurrentReference()
        {
            return NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static string CurrentAnonymousReference()
        //{
        //    return NWDAppConfiguration.SharedInstance().SelectedEnvironment().AnonymousPlayerAccountReference;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDAccounTest> SelectDatasForTests()
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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
