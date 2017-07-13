using System;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("ERR")]
	[NWDClassDescriptionAttribute ("Error descriptions Class")]
	[NWDClassMenuNameAttribute ("Errors")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDError : NWDBasis <NWDError>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		public string Domain { get; set; }
		public string Code { get; set; }
		public string LocalizedTitle { get; set; }
		public string LocalizedDescription { get; set; }
		public bool IsFatal { get; set; }
		public bool ShowAlert { get; set; }
		public string Type { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDError ()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public override bool AddonEdited (bool sNeedBeUpdate)
		{
			if (sNeedBeUpdate == true) {
				this.InternalKey = this.Code.ToString ();
			}
			return sNeedBeUpdate;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static NWDError GetObjectWithCode (string sCode)
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
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================