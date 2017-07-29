//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("ERR")]
	[NWDClassDescriptionAttribute ("Error descriptions Class")]
	[NWDClassMenuNameAttribute ("Errors")]
	[NWDInternalKeyNotEditableAttribute]
	//-------------------------------------------------------------------------------------------------------------
	[NWDPackageClassAttribute]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDError : NWDBasis <NWDError>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		public string Domain { get; set; }

		public string Code { get; set; }

		public NWDLocalizableStringType LocalizedTitle { get; set; }

		public NWDLocalizableStringType LocalizedDescription { get; set; }

		[NWDEnumString (new string[] { "alert", "critical", "verbose" })]
		public string Type { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDError ()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
		// My Method add-on
		//-------------------------------------------------------------------------------------------------------------
		public static NWDError GetErrorWithCode (string sCode)
		{
			NWDError rReturn = null;
			foreach (NWDError tObject in NWDError.ObjectsList) {
				if (tObject.Code == sCode) {
					rReturn = tObject;
					break;
				}
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static NWDError GetErrorWithDomainAndCode (string sDomain , string sCode)
		{
			NWDError rReturn = null;
			foreach (NWDError tObject in NWDError.ObjectsList) {
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
		public override bool AddonEdited (bool sNeedBeUpdate)
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
		public static NWDError CreateGenericError (string sDomain, string sCode, string sTitle, string sDescription, string sType = "verbose")
		{
			string tReference = "ERR-"+sDomain + "-" + sCode;
			// TODO: alert if reference is too long for ereg / or substring if too long
			NWDError tError = GetObjectByReference (tReference) as NWDError;
			if (tError == null) {
				tError = NWDBasis<NWDError>.NewInstance () as NWDError;
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
				tError.LocalizedTitle = tTitle;
				// description
				NWDLocalizableStringType tDescription = new NWDLocalizableStringType ();
				tDescription.Value = "BASE:" + sDescription;
				tError.LocalizedDescription = tDescription;
				// type of alert
				tError.Type = sType;
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
	}
}
//=====================================================================================================================