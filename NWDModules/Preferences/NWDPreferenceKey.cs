//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDPreferenceKeyDomain : int
    {
        AccountPref,
        GameSavePref,
        LocalPref,
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
    ///         public NWDPreferenceKeyConnection MyNetWorkedData;
    ///         public void UseData()
    ///             {
    ///                 NWDPreferenceKey tObject = MyNetWorkedData.GetObject();
    ///                 // Use tObject
    ///             }
    ///     }
    /// </code>
    /// </example>
    /// </summary>
    [Serializable]
    public class NWDPreferenceKeyConnection : NWDConnection<NWDPreferenceKey>
    {
        //-------------------------------------------------------------------------------------------------------------
        public void SetString(string sValue)
        {
            NWDPreferenceKey tPref = GetObject();
            if (tPref != null)
            {
                tPref.SetString(sValue);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetInt(int sValue)
        {
            NWDPreferenceKey tPref = GetObject();
            if (tPref != null)
            {
                tPref.SetInt(sValue);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetFloat(float sValue)
        {
            NWDPreferenceKey tPref = GetObject();
            if (tPref != null)
            {
                tPref.SetFloat(sValue);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetBool(bool sValue)
        {
            NWDPreferenceKey tPref = GetObject();
            if (tPref != null)
            {
                tPref.SetBool(sValue);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetString(string sNotExistValue = BTBConstants.K_EMPTY_STRING)
        {
            string rReturn = sNotExistValue;
            NWDPreferenceKey tPref = GetObject();
            if (tPref != null)
            {
                rReturn = tPref.GetString();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int GetInt(int sNotExistValue = 0)
        {
            int rReturn = sNotExistValue;
            NWDPreferenceKey tPref = GetObject();
            if (tPref != null)
            {
                rReturn = tPref.GetInt();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public float GetFloat(float sNotExistValue = 0.0F)
        {
            float rReturn = sNotExistValue;
            NWDPreferenceKey tPref = GetObject();
            if (tPref != null)
            {
                rReturn = tPref.GetFloat();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool GetBool(bool sNotExistValue = true)
        {
            bool rReturn = sNotExistValue;
            NWDPreferenceKey tPref = GetObject();
            if (tPref != null)
            {
                rReturn = tPref.GetBool();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ToogleBool(bool sNotExistValue = true)
        {
            bool rReturn = sNotExistValue;
            NWDPreferenceKey tPref = GetObject();
            if (tPref != null)
            {
                rReturn = tPref.GetBool();
                rReturn = !rReturn;
                tPref.SetBool(rReturn);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDPreferenceKey class. This class is use for (complete description here).
    /// </summary>
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("PFK")]
    [NWDClassDescriptionAttribute("preference key")]
    [NWDClassMenuNameAttribute("Preference Key")]
    [NWDClassPhpPostCalculateAttribute(" // write your php script here to update $tReference")]
    //[NWDInternalKeyNotEditableAttribute]
    public partial class NWDPreferenceKey : NWDBasis<NWDPreferenceKey>
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Instance Properties
        //-------------------------------------------------------------------------------------------------------------
        // generic notification
        public const string K_PREFERENCE_CHANGED_KEY = "K_PREFERENCE_CHANGED_KEY_8zQr95er"; // OK Needed by test & verify
        //-------------------------------------------------------------------------------------------------------------
        //PROPERTIES
        //public string Key
        //{
        //    get; set;
        //}
        public NWDPreferenceKeyDomain Domain
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
        public NWDPreferenceKey()
        {
            //Debug.Log("NWDPreferenceKey Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDPreferenceKey(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDPreferenceKey Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
            //Key = Reference;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public static void ErrorRegenerate()
        {
#if UNITY_EDITOR
            NWDError.CreateGenericError("NWDPreferenceKey BasicError", "PFKz01", "Internal error", "Internal error to test", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagInternal);
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ClassInitialization() // call by invoke
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exampel of implement for class method.
        /// </summary>
        public static void MyClassMethod()
        {
            // do something with this class
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exampel of implement for instance method.
        /// </summary>
        private string GetValue()
        {
            string rReturn = Default;
            switch (Domain)
            {
                case NWDPreferenceKeyDomain.AccountPref:
                    {
                        rReturn = NWDAccountPreference.GetString(Reference, Default);
                    }
                    break;
                case NWDPreferenceKeyDomain.GameSavePref:
                    {

                        rReturn = NWDUserPreference.GetString(Reference, Default);
                    }
                    break;
                default:
                    {
                        rReturn = PlayerPrefs.GetString(Reference, Default);
                    }
                    break;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int GetInt()
        {
            int rReturn = 0;
            string tR = GetValue();
            int.TryParse(tR, out rReturn);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public float GetFloat()
        {
            float rReturn = 0.0F;
            string tR = GetValue();
            float.TryParse(tR, out rReturn);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool GetBool()
        {
            bool rReturn = true;
            string tR = GetValue();
            bool.TryParse(tR, out rReturn);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetString()
        {
            return GetValue();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void SetValue(string sValue)
        {
            string tOldValue = NWDAccountPreference.GetString(Reference);
            if (!sValue.Equals(tOldValue))
            {
                switch (Domain)
                {
                    case NWDPreferenceKeyDomain.AccountPref:
                        {
                            NWDAccountPreference.SetString(Reference, sValue);
                        }
                        break;
                    case NWDPreferenceKeyDomain.GameSavePref:
                        {

                            NWDUserPreference.SetString(Reference, sValue);
                        }
                        break;
                    default:
                        {
                            PlayerPrefs.SetString(Reference, sValue);
                        }
                        break;
                }
                BTBNotificationManager.SharedInstance().PostNotification(this, K_PREFERENCE_CHANGED_KEY);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetString(string sValue)
        {
            SetValue(sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetInt(int sValue)
        {
            SetValue(sValue.ToString(NWDConstants.FormatCountry));
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetFloat(float sValue)
        {
            SetValue(sValue.ToString(NWDConstants.FormatCountry));
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetBool(bool sValue)
        {
            SetValue(sValue.ToString(NWDConstants.FormatCountry));
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region NetWorkedData addons methods
        //-------------------------------------------------------------------------------------------------------------
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDPreferenceKey)/*, typeof(NWDUserNickname), etc*/ };
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
        public override void AddonIndexMe()
        {
            // InsertInIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDesindexMe()
        {
            // RemoveFromIndex();
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