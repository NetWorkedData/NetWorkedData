//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:33:48
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDLocalization : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndexSimple<NWDLocalization> kKeyIndex = new NWDIndexSimple<NWDLocalization>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        public void InsertInKeyIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable() && string.IsNullOrEmpty(KeyValue) == false)
            {
                // Re-add !
                kKeyIndex.InsertData(this, this.KeyValue);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexRemove]
        public void RemoveFromKeyIndex()
        {
            // Remove from the actual indexation
            kKeyIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDLocalization FindFirstDataByKey(string sKeyValue)
        {
            return kKeyIndex.RawFirstDataByKey(sKeyValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDLocalization> RawDatasWithKey()
        {
            return kKeyIndex.RawDatas();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================