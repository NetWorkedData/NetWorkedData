using System;

using UnityEngine;

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	public partial  class NWDBasis <K> where K : NWDBasis <K>, new()
	{
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
	}
}
//=====================================================================================================================