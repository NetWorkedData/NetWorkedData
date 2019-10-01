//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:19
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================


using System;
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountAvatar : NWDBasis
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
        public static NWDItem GetItemAvatar()
        {
            NWDItem rItem = null;
            NWDAccountAvatar[] tAvatars = NWDBasisHelper.GetReachableDatas<NWDAccountAvatar>();
            if (tAvatars.Length > 0)
            {
                return tAvatars[0].RenderItem.GetRawData();
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
            NWDAccountAvatar[] tAvatars = NWDBasisHelper.GetReachableDatas<NWDAccountAvatar>();
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
        public NWDItem GetItem()
        {
            return RenderItem.GetRawData();
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