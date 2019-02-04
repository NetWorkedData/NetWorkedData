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
            // draw Flash My App

            foreach (string tProtocol in NWDAppEnvironment.SelectedEnvironment().AppProtocol.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries))
            {
                string tProto = tProtocol.Replace("://", string.Empty);
                EditorGUI.TextField(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "URI Scheme Action", URISchemePath(tProto, string.Empty));
                tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                if (GUI.Button(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "URI Scheme Action", tMiniButtonStyle))
                {
                    Application.OpenURL(URISchemePath(tProto, string.Empty));
                }
                tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;

                // Draw QRCode texture
                Texture2D tTexture = FlashMyApp(tProto,false, 256);
                EditorGUI.DrawPreviewTexture(new Rect(tX, tY + tYadd, NWDConstants.kPrefabSize * 2, NWDConstants.kPrefabSize * 2),
                                             tTexture);
                tYadd += NWDConstants.kPrefabSize * 2 + NWDConstants.kFieldMarge;
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
                tYadd = NWDConstants.kMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
                tYadd += NWDConstants.kFieldMarge;
                tYadd += NWDConstants.kFieldMarge;
                tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
                tYadd += NWDConstants.kPrefabSize * 2 + NWDConstants.kFieldMarge;
            }
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif