// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:46:8
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    ///
    /// </summary>
    [RequireComponent(typeof(Image))]
    public class NWDNetworkWebStatutRender : NWDNetworkStatutRender
    {
        //-------------------------------------------------------------------------------------------------------------
        [HeaderAttribute("Web Sync Statut")]
        public Sprite ImageError;
        public Sprite ImageSucces;
        public Sprite[] ImageSyncArray;
        [Range(0.02f, 1.0f)]
        public float Speed;
        //-------------------------------------------------------------------------------------------------------------
        //protected Sprite OldSprite;
        protected bool IsDowloading = false;
        protected int ImageSyncIndex = 0;

        //-------------------------------------------------------------------------------------------------------------
        public new void SetNetworkState(NWDNetworkState NetworkState)
        {
            if (SpriteImage != null)
            {
                switch (NetworkState)
                {
                    case NWDNetworkState.OffLine:
                        {
                            if (ImageOffline != null)
                            {
                                SpriteImage.sprite = ImageOffline;
                            }
                        }
                        break;
                    case NWDNetworkState.OnLine:
                        {
                            if (IsDowloading == false)
                            {
                                if (ImageOnline != null)
                                {
                                    SpriteImage.sprite = ImageOnline;
                                }
                            }
                        }
                        break;
                    case NWDNetworkState.Unknow:
                        {
                            if (ImageUnknow != null)
                            {
                                SpriteImage.sprite = ImageUnknow;
                            }
                        }
                        break;
                    case NWDNetworkState.Check:
                        {
                            if (IsDowloading == false)
                            {
                                if (ImageCheck != null)
                                {
                                    SpriteImage.sprite = ImageCheck;
                                }
                            }
                        }
                        break;
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SyncStart()
        {
            IsDowloading = true;
            if (SpriteImage != null)
            {
                ImageSyncIndex = 0;
                if (ImageSyncArray != null && ImageSyncArray.Length > 0)
                {
                    if (ImageSyncArray[ImageSyncIndex] != null)
                    {
                        SpriteImage.sprite = ImageSyncArray[ImageSyncIndex];
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SyncStep()
        {
            if (SpriteImage != null)
            {
                ImageSyncIndex++;
                if (ImageSyncIndex >= ImageSyncArray.Length)
                {
                    ImageSyncIndex = 0;
                }
                if (ImageSyncArray != null && ImageSyncArray.Length > 0)
                {
                    if (ImageSyncArray[ImageSyncIndex] != null)
                    {
                        SpriteImage.sprite = ImageSyncArray[ImageSyncIndex];
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SyncSucess()
        {
            IsDowloading = false;
            if (SpriteImage != null)
            {
                if (ImageSucces != null)
                {
                    SpriteImage.sprite = ImageSucces;
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SyncError()
        {
            IsDowloading = false;
            if (SpriteImage != null)
            {
                if (ImageError != null)
                {
                    SpriteImage.sprite = ImageError;
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        // Update is called once per frame
        //void Update()
        //{
        //    if (IsDowloading == true)
        //    {
        //        ImageSyncIndex++;
        //        if (ImageSyncIndex >= ImageSyncArray.Length)
        //        {
        //            ImageSyncIndex = 0;
        //        }
        //        if (ImageSyncArray != null && ImageSyncArray.Length > 0)
        //        {
        //            if (ImageSyncArray[ImageSyncIndex] != null)
        //            {
        //                SpriteImage.sprite = ImageSyncArray[ImageSyncIndex];
        //            }
        //        }
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
