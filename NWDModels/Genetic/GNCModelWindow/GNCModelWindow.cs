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
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using NetWorkedData.MacroDefine;
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
// You can create custom enum of macro
// Just follow this example class
public class NWDGeneticMacro : MDEDataTypeBoolGeneric<NWDGeneticMacro>
{
    //-------------------------------------------------------------------------------------------------------------
    // the title of bool controller
    public static string Title = SetTitle("NWD Genetic");
    public static string Group = SetGroup(MDEConstants.GroupModule);
    public static int Order = SetOrder(0);
    //-------------------------------------------------------------------------------------------------------------
    // declare one value
    public static NWDGeneticMacro MacroBool = SetValue("GNC_GENETIC");
    //-------------------------------------------------------------------------------------------------------------
}
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
#endif
//=====================================================================================================================
#if UNITY_EDITOR
#if GNC_GENETIC
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[NWDTypeWindowParamAttribute("Genetic ",
		"Genetic models",
		new Type[] {
        typeof(GNCGene),
		typeof(GNCSpecie),
        typeof(GNCGeneticIndividual),
		/* Add NWDBasis here*/
		})]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public class GNCModelWindow : NWDBasisWindow <GNCModelWindow>
	{
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDEditorMenu.K_NETWORKEDDATA + "Genetic " + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, NWDEditorMenu.K_MODULES_MANAGEMENT_INDEX + 2)]
		public static void MenuMethod ()
		{
            ShowWindow();
		}
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
#endif //GNC_GENETIC
