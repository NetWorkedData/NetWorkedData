//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	/// <summary>
	/// NWD app environment manager window.
	/// </summary>
	public class NWDAppEnvironmentManagerWindow : NWDBasisWindow <NWDAppEnvironmentManagerWindow>
	{
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

            this.minSize = new Vector2(300, 600);
            this.maxSize = new Vector2(600, 4096);
			mTitleKey = "Environments";
			IconOfWindow = FromGizmos("NWDIcons_03");
			mDescriptionKey = "Environments Edition";
			mTabTypeList = new Type[] {
				typeof(NWDAppEnvironmentManager),
			};
			DefineTab ();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
