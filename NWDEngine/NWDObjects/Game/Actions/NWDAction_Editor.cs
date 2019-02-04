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

            float tWidth = sInRect.width - NWDConstants.kFieldMarge * 2;
            float tX = sInRect.position.x + NWDConstants.kFieldMarge;
            float tY = sInRect.position.y + NWDConstants.kFieldMarge;

            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            float tYadd = 0.0f;

            tYadd += NWDConstants.kFieldMarge;
            // Draw line 
            EditorGUI.DrawRect(new Rect(tX, tY + tYadd, tWidth, 1), NWDConstants.kRowColorLine);
            tYadd += NWDConstants.kFieldMarge;

            // Draw the interface addon for editor
            if (GUI.Button(new Rect(sInRect.x, sInRect.y, sInRect.width, NWDConstants.kMiniButtonStyle.fixedHeight), "Post this Action"))
            {
                PostNotification();
            }
            //tYadd += NWDConstants.kMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            //// draw Flash My App
            //EditorGUI.TextField(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "URI Scheme Action", URISchemePath(""));
            //tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
            //if (GUI.Button(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "URI SchemeAction", tMiniButtonStyle))
            //{
            //    Application.OpenURL(URISchemePath(""));
            //}
            //tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;

            //// Draw QRCode texture
            //Texture2D tTexture = FlashMyApp(false, 256);
            //EditorGUI.DrawPreviewTexture(new Rect(tX, tY + tYadd, NWDConstants.kPrefabSize * 2, NWDConstants.kPrefabSize * 2),
            //                             tTexture);
            //tYadd += NWDConstants.kPrefabSize * 2 + NWDConstants.kFieldMarge;
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
            float tYadd = NWDConstants.kMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            tYadd += NWDConstants.kFieldMarge;
            tYadd += NWDConstants.kFieldMarge;
            tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
            tYadd += NWDConstants.kPrefabSize * 2 + NWDConstants.kFieldMarge;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif