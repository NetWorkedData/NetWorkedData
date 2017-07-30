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

using SQLite4Unity3d;

using BasicToolBox;

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
	public class NWDConnexionAttribut : PropertyAttribute
	{
		//-------------------------------------------------------------------------------------------------------------
		public bool ShowInspector = false;
		public bool Editable = false;
		public bool EditButton = true;
		public bool NewButton = true;
		//-------------------------------------------------------------------------------------------------------------
		public NWDConnexionAttribut ()
		{
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDConnexionAttribut (bool sShowInspector, bool sEditable = false, bool sEditButton = true, bool sNewButton = true)
		{
			ShowInspector = sShowInspector;
			Editable = sEditable;
			EditButton = sEditButton;
			NewButton = sNewButton;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================