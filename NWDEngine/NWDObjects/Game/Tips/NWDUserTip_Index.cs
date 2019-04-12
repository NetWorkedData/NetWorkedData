// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:41:55
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserTip : NWDBasis<NWDUserTip>
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDTipKey, NWDUserTip> kTipKeyIndex = new NWDIndex<NWDTipKey, NWDUserTip>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        public void InsertInTipKeyIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                // Re-add !
                string tKey = Tip.GetReference() + NWDConstants.kFieldSeparatorA + this.GameSave.GetReference();
                kTipKeyIndex.InsertData(this, tKey);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexRemove]
        public void RemoveFromTipKeyIndex()
        {
            // Remove from the actual indexation
            kTipKeyIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserTip FindDataByTip(NWDTipKey sKey, bool sOrCreate = false)
        {
            string tKey = sKey.Reference + NWDConstants.kFieldSeparatorA + NWDGameSave.CurrentData().Reference;
            NWDUserTip rReturn = kTipKeyIndex.RawFirstDataByKey(tKey);
            if (rReturn == null && sOrCreate == true)
            {
                rReturn = NewData();
                rReturn.Tip.SetObject(sKey);
                rReturn.UpdateData();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================