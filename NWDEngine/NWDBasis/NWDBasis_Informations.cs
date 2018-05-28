//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;

using UnityEngine;

using SQLite4Unity3d;

using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
		#region Class methods
		//-------------------------------------------------------------------------------------------------------------
		public static void ClassInformations (string sString)
        {
			Debug.Log ("From " + sString + " real [" + typeof(K).Name + "] = > " + NWDTypeInfos.Informations (typeof(K)) + "' ");
		}
		//-------------------------------------------------------------------------------------------------------------
		public static string Informations ()
		{
			int tCount = ObjectsList.Count;
			if (tCount == 0) {
                return "" + ClassNamePHP () + " " + NWDConstants.K_APP_BASIS_NO_OBJECT + " (sync at " + SynchronizationGetLastTimestamp(NWDAppEnvironment.SelectedEnvironment())+  ")\n";
			} else if (tCount == 1) {
                return "" + ClassNamePHP () + " : " + tCount + " " + NWDConstants.K_APP_BASIS_ONE_OBJECT + " (sync at " + SynchronizationGetLastTimestamp(NWDAppEnvironment.SelectedEnvironment()) + ")\n";
			} else {
                return "" + ClassNamePHP () + " : " + tCount + " " + NWDConstants.K_APP_BASIS_X_OBJECTS + " (sync at " + SynchronizationGetLastTimestamp(NWDAppEnvironment.SelectedEnvironment()) + ")\n";
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================