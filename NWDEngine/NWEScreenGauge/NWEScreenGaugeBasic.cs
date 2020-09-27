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

#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWEScreenGaugeBasic : NWEScreenGauge
    {
        [Header("Fill min values")]
        [Range(0.0F, 1.0F)]
        [Tooltip("The gauge render min value in horizontal")]
        public float HorizontalMin = 0.0f;
        [Range(0.0F, 1.0F)]
        [Tooltip("The gauge render min value in vertical")]
        public float VerticalMin = 0.0f;

        [Header("Border gauge")]
        [Tooltip("Use to color the border of gauge")]
        public Color ColorBorder = Color.black;
        [Tooltip("Determine width for the border of gauge")]
        public int Border = 1;

        [Header("Textures gauge")]
        public Texture TextureBackground;
        public Texture TextureFill;
        public Texture TextureOverLay;

        [Header("Offset position in ScreenPoint")]
        [Tooltip("Offset the position of gauge")]
        //-------------------------------------------------------------------------------------------------------------
        protected Texture2D ColorTextureBorder;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Prepare this instance.
        /// </summary>
        public override void Prepare()
        {
            base.Prepare();
            //ColorTextureBorder = null;
            if (ColorTextureBorder == null)
            {
                ColorTextureBorder = new Texture2D(1, 1);
            }
            ColorTextureBorder.SetPixel(0, 0, ColorBorder);
            ColorTextureBorder.Apply();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Draw this instance.
        /// </summary>
        public override void Draw()
        {
            // Debug.Log("NWEScreenGauge ReDraw()");
            // prepare GUI
            if (PrepareIsOk == false)
            {
                Prepare();
                PrepareIsOk = true;
            }
            else
            {
#if UNITY_EDITOR
                Prepare();
#endif
            }
            // Draw gauge
            if (IsVisible == true)
            {
                float tX = 0;
                switch (AlignHorizontal)
                {
                    case NWEScreenAlignHorizontal.Left:
                        {
                            tX = (Size.x / 2.0F);
                        }
                        break;
                    case NWEScreenAlignHorizontal.Middle:
                        {
                            tX = (Screen.width / 2.0F);
                        }
                        break;
                    case NWEScreenAlignHorizontal.Right:
                        {
                            tX = Screen.width - (Size.x / 2.0F);
                        }
                        break;
                }
                float tY = 0;
                switch (AlignVertical)
                {
                    case NWEScreenAlignVertical.Bottom:
                        {
                            tY = (Size.y / 2.0F);
                        }
                        break;
                    case NWEScreenAlignVertical.Center:
                        {
                            tY = (Screen.height / 2.0F);
                        }
                        break;
                    case NWEScreenAlignVertical.Top:
                        {
                            tY = Screen.height - (Size.y / 2.0F);
                        }
                        break;
                }
                Vector2 ScreenPos = new Vector2(tX, tY);
                Vector2Int ScreenPosition = new Vector2Int((int)(ScreenPos.x - Size.x / 2.0F + Offset.x), (int)(Screen.height - ScreenPos.y - Size.y / 2.0F + Offset.y));
                Rect tRectBackground = new Rect(ScreenPosition, Size);
                Vector2 tFillPosition;
                Vector2 tFillSize;
                float tH = Mathf.Max(HorizontalMin, HorizontalValue);
                float tV = Mathf.Max(VerticalMin, VerticalValue);
                switch (Reverse)
                {
                    case NWEScreenGaugeFill.Reverse:
                        {
                            tFillPosition = new Vector2Int((int)(ScreenPosition.x + Size.x), (int)(ScreenPosition.y + Size.y));
                            tFillSize = new Vector2Int((int)(-Size.x * tH), (int)(-Size.y * tV));
                        }
                        break;
                    case NWEScreenGaugeFill.Center:
                        {
                            tFillSize = new Vector2(Size.x * tH, Size.y * tV);
                            tFillPosition = new Vector2(ScreenPosition.x + (Size.x - tFillSize.x) / 2.0F, ScreenPosition.y + (Size.y - tFillSize.y) / 2.0F);
                        }
                        break;
                    case NWEScreenGaugeFill.Perim:
                        {
                            tH = Mathf.Min(1 - HorizontalMin, HorizontalValue);
                            tV = Mathf.Min(1 - VerticalMin, VerticalValue);

                            tFillSize = new Vector2(Size.x * (1.0F - tH), Size.y * (1.0F - tV));
                            tFillPosition = new Vector2(ScreenPosition.x + (Size.x - tFillSize.x) / 2.0F, ScreenPosition.y + (Size.y - tFillSize.y) / 2.0F);
                        }
                        break;
                    case NWEScreenGaugeFill.Normal:
                    default:
                        {
                            tFillPosition = new Vector2Int((int)(ScreenPosition.x), (int)(ScreenPosition.y));
                            tFillSize = new Vector2Int((int)(Size.x * tH), (int)(Size.y * tV));
                        }
                        break;
                }
                Rect tRectFill = new Rect(tFillPosition, tFillSize);
                // draw SpriteBackground
                if (TextureBackground != null)
                {
                    GUI.DrawTexture(tRectBackground, TextureBackground, ScaleMode.StretchToFill);
                }
                else
                {
                    GUI.DrawTexture(tRectBackground, ColorTextureBackground);
                }
                // draw SpriteFill
                if (TextureFill != null)
                {
                    GUI.DrawTexture(tRectFill, TextureFill, ScaleMode.StretchToFill);
                }
                else
                {
                    GUI.DrawTexture(tRectFill, ColorTextureFill);
                }
                // draw SpriteOverLay
                if (TextureOverLay != null)
                {
                    GUI.DrawTexture(tRectBackground, TextureOverLay, ScaleMode.StretchToFill);
                }
                else if (Border != 0)
                {
                    DrawRectBorder(tRectBackground, ColorTextureBorder, -Border);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //private void OnSceneGUI()
        //{
        //    Debug.Log("NWEScreenGaugeBasic OnSceneGUI()");
        //    //DrawOnScene();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        private void OnDrawGizmosSelected()
        {
            Rect tScreen = Camera.main.pixelRect;
            //Debug.Log("tScreen " + tScreen.ToString());
            if (IsVisible == true)
            {
                float tX = 0;
                switch (AlignHorizontal)
                {
                    case NWEScreenAlignHorizontal.Left:
                        {
                            tX = (Size.x / 2.0F);
                        }
                        break;
                    case NWEScreenAlignHorizontal.Middle:
                        {
                            tX = (tScreen.width / 2.0F);
                        }
                        break;
                    case NWEScreenAlignHorizontal.Right:
                        {
                            tX = tScreen.width - (Size.x / 2.0F);
                        }
                        break;
                }
                float tY = 0;
                switch (AlignVertical)
                {
                    case NWEScreenAlignVertical.Bottom:
                        {
                            tY = (Size.y / 2.0F);
                        }
                        break;
                    case NWEScreenAlignVertical.Center:
                        {
                            tY = (tScreen.height / 2.0F);
                        }
                        break;
                    case NWEScreenAlignVertical.Top:
                        {
                            tY = tScreen.height - (Size.y / 2.0F);
                        }
                        break;
                }
                Vector2 ScreenPos = new Vector2(tX, tY);
                //Debug.Log("ScreenPos " + ScreenPos.ToString());
                Vector2Int ScreenPosition = new Vector2Int((int)(ScreenPos.x - Size.x / 2.0F + Offset.x), (int)(ScreenPos.y - Size.y / 2.0F + Offset.y));
                Rect tRectBackground = new Rect(ScreenPosition, Size);
                Vector2 tFillPosition;
                Vector2 tFillSize;
                float tH = Mathf.Max(HorizontalMin, HorizontalValue);
                float tV = Mathf.Max(VerticalMin, VerticalValue);
                switch (Reverse)
                {
                    case NWEScreenGaugeFill.Reverse:
                        {
                            tFillPosition = new Vector2Int((int)(ScreenPosition.x + Size.x), (int)(ScreenPosition.y + Size.y));
                            tFillSize = new Vector2Int((int)(-Size.x * tH), (int)(-Size.y * tV));
                        }
                        break;
                    case NWEScreenGaugeFill.Center:
                        {
                            tFillSize = new Vector2(Size.x * tH, Size.y * tV);
                            tFillPosition = new Vector2(ScreenPosition.x + (Size.x - tFillSize.x) / 2.0F, ScreenPosition.y + (Size.y - tFillSize.y) / 2.0F);
                        }
                        break;
                    case NWEScreenGaugeFill.Perim:
                        {
                            tH = Mathf.Min(1 - HorizontalMin, HorizontalValue);
                            tV = Mathf.Min(1 - VerticalMin, VerticalValue);

                            tFillSize = new Vector2(Size.x * (1.0F - tH), Size.y * (1.0F - tV));
                            tFillPosition = new Vector2(ScreenPosition.x + (Size.x - tFillSize.x) / 2.0F, ScreenPosition.y + (Size.y - tFillSize.y) / 2.0F);
                        }
                        break;
                    case NWEScreenGaugeFill.Normal:
                    default:
                        {
                            tFillPosition = new Vector2Int((int)(ScreenPosition.x), (int)(ScreenPosition.y));
                            tFillSize = new Vector2Int((int)(Size.x * tH), (int)(Size.y * tV));
                        }
                        break;
                }
                Rect tRectFill = new Rect(tFillPosition, tFillSize);
                // draw SpriteBackground
                if (TextureBackground != null)
                {
                    Gizmos.DrawGUITexture(tRectBackground, TextureBackground);
                }
                else
                {
                    Gizmos.DrawGUITexture(tRectBackground, ColorTextureBackground);
                }
                // draw SpriteFill
                if (TextureFill != null)
                {
                    Gizmos.DrawGUITexture(tRectFill, TextureFill);
                }
                else
                {
                    Gizmos.DrawGUITexture(tRectFill, ColorTextureFill);
                }
                // draw SpriteOverLay
                if (TextureOverLay != null)
                {
                    Gizmos.DrawGUITexture(tRectBackground, TextureOverLay);
                }
                else if (Border != 0)
                {
                    GizmosDrawRectBorder(tRectBackground, ColorTextureBorder, -Border);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
