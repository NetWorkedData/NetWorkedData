// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:47:29
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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
        typeof(NWDAchievementKey),
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