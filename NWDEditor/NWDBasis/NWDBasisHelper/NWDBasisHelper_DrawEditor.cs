// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:27
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
using SQLite4Unity3d;
using BasicToolBox;
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        public void New_DrawTypeInInspector()
        {
            //BTBBenchmark.Start();
            if (SaltValid == false)
            {
                EditorGUILayout.HelpBox(NWDConstants.K_ALERT_SALT_SHORT_ERROR, MessageType.Error);
            }
            EditorGUILayout.LabelField(NWDConstants.K_APP_BASIS_CLASS_DESCRIPTION, EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(ClassDescription, MessageType.Info);
            if (NWDAppConfiguration.SharedInstance().DevEnvironment.Selected == true)
            {
                EditorGUILayout.LabelField(NWDConstants.K_APP_BASIS_CLASS_DEV, EditorStyles.boldLabel);
            }
            if (NWDAppConfiguration.SharedInstance().PreprodEnvironment.Selected == true)
            {
                EditorGUILayout.LabelField(NWDConstants.K_APP_BASIS_CLASS_PREPROD, EditorStyles.boldLabel);
            }
            if (NWDAppConfiguration.SharedInstance().ProdEnvironment.Selected == true)
            {
                EditorGUILayout.LabelField(NWDConstants.K_APP_BASIS_CLASS_PROD, EditorStyles.boldLabel);
            }
            GUIStyle tStyle = EditorStyles.foldout;
            FontStyle tPreviousStyle = tStyle.fontStyle;
            tStyle.fontStyle = FontStyle.Bold;
            mSettingsShowing = EditorGUILayout.Foldout(mSettingsShowing, NWDConstants.K_APP_BASIS_CLASS_WARNING_ZONE, tStyle);
            tStyle.fontStyle = tPreviousStyle;
            if (mSettingsShowing == true)
            {
                EditorGUILayout.HelpBox(NWDConstants.K_APP_BASIS_CLASS_WARNING_HELPBOX, MessageType.Warning);

                if (GUILayout.Button(NWDConstants.K_APP_BASIS_CLASS_RESET_TABLE, EditorStyles.miniButton))
                {
                    New_ResetTable();
                }
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(NWDConstants.K_APP_BASIS_CLASS_FIRST_SALT, SaltStart);
                if (GUILayout.Button(NWDConstants.K_APP_BASIS_CLASS_REGENERATE, EditorStyles.miniButton))
                {
                    GUI.FocusControl(null);
                    SaltStart = NWDToolbox.RandomString(UnityEngine.Random.Range(12, 24));
                    New_RecalculateAllIntegrities();
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(NWDConstants.K_APP_BASIS_CLASS_SECOND_SALT, SaltEnd);
                if (GUILayout.Button(NWDConstants.K_APP_BASIS_CLASS_REGENERATE, EditorStyles.miniButton))
                {
                    GUI.FocusControl(null);
                    SaltEnd = NWDToolbox.RandomString(UnityEngine.Random.Range(12, 24));
                    New_RecalculateAllIntegrities();
                }
                GUILayout.EndHorizontal();

                if (GUILayout.Button(NWDConstants.K_APP_BASIS_CLASS_INTEGRITY_REEVALUE, EditorStyles.miniButton))
                {
                    GUI.FocusControl(null);
                    New_RecalculateAllIntegrities();
                }
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void New_SelectedFirstObjectInTable(EditorWindow sEditorWindow)
        {
            //BTBBenchmark.Start();
            if (EditorTableDatas.Count > 0)
            {
                NWDTypeClass sObject = EditorTableDatas.ElementAt(0);
                New_SetObjectInEdition(sObject);
                sEditorWindow.Focus();
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif