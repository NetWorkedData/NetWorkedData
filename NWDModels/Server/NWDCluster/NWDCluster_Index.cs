//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:14
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
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
    public partial class NWDCluster : NWDBasis
    {
        NWDReferenceType<NWDSomething> SomethingReference { set; get; }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDCluster : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDLevel, NWDCluster> kLevelIndex = new NWDIndex<NWDLevel, NWDCluster>();
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
        public static NWDCluster FindFirstDataBySomething(NWDLevel sKey, bool sOrCreate = false)
        {
            NWDCluster rReturn = kLevelIndex.FirstRawDataByKey(sKey.Reference);
            if (rReturn == null && sOrCreate == true)
            {
                rReturn = new NWDCluster();
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
