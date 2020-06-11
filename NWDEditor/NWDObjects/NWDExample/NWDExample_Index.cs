//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // Example with fictive class NWDSomething
    // Connect by property Something
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /*
    public partial class NWDSomething : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDSomething()
        {
            //Debug.Log("NWDSomething Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDSomething(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDSomething Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDExample : NWDBasis
    {
        NWDReferenceType<NWDSomething> SomethingReference { set; get; }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDExample : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDLevel, NWDExample> kLevelIndex = new NWDIndex<NWDLevel, NWDExample>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInMemory]
        public void InsertInLevelIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                // Re-add !
                kLevelIndex.UpdateData(this, SomethingReference.GetReference());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDDeindexInMemory]
        public void RemoveFromLevelIndex()
        {
            // Remove from the actual indexation
            kLevelIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDExample FindFirstDataBySomething(NWDLevel sKey, bool sOrCreate = false)
        {
            NWDExample rReturn = kLevelIndex.FirstRawDataByKey(sKey.Reference);
            if (rReturn == null && sOrCreate == true)
            {
                rReturn = new NWDExample();
                rReturn.SaveData();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    */
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
