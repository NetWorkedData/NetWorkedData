//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:50:44
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
    [NWDTypeWindowParamAttribute("Quest",
        "Quest window description",
            new Type[] {
            typeof(NWDSetOfQuests),
            typeof(NWDQuest),
            typeof(NWDDialog),
            typeof(NWDCharacter),
            typeof(NWDAction),
            typeof(NWDUserQuestAdvancement),
            typeof(NWDYoghurtLyric),
		}
    )]
    public class NWDQuestWindow : NWDBasisWindow<NWDQuestWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Quests", false, 535)]
        public static void MenuMethod()
        {
            EditorWindow tWindow = EditorWindow.GetWindow(typeof(NWDQuestWindow));
            tWindow.Show();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif