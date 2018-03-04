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
	public class NWDMessageConnection : NWDConnection <NWDMessage> {}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("MES")]
	[NWDClassDescriptionAttribute ("Message descriptions Class")]
	[NWDClassMenuNameAttribute ("Messages")]
	//-------------------------------------------------------------------------------------------------------------
	[NWDInternalKeyNotEditableAttribute]
	//-------------------------------------------------------------------------------------------------------------
	//	[NWDTypeClassInPackageAttribute]
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public partial class NWDMessage : NWDBasis <NWDMessage>
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
		//public bool DiscoverItYourSelf { get; set; }
		[NWDGroupStartAttribute ("Informations", true, true, true)] //ok
		public string Domain { get; set; }
		public string Code { get; set; }
		[NWDGroupEndAttribute]

		[NWDGroupSeparatorAttribute]
           
		[NWDGroupStartAttribute ("Description", true, true, true)] // ok
		public NWDLocalizableStringType Title { get; set; } // TODO : rename by Title ?
        public NWDLocalizableStringType Message { get; set; } // TODO : rename by Description ?
        public NWDLocalizableStringType Ok  { get; set;  }
        //public NWDLocalizableStringType Cancel { get; set; }
		//[NWDGroupEndAttribute]
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
        #region Constructors
		//-------------------------------------------------------------------------------------------------------------
		public NWDMessage ()
        {
            //Debug.Log("NWDMessage Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDMessage(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
           //Debug.Log("NWDMessage Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Class methods
		//-------------------------------------------------------------------------------------------------------------
		public static NWDMessage GetMessageWithCode (string sCode)
		{
			NWDMessage rReturn = null;
			foreach (NWDMessage tObject in NWDMessage.ObjectsList) {
				if (tObject.Code == sCode) {
					rReturn = tObject;
					break;
				}
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static NWDMessage GetMessageWithDomainAndCode (string sDomain , string sCode)
		{
			NWDMessage rReturn = null;
			foreach (NWDMessage tObject in NWDMessage.ObjectsList) {
				if (tObject.Code == sCode && tObject.Domain == sDomain) {
					rReturn = tObject;
					break;
				}
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public static NWDMessage CreateGenericMessage (string sDomain, string sCode, string sTitle, string sDescription)
		{
			string tReference = "MES-"+sDomain + "-" + sCode;
			// TODO: alert if reference is too long for ereg / or substring if too long
			NWDMessage tError = InstanceByReference (tReference) as NWDMessage;
			if (tError == null) {
				tError = NWDBasis<NWDMessage>.NewObject ();
				RemoveObjectInListOfEdition (tError);
				tError.Reference = tReference;
				//				tError.InternalKey = Domain + " : " + sCode;
				tError.InternalDescription = sDescription;
				// domain code
				tError.Domain = sDomain;
				tError.Code = sCode;
				// title
				NWDLocalizableStringType tTitle = new NWDLocalizableStringType ();
				tTitle.Value = "BASE:" + sTitle;
				tError.Title = tTitle;
				// description
				NWDLocalizableStringType tDescription = new NWDLocalizableStringType ();
				tDescription.Value = "BASE:" + sDescription;
				tError.Message = tDescription;
				// add-on edited
				tError.AddonEdited (true);
				// reccord
				tError.UpdateMe ();
				AddObjectInListOfEdition (tError);
			}
			return tError;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
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
		public override bool AddonEdited( bool sNeedBeUpdate)
		{
			if (sNeedBeUpdate == true) {
				if (Domain == null || Domain == "") {
					Domain = "Unknow";
				}
				InternalKey = Domain + " : "+ Code;
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