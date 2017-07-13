using System;

using UnityEngine;

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("WRD")]
	[NWDClassDescriptionAttribute ("Worlds descriptions Class")]
	[NWDClassMenuNameAttribute ("Worlds")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDWorld :NWDBasis<NWDWorld>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		public NWDLocalizableStringType Name{ get; set; }

		public NWDLocalizableStringType SubName { get; set; }

		public NWDLocalizableStringType Description { get; set; }

		[NWDGroupStartAttribute("Colors of world",true,true,true)]
		public NWDColorType PrimaryColor { get; set; }

		public NWDColorType SecondaryColor { get; set; }
	
		public NWDColorType TertiaryColor { get; set; }
		[NWDGroupEndAttribute]

		[NWDGroupStartAttribute("Type of world",true,true,true)]
		public string Kind { get; set; }
		public NWDReferencesListType<NWDCategory> Categories { get; set; }
		public NWDReferencesListType<NWDFamily> Families { get; set; }
		public NWDReferencesListType<NWDKeyword>  Keywords { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDWorld()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================