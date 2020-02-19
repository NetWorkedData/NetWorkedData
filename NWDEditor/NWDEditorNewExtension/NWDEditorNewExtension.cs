﻿//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:22:47
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
using System.Text.RegularExpressions;
using UnityEngine;
using System;
using System.Reflection;
using System.IO;
using UnityEditor;
//using BasicToolBox;
using System.Linq;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWD editor new class. Can create a new classes based on NWDExample automatically from the form generated in this editor window.
    /// </summary>
    public class NWDEditorNewExtension : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        GUIContent IconAndTitle;
        Vector2 ScrollPosition = Vector2.zero;
        //-------------------------------------------------------------------------------------------------------------
        string ClassBase = "NWDBasis";
        /// <summary>
        /// The futur name of the class.
        /// </summary>
        List<KeyValuePair<string, string>> ClassNameProperties = new List<KeyValuePair<string, string>>();

        List<string> tListOfType = new List<string>();
        List<string> tListOfclass = new List<string>();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Generate the new class. It's not Magic, it's Sciences! (and a little bit of magic :-p )
        /// </summary>
        public void GenerateNewClass()
        {
            //NWEBenchmark.Start();
            GUI.FocusControl(null);

            if (string.IsNullOrEmpty(ClassBase))
            {
                ClassBase = "NWDExample";
            }
            // get the NWDExample code source
            string tClassExamplePath = NWDFindPackage.PathOfPackage() + "/NWDEditor/NWDObjects/NWDExample/NWDExample_Extension.cs";
            string tClassExample = File.ReadAllText(tClassExamplePath);
            // prepare properties 
            Dictionary<string, string> tPropertiesDico = new Dictionary<string, string>();
            foreach (KeyValuePair<string, string> tKeyValue in ClassNameProperties)
            {
                if (tKeyValue.Key != string.Empty && tKeyValue.Value != " " && tKeyValue.Value != string.Empty)
                {
                    if (tPropertiesDico.ContainsKey(tKeyValue.Key) == false)
                    {
                        string tTypeString = tKeyValue.Value;
                        if (tTypeString.Contains("<K>/"))
                        {
                            tTypeString = tTypeString.Replace("<K>/", "<") + ">";
                        }
                        tPropertiesDico.Add(tKeyValue.Key, tTypeString);
                    }
                }
            }
            // place the properties
            string tPropertiesLinearize = "//PROPERTIES\n\t\t[NWDInspectorGroupReset]\n";
            foreach (KeyValuePair<string, string> tKeyValue in tPropertiesDico)
            {
                tPropertiesLinearize += "\t\tpublic " + tKeyValue.Value + " " + tKeyValue.Key + " {get; set;}\n";
            }
            tClassExample = tClassExample.Replace("//#warning", "#warning");
            tClassExample = tClassExample.Replace("NWDExample", ClassBase);
            NWDBasisHelper tBasisHelper = NWDBasisHelper.FindTypeInfos(ClassBase);
            string tClassParent = "NWDBasis?";
            if (tBasisHelper.ClassType != null)
            {
                tClassParent = tBasisHelper.ClassType.BaseType.Name;
            }
            tClassExample = tClassExample.Replace("NWDBasis", tClassParent);
            tClassExample = tClassExample.Replace("//PROPERTIES", tPropertiesLinearize);
            // find the owner classes folder
            string tOwnerClassesFolderPath = NWDToolbox.FindOwnerClassesFolder() + "/" + ClassBase + "_Extension";
            // create directories
            Directory.CreateDirectory(tOwnerClassesFolderPath);
            // write file basis
            string tFilePath = tOwnerClassesFolderPath + "/" + ClassBase + "_Extension.cs";
            File.WriteAllText(tFilePath, tClassExample);

            // flush params
            ClassNameProperties = new List<KeyValuePair<string, string>>();

            // import new script
            AssetDatabase.ImportAsset(tFilePath);

            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(tFilePath);
            EditorGUIUtility.PingObject(Selection.activeObject);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnEnable()
        {
            //NWEBenchmark.Start();
            if (IconAndTitle == null)
            {
                IconAndTitle = new GUIContent();
                IconAndTitle.text = "NWD Custom Extension";
                if (IconAndTitle.image == null)
                {
                    string[] sGUIDs = AssetDatabase.FindAssets("NWDEditorNewWindow t:texture");
                    foreach (string tGUID in sGUIDs)
                    {
                        string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                        string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                        if (tPathFilename.Equals("NWDEditorNewWindow"))
                        {
                            IconAndTitle.image = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                        }
                    }
                }
                titleContent = IconAndTitle;
            }
            tListOfType = new List<string>();
            tListOfType.Add(" ");
            tListOfType.Add("string");
            tListOfType.Add("bool");
            tListOfType.Add("int");
            tListOfType.Add("long");
            tListOfType.Add("double");
            tListOfType.Add("float");
            tListOfType.Add("  "); // use as separator remove by ereg

            Type[] tAllTypes = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            Type[] tAllNWDTypes = (from System.Type type in tAllTypes
                                   where type.IsSubclassOf(typeof(NWEDataType))
                                   select type).ToArray();
            List<string> tClassPossiblesList = new List<string>();
            foreach (Type tType in tAllNWDTypes)
            {
                if (tType.ContainsGenericParameters == false)
                {
                    tListOfType.Add(tType.Name);
                }
                else
                {
                    tClassPossiblesList.Add(tType.Name.Replace("`1", ""));
                }
            }
            tListOfclass = new List<string>();
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeList)
            {
                NWDBasisHelper tDatas = NWDBasisHelper.FindTypeInfos(tType);
                if (tDatas != null)
                {
                    tListOfclass.Add(tDatas.ClassNamePHP);
                }
            }
            tListOfclass.Sort();
            tClassPossiblesList.Sort();
            foreach (string tTypeName in tListOfclass)
            {
                foreach (string tCC in tClassPossiblesList)
                {
                    tListOfType.Add(tCC + "<K>/" + tTypeName);
                }
            }
            if (tListOfclass.Contains("NWDBasis"))
            {
                tListOfclass.Remove("NWDBasis");
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Raises the OnGUI event. Create the interface to enter a new class.
        /// </summary>
        public override void OnPreventGUI()
        {
            //NWEBenchmark.Start();
            NWDGUI.LoadStyles();
            NWDGUILayout.Title("Custom Extension Generator");
            NWDGUILayout.Informations("Custom your class!");
            NWDGUILayout.Line();
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition);

            //Prepare the form varaible 
            Regex tRegExpression = new Regex("[^a-zA-Z]");
            Regex tRegExpressionProperties = new Regex("[^a-zA-Z0-9]");
            Regex tRegExpressionEmptyType = new Regex("[ ]+");
            // validate the form ?
            bool tCanCreate = true;
            NWDGUILayout.HelpBox("Helper to create a new NWDBasis herited class. NWDBasis is the class of data in NetWorkedData framework.");
            // futur class infos
            NWDGUILayout.SubSection("Class informations");
            int tBaseIndex = tListOfclass.IndexOf(ClassBase);
            tBaseIndex = EditorGUILayout.Popup("Base", tBaseIndex, tListOfclass.ToArray());
            if (tBaseIndex >= 0 && tBaseIndex < tListOfclass.Count)
            {
                ClassBase = tListOfclass[tBaseIndex];
            }
            NWDGUILayout.SubSection("Properties");
            // create properties type
            // prepare result properties
            List<KeyValuePair<string, string>> tNextClassNameProperties = new List<KeyValuePair<string, string>>();
            int tCounter = 0;
            foreach (KeyValuePair<string, string> tKeyValue in ClassNameProperties)
            {
                tCounter++;
                GUILayout.BeginHorizontal();
                int tIndex = tListOfType.IndexOf(tKeyValue.Value);
                if (tIndex < 0 || tIndex > tListOfType.Count)
                {
                    tIndex = 0;
                }
                tIndex = EditorGUILayout.Popup("Property " + tCounter, tIndex, tListOfType.ToArray());
                string tPropertyType = tListOfType[tIndex];
                tPropertyType = tRegExpressionEmptyType.Replace(tPropertyType, " ");
                string tPropertyName = tKeyValue.Key;
                tPropertyName = EditorGUILayout.TextField(tPropertyName, GUILayout.MaxWidth(160));
                tPropertyName = tRegExpressionProperties.Replace(tPropertyName, string.Empty);
                if (tPropertyType != string.Empty || tPropertyName != string.Empty)
                {
                    KeyValuePair<string, string> tEnter = new KeyValuePair<string, string>(tPropertyName, tPropertyType);
                    tNextClassNameProperties.Add(tEnter);
                }
                GUILayout.EndHorizontal();
            }
            // add New property
            GUILayout.BeginHorizontal();
            int tNextIndex = 0;
            tNextIndex = EditorGUILayout.Popup("New Property", tNextIndex, tListOfType.ToArray());
            string tNextPropertyType = tListOfType[tNextIndex];
            tNextPropertyType = tRegExpressionEmptyType.Replace(tNextPropertyType, " ");
            string tNextPropertyName = string.Empty;
            tNextPropertyName = EditorGUILayout.TextField(tNextPropertyName, GUILayout.MaxWidth(160));
            tNextPropertyName = tRegExpressionProperties.Replace(tNextPropertyName, string.Empty);
            if (tNextPropertyType != string.Empty || tNextPropertyName != string.Empty)
            {
                KeyValuePair<string, string> tEnter = new KeyValuePair<string, string>(tNextPropertyName, tNextPropertyType);
                tNextClassNameProperties.Add(tEnter);
            }
            GUILayout.EndHorizontal();
            // remove empty properties
            tNextClassNameProperties.RemoveAll(RemoveAllPredicate);
            // meorize new properties list
            ClassNameProperties = tNextClassNameProperties;
            // Generate Button
            EditorGUILayout.Space();
            // if ok continue else disable
            GUILayout.EndScrollView();
            NWDGUILayout.Line();
            NWDGUILayout.LittleSpace();
            EditorGUI.BeginDisabledGroup(!tCanCreate);
            if (GUILayout.Button("Generate class"))
            {
                // ok generate!
                GenerateNewClass();
            }
            EditorGUI.EndDisabledGroup();
            NWDGUILayout.BigSpace();
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
            if (tObject.Key == string.Empty && tObject.Value == " ")
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