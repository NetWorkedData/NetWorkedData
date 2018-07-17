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
	//-------------------------------------------------------------------------------------------------------------
	[NWDTypeWindowParamAttribute("AIR Management",
		"AIR Window",
		"NWDIcons_02", // AIRWindow_ICON
		new Type[] {
        typeof(AIRDimension),
        typeof(AIRUserSurface),
        typeof(AIRCharacterSurface),
        typeof(AIRUserRelationship),
        typeof(AIRGroupSurface),
        typeof(AIRUserNotoriety),
        typeof(AIRTargetMatch),
		/* Add NWDBasis here*/
		}
	)]
	//-------------------------------------------------------------------------------------------------------------
	public class AIRWindow : NWDBasisWindow <AIRWindow>
	{
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_BASE + "AIR Management" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, 2000)]
		//-------------------------------------------------------------------------------------------------------------
		public static void MenuMethod ()
		{
			EditorWindow tWindow = EditorWindow.GetWindow (typeof(AIRWindow));
			tWindow.Show ();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif