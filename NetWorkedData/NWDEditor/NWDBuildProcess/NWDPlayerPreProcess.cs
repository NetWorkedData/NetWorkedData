using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using BasicToolBox;
using UnityEditor;
using UnityEditor.Build;
//=====================================================================================================================
namespace NetWorkedData
{
	[InitializeOnLoad]
	public class NWDPlayerPreProcess : EditorWindow {
		//-------------------------------------------------------------------------------------------------------------
		static NWDPlayerPreProcess()
		{
			EditorApplication.playmodeStateChanged -= PlayModeCallback; // remove old callback from compile for this class
			EditorApplication.playmodeStateChanged += PlayModeCallback;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void PlayModeCallback () {
			BTBDebug.Log("Player State Changed!");
			NWDVersion.UpdateVersionBundle ();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif