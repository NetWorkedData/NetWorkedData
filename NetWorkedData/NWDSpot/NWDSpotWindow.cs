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
    [NWDTypeWindowParamAttribute("SOB",
                                 "SOB management … ",
                                 "NWDIcons_02",
		new Type[] {
			typeof(NWDSpot),
			typeof(NWDArena),
			typeof(NWDEmbassy),
			typeof(NWDSpotUser),
            typeof(NWDUserInfos),
			/* Add NWDBasis here*/
		}
	)]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDSpotWindow : NWDBasisWindow <NWDSpotWindow>
	{
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_BASE + "SOB" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, 2003)]
		//-------------------------------------------------------------------------------------------------------------
		public static void MenuMethod ()
		{
			EditorWindow tWindow = GetWindow (typeof(NWDSpotWindow));
			tWindow.Show ();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================
#endif 