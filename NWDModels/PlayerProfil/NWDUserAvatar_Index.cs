//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:19
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
    public partial class NWDUserAvatar : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDGameSave,NWDUserAvatar> kIndex = new NWDIndex<NWDGameSave, NWDUserAvatar>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInMemory]
        public void InsertInTipKeyIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                // Re-add !
                kIndex.UpdateData(this, this.GameSave.GetReference());
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
        public static NWDUserAvatar CurrentData(bool sOrCreate = true)
        {
            //NWDUserAvatar rReturn = kIndex.RawFirstDataByKey(NWDGameSave.CurrentData());
            //if (rReturn == null && sOrCreate == true)
            //{
            //    rReturn = NWDBasisHelper.NewData<NWDUserAvatar>();
            //    rReturn.GameSave.SetData(NWDGameSave.CurrentData());
            //    rReturn.UpdateData();
            //}
            //return rReturn;

            NWDUserAvatar rReturn = null;
            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(typeof(NWDUserAvatar));
            if (tHelper.AllDatabaseIsLoaded() && tHelper.AllDatabaseIsIndexed() == true)
            {
                rReturn = kIndex.FirstRawDataByKey(NWDGameSave.CurrentData());
                if (rReturn == null && sOrCreate == true)
                {
                    rReturn = NWDBasisHelper.NewData<NWDUserAvatar>();
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