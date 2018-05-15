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
    public class NWDNetworkStatutRender : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        private NWDNetworkState NetworkState;
        private Image SpriteImage; 
        //-------------------------------------------------------------------------------------------------------------
        public Sprite ImageOnline;
        public Sprite ImageOffline;
        public Sprite ImageUnknow;
        public Sprite ImageCheck;
        //-------------------------------------------------------------------------------------------------------------
        // Use this for initialization
        void Start()
        {
            SpriteImage = GetComponent<Image>();
            SetNetworkState(NWDNetworkState.Unknow);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetNetworkState(NWDNetworkState NetworkState)
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
                            if (ImageOnline != null)
                            {
                                SpriteImage.sprite = ImageOnline;
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
                            if (ImageCheck != null)
                            {
                                SpriteImage.sprite = ImageCheck;
                            }
                        }
                        break;
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        // Update is called once per frame
        void Update()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
