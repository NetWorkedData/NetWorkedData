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
	[NWDTypeWindowParamAttribute("NWD Classes",
		"All NWD Basis herited classes edition window.",
		"NWDIcons_02",
		null
	)]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDAllClassesWindow : NWDBasisWindow <NWDAllClassesWindow>
	{
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_ALL_CLASSES, false, 60)]
		//-------------------------------------------------------------------------------------------------------------
		public static void MenuMethod ()
		{
			EditorWindow tWindow = EditorWindow.GetWindow (typeof(NWDAllClassesWindow));
			NWDAllClassesWindow tAllClassWindow = tWindow as NWDAllClassesWindow;
			tWindow.Show ();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif
