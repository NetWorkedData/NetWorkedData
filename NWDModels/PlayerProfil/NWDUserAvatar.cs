//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:18
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserInfos : NWDBasisGameSaveDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupOrder("Player Informations", 4)]
        public NWDReferenceType<NWDUserAvatar> Avatar { get; set; }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
#if UNITY_EDITOR
    public partial class NWDUserWindow : NWDBasisWindow<NWDUserWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "User/Avatar", false, 302)]
        public static void MenuMethodAvatar()
        {
            ShowWindow(typeof(NWDUserAvatar));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
#endif
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UAV")]
    [NWDClassDescriptionAttribute("Avatar composer for user")]
    [NWDClassMenuNameAttribute("User Avatar")]
    [NWDClassClusterAttribute(1, 32)]
#if UNITY_EDITOR
    [NWDWindowOwner(typeof(NWDUserWindow))]
#endif
    public partial class NWDUserAvatar : NWDBasisGameSaveDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupReset]
        [NWDInspectorGroupStart("Account and final render")]
        [NWDTooltips("The account reference of user")]
        public NWDReferenceType<NWDAccount> Account { get; set; }
        public NWDReferenceType<NWDGameSave> GameSave { get; set; }
        [NWDTooltips("Item used to render Avatar in simple game ")]
        public NWDReferenceType<NWDItem> RenderItem { get; set; }
        [NWDTooltips("PNG bytes file used to render Avatar in game (use as picture or as render)")]
        public NWDImagePNGType RenderTexture { get; set; }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================