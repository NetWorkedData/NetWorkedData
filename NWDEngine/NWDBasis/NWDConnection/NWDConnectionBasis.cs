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
using System.IO;
using System.Reflection;

using SQLite4Unity3d;

using BasicToolBox;

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDConnectionBasis is an abstract class to refere to an NWDBasis generic class object by its reference.
    /// </summary>
	[Serializable]
	public abstract class NWDConnectionBasis
	{
		//-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The reference of refered object.
        /// </summary>
		[SerializeField]
		public string Reference;
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================