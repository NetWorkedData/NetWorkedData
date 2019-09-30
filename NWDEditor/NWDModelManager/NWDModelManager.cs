//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:16
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using UnityEngine;
using System.IO;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDModelManager : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The icon and title.
        /// </summary>
        GUIContent IconAndTitle;
        Vector2 ScrollPosition;
        public List<Type> TypeList = new List<Type>();
        public List<Type> TypeErrorList = new List<Type>();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance.
        /// </summary>
        public static NWDModelManager kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDModelManager SharedInstance()
        {
            //NWEBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDModelManager)) as NWDModelManager;
            }
            //NWEBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDModelManager SharedInstanceFocus()
        {
            //NWEBenchmark.Start();
            SharedInstance().ShowUtility();
            SharedInstance().Focus();
            //NWEBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Refresh()
        {
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWDModelManager));
            foreach (NWDModelManager tWindow in tWindows)
            {
                tWindow.Repaint();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool IsSharedInstanced()
        {
            if (kSharedInstance != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnEnable()
        {
            //NWEBenchmark.Start();
            if (IconAndTitle == null)
            {
                IconAndTitle = new GUIContent();
                IconAndTitle.text = NWDConstants.K_APP_MODEL_MANAGER_TITLE;
                if (IconAndTitle.image == null)
                {
                    string[] sGUIDs = AssetDatabase.FindAssets(typeof(NWDModelManager).Name + " t:texture");
                    foreach (string tGUID in sGUIDs)
                    {
                        string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                        string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                        if (tPathFilename.Equals(typeof(NWDModelManager).Name))
                        {
                            IconAndTitle.image = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                        }
                    }
                }
                titleContent = IconAndTitle;
            }
            TypeList.Clear();
            TypeErrorList.Clear();
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeLoadedList.OrderBy(A => NWDBasisHelper.FindTypeInfos(A).ClassNamePHP))
            {
                TypeList.Add(tType);
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                if (tHelper.WebModelChanged == true || tHelper.WebModelDegraded == true || tHelper.SaltValid == false)
                {
                    TypeErrorList.Add(tType);
                }
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void DrawType(Type sType)
        {
            GUILayout.BeginVertical(/*EditorStyles.helpBox*/);
            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(sType);
            NWDGUILayout.SubSection(tHelper.ClassNamePHP);
            GUILayout.BeginHorizontal();
            Texture2D tTextureOfClass = tHelper.TextureOfClass();
            if (tTextureOfClass != null)
            {
                GUILayout.Label(tTextureOfClass, NWDGUI.KTableSearchClassIcon, GUILayout.Width(48.0F), GUILayout.Height(48.0F));
            }
            GUILayout.BeginVertical();
            GUILayout.Label(tHelper.ClassMenuName, EditorStyles.boldLabel);
            GUILayout.Label("Webservice last version generated for this Class  is " + tHelper.LastWebBuild.ToString() + " ( App use Webservice " + NWDAppConfiguration.SharedInstance().WebBuild.ToString() + ")");
            GUILayout.Label(tHelper.ClassDescription);
            NWDGUILayout.Separator();
            foreach (KeyValuePair<int, string> tModels in tHelper.WebModelSQLOrder)
            {
                GUILayout.Label("Model has definition for Webservice " + tModels.Key.ToString());
            }

            if (tHelper.SaltValid == false)
            {
                if (NWDGUILayout.AlertBoxButton(NWDConstants.K_ALERT_SALT_SHORT_ERROR, NWDConstants.K_APP_CLASS_SALT_REGENERATE))
                {
                    tHelper.DeleteOldsModels();
                    NWDEditorWindow.GenerateCSharpFile();
                    //NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
                    GUIUtility.ExitGUI();
                }
            }
            if (tHelper.WebModelChanged == true)
            {
                // draw reintegrate the model
                if (NWDGUILayout.WarningBoxButton(NWDConstants.K_APP_BASIS_WARNING_MODEL + "\n" + tHelper.ModelChangedGetChange(), NWDConstants.K_APP_WS_PHP_TOOLS.Replace("XXXX", NWDAppConfiguration.SharedInstance().WebBuild.ToString("0000"))))
                {
                    tHelper.ForceOrders(NWDAppConfiguration.SharedInstance().WebBuild);
                    NWDAppConfiguration.SharedInstance().DevEnvironment.CreatePHP(new List<Type> { sType }, false, false);
                    NWDAppConfiguration.SharedInstance().PreprodEnvironment.CreatePHP(new List<Type> { sType }, false, false);
                    NWDAppConfiguration.SharedInstance().ProdEnvironment.CreatePHP(new List<Type> { sType }, false, false);
                    NWDEditorWindow.GenerateCSharpFile();
                    //NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
                    GUIUtility.ExitGUI();
                }
            }
            if (tHelper.WebModelDegraded == true)
            {
                if (NWDGUILayout.WarningBoxButton(NWDConstants.K_APP_BASIS_WARNING_MODEL_DEGRADED + "\n" + tHelper.ModelChangedGetChange(), NWDConstants.K_APP_WS_DELETE_OLD_MODEL_TOOLS))
                {
                    tHelper.DeleteOldsModels();
                    NWDEditorWindow.GenerateCSharpFile();
                    //NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
                    GUIUtility.ExitGUI();
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnPreventGUI()
        {
            //NWEBenchmark.Start();
            NWDGUI.LoadStyles();
            NWDGUILayout.Title("Model Manager");
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if (TypeErrorList.Count > 0)
            {
                NWDGUILayout.Section("Models in error");
                foreach (Type tType in TypeErrorList)
                {
                    DrawType(tType);
                }
            }
            NWDGUILayout.Section("Models list");
            foreach (Type tType in TypeList)
            {
                DrawType(tType);
            }
            GUILayout.EndScrollView();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
