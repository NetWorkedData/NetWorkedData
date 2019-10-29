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

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountSignType : NWEDataTypeEnumGeneric<NWDAccountSignType>
    {
        // 0 is reserved by None and invalidate the signup process
        public static NWDAccountSignType DeviceID = Add(1, "DeviceID"); // NEVER CHANGE INT VALUE !!!

        public static NWDAccountSignType EmailPassword = Add(10, "EmailPassword"); // NEVER CHANGE INT VALUE !!!
        public static NWDAccountSignType LoginPasswordEmail = Add(11, "LoginPasswordEmail"); // NEVER CHANGE INT VALUE !!!
                                                                                             // NEVER CHANGE INT VALUE !!!
        public static NWDAccountSignType Facebook = Add(20, "FacebookID"); // NEVER CHANGE INT VALUE !!!
        public static NWDAccountSignType Google = Add(21, "GoogleID"); // NEVER CHANGE INT VALUE !!!
        public static NWDAccountSignType Apple = Add(22, "AppleID"); // NEVER CHANGE INT VALUE !!!
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountSignType : NWEDataTypeEnumGeneric<NWDAccountSignType>
    {
#if UNITY_EDITOR
        public static NWDAccountSignType Fake = Add(88, "FakeID"); // NEVER CHANGE INT VALUE !!!
        public static NWDAccountSignType EditorID = Add(99, "EditorID"); // NEVER CHANGE INT VALUE !!!
#endif
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDAccountSignAction
    {
        None = 0, // NEVER CHANGE INT VALUE !!!

        TryToAssociate = 10, // NEVER CHANGE INT VALUE !!!
        Associated = 11, // NEVER CHANGE INT VALUE !!!
        ErrorAssociated = 12, // NEVER CHANGE INT VALUE !!!

        TryToDissociate = 20, // NEVER CHANGE INT VALUE !!!
        Dissociated = 21, // NEVER CHANGE INT VALUE !!!
        //ErrorDissociated = 22, // no possible case  // NEVER CHANGE INT VALUE !!!
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
    [NWDClassClusterAttribute(3, 32)]
    public partial class NWDAccountSign : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Informations")]
        public NWDReferenceType<NWDAccount> Account { get; set; }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Sign Send")]
        [NWDNotEditable]
        public NWDAccountSignType SignType { get; set; }
        [NWDNotEditable]
        public string SignHash { get; set; }
        [NWDNotEditable]
        public string RescueHash { get; set; }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Server Action")]
        [NWDNotEditable]
        public NWDAccountSignAction SignStatus { get; set; }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================