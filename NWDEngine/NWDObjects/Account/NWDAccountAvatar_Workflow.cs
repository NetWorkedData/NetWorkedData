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
    public partial class NWDAccountAvatar : NWDBasis<NWDAccountAvatar>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountAvatar()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountAvatar(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get Account NWDItem Avatar
        /// </summary>
        public static NWDItem GetNWDItemAvatar()
        {
            NWDItem rItem = null;
            NWDAccountAvatar[] tAvatars = FindDatas();
            if (tAvatars.Length > 0)
            {
                return tAvatars[0].RenderItem.GetObject();
            }
            return rItem;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get Account Sprite Avatar
        /// </summary>
        public static Sprite GetTextureAvatar()
        {
            Sprite rSprite = null;
            NWDAccountAvatar[] tAvatars = FindDatas();
            if (tAvatars.Length > 0)
            {
                return tAvatars[0].RenderTexture.ToSprite();
            }
            return rSprite;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get Account NWDItem Avatar
        /// </summary>
        public NWDItem GetNWDItem()
        {
            return RenderItem.GetObject();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get Account Sprite Avatar
        /// </summary>
        public Sprite GetTexture()
        {
            return RenderTexture.ToSprite();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================