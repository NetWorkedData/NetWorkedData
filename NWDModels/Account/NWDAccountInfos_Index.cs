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
        /// <summary>
        /// Get the current account infos instance for the current account
        /// </summary>
        /// <returns></returns>
        public static NWDAccountInfos CurrentData()
        {
            //Debug.Log("<color=red> ###### I NEED THE CURRENT DATA </color>");
            NWDAccountInfos tInfos = null;
            if (NWDBasisHelper.FindTypeInfos(typeof(NWDAccountInfos)).IsLoaded())
            {
                string tUniqueReference = NWDAccount.GetUniqueReferenceFromCurrentAccount<NWDAccountInfos>();
                tInfos = NWDBasisHelper.GetRawDataByReference<NWDAccountInfos>(tUniqueReference);
                if (tInfos == null && string.IsNullOrEmpty(tUniqueReference) == false)
                {
                    tInfos = NWDBasisHelper.NewDataWithReference<NWDAccountInfos>(tUniqueReference);
                    tInfos.SaveData();
                }
                //Debug.Log("<color=red> ###### I NEED THE CURRENT DATA  I RETURN " + tUniqueReference + "</color>");
            }
            return tInfos;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAccountInfos CurrentDataForAccount( string sAccountReference)
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