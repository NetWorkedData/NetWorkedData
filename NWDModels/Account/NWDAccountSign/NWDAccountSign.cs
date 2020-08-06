//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
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

        public static NWDAccountSignType DeviceID = Add(1, "DeviceID");                         // NEVER CHANGE INT VALUE !

        public static NWDAccountSignType EmailPassword = Add(10, "EmailPassword");              // NEVER CHANGE INT VALUE !
        public static NWDAccountSignType LoginPasswordEmail = Add(11, "LoginPasswordEmail");    // NEVER CHANGE INT VALUE ! //TODO Perhaps forbidden this sign (login is soo hasbeen)
                                                                                                // NEVER CHANGE INT VALUE !
        public static NWDAccountSignType Facebook = Add(20, "FacebookID");                      // NEVER CHANGE INT VALUE !
        public static NWDAccountSignType Google = Add(21, "GoogleID");                          // NEVER CHANGE INT VALUE !
        public static NWDAccountSignType Apple = Add(22, "AppleID");                            // NEVER CHANGE INT VALUE !


        //public static NWDAccountSignType Biometric = Add(66, "Biometric");                      // NEVER CHANGE INT VALUE !
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountSignType : NWEDataTypeEnumGeneric<NWDAccountSignType>
    {
#if UNITY_INCLUDE_TESTS
        public static NWDAccountSignType Fake = Add(88, "FakeID"); // NEVER CHANGE INT VALUE !!!
#endif
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountSignType : NWEDataTypeEnumGeneric<NWDAccountSignType>
    {
#if UNITY_EDITOR
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
    [NWDInternalKeyNotEditable]
    //[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("SKD")]
    [NWDClassDescriptionAttribute("Account Sign to connect by hash of sign")]
    [NWDClassMenuNameAttribute("Account Sign")]
    [NWDForceSecureData]
    [NWDClassClusterAttribute(3, 32)]
    public partial class NWDAccountSign : NWDBasisAccountDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        public const string K_NO_HASH = "-";
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Sign Send")]
        [NWDNotEditable]
        public NWDAccountSignType SignType { get; set; }
        [NWDNotEditable]
        public string SignHash { get; set; }
        [NWDNotEditable]
        public string RescueHash { get; set; }
        [NWDNotEditable]
        public string LoginHash { get; set; }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Server Action")]
        [NWDNotEditable]
        public NWDAccountSignAction SignStatus { get; set; }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================