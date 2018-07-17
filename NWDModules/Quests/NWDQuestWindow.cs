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
	[NWDTypeWindowParamAttribute("Quest",
		"Quest window description",
		"NWDIcons_02",
            new Type[] {
            typeof(NWDSetOfQuests),
			typeof(NWDQuest),
			typeof(NWDDialog),
            typeof(NWDCharacter),
            typeof(NWDAction),
			typeof(NWDUserQuestAdvancement),
        typeof(NWDYoghurtLyric),
			/* Add NWDBasis here*/
		}
	)]
	public class NWDQuestWindow : NWDBasisWindow <NWDQuestWindow>
	{
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_BASE+"Quest"+NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, 2010)]
		//-------------------------------------------------------------------------------------------------------------
		public static void MenuMethod ()
		{
			EditorWindow tWindow = EditorWindow.GetWindow (typeof(NWDQuestWindow));
			tWindow.Show ();
		}
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif