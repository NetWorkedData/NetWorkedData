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

using SQLite4Unity3d;

using BasicToolBox;

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
        /// <summary>
        /// Editable/selectionale in inspector of editor.
        /// </summary>
		public bool Editable = false;
        /// <summary>
        /// Show the 'edit' button in inspector of editor.
        /// </summary>
		public bool EditButton = true;
        /// <summary>
        /// Show the 'new' button in inspector of editor.
        /// </summary>
		public bool NewButton = true;
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
        /// <param name="sEditable">If set to <c>true</c> is editable.</param>
        /// <param name="sEditButton">If set to <c>true</c> show 'edit' button.</param>
        /// <param name="sNewButton">If set to <c>true</c> show 'new' button.</param>
		public NWDConnectionAttribut (bool sShowInspector, bool sEditable = false, bool sEditButton = true, bool sNewButton = true)
		{
			ShowInspector = sShowInspector;
			Editable = sEditable;
			EditButton = sEditButton;
			NewButton = sNewButton;
		}
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================