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
using SQLite.Attribute;

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
    ///         public NWDAccountPreferenceConnection MyNetWorkedData;
    ///         public void UseData()
    ///             {
    ///                 NWDAccountPreference tObject = MyNetWorkedData.GetObject();
    ///                 // Use tObject
    ///             }
    ///     }
    /// </code>
    /// </example>
    /// </summary>
	[Serializable]
    public class NWDAccountPreferenceConnection : NWDConnection<NWDAccountPreference>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDAccountPreference class. This class is used to reccord the preference of an account.
    /// </summary>
	[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("APF")]
    [NWDClassDescriptionAttribute("Account Preferences descriptions Class")]
    [NWDClassMenuNameAttribute("Account Preferences")]
    public partial class NWDAccountPreference : NWDBasis<NWDAccountPreference>
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        [Indexed("AccountIndex", 0)]
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        public NWDMultiType Value
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountPreference()
        {
            //Debug.Log("NWDAccountPreference Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountPreference(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDAccountPreference Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAccountPreference GetPreferenceByInternalKeyOrCreate(string sInternalKey,
                                                                              string sDefaultValue) //string sInternalDescription = BTBConstants.K_EMPTY_STRING
        {
            NWDAccountPreference rObject = FindFirstDatasByInternalKey(sInternalKey) as NWDAccountPreference;
            if (rObject == null)
            {
                rObject = NewData();
                //RemoveObjectInListOfEdition(rObject);
                rObject.InternalKey = sInternalKey;
                NWDReferenceType<NWDAccount> tAccount = new NWDReferenceType<NWDAccount>();
                tAccount.SetReference(NWDAccount.GetCurrentAccountReference());
                rObject.Account = tAccount;
                NWDMultiType tValue = new NWDMultiType(sDefaultValue);
                rObject.Value = tValue;
                #if UNITY_EDITOR
                //rObject.InternalDescription = NWDAccountInfos.GetNickname();
                #endif
                rObject.Tag = NWDBasisTag.TagDeviceCreated;
                rObject.UpdateData();
                //AddObjectInListOfEdition(rObject);
            }
            return rObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the local string for internal key.
        /// </summary>
        /// <returns>The local string.</returns>
        /// <param name="sKey">key.</param>
        /// <param name="sDefault">default value.</param>
        public static string GetString(string sKey, string sDefault = "")
        {
            NWDAccountPreference tObject = GetPreferenceByInternalKeyOrCreate(sKey, sDefault);
            return tObject.Value.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Set the string for internal key.
        /// </summary>
        /// <param name="sKey">S key.</param>
        /// <param name="sValue">S value.</param>
        public static void SetString(string sKey, string sValue)
        {
            NWDAccountPreference tObject = GetPreferenceByInternalKeyOrCreate(sKey, sValue);
            tObject.Value.SetString(sValue);
            tObject.UpdateData();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the int value for internal key.
        /// </summary>
        /// <returns>The int.</returns>
        /// <param name="sKey">key.</param>
        /// <param name="sDefault">default value.</param>
        public static int GetInt(string sKey, int sDefault = 0)
        {
            NWDAccountPreference tObject = GetPreferenceByInternalKeyOrCreate(sKey, sDefault.ToString());
            return tObject.Value.GetInt();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Set the int for internal key.
        /// </summary>
        /// <param name="sKey">S key.</param>
        /// <param name="sValue">S value.</param>
        public static void SetInt(string sKey, int sValue)
        {
            NWDAccountPreference tObject = GetPreferenceByInternalKeyOrCreate(sKey, sValue.ToString());
            tObject.Value.SetInt(sValue);
            tObject.UpdateData();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the bool value for internal key.
        /// </summary>
        /// <returns><c>true</c>, if bool was gotten, <c>false</c> otherwise.</returns>
        /// <param name="sKey">key.</param>
        /// <param name="sDefault">If set to <c>true</c> default value.</param>
        public static bool GetBool(string sKey, bool sDefault = false)
        {
            NWDAccountPreference tObject = GetPreferenceByInternalKeyOrCreate(sKey, sDefault.ToString());
            return tObject.Value.GetBool();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Set the bool value for internal key.
        /// </summary>
        /// <param name="sKey">S key.</param>
        /// <param name="sValue">If set to <c>true</c> s value.</param>
        public static void SetBool(string sKey, bool sValue)
        {
            NWDAccountPreference tObject = GetPreferenceByInternalKeyOrCreate(sKey, sValue.ToString());
            tObject.Value.SetBool(sValue);
            tObject.UpdateData();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the float value for internal key.
        /// </summary>
        /// <returns>The float.</returns>
        /// <param name="sKey">key.</param>
        /// <param name="sDefault">default value.</param>
        public static float GetFloat(string sKey, float sDefault = 0.0F)
        {
            NWDAccountPreference tObject = GetPreferenceByInternalKeyOrCreate(sKey, sDefault.ToString());
            return tObject.Value.GetFloat();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Set the float value for internal key.
        /// </summary>
        /// <param name="sKey">S key.</param>
        /// <param name="sValue">S value.</param>
        public static void SetFloat(string sKey, float sValue)
        {
            NWDAccountPreference tObject = GetPreferenceByInternalKeyOrCreate(sKey, sValue.ToString());
            tObject.Value.SetFloat(sValue);
            tObject.UpdateData();
        }
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
