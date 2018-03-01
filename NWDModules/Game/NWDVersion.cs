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
        public NWDVersionType Version
        {
            get; set;
        }
        public bool Buildable
        {
            get; set;
        }
        public bool Editable
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDSeparatorAttribute]
        [NWDGroupStartAttribute("Environment", true, true, true)]
        public bool ActiveDev
        {
            get; set;
        }
        public bool ActivePreprod
        {
            get; set;
        }
        public bool ActiveProd
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDSeparatorAttribute]
        [NWDGroupStartAttribute("Options", true, true, true)]
        public bool BlockDataUpdate
        {
            get; set;
        }
        public bool BlockApplication
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        [NWDSeparatorAttribute]
        [NWDGroupStartAttribute("Message and links", true, true, true)]
        public NWDLocalizableStringType AlertTitle
        {
            get; set;
        }
        public NWDLocalizableStringType AlertMessage
        {
            get; set;
        }
        public NWDLocalizableStringType AlertButtonOK
        {
            get; set;
        }
        public string AppleStoreURL
        {
            get; set;
        }
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
        #if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public static string GetVersionForEnvironemt(NWDAppEnvironment sEnvironment)
        {
            Debug.Log("GetVersionForEnvironemt");
            // I will change th last version of my App
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
                        }
                    }
                }
            }
            // sEnvironment.Version = tVersionString;
            return tVersionString;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void UpdateVersionBundle()
        {
            if (NWDAppConfiguration.SharedInstance().IsDevEnvironement() == false &&
                NWDAppConfiguration.SharedInstance().IsPreprodEnvironement() == false &&
                NWDAppConfiguration.SharedInstance().IsProdEnvironement() == false
            )
            {
                // error no environnment selected 
                NWDAppConfiguration.SharedInstance().DevEnvironment.Selected = true;
            }
            // I will change th last version of my App
            string tVersionString = "0.00.00";
            int tVersionInt = 0;
            int.TryParse(tVersionString.Replace(".", ""), out tVersionInt);
            NWDVersion tLastVersionObject = null;
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
                            tLastVersionObject = tVersionObject;
                        }
                    }
                }
            }
            if (tLastVersionObject != null)
            {
                if (PlayerSettings.bundleVersion != tLastVersionObject.Version.ToString())
                {
                    PlayerSettings.bundleVersion = tLastVersionObject.Version.ToString();
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
        public override void AddonUpdatedMe()
        {
            // do something when object finish to be updated
#if UNITY_EDITOR
            NWDVersion.UpdateVersionBundle();
            NWDDataManager.SharedInstance.RepaintWindowsInManager(typeof(NWDVersion));
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
        #endregion
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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================