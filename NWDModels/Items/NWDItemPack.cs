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
#if NWD_ITEMPACK
using System;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassMacro("NWD_ITEMPACK")]
    [NWDClassTrigrammeAttribute("ITP")]
    [NWDClassDescriptionAttribute("Item Pack descriptions Class")]
    [NWDClassMenuNameAttribute("Item Pack")]
    public partial class NWDItemPack : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Description Item", true, true, true)]
        public NWDReferenceType<NWDItem> ItemDescription
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Items in this Item Pack", true, true, true)]
        public NWDReferencesQuantityType<NWDItem> Items
        {
            get; set;
        }
        //[NWDInspectorGroupEnd]

        //[NWDInspectorGroupStart("Classification", true, true, true)]
#if NWD_MODULE_GAME
        public NWDReferencesListType<NWDWorld> Worlds
        {
            get; set;
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif