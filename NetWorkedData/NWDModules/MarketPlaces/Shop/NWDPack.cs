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
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("PCK")]
	[NWDClassDescriptionAttribute ("Pack descriptions Class")]
	[NWDClassMenuNameAttribute ("Pack")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDPack :NWDBasis <NWDPack>
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
		// for example : pack of forest hunter 
		// referenceList : pack of 5 arrows; longbow
		[NWDHeaderAttribute("Informations")]
		public NWDLocalizableStringType Name { get; set; }
		public NWDLocalizableStringType SubName { get; set; }
		public NWDLocalizableStringType Description { get; set; }
		[NWDHeaderAttribute("Images")]
		public NWDTextureType NormalTexture { get; set; }
		public NWDTextureType SelectedTexture { get; set; }

		[NWDGroupStartAttribute("Classification",true, true, true)]
		public NWDReferencesListType<NWDWorld> Worlds { get; set; }
		public NWDReferencesListType<NWDCategory> Categories { get; set; }
		public NWDReferencesListType<NWDFamily> Families { get; set; }
		public NWDReferencesListType<NWDKeyword>  Keywords { get; set; }
		[NWDGroupEndAttribute]

		[NWDHeaderAttribute("Item Pack in this Pack")]
		public NWDReferencesQuantityType<NWDItemPack> ItemPackReference { get; set; }

		[NWDHeaderAttribute("Pay with these items")]
		// the itemPack reference to pay this  or if this null,
		public NWDReferencesQuantityType<NWDItem> ItemsToPay { get; set; }
		[NWDHeaderAttribute("Or pay with in app purchase Key")]
		// … the inAppPack to paid this pack with in app purchase
		public NWDReferenceType<NWDInAppPack> InAppPurchasePack { get; set; }
		//
		//		[NWDHeaderAttribute("Shop management")]
		//		public int QuantityInShop { get; set; }
		//		public int StartSellAtDate { get; set; }
		//		public int DeliveryDelay { get; set; }
		//		public int ReapprovisioningDelay { get; set; }
		//		public int FinishSellAtDate { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Constructors
		//-------------------------------------------------------------------------------------------------------------
		public NWDPack()
		{
			//Init your instance here
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
		public NWDItem[] GetAllItemsInPack ()
		{
			List<NWDItem> tlist = new List<NWDItem> ();
			foreach (NWDItemPack tItemPack in ItemPackReference.GetObjects ()) {
				tlist.AddRange (tItemPack.Items.GetObjects ());
			}
			return tlist.ToArray ();
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
		public override bool AddonEdited( bool sNeedBeUpdate)
		{
			if (sNeedBeUpdate == true) 
			{
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
	#region Connexion NWDPack with Unity MonoBehavior
	//-------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// NWDPack connexion.
	/// In your MonoBehaviour Script connect object with :
	/// <code>
	///	[NWDConnexionAttribut(true,true, true, true)]
	/// public NWDPackConnexion MyNWDPackObject;
	/// </code>
	/// </summary>
	//-------------------------------------------------------------------------------------------------------------
	// CONNEXION STRUCTURE METHODS
	//-------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class NWDPackConnexion
	{
		//-------------------------------------------------------------------------------------------------------------
		[SerializeField]
		public string Reference;
		//-------------------------------------------------------------------------------------------------------------
		public NWDPack GetObject ()
		{
			return NWDPack.GetObjectByReference (Reference);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetObject (NWDPack sObject)
		{
			if (sObject != null) {
				Reference = sObject.Reference;
			} else {
				Reference = "";
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDPack NewObject ()
		{
			NWDPack tObject = NWDPack.NewObject ();
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
	[CustomPropertyDrawer (typeof(NWDPackConnexion))]
	public class NWDPackConnexionDrawer : PropertyDrawer
	{
		//-------------------------------------------------------------------------------------------------------------
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			Debug.Log ("GetPropertyHeight");
			NWDConnexionAttribut tReferenceConnexion = new NWDConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true)[0];
			}
			return NWDPack.ReferenceConnexionHeightSerialized(property, tReferenceConnexion.ShowInspector);
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			Debug.Log ("OnGUI");
			NWDConnexionAttribut tReferenceConnexion = new NWDConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true)[0];
			}
			NWDPack.ReferenceConnexionFieldSerialized (position, property.displayName, property, "", tReferenceConnexion.ShowInspector, tReferenceConnexion.Editable, tReferenceConnexion.EditButton, tReferenceConnexion.NewButton);
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//-------------------------------------------------------------------------------------------------------------
	#endif
	//-------------------------------------------------------------------------------------------------------------
	#endregion
	//-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================