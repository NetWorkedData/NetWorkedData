//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEngine;
//=====================================================================================================================
using UnityEditor;
using NetWorkedData.NWDEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDVersion : NWDBasis
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
            //string tVersionString = "0.00.00";
            string tVersionString = PlayerSettings.bundleVersion;
            int tVersionInt = -1;
            //int.TryParse(tVersionString.Replace(".", string.Empty), out tVersionInt);
            //NWDVersion tMaxVersionObject = null;
            foreach (NWDVersion tVersionObject in NWDBasisHelper.BasisHelper<NWDVersion>().Datas)
            {
                //Debug.Log("Do this item be used ? : " + tVersionObject.InternalKey);
                if (tVersionObject.IntegrityIsValid() == true && tVersionObject.AC == true && tVersionObject.Buildable == true)
                {
                    if ((NWDAppConfiguration.SharedInstance().IsDevEnvironement() && tVersionObject.ActiveDev == true) ||
                        (NWDAppConfiguration.SharedInstance().IsPreprodEnvironement() && tVersionObject.ActivePreprod == true) ||
                        (NWDAppConfiguration.SharedInstance().IsProdEnvironement() && tVersionObject.ActiveProd == true))
                    {

                        //Debug.Log("YES USE : " + tVersionObject.InternalKey);
                        int tVersionInteger = 0;
                        int.TryParse(tVersionObject.Version.ToString().Replace(".", string.Empty), out tVersionInteger);
                        if (tVersionInt < tVersionInteger)
                        {
                            tVersionInt = tVersionInteger;
                            tVersionString = tVersionObject.Version.ToString();
                            //tMaxVersionObject = tVersionObject;
                        }
                    }
                }
            }
            //if (tMaxVersionObject != null)
            //{
            //    if (PlayerSettings.bundleVersion != tMaxVersionObject.Version.ToString())
            //    {
            //        PlayerSettings.bundleVersion = tMaxVersionObject.Version.ToString();
            //    }
            //}
            //else
            {
                if (PlayerSettings.bundleVersion != tVersionString)
                {
                    PlayerSettings.bundleVersion = tVersionString;
                    NWDAppEnvironmentChooser.Refresh();
                    NWDAppEnvironmentSync.Refresh();
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
            //if (QRCodeTexture == null)
            //{
            //    QRCodeTexture = FlashMyApp(false, 256);
            //}
            //EditorGUI.DrawPreviewTexture(new Rect(sInRect.x, sInRect.y, NWDGUI.kPrefabSize * 2, NWDGUI.kPrefabSize * 2), QRCodeTexture);
            //sInRect.y += NWDGUI.kPrefabSize * 2 + NWDGUI.kFieldMarge;

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
            NWDBenchmark.Start();
            // check default value for default version
            NWDVersion tDefaultVersion = NWDBasisHelper.GetEditorDataByReference<NWDVersion>(DefaultVersionReference());
            if (tDefaultVersion == sVersion)
            {
                Debug.LogWarning("Default version lock some properties");
                // force version default to default value
                sVersion.ResetToDefaultVersionValue();
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Check if default version exists or create one.
        /// </summary>
        public static void CheckDefaultVersion()
        {
            //NWDBenchmark.Start();
            GetDefaultVersion();
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return the default version. Create one if necessary.
        /// </summary>
        /// <returns></returns>
        private static NWDVersion GetDefaultVersion()
        {
            //NWDBenchmark.Start();
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
            else
            {
                if (tVersionDefault.IntegrityIsValid() == false)
                {
                    tVersionDefault.ResetToDefaultVersionValue();
                    tVersionDefault.SaveDataIfModified();
                }
            }
            //NWDBenchmark.Finish();
            return tVersionDefault;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Reset to default version value
        /// </summary>
        private void ResetToDefaultVersionValue()
        {
            //NWDBenchmark.Start();
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
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif