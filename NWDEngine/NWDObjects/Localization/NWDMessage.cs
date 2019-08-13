//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:33:52
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
using UnityEngine;
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDMessageType : int
    {
        InGame = 10, // Use in game alert BTBNotification Post notification
        Alert = 20, // System Dialog
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("MES")]
    [NWDClassDescriptionAttribute("Message descriptions Class")]
    [NWDClassMenuNameAttribute("Messages")]
    [NWDInternalKeyNotEditableAttribute]
    public partial class NWDMessage : NWDBasis<NWDMessage>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Informations", true, true, true)]
        [NWDTooltips("Type of message")]
        public NWDMessageType Type
        {
            get; set;
        }
        public string Domain { get; set; }
        public string Code { get; set; }
        [NWDInspectorGroupEnd]
        
        [NWDInspectorGroupStart("Description", true, true, true)]
        public NWDLocalizableStringType Title { get; set; }
        public NWDLocalizableTextType Description { get; set; }
        [NWDInspectorGroupEnd]
        
        [NWDInspectorGroupStart("User choose", true, true, true)]
        //public bool HasValidButton { get; set; }
        public NWDLocalizableStringType Validation { get; set; }
        //public NWDReferenceType<NWDAction> ValidAction { get; set; }
        //public bool HasCancelButton { get; set; }
        public NWDLocalizableStringType Cancel { get; set; }
        //public NWDReferenceType<NWDAction> CancelAction { get; set;}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================