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
#if UNITY_EDITOR
using System;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDInternalKeyNotEditable]
    [NWDClassMacro("UNITY_EDITOR")]
    [NWDClassTrigrammeAttribute("SSS")]
    [NWDClassDescriptionAttribute("Server Services descriptions Class")]
    [NWDClassMenuNameAttribute("Server Services")]
    [NWDInternalDescriptionNotEditable]
    public partial class NWDServerServices : NWDBasisUnsynchronize
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Server SSH")]
        public NWDReferenceType<NWDServer> Server { get; set; }
        //public NWDServerContinent Continent { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Server Services")]
        public NWDReferenceType<NWDServerDomain> ServerDomain { get; set; }
        [NWDEntitled("Email for SSL certification")]
        public string Email { get; set; }
        public bool UserInstalled { get; set; }
        [NWDIf("UserInstalled", false)]
        public string Folder { get; set; }
        [NWDIf("UserInstalled", false)]
        [NWDEntitled("SFTP User")]
        public string User { get; set; }
        [NWDIf("UserInstalled", false)]
        [NWDEntitled("SFTP Password (AES)")]
        public NWDSecurePassword Secure_Password { get; set; }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Server Environment Actif")]
        [NWDNotEditable]
        public bool Dev { get; set; }
        [NWDNotEditable]
        public bool Preprod { get; set; }
        [NWDNotEditable]
        public bool Prod { get; set; }
        [NWDNotEditable]
        public string Information { get; set; }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif