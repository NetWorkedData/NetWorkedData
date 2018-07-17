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
	[NWDTypeWindowParamAttribute("User Party Window",
                                 "User Party Window",
                                 "NWDIcons_02", // NWDUserPartyWindow_ICON
        new Type[] {
        typeof(NWDGameSave),
        typeof(NWDUserOwnership),
        typeof(NWDUserQuestAdvancement),
        typeof(NWDUserConsolidatedStats),
        typeof(NWDUserStats),
		/* Add NWDBasis here*/
		}
                                )]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public class NWDGameSaveWindow : NWDBasisWindow <NWDGameSaveWindow>
	{
		//-------------------------------------------------------------------------------------------------------------
        [MenuItem (NWDConstants.K_MENU_BASE + "User Game Save" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, 2000)]
		//-------------------------------------------------------------------------------------------------------------
		public static void MenuMethod ()
		{
			EditorWindow tWindow = EditorWindow.GetWindow (typeof(NWDGameSaveWindow));
			tWindow.Show ();
		}
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif