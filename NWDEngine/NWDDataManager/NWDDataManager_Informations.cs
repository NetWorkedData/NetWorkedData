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

//=====================================================================================================================
namespace NetWorkedData
{
	public partial class NWDDataManager
	{
		private string InformationsString;
		public void InformationsUpdate ()
		{
			//			Debug.Log ("NWDDataManager Informations");
			NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment ();
			string tInformations = "Environment : "+tEnvironment.Environment+" account "+tEnvironment.PlayerAccountReference+"\n_______________\n";
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

		public string Informations ()
		{
			return InformationsString;
		}
	}
}
//=====================================================================================================================