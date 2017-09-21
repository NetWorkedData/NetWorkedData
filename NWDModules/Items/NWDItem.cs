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
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("ITM")]
	[NWDClassDescriptionAttribute ("Item descriptions Class")]
	[NWDClassMenuNameAttribute ("Item")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDItem : NWDBasis <NWDItem>
	{
		//-------------------------------------------------------------------------------------------------------------
		//#warning YOU MUST FOLLOW THIS INSTRUCTIONS
		//-------------------------------------------------------------------------------------------------------------
		// YOU MUST GENERATE PHP FOR THIS CLASS AFTER FIELD THIS CLASS WITH YOUR PROPERTIES
		// YOU MUST GENERATE WEBSITE AND UPLOAD THE FOLDER ON YOUR SERVER
		// YOU MUST UPDATE TABLE ON THE SERVER WITH THE MENU FOR DEV, FOR PREPROD AND FOR PROD
		//-------------------------------------------------------------------------------------------------------------

		#region Properties

		//-------------------------------------------------------------------------------------------------------------
		// Your properties
		[NWDGroupStartAttribute ("Description", true, true, true)]
		public NWDLocalizableStringType Name { get; set; }
		public NWDLocalizableStringType SubName { get; set; }
		public NWDLocalizableStringType Description { get; set; }
		[NWDGroupEndAttribute]

		[NWDSeparatorAttribute]

		[NWDGroupStartAttribute ("Classification", true, true, true)]
		public NWDReferencesListType<NWDWorld> Worlds { get; set; }
		public NWDReferencesListType<NWDCategory> Categories { get; set; }
		public NWDReferencesListType<NWDFamily> Families { get; set; }
		public NWDReferencesListType<NWDKeyword> Keywords { get; set; }
		[NWDGroupEndAttribute]

		[NWDSeparatorAttribute]
	
		[NWDGroupStartAttribute ("Rarity", true, true, true)]
		[NWDFloatSliderAttribute (0.0F, 1.0F)]
		[NWDEntitledAttribute ("Rarity : float [0,1]")]
		public float Rarity { get; set; }
		[NWDGroupEndAttribute]

		[NWDSeparatorAttribute]

		[NWDGroupStartAttribute ("Usage", true, true, true)]
		public bool Usable { get; set; }
		public int DelayToUse { get; set; }
		public int DelayToReUse { get; set; }
		[NWDGroupEndAttribute]

		[NWDSeparatorAttribute]

		[NWDGroupStartAttribute ("Extensions", true, true, true)]
		public NWDReferencesQuantityType<NWDItem> ItemsContained{ get; set; }
		public NWDReferencesQuantityType<NWDItemProperties> ItemProperties { get; set; }
		[NWDGroupEndAttribute]

		[NWDSeparatorAttribute]

		[NWDGroupStartAttribute ("Assets", true, true, true)]
		[NWDHeaderAttribute ("Textures")]
		public NWDTextureType PrimaryTexture { get; set; }
		public NWDTextureType SecondaryTexture { get; set; }
		public NWDTextureType TertiaryTexture { get; set; }
		[NWDHeaderAttribute ("Colors")]
		public NWDColorType PrimaryColor { get; set; }
		public NWDColorType SecondaryColor { get; set; }
		public NWDColorType TertiaryColor { get; set; }
		[NWDHeaderAttribute ("Prefabs")]
		public NWDPrefabType PrimaryPrefab { get; set; }
		public NWDPrefabType SecondaryPrefab { get; set; }
		public NWDPrefabType TertiaryPrefab { get; set; }
		[NWDGroupEndAttribute]

		[NWDSeparatorAttribute]

		[NWDGroupStartAttribute ("Development addons", true, true, true)]
		public string JSON { get; set; }
		public string KeysValues { get; set; }
		//[NWDGroupEndAttribute]

		//-------------------------------------------------------------------------------------------------------------

		#endregion

		//-------------------------------------------------------------------------------------------------------------

		#region Constructors

		//-------------------------------------------------------------------------------------------------------------
		public NWDItem ()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------

		#endregion

		//-------------------------------------------------------------------------------------------------------------

		#region Class methods

		//-------------------------------------------------------------------------------------------------------------
		public static void MyClassMethod ()
		{
			// do something with this class
		}
		//-------------------------------------------------------------------------------------------------------------

		#endregion

		//-------------------------------------------------------------------------------------------------------------

		#region Instance methods

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
	#region Connexion NWDItem with Unity MonoBehavior
	//-------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// NWDItem connexion.
	/// In your MonoBehaviour Script connect object with :
	/// <code>
	///	[NWDConnexionAttribut(true,true, true, true)]
	/// public NWDItemConnexion MyNWDItemObject;
	/// </code>
	/// </summary>
	//-------------------------------------------------------------------------------------------------------------
	// CONNEXION STRUCTURE METHODS
	//-------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class NWDItemConnexion
	{
		//-------------------------------------------------------------------------------------------------------------
		[SerializeField]
		public string Reference;
		//-------------------------------------------------------------------------------------------------------------
		public NWDItem GetObject ()
		{
			return NWDItem.GetObjectByReference (Reference);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetObject (NWDItem sObject)
		{
			if (sObject != null) {
				Reference = sObject.Reference;
			} else {
				Reference = "";
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDItem NewObject ()
		{
			NWDItem tObject = NWDItem.NewObject ();
			Reference = tObject.Reference;
			return tObject;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//-------------------------------------------------------------------------------------------------------------
	// CUSTOM PROPERTY DRAWER METHODS
	//-------------------------------------------------------------------------------------------------------------
	#if UNITY_EDITOR
	//-------------------------------------------------------------------------------------------------------------
	[CustomPropertyDrawer (typeof(NWDItemConnexion))]
	public class NWDItemConnexionDrawer : PropertyDrawer
	{
		//-------------------------------------------------------------------------------------------------------------
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			Debug.Log ("GetPropertyHeight");
			NWDConnexionAttribut tReferenceConnexion = new NWDConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true).Length > 0) {
				tReferenceConnexion = (NWDConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true) [0];
			}
			return NWDItem.ReferenceConnexionHeightSerialized (property, tReferenceConnexion.ShowInspector);
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			Debug.Log ("OnGUI");
			NWDConnexionAttribut tReferenceConnexion = new NWDConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true).Length > 0) {
				tReferenceConnexion = (NWDConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true) [0];
			}
			NWDItem.ReferenceConnexionFieldSerialized (position, property.displayName, property, "", tReferenceConnexion.ShowInspector, tReferenceConnexion.Editable, tReferenceConnexion.EditButton, tReferenceConnexion.NewButton);
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//-------------------------------------------------------------------------------------------------------------
	#endif
	//-------------------------------------------------------------------------------------------------------------
	// Example of monobehaviour component
	// This class example can be use to simple connect gameobject with NWDItem Data
	// You can use this class to connect prefab, gameobject , etc.
	//-------------------------------------------------------------------------------------------------------------
	public class NWDItemMonoBehavior : MonoBehaviour
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDItemConnexion NetWorkedDataObject;
		//-------------------------------------------------------------------------------------------------------------
		public static NWDItemMonoBehavior SetNetWorkedDataObject (GameObject sGameObject, NWDItem sNetWorkedDataObject)
		{
			NWDItemMonoBehavior tMonoBehavior = sGameObject.GetComponent<NWDItemMonoBehavior> () as NWDItemMonoBehavior;
			if (tMonoBehavior == null) {
				tMonoBehavior = sGameObject.AddComponent<NWDItemMonoBehavior> ();
			}
			tMonoBehavior.SetNetWorkedDataObject (sNetWorkedDataObject);
			return tMonoBehavior;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static NWDItem GetNetWorkedDataObject (GameObject sGameObject)
		{
			NWDItem rReturn = null;
			NWDItemMonoBehavior tMonoBehavior = sGameObject.GetComponent<NWDItemMonoBehavior> () as NWDItemMonoBehavior;
			if (tMonoBehavior != null) {
				rReturn = tMonoBehavior.GetNetWorkedDataObject ();
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDItem GetNetWorkedDataObject ()
		{
			return NetWorkedDataObject.GetObject ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetNetWorkedDataObject (NWDItem sNetWorkedDataObject)
		{
			NetWorkedDataObject.SetObject (sNetWorkedDataObject);
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDItem : NWDBasis <NWDItem>
	{
		//-------------------------------------------------------------------------------------------------------------
		public static NWDItemMonoBehavior SetNetWorkedDataObject (GameObject sGameObject, NWDItem sNetWorkedDataObject)
		{
			return NWDItemMonoBehavior.SetNetWorkedDataObject (sGameObject, sNetWorkedDataObject);
		}
		//-------------------------------------------------------------------------------------------------------------
		public static NWDItem GetNetWorkedDataObject (GameObject sGameObject)
		{
			return NWDItemMonoBehavior.GetNetWorkedDataObject (sGameObject);
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//-------------------------------------------------------------------------------------------------------------
	#endregion
	//-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================