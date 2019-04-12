// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:23:4
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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
	[NWDTypeWindowParamAttribute("NWDWindowExample_Name",
		"NWDWindowExample_Description",
		new Type[] {
        typeof(NWDExample),/* Add NWDBasis here*/
		})]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public class NWDWindowExample : NWDBasisWindow <NWDWindowExample>
	{
		//-------------------------------------------------------------------------------------------------------------
		//[MenuItem (NWDConstants.K_MENU_BASE + "NWDWindowExample_Name" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, 12345)]
		public static void MenuMethod ()
		{
			EditorWindow tWindow = EditorWindow.GetWindow (typeof(NWDWindowExample));
			tWindow.Show ();
		}
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif