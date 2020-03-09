//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:25:38
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
using System.Reflection;
using System.IO;

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDConnectionAttribut is used to custom inspector in the editor.
    /// </summary>
	public class NWDConnectionAttribut : PropertyAttribute
	{
		//-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Show in inspector.
        /// </summary>
		public bool ShowInspector = false;
		//-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="T:NetWorkedData.NWDConnectionAttribut"/> class.
        /// </summary>
		public NWDConnectionAttribut ()
		{
		}
		//-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="T:NetWorkedData.NWDConnectionAttribut"/> class.
        /// </summary>
        /// <param name="sShowInspector">If set to <c>true</c> show in inspector.</param>
		public NWDConnectionAttribut (bool sShowInspector)
		{
			ShowInspector = sShowInspector;
		}
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================