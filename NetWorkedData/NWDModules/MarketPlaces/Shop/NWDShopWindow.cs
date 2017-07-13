using System;

using UnityEngine;

using SQLite4Unity3d;

#if UNITY_EDITOR
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDTypeWindowParamAttribute("Shop",
		"settings",
		"Shop window description",
		new Type[] {
			typeof(NWDShop),
			typeof(NWDRack), 
			typeof(NWDPack), 
			typeof(NWDItemPack), 
			typeof(NWDSpent),
			/* Add NWDBasis here*/
		}
	)]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDShopWindow : NWDBasisWindow <NWDShopWindow>
	{
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_BASE + "Shop" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, 100)]
		//-------------------------------------------------------------------------------------------------------------
		public static void MenuMethod ()
		{
			EditorWindow tWindow = EditorWindow.GetWindow (typeof(NWDShopWindow));
			tWindow.Show ();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif