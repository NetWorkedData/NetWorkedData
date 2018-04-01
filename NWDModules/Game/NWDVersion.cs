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

using SQLite4Unity3d;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDRecommendationType : int
    {
        SMS,
        Email,
        EmailHTML,
        //Facebook,
        //Twitter,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public class NWDVersionConnection : NWDConnection<NWDVersion>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("VRS")]
    [NWDClassDescriptionAttribute("Version of game descriptions Class")]
    [NWDClassMenuNameAttribute("Version")]
    [NWDInternalKeyNotEditableAttribute]
    [NWDNotVersionnableAttribute]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDVersion : NWDBasis<NWDVersion>
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStartAttribute("Information", true, true, true)]
        [NWDTooltips("Version reccord in database. The format is X.XX.XX")]
        public NWDVersionType Version
        {
            get; set;
        }
        [NWDTooltips("This version can be used to build")]
        public bool Buildable
        {
            get; set;
        }
        [NWDTooltips("This version can be used to build a production build")]
        public bool Editable
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDGroupSeparatorAttribute]
        [NWDGroupStartAttribute("Environment", true, true, true)]
        [NWDTooltips("This version can be used to build dev environement")]
        public bool ActiveDev
        {
            get; set;
        }
        [NWDTooltips("This version can be used to build preprod environement")]
        public bool ActivePreprod
        {
            get; set;
        }
        [NWDTooltips("This version can be used to build prod environement")]
        public bool ActiveProd
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDGroupSeparatorAttribute]
        [NWDGroupStartAttribute("Options", true, true, true)]
        [NWDTooltips("This version block data push")]
        public bool BlockDataUpdate
        {
            get; set;
        }
        [NWDTooltips("This version block app and show Alert")]
        public bool BlockApplication
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDGroupSeparatorAttribute]
        [NWDGroupStartAttribute("Alert depriciated", true, true, true)]
        [NWDTooltips("Alert App is depriciated Title")]
        public NWDLocalizableStringType AlertTitle
        {
            get; set;
        }
        [NWDTooltips("Alert App is depriciated Message")]
        public NWDLocalizableStringType AlertMessage
        {
            get; set;
        }
        [NWDTooltips("Alert App is depriciated button text validation")]
        public NWDLocalizableStringType AlertValidation
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDGroupSeparatorAttribute]
        [NWDGroupStartAttribute("Links", true, true, true)]

        [NWDTooltips("Recommendation Subject")]
        public NWDLocalizableTextType RecommendationSubject
        {
            get; set;
        }
        [NWDTooltips("Recommendation before links")]
        public NWDLocalizableStringType Recommendation
        {
            get; set;
        }
        [NWDTooltips("URL to download App in MacOS AppStore")]
        public string OSXStoreURL
        {
            get; set;
        }
        [NWDTooltips("URL to download App in iOS AppStore")]
        public string IOSStoreURL
        {
            get; set;
        }
        [NWDTooltips("URL to download App in Google Play Store")]
        public string GooglePlayURL
        {
            get; set;
        }
        //[NWDGroupEndAttribute]
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDVersion()
        {
            //Debug.Log("NWDVersion Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDVersion(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDVersion Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public static void RecommendationBy(NWDRecommendationType sType)
        {
            Debug.Log("NWDVersion RecommendationBy()");
            NWDVersion tVersion = GetMaxVersionForEnvironemt(NWDAppEnvironment.SelectedEnvironment());
            switch (sType)
            {
                case NWDRecommendationType.SMS:
                    {
                        string tText = tVersion.Recommendation.GetLocalString() + "\r\n";
                        if (string.IsNullOrEmpty(tVersion.OSXStoreURL) == false)
                        {
                            tText += " OSX : " + tVersion.OSXStoreURL + "\r\n";
                        }
                        if (string.IsNullOrEmpty(tVersion.IOSStoreURL) == false)
                        {
                            tText += " IOS : " + tVersion.IOSStoreURL + "\r\n";
                        }
                        if (string.IsNullOrEmpty(tVersion.GooglePlayURL) == false)
                        {
                            tText += " Android : " + tVersion.GooglePlayURL + "\r\n";
                        }
                        //tText = tText.Replace(";", "");
                        tText = WWW.EscapeURL(tText).Replace("+", "%20");

                        string tSubject = tVersion.RecommendationSubject.GetLocalString();
                        tSubject = WWW.EscapeURL(tSubject).Replace("+", "%20");
                        string tSMS = "sms:?body=" + tText;
                        Debug.Log("NWDVersion RecommendationBy SMS => " + tSMS);
                        Application.OpenURL(tSMS);
                    }
                    break;
                case NWDRecommendationType.Email:
                    {

                        string tText = tVersion.Recommendation.GetLocalString() + "\n\r";
                        if (string.IsNullOrEmpty(tVersion.OSXStoreURL) == false)
                        {
                            tText += " OSX : " + tVersion.OSXStoreURL + "\n\r";
                        }
                        if (string.IsNullOrEmpty(tVersion.IOSStoreURL) == false)
                        {
                            tText += " IOS : " + tVersion.IOSStoreURL + "\n\r";
                        }
                        if (string.IsNullOrEmpty(tVersion.GooglePlayURL) == false)
                        {
                            tText += " Android : " + tVersion.GooglePlayURL + "\n\r";
                        }
                        tText += "";
                        tText = WWW.EscapeURL(tText).Replace("+", "%20");

                        string tSubject = tVersion.RecommendationSubject.GetLocalString();
                        tSubject = WWW.EscapeURL(tSubject).Replace("+", "%20");
                        string tEmail = "mailto:?subject=" + tSubject + "&body=" + tText;
                        Debug.Log("NWDVersion RecommendationBy Email => " + tEmail);
                        Application.OpenURL(tEmail);
                    }
                    break;
                case NWDRecommendationType.EmailHTML:
                    {
                        string tText = "";
                        //tText+= "<HTML><BODY>";
                        tText+= tVersion.Recommendation.GetLocalString() + "</BR>";
                        if (string.IsNullOrEmpty(tVersion.OSXStoreURL) == false)
                        {
                            tText += " OSX : <A HREF='" + tVersion.OSXStoreURL + "'>Apple Store</A></BR>";
                        }
                        if (string.IsNullOrEmpty(tVersion.IOSStoreURL) == false)
                        {
                            tText += " IOS : <A HREF='" + tVersion.IOSStoreURL + "'>" + tVersion.IOSStoreURL + "</A></BR>";
                        }
                        if (string.IsNullOrEmpty(tVersion.GooglePlayURL) == false)
                        {
                            tText += " Android : <A HREF='" + tVersion.GooglePlayURL + "'>" + tVersion.GooglePlayURL + "</A></BR>";
                        }
                        //tText += "</BODY></HTML>";
                        //tText = tText.Replace("<", "");
                        //tText = tText.Replace(">", "");
                        tText = WWW.EscapeURL(tText).Replace("+", "%20");

                        string tSubject = tVersion.RecommendationSubject.GetLocalString();
                        tSubject = WWW.EscapeURL(tSubject).Replace("+", "%20");
                        string tEmail = "mailto:?subject=" + tSubject + "&body=" + tText;
                        Debug.Log("NWDVersion RecommendationBy Email => " + tEmail);
                        Application.OpenURL(tEmail);
                    }
                    break;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetMaxVersion()
        {
            Debug.Log("NWDVersion GetMaxVersion()");
            return GetMaxVersionStringForEnvironemt(NWDAppEnvironment.SelectedEnvironment());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDVersion GetMaxVersionForEnvironemt(NWDAppEnvironment sEnvironment)
        {
            //Debug.Log("GetMaxVersionForEnvironemt");
            // I will change th last version of my App
            NWDVersion tVersion = null;
            string tVersionString = "0.00.00";
            int tVersionInt = 0;
            int.TryParse(tVersionString.Replace(".", ""), out tVersionInt);
            foreach (NWDVersion tVersionObject in NWDVersion.ObjectsList)
            {
                if (tVersionObject.TestIntegrity() == true && tVersionObject.AC == true && tVersionObject.Buildable == true)
                {
                    if ((NWDAppConfiguration.SharedInstance().DevEnvironment == sEnvironment && tVersionObject.ActiveDev == true) ||
                        (NWDAppConfiguration.SharedInstance().PreprodEnvironment == sEnvironment && tVersionObject.ActivePreprod == true) ||
                        (NWDAppConfiguration.SharedInstance().ProdEnvironment == sEnvironment && tVersionObject.ActiveProd == true))
                    {
                        int tVersionInteger = 0;
                        int.TryParse(tVersionObject.Version.ToString().Replace(".", ""), out tVersionInteger);
                        if (tVersionInt < tVersionInteger)
                        {
                            tVersionInt = tVersionInteger;
                            tVersionString = tVersionObject.Version.ToString();
                            tVersion = tVersionObject;
                        }
                    }
                }
            }
            return tVersion;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDVersion GetActualVersion()
        {
            Debug.Log("NWDVersion GetActualVersion()");
            return GetActualVersionForEnvironemt(NWDAppEnvironment.SelectedEnvironment());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDVersion GetActualVersionForEnvironemt(NWDAppEnvironment sEnvironment)
        {
            Debug.Log("NWDVersion GetActualVersionForEnvironemt()");
            NWDVersion tVersion = null;
            foreach (NWDVersion tVersionObject in NWDVersion.ObjectsList)
            {
                if (tVersionObject.TestIntegrity() == true && tVersionObject.AC == true && tVersionObject.Buildable == true)
                {
                    if ((NWDAppConfiguration.SharedInstance().DevEnvironment == sEnvironment && tVersionObject.ActiveDev == true) ||
                        (NWDAppConfiguration.SharedInstance().PreprodEnvironment == sEnvironment && tVersionObject.ActivePreprod == true) ||
                        (NWDAppConfiguration.SharedInstance().ProdEnvironment == sEnvironment && tVersionObject.ActiveProd == true))
                    {
                        if (Application.version == tVersionObject.Version.ToString())
                        {
                            tVersion = tVersionObject;
                        }
                    }
                }
            }
            return tVersion;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetMaxVersionStringForEnvironemt(NWDAppEnvironment sEnvironment)
        {
            Debug.Log("NWDVersion GetMaxVersionStringForEnvironemt()");
            string tVersionString = "0.00.00";
            NWDVersion tVersion = GetMaxVersionForEnvironemt(sEnvironment);
            if (tVersion != null)
            {
                tVersionString = tVersion.Version.ToString();
            }
            return tVersionString;
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public static void UpdateVersionBundle()
        {
            Debug.Log("NWDVersion UpdateVersionBundle()");
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
            int.TryParse(tVersionString.Replace(".", ""), out tVersionInt);
            NWDVersion tMaxVersionObject = null;
            foreach (NWDVersion tVersionObject in NWDVersion.ObjectsList)
            {
                if (tVersionObject.TestIntegrity() == true && tVersionObject.AC == true && tVersionObject.Buildable == true)
                {
                    if ((NWDAppConfiguration.SharedInstance().IsDevEnvironement() && tVersionObject.ActiveDev == true) ||
                        (NWDAppConfiguration.SharedInstance().IsPreprodEnvironement() && tVersionObject.ActivePreprod == true) ||
                        (NWDAppConfiguration.SharedInstance().IsProdEnvironement() && tVersionObject.ActiveProd == true))
                    {
                        int tVersionInteger = 0;
                        int.TryParse(tVersionObject.Version.ToString().Replace(".", ""), out tVersionInteger);
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
#endif
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public override int WebServiceVersionToUse()
        {
            return 0;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public void MyInstanceMethod()
        {
            // do something with this object
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region NetWorkedData addons methods
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonInsertMe()
        {
            // do something when object will be inserted
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            // do something when object will be updated
        }
        //-------------------------------------------------------------------------------------------------------------
        //public override void AddonVersionMe(){
        //    WebServiceVersion = 0;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdatedMe()
        {
            // do something when object finish to be updated
#if UNITY_EDITOR
            NWDVersion.UpdateVersionBundle();
            NWDDataManager.SharedInstance().RepaintWindowsInManager(typeof(NWDVersion));
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDuplicateMe()
        {
            // do something when object will be dupplicate
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonEnableMe()
        {
            // do something when object will be enabled
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDisableMe()
        {
            // do something when object will be disabled
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonTrashMe()
        {
            // do something when object will be put in trash
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUnTrashMe()
        {
            // do something when object will be remove from trash
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        //Addons for Edition
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
            float tWidth = sInRect.width - NWDConstants.kFieldMarge * 2;
            float tX = sInRect.position.x + NWDConstants.kFieldMarge;
            float tY = sInRect.position.y + NWDConstants.kFieldMarge;

            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent("A"), tWidth);

            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent("A"), tWidth);

            float tYadd = 0.0f;
            // darw information about actual bundle 
            EditorGUI.BeginDisabledGroup(true);

            EditorGUI.DrawRect(new Rect(tX, tY + tYadd, tWidth, 1), NWDVersion.kRowColorLine);
            tYadd += NWDConstants.kFieldMarge;

            GUI.Label(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "Environement selected to build", EditorStyles.boldLabel);
            tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

            EditorGUI.LabelField(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "Environment", NWDAppConfiguration.SharedInstance().SelectedEnvironment().Environment);
            tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

            EditorGUI.LabelField(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "Version", PlayerSettings.bundleVersion);
            tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

            EditorGUI.DrawRect(new Rect(tX, tY + tYadd, tWidth, 1), NWDVersion.kRowColorLine);
            tYadd += NWDConstants.kFieldMarge;

            EditorGUI.EndDisabledGroup();

            if (GUI.Button(new Rect(tX, tY + tYadd, tWidth, tMiniButtonStyle.fixedHeight), "Environment chooser", tMiniButtonStyle))
            {
                NWDEditorMenu.EnvironementChooserShow();
            }
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;

            tYadd += NWDConstants.kFieldMarge;

            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight()
        {
            // Height calculate for the interface addon for editor
            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight(new GUIContent("A"), 100);
            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent("A"), 100);

            float tYadd = 0.0f;

            tYadd += NWDConstants.kFieldMarge;

            tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

            tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

            tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

            tYadd += NWDConstants.kFieldMarge;

            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;

            tYadd += NWDConstants.kFieldMarge;

            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endif
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================