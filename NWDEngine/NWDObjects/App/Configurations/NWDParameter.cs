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
    public class NWDParameterConnection : NWDConnection<NWDParameter>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("PRM")]
    [NWDClassDescriptionAttribute("Parameters of game. You can use this class to create Parameters of your game. \n" +
                                   "Parameters are set for all user. Use InternalKey to find them and use them. \n" +
                                   "")]
    [NWDClassMenuNameAttribute("Parameters")]
    public partial class NWDParameter : NWDBasis<NWDParameter>
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStartAttribute("Value(s)", true, true, true)]
        /// <summary>
        /// Gets or sets the value string.
        /// </summary>
        /// <value>The value string.</value>
        public NWDLocalizableType LocalizableString
        {
            get; set;
        }
        public NWDMultiType Value
        {
            get; set;
        }
        ///// <summary>
        ///// Gets or sets the value int.
        ///// </summary>
        ///// <value>The value int.</value>
        //public int ValueInt { get; set; }
        ///// <summary>
        ///// Gets or sets a value indicating whether this <see cref="NetWorkedData.NWDParameter"/> value bool.
        ///// </summary>
        ///// <value><c>true</c> if value bool; otherwise, <c>false</c>.</value>
        //public bool ValueBool { get; set; }
        ///// <summary>
        ///// Gets or sets the value float.
        ///// </summary>
        ///// <value>The value float.</value>
        //public float ValueFloat { get; set; }
        //[NWDGroupEndAttribute]
        //-------------------------------------------------------------------------------------------------------------

        #endregion

        //-------------------------------------------------------------------------------------------------------------

        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDParameter()
        {
            //Debug.Log("NWDParameter Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDParameter(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDParameter Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------

        #endregion

        //-------------------------------------------------------------------------------------------------------------

        #region Class methods

        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the local string for internal key.
        /// </summary>
        /// <returns>The local string.</returns>
        /// <param name="sKey">key.</param>
        /// <param name="sDefault">default value.</param>
        public static string GetLocalString(string sKey, string sDefault = BTBConstants.K_EMPTY_STRING)
        {
            NWDParameter tObject = NWDBasis<NWDParameter>.FindFirstDatasByInternalKey(sKey) as NWDParameter;
            string rReturn = sDefault;
            if (tObject != null)
            {
                rReturn = tObject.LocalizableString.GetLocalString();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the int value for internal key.
        /// </summary>
        /// <returns>The int.</returns>
        /// <param name="sKey">key.</param>
        /// <param name="sDefault">default value.</param>
        //public static int GetInt (string sKey, int sDefault = 0)
        //{
        //	NWDParameter tObject = NWDBasis<NWDParameter>.GetObjectByInternalKey (sKey) as NWDParameter;
        //	int rReturn = sDefault;
        //	if (tObject != null) {
        //		rReturn = tObject.ValueInt;
        //	}
        //	return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// Get the bool value for internal key.
        ///// </summary>
        ///// <returns><c>true</c>, if bool was gotten, <c>false</c> otherwise.</returns>
        ///// <param name="sKey">key.</param>
        ///// <param name="sDefault">If set to <c>true</c> default value.</param>
        //public static bool GetBool (string sKey, bool sDefault = false)
        //{
        //	NWDParameter tObject = NWDBasis<NWDParameter>.GetObjectByInternalKey (sKey) as NWDParameter;
        //	bool rReturn = sDefault;
        //	if (tObject != null) {
        //		rReturn = tObject.ValueBool;
        //	}
        //	return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// Get the float value for internal key.
        ///// </summary>
        ///// <returns>The float.</returns>
        ///// <param name="sKey">key.</param>
        ///// <param name="sDefault">default value.</param>
        //public static float GetFloat (string sKey, float sDefault = 0.0F)
        //{
        //	NWDParameter tObject = NWDBasis<NWDParameter>.GetObjectByInternalKey (sKey) as NWDParameter;
        //	float rReturn = sDefault;
        //	if (tObject != null) {
        //		rReturn = tObject.ValueFloat;
        //	}
        //	return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------

        #endregion

        //-------------------------------------------------------------------------------------------------------------

        #region Instance methods

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
    }
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================