// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:16
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
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("AAV")]
    [NWDClassDescriptionAttribute("Avatar composer for account")]
    [NWDClassMenuNameAttribute("Account Avatar")]
    public partial class NWDAccountAvatar : NWDBasis<NWDAccountAvatar>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupReset]
        [NWDInspectorGroupStart("Account and final render")]
        [NWDTooltips("The account reference of user")]
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
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