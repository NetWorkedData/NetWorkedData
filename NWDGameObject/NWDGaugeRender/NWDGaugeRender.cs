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
    [ExecuteInEditMode] // I do that to run this object in edit mode too (on scene)
    public class NWDGaugeRender : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        [Header("Connect images")]
        public Image ImageBackground;
        public Image ImageFill;
        public Image ImageOverLay;
        //-------------------------------------------------------------------------------------------------------------
        [Header("Animator zone")]
        public Animator Anim;
        public CanvasGroup Group;
        //-------------------------------------------------------------------------------------------------------------
        [Header("Expand zone")]
        public bool HorizontalExpand = true;
        public float HorizontalMin = 30.0f;
        public bool VerticalExpand = false;
        public float VerticalMin = 30.0f;
        //-------------------------------------------------------------------------------------------------------------
        [Header("Expand value")]
        [Range(0.0F, 1.0F)]
        public float HorizontalValue = 1.0f;
        [Range(0.0F, 1.0F)]
        public float VerticalValue = 1.0f;
        private bool IsHidden = true;
        //-------------------------------------------------------------------------------------------------------------
        public void SetHidden(bool sValue)
        {
            if (IsHidden != sValue)
            {
                IsHidden = sValue;
                if (IsHidden == false)
                {
                    Anim.SetTrigger("Start");
                }
                else
                {
                    Anim.SetTrigger("Finish");
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetHorizontalValue(float sValue)
        {
            if (sValue > 1.0F)
            {
                sValue = 1.0F;
            }
            if (sValue < 0)
            {
                sValue = 0.0F;
            }
            HorizontalValue = sValue;
            ReDraw();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetVerticalValue(float sValue)
        {
            if (sValue > 1.0F)
            {
                sValue = 1.0F;
            }
            if (sValue < 0)
            {
                sValue = 0.0F;
            }
            VerticalValue = sValue;
            ReDraw();
        }
        //-------------------------------------------------------------------------------------------------------------
        void Awake()
        {
            ReDraw();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Update()
        {
#if UNITY_EDITOR
            ReDraw ();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        void ReDraw()
        {
            if (ImageBackground != null && ImageFill != null)
            {
                Rect tRect = ImageBackground.rectTransform.rect;
                float tW = tRect.width;
                if (HorizontalExpand)
                {
                    tW = HorizontalMin + (tRect.width - HorizontalMin) * HorizontalValue;
                }
                float tH = tRect.height;
                if (VerticalExpand)
                {
                    tH = VerticalMin + (tRect.height - VerticalMin) * VerticalValue;
                }
                ImageFill.rectTransform.sizeDelta = new Vector2(tW, tH);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            Anim = GetComponent<Animator>();
            Group = GetComponent<CanvasGroup>();
            Group.alpha = 0.0F;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
