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
    public enum NWEScreenGaugeFill : int
    {
        Normal = 1,
        Reverse = 2,
        Center = 3,
        Perim = 4,
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWEScreenAlignHorizontal : int
    {
        Middle = 2,
        Left = -1,
        Right = 1,
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWEScreenAlignVertical : int
    {
        Center = 2,
        Top = -1,
        Bottom = 1,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWEScreenGauge : MonoBehaviour
    {

        [Header("Gauge Visibility")]
        public bool IsVisible = true;
        [Header("Color gauge")]
        [Tooltip("Use to color the background")]
        public Color ColorBackground = Color.white;
        [Tooltip("Use to color the fill effect")]
        public Color ColorFill = Color.red;

        [Header("Size in ScreenPoint")]
        [Tooltip("Size the gauge")]
        public Vector2Int Size = new Vector2Int(200, 20);
        public NWEScreenAlignHorizontal AlignHorizontal = NWEScreenAlignHorizontal.Middle;
        public NWEScreenAlignVertical AlignVertical = NWEScreenAlignVertical.Center;
        public Vector2Int Offset = new Vector2Int(0, 0);
        //public Vector2 PurcentOffset = new Vector2Int(0.0F, 0.0F);
        [Header("Fill parameters")]
        [Tooltip("The gauge fill reverse ?")]
        public NWEScreenGaugeFill Reverse = NWEScreenGaugeFill.Normal;

        [Header("Fill values")]
        [Tooltip("The gauge value in horizontal")]
        [Range(0.0F, 1.0F)]
        public float HorizontalValue = 1.0f;
        [Tooltip("The gauge value in vertical")]
        [Range(0.0F, 1.0F)]
        public float VerticalValue = 1.0f;
        //-------------------------------------------------------------------------------------------------------------
        protected bool PrepareIsOk = false;

        protected Texture2D ColorTextureBackground;
        protected Texture2D ColorTextureFill;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Set active or not .
        /// </summary>
        /// <param name="sValue">If set to <c>true</c> s value.</param>
        public void SetHidden(bool sValue)
        {
            gameObject.SetActive(!sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Show the gauge.
        /// </summary>
        /// <returns>The show.</returns>
        /// <param name="sVisible">If set to <c>true</c> s visible.</param>
        public void Show(bool sVisible)
        {
            IsVisible = sVisible;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Set the horizontal value of fill.
        /// </summary>
        /// <param name="sValue">S value.</param>
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
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Set the vertical value of fill.
        /// </summary>
        /// <param name="sValue">S value.</param>
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
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Draws the rect border.
        /// </summary>
        /// <param name="sRect">S rect.</param>
        /// <param name="sTexture">S texture.</param>
        /// <param name="sBorder">S border.</param>
        public void DrawRectBorder(Rect sRect, Texture2D sTexture, float sBorder)
        {
            GUI.DrawTexture(new Rect(sRect.x, sRect.y, sRect.width, sBorder), sTexture);
            GUI.DrawTexture(new Rect(sRect.x, sRect.y + sRect.height - sBorder, sRect.width, sBorder), sTexture);
            GUI.DrawTexture(new Rect(sRect.x, sRect.y + sBorder, sBorder, sRect.height - sBorder * 2), sTexture);
            GUI.DrawTexture(new Rect(sRect.x + sRect.width - sBorder, sRect.y + sBorder, sBorder, sRect.height - sBorder * 2), sTexture);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GizmosDrawRectBorder(Rect sRect, Texture2D sTexture, float sBorder)
        {
            Gizmos.DrawGUITexture(new Rect(sRect.x, sRect.y, sRect.width, sBorder), sTexture);
            Gizmos.DrawGUITexture(new Rect(sRect.x, sRect.y + sRect.height - sBorder, sRect.width, sBorder), sTexture);
            Gizmos.DrawGUITexture(new Rect(sRect.x, sRect.y + sBorder, sBorder, sRect.height - sBorder * 2), sTexture);
            Gizmos.DrawGUITexture(new Rect(sRect.x + sRect.width - sBorder, sRect.y + sBorder, sBorder, sRect.height - sBorder * 2), sTexture);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Prepare this instance.
        /// </summary>
        public virtual void Prepare()
        {
            // Prepare colortexture to draw color

            //ColorTextureBackground = null;
            if (ColorTextureBackground == null)
            {
                ColorTextureBackground = new Texture2D(1, 1);
            }
            ColorTextureBackground.SetPixel(0, 0, ColorBackground);
            ColorTextureBackground.Apply();

            //ColorTextureFill = null;
            if (ColorTextureFill == null)
            {
                ColorTextureFill = new Texture2D(1, 1);
            }
            ColorTextureFill.SetPixel(0, 0, ColorFill);
            ColorTextureFill.Apply();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Draw this instance.
        /// </summary>
        public virtual void Draw()
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
                switch (Reverse)
                {
                    case NWEScreenGaugeFill.Reverse:
                        {
                            tFillPosition = new Vector2Int((int)(ScreenPosition.x + Size.x), (int)(ScreenPosition.y + Size.y));
                            tFillSize = new Vector2Int((int)(-Size.x * HorizontalValue), (int)(-Size.y * VerticalValue));
                        }
                        break;
                    case NWEScreenGaugeFill.Center:
                        {
                            tFillSize = new Vector2(Size.x * HorizontalValue, Size.y * VerticalValue);
                            tFillPosition = new Vector2(ScreenPosition.x + (Size.x - tFillSize.x) / 2.0F, ScreenPosition.y + (Size.y - tFillSize.y) / 2.0F);
                        }
                        break;
                    case NWEScreenGaugeFill.Perim:
                        {
                            tFillSize = new Vector2(Size.x * (1.0F - HorizontalValue), Size.y * (1.0F - VerticalValue));
                            tFillPosition = new Vector2(ScreenPosition.x + (Size.x - tFillSize.x) / 2.0F, ScreenPosition.y + (Size.y - tFillSize.y) / 2.0F);
                        }
                        break;
                    case NWEScreenGaugeFill.Normal:
                    default:
                        {
                            tFillPosition = new Vector2Int((int)(ScreenPosition.x), (int)(ScreenPosition.y));
                            tFillSize = new Vector2Int((int)(Size.x * HorizontalValue), (int)(Size.y * VerticalValue));
                        }
                        break;
                }
                Rect tRectFill = new Rect(tFillPosition, tFillSize);
                // draw Background
                GUI.DrawTexture(tRectBackground, ColorTextureBackground);
                // draw Fill
                GUI.DrawTexture(tRectFill, ColorTextureFill);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// On GUI render.
        /// </summary>
        private void OnGUI()
        {
            //Debug.Log("NWEScreenGauge OnGUI()");
            Draw();
        }

#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        private void OnSceneGUI()
        {
            //Debug.Log("NWEScreenGauge OnSceneGUI()");
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnDrawGizmosSelected()
        {
            //Debug.Log("NWEScreenGauge OnDrawGizmosSelected()");
            Prepare();

            if (IsVisible == true)
            {
                //Handles.BeginGUI();
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

                Vector2Int ScreenPosition = new Vector2Int((int)(ScreenPos.x - Size.x / 2.0F), (int)(Screen.height - ScreenPos.y - Size.y / 2.0F));
                Rect tRectBackground = new Rect(ScreenPosition, Size);
                Vector2 tFillPosition;
                Vector2 tFillSize;
                switch (Reverse)
                {
                    case NWEScreenGaugeFill.Reverse:
                        {
                            tFillPosition = new Vector2Int((int)(ScreenPosition.x + Size.x), (int)(ScreenPosition.y + Size.y));
                            tFillSize = new Vector2Int((int)(-Size.x * HorizontalValue), (int)(-Size.y * VerticalValue));
                        }
                        break;
                    case NWEScreenGaugeFill.Center:
                        {
                            tFillSize = new Vector2(Size.x * HorizontalValue, Size.y * VerticalValue);
                            tFillPosition = new Vector2(ScreenPosition.x + (Size.x - tFillSize.x) / 2.0F, ScreenPosition.y + (Size.y - tFillSize.y) / 2.0F);
                        }
                        break;
                    case NWEScreenGaugeFill.Perim:
                        {
                            tFillSize = new Vector2(Size.x * (1.0F - HorizontalValue), Size.y * (1.0F - VerticalValue));
                            tFillPosition = new Vector2(ScreenPosition.x + (Size.x - tFillSize.x) / 2.0F, ScreenPosition.y + (Size.y - tFillSize.y) / 2.0F);
                        }
                        break;
                    case NWEScreenGaugeFill.Normal:
                    default:
                        {
                            tFillPosition = new Vector2Int((int)(ScreenPosition.x), (int)(ScreenPosition.y));
                            tFillSize = new Vector2Int((int)(Size.x * HorizontalValue), (int)(Size.y * VerticalValue));
                        }
                        break;
                }
                Rect tRectFill = new Rect(tFillPosition, tFillSize);
                // draw Background
                Gizmos.DrawGUITexture(tRectBackground, ColorTextureBackground);
                // draw Fill
                Gizmos.DrawGUITexture(tRectFill, ColorTextureFill);
                //Handles.EndGUI();
            }
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
