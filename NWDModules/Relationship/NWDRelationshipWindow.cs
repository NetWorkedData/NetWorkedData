//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR

using System;
using UnityEngine;
using SQLite4Unity3d;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[NWDTypeWindowParamAttribute("Relationship",
        "Relationship window description",
		new Type[] {
            typeof(NWDRelationshipPlace),
            typeof(NWDAccountRelationship),
            typeof(NWDUserRelationship),
        }
	)]
	public class NWDRelationshipWindow : NWDBasisWindow <NWDRelationshipWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem (NWDConstants.K_MENU_BASE+ "Relationship" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, 550)]
		public static void MenuMethod ()
		{
			EditorWindow tWindow = EditorWindow.GetWindow (typeof(NWDRelationshipWindow));
			tWindow.Show ();
		}
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif