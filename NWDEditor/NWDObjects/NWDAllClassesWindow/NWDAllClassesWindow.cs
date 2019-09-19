//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:23:3
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
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
	[NWDTypeWindowParamAttribute("NWD All",
		"All NWD Basis herited classes edition window.",
         "NWDAllClassesWindow",
		null
                                )]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public class NWDAllClassesWindow : NWDBasisWindow <NWDAllClassesWindow>
	{
		//-------------------------------------------------------------------------------------------------------------
		[MenuItem (NWDConstants.K_MENU_ALL_CLASSES, false, 100)]
		public static void MenuMethod ()
		{
			EditorWindow tWindow = GetWindow (typeof(NWDAllClassesWindow));
			tWindow.Show ();
		}
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
