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
	[NWDTypeWindowParamAttribute("Classification",
		"World Edition, You can add, change, remove the item of your game here." +
		" Everythings can be item : money, gold, dress. The item can be win, buy in the pack, etc.",
		"NWDIcons_02",
		new Type[] {
			typeof(NWDWorld), 
			typeof(NWDCategory), 
			typeof(NWDFamily), 
			typeof(NWDKeyword),
			/* Add NWDBasis here*/
		}
	)]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDClassificationWindow : NWDBasisWindow <NWDClassificationWindow>
	{
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_BASE + "Classification" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, 52)]
		//-------------------------------------------------------------------------------------------------------------
		public static void MenuMethod ()
		{
			EditorWindow tWindow = EditorWindow.GetWindow (typeof(NWDClassificationWindow));
			tWindow.Show ();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif