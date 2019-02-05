//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[NWDTypeWindowParamAttribute("Achievement",
		"Achievements",
        new Type[] {
        typeof(NWDAchievement),
        typeof(NWDAccountAchievement),
		typeof(NWDUserAchievement),
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