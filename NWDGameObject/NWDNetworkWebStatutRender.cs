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
