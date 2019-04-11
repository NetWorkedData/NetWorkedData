//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserInfos : NWDBasis<NWDUserInfos>
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDGameSave, NWDUserInfos> kIndex = new NWDIndex<NWDGameSave, NWDUserInfos>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        public void InsertInTipKeyIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                // Re-add !
                kIndex.InsertData(this, this.GameSave.GetReference());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexRemove]
        public void RemoveFromTipKeyIndex()
        {
            // Remove from the actual indexation
            kIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserInfos CurrentData(bool sOrCreate = true)
        {
            NWDUserInfos rReturn = kIndex.RawFirstDataByKey(NWDGameSave.CurrentData());
            if (rReturn == null && sOrCreate == true)
            {
                rReturn = NewData();
                rReturn.GameSave.SetObject(NWDGameSave.CurrentData());
                rReturn.UpdateData();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================