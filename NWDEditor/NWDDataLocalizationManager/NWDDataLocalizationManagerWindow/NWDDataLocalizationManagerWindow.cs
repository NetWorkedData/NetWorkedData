﻿//=====================================================================================================================
//
// ideMobi copyright 2017 
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
	public class NWDDataLocalizationManagerWindow : NWDBasisWindow <NWDDataLocalizationManagerWindow>
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDDataLocalizationManagerWindow ()
		{
		}
		//-------------------------------------------------------------------------------------------------------------
		public void OnEnable ()
		{
			mTitleKey = "Localization";
            //IconOfWindow = FromGizmos("NWDIcons_03");
            IconOfWindow = NWDFindPackage.EditorTexture("NWDIcons_03");
			mDescriptionKey = string.Empty;
			mTabTypeList = new Type[] {
				typeof(NWDDataLocalizationManager),
			};
			DefineTab ();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
