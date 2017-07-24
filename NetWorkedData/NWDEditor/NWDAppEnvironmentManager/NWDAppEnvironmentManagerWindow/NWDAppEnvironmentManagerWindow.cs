using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
	/// <summary>
	/// NWD app environment manager window.
	/// </summary>
	public class NWDAppEnvironmentManagerWindow : NWDBasisWindow <NWDAppEnvironmentManagerWindow>
	{
//		//-------------------------------------------------------------------------------------------------------------
//		[MenuItem (NWDConstants.K_MENU_ENVIRONMENT_EDIT, false, 24)]
//		//-------------------------------------------------------------------------------------------------------------
//		/// <summary>
//		/// Menus the method.
//		/// </summary>
//		public static void MenuMethod ()
//		{
//			EditorWindow tWindow = EditorWindow.GetWindow (typeof(NWDAppEnvironmentManagerWindow));
//			tWindow.Show ();
//		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="NetWorkedData.NWDAppEnvironmentManagerWindow"/> class.
		/// </summary>
		public NWDAppEnvironmentManagerWindow ()
		{
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the OnEnable event.
		/// </summary>
		public void OnEnable ()
		{
			mTitleKey = "Environments";
			IconOfWindow = FromGizmos("NWDIcons_03");
			mDescriptionKey = "Environments Edition, You can add, change, remove the item of your game here. Everythings can be item : money, gold, dress. The item can be win, buy in the pack, etc.";
			mTabTypeList = new Type[] {
				typeof(NWDAppEnvironmentManager),
			};
			DefineTab ();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif
