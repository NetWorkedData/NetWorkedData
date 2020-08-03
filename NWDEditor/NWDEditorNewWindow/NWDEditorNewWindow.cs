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
#endif
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
        string WindowMacro = string.Empty;
        string WindowMenuName = string.Empty;
        string WindowDescription = string.Empty;
        int WindowMenuPosition = 0; // 0-1000 + 2000 : => [2000 … 3000]
        List<string> ClassesList = new List<string>();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance.
        /// </summary>
        private static NWDEditorNewWindow kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the SharedInstance or instance one
        /// </summary>
        /// <returns></returns>
        public static NWDEditorNewWindow SharedInstance()
        {
            //NWDBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDEditorNewWindow)) as NWDEditorNewWindow;
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
        /// <summary>
        /// Raises the OnGUI event.
        /// </summary>
        public void GenerateNewWindow()
        {
            //NWDBenchmark.Start();
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
            if (string.IsNullOrEmpty(WindowMacro) == false)
            {
                tClassExample = tClassExample.Replace("NWD_EXAMPLE_MACRO", WindowMacro);
                tClassExample = tClassExample.Replace("//MACRO_DEFINE ", "");
            }
            // place the classes
            string tClassesLinearize = string.Empty;
            foreach (string tKey in ClassesList)
            {
                tClassesLinearize += "typeof(" + tKey + "),\n\t\t";
            }
            tClassExample = tClassExample.Replace("typeof(NWDExample),", tClassesLinearize);
            // find the owner classes folder
            string tOwnerClassesFolderPath = NWDToolbox.FindOwnerClassesFolder();
            Directory.CreateDirectory(tOwnerClassesFolderPath);
            Directory.CreateDirectory(tOwnerClassesFolderPath + "/" + WindowName);
            // write file
            string tFilePath = tOwnerClassesFolderPath + "/" + WindowName + "/" + WindowName + ".cs";
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
            Directory.CreateDirectory(tOwnerClassesFolderPath + "/" + WindowName + "/Editor");
            // write icon to modify
            string tIconPath = NWDFindPackage.PathOfPackage() + "/NWDEditor/Editor/NWDExampleWindow.psd";
            string tIconPathNew = tOwnerClassesFolderPath + "/" + WindowName + "/Editor/" + WindowName + ".psd";
            File.Copy(tIconPath, tIconPathNew);
            string tIconPathPro = NWDFindPackage.PathOfPackage() + "/NWDEditor/Editor/NWDExampleWindow_pro.psd";
            string tIconPathNewPro = tOwnerClassesFolderPath + "/" + WindowName + "/Editor/" + WindowName + "_pro.psd";
            File.Copy(tIconPathPro, tIconPathNewPro);
            AssetDatabase.ImportAsset(tIconPathNew);
            AssetDatabase.ImportAsset(tIconPathNewPro);

            // change meta
            TextureImporter tIconPathNewImporter = AssetImporter.GetAtPath(tIconPathNew) as TextureImporter;
            if (tIconPathNewImporter != null)
            {
                tIconPathNewImporter.textureType = TextureImporterType.GUI;
                tIconPathNewImporter.alphaSource = TextureImporterAlphaSource.FromInput;
                tIconPathNewImporter.alphaIsTransparency = true;
                //tIconPathNewImporter. // remove matte ?
                var tIconPathNewImporterSerialized = new SerializedObject(tIconPathNewImporter);
                if (tIconPathNewImporterSerialized != null)
                {
                    tIconPathNewImporterSerialized.FindProperty("m_PSDRemoveMatte").boolValue = true;
                    tIconPathNewImporterSerialized.FindProperty("m_PSDShowRemoveMatteOption").boolValue = true; // this is not needed unless you want to show the option (and warning)
                    tIconPathNewImporterSerialized.ApplyModifiedProperties();
                }
            }
            AssetDatabase.WriteImportSettingsIfDirty(tIconPathNew);

            // change meta pro
            TextureImporter tIconPathNewProImporter = AssetImporter.GetAtPath(tIconPathNewPro) as TextureImporter;
            if (tIconPathNewProImporter != null)
            {
                tIconPathNewProImporter.textureType = TextureImporterType.GUI;
                tIconPathNewProImporter.alphaSource = TextureImporterAlphaSource.FromInput;
                tIconPathNewProImporter.alphaIsTransparency = true;
                //tIconPathNewImporter. // remove matte ?
                var tIconPathNewProImporterSerialized = new SerializedObject(tIconPathNewProImporter);
                if (tIconPathNewProImporterSerialized != null)
                {
                    tIconPathNewProImporterSerialized.FindProperty("m_PSDRemoveMatte").boolValue = true;
                    tIconPathNewProImporterSerialized.FindProperty("m_PSDShowRemoveMatteOption").boolValue = true; // this is not needed unless you want to show the option (and warning)
                    tIconPathNewProImporterSerialized.ApplyModifiedProperties();
                }
            }
            AssetDatabase.WriteImportSettingsIfDirty(tIconPathNewPro);

            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Raises the enable event.
        /// </summary>
        public void OnEnable()
        {
            //NWDBenchmark.Start();
            TitleInit("Custom Window Manager", typeof(NWDEditorNewWindow));
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Raises the OnGUI event. Create the interface to enter a new class.
        /// </summary>
        public override void OnPreventGUI()
        {
            //NWDBenchmark.Start();
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
            WindowMacro = EditorGUILayout.TextField("Macro limit", WindowMacro);
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
            foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeList)
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
            if (tObject.Key == "" && tObject.Value == " ")
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
