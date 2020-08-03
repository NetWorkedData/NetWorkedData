//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-09-9 18:24:43
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	MacroDefineEditor for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using UnityEditor.Build;

//=====================================================================================================================
namespace MacroDefineEditor
{
	[InitializeOnLoad]
	/// <summary>
	/// Macro define can find if kMacro is set in the settings and add it if necessary.
	/// This class auto run at build project.
	/// You can use kMacro in precompile definition.
	/// </summary>
	public class MDEMacroDefine : IActiveBuildTargetChanged
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The macro to check in this project. It's tag in settings. The you can use #if define xxxxx #endif in your 
		/// code.
		/// </summary>
		public const string kMacro = "MACRO_DEFINE_EDITOR";
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The shared instance used for this class.
		/// </summary>
		static MDEMacroDefine kSharedInstance;
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes the MDEMacroDefine class. 
		/// Instance kSharedInstance to use it when method OnActiveBuildTargetChanged must be invoked
		/// </summary>
		static MDEMacroDefine ()
		{
			if (kSharedInstance == null) {
				kSharedInstance = new MDEMacroDefine ();
				kSharedInstance.OnChangedPlatform ();
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the callback order for IActiveBuildTargetChanged
		/// </summary>
		/// <value>The callback order.</value>
		public int callbackOrder { get { return 0; } }
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the active build target changed event.
		/// </summary>
		/// <param name="previousTarget">Previous target.</param>
		/// <param name="newTarget">New target.</param>
		public void OnActiveBuildTargetChanged (BuildTarget previousTarget, BuildTarget newTarget)
		{
			OnChangedPlatform ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the changed platform event.
		/// </summary>
		public void OnChangedPlatform ()
		{
			InstallMacro (EditorUserBuildSettings.selectedBuildTargetGroup);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Installs the macro.
		/// </summary>
		/// <param name="sBuildTarget">S build target.</param>
		public void InstallMacro (BuildTargetGroup sBuildTarget)
		{
			if (PlayerSettings.GetScriptingDefineSymbolsForGroup (sBuildTarget).Contains (kMacro) == false) {
				//Debug.Log ("Install macro " + kMacro + " in " + sBuildTarget + " player settings");
				PlayerSettings.SetScriptingDefineSymbolsForGroup (sBuildTarget, PlayerSettings.GetScriptingDefineSymbolsForGroup (sBuildTarget) + ";" + kMacro);
				if (PlayerSettings.GetScriptingDefineSymbolsForGroup (sBuildTarget).Contains (kMacro) == false) {
					//Debug.LogError ("Fail to install macro " + kMacro + " in " + sBuildTarget + " player settings!");
				}
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Installs the macro in all build target.
		/// </summary>
		public void InstallMacroAll ()
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
