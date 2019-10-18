//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:33:55
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;

//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
using UnityEngine;
//using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDMessage : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndexSimple<NWDMessage> kCodeIndex = new NWDIndexSimple<NWDMessage>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        public void InsertInCodeIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                // Re-add !
                kCodeIndex.InsertData(this, this.Code);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexRemove]
        public void RemoveFromCodeIndex()
        {
            // Remove from the actual indexation
            kCodeIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDMessage FindDataByCode(string sCode)
        {
            return kCodeIndex.RawFirstDataByKey(sCode);
        }
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndexSimple<NWDMessage> kDomainCodeIndex = new NWDIndexSimple<NWDMessage>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
        public void InsertInDomainCodeIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                // Re-add !
                kDomainCodeIndex.InsertData(this, this.Domain + NWDConstants.kFieldSeparatorA + this.Code);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexRemove]
        public void RemoveFromDomainCodeIndex()
        {
            // Remove from the actual indexation
            kDomainCodeIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDMessage FindDataByDomainAndCode(string sDomain, string sCode)
        {
            return kDomainCodeIndex.RawFirstDataByKey(sDomain + NWDConstants.kFieldSeparatorA + sCode);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================