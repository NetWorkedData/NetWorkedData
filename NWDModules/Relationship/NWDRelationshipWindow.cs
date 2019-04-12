// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:51:9
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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
        [MenuItem (NWDConstants.K_MENU_BASE+ "Relationship", false, 550)]
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