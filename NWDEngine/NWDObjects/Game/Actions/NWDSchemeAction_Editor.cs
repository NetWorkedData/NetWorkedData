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
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDSchemeAction : NWDBasis<NWDSchemeAction>
    {
        //-------------------------------------------------------------------------------------------------------------
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

            tYadd += NWDGUI.Line(EditorGUI.IndentedRect(new Rect(tX, tY, tWidth, 1))).height;
            // draw Flash My App

            foreach (string tProtocol in NWDAppEnvironment.SelectedEnvironment().AppProtocol.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                string tProto = tProtocol.Replace("://", string.Empty);
                EditorGUI.TextField(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "URI Scheme Action", URISchemePath(tProto, string.Empty));
                tYadd += tTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                if (GUI.Button(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "URI Scheme Action", tMiniButtonStyle))
                {
                    Application.OpenURL(URISchemePath(tProto, string.Empty));
                }
                tYadd += tMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;

                // Draw QRCode texture
                Texture2D tTexture = FlashMyApp(tProto,false, 256);
                EditorGUI.DrawPreviewTexture(new Rect(tX, tY + tYadd, NWDGUI.kPrefabSize * 2, NWDGUI.kPrefabSize * 2),
                                             tTexture);
                tYadd += NWDGUI.kPrefabSize * 2 + NWDGUI.kFieldMarge;
            }
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight()
        {
            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);
            // Height calculate for the interface addon for editor
            float tYadd = 0.0F;
            foreach (string tProtocol in NWDAppEnvironment.SelectedEnvironment().AppProtocol.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                tYadd = NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
                tYadd += NWDGUI.kFieldMarge;
                tYadd += NWDGUI.kFieldMarge;
                tYadd += tTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                tYadd += NWDGUI.kPrefabSize * 2 + NWDGUI.kFieldMarge;
            }
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif