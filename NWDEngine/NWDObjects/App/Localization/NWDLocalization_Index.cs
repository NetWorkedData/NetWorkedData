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
using System.Linq;
using UnityEngine;
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDLocalization : NWDBasis<NWDLocalization>
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndexSimple<NWDLocalization> kKeyIndex = new NWDIndexSimple<NWDLocalization>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        public void InsertInCodeIndex()
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
        public void RemoveFromCodeIndex()
        {
            // Remove from the actual indexation
            kKeyIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDLocalization FindFirstDataByCode(string sKeyValue)
        {
            return kKeyIndex.RawFirstDataByKey(sKeyValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDLocalization> RawDatasByIndex()
        {
            return kKeyIndex.RawDatas();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================