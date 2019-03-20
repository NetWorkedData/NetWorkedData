//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using UnityEngine;
using BasicToolBox;
using ZXing;
using ZXing.QrCode;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAction : NWDBasis<NWDAction>
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons editor interface.
        /// </summary>
        /// <returns>The editor height addon.</returns>
        /// <param name="sInRect">S in rect.</param>
        public override float AddonEditor(Rect sInRect)
        {

            float tWidth = sInRect.width - NWDGUI.kFieldMarge * 2;
            float tX = sInRect.position.x + NWDGUI.kFieldMarge;
            float tY = sInRect.position.y + NWDGUI.kFieldMarge;

            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            float tYadd = 0.0f;

            tYadd += NWDGUI.kFieldMarge;
            // Draw line 
            tYadd += NWDGUI.Separator(EditorGUI.IndentedRect(new Rect(tX, tY, tWidth, 1))).height;

            // Draw the interface addon for editor
            if (GUI.Button(new Rect(sInRect.x, sInRect.y, sInRect.width, NWDGUI.kMiniButtonStyle.fixedHeight), "Post this Action"))
            {
                PostNotification();
            }
            //tYadd += NWDGUI.tMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
            //// draw Flash My App
            //EditorGUI.TextField(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "URI Scheme Action", URISchemePath(""));
            //tYadd += tTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
            //if (GUI.Button(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "URI SchemeAction", tMiniButtonStyle))
            //{
            //    Application.OpenURL(URISchemePath(""));
            //}
            //tYadd += tMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;

            //// Draw QRCode texture
            //Texture2D tTexture = FlashMyApp(false, 256);
            //EditorGUI.DrawPreviewTexture(new Rect(tX, tY + tYadd, NWDGUI.kPrefabSize * 2, NWDGUI.kPrefabSize * 2),
            //                             tTexture);
            //tYadd += NWDGUI.kPrefabSize * 2 + NWDGUI.kFieldMarge;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons editor intreface expected height.
        /// </summary>
        /// <returns>The editor expected height.</returns>
        public override float AddonEditorHeight()
        {
            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);
            // Height calculate for the interface addon for editor
            float tYadd = NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
            tYadd += NWDGUI.kFieldMarge;
            tYadd += NWDGUI.kFieldMarge;
            tYadd += tTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
            tYadd += NWDGUI.kPrefabSize * 2 + NWDGUI.kFieldMarge;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif