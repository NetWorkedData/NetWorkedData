//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

using SQLite4Unity3d;

using BasicToolBox;

using ZXing;
using ZXing.QrCode;
using UnityEngine.Networking;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDVersion : NWDBasis<NWDVersion>
    {
        //-------------------------------------------------------------------------------------------------------------
        public static void UpdateVersionBundle()
        {
            //Debug.Log("NWDVersion UpdateVersionBundle()");
            if (NWDAppConfiguration.SharedInstance().IsDevEnvironement() == false &&
                NWDAppConfiguration.SharedInstance().IsPreprodEnvironement() == false &&
                NWDAppConfiguration.SharedInstance().IsProdEnvironement() == false
            )
            {
                // error no environnment selected 
                NWDAppConfiguration.SharedInstance().DevEnvironment.Selected = true;
            }
            // I will change the last version of my App
            string tVersionString = "0.00.00";
            int tVersionInt = 0;
            int.TryParse(tVersionString.Replace(".", string.Empty), out tVersionInt);
            NWDVersion tMaxVersionObject = null;
            foreach (NWDVersion tVersionObject in NWDVersion.BasisHelper().Datas)
            {
                if (tVersionObject.TestIntegrity() == true && tVersionObject.AC == true && tVersionObject.Buildable == true)
                {
                    if ((NWDAppConfiguration.SharedInstance().IsDevEnvironement() && tVersionObject.ActiveDev == true) ||
                        (NWDAppConfiguration.SharedInstance().IsPreprodEnvironement() && tVersionObject.ActivePreprod == true) ||
                        (NWDAppConfiguration.SharedInstance().IsProdEnvironement() && tVersionObject.ActiveProd == true))
                    {
                        int tVersionInteger = 0;
                        int.TryParse(tVersionObject.Version.ToString().Replace(".", string.Empty), out tVersionInteger);
                        if (tVersionInt < tVersionInteger)
                        {
                            tVersionInt = tVersionInteger;
                            tVersionString = tVersionObject.Version.ToString();
                            tMaxVersionObject = tVersionObject;
                        }
                    }
                }
            }
            if (tMaxVersionObject != null)
            {
                if (PlayerSettings.bundleVersion != tMaxVersionObject.Version.ToString())
                {
                    PlayerSettings.bundleVersion = tMaxVersionObject.Version.ToString();
                }
            }
            else
            {
                if (PlayerSettings.bundleVersion != tVersionString)
                {
                    PlayerSettings.bundleVersion = tVersionString;
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool AddonEdited(bool sNeedBeUpdate)
        {
            if (sNeedBeUpdate == true)
            {
                this.InternalKey = this.Version.ToString();
            }
            return sNeedBeUpdate;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditor(Rect sInRect)
        {
            // force update 
            NWDVersion.UpdateVersionBundle();
            // show editor add-on
            float tWidth = sInRect.width - NWDGUI.kFieldMarge * 2;
            float tX = sInRect.position.x + NWDGUI.kFieldMarge;
            float tY = sInRect.position.y + NWDGUI.kFieldMarge;

            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidth);

            float tYadd = 0.0f;
            // darw information about actual bundle 
            EditorGUI.BeginDisabledGroup(true);


            tYadd += NWDGUI.Separator(EditorGUI.IndentedRect(new Rect(tX, tY, tWidth, 1))).height;

            GUI.Label(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "Environement selected to build", EditorStyles.boldLabel);
            tYadd += tTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;

            EditorGUI.LabelField(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "Environment", NWDAppConfiguration.SharedInstance().SelectedEnvironment().Environment);
            tYadd += tTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;

            EditorGUI.LabelField(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "Version", PlayerSettings.bundleVersion);
            tYadd += tTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;

            EditorGUI.EndDisabledGroup();


            // draw Flash My App
            EditorGUI.TextField(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "URL My App", URLMyApp(false));
            tYadd += tTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
            if (GUI.Button(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "URL My App without redirection", tMiniButtonStyle))
            {
                Application.OpenURL(URLMyApp(false));
            }
            tYadd += tMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;

            EditorGUI.TextField(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "URL My App", URLMyApp(true));
            tYadd += tTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
            if (GUI.Button(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "URL My App with redirection", tMiniButtonStyle))
            {
                Application.OpenURL(URLMyApp(true));
            }
            tYadd += tMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;

            // Draw QRCode texture
            Texture2D tTexture = FlashMyApp(false, 256);
            EditorGUI.DrawPreviewTexture(new Rect(tX, tY + tYadd, NWDGUI.kPrefabSize * 2, NWDGUI.kPrefabSize * 2),
                                         tTexture);
            tYadd += NWDGUI.kPrefabSize * 2 + NWDGUI.kFieldMarge;

            // Draw line 
            tYadd += NWDGUI.Separator(EditorGUI.IndentedRect(new Rect(tX, tY, tWidth, 1))).height;

            // Draw button choose env
            if (GUI.Button(new Rect(tX, tY + tYadd, tWidth, tMiniButtonStyle.fixedHeight), "Environment chooser", tMiniButtonStyle))
            {
                NWDEditorMenu.EnvironementChooserShow();
            }
            tYadd += tMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
            tYadd += NWDGUI.kFieldMarge;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight()
        {
            // Height calculate for the interface addon for editor
            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);
            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100);

            float tYadd = 0.0f;

            tYadd += NWDGUI.kFieldMarge;

            tYadd += tTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;

            tYadd += tTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;

            tYadd += tTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;

            tYadd += tTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
            tYadd += tTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;

            tYadd += NWDGUI.kFieldMarge;

            tYadd += tMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
            tYadd += tMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
            tYadd += tMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;

            tYadd += NWDGUI.kPrefabSize * 2 + NWDGUI.kFieldMarge;

            tYadd += NWDGUI.kFieldMarge;

            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif