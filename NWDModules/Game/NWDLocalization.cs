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
	public class NWDLocalizationConnection : NWDConnection <NWDLocalization> {}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("LCL")]
	[NWDClassDescriptionAttribute ("Localization of game descriptions Class")]
	[NWDClassMenuNameAttribute ("Localization")]
	//-------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// NWD game configuration.
	/// </summary>
	public partial class NWDLocalization : NWDBasis <NWDLocalization>
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
		[NWDGroupStartAttribute ("Localization", true, true, true)]
		/// <summary>
		/// Gets or sets the value string.
		/// </summary>
		/// <value>The value string.</value>
		public NWDLocalizableTextType TextValue { get; set; }
		[NWDGroupEndAttribute]

		[NWDGroupSeparatorAttribute]
		[NWDGroupStartAttribute ("Development addons", true, true, true)]
		/// <summary>
		/// Gets or sets the annexe value.
		/// </summary>
		/// <value>The annexe value.</value>
		public NWDMultiType AnnexeValue { get; set; }
		//[NWDGroupEndAttribute]
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
        #region Constructors
		//-------------------------------------------------------------------------------------------------------------
		public NWDLocalization ()
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
		public static NWDLocalization CreateLocalizationTextValue (string sKey, string sDefault = "")
		{
			NWDLocalization rReturn = NewObject ();
			rReturn.InternalKey = sKey;
			if (sDefault != "") {
                rReturn.TextValue.AddBaseString (sDefault);
			} else {
                rReturn.TextValue.AddBaseString (sKey);
			}
			rReturn.SaveModifications ();
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Add an annexe value for a string.
        /// </summary>
        /// <returns>The base string.</returns>
        /// <param name="sKey">key.</param>
        /// <param name="sDefault">default value.</param>
		public static NWDLocalization CreateLocalizationAnnexe (string sKey, string sDefault)
		{
			NWDLocalization rReturn = NewObject ();
			rReturn.InternalKey = sKey;
			rReturn.AnnexeValue = new NWDMultiType (sDefault);
			rReturn.SaveModifications ();
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
		public static string GetLocalText (string sKey, string sDefault = "")
		{
            NWDLocalization tObject = GetObjectByInternalKey (sKey, true) as NWDLocalization;
			string rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.TextValue.GetLocalString ();
			} else {
				CreateLocalizationTextValue (sKey, sDefault);
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static NWDMultiType GetAnnexeValue (string sKey, string sDefault = "")
		{
            NWDLocalization tObject = GetObjectByInternalKey (sKey, true) as NWDLocalization;
			NWDMultiType rReturn = new NWDMultiType ();
			if (tObject != null) {
				rReturn = tObject.AnnexeValue;
			} else {
				CreateLocalizationAnnexe (sKey,sDefault);
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static string GetAnnexeString (string sKey, string sDefault = "")
		{
            NWDLocalization tObject = GetObjectByInternalKey (sKey, true) as NWDLocalization;
			string rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.AnnexeValue.ToString ();
			} else {
				CreateLocalizationAnnexe (sKey,sDefault);
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static bool GetAnnexeBool (string sKey, bool sDefault = false)
		{
            NWDLocalization tObject = GetObjectByInternalKey (sKey, true) as NWDLocalization;
			bool rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.AnnexeValue.ToBool ();
			} else {
				CreateLocalizationAnnexe (sKey,sDefault.ToString ());
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static float GetAnnexeFloat (string sKey, float sDefault = 0.0f)
		{
            NWDLocalization tObject = GetObjectByInternalKey (sKey, true) as NWDLocalization;
			float rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.AnnexeValue.ToFloat ();
			} else {
				CreateLocalizationAnnexe (sKey,sDefault.ToString ());
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static float GetAnnexeInt (string sKey, int sDefault = 0)
		{
            NWDLocalization tObject = GetObjectByInternalKey (sKey, true) as NWDLocalization;
			int rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.AnnexeValue.ToInt ();
			} else {
				CreateLocalizationAnnexe (sKey,sDefault.ToString ());
			}
			return rReturn;
		}
        //-------------------------------------------------------------------------------------------------------------
        public static void AutoLocalize(UnityEngine.UI.Text sText, string sDefault = "")
        {
            if (sText != null)
            {
                if (sDefault.Equals(""))
                {
                    sDefault = sText.text;
                }

                NWDLocalization tObject = GetObjectByInternalKey(sText.text, true) as NWDLocalization;
                if (tObject != null)
                {
                    string tText = tObject.TextValue.GetLocalString();
                    sText.text = tText.Replace("<br>", "\n");
                }
                else
                {
                    CreateLocalizationTextValue(sText.text, sDefault);
                }
            }
            else
            {
				#if UNITY_EDITOR
                EditorUtility.DisplayDialog("AutoLocalize", "Text component is null", "OK");
				#endif
            }
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
		#region NetWorkedData addons methods
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