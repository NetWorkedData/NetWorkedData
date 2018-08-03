//=====================================================================================================================
//
// ideMobi copyright 2018  
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
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDConnection is generic class to create connection to NWDBasis generic class by object's reference.
    /// </summary>
	[Serializable]
	public class NWDConnection <K> : NWDConnectionBasis where K : NWDBasis <K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the object instance referenced.
        /// </summary>
        /// <returns>The object.</returns>
		public K GetObject ()
		{
            return NWDBasis <K>.NEW_GetDataAccountByReference (Reference);
		}
		//-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Set the object instance by its reference.
        /// </summary>
        /// <param name="sObject">S object.</param>
		public void SetObject (K sObject)
		{
			if (sObject != null) {
				Reference = sObject.Reference;
			} else {
				Reference = "";
			}
		}
		//-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Object instance creation and reference it automatically.
        /// </summary>
        /// <returns>The object.</returns>
        public K NewObject (NWDWritingMode sWritingMode = NWDWritingMode.MainThread)
		{
            K tObject = NWDBasis <K>.NewData (sWritingMode);
			Reference = tObject.Reference;
			return tObject;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================