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
using SQLite.Attribute;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[NWDClassServerSynchronizeAttribute (false)]
	[NWDClassTrigrammeAttribute ("RQT")]
	[NWDClassDescriptionAttribute ("RequestToken descriptions Class")]
	[NWDClassMenuNameAttribute ("RequestToken")]
	public partial class NWDRequestToken : NWDBasis <NWDRequestToken>
	{
		//-------------------------------------------------------------------------------------------------------------
		[Indexed ("AccountIndex", 0)]
		public NWDReferenceHashType<NWDAccount> UUIDHash { get; set; } // TODO: A virer
		public NWDReferenceHashType<NWDAccount> AccountReferenceHash { get; set; }
		public string Token { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDRequestToken()
        {
            //Debug.Log("NWDRequestToken Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDRequestToken(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDRequestToken Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
