//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
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
		public void DrawInEditor (EditorWindow sEditorWindow, bool sAutoSelect=false)
		{
			// TODO use NWDConstants for these strings
			// TODO use GUI without layout
			EditorGUILayout.BeginVertical (GUILayout.Width(300.0f));
			EditorGUILayout.HelpBox ("Project configuration " + Environment + " for connection with server", MessageType.None);
			EditorGUILayout.TextField ("AppName for server action "+ Environment, EditorStyles.boldLabel);
			AppName = EditorGUILayout.TextField ("AppName", AppName);
			EditorGUILayout.TextField ("Security of Datas"+ Environment, EditorStyles.boldLabel);
			DataSHAPassword = EditorGUILayout.TextField ("SHA Password", DataSHAPassword);
			DataSHAVector = EditorGUILayout.TextField ("SHA Vector", DataSHAVector);
			EditorGUILayout.TextField ("Hash of Datas"+ Environment, EditorStyles.boldLabel);
			SaltStart = EditorGUILayout.TextField ("Salt start", SaltStart);
            SaltEnd = EditorGUILayout.TextField ("Salt end", SaltEnd);
            SaltServer = EditorGUILayout.TextField("Salt server", SaltServer);
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
			//RescueEmail = EditorGUILayout.TextField ("RescueEmail", RescueEmail);
			EditorGUILayout.TextField ("Admin Key for "+ Environment, EditorStyles.boldLabel);
			AdminKey = EditorGUILayout.TextField ("AdminKey", AdminKey);
			EditorGUILayout.TextField ("Token Historic limit for "+ Environment, EditorStyles.boldLabel);
			TokenHistoric = EditorGUILayout.IntSlider ("Token number", TokenHistoric, 1, 10);
			EditorGUILayout.TextField ("Version for "+ Environment, EditorStyles.boldLabel);
			EditorGUILayout.LabelField ("version", NWDVersion.GetVersionForEnvironemt (this), EditorStyles.boldLabel);
			EditorGUILayout.EndVertical();
			FormatVerification ();
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================