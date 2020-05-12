//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("AAV")]
    [NWDClassDescriptionAttribute("Avatar composer for account")]
    [NWDClassMenuNameAttribute("Account Avatar")]
    public partial class NWDAccountAvatar : NWDBasisAccountDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupReset]
        [NWDInspectorGroupStart("Final render")]
        [NWDTooltips("Item used to render Avatar in simple game ")]
        public NWDReferenceType<NWDItem> RenderItem
        {
            get; set;
        }
        [NWDTooltips("PNG bytes file used to render Avatar in game (use as picture or as render)")]
        public NWDImagePNGType RenderTexture
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================