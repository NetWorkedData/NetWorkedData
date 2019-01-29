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
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using SQLite4Unity3d;
using System.IO;
//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_DrawTypeInInspector)]
        public static void DrawTypeInInspector ()
		{
            if (Datas().TestSaltValid() == false)
			{
				EditorGUILayout.HelpBox (NWDConstants.kAlertSaltShortError, MessageType.Error);
			}

//			if (GUILayout.Button (NWDConstants.K_APP_BASIS_CLASS_SEE_WORKFLOW, EditorStyles.miniButton)) {
//				TableMapping tTableMapping = new TableMapping (ClassType ());
//				string tClassName = tTableMapping.TableName;
//				string tEngineRootFolder = "Assets/NetWorkedDataWorkflow";
//				Selection.activeObject=AssetDatabase.LoadMainAssetAtPath(tEngineRootFolder + "/" + tClassName + "-Workflow.cs");
//			}


			EditorGUILayout.LabelField (NWDConstants.K_APP_BASIS_CLASS_DESCRIPTION, EditorStyles.boldLabel);
//			EditorGUI.indentLevel++;
            EditorGUILayout.HelpBox (Datas().ClassDescription, MessageType.Info);
//			EditorGUI.indentLevel--;
			if (NWDAppConfiguration.SharedInstance().DevEnvironment.Selected == true) {
				EditorGUILayout.LabelField (NWDConstants.K_APP_BASIS_CLASS_DEV, EditorStyles.boldLabel);
			}
			if (NWDAppConfiguration.SharedInstance().PreprodEnvironment.Selected == true) {
				EditorGUILayout.LabelField (NWDConstants.K_APP_BASIS_CLASS_PREPROD, EditorStyles.boldLabel);
			}
			if (NWDAppConfiguration.SharedInstance().ProdEnvironment.Selected == true) {
				EditorGUILayout.LabelField (NWDConstants.K_APP_BASIS_CLASS_PROD, EditorStyles.boldLabel);
			}
//			EditorGUI.indentLevel++;
//			mForceSynchronization = GUILayout.Toggle (mForceSynchronization, NWDConstants.K_APP_BASIS_CLASS_SYNC_FORCE);
//			if (GUILayout.Button (NWDConstants.K_APP_BASIS_CLASS_SYNC + MenuName () + NWDConstants.K_APP_BASIS_CLASS_DATAS)) {
//
//				if (NWDAppConfiguration.SharedInstance().IsProdEnvironement () == true) {
//					if (EditorUtility.DisplayDialog (NWDConstants.K_SYNC_ALERT_TITLE,
//						NWDConstants.K_SYNC_ALERT_MESSAGE,
//						NWDConstants.K_SYNC_ALERT_OK,
//						NWDConstants.K_SYNC_ALERT_CANCEL)) {
//						SynchronizationFromWebService (mForceSynchronization, NWDAppConfiguration.SharedInstance().SelectedEnvironment());
//					}
//
//				} else {
//					SynchronizationFromWebService (mForceSynchronization, NWDAppConfiguration.SharedInstance().SelectedEnvironment());
//				}
//			}
//
//			NWDAppEnvironment sEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment ();
//			if (GUILayout.Button (NWDConstants.K_APP_BASIS_CLASS_SYNC_ALL_DATAS)) {
//
//				if (NWDAppConfiguration.SharedInstance().IsProdEnvironement () == true) {
//					if (EditorUtility.DisplayDialog (NWDConstants.K_SYNC_ALERT_TITLE,
//						NWDConstants.K_SYNC_ALERT_MESSAGE,
//						NWDConstants.K_SYNC_ALERT_OK,
//						NWDConstants.K_SYNC_ALERT_CANCEL)) {
//						NWDDataManager.SharedInstance().SynchronizeAllData (sEnvironment, mForceSynchronization);
//					}
//
//				} else {
//					NWDDataManager.SharedInstance().SynchronizeAllData (sEnvironment, mForceSynchronization);
//				}
//			}
//			EditorGUI.indentLevel--;
			DrawSettingsEditor ();
		}

		//-------------------------------------------------------------------------------------------------------------
		public static void DrawSettingsEditor ()
		{
			GUIStyle tStyle = EditorStyles.foldout;
			FontStyle tPreviousStyle = tStyle.fontStyle;
			tStyle.fontStyle = FontStyle.Bold;
            Datas().mSettingsShowing = EditorGUILayout.Foldout (Datas().mSettingsShowing, NWDConstants.K_APP_BASIS_CLASS_WARNING_ZONE, tStyle);
			tStyle.fontStyle = tPreviousStyle;

            if (Datas().mSettingsShowing == true) 
			{
				EditorGUILayout.HelpBox (NWDConstants.K_APP_BASIS_CLASS_WARNING_HELPBOX, MessageType.Warning);

				if (GUILayout.Button (NWDConstants.K_APP_BASIS_CLASS_RESET_TABLE, EditorStyles.miniButton)) {
					NWDBasis<K>.ResetTable ();
//					UpdateReferencesList ();
					//LoadTableEditor ();
                    //LoadFromDatabase();
                    //RepaintTableEditor();
				}
				// -------------------------------------------
				GUILayout.BeginHorizontal ();
                string tPrefSaltA = EditorGUILayout.TextField (NWDConstants.K_APP_BASIS_CLASS_FIRST_SALT, Datas().SaltA);
                Datas().SaltA = tPrefSaltA;
				if (GUILayout.Button (NWDConstants.K_APP_BASIS_CLASS_REGENERATE, EditorStyles.miniButton)) {
					GUI.FocusControl (null);
					tPrefSaltA = GenerateNewSalt ();
                    Datas().SaltA =tPrefSaltA;
                    Datas().PrefSave ();
					RecalculateAllIntegrities ();
				}
				GUILayout.EndHorizontal ();
				// -------------------------------------------
				GUILayout.BeginHorizontal ();
                string tPrefSaltB = EditorGUILayout.TextField (NWDConstants.K_APP_BASIS_CLASS_SECOND_SALT, Datas().SaltB);
                Datas().SaltB =tPrefSaltB;
				if (GUILayout.Button (NWDConstants.K_APP_BASIS_CLASS_REGENERATE, EditorStyles.miniButton)) {
					GUI.FocusControl (null);
					tPrefSaltB = GenerateNewSalt ();
                    Datas().SaltB =tPrefSaltB;
                    Datas().PrefSave ();
					RecalculateAllIntegrities ();
				}
				GUILayout.EndHorizontal ();

				if (GUILayout.Button (NWDConstants.K_APP_BASIS_CLASS_INTEGRITY_REEVALUE, EditorStyles.miniButton)) {
					GUI.FocusControl (null);
                    Datas().PrefSave ();
					RecalculateAllIntegrities ();
				}

                //GUILayout.BeginHorizontal();
                //if (GUILayout.Button("dev force push", EditorStyles.miniButton))
                //{
                //    EditorPrefs.SetInt(SynchronizationPrefsKey(NWDAppConfiguration.SharedInstance().DevEnvironment), 0);
                //    foreach (K tObject in ObjectsList)
                //    {
                //        if (tObject.IntegrityValue() == tObject.Integrity)
                //        {
                //            tObject.DevSync = 0;
                //            tObject.PreprodSync = 0;
                //            tObject.ProdSync = 0;
                //            //tObject.UpdateMe();
                //        }
                //    }
                //}
                //if (GUILayout.Button("preprod force push", EditorStyles.miniButton))
                //{
                //    EditorPrefs.SetInt(SynchronizationPrefsKey(NWDAppConfiguration.SharedInstance().PreprodEnvironment), 0);
                //    foreach (K tObject in ObjectsList)
                //    {
                //        if (tObject.IntegrityValue() == tObject.Integrity)
                //        {
                //            tObject.DevSync = 0;
                //            tObject.PreprodSync = 0;
                //            tObject.ProdSync = 0;
                //            //tObject.UpdateMe();
                //        }
                //    }
                //}
                //if (GUILayout.Button("prod force push", EditorStyles.miniButton))
                //{
                //    EditorPrefs.SetInt(SynchronizationPrefsKey(NWDAppConfiguration.SharedInstance().ProdEnvironment), 0);
                //    foreach (K tObject in ObjectsList)
                //    {
                //        if (tObject.IntegrityValue() == tObject.Integrity)
                //        {
                //            tObject.DevSync = 0;
                //            tObject.PreprodSync = 0;
                //            tObject.ProdSync = 0;
                //            //tObject.UpdateMe();
                //        }
                //    }
                //}
                //GUILayout.EndHorizontal();


				//GUILayout.Space (10.0f);
				//// -------------------------------------------
				//if (GUILayout.Button (NWDConstants.K_APP_BASIS_CLASS_PHP_GENERATE+" '" + MenuName () + "'", EditorStyles.miniButton))
				//{
				//	CreateAllPHP ();
				//}
//				if (GUILayout.Button (NWDConstants.K_APP_BASIS_CLASS_CSHARP_GENERATE+" '" + MenuName () + "'", EditorStyles.miniButton))
//				{
//					CreateCSharp ();
//				}
			}
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif