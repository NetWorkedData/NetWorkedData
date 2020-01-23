//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:34:21
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDTypeWindowParamAttribute("Classification",
        "Create objects to sort and order your game, objects, items, etc.",
        new Type[] {
            typeof(NWDKeyword),
            typeof(NWDCategory),
            typeof(NWDFamily),
			/* Add NWDBasis here*/
		}
    )]
    public class NWDClassificationWindow : NWDBasisWindow<NWDClassificationWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Game/Classification", false, 220)]
        public static void MenuMethod()
        {
            ShowWindow();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif