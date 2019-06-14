// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:30
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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
		[NWDInspectorGroupStart("Informations",true, true, true)] // ok
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