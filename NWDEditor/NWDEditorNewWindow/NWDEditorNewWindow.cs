//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:22:49
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text.RegularExpressions;
using System;
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDEditorNewWindow : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        Vector2 ScrollPosition = Vector2.zero;
        //-------------------------------------------------------------------------------------------------------------
        string WindowName = string.Empty;
        string WindowMenuName = string.Empty;
        string WindowDescription = string.Empty;
        int WindowMenuPosition = 0; // 0-1000 + 2000 : => [2000 … 3000]
        List<string> ClassesList = new List<string>();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Raises the OnGUI event.
        /// </summary>
        public void GenerateNewWindow()
        {
            //NWEBenchmark.Start();
            GUI.FocusControl(null);
            // get the NWDExample code source
            string tClassExamplePath = NWDFindPackage.PathOfPackage() + "/NWDEditor/NWDObjects/NWDWindowExample/NWDWindowExample.cs";
            string tClassExample = File.ReadAllText(tClassExamplePath);
            // replace template by this params
            tClassExample = tClassExample.Replace("NWDWindowExample_Name", WindowMenuName);
            tClassExample = tClassExample.Replace("NWDWindowExample_Description", WindowDescription);
            tClassExample = tClassExample.Replace("12345", (2000 + WindowMenuPosition).ToString());
            tClassExample = tClassExample.Replace("NWDWindowExample", WindowName);
            tClassExample = tClassExample.Replace("//[MenuItem ", "[MenuItem ");
            // place the classes
            string tClassesLinearize = string.Empty;
            foreach (string tKey in ClassesList)
            {
                tClassesLinearize += "typeof(" + tKey + "),\n\t\t";
            }
            tClassExample = tClassExample.Replace("typeof(NWDExample),", tClassesLinearize);
            // find the owner classes folder
            string tOwnerClassesFolderPath = NWDToolbox.FindOwnerClassesFolder();
            // write file
            string tFilePath = tOwnerClassesFolderPath + "/" + WindowName + ".cs";
            File.WriteAllText(tFilePath, tClassExample);
            // flush params
            WindowName = string.Empty;
            WindowMenuName = string.Empty;
            WindowDescription = string.Empty;
            WindowMenuPosition = 1000;
            ClassesList = new List<string>();
            // import new script
            AssetDatabase.ImportAsset(tFilePath);
            // create directories
            Directory.CreateDirectory(tOwnerClassesFolderPath);
            Directory.CreateDirectory(tOwnerClassesFolderPath + "/Editor");
            // write icon to modify
            string tIconPath = NWDFindPackage.PathOfPackage() + "/NWDEditor/Editor/Textures/NWDExampleWindow.psd";
            string tIconPathNew = tOwnerClassesFolderPath + "/Editor/" + WindowName + ".psd";
            File.Copy(tIconPath, tIconPathNew);
            string tIconPathPro = NWDFindPackage.PathOfPackage() + "/NWDEditor/Editor/Textures/NWDExampleWindow_pro.psd";
            string tIconPathNewPro = tOwnerClassesFolderPath + "/Editor/" + WindowName + "_pro.psd";
            File.Copy(tIconPathPro, tIconPathNewPro);
            AssetDatabase.ImportAsset(tIconPathNew);
            AssetDatabase.ImportAsset(tIconPathNewPro);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Raises the enable event.
        /// </summary>
        public void OnEnable()
        {
            //NWEBenchmark.Start();
            TitleInit("Custom Window Manager", typeof(NWDEditorNewWindow));
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Raises the OnGUI event. Create the interface to enter a new class.
        /// </summary>
        public override void OnPreventGUI()
        {
            //NWEBenchmark.Start();
            NWDGUILayout.Title("Custom Window Manager ");
            NWDGUILayout.Informations("Custom your window!");
            NWDGUILayout.Line();
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition);
            //Prepare the form varaible 
            Regex tRegExpression = new Regex("[^a-zA-Z]");
            //		Regex tRegExpressionProperties = new Regex ("[^a-zA-Z0-9]");
            Regex tRegExpressionEmptyType = new Regex("[ ]+");
            // validate the form ?
            bool tCanCreate = true;
            // start Layout
            NWDGUILayout.HelpBox("Helper to create a new NWDBasis herited class. NWDBasis is the class of data in NetWorkedData framework.");
            // futur class infos
            NWDGUILayout.SubSection("Class informations");
            WindowName = EditorGUILayout.TextField("Name ", WindowName);
            WindowName = tRegExpression.Replace(WindowName, string.Empty);
            if (WindowName.Length < 3)
            {
                EditorGUILayout.LabelField(" ", "name must be longer than 3 characters");
                tCanCreate = false;
            }
            else
            {
                //				TODO: find if Type exists
                //			foreach (Type tType in NWDDataManager.SharedInstance().mTypeList) {
                //				if (tType.Name == ClassName) {
                //					tCanCreate = false;
                //				}
                //			}
                if (tCanCreate == false)
                {
                    EditorGUILayout.LabelField(" ", "this class already exists");
                }
                else
                {
                    EditorGUILayout.LabelField(" ", "class name is Ok!");
                }
            }
            NWDGUILayout.SubSection("Window description");
            // futur class description
            WindowDescription = EditorGUILayout.TextField("Description", WindowDescription);
            WindowDescription = WindowDescription.Replace("\\", string.Empty);
            NWDGUILayout.SubSection("Menu in interface");
            // futur class menu name
            WindowMenuName = EditorGUILayout.TextField("Menu name", WindowMenuName);
            WindowMenuName = WindowMenuName.Replace("\\", string.Empty);
            if (WindowMenuName.Length < 3)
            {
                EditorGUILayout.LabelField(" ", "menu name must be longer than 2 characters");
                tCanCreate = false;
            }
            else if (WindowMenuName.Length > 16)
            {
                EditorGUILayout.LabelField(" ", "menu name must be shorter than 16 characters");
                tCanCreate = false;
            }
            else
            {
                EditorGUILayout.LabelField(" ", "menu name is Ok!");
            }
            WindowMenuPosition = EditorGUILayout.IntField("Menu position", WindowMenuPosition);
            if (WindowMenuPosition < 0)
            {
                EditorGUILayout.LabelField(" ", "menu Position  must be greater than 0");
                tCanCreate = false;
            }
            else if (WindowMenuPosition > 1000)
            {
                EditorGUILayout.LabelField(" ", "menu Position  must be shorter than 1000");
                tCanCreate = false;
            }
            else
            {
                EditorGUILayout.LabelField(" ", "menu Position  is Ok!");
            }

            NWDGUILayout.SubSection("Classes management");
            // create properties type
            List<string> tListOfType = new List<string>();
            tListOfType.Add(" ");
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeList)
            {
                tListOfType.Add(tType.Name);
            }
            tListOfType.Sort();
            // prepare result properties
            List<string> tNextClassList = new List<string>();
            int tCounter = 0;
            foreach (string tKey in ClassesList)
            {
                tCounter++;
                int tIndex = tListOfType.IndexOf(tKey);
                if (tIndex < 0 || tIndex > tListOfType.Count)
                {
                    tIndex = 0;
                }
                tIndex = EditorGUILayout.Popup("Class " + tCounter, tIndex, tListOfType.ToArray());
                string tSelectedType = tListOfType[tIndex];
                tSelectedType = tRegExpressionEmptyType.Replace(tSelectedType, " ");
                tNextClassList.Add(tSelectedType);
                // remove this string from possiblities 
                tListOfType.Remove(tSelectedType);
            }
            // add New property
            int tNextIndex = EditorGUILayout.Popup("Class " + tCounter, 0, tListOfType.ToArray());
            string tNextSelectedType = tListOfType[tNextIndex];
            tNextSelectedType = tRegExpressionEmptyType.Replace(tNextSelectedType, " ");
            tNextClassList.Add(tNextSelectedType);

            // remove empty properties
            tNextClassList.Remove(" ");
            // meorize new properties list
            ClassesList = tNextClassList;
            // Generate Button
            EditorGUILayout.Space();
            // if ok continue else disable
            GUILayout.EndScrollView();
            NWDGUILayout.Line();
            NWDGUILayout.LittleSpace();
            EditorGUI.BeginDisabledGroup(!tCanCreate);
            if (GUILayout.Button("Generate window"))
            {
                // ok generate!
                GenerateNewWindow();
            }
            EditorGUI.EndDisabledGroup();
            NWDGUILayout.BigSpace();
            // calculate the good dimension for window
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Removes all predicate for the empty properties value key at the end of GUI.
        /// </summary>
        /// <returns><c>true</c>, if all predicate was removed, <c>false</c> otherwise.</returns>
        /// <param name="tObject">T object.</param>
        bool RemoveAllPredicate(KeyValuePair<string, string> tObject)
        {
            //NWEBenchmark.Start();
            bool tReturn = false;
            if (tObject.Key == "" && tObject.Value == " ")
            {
                tReturn = true;
            }
            //NWEBenchmark.Finish();
            return tReturn;
        }

        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
