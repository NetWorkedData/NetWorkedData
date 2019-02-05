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
    public class NWDInAppPackConnection : NWDConnection <NWDInAppPack> {}
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("IAP")]
	[NWDClassDescriptionAttribute ("In App Purchase descriptions Class")]
	[NWDClassMenuNameAttribute ("In App Purchase")]
	public partial class NWDInAppPack :NWDBasis <NWDInAppPack>
	{
		//-------------------------------------------------------------------------------------------------------------
        [NWDGroupStartAttribute("Specific Store ID", true, true, true)]
        public string AppleID { get; set; }
		public string GoogleID { get; set; }
		public string UnityID { get; set; }
		public string SteamID { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDInAppPack()
        {
            //Debug.Log("NWDInAppPack Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDInAppPack(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDInAppPack Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
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
        public string GetIAPKey ()
		{
            if (Application.platform == RuntimePlatform.Android)
            {
                return GoogleID;
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return AppleID;
            }

            return string.Empty;
		}
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================