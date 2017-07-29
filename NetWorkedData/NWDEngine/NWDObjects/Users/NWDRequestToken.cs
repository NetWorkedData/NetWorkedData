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

#if UNITY_EDITOR
using UnityEditor;
#endif

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (false)]
	[NWDClassTrigrammeAttribute ("RQT")]
	[NWDClassDescriptionAttribute ("RequestToken descriptions Class")]
	[NWDClassMenuNameAttribute ("RequestToken")]
	//-------------------------------------------------------------------------------------------------------------
	[NWDPackageClassAttribute]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDRequestToken : NWDBasis <NWDRequestToken>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		[Indexed ("AccountIndex", 0)]
		public NWDReferenceHashType<NWDAccount> UUIDHash { get; set; } // TODO: A virer
		public NWDReferenceHashType<NWDAccount> AccountReferenceHash { get; set; }
		public string Token { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDRequestToken()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================