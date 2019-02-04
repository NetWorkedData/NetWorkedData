//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	/// <summary>
	/// NWD app environment manager window.
	/// </summary>
	public class NWDEditorConfigurationManagerWindow : NWDBasisWindow <NWDAppEnvironmentManagerWindow>
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDEditorConfigurationManagerWindow()
		{
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the OnEnable event.
		/// </summary>
		public void OnEnable ()
		{
            this.minSize = new Vector2(300, 600);
            this.maxSize = new Vector2(600, 4096);
			mTitleKey = "Editor Configuration";
			//IconOfWindow = FromGizmos("NWDIcons_03");
            IconOfWindow = NWDFindPackage.EditorTexture("NWDIcons_03");
			mDescriptionKey = "Editor Configuration";
			mTabTypeList = new Type[] {
				typeof(NWDEditorConfigurationManager),
			};
			DefineTab ();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
