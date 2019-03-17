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
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("MES")]
    [NWDClassDescriptionAttribute("Message descriptions Class")]
    [NWDClassMenuNameAttribute("Messages")]
    [NWDInternalKeyNotEditableAttribute]
    public partial class NWDMessage : NWDBasis<NWDMessage>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStartAttribute("Informations", true, true, true)]
        public string Domain { get; set; }
        public string Code { get; set; }
        [NWDInspectorGroupEndAttribute]
        
        [NWDInspectorGroupStartAttribute("Description", true, true, true)]
        public NWDLocalizableStringType Title { get; set; }
        public NWDLocalizableTextType Message { get; set; }
        [NWDInspectorGroupEndAttribute]
        
        [NWDInspectorGroupStartAttribute("User choose", true, true, true)]
        public bool HasValidButton { get; set; }
        public NWDLocalizableStringType ValidText { get; set; }
        public NWDReferenceType<NWDAction> ValidAction { get; set; }
        public bool HasCancelButton { get; set; }
        public NWDLocalizableStringType CancelText { get; set; }
        public NWDReferenceType<NWDAction> CancelAction { get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================