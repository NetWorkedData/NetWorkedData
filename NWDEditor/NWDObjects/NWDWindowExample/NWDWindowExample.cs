//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[NWDTypeWindowParamAttribute("NWDWindowExample_Name",
		"NWDWindowExample_Description",
		"NWDIcons_02", // NWDWindowExample_ICON
		new Type[] {typeof(NWDExample),/* Add NWDBasis here*/
		}
                                )]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public class NWDWindowExample : NWDBasisWindow <NWDWindowExample>
	{
		//-------------------------------------------------------------------------------------------------------------
		//[MenuItem (NWDConstants.K_MENU_BASE + "NWDWindowExample_Name" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, 12345)]
		//-------------------------------------------------------------------------------------------------------------
		public static void MenuMethod ()
		{
			EditorWindow tWindow = EditorWindow.GetWindow (typeof(NWDWindowExample));
			tWindow.Show ();
		}
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif