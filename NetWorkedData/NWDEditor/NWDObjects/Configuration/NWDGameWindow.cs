﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDTypeWindowParamAttribute("Game",
		"settings",
		"Project Edition, You can add, change, remove the item of your game here. " +
		"Everythings can be item : money, gold, dress. The item can be win, buy in the pack, etc.",
		new Type[] { 
			typeof(NWDGameVersion),
			typeof(NWDGameConfiguration),
			typeof(NWDKeyword),
			typeof(NWDError), 
			/* Add NWDBasis here*/
		}
	)]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDGameWindow : NWDBasisWindow <NWDGameWindow>
	{
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_GAME, false, 30)]
		//-------------------------------------------------------------------------------------------------------------
		public static void MenuMethod ()
		{
			EditorWindow tWindow = EditorWindow.GetWindow (typeof(NWDGameWindow));
			tWindow.Show ();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif