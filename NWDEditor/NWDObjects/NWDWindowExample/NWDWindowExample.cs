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
//MACRO_DEFINE #if NWD_EXAMPLE_MACRO
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[NWDTypeWindowParamAttribute("NWDWindowExample_Name",
		"NWDWindowExample_Description",
		new Type[] {
        typeof(NWDExample),/* Add NWDBasis here*/
		})]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public class NWDWindowExample : NWDBasisWindow <NWDWindowExample>
	{
		//-------------------------------------------------------------------------------------------------------------
		//[MenuItem (NWDEditorMenu.K_NETWORKEDDATA + "NWDWindowExample_Name" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, NWDEditorMenu.K_CUSTOMS_MANAGEMENT_INDEX + 1)]
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
//MACRO_DEFINE #endif //NWD_EXAMPLE_MACRO
