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

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

using SQLite4Unity3d;
using System.IO;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDSelector {
		public static string NWDBasis_DRAW_IN_EDITOR_SELECTOR = "DrawInEditor";
	}
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDBasis <K> where K : NWDBasis <K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		// WARNING DECLARE SELECTOR FOR INVOKE IN NWDBasis_DRAW_IN_EDITOR_SELECTOR
		//-------------------------------------------------------------------------------------------------------------
		public static void DrawInEditor (EditorWindow sEditorWindow, bool sAutoSelect=false)
		{
			DrawTableEditor (sEditorWindow);
			if (sAutoSelect == true) {
				SelectedFirstObjectInTable (sEditorWindow);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================