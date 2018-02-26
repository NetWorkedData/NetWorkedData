//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

using SQLite4Unity3d;

#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.Build;

//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public class NWDBuildPostProcess : IPostprocessBuild {
		//-------------------------------------------------------------------------------------------------------------
		public int callbackOrder { get { return 0; } }
		//-------------------------------------------------------------------------------------------------------------
		public void OnPostprocessBuild(BuildTarget target, string path)
		{
			Debug.Log ("NWDBuildPostProcess OnPostprocessBuild for target " + target + " at path " + path);
			BuildTarget tBuildTarget = EditorUserBuildSettings.activeBuildTarget;
			switch (tBuildTarget) {
                case BuildTarget.StandaloneOSX: 
				{
				}
				break;
			case BuildTarget.StandaloneWindows: 
				{
				}
				break;
			case BuildTarget.iOS: 
				{
				}
				break;
			case BuildTarget.Android: 
				{
                }
				break;
			case BuildTarget.StandaloneLinux: 
				{
				}
				break;
			case BuildTarget.StandaloneWindows64: 
				{
				}
				break;
			case BuildTarget.WebGL: 
				{
				}
				break;
			case BuildTarget.WSAPlayer: 
				{
				}
				break;
			case BuildTarget.StandaloneLinux64: 
				{
				}
				break;
			case BuildTarget.StandaloneLinuxUniversal: 
				{
				}
				break;
			case BuildTarget.Tizen: 
				{
				}
				break;
			case BuildTarget.PSP2: 
				{
				}
				break;
			case BuildTarget.PS4: 
				{
				}
				break;
			case BuildTarget.XboxOne: 
				{
				}
				break;
			case BuildTarget.N3DS: 
				{
				}
				break;
			case BuildTarget.WiiU: 
				{
				}
				break;
			case BuildTarget.tvOS: 
				{
				}
				break;
			case BuildTarget.Switch: 
				{
				}
				break;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif