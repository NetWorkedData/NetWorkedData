using System;

using UnityEngine;

using SQLite4Unity3d;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
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
		//-------------------------------------------------------------------------------------------------------------
		public NWDItemWindow ()
		{
		}
		//-------------------------------------------------------------------------------------------------------------
		public void OnEnable ()
		{
			mTitleKey = "Items";
			mDescriptionKey = "•Items Edition, You can add, change, remove the item of your game here. Everythings can be item : money, gold, dress. The item can be win, buy in the pack, etc.";
			mTabTypeList = new Type[] {
				typeof(NWDItem), 
				typeof(NWDItemExtension), 
				typeof(NWDBattleProperty), 
				typeof(NWDItemPack), 
				typeof(NWDPack), 
				typeof(NWDSpent), 
				typeof(NWDInAppPack), 
				typeof(NWDTransaction), 
				typeof(NWDOwnership), 
				typeof(NWDUsage),
			};
			DefineTab ();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif