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
