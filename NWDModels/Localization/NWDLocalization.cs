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
using System.Linq;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("LCL")]
    [NWDClassDescriptionAttribute("Localization are used to localize the string of your game.\n" +
                                   "It's dependent from the \"Localization\" menu items in editor.\n" +
                                   "")]
    [NWDClassMenuNameAttribute("Localization")]
    //-------------------------------------------------------------------------------------------------------------
    public partial class NWDLocalization : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Localization", true, true, true)]
        [NWDInformation("Use Dialog or Message for more complex localization.")]
        
        [NWDCertified]
        [NWDTooltips("The localizable value")]
        public NWDLocalizableLongTextType TextValue
        {
            get; set;
        }

        [NWDCertified]
        [NWDTooltips("The Key to use to replace by the value use something like {xxxxx} or #xxxx# empty if localization is not use as autoreplace value")]
        public string KeyValue
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================