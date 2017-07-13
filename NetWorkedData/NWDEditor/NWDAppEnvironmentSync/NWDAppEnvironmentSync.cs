using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
	/// <summary>
	/// NWD app configuration window.
	/// </summary>
	public class NWDAppEnvironmentSync : EditorWindow
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the OnGUI event.
		/// </summary>
		public void OnGUI ()
		{
			this.minSize = new Vector2 (300, 72);
			this.maxSize = new Vector2 (300, 72);
			// set title of window
			titleContent = new GUIContent (NWDConstants.K_APP_SYNC_ENVIRONMENT_TITLE);
			// show helpbox
			EditorGUILayout.HelpBox (NWDConstants.K_APP_SYNC_ENVIRONMENT, MessageType.None);

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Dev Sync", EditorStyles.miniButton)) {
				NWDDataManager.SharedInstance.AddWebRequestAllSynchronization (false, NWDAppConfiguration.SharedInstance.DevEnvironment);
			}
			if (GUILayout.Button ("Preprod Sync", EditorStyles.miniButton)) {
				NWDDataManager.SharedInstance.AddWebRequestAllSynchronization (false, NWDAppConfiguration.SharedInstance.PreprodEnvironment);
			}
			if (GUILayout.Button ("Prod Sync", EditorStyles.miniButton)) {
				NWDDataManager.SharedInstance.AddWebRequestAllSynchronization (false, NWDAppConfiguration.SharedInstance.ProdEnvironment);
			}
			GUILayout.EndHorizontal ();

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button ("Dev Sync Force", EditorStyles.miniButton)) {
				NWDDataManager.SharedInstance.AddWebRequestAllSynchronizationForce (false, NWDAppConfiguration.SharedInstance.DevEnvironment);
			}
			if (GUILayout.Button ("Preprod Sync Force", EditorStyles.miniButton)) {
				NWDDataManager.SharedInstance.AddWebRequestAllSynchronizationForce (false, NWDAppConfiguration.SharedInstance.PreprodEnvironment);
			}
			if (GUILayout.Button ("Prod Sync Force", EditorStyles.miniButton)) {
				NWDDataManager.SharedInstance.AddWebRequestAllSynchronizationForce (false, NWDAppConfiguration.SharedInstance.ProdEnvironment);
			}
			GUILayout.EndHorizontal ();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif
