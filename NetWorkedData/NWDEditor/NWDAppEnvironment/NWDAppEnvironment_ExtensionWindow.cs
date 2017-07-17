﻿/// <summary>
/// NWD app environment.
/// </summary>
/// 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	public partial class NWDAppEnvironment
	{
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Name for the menu.
		/// </summary>
		/// <returns>The name.</returns>
		public static string MenuName ()
		{
			return NWDConstants.K_APP_ENVIRONMENT_MENU_NAME;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Draw the interface in editor.
		/// </summary>
		public void DrawInEditor ()
		{
			// TODO use NWDConstants for these strings
			// TODO GUI without layout
			EditorGUILayout.BeginVertical (GUILayout.Width(300.0f));
			EditorGUILayout.HelpBox ("Project configuration " + Environment + " for connexion with server", MessageType.None);	
//			EditorGUILayout.TextField ("Environment folder name for "+ Environment, EditorStyles.boldLabel);
//			Environment = EditorGUILayout.TextField ("Environment", Environment);
//			EditorGUILayout.TextField ("Versions of general compilation for "+ Environment, EditorStyles.boldLabel);
//			Version = EditorGUILayout.TextField ("Global Version X.X.X", Version);
//			EditorGUILayout.TextField ("Versions of app compilation"+ Environment, EditorStyles.boldLabel);
//			VersionRequiredIOS = EditorGUILayout.TextField ("Version specific for IOS X.X.X", VersionRequiredIOS);
//			VersionRequiredMacOSX = EditorGUILayout.TextField ("Version specific for MacOSX X.X.X", VersionRequiredMacOSX);
//			VersionRequiredAndroid = EditorGUILayout.TextField ("Version specific for Android X.X.X", VersionRequiredAndroid);
//			VersionRequiredWindows = EditorGUILayout.TextField ("Version specific for Windows X.X.X", VersionRequiredWindows);
			//			VersionRequiredWeb = EditorGUILayout.TextField ("Version specific for Web X.X.X", VersionRequiredWeb);
			EditorGUILayout.TextField ("AppName for server action "+ Environment, EditorStyles.boldLabel);
			AppName = EditorGUILayout.TextField ("AppName", AppName);
			EditorGUILayout.TextField ("Security of Datas"+ Environment, EditorStyles.boldLabel);
			DataSHAPassword = EditorGUILayout.TextField ("SHA Password", DataSHAPassword);
			DataSHAVector = EditorGUILayout.TextField ("SHA Vector", DataSHAVector);
			EditorGUILayout.TextField ("Hash of Datas"+ Environment, EditorStyles.boldLabel);
			SaltStart = EditorGUILayout.TextField ("Salt A", SaltStart);
			SaltEnd = EditorGUILayout.TextField ("Salt B", SaltEnd);
			SaltFrequency = EditorGUILayout.IntField ("Salt Frequency", SaltFrequency);
			EditorGUILayout.TextField ("Server Params for "+ Environment, EditorStyles.boldLabel);
			ServerHTTPS = EditorGUILayout.TextField ("Server (https://…)", ServerHTTPS);
			ServerHost = EditorGUILayout.TextField ("MySQL Host", ServerHost);
			ServerUser = EditorGUILayout.TextField ("MySQL user", ServerUser);
			ServerPassword = EditorGUILayout.TextField ("MySQL password", ServerPassword);
			ServerBase = EditorGUILayout.TextField ("MySQL base", ServerBase);
			EditorGUILayout.TextField ("Social Params for "+ Environment, EditorStyles.boldLabel);
			FacebookAppID = EditorGUILayout.TextField ("FacebookAppID", FacebookAppID);
			FacebookAppSecret = EditorGUILayout.TextField ("FacebookAppSecret", FacebookAppSecret);
			GoogleAppKey = EditorGUILayout.TextField ("GoogleAppKey", GoogleAppKey);
			UnityAppKey = EditorGUILayout.TextField ("UnityAppKey", UnityAppKey);
			TwitterAppKey = EditorGUILayout.TextField ("TwitterAppKey", TwitterAppKey);
			EditorGUILayout.TextField ("Email to send forgotten code "+ Environment, EditorStyles.boldLabel);
			RescueEmail = EditorGUILayout.TextField ("RescueEmail", RescueEmail);
			EditorGUILayout.TextField ("Admin Key for "+ Environment, EditorStyles.boldLabel);
			AdminKey = EditorGUILayout.TextField ("AdminKey", AdminKey);
			EditorGUILayout.TextField ("Version for "+ Environment, EditorStyles.boldLabel);
			EditorGUILayout.LabelField ("version", NWDGameVersion.GetVersionForEnvironemt (this), EditorStyles.boldLabel);
			EditorGUILayout.EndVertical();
			FormatVerification ();
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================