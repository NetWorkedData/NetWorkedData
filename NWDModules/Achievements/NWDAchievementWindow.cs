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
	[NWDTypeWindowParamAttribute("Achievement",
		"Achievements",
        "NWDAchievementWindow", // SOBAchievementWindow_ICON
                                 new Type[] {typeof(NWDAchievement),
        typeof(NWDAccountAchievement),
		typeof(NWDUserAchievement),
		/* Add NWDBasis here*/
		}
	)]
	public class NWDAchievementWindow : NWDBasisWindow <NWDAchievementWindow>
	{
		//-------------------------------------------------------------------------------------------------------------
        [MenuItem (NWDConstants.K_MENU_BASE + "Game/Achievements", false, 300)]
		public static void MenuMethod ()
		{
			EditorWindow tWindow = EditorWindow.GetWindow (typeof(NWDAchievementWindow));
			tWindow.Show ();
		}
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif