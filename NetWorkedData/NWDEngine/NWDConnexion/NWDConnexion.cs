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
	public class NWDConnexion <W> : NWDConnexionBasis where W : NWDBasis <W>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
		public W GetObject ()
		{
			return NWDBasis <W>.GetObjectByReference (Reference);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetObject (W sObject)
		{
			Reference = sObject.Reference;
		}
		//-------------------------------------------------------------------------------------------------------------
		public W NewObject ()
		{
			W tObject = NWDBasis <W>.NewObject ();
			Reference = tObject.Reference;
			return tObject;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================