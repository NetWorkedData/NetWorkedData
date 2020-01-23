//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:34:15
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDSchemeAction : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonEditor(Rect sRect)
        {

            float tWidth = sRect.width - NWDGUI.kFieldMarge * 2;
            float tX = sRect.position.x + NWDGUI.kFieldMarge;
            float tY = sRect.position.y + NWDGUI.kFieldMarge;

            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent(NWEConstants.K_A), tWidth);

            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent(NWEConstants.K_A), tWidth);

            float tYadd = 0.0f;

            tYadd += NWDGUI.kFieldMarge;

            tYadd += NWDGUI.Line(EditorGUI.IndentedRect(new Rect(tX, tY, tWidth, 1))).height;

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

                Texture2D tTexture = FlashMyApp(tProto,false, 256);
                EditorGUI.DrawPreviewTexture(new Rect(tX, tY + tYadd, NWDGUI.kPrefabSize * 2, NWDGUI.kPrefabSize * 2),
                                             tTexture);
                tYadd += NWDGUI.kPrefabSize * 2 + NWDGUI.kFieldMarge;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight(float sWidth)
        {
            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent(NWEConstants.K_A), 100);
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