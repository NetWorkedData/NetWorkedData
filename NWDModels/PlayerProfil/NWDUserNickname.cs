//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:27
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserInfos : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupOrder("Player Informations", 3)]
        public NWDReferenceType<NWDUserNickname> Nickname { get; set; }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
#if UNITY_EDITOR
    public partial class NWDUserWindow : NWDBasisWindow<NWDUserWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "User/Nickname", false, 301)]
        public static void MenuMethodNickname()
        {
            ShowWindow(typeof(NWDUserNickname));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
#endif
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UNN")]
    [NWDClassDescriptionAttribute("User Nickname")]
    [NWDClassMenuNameAttribute("User Nickname")]
    [NWDClassClusterAttribute(1, 32)]
#if UNITY_EDITOR
    [NWDWindowOwner(typeof(NWDUserWindow))]
#endif
    public partial class NWDUserNickname : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorHeader("Player Informations")]
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        public NWDReferenceType<NWDGameSave> GameSave
        {
            get; set;
        }
        public string Nickname
        {
            get; set;
        }
        [NWDNotEditable]
        public string UniqueNickname
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================