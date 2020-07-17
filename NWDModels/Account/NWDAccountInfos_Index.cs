//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountInfos : NWDBasisAccountDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        static private NWDAccountInfos kAccountInfos;
        //-------------------------------------------------------------------------------------------------------------
        public static void ResetCurrentData()
        {
            kAccountInfos = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the current account infos instance for the current account
        /// </summary>
        /// <returns></returns>
        public static NWDAccountInfos CurrentData()
        {
            //NWDBenchmark.Start();
            NWDAccountInfos tInfos = null;
            if (kAccountInfos == null)
            {
                //Debug.Log("<color=red> ###### I NEED THE CURRENT DATA </color>");
                //NWDBenchmark.Step();
                if (NWDLauncher.GetState() == NWDStatut.NetWorkedDataReady)
                //if (NWDBasisHelper.FindTypeInfos(typeof(NWDAccountInfos)).IsLoaded())
                {
                    string tUniqueReference = NWDAccount.GetUniqueReferenceFromCurrentAccount<NWDAccountInfos>();
                    tInfos = NWDBasisHelper.GetRawDataByReference<NWDAccountInfos>(tUniqueReference);
                    //NWDBenchmark.Step();
                    if (tInfos == null && string.IsNullOrEmpty(tUniqueReference) == false)
                    {
                        tInfos = NWDBasisHelper.NewDataWithReference<NWDAccountInfos>(tUniqueReference);
                        tInfos.SaveData();
                    }
                    //NWDBenchmark.Step();
                    //Debug.Log("<color=red> ###### I NEED THE CURRENT DATA  I RETURN " + tUniqueReference + "</color>");
                    kAccountInfos = tInfos;
                }
            }
            else
            {
                tInfos = kAccountInfos;
            }
            //NWDBenchmark.Step();
            //NWDBenchmark.Finish();
            return tInfos;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAccountInfos CurrentDataForAccount(string sAccountReference)
        {
            NWDAccountInfos tInfos = null;
            if (NWDBasisHelper.FindTypeInfos(typeof(NWDAccountInfos)).IsLoaded())
            {
                string tUniqueReference = NWDAccount.GetUniqueReference(sAccountReference, typeof(NWDAccountInfos));
                tInfos = NWDBasisHelper.GetRawDataByReference<NWDAccountInfos>(tUniqueReference);
            }
            return tInfos;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================