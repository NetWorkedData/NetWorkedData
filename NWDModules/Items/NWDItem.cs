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
	/// NWDExampleConnection can be use in MonBehaviour script to connect GameObject with NWDBasis<Data> in editor.
	/// Use like :
	/// public class MyScriptInGame : MonoBehaviour
	/// { 
	/// [NWDConnectionAttribut (true, true, true, true)] // optional
	/// public NWDExampleConnection MyNetWorkedData;
	/// }
	/// </summary>
	[Serializable]
	public class NWDItemConnection : NWDConnection <NWDItem> {}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
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
        public NWDLocalizableStringType PluralName { get; set;}
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
		public float DelayBeforeUse { get; set; }
		public float DurationOfUse { get; set; }
		public float DelayBeforeReUse { get; set; }
        [NWDGroupEndAttribute]

		[NWDSeparatorAttribute]

		[NWDGroupStartAttribute("Craft Usage", true, true, true)]
		[NWDNotEditableAttribute]
		public NWDReferencesListType<NWDItemGroup> ItemGroupList { get; set; }
        public float DelayBeforeCraft { get; set; }
        public float DurationOfCraft { get; set; }
        public float DelayOfImmunity { get; set; }
        public NWDReferenceType<NWDRecipientGroup> RecipientType { get; set; }
		[NWDGroupEndAttribute]

		[NWDSeparatorAttribute]

		[NWDGroupStartAttribute ("Extensions", true, true, true)]
        public NWDReferencesQuantityType<NWDItem> ItemsContained { get; set; }
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
		[NWDGroupEndAttribute]

		[NWDSeparatorAttribute]

		[NWDGroupStartAttribute ("Precalculate", true, true, true)]
		//[NWDNotEditableAttribute]
		//public NWDReferencesListType<NWDItemGroup> ItemGroupList { get; set; }
		[NWDNotEditableAttribute]
		public NWDReferencesListType<NWDRecipientGroup> RecipientGroupList { get; set; }
		//[NWDGroupEndAttribute]

		//-------------------------------------------------------------------------------------------------------------

		#endregion

		//-------------------------------------------------------------------------------------------------------------

		#region Constructors

		//-------------------------------------------------------------------------------------------------------------
		public NWDItem ()
        {
            //Debug.Log("NWDItem Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDItem(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDItem Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
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
//			NWDReferencesListType<NWDItemGroup>  tList = new NWDReferencesListType<NWDItemGroup> ();
//			tList.AddObjects (NWDItemGroup.GetItemGroupForItem (this).ToArray());
//			ItemGroupList = tList;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonDuplicateMe ()
		{
			// do something when object will be dupplicate
			ItemGroupList = new NWDReferencesListType<NWDItemGroup>();
			RecipientGroupList = new NWDReferencesListType<NWDRecipientGroup> ();
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
        public override float AddOnNodeDrawWidth(float sDocumentWidth)
        {
            return 200.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddOnNodeDrawHeight(float sCardWidth)
        {
            return 50.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddOnNodeDraw(Rect sRect, bool sPropertysGroup)
        {
            GUI.Label(sRect, InternalDescription);
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