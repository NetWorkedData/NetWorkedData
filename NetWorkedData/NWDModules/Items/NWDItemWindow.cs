using System;

using UnityEngine;

using SQLite4Unity3d;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDTypeWindowParamAttribute("Items",
		"Items Edition",
		"NWDIcons_02",
		new Type[] {
			typeof(NWDItemGroup), 
			typeof(NWDItem), 
			typeof(NWDItemExtension), 
			typeof(NWDBattleProperty), 
//			typeof(NWDItemPack), 
//			typeof(NWDPack), 
//			typeof(NWDSpent), 
//			typeof(NWDInAppPack), 
//			typeof(NWDTransaction), 
			typeof(NWDOwnership), 
			typeof(NWDUsage),
			/* Add NWDBasis here*/
		}
	)]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDItemWindow : NWDBasisWindow <NWDItemWindow>
	{
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_BASE + "Item" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, 100)]
		//-------------------------------------------------------------------------------------------------------------
		public static void MenuMethod ()
		{
			EditorWindow tWindow = EditorWindow.GetWindow (typeof(NWDItemWindow));
			tWindow.Show ();
		}
	}
}
//=====================================================================================================================
#endif