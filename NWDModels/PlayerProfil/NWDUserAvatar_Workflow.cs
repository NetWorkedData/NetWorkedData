//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:20
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
    public partial class NWDUserAvatar : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserAvatar()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserAvatar(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDItem GetNWDItemAvatar()
        {
            NWDItem rItem = null;
            NWDUserAvatar[] tAvatars = NWDBasisHelper.GetReachableDatas<NWDUserAvatar>();
            if (tAvatars.Length > 0)
            {
                return tAvatars[0].RenderItem.GetRawData();
            }
            return rItem;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Sprite GetTextureAvatar()
        {
            Sprite rSprite = null;
            NWDUserAvatar[] tAvatars = NWDBasisHelper.GetReachableDatas<NWDUserAvatar>();
            if (tAvatars.Length > 0)
            {
                return tAvatars[0].RenderTexture.ToSprite();
            }
            return rSprite;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDItem GetNWDItem()
        {
            return RenderItem.GetRawData();
        }
        //-------------------------------------------------------------------------------------------------------------
        public Sprite GetTexture()
        {
            return RenderTexture.ToSprite();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================