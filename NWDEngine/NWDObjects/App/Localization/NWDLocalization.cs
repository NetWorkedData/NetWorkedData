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
    public class NWDLocalizationConnection : NWDConnection<NWDLocalization>     {         public string GetLocalString(string sDefault = BTBConstants.K_EMPTY_STRING)         {             NWDLocalization tObject = GetObject();             if (tObject != null)             {                 return GetObject().GetLocalString();             }              return sDefault;         }     }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("LCL")]
    [NWDClassDescriptionAttribute("Localization are used to localize the string of your game.\n" +
                                   "It's dependent from the \"Localization\" menu items in editor.\n" +
                                   "")]
    [NWDClassMenuNameAttribute("Localization")]
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// NWD game configuration.
    /// </summary>
    public partial class NWDLocalization : NWDBasis<NWDLocalization>
    {
        //#warning YOU MUST FOLLOW THIS INSTRUCTIONS
        //-------------------------------------------------------------------------------------------------------------
        // YOU MUST GENERATE PHP FOR THIS CLASS AFTER FIELD THIS CLASS WITH YOUR PROPERTIES
        // YOU MUST GENERATE WEBSITE AND UPLOAD THE FOLDER ON YOUR SERVER
        // YOU MUST UPDATE TABLE ON THE SERVER WITH THE MENU FOR DEV, FOR PREPROD AND FOR PROD
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        // Your properties
        [NWDGroupStartAttribute("Localization", true, true, true)]


        /// <summary>
        /// Gets or sets the value string.
        /// </summary>
        /// <value>The value string.</value>
        [NWDTooltips("The localizable value")]
        public NWDLocalizableTextType TextValue
        {
            get; set;
        }
        [NWDTooltips("The Key to use to replace by the value use something like {xxxxx} or #xxxx# empty if localization is not use as autoreplace value")]
        public string KeyValue
        {
            get; set;
        }
        [NWDGroupEndAttribute]

        [NWDGroupSeparatorAttribute]
        [NWDGroupStartAttribute("Development addons", true, true, true)]
        /// <summary>
        /// Gets or sets the annexe value.
        /// </summary>
        /// <value>The annexe value.</value>
        public NWDMultiType AnnexeValue
        {
            get; set;
        }
        //[NWDGroupEndAttribute]
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDLocalization()
        {
            //Debug.Log("NWDLocalization Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDLocalization(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDLocalization Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Add a base string for internal key.
        /// </summary>
        /// <returns>The base string.</returns>
        /// <param name="sKey">key.</param>
        /// <param name="sDefault">default value.</param>
        public static NWDLocalization CreateLocalizationTextValue(string sKey, string sDefault = BTBConstants.K_EMPTY_STRING)
        {
            NWDLocalization rReturn = NewData();
            rReturn.InternalKey = sKey;
            if (sDefault != string.Empty)
            {
                rReturn.TextValue.AddBaseString(sDefault);
            }
            else
            {
                rReturn.TextValue.AddBaseString(sKey);
            }
            rReturn.UpdateData();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Add an annexe value for a string.
        /// </summary>
        /// <returns>The base string.</returns>
        /// <param name="sKey">key.</param>
        /// <param name="sDefault">default value.</param>
        public static NWDLocalization CreateLocalizationAnnexe(string sKey, string sDefault)
        {
            NWDLocalization rReturn = NewData();
            rReturn.InternalKey = sKey;
            rReturn.AnnexeValue = new NWDMultiType(sDefault);
            rReturn.UpdateData();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the local string for internal key.
        /// Add a base string if internal key is not found.
        /// </summary>
        /// <returns>The local string.</returns>
        /// <param name="sKey">key.</param>
        /// <param name="sDefault">default value.</param>
        public static string GetLocalText(string sKey, string sDefault = BTBConstants.K_EMPTY_STRING)
        {
            NWDLocalization tObject = FindFirstDatasByInternalKey(sKey, true) as NWDLocalization;
            string rReturn = sDefault;
            if (tObject != null)
            {
                rReturn = tObject.TextValue.GetLocalString();
            }
            else
            {
                CreateLocalizationTextValue(sKey, sDefault);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDMultiType GetAnnexeValue(string sKey, string sDefault = BTBConstants.K_EMPTY_STRING)
        {
            NWDLocalization tObject = FindFirstDatasByInternalKey(sKey, true) as NWDLocalization;
            NWDMultiType rReturn = new NWDMultiType();
            if (tObject != null)
            {
                rReturn = tObject.AnnexeValue;
            }
            else
            {
                CreateLocalizationAnnexe(sKey, sDefault);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetAnnexeString(string sKey, string sDefault = BTBConstants.K_EMPTY_STRING)
        {
            NWDLocalization tObject = FindFirstDatasByInternalKey(sKey, true) as NWDLocalization;
            string rReturn = sDefault;
            if (tObject != null)
            {
                rReturn = tObject.AnnexeValue.ToString();
            }
            else
            {
                CreateLocalizationAnnexe(sKey, sDefault);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool GetAnnexeBool(string sKey, bool sDefault = false)
        {
            NWDLocalization tObject = FindFirstDatasByInternalKey(sKey, true) as NWDLocalization;
            bool rReturn = sDefault;
            if (tObject != null)
            {
                rReturn = tObject.AnnexeValue.GetBool();
            }
            else
            {
                CreateLocalizationAnnexe(sKey, sDefault.ToString());
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static float GetAnnexeFloat(string sKey, float sDefault = 0.0f)
        {
            NWDLocalization tObject = FindFirstDatasByInternalKey(sKey, true) as NWDLocalization;
            float rReturn = sDefault;
            if (tObject != null)
            {
                rReturn = tObject.AnnexeValue.GetFloat();
            }
            else
            {
                CreateLocalizationAnnexe(sKey, sDefault.ToString());
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static float GetAnnexeInt(string sKey, int sDefault = 0)
        {
            NWDLocalization tObject = FindFirstDatasByInternalKey(sKey, true) as NWDLocalization;
            int rReturn = sDefault;
            if (tObject != null)
            {
                rReturn = tObject.AnnexeValue.GetInt();
            }
            else
            {
                CreateLocalizationAnnexe(sKey, sDefault.ToString());
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /*public static void AutoLocalize(UnityEngine.UI.Text sText, string sDefault = "", GameObject tParent = null)
        {
            if (NWDTypeLauncher.DataLoaded)
            {
                if (sText != null)
                {
                    if (sDefault.Equals(""))
                    {
                        sDefault = sText.text;
                    }

                    NWDLocalization tObject = FindFirstDatasByInternalKey(sText.text, true) as NWDLocalization;
                    if (tObject != null)
                    {
                        string tText = tObject.TextValue.GetLocalString();
                        sText.text = tText.Replace("<br>", "\n");
                    }
                    else
                    {
                        tObject = CreateLocalizationTextValue(sText.text, sDefault);
                        tObject.Tag = NWDBasisTag.TagInternal;
                        tObject.UpdateData();
                    }
                }
                else
                {
#if UNITY_EDITOR
                    if (tParent != null)
                    {
                        Debug.LogWarning("AutoLocalize : Text component is null", tParent);
                    }
                    if (EditorUtility.DisplayDialog("AutoLocalize", "Text component is null", "OK"))
                    {
                        if (tParent != null)
                        {
                            Selection.activeObject = tParent;
                        }
                    }
#endif
                }
            }
            else
            {
#if UNITY_EDITOR
                EditorUtility.DisplayDialog("AutoLocalize", "NWD engine not loaded", "OK");
#endif
            }
        }*/
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Enrichment the specified sText, with sLanguage and sBold.
        /// </summary>
        /// <returns>The enrichment.</returns>
        /// <param name="sText">S text.</param>
        /// <param name="sLanguage">S language.</param>
        /// <param name="sBold">If set to <c>true</c> s bold.</param>
        public static string Enrichment(string sText, string sLanguage = null, bool sBold = true)
        {
            string rText = sText;
            string tBstart = "<b>";
            string tBend = "</b>";
            if (sBold == false)
            {
                tBstart = string.Empty;
                tBend = string.Empty;
            }
            if (sLanguage == null)
            {
                sLanguage = NWDDataManager.SharedInstance().PlayerLanguage;
            }
            int tI = 0;
            while (tI <= 10)
            {
                tI++;
                int tJ = 0;
                foreach (NWDLocalization tObject in GlobalLocalizerKeys)
                {
                    if (sText.Contains(tObject.KeyValue))
                    {
                        tJ++;
                        if (string.IsNullOrEmpty(tObject.InternalKey) == false)
                        {
                            rText = rText.Replace(tObject.InternalKey, tBstart + tObject.TextValue.GetLanguageString(sLanguage) + tBend);
                        }
                    }
                }
                if (tJ == 0)
                {
                    break;
                }
            }
            return rText;
        }
        //-------------------------------------------------------------------------------------------------------------
        private static List<NWDLocalization> GlobalLocalizerKeys;
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
        public string GetLocalString()         {             string tText = TextValue.GetLocalString();             return tText.Replace("<br>", Environment.NewLine);         }
        //-------------------------------------------------------------------------------------------------------------
        #region NetWorkedData addons methods
        //-------------------------------------------------------------------------------------------------------------
        private void UpdateInEnrichment()
        {
            if (GlobalLocalizerKeys == null)
            {
                GlobalLocalizerKeys = new List<NWDLocalization>();
            }
            if (XX > 0)
            {
                RemoveEnrichment();
            }
            else
            {
                if (string.IsNullOrEmpty(KeyValue) == false && GlobalLocalizerKeys.Contains(this) == false)
                {
                    GlobalLocalizerKeys.Add(this);
                }
                else if (string.IsNullOrEmpty(KeyValue) == true && GlobalLocalizerKeys.Contains(this) == true)
                {
                    GlobalLocalizerKeys.Remove(this);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void RemoveEnrichment()
        {
            if (GlobalLocalizerKeys == null)
            {
                GlobalLocalizerKeys = new List<NWDLocalization>();
            }
            if (GlobalLocalizerKeys.Contains(this) == true)
            {
                GlobalLocalizerKeys.Remove(this);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just after loaded from database.
        /// </summary>
        public override void AddonLoadedMe()
        {
            // do something when object was loaded
            // TODO verif if method is call in good place in good timing
            UpdateInEnrichment();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before unload from memory.
        /// </summary>
        public override void AddonUnloadMe()
        {
            // do something when object will be unload
            // TODO verif if method is call in good place in good timing
            RemoveEnrichment();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonInsertMe()
        {
            // do something when object will be inserted
            UpdateInEnrichment();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            // do something when object will be updated
            UpdateInEnrichment();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdatedMe()
        {
            // do something when object finish to be updated
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
            UpdateInEnrichment();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUnTrashMe()
        {
            // do something when object will be remove from trash
            UpdateInEnrichment();
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
                // do something
            }
            return sNeedBeUpdate;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditor(Rect sInRect)
        {
            // Draw the interface addon for editor
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight()
        {
            // Height calculate for the interface addon for editor
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================