using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using SQLite4Unity3d;

using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
	public partial class NWDDataManager
	{
		//-------------------------------------------------------------------------------------------------------------
		public void SavePreferences (NWDAppEnvironment sEnvironment)
		{
			sEnvironment.SavePreferences ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public void LoadPreferences (NWDAppEnvironment sEnvironment)
		{
			sEnvironment.LoadPreferences ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public void ResetPreferences (NWDAppEnvironment sEnvironment)
		{
			sEnvironment.ResetPreferences ();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================