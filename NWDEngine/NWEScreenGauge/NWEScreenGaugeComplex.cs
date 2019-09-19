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
    public class NWEScreenGaugeComplex : NWEScreenGaugeBasic
    {

        [Header("Color gauge limit highlight")]
        [Tooltip("Use to color highlight of limit effect")]
        public Color ColorLimit = Color.gray;
        [Tooltip("Determine width for the border of highlight")]
        public int BorderLimit = 1;

        [Header("Text Information addon")]
        public bool ShowText = true;
        public string OverlayText = null;
        public Vector2Int OverlayTextOffset = new Vector2Int(0, 0);
        public Vector2Int OverlayTextOverset = new Vector2Int(0, 0);
        public Font OverlayFont = null;
        public int OverlaySize = 10;
        public Color OverlayColor = Color.black;
        public TextAnchor OverlayTextAnchor = TextAnchor.MiddleCenter;
        //-------------------------------------------------------------------------------------------------------------
        protected GUIStyle OverlayStyle;
        protected Texture2D ColorTextureBorderLimit;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Prepare this instance.
        /// </summary>
        public override void Prepare()
        {
            base.Prepare();
            //ColorTextureBorderLimit = null;
            if (ColorTextureBorderLimit == null)
            {
                ColorTextureBorderLimit = new Texture2D(1, 1);
            }
            ColorTextureBorderLimit.SetPixel(0, 0, ColorLimit);
            ColorTextureBorderLimit.Apply();

            // Prepare Style for Label;
            //OverlayStyle = null;
            if (OverlayStyle == null)
            {
                OverlayStyle = new GUIStyle(GUI.skin.label);
            }
            if (OverlayFont != null)
            {
                OverlayStyle.font = OverlayFont;
            }
            OverlayStyle.normal.textColor = OverlayColor;
            OverlayStyle.fontSize = OverlaySize;
            OverlayStyle.alignment = OverlayTextAnchor;
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
                // Calculate Rect
                float tX = 0;
                switch (AlignHorizontal)
                {
                    case NWEScreenAlignHorizontal.Left :
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
                            tX = Screen.width-(Size.x / 2.0F);
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
                Vector2 ScreenPos = new Vector2(tX,tY);
                Vector2Int ScreenPosition = new Vector2Int((int)(ScreenPos.x - Size.x / 2.0F + Offset.x), (int)(Screen.height - ScreenPos.y - Size.y / 2.0F + Offset.y));
                Rect tRectBackground = new Rect(ScreenPosition, Size);
                Vector2 tFillPosition;
                Vector2 tFillSize;
                Rect tRectFillLimitVertical = Rect.zero;
                Rect tRectFillLimitHorizontal = Rect.zero;
                Rect tRectFillLimitVerticalTwo = Rect.zero;
                Rect tRectFillLimitHorizontalTwo = Rect.zero;
                Rect tRectOverlayText = new Rect(ScreenPosition.x + OverlayTextOffset.x - OverlayTextOverset.x,
                                                 ScreenPosition.y + OverlayTextOffset.y - OverlayTextOverset.y,
                                                 Size.x + OverlayTextOverset.x * 2, Size.y + OverlayTextOverset.y * 2);

                float tH = Mathf.Max(HorizontalMin, HorizontalValue);
                float tV = Mathf.Max(VerticalMin, VerticalValue);
                int tBorderLimitH = BorderLimit;
                if (tBorderLimitH > (Size.x - Size.x * tH))
                {
                    tBorderLimitH = (int)(Size.x - Size.x * tH) + 1;
                }
                int tBorderLimitV = BorderLimit;
                if (tBorderLimitV > (Size.y - Size.y * tV))
                {
                    tBorderLimitV = (int)(Size.y - Size.y * tV) + 1;
                }
                switch (Reverse)
                {
                    case NWEScreenGaugeFill.Reverse:
                        {
                            tFillPosition = new Vector2Int((int)(ScreenPosition.x + Size.x), (int)(ScreenPosition.y + Size.y));
                            tFillSize = new Vector2Int((int)(-Size.x * tH), (int)(-Size.y * tV));
                            tRectFillLimitHorizontal = new Rect(tFillPosition.x + tFillSize.x, tFillPosition.y, -tBorderLimitH, -Size.y);
                            tRectFillLimitVertical = new Rect(tFillPosition.x, tFillPosition.y + tFillSize.y, -Size.x, -tBorderLimitV);
                        }
                        break;
                    case NWEScreenGaugeFill.Center:
                        {
                            if (tBorderLimitH > (Size.x - Size.x * tH) / 2.0F)
                            {
                                tBorderLimitH = (int)((Size.x - Size.x * tH) / 2.0F);
                            }
                            if (tBorderLimitV > (Size.y - Size.y * tV) / 2.0F)
                            {
                                tBorderLimitV = (int)((Size.y - Size.y * tV) / 2.0F);
                            }

                            tFillSize = new Vector2(Size.x * tH, Size.y * tV);
                            tFillPosition = new Vector2(ScreenPosition.x + (Size.x - tFillSize.x) / 2.0F, ScreenPosition.y + (Size.y - tFillSize.y) / 2.0F);

                            tRectFillLimitHorizontal = new Rect(tFillPosition.x + tFillSize.x, ScreenPosition.y, tBorderLimitH, Size.y);
                            tRectFillLimitHorizontalTwo = new Rect(tFillPosition.x - tBorderLimitH, ScreenPosition.y, tBorderLimitH, Size.y);

                            tRectFillLimitVertical = new Rect(ScreenPosition.x, tFillPosition.y + tFillSize.y, Size.x, tBorderLimitV);
                            tRectFillLimitVerticalTwo = new Rect(ScreenPosition.x, tFillPosition.y - tBorderLimitV, Size.x, tBorderLimitV);
                        }
                        break;
                    case NWEScreenGaugeFill.Perim:
                        {
                            if (tBorderLimitH > (Size.x - Size.x * tH) / 2.0F)
                            {
                                tBorderLimitH = (int)((Size.x - Size.x * tH) / 2.0F);
                            }
                            if (tBorderLimitV > (Size.y - Size.y * tV) / 2.0F)
                            {
                                tBorderLimitV = (int)((Size.y - Size.y * tV) / 2.0F);
                            }

                            tH = Mathf.Min(1 - HorizontalMin, HorizontalValue);
                            tV = Mathf.Min(1 - VerticalMin, VerticalValue);

                            tFillSize = new Vector2(Size.x * (1.0F - tH), Size.y * (1.0F - tV));
                            tFillPosition = new Vector2(ScreenPosition.x + (Size.x - tFillSize.x) / 2.0F, ScreenPosition.y + (Size.y - tFillSize.y) / 2.0F);

                            tRectFillLimitHorizontal = new Rect(tFillPosition.x + tFillSize.x, ScreenPosition.y, tBorderLimitH, Size.y);
                            tRectFillLimitHorizontalTwo = new Rect(tFillPosition.x - tBorderLimitH, ScreenPosition.y, tBorderLimitH, Size.y);

                            tRectFillLimitVertical = new Rect(ScreenPosition.x, tFillPosition.y + tFillSize.y, Size.x, tBorderLimitV);
                            tRectFillLimitVerticalTwo = new Rect(ScreenPosition.x, tFillPosition.y - tBorderLimitV, Size.x, tBorderLimitV);
                        }
                        break;
                    case NWEScreenGaugeFill.Normal:
                    default:
                        {
                            tFillPosition = new Vector2Int((int)(ScreenPosition.x), (int)(ScreenPosition.y));
                            tFillSize = new Vector2Int((int)(Size.x * tH), (int)(Size.y * tV));
                            tRectFillLimitHorizontal = new Rect(tFillPosition.x + tFillSize.x, tFillPosition.y, tBorderLimitH, Size.y);
                            tRectFillLimitVertical = new Rect(tFillPosition.x, tFillPosition.y + tFillSize.y, Size.x, tBorderLimitV);
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
                // draw border limit
                if (BorderLimit != 0)
                {
                    if (tV < 1)
                    {
                        GUI.DrawTexture(tRectFillLimitVertical, ColorTextureBorderLimit);
                        GUI.DrawTexture(tRectFillLimitVerticalTwo, ColorTextureBorderLimit);
                    }
                    if (tH < 1)
                    {
                        GUI.DrawTexture(tRectFillLimitHorizontal, ColorTextureBorderLimit);
                        GUI.DrawTexture(tRectFillLimitHorizontalTwo, ColorTextureBorderLimit);
                    }
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
                //OverlayText
                if (string.IsNullOrEmpty(OverlayText) == false && ShowText == true)
                {
                    GUI.Label(tRectOverlayText, new GUIContent(OverlayText), OverlayStyle);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //private void OnSceneGUI()
        //{
        //    Debug.Log("NWEScreenGaugeComplex OnSceneGUI()");
        //    //DrawOnScene();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        private void OnDrawGizmosSelected()
        {
            //Debug.Log("NWEScreenGaugeComplex OnDrawGizmosSelected()");
            //DrawOnScene();

            Prepare();
            SceneView tSceneView = SceneView.lastActiveSceneView;
            Rect tScreen= tSceneView.camera.pixelRect;
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
                            tX = (tScreen.width/ 2.0F);
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

                Vector2Int ScreenPosition = new Vector2Int((int)(ScreenPos.x - Size.x / 2.0F + Offset.x), (int)(Screen.height - ScreenPos.y - Size.y / 2.0F + Offset.y));
                Rect tRectBackground = new Rect(ScreenPosition, Size);
                Vector2 tFillPosition;
                Vector2 tFillSize;
                Rect tRectFillLimitVertical = Rect.zero;
                Rect tRectFillLimitHorizontal = Rect.zero;
                Rect tRectFillLimitVerticalTwo = Rect.zero;
                Rect tRectFillLimitHorizontalTwo = Rect.zero;
                //Rect tRectOverlayText = new Rect(ScreenPosition.x + OverlayTextOffset.x - OverlayTextOverset.x,
                                                 //ScreenPosition.y + OverlayTextOffset.y - OverlayTextOverset.y,
                                                 //Size.x + OverlayTextOverset.x * 2, Size.y + OverlayTextOverset.y * 2);

                float tH = Mathf.Max(HorizontalMin, HorizontalValue);
                float tV = Mathf.Max(VerticalMin, VerticalValue);
                int tBorderLimitH = BorderLimit;
                if (tBorderLimitH > (Size.x - Size.x * tH))
                {
                    tBorderLimitH = (int)(Size.x - Size.x * tH) + 1;
                }
                int tBorderLimitV = BorderLimit;
                if (tBorderLimitV > (Size.y - Size.y * tV))
                {
                    tBorderLimitV = (int)(Size.y - Size.y * tV) + 1;
                }
                switch (Reverse)
                {
                    case NWEScreenGaugeFill.Reverse:
                        {
                            tFillPosition = new Vector2Int((int)(ScreenPosition.x + Size.x), (int)(ScreenPosition.y + Size.y));
                            tFillSize = new Vector2Int((int)(-Size.x * tH), (int)(-Size.y * tV));
                            tRectFillLimitHorizontal = new Rect(tFillPosition.x + tFillSize.x, tFillPosition.y, -tBorderLimitH, -Size.y);
                            tRectFillLimitVertical = new Rect(tFillPosition.x, tFillPosition.y + tFillSize.y, -Size.x, -tBorderLimitV);
                        }
                        break;
                    case NWEScreenGaugeFill.Center:
                        {
                            if (tBorderLimitH > (Size.x - Size.x * tH) / 2.0F)
                            {
                                tBorderLimitH = (int)((Size.x - Size.x * tH) / 2.0F);
                            }
                            if (tBorderLimitV > (Size.y - Size.y * tV) / 2.0F)
                            {
                                tBorderLimitV = (int)((Size.y - Size.y * tV) / 2.0F);
                            }

                            tFillSize = new Vector2(Size.x * tH, Size.y * tV);
                            tFillPosition = new Vector2(ScreenPosition.x + (Size.x - tFillSize.x) / 2.0F, ScreenPosition.y + (Size.y - tFillSize.y) / 2.0F);

                            tRectFillLimitHorizontal = new Rect(tFillPosition.x + tFillSize.x, ScreenPosition.y, tBorderLimitH, Size.y);
                            tRectFillLimitHorizontalTwo = new Rect(tFillPosition.x - tBorderLimitH, ScreenPosition.y, tBorderLimitH, Size.y);

                            tRectFillLimitVertical = new Rect(ScreenPosition.x, tFillPosition.y + tFillSize.y, Size.x, tBorderLimitV);
                            tRectFillLimitVerticalTwo = new Rect(ScreenPosition.x, tFillPosition.y - tBorderLimitV, Size.x, tBorderLimitV);
                        }
                        break;
                    case NWEScreenGaugeFill.Perim:
                        {
                            if (tBorderLimitH > (Size.x - Size.x * tH) / 2.0F)
                            {
                                tBorderLimitH = (int)((Size.x - Size.x * tH) / 2.0F);
                            }
                            if (tBorderLimitV > (Size.y - Size.y * tV) / 2.0F)
                            {
                                tBorderLimitV = (int)((Size.y - Size.y * tV) / 2.0F);
                            }

                            tH = Mathf.Min(1 - HorizontalMin, HorizontalValue);
                            tV = Mathf.Min(1 - VerticalMin, VerticalValue);

                            tFillSize = new Vector2(Size.x * (1.0F - tH), Size.y * (1.0F - tV));
                            tFillPosition = new Vector2(ScreenPosition.x + (Size.x - tFillSize.x) / 2.0F, ScreenPosition.y + (Size.y - tFillSize.y) / 2.0F);

                            tRectFillLimitHorizontal = new Rect(tFillPosition.x + tFillSize.x, ScreenPosition.y, tBorderLimitH, Size.y);
                            tRectFillLimitHorizontalTwo = new Rect(tFillPosition.x - tBorderLimitH, ScreenPosition.y, tBorderLimitH, Size.y);

                            tRectFillLimitVertical = new Rect(ScreenPosition.x, tFillPosition.y + tFillSize.y, Size.x, tBorderLimitV);
                            tRectFillLimitVerticalTwo = new Rect(ScreenPosition.x, tFillPosition.y - tBorderLimitV, Size.x, tBorderLimitV);
                        }
                        break;
                    case NWEScreenGaugeFill.Normal:
                    default:
                        {
                            tFillPosition = new Vector2Int((int)(ScreenPosition.x), (int)(ScreenPosition.y));
                            tFillSize = new Vector2Int((int)(Size.x * tH), (int)(Size.y * tV));
                            tRectFillLimitHorizontal = new Rect(tFillPosition.x + tFillSize.x, tFillPosition.y, tBorderLimitH, Size.y);
                            tRectFillLimitVertical = new Rect(tFillPosition.x, tFillPosition.y + tFillSize.y, Size.x, tBorderLimitV);
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
                // draw border limit
                if (BorderLimit != 0)
                {
                    if (tV < 1)
                    {
                        Gizmos.DrawGUITexture(tRectFillLimitVertical, ColorTextureBorderLimit);
                        Gizmos.DrawGUITexture(tRectFillLimitVerticalTwo, ColorTextureBorderLimit);
                    }
                    if (tH < 1)
                    {
                        Gizmos.DrawGUITexture(tRectFillLimitHorizontal, ColorTextureBorderLimit);
                        Gizmos.DrawGUITexture(tRectFillLimitHorizontalTwo, ColorTextureBorderLimit);
                    }
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
                //OverlayText
                if (string.IsNullOrEmpty(OverlayText) == false && ShowText == true)
                {
                    //Handles.Label(tRectOverlayText, new GUIContent(OverlayText), OverlayStyle);
                }
            }
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
