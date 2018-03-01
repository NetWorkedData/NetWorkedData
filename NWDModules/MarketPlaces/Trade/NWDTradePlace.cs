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
	public class NWDTradePlaceConnection : NWDConnection <NWDTradePlace> {}
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("TRW")]
	[NWDClassDescriptionAttribute ("Trade Place descriptions Class")]
	[NWDClassMenuNameAttribute ("Trade Place")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDTradePlace :NWDBasis <NWDTradePlace>
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
		[NWDHeaderAttribute("Representation")]

		public NWDReferenceType<NWDItem> ItemToDescribe { get; set; }

		[NWDHeaderAttribute("Informations")]

		public NWDLocalizableStringType Name { get; set; }

		public NWDLocalizableStringType SubName { get; set; }

		public NWDLocalizableStringType Description { get; set; }


		[NWDGroupStartAttribute("Classification",true, true, true)]
		public NWDReferencesListType<NWDWorld> Worlds { get; set; }
		public NWDReferencesListType<NWDCategory> Categories { get; set; }
		public NWDReferencesListType<NWDFamily> Families { get; set; }
		public NWDReferencesListType<NWDKeyword>  Keywords { get; set; }
		[NWDGroupEndAttribute]

		[NWDGroupStartAttribute("Items Authorization",true, true, true)]
		public NWDReferencesListType<NWDWorld> FilterWorlds { get; set; }
		public NWDReferencesListType<NWDCategory> FilterCategories { get; set; }
		public NWDReferencesListType<NWDFamily> FilterFamilies { get; set; }
		public NWDReferencesListType<NWDKeyword>  FilterKeywords { get; set; }
		[NWDGroupEndAttribute]

		[NWDGroupStartAttribute("Money Authorization",true, true, true)]
		public NWDReferencesListType<NWDWorld> FilterMoneyWorlds { get; set; }
		public NWDReferencesListType<NWDCategory> FilterMoneyCategories { get; set; }
		public NWDReferencesListType<NWDFamily> FilterMoneyFamilies { get; set; }
		public NWDReferencesListType<NWDKeyword>  FilterMoneyKeywords { get; set; }
		[NWDGroupEndAttribute]


		public int OpenDateTime { get; set; }
		public int CloseDateTime { get; set; }
		public int Calendar { get; set; }

		[NWDEnumAttribute(new int[]{0,1,2}, new string[]{"marketplace", "BarterPlace"})]
		public int TypeOfTrade { get; set; }
		public NWDReferencesQuantityType<NWDItem> RequestCreationItemsCost { get; set; } // not resell if cancel
		public NWDReferencesQuantityType<NWDItem> TransactionFixItemsCost { get; set; } 
		public NWDReferencesQuantityType<NWDItem> TransactionNumberOfItemsCost { get; set; }
		public float NumberStep { get; set; }

		public int RequestLifeTime { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Constructors
		//-------------------------------------------------------------------------------------------------------------
		public NWDTradePlace()
        {
            Debug.Log("NWDTradePlace Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTradePlace(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            Debug.Log("NWDTradePlace Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
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