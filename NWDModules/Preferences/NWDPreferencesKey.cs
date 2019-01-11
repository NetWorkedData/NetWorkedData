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

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDPreferencesDomain : int
    {
        AccountPreferences = 0,
        UserPreferences = 1,
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
    ///         public NWDStatKeyConnection MyNetWorkedData;
    ///         public void UseData()
    ///             {
    ///                 NWDStatKey tObject = MyNetWorkedData.GetObject();
    ///                 // Use tObject
    ///             }
    ///     }
    /// </code>
    /// </example>
    /// </summary>
    [Serializable]
    public class NWDPreferencesKeyConnection : NWDConnection<NWDPreferencesKey>
    {
        //-------------------------------------------------------------------------------------------------------------
        public void SetString(string sValue)
        {
            NWDPreferencesKey tPref = GetObject();
            if (tPref != null)
            {
                tPref.AddEnter(new NWDMultiType (sValue));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetInt(int sValue)
        {
            NWDPreferencesKey tPref = GetObject();
            if (tPref != null)
            {
                tPref.AddEnter(new NWDMultiType(sValue));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetFloat(float sValue)
        {
            NWDPreferencesKey tPref = GetObject();
            if (tPref != null)
            {
                tPref.AddEnter(new NWDMultiType(sValue));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetBool(bool sValue)
        {
            NWDPreferencesKey tPref = GetObject();
            if (tPref != null)
            {
                tPref.AddEnter(new NWDMultiType(sValue));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetString(string sNotExistValue = BTBConstants.K_EMPTY_STRING)
        {
            string rReturn = sNotExistValue;
            NWDPreferencesKey tPref = GetObject();
            if (tPref != null)
            {
                rReturn = tPref.GetEnter().GetString();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int GetInt(int sNotExistValue = 0)
        {
            int rReturn = sNotExistValue;
            NWDPreferencesKey tPref = GetObject();
            if (tPref != null)
            {
                rReturn = tPref.GetEnter().GetInt();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public float GetFloat(float sNotExistValue = 0.0F)
        {
            float rReturn = sNotExistValue;
            NWDPreferencesKey tPref = GetObject();
            if (tPref != null)
            {
                rReturn = tPref.GetEnter().GetFloat();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool GetBool(bool sNotExistValue = true)
        {
            bool rReturn = sNotExistValue;
            NWDPreferencesKey tPref = GetObject();
            if (tPref != null)
            {
                rReturn = tPref.GetEnter().GetBool();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ToogleBool(bool sNotExistValue = true)
        {
            bool rReturn = sNotExistValue;
            NWDPreferencesKey tPref = GetObject();
            if (tPref != null)
            {
                rReturn = tPref.GetEnter().GetBool();
                rReturn = !rReturn;
                tPref.AddEnter(new NWDMultiType(rReturn));
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDStatKey class. This class is use for (complete description here).
    /// </summary>
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("PRK")]
    [NWDClassDescriptionAttribute("Preferences Key")]
    [NWDClassMenuNameAttribute("Preferences Key")]
    //[NWDInternalKeyNotEditableAttribute]
    public partial class NWDPreferencesKey : NWDBasis<NWDPreferencesKey>
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Class Properties
        //-------------------------------------------------------------------------------------------------------------
        // generic notification
        public const string K_PREFERENCES_CHANGED_KEY = "K_PREFERENCES_CHANGED_KEY_8z12ZEer"; // OK Needed by test & verify
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance Properties
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Information")]
        public NWDLocalizableStringType Title
        {
            get; set;
        }
        public NWDLocalizableTextType Description
        {
            get; set;
        }
        public NWDPreferencesDomain Domain
        {
            get; set;
        }
        public string Default
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDPreferencesKey()
        {
            //Debug.Log("NWDStatKey Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDPreferencesKey(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDStatKey Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public static void ErrorRegenerate()
        {
#if UNITY_EDITOR
            NWDError.CreateGenericError("NWDPreferencesKey BasicError", "STKz01", "Internal error", "Internal error to test", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagInternal);
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod("ClassInitialization")]
        public static void ClassInitialization() // call by invoke
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddEnter(NWDMultiType sValue)
        {
            switch (Domain)
            {
                case NWDPreferencesDomain.AccountPreferences:
                    {
                        NWDAccountPreferences.PreferencesForKey(Reference).AddEnter(sValue);
                    }
                    break;
                case NWDPreferencesDomain.UserPreferences:
                    {
                        NWDUserPreferences.PreferencesForKey(Reference).AddEnter(sValue);
                    }
                    break;
            }
            BTBNotificationManager.SharedInstance().PostNotification(this, K_PREFERENCES_CHANGED_KEY);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDMultiType GetEnter()
        {
            NWDMultiType rReturn = new NWDMultiType();
            switch (Domain)
            {
                case NWDPreferencesDomain.AccountPreferences:
                    {
                        rReturn = NWDAccountPreferences.PreferencesForKey(Reference).GetEnter();
                    }
                    break;
                case NWDPreferencesDomain.UserPreferences:
                    {
                        rReturn =  NWDUserPreferences.PreferencesForKey(Reference).GetEnter();
                    }
                    break;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exampel of implement for instance method.
        /// </summary>
        public void MyInstanceMethod()
        {
            // do something with this object
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region NetWorkedData addons methods
        //-------------------------------------------------------------------------------------------------------------
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDStatKey) };
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just after loaded from database.
        /// </summary>
        public override void AddonLoadedMe()
        {
            // do something when object was loaded
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before unload from memory.
        /// </summary>
        public override void AddonUnloadMe()
        {
            // do something when object will be unload
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before insert.
        /// </summary>
        public override void AddonInsertMe()
        {
            // do something when object will be inserted
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before update.
        /// </summary>
        public override void AddonUpdateMe()
        {
            // do something when object will be updated
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method when updated.
        /// </summary>
        public override void AddonUpdatedMe()
        {
            // do something when object finish to be updated
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method when updated me from Web.
        /// </summary>
        public override void AddonUpdatedMeFromWeb()
        {
            // do something when object finish to be updated from CSV from WebService response
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before dupplicate.
        /// </summary>
        public override void AddonDuplicateMe()
        {
            // do something when object will be dupplicate
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before enable.
        /// </summary>
        public override void AddonEnableMe()
        {
            // do something when object will be enabled
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before disable.
        /// </summary>
        public override void AddonDisableMe()
        {
            // do something when object will be disabled
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before put in trash.
        /// </summary>
        public override void AddonTrashMe()
        {
            // do something when object will be put in trash
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before remove from trash.
        /// </summary>
        public override void AddonUnTrashMe()
        {
            // do something when object will be remove from trash
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons the delete me.
        /// </summary>
        public override void AddonDeleteMe()
        {
            // do something when object will be delete from local base
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonWebversionUpgradeMe(int sOldWebversion, int sNewWebVersion)
        {
            // do something when object will be web service upgrade
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Editor
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        //Addons for Edition
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons in edition state of object.
        /// </summary>
        /// <returns><c>true</c>, if object need to be update, <c>false</c> or not not to be update.</returns>
        /// <param name="sNeedBeUpdate">If set to <c>true</c> need be update in enter.</param>
        public override bool AddonEdited(bool sNeedBeUpdate)
        {
            if (sNeedBeUpdate == true)
            {
                // do something
            }
            return sNeedBeUpdate;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons editor interface.
        /// </summary>
        /// <returns>The editor height addon.</returns>
        /// <param name="sInRect">S in rect.</param>
        public override float AddonEditor(Rect sInRect)
        {
            // Draw the interface addon for editor
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons editor intreface expected height.
        /// </summary>
        /// <returns>The editor expected height.</returns>
        public override float AddonEditorHeight()
        {
            // Height calculate for the interface addon for editor
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the width of node draw.
        /// </summary>
        /// <returns>The on node draw width.</returns>
        /// <param name="sDocumentWidth">S document width.</param>
        public override float AddOnNodeDrawWidth(float sDocumentWidth)
        {
            return 250.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the height of node draw.
        /// </summary>
        /// <returns>The on node draw height.</returns>
        public override float AddOnNodeDrawHeight(float sCardWidth)
        {
            return 130.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds node draw.
        /// </summary>
        /// <param name="sRect">S rect.</param>
        public override void AddOnNodeDraw(Rect sRect, bool sPropertysGroup)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds color on node.
        /// </summary>
        /// <returns>The on node color.</returns>
        public override Color AddOnNodeColor()
        {
            return Color.gray;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool AddonErrorFound()
        {
            bool rReturnErrorFound = false;
            // check if you found error in Data values.
            // normal way is return false!
            return rReturnErrorFound;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================