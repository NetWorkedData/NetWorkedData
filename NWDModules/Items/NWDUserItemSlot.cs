// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:48:41
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
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDUserItemSlot class. This class is use for (complete description here).
    /// </summary>
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UIS")]
    [NWDClassDescriptionAttribute("User Item Slot")]
    [NWDClassMenuNameAttribute("User Item Slot")]
    //[NWDInternalKeyNotEditableAttribute]
    public partial class NWDUserItemSlot : NWDBasis<NWDUserItemSlot>
    {
       //-------------------------------------------------------------------------------------------------------------
		public NWDReferenceType<NWDAccount> Account {get; set;}
		public NWDReferenceType<NWDGameSave> GameSave {get; set;}
		public NWDReferenceType<NWDItemSlot> ItemSlot {get; set;}
		public NWDReferencesArrayType<NWDItem> ItemsUsed {get; set;}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================