// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:25:36
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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
		public K GetObject () // TODO rename GetData
		{
            return NWDBasis <K>.RawDataByReference (Reference);
		}
		//-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Set the object instance by its reference.
        /// </summary>
        /// <param name="sObject">S object.</param>
		public void SetObject (K sObject) // TODO rename SetData
        {
			if (sObject != null) {
				Reference = sObject.Reference;
			} else {
				Reference = string.Empty;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Object instance creation and reference it automatically.
        /// </summary>
        /// <returns>The object.</returns>
        public K NewObject (NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal) // TODO rename NewData
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