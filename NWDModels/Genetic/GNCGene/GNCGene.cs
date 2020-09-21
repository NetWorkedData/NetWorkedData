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
    public partial class GNCOrganExpression : NWEDataTypeEnumGeneric<GNCOrganExpression>
    {
        public static GNCOrganExpression Petal = Add(1, "petal");
        public static GNCOrganExpression PetalColor = Add(2, "PetalColor");
        public static GNCOrganExpression PetalSecondColor = Add(3, "PetalSecondColor");
        public static GNCOrganExpression PetalDesign = Add(4, "PetalDesign");
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class GNCSexualExpression : NWEDataTypeEnumGeneric<GNCSexualExpression>
    {
        public static GNCSexualExpression NoSexual = Add(0, "No sexual");
        public static GNCSexualExpression Male = Add(1, "Male");
        public static GNCSexualExpression Female = Add(2, "Female");
        public static GNCSexualExpression SterileMale = Add(3, "SterileMale");
        public static GNCSexualExpression SterilFemale = Add(4, "SterilFemale");
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class GNCGeneHelper : NWDHelper<GNCGene>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// GNCGene class. This class is use for (complete description here).
    /// </summary>
    [NWDClassMacroAttribute("GNC_GENETIC")]
    [NWDClassTrigrammeAttribute("GNCG")]
    [NWDClassDescriptionAttribute("Gene")]
    [NWDClassMenuNameAttribute("Gene model")]
    public partial class GNCGene : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        //[NWDInspectorGroupReset]
        public GNCOrganExpression OrganExpression { get; set; }
        public GNCSexualExpression SexualExpression { get; set; }
        public NWDReferencesAverageType<GNCGene> AlleleMutation { get; set; }
        public int Chromosome { get; set; }
        public float ChromosomeDistance { get; set; }
        public float GeneticExpression { get; set; }
        public bool Domination { get; set; }
        public string GeneticNotation { get; set; }

        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        // never change the constructors! they are used by the NetWorkedData Writing System
        //-------------------------------------------------------------------------------------------------------------
        public GNCGene()
        {
            //Debug.Log("GNCGene Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public GNCGene(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("GNCGene Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif //GNC_GENETIC
