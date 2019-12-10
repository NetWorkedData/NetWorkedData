//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:9
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDEditorWindow : EditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        static List<NWDEditorWindow> AllWindowsList = new List<NWDEditorWindow>();
        //-------------------------------------------------------------------------------------------------------------
        public static void GenerateCSharpFile()
        {
            //Debug.Log("NWDEditorWindow GenerateCSharpFile()");
            if (NWDAppConfiguration.SharedInstance().ShowCompile == true)
            {
                foreach (NWDEditorWindow tWindow in AllWindowsList)
                {
                    //Debug.Log("NWDEditorWindow RecompilePrevent() tWindow = " + tWindow.titleContent.text);
                    tWindow.Recompile = true;
                    tWindow.Repaint();
                }
            }
            NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
        }
        //-------------------------------------------------------------------------------------------------------------
        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            //Debug.Log("NWDEditorWindow OnScriptsReloaded()");
            if (AllWindowsList != null)
            {
                foreach (NWDEditorWindow tWindow in AllWindowsList)
                {
                    if (tWindow != null)
                    {
                        tWindow.Recompile = false;
                        tWindow.Repaint();
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        bool Recompile = false;
        //-------------------------------------------------------------------------------------------------------------
        public NWDEditorWindow()
        {
            //Debug.Log("NWDEditorWindow()");
            if (AllWindowsList.Contains(this) == false)
            {
                AllWindowsList.Add(this);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        ~NWDEditorWindow()
        {
            //Debug.Log("~NWDEditorWindow()");
            if (AllWindowsList.Contains(this) == false)
            {
                AllWindowsList.Remove(this);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnGUI()
        {
            //Debug.Log("NWDEditorWindow OnGUI()");
            NWDGUI.LoadStyles();
            if (Recompile == false || EditorApplication.isCompiling == false)
            {
                OnPreventGUI();
            }
            else
            {
                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                EditorGUILayout.HelpBox("...compile in progress...", MessageType.Warning, false);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
            }
           // if (EditorGUIUtility.isProSkin == true)
            {
                float LogoSize = NWDGUI.kTitleStyle.fixedHeight;
                GUI.Label(new Rect(position.width - LogoSize, 0, LogoSize, LogoSize), NWDGUI.kNetWorkedDataLogoContent);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnPreventGUI()
        {
            //Debug.Log("NWDEditorWindow OnPreventGUI()");
            throw new Exception("override OnPreventGUI() in place of OnGUI");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
