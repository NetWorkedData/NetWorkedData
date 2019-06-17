//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:46:6
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
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
        protected NWDNetworkState NetworkState;
        protected Image SpriteImage; 
        //-------------------------------------------------------------------------------------------------------------
        [HeaderAttribute("NetWork Analyze")]
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
