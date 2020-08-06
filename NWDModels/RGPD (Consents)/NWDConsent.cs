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
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDConsent class. This class is used to reccord the consent available in the game. 
    /// </summary>
    //[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("CST")]
    [NWDClassDescriptionAttribute("NWDConsent class. This class is used to reccord the consent available in the game for RGPD")]
    [NWDClassMenuNameAttribute("Consent")]
    public partial class NWDConsent : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Consent description")]
        public NWDLocalizableStringType Title
        {
            get; set;
        }
        public NWDLocalizableTextType Description
        {
            get; set;
        }
        public string LawReferences
        {
            get; set;
        }
        [NWDInspectorGroupEnd()]
        
        [NWDInspectorGroupStart("Consent version")]
        public string KeyOfConsent
        {
            get; set;
        }
        public NWDVersionType Version
        {
            get; set;
        }
        [NWDInspectorGroupEnd()]
        
        [NWDInspectorGroupStart("Consent default state proposition")]
        public NWESwitchState DefaultState
        {
            get; set;
        }
        [NWDTooltips("Expected state to continue the game. If 'Unknow' any value is ok")]
        public NWESwitchState ExpectedState
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================