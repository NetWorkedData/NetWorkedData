//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        //[NWDAliasMethod(NWDConstants.M_DrawTypeInInspector)]
       /*public static void DrawTypeInInspector()
        {
            if (BasisHelper().SaltValid == false)
            {
                EditorGUILayout.HelpBox(NWDConstants.K_ALERT_SALT_SHORT_ERROR, MessageType.Error);
            }
            EditorGUILayout.LabelField(NWDConstants.K_APP_BASIS_CLASS_DESCRIPTION, EditorStyles.boldLabel);
            EditorGUILayout.HelpBox(BasisHelper().ClassDescription, MessageType.Info);
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
            DrawSettingsEditor();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void DrawSettingsEditor()
        {
            GUIStyle tStyle = EditorStyles.foldout;
            FontStyle tPreviousStyle = tStyle.fontStyle;
            tStyle.fontStyle = FontStyle.Bold;
            BasisHelper().mSettingsShowing = EditorGUILayout.Foldout(BasisHelper().mSettingsShowing, NWDConstants.K_APP_BASIS_CLASS_WARNING_ZONE, tStyle);
            tStyle.fontStyle = tPreviousStyle;
            if (BasisHelper().mSettingsShowing == true)
            {
                EditorGUILayout.HelpBox(NWDConstants.K_APP_BASIS_CLASS_WARNING_HELPBOX, MessageType.Warning);

                if (GUILayout.Button(NWDConstants.K_APP_BASIS_CLASS_RESET_TABLE, EditorStyles.miniButton))
                {
                    NWDBasis<K>.ResetTable();
                }
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(NWDConstants.K_APP_BASIS_CLASS_FIRST_SALT, BasisHelper().SaltStart);
                if (GUILayout.Button(NWDConstants.K_APP_BASIS_CLASS_REGENERATE, EditorStyles.miniButton))
                {
                    GUI.FocusControl(null);
                    BasisHelper().SaltStart = NWDToolbox.RandomString(UnityEngine.Random.Range(12, 24));
                    RecalculateAllIntegrities();
                }
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(NWDConstants.K_APP_BASIS_CLASS_SECOND_SALT, BasisHelper().SaltEnd);
                if (GUILayout.Button(NWDConstants.K_APP_BASIS_CLASS_REGENERATE, EditorStyles.miniButton))
                {
                    GUI.FocusControl(null);
                    BasisHelper().SaltEnd = NWDToolbox.RandomString(UnityEngine.Random.Range(12, 24));
                    RecalculateAllIntegrities();
                }
                GUILayout.EndHorizontal();

                if (GUILayout.Button(NWDConstants.K_APP_BASIS_CLASS_INTEGRITY_REEVALUE, EditorStyles.miniButton))
                {
                    GUI.FocusControl(null);
                    RecalculateAllIntegrities();
                }
            }
        }
        */
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif