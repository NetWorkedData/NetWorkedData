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
        [NWDGroupReset]
        [NWDGroupStart("Account and final render")]
        [NWDTooltips("The account reference of user")]
        public NWDReferenceType<NWDAccount> Account { get; set; }
        [NWDTooltips("Item used to render Avatar in simple game ")]
        public NWDReferenceType<NWDItem> RenderItem { get; set; }
        [NWDTooltips("PNG bytes file used to render Avatar in game (use as picture or as render)")]
        public NWDImagePNGType RenderTexture { get; set; }
        //-------------------------------------------------------------------------------------------------------------
        //public NWDAccountAvatar()
        //{

        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public NWDAccountAvatar(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        //{

        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        //{
           
        //}
        ////-------------------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// Get Account NWDItem Avatar
        ///// </summary>
        //public static NWDItem GetNWDItemAvatar()
        //{
        //    NWDItem rItem = null;
        //    NWDAccountAvatar[] tAvatars = FindDatas();
        //    if (tAvatars.Length > 0)
        //    {
        //        return tAvatars[0].RenderItem.GetObject();
        //    }
        //    return rItem;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// Get Account Sprite Avatar
        ///// </summary>
        //public static Sprite GetTextureAvatar()
        //{
        //    Sprite rSprite = null;
        //    NWDAccountAvatar[] tAvatars = FindDatas();
        //    if (tAvatars.Length > 0)
        //    {
        //        return tAvatars[0].RenderTexture.ToSprite();
        //    }
        //    return rSprite;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// Get Account NWDItem Avatar
        ///// </summary>
        //public NWDItem GetNWDItem()
        //{
        //    return RenderItem.GetObject();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// Get Account Sprite Avatar
        ///// </summary>
        //public Sprite GetTexture()
        //{
        //    return RenderTexture.ToSprite();
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================