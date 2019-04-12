// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:41:43
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

using System;

using UnityEngine;

using SQLite4Unity3d;

#if UNITY_EDITOR
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDTypeWindowParamAttribute("Levels",
        "Level Edition, You can add, change",
                                 "NWDLevelsWindow",
        new Type[] {
            typeof(NWDLevel),
            typeof(NWDUserLevelScore),
			/* Add NWDBasis here*/
		}
    )]
    public class NWDLevelWindow : NWDBasisWindow<NWDLevelWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Game/Levels", false, 201)]
        public static void MenuMethod()
        {
            EditorWindow tWindow = EditorWindow.GetWindow(typeof(NWDLevelWindow));
            tWindow.Show();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif