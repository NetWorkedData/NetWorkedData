//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:48:52
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================



using System;
using UnityEngine;
using SQLite4Unity3d;
#if UNITY_EDITOR
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[NWDTypeWindowParamAttribute("Barter",
		"Barter window description",
		new Type[] {
			typeof(NWDBarterPlace), 
			typeof(NWDUserBarterRequest), 
			typeof(NWDUserBarterProposition),
            typeof(NWDUserBarterFinder),
			/* Add NWDBasis here*/
		}
	)]
	public class NWDBarterWindow : NWDBasisWindow <NWDBarterWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem (NWDConstants.K_MENU_BASE+ "Marketplaces/Barter"+NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, 550)]
		public static void MenuMethod ()
		{
			EditorWindow tWindow = EditorWindow.GetWindow (typeof(NWDBarterWindow));
			tWindow.Show ();
		}
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif