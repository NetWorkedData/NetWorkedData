// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:28
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
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserNickname : NWDBasis<NWDUserNickname>
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDGameSave, NWDUserNickname> kIndex = new NWDIndex<NWDGameSave, NWDUserNickname>();
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
        public static NWDUserNickname CurrentData(bool sOrCreate = true)
        {
            NWDUserNickname rReturn = kIndex.RawFirstDataByKey(NWDGameSave.CurrentData());
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