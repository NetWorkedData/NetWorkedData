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
#endif

//=====================================================================================================================
//namespace MacroDefineEditor
//{
//    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//    // You can create custom enum of macro
//    // Just follow this example class
//    public class GNC_GENETIC_DefineBool : MDEDataTypeBoolGeneric<GNC_GENETIC_DefineBool>
//    {
//        //-------------------------------------------------------------------------------------------------------------
//        // the title of enum controller
//        public static string Title = SetTitle("GNC_GENETIC");
//        //-------------------------------------------------------------------------------------------------------------
//        // declare one value
//        public static GNC_GENETIC_DefineBool Macro = SetValue("GNC_GENETIC");
//        //-------------------------------------------------------------------------------------------------------------
//    }
//    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//}
//=====================================================================================================================

#if GNC_GENETIC
//=====================================================================================================================

using System;
using UnityEngine;
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class GNCSeedHelper : NWDHelper<GNCSeed>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// GNCSeed class. This class is use for (complete description here).
    /// </summary>
    [NWDClassMacroAttribute("GNC_GENETIC")]
    [NWDClassTrigrammeAttribute("GNCE")]
    [NWDClassDescriptionAttribute("Seed with predifine genetic to sell in store")]
    [NWDClassMenuNameAttribute("Seed")]
    public partial class GNCSeed : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        //PROPERTIES
        //[NWDInspectorGroupReset]
        public NWDLocalizableStringType Name { get; set; }
        public NWDReferenceType<GNCSpecie> Specie { get; set; }
        public NWDReferencesListType<GNCGene> PaternalHeritage { get; set; }
        public NWDReferencesListType<GNCGene> MaternalHeritage { get; set; }
        [NWDFloatSlider(0, 1)]
        public float CytoplasmicMaleSterility { get; set; }


        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        // never change the constructors! they are used by the NetWorkedData Writing System
        //-------------------------------------------------------------------------------------------------------------
        public GNCSeed()
        {
            //Debug.Log("GNCSeed Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public GNCSeed(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("GNCSeed Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif //GNC_GENETIC
