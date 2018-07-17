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
	[NWDTypeWindowParamAttribute("Tips & Tricks",
		"Tips & Tricks window description",
		"NWDIcons_02",
		new Type[] {
			typeof(NWDTip),
			typeof(NWDUserTip),
			/* Add NWDBasis here*/
		}
                                )]
	public class NWDTipWindow : NWDBasisWindow <NWDTipWindow>
	{
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_BASE+ "Tips and Tricks"+NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, 2010)]
		//-------------------------------------------------------------------------------------------------------------
		public static void MenuMethod ()
		{
			EditorWindow tWindow = EditorWindow.GetWindow (typeof(NWDTipWindow));
			tWindow.Show ();
		}
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif