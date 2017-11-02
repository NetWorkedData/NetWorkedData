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
	public partial  class NWDBasis <K> where K : NWDBasis <K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
		#region Class methods
		//-------------------------------------------------------------------------------------------------------------
		public static void ClassInfos (string sString)
		{
			Debug.Log ("From " + sString + " real [" + typeof(K).Name + "] = > " + NWDTypeInfos.GetInfos (typeof(K)) + "' ");
		}
		//-------------------------------------------------------------------------------------------------------------
		public static string Informations ()
		{
			int tCount = ObjectsList.Count;
			if (tCount == 0) {
				return "" + ClassNamePHP () + " " + NWDConstants.K_APP_BASIS_NO_OBJECT + "\n";
			} else if (tCount == 1) {
				return "" + ClassNamePHP () + " : " + tCount + " " + NWDConstants.K_APP_BASIS_ONE_OBJECT + "\n";
			} else {
				return "" + ClassNamePHP () + " : " + tCount + " " + NWDConstants.K_APP_BASIS_X_OBJECTS + "\n";
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================