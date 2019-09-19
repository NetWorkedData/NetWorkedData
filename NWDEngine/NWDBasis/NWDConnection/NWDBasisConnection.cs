//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:25:40
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
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
    /// NWDBasisConnection is an abstract class to refere to an NWDBasis generic class object by its reference.
    /// </summary>
	[Serializable]
	public abstract class NWDBasisConnection
	{
		//-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The reference of refered object.
        /// </summary>
		[SerializeField]
		public string Reference = string.Empty;
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================