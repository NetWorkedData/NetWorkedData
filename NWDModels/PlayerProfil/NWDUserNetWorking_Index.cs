//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:25
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserNetWorking : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDGameSave, NWDUserNetWorking> kIndex = new NWDIndex<NWDGameSave, NWDUserNetWorking>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInMemory]
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
        [NWDDeindexInMemory]
        public void RemoveFromTipKeyIndex()
        {
            // Remove from the actual indexation
            kIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserNetWorking CurrentData(bool sOrCreate = true)
        {
            NWDUserNetWorking rReturn = null;
            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(typeof(NWDUserNetWorking));
            if (tHelper.AllDatabaseIsLoaded() && tHelper.AllDatabaseIsIndexed() == true)
            {
                 rReturn = kIndex.RawFirstDataByKey(NWDGameSave.CurrentData());
                if (rReturn == null && sOrCreate == true)
                {
                    rReturn = NWDBasisHelper.NewData<NWDUserNetWorking>();
                    rReturn.GameSave.SetData(NWDGameSave.CurrentData());
                    rReturn.UpdateData();
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================