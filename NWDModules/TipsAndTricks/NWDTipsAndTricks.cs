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
	[Serializable]
	public class NWDTipsAndTricksConnection : NWDConnection <NWDTipsAndTricks> {}
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("TAT")]
	[NWDClassDescriptionAttribute ("Tips And Tricks descriptions Class")]
	[NWDClassMenuNameAttribute ("Tips And Tricks")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDTipsAndTricks :NWDBasis <NWDTipsAndTricks>
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
		[NWDGroupStartAttribute("Classification", false , true ,true)]
        public NWDReferencesListType<NWDWorld> WorldList { get; set; }
        public NWDReferencesListType<NWDCategory> CategoryList { get; set; }
        public NWDReferencesListType<NWDFamily> FamilyList { get; set; }
        public NWDReferencesListType<NWDKeyword>  KeywordList { get; set; }
		[NWDGroupEndAttribute]

		[NWDGroupStartAttribute("Tips and Tricks", false , true ,true)]

		public NWDLocalizableStringType Title { get; set; }

		public NWDLocalizableStringType SubTitle { get; set; }

		public NWDLocalizableTextType Message { get; set; }

		public NWDSpriteType Icon { get; set; }

		[NWDHeaderAttribute("Items required to be visible")]
        public NWDReferencesConditionalType<NWDItem> ItemConditional{ get; set; }

		[NWDHeaderAttribute("Weighting")]
		[NWDIntSliderAttribute(1,9)]
		public int Weighting { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Constructors
		//-------------------------------------------------------------------------------------------------------------
		public NWDTipsAndTricks()
        {
            //Debug.Log("NWDTipsAndTricks Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTipsAndTricks(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDTipsAndTricks Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            Weighting = 1;
            Title = new NWDLocalizableStringType();
            SubTitle = new NWDLocalizableStringType();
            Message = new NWDLocalizableTextType();
        }
		//-------------------------------------------------------------------------------------------------------------
		static List<NWDTipsAndTricks> ListForRandom;
		//-------------------------------------------------------------------------------------------------------------
		public static List<NWDTipsAndTricks> PrepareListForRandom (NWDWorld sWorld=null, NWDFamily sFamily=null, NWDKeyword sKeyword=null, NWDOwnership sOwnership=null)
		{
			ListForRandom = new List<NWDTipsAndTricks> ();
			foreach (NWDTipsAndTricks tObject in NWDTipsAndTricks.ObjectsList) 
			{
				/* I list the object compatible with request
			 	* I insert in the list  each object (Frequency) times
			 	* I return the List
				*/
				for (int i = 0; i < tObject.Weighting; i++) 
				{
					ListForRandom.Add (tObject);
				}
			}
			return ListForRandom;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static NWDTipsAndTricks SelectRandomTips (bool sAbsoluteRemove = true)
		{
			NWDTipsAndTricks rReturn = null;
			// I select the tick by random 
			int tCount = ListForRandom.Count-1;
			int tIndex  = UnityEngine.Random.Range (0, tCount);
			if (tIndex >=0 && tIndex <= tCount) {
				rReturn = ListForRandom [tIndex];
				if (sAbsoluteRemove == false) {
					ListForRandom.RemoveAt (tIndex);
				} else {
					while (ListForRandom.Contains (rReturn)) {
						ListForRandom.Remove (rReturn);
					}
				}
			}
			return rReturn;
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
}
//=====================================================================================================================