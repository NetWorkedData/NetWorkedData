//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
#if NWD_USER_IDENTITY
using System;
using UnityEngine;
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEditor;
using NetWorkedData.NWDEditor;
#endif
//=====================================================================================================================
#if UNITY_EDITOR
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserWindow : NWDBasisWindow<NWDUserWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_PREFERENCES_MENU + "/Avatar", false, NWDEditorMenu.K_PLAYER_MANAGEMENT_INDEX + 20)]
        public static void MenuMethodAvatar()
        {
            ShowWindow(typeof(NWDUserAvatar));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
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
    [NWDClassMacro("NWD_USER_IDENTITY")]
    [NWDClassTrigrammeAttribute("UAV")]
    [NWDClassDescriptionAttribute("Avatar composer for user")]
    [NWDClassMenuNameAttribute("User Avatar")]
#if UNITY_EDITOR
    [NWDClassClusterAttribute(1, 32)]
    [NWDWindowOwner(typeof(NWDUserWindow))]
#endif
    public partial class NWDUserAvatar : NWDBasisGameSaveDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupReset]
        [NWDInspectorGroupStart("Final render")]
        [NWDTooltips("Item used to render Avatar in simple game ")]
        public NWDReferenceType<NWDItem> RenderItem { get; set; }
        [NWDTooltips("PNG bytes file used to render Avatar in game (use as picture or as render)")]
        public NWDImagePNGType RenderTexture { get; set; }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
