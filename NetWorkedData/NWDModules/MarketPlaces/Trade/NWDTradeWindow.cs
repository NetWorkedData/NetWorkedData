using System;

using UnityEngine;

using SQLite4Unity3d;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDTypeWindowParamAttribute("Trade",
		"Trade window description",
		"NWDIcons_02",
		new Type[] {
			typeof(NWDTradePlace),
			typeof(NWDTradeProposition),
			typeof(NWDTradeRequest),
			typeof(NWDTradeNotification),
			/* Add NWDBasis here*/
		}
	)]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDTradeWindow : NWDBasisWindow <NWDTradeWindow>
	{
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_BASE + "Trade" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, 100)]
		//-------------------------------------------------------------------------------------------------------------
		public static void MenuMethod ()
		{
			EditorWindow tWindow = EditorWindow.GetWindow (typeof(NWDTradeWindow));
			tWindow.Show ();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif