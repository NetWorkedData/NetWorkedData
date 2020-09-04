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
    public partial class NWDEditorNewClassContent : NWDEditorWindowContent
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Scroll position in window
        /// </summary>
        private static Vector2 _kScrollPosition;
        //-------------------------------------------------------------------------------------------------------------
        const int K_TRIGRAM_MIN = 2;
        const int K_TRIGRAM_MAX = 6;
        const int K_MENU_MIN = 3;
        const int K_MENU_MAX = 24;
        const int K_CLASSNAME_MIN = 3;
        //-------------------------------------------------------------------------------------------------------------

        //bool ClassUnityEditorOnly = false;
        //bool ClassSynchronize = true;
        bool ClassUnityConnection = true;

        string ClassBase = "NWDBasis";
        /// <summary>
        /// The futur name of the class.
        /// </summary>
        string ClassName = string.Empty;
        /// <summary>
        /// The futur trigramme of the class.
        /// </summary>
        string ClassNameTrigramme = string.Empty;
        /// <summary>
        /// The futur class description.
        /// </summary>
        string ClassNameDescription = string.Empty;
        /// <summary>
        /// The futur menu name use for this class.
        /// </summary>
        string ClassNameMenuName = string.Empty;
        /// <summary>
        /// The class name properties list.
        /// </summary>
        List<KeyValuePair<string, string>> ClassNameProperties = new List<KeyValuePair<string, string>>();


        string MacroLimit = string.Empty;

        List<string> tListOfType = new List<string>();
        List<string> tListOfclass = new List<string>();
        NWDTemplateHelper TemplateHelper = new NWDTemplateHelper();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance for deamon class.
        /// </summary>
        private static NWDEditorNewClassContent _kSharedInstanceContent;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the <see cref="_kSharedInstanceContent"/> or instance one
        /// </summary>
        /// <returns></returns>
        public static NWDEditorNewClassContent SharedInstance()
        {
            NWDBenchmark.Start();
            if (_kSharedInstanceContent == null)
            {
                _kSharedInstanceContent = new NWDEditorNewClassContent();
            }
            NWDBenchmark.Finish();
            return _kSharedInstanceContent;
        }
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
                ClassBase = "NWDBasis";
            }
            // get the NWDExample code source
            string tClassExamplePath = NWDFindPackage.PathOfPackage() + "/NWDEditor/NWDObjects/NWDExample/NWDExample.cs";
            string tClassExample = File.ReadAllText(tClassExamplePath);
            // replace template by this params
            //if (ClassUnityEditorOnly == false)
            //{
            //    tClassExample = tClassExample.Replace("//["+typeof(NWDClassUnityEditorOnlyAttribute).Name+"]", "[" + typeof(NWDClassUnityEditorOnlyAttribute).Name + "]");
            //}
            //if (ClassSynchronize == false)
            //{
            //    tClassExample = tClassExample.Replace("[" +typeof(NWDClassServerSynchronizeAttribute).Name+"(true)]", "["+typeof(NWDClassServerSynchronizeAttribute).Name+"(false)]");
            //}
            Debug.Log("MacroLimit = " + MacroLimit);
            tClassExample = tClassExample.Replace("NWDExample_Tri", ClassNameTrigramme);
            tClassExample = tClassExample.Replace("NWDExample_Description", ClassNameDescription);
            tClassExample = tClassExample.Replace("NWDExample_MenuName", ClassNameMenuName);
            tClassExample = tClassExample.Replace("//#warning", "#warning");
            tClassExample = tClassExample.Replace("NWDExample", ClassName);
            tClassExample = tClassExample.Replace("NWDBasis", ClassBase);
            if (string.IsNullOrEmpty(MacroLimit) == false)
            {
                tClassExample = tClassExample.Replace("NWD_EXAMPLE_MACRO", MacroLimit);
                tClassExample = tClassExample.Replace("//MACRO_DEFINE ", "");
            }
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
            string tPropertiesLinearize = "//PROPERTIES\n\t\t//[NWDInspectorGroupReset]\n";
            foreach (KeyValuePair<string, string> tKeyValue in tPropertiesDico)
            {
                tPropertiesLinearize += "\t\tpublic " + tKeyValue.Value + " " + tKeyValue.Key + " {get; set;}\n";
            }
            tClassExample = tClassExample.Replace("//PROPERTIES", tPropertiesLinearize);
            // find the owner classes folder
            string tOwnerClassesFolderPath = NWDToolbox.FindOwnerClassesFolder() + "/" + ClassName;
            // create directories
            Directory.CreateDirectory(tOwnerClassesFolderPath);
            Directory.CreateDirectory(tOwnerClassesFolderPath + "/Editor");
            // write file basis
            string tFilePath = tOwnerClassesFolderPath + "/" + ClassName + ".cs";
            File.WriteAllText(tFilePath, tClassExample);
            AssetDatabase.ImportAsset(tFilePath);
            // write file connection with unity
            if (ClassUnityConnection == true)
            {
                GenerateFileConnection(ClassName, ClassBase, MacroLimit);
            }
            // write file workflow
            GenerateFileWorkflow(ClassName, ClassBase, MacroLimit);
            // write file helper
            GenerateFileHelper(ClassName, ClassBase, MacroLimit);
            // write file override
            GenerateFileOverride(ClassName, ClassBase, MacroLimit);
            // write file editor
            GenerateFileEditor(ClassName, ClassBase, MacroLimit);
            // write file index example
            GenerateFileIndex(ClassName, ClassBase, MacroLimit);
            // write file PHP extension
            GenerateFilePHP(ClassName, ClassBase, MacroLimit);
            // write icon to modify
            GenerateFileIcon(ClassName);
            // write file UnitTests
            GenerateFileUnitTest(ClassName, MacroLimit);
            // flush params

            //ClassName = string.Empty;
            //ClassNameTrigramme = string.Empty;
            //ClassNameDescription = string.Empty;
            //ClassNameMenuName = string.Empty;
            //MacroLimit = string.Empty;
            //ClassNameProperties = new List<KeyValuePair<string, string>>();

            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath(tFilePath);
            EditorGUIUtility.PingObject(Selection.activeObject);

            AssetDatabase.ImportAsset(tOwnerClassesFolderPath);
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
        public override void OnEnable(NWDEditorWindow sEditorWindow)
        {
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
            // Remove essentiel from normal proposition
            tListOfclass.Remove(typeof(NWDAccount).Name);
            tListOfclass.Remove(typeof(NWDAccountInfos).Name);
            tListOfclass.Remove(typeof(NWDAccountSign).Name);
            tListOfclass.Remove(typeof(NWDRequestToken).Name);
            tListOfclass.Remove(typeof(NWDIPBan).Name);
            tListOfclass.Remove(typeof(NWDVersion).Name);
            tListOfclass.Remove(typeof(NWDError).Name);
            tListOfclass.Remove(typeof(NWDExample).Name);
            tListOfclass.Remove(typeof(NWDBasisPreferences).Name);

            // Remove essentiel from normal proposition
            tListOfclass.Remove(typeof(NWDCluster).Name);
            tListOfclass.Remove(typeof(NWDServer).Name);
            tListOfclass.Remove(typeof(NWDServerDatas).Name);
            tListOfclass.Remove(typeof(NWDServerServices).Name);
            tListOfclass.Remove(typeof(NWDServerOther).Name);
            tListOfclass.Remove(typeof(NWDServerDomain).Name);

            // Add essentiel to normal proposition
            tListOfclass.Insert(0, "  ");
            tListOfclass.Insert(0, typeof(NWDBasisAccountUnsynchronize).Name);
            tListOfclass.Insert(0, typeof(NWDBasisUnsynchronize).Name);
            tListOfclass.Insert(0, typeof(NWDBasisAccountShared).Name);
            tListOfclass.Insert(0, typeof(NWDBasisGameSaveShared).Name);
            tListOfclass.Insert(0, typeof(NWDBasisAccountPublish).Name);
            tListOfclass.Insert(0, typeof(NWDBasisGameSavePublish).Name);
            tListOfclass.Insert(0, typeof(NWDBasisGameSaveDependent).Name);
            tListOfclass.Insert(0, typeof(NWDBasisAccountDependent).Name);
            //tListOfclass.Insert(0, typeof(NWDBasisAccountRestricted).Name); // not accessible to create Data in framework
            tListOfclass.Insert(0, typeof(NWDBasisBundled).Name);
            tListOfclass.Insert(0, typeof(NWDBasis).Name);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnDisable(NWDEditorWindow sEditorWindow)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///  On GUI drawing.
        /// </summary>
        public override void OnPreventGUI(Rect sRect)
        {
            base.OnPreventGUI(sRect);
            NWDBenchmark.Start();
            NWDGUILayout.Title("Custom class generator");
            //NWDGUILayout.Informations("Custom your class!");
            //NWDGUILayout.Line();
            _kScrollPosition = GUILayout.BeginScrollView(_kScrollPosition);
            //NWDGUILayout.HelpBox("Helper to create a new NWDBasis herited class. NWDBasis is the class of data in NetWorkedData framework.");

            //Prepare the form varaible 
            Regex tRegExpression = new Regex("[^a-zA-Z]");
            Regex tRegExpressionProperties = new Regex("[^a-zA-Z0-9]");
            Regex tRegExpressionEmptyType = new Regex("[ ]+");
            // validate the form ?
            bool tCanCreate = true;
            // futur class infos
            NWDGUILayout.Section("Class informations");
            //ClassUnityEditorOnly = EditorGUILayout.Toggle("Only for unity Editor", ClassUnityEditorOnly);
            //ClassSynchronize = EditorGUILayout.Toggle("Synchronize on servers", ClassSynchronize);
            int tBaseIndex = tListOfclass.IndexOf(ClassBase);
            tBaseIndex = EditorGUILayout.Popup("Base", tBaseIndex, tListOfclass.ToArray());
            if (tBaseIndex >= 0 && tBaseIndex < tListOfclass.Count)
            {
                ClassBase = tListOfclass[tBaseIndex];
            }
            TemplateHelper.SetClassType(Type.GetType("NetWorkedData." + ClassBase));
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.EnumPopup("Device database", TemplateHelper.GetDeviceDatabase());
            EditorGUILayout.EnumPopup("Synchronizable", TemplateHelper.GetSynchronizable());
            EditorGUILayout.EnumPopup("Bundlisable", TemplateHelper.GetBundlisable());
            EditorGUILayout.EnumPopup("Account dependent", TemplateHelper.GetAccountDependent());
            EditorGUILayout.EnumPopup("Gamesave dependent", TemplateHelper.GetGamesaveDependent());
            EditorGUI.EndDisabledGroup();

            ClassUnityConnection = EditorGUILayout.Toggle("Connect in GameObject", ClassUnityConnection);

            ClassName = EditorGUILayout.TextField("Name ", ClassName);
            ClassName = tRegExpression.Replace(ClassName, string.Empty);
            if (ClassName.Length < K_CLASSNAME_MIN)
            {
                EditorGUILayout.LabelField(" ", "name must be longer than " + K_CLASSNAME_MIN + " characters");
                tCanCreate = false;
            }
            else
            {
                foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeList)
                {
                    if (tType.Name == ClassName)
                    {
                        tCanCreate = false;
                    }
                }
                if (tCanCreate == false)
                {
                    EditorGUILayout.LabelField(" ", "this class already exists");
                }
                else
                {
                    EditorGUILayout.LabelField(" ", "class name is Ok!");
                }
            }
            ClassNameTrigramme = EditorGUILayout.TextField("Trigramme", ClassNameTrigramme);
            ClassNameTrigramme = tRegExpression.Replace(ClassNameTrigramme, string.Empty);
            ClassNameTrigramme = ClassNameTrigramme.ToUpper();
            if (ClassNameTrigramme.Length < K_TRIGRAM_MIN)
            {
                EditorGUILayout.LabelField(" ", "trigramme must be longer than " + K_TRIGRAM_MIN + " characters");
                tCanCreate = false;
            }
            else if (ClassNameTrigramme.Length > K_TRIGRAM_MAX)
            {
                EditorGUILayout.LabelField(" ", "trigramme must be shorter than " + K_TRIGRAM_MAX + " characters");
                tCanCreate = false;
            }
            else
            {
                //  but Trigramme already exists ?
                if (NWDDataManager.SharedInstance().ClassTrigramDictionary.ContainsKey(ClassNameTrigramme))
                {
                    tCanCreate = false;
                    EditorGUILayout.LabelField(" ", "trigramme already used by '" + NWDDataManager.SharedInstance().ClassTrigramDictionary[ClassNameTrigramme].Name + "'!");
                }
                else
                {
                    EditorGUILayout.LabelField(" ", "trigramme is Ok!");
                }
            }
            MacroLimit = EditorGUILayout.TextField("Macro limit", MacroLimit);
            NWDGUILayout.Section("Class description");
            ClassNameDescription = EditorGUILayout.TextField("Description", ClassNameDescription, GUILayout.Height(80.0F));
            ClassNameDescription = ClassNameDescription.Replace("\\", string.Empty);
            NWDGUILayout.Section("Menu in interface");
            // futur class menu name
            ClassNameMenuName = EditorGUILayout.TextField("Menu name", ClassNameMenuName);
            ClassNameMenuName = ClassNameMenuName.Replace("\\", string.Empty);
            if (ClassNameMenuName.Length < K_MENU_MIN)
            {
                EditorGUILayout.LabelField(" ", "menu name must be longer than " + K_MENU_MIN + " characters");
                tCanCreate = false;
            }
            else if (ClassNameMenuName.Length > K_MENU_MAX)
            {
                EditorGUILayout.LabelField(" ", "menu name must be shorter than " + K_MENU_MAX + " characters");
                tCanCreate = false;
            }
            else
            {
                EditorGUILayout.LabelField(" ", "menu name is Ok!");
            }
            NWDGUILayout.Section("Properties");
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
            tNextIndex = EditorGUILayout.Popup("New property", tNextIndex, tListOfType.ToArray());
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
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWD editor new class. Can create a new classes based on NWDExample automatically from the form generated in this editor window.
    /// </summary>
    public partial class NWDEditorNewClass : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string TutorialLink(string sLink = "")
        {
            return NWDConstants.K_NWDURL + "editor-new-class/";
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance.
        /// </summary>
        private static NWDEditorNewClass _kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDEditorNewClass SharedInstance()
        {
            //NWDBenchmark.Start();
            if (_kSharedInstance == null)
            {
                _kSharedInstance = EditorWindow.GetWindow(typeof(NWDEditorNewClass), ShowAsWindow()) as NWDEditorNewClass;
            }
            //NWDBenchmark.Finish();
            return _kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SharedInstanceFocus()
        {
            //NWDBenchmark.Start();
            //SharedInstance().ShowUtility();
            SharedInstance().ShowMe();
            SharedInstance().Focus();
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnEnable()
        {
            //NWDBenchmark.Start();
            // set normal size
            NormalizeWidth = 450;
            NormalizeHeight = 700;
            // set title
            TitleInit("Custom class", typeof(NWDEditorNewClass));
            NWDEditorNewClassContent.SharedInstance().OnEnable(this);
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
            NWDEditorNewClassContent.SharedInstance().OnPreventGUI(position);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
