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

using ZXing;
using ZXing.QrCode;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDRecommendationType define the style of a recommendation in sharing app by user message.
    /// </summary>
    public enum NWDRecommendationType : int
    {
        SMS,
        Email,
        EmailHTML,
        //Facebook,
        //Twitter,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// <para>Connection is used in MonBehaviour script to connect an object by its reference from popmenu list.</para>
    /// <para>The GameObject can use the object referenced by binding in game. </para>
    /// <example>
    /// Example :
    /// <code>
    /// public class MyScriptInGame : MonoBehaviour<br/>
    ///     {
    ///         NWDConnectionAttribut (true, true, true, true)] // optional
    ///         public NWDExampleConnection MyNetWorkedData;
    ///         public void UseData()
    ///             {
    ///                 NWDExample tObject = MyNetWorkedData.GetObject();
    ///                 // Use tObject
    ///             }
    ///     }
    /// </code>
    /// </example>
    /// </summary>
    [Serializable]
    public class NWDVersionConnection : NWDConnection<NWDVersion>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("VRS")]
    [NWDClassDescriptionAttribute("Version of game with limit and block for obsolete version. \n" +
                                  "Integrate the links to store to download this version or new version. \n" +
                                  "SMS and Email message to recommend thsi version. \n" +
                                  "Manage the Dev/Preprod/Prod environement by version. \n" +
                                  "Auto change the build version of unity project. \n")]
    [NWDClassMenuNameAttribute("Version")]
    [NWDInternalKeyNotEditableAttribute]
    [NWDNotVersionnableAttribute]
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
        public NWDColorType Cartridge
        {
            get; set;
        }
        [NWDTooltips("This version can be used to build")]
        public bool Buildable
        {
            get; set;
        }
        [NWDTooltips("This version can be used to build a production build (not used)")]
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
        [NWDGroupEndAttribute]
        [NWDGroupSeparatorAttribute]
        [NWDGroupStartAttribute("Links by 'Flash By App' module ", true, true, true)]
        //[NWDNotEditable]
        [NWDTooltips("ID to download App in MacOS AppStore")]
        public string OSXStoreID
        {
            get; set;
        }
        [NWDTooltips("ID to download App in iOS AppStore")]
        public string IOSStoreID
        {
            get; set;
        }
        [NWDTooltips("ID to download App in iOS AppStore for IPad")]
        public string IPadStoreID
        {
            get; set;
        }
        [NWDTooltips("ID to download App in Google Play Store")]
        public string GooglePlayID
        {
            get; set;
        }
        //[NWDNotEditable]
        [NWDTooltips("ID to download App in Google Tablet Play Store")]
        public string GooglePlayTabID
        {
            get; set;
        }
        //[NWDNotEditable]
        [NWDTooltips("ID to download App in Windows Phone Store")]
        public string WindowsPhoneID
        {
            get; set;
        }
        //[NWDNotEditable]
        [NWDTooltips("ID to download App in Windows Store")]
        public string WindowsStoreID
        {
            get; set;
        }
        //[NWDNotEditable]
        [NWDTooltips("ID to download App in Steam Store")]
        public string SteamStoreID
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
            //Debug.Log("NWDVersion RecommendationBy()");
            NWDVersion tVersion = GetMaxVersionForEnvironemt(NWDAppEnvironment.SelectedEnvironment());

            string tToFlash = tVersion.URLMyApp(false);
            switch (sType)
            {
                case NWDRecommendationType.SMS:
                    {
                        string tText = tVersion.Recommendation.GetLocalString() + "\r\n";
                        tText += " Magic link : " + tToFlash + "\r\n";
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
                        //Debug.Log("NWDVersion RecommendationBy SMS => " + tSMS);
                        Application.OpenURL(tSMS);
                    }
                    break;
                case NWDRecommendationType.Email:
                    {

                        string tText = tVersion.Recommendation.GetLocalString() + "\n\r";
                        tText += " Magic link : " + tToFlash + "\r\n";
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
                        //Debug.Log("NWDVersion RecommendationBy Email => " + tEmail);
                        Application.OpenURL(tEmail);
                    }
                    break;
                case NWDRecommendationType.EmailHTML:
                    {
                        string tText = "";
                        //tText+= "<HTML><BODY>";
                        tText += tVersion.Recommendation.GetLocalString() + "</BR>";
                        tText += " Magic link : <A HREF='" + tToFlash + "'> got to store</A></BR>";
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
                        //Debug.Log("NWDVersion RecommendationBy Email => " + tEmail);
                        Application.OpenURL(tEmail);
                    }
                    break;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public Texture2D FlashMyApp(bool sRedirection, int sDimension)
        {
            Texture2D rTexture = new Texture2D(sDimension, sDimension);
            var color32 = Encode(URLMyApp(sRedirection), rTexture.width, rTexture.height);
            rTexture.SetPixels32(color32);
            rTexture.Apply();
            return rTexture;
        }
        //-------------------------------------------------------------------------------------------------------------
        private static Color32[] Encode(string textForEncoding, int width, int height)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width
                }
            };
            return writer.Write(textForEncoding);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string URLMyApp(bool sRedirection)
        {
            string tText = NWDAppEnvironment.SelectedEnvironment().ServerHTTPS;
            tText += NWDAppConfiguration.SharedInstance().WebServiceFolder();
            if (sRedirection == true)
            {
                tText += "/FlashMyApp.php?r=0";
            }
            else
            {
                tText += "/FlashMyApp.php?r=1";
            }
            if (string.IsNullOrEmpty(OSXStoreID) == false)
            {
                tText += "&m=" + OSXStoreID;
            }

            if (string.IsNullOrEmpty(IOSStoreID) == false)
            {
                tText += "&a=" + IOSStoreID;
            }
            if (string.IsNullOrEmpty(IPadStoreID) == false)
            {
                tText += "&b=" + IPadStoreID;
            }

            if (string.IsNullOrEmpty(GooglePlayID) == false)
            {
                tText += "&g=" + GooglePlayID;
            };
            if (string.IsNullOrEmpty(GooglePlayTabID) == false)
            {
                tText += "&h=" + GooglePlayTabID;
            };

            if (string.IsNullOrEmpty(WindowsPhoneID) == false)
            {
                tText += "&w=" + WindowsPhoneID;
            };
            if (string.IsNullOrEmpty(WindowsStoreID) == false)
            {
                tText += "&x=" + WindowsStoreID;
            };

            if (string.IsNullOrEmpty(SteamStoreID) == false)
            {
                tText += "&s=" + SteamStoreID;
            };
            return tText;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetMaxVersion()
        {
            //Debug.Log("NWDVersion GetMaxVersion()");
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
            if (NWDVersion.Datas() != null)
            {
                foreach (NWDVersion tVersionObject in NWDVersion.Datas().Datas)
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
            }
            return tVersion;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDVersion GetActualVersion()
        {
            //Debug.Log("NWDVersion GetActualVersion()");
            return GetActualVersionForEnvironemt(NWDAppEnvironment.SelectedEnvironment());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDVersion GetActualVersionForEnvironemt(NWDAppEnvironment sEnvironment)
        {
            //Debug.Log("NWDVersion GetActualVersionForEnvironemt()");
            NWDVersion tVersion = null;
            foreach (NWDVersion tVersionObject in NWDVersion.Datas().Datas)
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
            //Debug.Log("NWDVersion GetMaxVersionStringForEnvironemt()");
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
            int.TryParse(tVersionString.Replace(".", ""), out tVersionInt);
            NWDVersion tMaxVersionObject = null;
            foreach (NWDVersion tVersionObject in NWDVersion.Datas().Datas)
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

            EditorGUI.DrawRect(new Rect(tX, tY + tYadd, tWidth, 1), NWDConstants.kRowColorLine);
            tYadd += NWDConstants.kFieldMarge;

            GUI.Label(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "Environement selected to build", EditorStyles.boldLabel);
            tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

            EditorGUI.LabelField(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "Environment", NWDAppConfiguration.SharedInstance().SelectedEnvironment().Environment);
            tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

            EditorGUI.LabelField(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "Version", PlayerSettings.bundleVersion);
            tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

            EditorGUI.EndDisabledGroup();


            // draw Flash My App
            EditorGUI.TextField(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "URL My App", URLMyApp(false));
            tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
            if (GUI.Button(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "URL My App without redirection", tMiniButtonStyle))
            {
                Application.OpenURL(URLMyApp(false));
            }
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;

            EditorGUI.TextField(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "URL My App", URLMyApp(true));
            tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
            if (GUI.Button(new Rect(tX, tY + tYadd, tWidth, tTextFieldStyle.fixedHeight), "URL My App with redirection", tMiniButtonStyle))
            {
                Application.OpenURL(URLMyApp(true));
            }
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;

            // Draw QRCode texture
            Texture2D tTexture = FlashMyApp(false, 256);
            EditorGUI.DrawPreviewTexture(new Rect(tX, tY + tYadd, NWDConstants.kPrefabSize * 2, NWDConstants.kPrefabSize * 2),
                                         tTexture);
            tYadd += NWDConstants.kPrefabSize * 2 + NWDConstants.kFieldMarge;

            // Draw line 
            EditorGUI.DrawRect(new Rect(tX, tY + tYadd, tWidth, 1), NWDConstants.kRowColorLine);
            tYadd += NWDConstants.kFieldMarge;

            // Draw button choose env
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

            tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
            tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

            tYadd += NWDConstants.kFieldMarge;

            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
            tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;

            tYadd += NWDConstants.kPrefabSize * 2 + NWDConstants.kFieldMarge;

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