using System;

using UnityEngine;

using SQLite4Unity3d;

#if UNITY_EDITOR
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDTypeWindowParamAttribute("Story",
		"settings",
		"Story window description",
		new Type[] {
			typeof(NWDQuest),
			typeof(NWDDialog),
			typeof(NWDCharacter),
			typeof(NWDQuestUserAdvancement),
			/* Add NWDBasis here*/
		}
	)]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDStoryWindow : NWDBasisWindow <NWDStoryWindow>
	{
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_BASE+"Story"+NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, 100)]
		//-------------------------------------------------------------------------------------------------------------
		public static void MenuMethod ()
		{
			EditorWindow tWindow = EditorWindow.GetWindow (typeof(NWDStoryWindow));
			tWindow.Show ();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif