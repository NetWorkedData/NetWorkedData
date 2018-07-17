//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class AirDraw
    {
        //-------------------------------------------------------------------------------------------------------------
        static Texture2D kTexture;
        static Material kMaterialUI;
        static string kShaderNameUI = "UI/Default";
        static Texture2D aaLineTex = null;
        private static Material lineMaterial = null;
        //-------------------------------------------------------------------------------------------------------------
        static void InitializeDraw()
        {
            //kShaderNameUI = "UI/Unlit/Detail";
            if (kMaterialUI == null)
            {
                kMaterialUI = new Material(Shader.Find(kShaderNameUI));
            }
            if (kTexture == null)
            {
                kTexture = new Texture2D(1, 1);
            }
            if (aaLineTex == null)
            {
                // TODO: better anti-aliasing of wide lines with a larger texture? or use Graphics.DrawTexture with border settings
                aaLineTex = new Texture2D(1, 3, TextureFormat.ARGB32, false);
                aaLineTex.SetPixel(0, 0, new Color(1, 1, 1, 0));
                aaLineTex.SetPixel(0, 1, Color.white);
                aaLineTex.SetPixel(0, 2, new Color(1, 1, 1, 0));
                aaLineTex.Apply();
            }
            //    lineMaterial = new Material("Shader \"Lines/Colored Blended\" {" +
            //"SubShader { Pass {" +
            //"   BindChannels { Bind \"Color\",color }" +
            //"   Blend SrcAlpha OneMinusSrcAlpha" +
            //"   ZWrite Off Cull Off Fog { Mode Off }" +
            //"} } }");
            //lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            //lineMaterial.shader.hideFlags = HideFlags.HideAndDontSave;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void DrawQuad(Vector2 sA, Vector2 sB, Vector2 sC, Vector2 sD, Color sColor)
        {
            if (Event.current.type.Equals(EventType.Repaint))
            {
#if UNITY_EDITOR
                InitializeDraw();
#endif
                GL.PushMatrix();
                kMaterialUI.SetPass(0);
                GL.LoadPixelMatrix();
                GL.Begin(GL.QUADS);
                GL.Color(sColor);
                GL.Vertex3(sA.x, sA.y, 0);
                GL.Vertex3(sB.x, sB.y, 0);
                GL.Vertex3(sC.x, sC.y, 0);
                GL.Vertex3(sD.x, sD.y, 0);
                GL.End();
                GL.PopMatrix();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void DrawRect(Rect sRect, Color sColor)
        {
            if (Event.current.type.Equals(EventType.Repaint))
            {
#if UNITY_EDITOR
                InitializeDraw();
#endif
                GL.PushMatrix();
                kMaterialUI.SetPass(0);
                GL.LoadPixelMatrix();
                // QUADS Method
                GL.Begin(GL.QUADS);
                GL.Color(sColor);
                /*A*/
                GL.Vertex3(sRect.x, sRect.y, 0);
                /*B*/
                GL.Vertex3(sRect.x, sRect.y + sRect.height, 0);
                /*C*/
                GL.Vertex3(sRect.x + sRect.width, sRect.y + sRect.height, 0);
                /*D*/
                GL.Vertex3(sRect.x + sRect.width, sRect.y, 0);
                GL.End();
                GL.PopMatrix();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void DrawLine(Vector2 sA, Vector2 sB, Color sColor, float sWidth = 1.0F, bool sAntiAlias = true)
        {
            if (Event.current.type.Equals(EventType.Repaint))
            {
#if UNITY_EDITOR
                InitializeDraw();
#endif
                GL.PushMatrix();
                //if (sAntiAlias == true)
                //{
                //    blendMaterial.SetPass(0);
                //}
                //else
                //{
                kMaterialUI.SetPass(0);
                //lineMaterial.SetPass(0);
                //}
                //GL.LoadPixelMatrix();
                //GL.LoadOrtho();
                //float dx = sB.x - sA.x;
                //float dy = sB.y - sA.y;
                //float len = Mathf.Sqrt(dx * dx + dy * dy);
                //float wdx = sWidth * dy / len;
                //float wdy = sWidth * dx / len;
                //Matrix4x4 matrix = Matrix4x4.identity;
                //matrix.m00 = dx;
                //matrix.m01 = -wdx;
                //matrix.m03 = sA.x + 0.5f * wdx;
                //matrix.m10 = dy;
                //matrix.m11 = wdy;
                //matrix.m13 = sA.y - 0.5f * wdy;
                //GL.ma(matrix);

                GL.Begin(GL.LINES);
                GL.Color(sColor);
                GL.Vertex(sA);
                GL.Vertex(sB);
                GL.End();
                GL.PopMatrix();
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        public static void DrawPeri(Vector2[] sPointArray, Color sColor, float sWidth, bool sAntiAlias)
        {
            if (Event.current.type.Equals(EventType.Repaint))
            {
                if (sPointArray.Length > 1)
                {
#if UNITY_EDITOR
                    InitializeDraw();
#endif
                    kMaterialUI.SetPass(0);
                    //GL.LoadPixelMatrix();
                    //GL.LoadOrtho();
                    GL.Begin(GL.LINES);
                    GL.Color(sColor);

                    for (int ti = 1; ti < sPointArray.Length; ti++)
                    {
                        GL.Vertex(sPointArray[ti - 1]);
                        GL.Vertex(sPointArray[ti]);
                    }
                    GL.Vertex(sPointArray[sPointArray.Length - 1]);
                    GL.Vertex(sPointArray[0]);
                    GL.End();
                    //GL.PopMatrix();
                }
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        public static void DrawPeriArea(Vector2[] sPointArrayA, Vector2[] sPointArrayB, Color sColor, bool sAntiAlias = true)
        {
            if (Event.current.type.Equals(EventType.Repaint))
            {
                if (sPointArrayA.Length > 1)
                {
#if UNITY_EDITOR
                    InitializeDraw();
#endif
                    kMaterialUI.SetPass(0);
                    //GL.LoadPixelMatrix();
                    //GL.LoadOrtho();
                    GL.Begin(GL.TRIANGLES);
                    GL.Color(sColor);
                    for (int ti = 1; ti < sPointArrayA.Length; ti++)
                    {
                        GL.Vertex(sPointArrayA[ti - 1]);
                        GL.Vertex(sPointArrayA[ti]);
                        GL.Vertex(sPointArrayB[ti]);

                        GL.Vertex(sPointArrayB[ti - 1]);
                        GL.Vertex(sPointArrayB[ti]);
                        GL.Vertex(sPointArrayA[ti - 1]);
                    }

                    GL.Vertex(sPointArrayA[sPointArrayA.Length - 1]);
                    GL.Vertex(sPointArrayA[0]);
                    GL.Vertex(sPointArrayB[0]);

                    GL.Vertex(sPointArrayB[sPointArrayA.Length - 1]);
                    GL.Vertex(sPointArrayB[0]);
                    GL.Vertex(sPointArrayA[sPointArrayA.Length - 1]);

                    GL.End();
                    //GL.PopMatrix();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void DrawTriangles(Vector2[] sPoints, Color sColor)
        {
            if (Event.current.type.Equals(EventType.Repaint))
            {
#if UNITY_EDITOR
                InitializeDraw();
#endif
                GL.PushMatrix();
                kMaterialUI.SetPass(0);
                GL.LoadPixelMatrix();
                GL.Begin(GL.TRIANGLES);
                GL.Color(sColor);
                foreach (Vector2 tV in sPoints)
                {
                    GL.Vertex3(tV.x, tV.y, 0);
                }
                GL.End();
                GL.PopMatrix();
            }
        }

    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
