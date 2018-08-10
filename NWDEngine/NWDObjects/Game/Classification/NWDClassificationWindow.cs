//=====================================================================================================================
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
    /// <summary>
    /// NWDClassificationWindow show NWDBasisWindow for classification NWDBasis Class.
    /// </summary>
    [NWDTypeWindowParamAttribute("Classification",
        "Create objects to sort and order your game, objects, items, etc.",
        "NWDClassificationWindow",
        new Type[] {
            typeof(NWDWorld),
            typeof(NWDDistrict),
            typeof(NWDCategory),
            typeof(NWDFamily),
            typeof(NWDKeyword),
			/* Add NWDBasis here*/
		}
    )]
    public class NWDClassificationWindow : NWDBasisWindow<NWDClassificationWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Game/Classification", false, 220)]
        //-------------------------------------------------------------------------------------------------------------
        public static void MenuMethod()
        {
            EditorWindow tWindow = EditorWindow.GetWindow(typeof(NWDClassificationWindow));
            tWindow.Show();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif