//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using UnityEngine;
using SQLite4Unity3d;
#if UNITY_EDITOR
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[NWDTypeWindowParamAttribute("Barter",
		"Barter window description",
		"NWDIcons_02",
		new Type[] {
			typeof(NWDBarterPlace), 
			typeof(NWDUserBarterRequest), 
			typeof(NWDUserBarterProposition),
			/* Add NWDBasis here*/
		}
	)]
	public class NWDBarterWindow : NWDBasisWindow <NWDBarterWindow>
	{
		[MenuItem (NWDConstants.K_MENU_BASE+ "Marketplaces/Barter"+NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, 550)]
		//-------------------------------------------------------------------------------------------------------------
		public static void MenuMethod ()
		{
			EditorWindow tWindow = EditorWindow.GetWindow (typeof(NWDBarterWindow));
			tWindow.Show ();
		}
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif