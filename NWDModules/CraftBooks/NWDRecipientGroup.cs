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
	/// NWDExampleConnexion can be use in MonBehaviour script to connect GameObject with NWDBasis<Data> in editor.
	/// Use like :
	/// public class MyScriptInGame : MonoBehaviour
	/// { 
	/// [NWDConnexionAttribut (true, true, true, true)] // optional
	/// public NWDExampleConnexion MyNetWorkedData;
	/// }
	/// </summary>
	[Serializable]
	public class NWDRecipientGroupConnexion : NWDConnexion <NWDRecipientGroup> {}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("RCP")]
	[NWDClassDescriptionAttribute ("Recipient descriptions Class")]
	[NWDClassMenuNameAttribute ("Recipient")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDRecipientGroup :NWDBasis <NWDRecipientGroup>
	{
		//-------------------------------------------------------------------------------------------------------------
		//[NWDGroupEndAttribute]
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
		[NWDGroupStartAttribute("Description",true, true, true)] // ok
		public NWDReferenceType<NWDItem> ItemToDescribe { get; set; }
		[NWDGroupEndAttribute]

		[NWDSeparatorAttribute]

		[NWDGroupStartAttribute("Usage",true, true, true)] // ok
		public bool CraftOnlyMax {get; set;}
		public bool CraftUnUsedElements {get; set;}
		[NWDGroupEndAttribute]

		[NWDSeparatorAttribute]

		[NWDGroupStartAttribute("Item(s) use as recipient",true, true, true)] // ok
		public NWDReferencesListType<NWDItem> ItemList { get; set; }
		//[NWDGroupEndAttribute]
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Constructors
		//-------------------------------------------------------------------------------------------------------------
		public NWDRecipientGroup()
		{
			//Init your instance here
			CraftOnlyMax = true;
			CraftUnUsedElements = true;
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

			foreach (NWDItem tItem in NWDItem.GetAllObjects()) {
				if (tItem.RecipientGroupList != null) {
					tItem.RecipientGroupList.RemoveObjects (new NWDRecipientGroup[]{ this });
					tItem.UpdateMeIfModified();
				}
			}
			foreach (NWDItem tItem in ItemList.GetObjects()) {
				Debug.Log ("tItem must be update " + tItem.InternalKey );
				if (tItem.RecipientGroupList == null) {
					tItem.RecipientGroupList = new NWDReferencesListType<NWDRecipientGroup> ();
				}
				tItem.RecipientGroupList.AddObject(this);
				tItem.UpdateMeIfModified();
			}
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

//		public static List<NWDRecipientGroup> GetItemGroupForItem (NWDItem sItem)
//		{
//			List<NWDRecipientGroup> rReturn = new List<NWDRecipientGroup> ();
//			foreach (NWDRecipientGroup tGroup in GetAllObjects()) {
//				if (tGroup.ItemList.ContainsObject (sItem)) {
//					rReturn.Add (tGroup);
//				}
//			}
//			return rReturn;
//		}

		//-------------------------------------------------------------------------------------------------------------

	}
	//-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================