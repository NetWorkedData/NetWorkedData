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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;



#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("PCK")]
    [NWDClassDescriptionAttribute("Pack descriptions Class")]
    [NWDClassMenuNameAttribute("Pack")]
    public partial class NWDPack : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Description Item", true, true, true)]
        public NWDReferenceType<NWDItem> ItemDescription { get; set; }
        [NWDInspectorGroupEnd]



        [NWDInspectorGroupStart("Item Pack in this Pack", true, true, true)]
        public NWDReferencesQuantityType<NWDItemPack> ItemPackReference { get; set; }
        public int Quantity { get; set; }
        [NWDInspectorGroupEnd]



        [NWDInspectorGroupStart("Item to Pay for this Pack", true, true, true)]
        public NWDReferencesQuantityType<NWDItem> ItemsToPay { get; set; }
        //public NWDReferenceType<NWDInAppPack> InAppReference { get; set; }
        public bool EnableFreePack { get; set; }
        [NWDInspectorGroupEnd]



        [NWDInspectorGroupStart("Classification", true, true, true)]
#if NWD_MODULE_GAME
        public NWDReferencesListType<NWDWorld> Worlds { get; set; }
#endif
        [NWDInspectorGroupEnd]



        [NWDInspectorGroupStart("Availability schedule ", true, true, true)]
        [NWDTooltips("Availability schedule of this Pack")]
        public NWDDateTimeScheduleType AvailabilitySchedule { get; set; }

        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================