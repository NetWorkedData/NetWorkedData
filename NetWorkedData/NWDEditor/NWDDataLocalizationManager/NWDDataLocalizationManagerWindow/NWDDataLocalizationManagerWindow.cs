using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
	public class NWDDataLocalizationManagerWindow : NWDBasisWindow <NWDDataLocalizationManagerWindow>
	{
//		//-------------------------------------------------------------------------------------------------------------
//		[MenuItem (NWDConstants.K_MENU_LOCALIZATION_CONFIG, false, 2000)]
//		//-------------------------------------------------------------------------------------------------------------
//		public static void MenuMethod ()
//		{
//			EditorWindow tWindow = EditorWindow.GetWindow (typeof(NWDDataLocalizationManagerWindow));
//			tWindow.Show ();
//		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDDataLocalizationManagerWindow ()
		{
		}
		//-------------------------------------------------------------------------------------------------------------
		public void OnEnable ()
		{
			mTitleKey = "Localization";
			IconOfWindow = FromGizmos("NWDIcons_03");
			mDescriptionKey = "";
			mTabTypeList = new Type[] {
				typeof(NWDDataLocalizationManager),
			};
			DefineTab ();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif
