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
	public class NWDCategoryConnexion : NWDConnexion <NWDCategory> {}
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("CAT")]
	[NWDClassDescriptionAttribute ("Categories descriptions Class")]
	[NWDClassMenuNameAttribute ("Categories")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDCategory : NWDBasis <NWDCategory>
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
		[NWDGroupStartAttribute ("Informations", true, true, true)]
		public NWDLocalizableStringType Name { get; set; }
		public NWDReferenceType<NWDCategory> Parent { get; set; } // TODO : NWDHiearchicList
		[NWDGroupEndAttribute]
		[NWDSeparatorAttribute]
		[NWDGroupStartAttribute ("Description", true, true, true)]
		public NWDReferenceType<NWDItem> ItemToDescribe { get; set;}
		//[NWDGroupEndAttribute]
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Constructors
		//-------------------------------------------------------------------------------------------------------------
		public NWDCategory()
        {
            //Debug.Log("NWDCategory Constructor");
            //Insert in NetWorkedData;
            NewNetWorkedData();
            //Init your instance here
            Initialization();
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCategory(bool sInsertInNetWorkedData)
        {
            //Debug.Log("NWDCategory Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
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
        public override float AddOnNodeDrawWidth(float sDocumentWidth)
        {
            return 250.0f;
            //return sDocumentWidth;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddOnNodeDrawHeight()
        {
            return 130.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddOnNodeDraw(Rect sRect)
        {
            
        }
        //-------------------------------------------------------------------------------------------------------------
        public override Color AddOnNodeColor()
        {
            return Color.gray;
        }
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
