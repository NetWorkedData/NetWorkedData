//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UAV")]
    [NWDClassDescriptionAttribute("Avatar composer for user")]
    [NWDClassMenuNameAttribute("User Avatar")]
    public partial class NWDUserAvatar : NWDBasis<NWDUserAvatar>
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