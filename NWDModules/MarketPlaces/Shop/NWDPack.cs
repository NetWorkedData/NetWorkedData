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
	public class NWDPackConnection : NWDConnection <NWDPack> {}
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
		[NWDHeaderAttribute("Representation")]
		public NWDReferenceType<NWDItem> ItemToDescribe { get; set; }

        [NWDSeparatorAttribute]

        [NWDGroupStartAttribute("Availability schedule ", true, true, true)]
        [NWDTooltips("Availability schedule of this Pack")]
        public NWDDateTimeScheduleType AvailabilitySchedule
        {
            get; set;
        }
        [NWDGroupEndAttribute]

        [NWDSeparatorAttribute]

        [NWDGroupStartAttribute("Item Pack in this Pack", true, true, true)]
		public NWDReferencesQuantityType<NWDItemPack> ItemPackReference { get; set; }
        public NWDReferencesQuantityType<NWDItem> ItemsToPay { get; set; }
        public bool EnableFreePack { get; set; }
        [NWDGroupEndAttribute]

        [NWDSeparatorAttribute]

        [NWDGroupStartAttribute("Specific Store ID", true, true, true)]
        public string AppleID { get; set; }
        public string GoogleID { get; set; }
        public string SteamID { get; set; }
        [NWDGroupEndAttribute]

        [NWDSeparatorAttribute]

        [NWDGroupStartAttribute("Classification", true, true, true)]
        public NWDReferencesListType<NWDWorld> Worlds { get; set; }
        public NWDReferencesListType<NWDCategory> Categories { get; set; }
        public NWDReferencesListType<NWDFamily> Families { get; set; }
        public NWDReferencesListType<NWDKeyword> Keywords { get; set; }
        //[NWDGroupEndAttribute]
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Constructors
		//-------------------------------------------------------------------------------------------------------------
		public NWDPack()
        {
            //Debug.Log("NWDPack Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDPack(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDPack Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
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
        public override  void Initialization()
        {
        }
		//-------------------------------------------------------------------------------------------------------------
        public string GetIAPKey()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return GoogleID;
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return AppleID;
            }

            return "";
        }
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
        public NWDReferencesQuantityType<NWDItem> GetAllItemReferenceAndQuantity()
        {
            NWDReferencesQuantityType<NWDItem> rResult = new NWDReferencesQuantityType<NWDItem>();
            Dictionary<string, int> tDico = new Dictionary<string, int>();

            foreach (KeyValuePair<NWDItemPack, int> pair in ItemPackReference.GetObjectAndQuantity())
            {
                // Get Item Pack data
                NWDItemPack tItemPack = pair.Key;
                int tItemPackQte = pair.Value;

                // Init all Items in Item Pack
                Dictionary<NWDItem, int> tItems = tItemPack.Items.GetObjectAndQuantity();
                foreach (KeyValuePair<NWDItem, int> p in tItems)
                {
                    // Get Item data
                    NWDItem tNWDItem = p.Key;
                    int tItemQte = p.Value;

                    if(tDico.ContainsKey(tNWDItem.Reference))
                    {
                        tDico[tNWDItem.Reference] += tItemQte;
                    }
                    else
                    {
                        tDico.Add(tNWDItem.Reference, tItemQte);
                    }
                }
            }

            rResult.SetReferenceAndQuantity(tDico);

            return rResult;
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