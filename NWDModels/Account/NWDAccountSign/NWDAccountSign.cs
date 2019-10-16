//=====================================================================================================================
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
//=====================================================================================================================

using System;
using System.Collections.Generic;
//using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountSignType : NWEDataTypeEnumGeneric<NWDAccountSignType>
    {
        // 0 is reserved by None and invalidate the signup process
        public static NWDAccountSignType DeviceID = Add(1, "DeviceID");
        public static NWDAccountSignType EmailPassword = Add(10, "EmailPassword");
        public static NWDAccountSignType LoginPasswordEmail = Add(11, "LoginPasswordEmail");

        public static NWDAccountSignType Facebook = Add(20, "FacebookID");
        public static NWDAccountSignType Google = Add(21, "GoogleID");
        public static NWDAccountSignType Apple = Add(22, "AppleID");
#if UNITY_EDITOR
        public static NWDAccountSignType Fake = Add(88, "FakeID");
        public static NWDAccountSignType EditorID = Add(99, "EditorID");
#endif
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
        //ErrorDissociated = 22, // no possible case
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
    public partial class NWDAccountSign : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Informations")]
		public NWDReferenceType<NWDAccount> Account {get; set;}
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Sign Send")]
        [NWDNotEditable]
		public NWDAccountSignType SignType {get; set; }
        [NWDNotEditable]
        public string SignHash {get; set; }
        [NWDNotEditable]
        public string RescueHash {get; set; }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Server Action")]
        [NWDNotEditable]
        public NWDAccountSignAction SignStatus { get; set; }


        //[NWDNotEditable]// TODO Delete
        //public string RescueHashServer { get; set; } // TODO Delete


        //[NWDNotEditable]// TODO Delete
        //public string SignHashServer { get; set; } // TODO Delete
        //[NWDInspectorGroupEnd]
        //[NWDInspectorGroupStart("Rescue")]// TODO Delete
        //[NWDNotEditable]// TODO Delete
        //public string RescuePinCode { get; set; } // TODO Delete
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================