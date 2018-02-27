﻿//=====================================================================================================================
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
	//-------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class NWDConfigurationConnexion : NWDConnexion <NWDConfiguration> {}
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("CNF")]
	[NWDClassDescriptionAttribute ("Configurations of game descriptions Class")]
	[NWDClassMenuNameAttribute ("Configurations")]
	//-------------------------------------------------------------------------------------------------------------
//	[NWDTypeClassInPackageAttribute]
	//-------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// NWD configuration NetWorkedData class. Use to configure the game throught NetWorkedData.
	/// </summary>
	public partial class NWDConfiguration : NWDBasis <NWDConfiguration>
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
		[NWDGroupStartAttribute ("Value(s)", true, true, true)]
		/// <summary>
		/// Gets or sets the value string.
		/// </summary>
		/// <value>The value string.</value>
		public NWDLocalizableStringType ValueString { get; set; }

		/// <summary>
		/// Gets or sets the value int.
		/// </summary>
		/// <value>The value int.</value>
		public int ValueInt { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="NetWorkedData.NWDConfiguration"/> value bool.
		/// </summary>
		/// <value><c>true</c> if value bool; otherwise, <c>false</c>.</value>
		public bool ValueBool { get; set; }

		/// <summary>
		/// Gets or sets the value float.
		/// </summary>
		/// <value>The value float.</value>
		public float ValueFloat { get; set; }
		//[NWDGroupEndAttribute]
		//-------------------------------------------------------------------------------------------------------------

		#endregion

		//-------------------------------------------------------------------------------------------------------------

		#region Constructors

		//-------------------------------------------------------------------------------------------------------------
		public NWDConfiguration ()
        {
           // Debug.Log("NWDConfiguration Constructor");
            //Insert in NetWorkedData;
            NewNetWorkedData();
            //Init your instance here
            Initialization();
			//DiscoverItYourSelf = true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDConfiguration(bool sInsertInNetWorkedData)
        {
           // Debug.Log("NWDConfiguration Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
            if (sInsertInNetWorkedData == false)
            {
                // do nothing 
                // perhaps the data came from database and is allready in NetWorkedData;
            }
            else
            {
                //Insert in NetWorkedData;
                NewNetWorkedData();
                //Init your instance here
                Initialization();
            }
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
		public static string GetLocalString (string sKey, string sDefault = "")
		{
			NWDConfiguration tObject = NWDBasis<NWDConfiguration>.GetObjectByInternalKey (sKey) as NWDConfiguration;
			string rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.ValueString.GetLocalString ();
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
		public static int GetInt (string sKey, int sDefault = 0)
		{
			NWDConfiguration tObject = NWDBasis<NWDConfiguration>.GetObjectByInternalKey (sKey) as NWDConfiguration;
			int rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.ValueInt;
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Get the bool value for internal key.
		/// </summary>
		/// <returns><c>true</c>, if bool was gotten, <c>false</c> otherwise.</returns>
		/// <param name="sKey">key.</param>
		/// <param name="sDefault">If set to <c>true</c> default value.</param>
		public static bool GetBool (string sKey, bool sDefault = false)
		{
			NWDConfiguration tObject = NWDBasis<NWDConfiguration>.GetObjectByInternalKey (sKey) as NWDConfiguration;
			bool rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.ValueBool;
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Get the float value for internal key.
		/// </summary>
		/// <returns>The float.</returns>
		/// <param name="sKey">key.</param>
		/// <param name="sDefault">default value.</param>
		public static float GetFloat (string sKey, float sDefault = 0.0F)
		{
			NWDConfiguration tObject = NWDBasis<NWDConfiguration>.GetObjectByInternalKey (sKey) as NWDConfiguration;
			float rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.ValueFloat;
			}
			return rReturn;
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
		public void MyInstanceMethod ()
		{
			// do something with this object
		}
		//-------------------------------------------------------------------------------------------------------------

		#region override of NetWorkedData addons methods

		//-------------------------------------------------------------------------------------------------------------
		public override void AddonInsertMe ()
		{
			// do something when object will be inserted
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonUpdateMe ()
		{
			// do something when object will be updated
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonUpdatedMe ()
		{
			// do something when object finish to be updated
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonDuplicateMe ()
		{
			// do something when object will be dupplicate
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonEnableMe ()
		{
			// do something when object will be enabled
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonDisableMe ()
		{
			// do something when object will be disabled
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonTrashMe ()
		{
			// do something when object will be put in trash
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonUnTrashMe ()
		{
			// do something when object will be remove from trash
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		//Addons for Edition
		//-------------------------------------------------------------------------------------------------------------
		public override bool AddonEdited (bool sNeedBeUpdate)
		{
			if (sNeedBeUpdate == true) {
				// do something
			}
			return sNeedBeUpdate;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override float AddonEditor (Rect sInRect)
		{
			// Draw the interface addon for editor
			float tYadd = 0.0f;
			return tYadd;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override float AddonEditorHeight ()
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