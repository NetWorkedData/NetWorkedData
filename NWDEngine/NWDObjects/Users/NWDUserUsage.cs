//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using SQLite.Attribute;

//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("UUG")]
	[NWDClassDescriptionAttribute ("Usage descriptions Class")]
	[NWDClassMenuNameAttribute ("Usage")]
	public partial class NWDUserUsage :NWDBasis <NWDUserUsage>
	{
		//-------------------------------------------------------------------------------------------------------------
		[NWDInspectorGroupStartAttribute("Informations",true, true, true)] // ok
		[Indexed ("AccountIndex", 0)]
        public NWDReferenceType<NWDAccount> Account { get; set; }
        public NWDReferenceType<NWDGameSave> GameSave
        {
            get; set;
        }
		public NWDReferencesQuantityType<NWDItem> ItemQuantitySpent { get; set; }
		public NWDReferencesQuantityType<NWDItem> ItemQuantityGot { get; set; }
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================