using System;

using UnityEngine;

using SQLite4Unity3d;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDTypeWindowParamAttribute("Tips & Tricks",
		"settings",
		"Tips & Tricks window description",
		new Type[] {
			typeof(NWDTipsAndTricks),
			typeof(NWDTipsAndTricksOwnership),
			/* Add NWDBasis here*/
		}
	)]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDTipsAndTricksWindow : NWDBasisWindow <NWDTipsAndTricksWindow>
	{
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_BASE+ "Tips and Tricks"+NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, 100)]
		//-------------------------------------------------------------------------------------------------------------
		public static void MenuMethod ()
		{
			EditorWindow tWindow = EditorWindow.GetWindow (typeof(NWDTipsAndTricksWindow));
			tWindow.Show ();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif