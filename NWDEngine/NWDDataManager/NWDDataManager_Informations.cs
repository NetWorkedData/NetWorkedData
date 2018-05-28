//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

using SQLite4Unity3d;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public partial class NWDDataManager
    {
        //-------------------------------------------------------------------------------------------------------------
        private string InformationsString;
        //-------------------------------------------------------------------------------------------------------------
		public void InformationsUpdate ()
		{
            //			Debug.Log ("NWDDataManager Informations");
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            DateTime tDate = BTBDateHelper.ConvertFromTimestamp(tEnvironment.BuildTimestamp);
            string tInformations = "Environment : "+tEnvironment.Environment+"\n" +
                                   "BuildTimestamp : " + tEnvironment.BuildTimestamp + "\n" +
                                   "BuildTimestamp => " + tDate.ToString("yyyy/MM/dd HH:mm:ss") + "\n" +
                                   "BuildDate : " + tEnvironment.BuildDate + "\n" +
                                   "Account : "+tEnvironment.PlayerAccountReference+"\n" +
                                   "_______________\n";
			foreach (Type tType in mTypeLoadedList) 
			{
				var tMethodInfo = tType.GetMethod ("Informations", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null) {
					tInformations += " • "+tMethodInfo.Invoke (null, null);
				} 
				else 
				{
					tInformations += tType.Name + " error \n";
				}
			}
			InformationsString = tInformations;
        }
        //-------------------------------------------------------------------------------------------------------------
		public string Informations ()
		{
			return InformationsString;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================