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
using System.IO;

using BasicToolBox;

using SQLite4Unity3d;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	[Serializable]
	public class NWDConnexion 
	{
		//-------------------------------------------------------------------------------------------------------------
		public string Reference;
		public Type PrivateType;
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================