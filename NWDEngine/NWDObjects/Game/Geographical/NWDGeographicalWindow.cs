//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date        2019-4-12 18:34:57
//  Author        Kortex (Jean-François CONTART) 
//  Email        jfcontart@idemobi.com
//  Project     NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDTypeWindowParamAttribute("Geographical",
        "Create sector and area to sort and order your game, objects, items, etc.",
        new Type[] {
            typeof(NWDWorld),
            typeof(NWDSector),
            typeof(NWDArea),
            /* Add NWDBasis here*/
        }
    )]
    public class NWDGeographicalWindow : NWDBasisWindow<NWDGeographicalWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Game/Geographical", false, 221)]
        public static void MenuMethod()
        {
            EditorWindow tWindow = EditorWindow.GetWindow(typeof(NWDGeographicalWindow));
            tWindow.Show();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif