// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:10
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
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountSignType : BTBDataTypeEnumGeneric<NWDAccountSignType>
    {
        public static NWDAccountSignType DeviceID = Add(1, "DeviceID");
        public static NWDAccountSignType LoginPassword = Add(2, "LoginPassword");
        public static NWDAccountSignType Facebook = Add(3, "Facebook");
        public static NWDAccountSignType Google = Add(4, "Google");
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDAccountSignAction : int
    {
        None = 0,

        TryToAssociate = 10,
        Associated = 11,
        ErrorAssociated = 12,

        TryToDissociate = 20,
        Dissociated = 21,
        //ErrorDissociated = 22,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountSignHelper : NWDHelper<NWDAccountSign>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDAccountSign class. This class is use for (complete description here).
    /// </summary>
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("SKD")]
    [NWDClassDescriptionAttribute("Account Sign to connect by hash of sign")]
    [NWDClassMenuNameAttribute("Account Sign")]
    [NWDForceSecureData]
    public partial class NWDAccountSign : NWDBasis<NWDAccountSign>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Informations")]
		public NWDReferenceType<NWDAccount> Account {get; set;}
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Sign Send")]
      //  [NWDNotEditable]
		public NWDAccountSignType SignType {get; set; }
       // [NWDNotEditable]
        public string SignHash {get; set; }
       // [NWDNotEditable]
        public string RescueHash {get; set; }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Server Action")]
      //  [NWDNotEditable]
        public NWDAccountSignAction SignAction { get; set; }
      //  [NWDNotEditable]
        public string SignHashServer { get; set; }
       // [NWDNotEditable]
        public string RescueHashServer { get; set; }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Rescue")]
       // [NWDNotEditable]
        public string RescuePinCode { get; set; } // to recreate the password and send by email the url send to user containt the email and the rescue pincode 
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================