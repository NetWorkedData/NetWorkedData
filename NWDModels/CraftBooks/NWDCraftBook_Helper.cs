//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:47:40
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
//using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDCraftBookHelper : NWDHelper<NWDCraftBook>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override List<Type> OverrideClasseInThisSync()
        {
            Debug.Log("NWDCraftBookHelper OverrideClasseInThisSync() give list");
            return new List<Type> { typeof(NWDCraftReward), typeof(NWDCraftBook), typeof(NWDCraftRecipient) };
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================