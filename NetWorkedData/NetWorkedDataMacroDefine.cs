using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
	[InitializeOnLoad]
	/// <summary>
	/// Net worked data macro define.
	/// </summary>
	public class NetWorkedDataMacroDefine
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The macro to check this package.
		/// </summary>
		const string kMacro = "NET_WORKED_DATA";
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the <see cref="NetWorkedData.NetWorkedDataMacroDefine"/> class.
		/// </summary>
		static NetWorkedDataMacroDefine ()
		{
			//start as platform was changed
			OnChangedPlatform ();
			// add method on build target change
			EditorUserBuildSettings.activeBuildTargetChanged += OnChangedPlatform;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the changed platform event.
		/// </summary>
		static void OnChangedPlatform ()
		{
			InstallMacro (EditorUserBuildSettings.selectedBuildTargetGroup);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Installs the macro.
		/// </summary>
		/// <param name="sBuildTarget">S build target.</param>
		static void InstallMacro (BuildTargetGroup sBuildTarget)
		{
			if (PlayerSettings.GetScriptingDefineSymbolsForGroup (sBuildTarget).Contains (kMacro) == false) {
				Debug.Log ("Install macro " + kMacro + " in " + sBuildTarget + " player settings");
				PlayerSettings.SetScriptingDefineSymbolsForGroup (sBuildTarget, PlayerSettings.GetScriptingDefineSymbolsForGroup (sBuildTarget) + ";" + kMacro);
				if (PlayerSettings.GetScriptingDefineSymbolsForGroup (sBuildTarget).Contains (kMacro) == false) {
					Debug.LogError ("Fail to install macro " + kMacro + " in " + sBuildTarget + " player settings!");
				}
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Installs the macro in all build target.
		/// </summary>
		static void InstallMacroAll ()
		{
			Array BuildTargetGroupsArray = Enum.GetValues (typeof(BuildTargetGroup));
			foreach (BuildTargetGroup tBuildTarget in BuildTargetGroupsArray) {
				if (tBuildTarget != BuildTargetGroup.Unknown) {
					InstallMacro (tBuildTarget);
				}
			}
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif
