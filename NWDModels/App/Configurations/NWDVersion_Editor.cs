//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:48
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
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
using ZXing;
using ZXing.QrCode;
using UnityEngine.Networking;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDVersion : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        Texture2D QRCodeTexture = null;
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
            //string tVersionString = "0.00.00";
            string tVersionString = PlayerSettings.bundleVersion;
            int tVersionInt = 0;
            //int.TryParse(tVersionString.Replace(".", string.Empty), out tVersionInt);
            NWDVersion tMaxVersionObject = null;
            foreach (NWDVersion tVersionObject in NWDBasisHelper.BasisHelper<NWDVersion>().Datas)
            {
                if (tVersionObject.IntegrityIsValid() == true && tVersionObject.AC == true && tVersionObject.Buildable == true)
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
        public override void AddonEditor(Rect sRect)
        {
            // force update 
            //NWDVersion.UpdateVersionBundle();
            // show editor add-on
            //float tWidth = sInRect.width - NWDGUI.kFieldMarge * 2;
            //float tX = sInRect.position.x + NWDGUI.kFieldMarge;
            //float tY = sInRect.position.y + NWDGUI.kFieldMarge;
            // darw information about actual bundle 
            Rect sInRect = NWDGUI.MargeLeftRight(sRect);
            // Draw line 
            sInRect.y += NWDGUI.Separator(sInRect).height;

            EditorGUI.BeginDisabledGroup(true);

            sInRect.height = NWDGUI.kBoldLabelStyle.fixedHeight;
            GUI.Label(sInRect, "Environement selected to build", NWDGUI.kBoldLabelStyle);
            sInRect.y += sInRect.height + NWDGUI.kFieldMarge;

            sInRect.height = NWDGUI.kLabelStyle.fixedHeight;
            EditorGUI.LabelField(sInRect, "Environment", NWDAppConfiguration.SharedInstance().SelectedEnvironment().Environment, NWDGUI.kLabelStyle);
            sInRect.y += sInRect.height + NWDGUI.kFieldMarge;

            EditorGUI.LabelField(sInRect, "Version", PlayerSettings.bundleVersion, NWDGUI.kLabelStyle);
            sInRect.y += sInRect.height + NWDGUI.kFieldMarge;

            EditorGUI.EndDisabledGroup();

            // Draw line 
            sInRect.y += NWDGUI.Separator(sInRect).height;

            sInRect.height = NWDGUI.kMiniButtonStyle.fixedHeight;
            if (GUI.Button(sInRect, "Recommendation by SMS", NWDGUI.kMiniButtonStyle))
            {
                RecommendationBy(NWDRecommendationType.SMS);
            }
            sInRect.y += sInRect.height + NWDGUI.kFieldMarge;
            if (GUI.Button(sInRect, "Recommendation by Email", NWDGUI.kMiniButtonStyle))
            {
                RecommendationBy(NWDRecommendationType.Email);
            }
            sInRect.y += sInRect.height + NWDGUI.kFieldMarge;
            if (GUI.Button(sInRect, "Recommendation by Email HTML", NWDGUI.kMiniButtonStyle))
            {
                RecommendationBy(NWDRecommendationType.EmailHTML);
            }
            sInRect.y += sInRect.height + NWDGUI.kFieldMarge;

            // Draw line 
            sInRect.y += NWDGUI.Separator(sInRect).height;

            // Draw QRCode texture
            if (QRCodeTexture == null)
            {
                QRCodeTexture = FlashMyApp(false, 256);
            }
            EditorGUI.DrawPreviewTexture(new Rect(sInRect.x, sInRect.y, NWDGUI.kPrefabSize * 2, NWDGUI.kPrefabSize * 2), QRCodeTexture);
            sInRect.y += NWDGUI.kPrefabSize * 2 + NWDGUI.kFieldMarge;

            // Draw line 
            sInRect.y += NWDGUI.Separator(sInRect).height;


            sInRect.height = NWDGUI.kLabelStyle.fixedHeight;
            EditorGUI.TextField(sInRect, "URL My App", URLMyApp(false), NWDGUI.kLabelStyle);
            sInRect.y += sInRect.height + NWDGUI.kFieldMarge;
            sInRect.height = NWDGUI.kMiniButtonStyle.fixedHeight;
            if (GUI.Button(sInRect, "URL My App without redirection", NWDGUI.kMiniButtonStyle))
            {
                Application.OpenURL(URLMyApp(false));
            }
            sInRect.y += sInRect.height + NWDGUI.kFieldMarge;

            sInRect.height = NWDGUI.kLabelStyle.fixedHeight;
            EditorGUI.TextField(sInRect, "URL My App", URLMyApp(true), NWDGUI.kLabelStyle);
            sInRect.y += sInRect.height + NWDGUI.kFieldMarge;
            sInRect.height = NWDGUI.kMiniButtonStyle.fixedHeight;
            if (GUI.Button(sInRect, "URL My App with redirection", NWDGUI.kMiniButtonStyle))
            {
                Application.OpenURL(URLMyApp(true));
            }
            sInRect.y += sInRect.height + NWDGUI.kFieldMarge;
            // Draw line 
            sInRect.y += NWDGUI.Separator(NWDGUI.MargeLeftRight(sInRect)).height;
            // Draw button choose env
            if (GUI.Button(sInRect, "Environment chooser", NWDGUI.kMiniButtonStyle))
            {
                NWDEditorMenu.EnvironementChooserShow();
            }
            sInRect.y += sInRect.height + NWDGUI.kFieldMarge;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight(float sWidth)
        {
            // Height calculate for the interface addon for editor
            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent(NWEConstants.K_A), 100);
            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent(NWEConstants.K_A), 100);

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
        private static string DefaultVersionReference()
        {
            return NWDBasisHelper.BasisHelper<NWDVersion>().ClassTrigramme + "-00000000-000";
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Prevent Default Version
        /// </summary>
        private static void PreventDefaultVersion(NWDVersion sVersion)
        {
            //NWEBenchmark.Start();
            // check default value for default version
            NWDVersion tDefaultVersion = NWDBasisHelper.GetEditorDataByReference<NWDVersion>(DefaultVersionReference());
            if (tDefaultVersion == sVersion)
            {
                Debug.LogWarning("Default version lock some properties");
                // force version default to default value
                sVersion.ResetToDefaultVersionValue();
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Check if default version exists or create one.
        /// </summary>
        public static void CheckDefaultVersion()
        {
            //NWEBenchmark.Start();
            GetDefaultVersion();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return the default version. Create one if necessary.
        /// </summary>
        /// <returns></returns>
        private static NWDVersion GetDefaultVersion()
        {
            //NWEBenchmark.Start();
            // Add version by default, the version 0.00.00 of Application
            string tReference = DefaultVersionReference();
            NWDVersion tVersionDefault = NWDBasisHelper.GetEditorDataByReference<NWDVersion>(tReference);
            if (tVersionDefault == null)
            {
                tVersionDefault = NWDBasisHelper.NewDataWithReference<NWDVersion>(tReference);
                tVersionDefault.Version = new NWDVersionType();
                tVersionDefault.ResetToDefaultVersionValue();
                tVersionDefault.SaveDataIfModified();
            }
            //NWEBenchmark.Finish();
            return tVersionDefault;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Reset to default version value
        /// </summary>
        private void ResetToDefaultVersionValue()
        {
            //NWEBenchmark.Start();
            AC = true;
            XX = 0;
            DD = 0;
            Version.Default();
            if (DevSync < 0)
            {
                DevSync = 0;
            }
            if (PreprodSync < 0)
            {
                PreprodSync = 0;
            }
            if (ProdSync < 0)
            {
                ProdSync = 0;
            }
            ActiveDev = true;
            ActivePreprod = true;
            ActiveProd = true;
            Editable = true;
            Buildable = true;
            InternalDescription = "Default version";
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif