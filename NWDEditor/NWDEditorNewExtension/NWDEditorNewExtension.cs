//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
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
using System.Linq;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
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
        /// <summary>
        /// The Shared Instance.
        /// </summary>
        private static NWDEditorNewExtension kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the SharedInstance or instance one
        /// </summary>
        /// <returns></returns>
        public static NWDEditorNewExtension SharedInstance()
        {
            //NWDBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDEditorNewExtension)) as NWDEditorNewExtension;
            }
            //NWDBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Show the SharedInstance of Editor Configuration Manager Window and focus on.
        /// </summary>
        /// <returns></returns>
        public static void SharedInstanceFocus()
        {
            //NWDBenchmark.Start();
            SharedInstance().ShowUtility();
            SharedInstance().Focus();
            //NWDBenchmark.Finish();
        }
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
            //NWDBenchmark.Start();
            GUI.FocusControl(null);

            if (string.IsNullOrEmpty(ClassBase))
            {
                ClassBase = "NWDExample";
            }
            // get the NWDExample code source
            string tClassExamplePath = NWDFindPackage.PathOfPackage() + "/NWDEditor/NWDObjects/NWDExample/NWDExample_Extension.cs";
            string tClassExample = File.ReadAllText(tClassExamplePath);
            // prepare properties 
            Dictionary<string, string> tPropertiesDico = new Dictionary<string, string>(new StringIndexKeyComparer());
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

            string tMacro = null;
            foreach (NWDClassMacroAttribute tMacroAttribute in tBasisHelper.ClassType.GetCustomAttributes(typeof(NWDClassMacroAttribute), true))
            {
                tMacro = tMacroAttribute.Macro;
            }
            if (string.IsNullOrEmpty(tMacro) == false)
            {
                tClassExample = tClassExample.Replace("NWD_EXAMPLE_MACRO", tMacro);
                tClassExample = tClassExample.Replace("//MACRO_DEFINE ", "");
            }

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
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnEnable()
        {
            //NWDBenchmark.Start();
            if (IconAndTitle == null)
            {
                IconAndTitle = new GUIContent();
                IconAndTitle.text = "Custom Extension";
                if (IconAndTitle.image == null)
                {
                    string[] sGUIDs = AssetDatabase.FindAssets(typeof(NWDEditorNewExtension).Name +" t:texture");
                    foreach (string tGUID in sGUIDs)
                    {
                        string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                        string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                        if (tPathFilename.Equals(typeof(NWDEditorNewExtension).Name))
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
            foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeList)
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
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Raises the OnGUI event. Create the interface to enter a new class.
        /// </summary>
        public override void OnPreventGUI()
        {
            //NWDBenchmark.Start();
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
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Removes all predicate for the empty properties value key at the end of GUI.
        /// </summary>
        /// <returns><c>true</c>, if all predicate was removed, <c>false</c> otherwise.</returns>
        /// <param name="tObject">T object.</param>
        bool RemoveAllPredicate(KeyValuePair<string, string> tObject)
        {
            //NWDBenchmark.Start();
            bool tReturn = false;
            if (tObject.Key == string.Empty && tObject.Value == " ")
            {
                tReturn = true;
            }
            //NWDBenchmark.Finish();
            return tReturn;
        }

        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
