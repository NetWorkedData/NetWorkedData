//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================

#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;

//=====================================================================================================================
namespace NetWorkedData.MacroDefine
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class MDEMacroDefineEditor : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        public static MDEMacroDefineEditor kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        private bool waiting = false;
        private GUIStyle TitleStyle;
        private GUIStyle ButtonStyle;
        Texture2D ImageLogo;
        GUIStyle Style;
        GUIStyle StyleBold;
        GUIStyle StyleItalic;
        GUIStyle StyleImage;
        GUIContent ImageContent;
        //-------------------------------------------------------------------------------------------------------------
        List<BuildTargetGroup> ActiveGroup = new List<BuildTargetGroup>();
        Dictionary<BuildTargetGroup, List<string>> ActiveGroupMacroDefine = new Dictionary<BuildTargetGroup, List<string>>();
        Dictionary<BuildTargetGroup, List<string>> ActiveGroupMacroDefineOriginal = new Dictionary<BuildTargetGroup, List<string>>();
        List<MDEDataTypeEnum> EnumTypeList = new List<MDEDataTypeEnum>();
        List<NWDMacroDefinerBool> BoolTypeList = new List<NWDMacroDefinerBool>();
        List<string> AllMacrosOriginal = new List<string>();
        string NewMacro = string.Empty;
        Vector2 ScroolPoint;
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(MDEConstants.Menu, false, 860)]
        public static void OpenSceneManager()
        {
            kSharedInstance = EditorWindow.GetWindow(typeof(MDEMacroDefineEditor)) as MDEMacroDefineEditor;
            kSharedInstance.Focus();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string UnixCleaner(string sString)
        {
            Regex rgx = new Regex("[^a-zA-Z0-9_]");
            return rgx.Replace(sString.ToUpper().Replace(" ", "_").Replace("-", "_"), string.Empty);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReadPref()
        {
            AllMacrosOriginal.Clear();
            if (File.Exists(MDEConstants.Path))
            {
                foreach (string tM in File.ReadAllText(MDEConstants.Path).Split(new char[] { ';' }))
                {
                    if (string.IsNullOrEmpty(tM) == false)
                    {
                        if (AllMacrosOriginal.Contains(tM) == false)
                        {
                            AllMacrosOriginal.Add(tM);
                        }
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void WritePref()
        {
            File.WriteAllText(MDEConstants.Path, ";" + string.Join(";", AllMacrosOriginal) + ";");
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Awake()
        {
            waiting = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Start()
        {
            waiting = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnEnable()
        {
            TitleInit(NWDConstants.K_APP_MACRO_DEFINE_TITLE, typeof(MDEMacroDefineEditor));
            Load();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Load()
        {
            waiting = false;
            //titleContent = new GUIContent(MDEConstants.MacroDefineEditor, AssetDatabase.LoadAssetAtPath<Texture2D>(MDEFindPackage.PathOfPackage("/Resources/Textures/MDEIcon.psd")));
            //ImageLogo = AssetDatabase.LoadAssetAtPath<Texture2D>(MDEFindPackage.PathOfPackage("/Resources/Textures/IdemobiIconAlpha.png"));
            ActiveGroup.Clear();
            ActiveGroupMacroDefine.Clear();
            ActiveGroupMacroDefineOriginal.Clear();
            NewMacro = string.Empty;
            ReadPref();
            // TODO Open file and read macro define

            Array BuildTargetArray = Enum.GetValues(typeof(BuildTarget));
            foreach (BuildTarget tBuildTarget in BuildTargetArray)
            {
                BuildTargetGroup tBuildTargetGroup = BuildPipeline.GetBuildTargetGroup(tBuildTarget);
                if (tBuildTargetGroup != BuildTargetGroup.Unknown)
                {
                    if (BuildPipeline.IsBuildTargetSupported(tBuildTargetGroup, tBuildTarget))
                    {
                        if (ActiveGroup.Contains(tBuildTargetGroup) == false)
                        {
                            ActiveGroup.Add(tBuildTargetGroup);
                            string tMacro = PlayerSettings.GetScriptingDefineSymbolsForGroup(tBuildTargetGroup);
                            List<string> tMacroList = new List<string>();
                            List<string> tMacroListOriginal = new List<string>();
                            foreach (string tM in tMacro.Split(new char[] { ';' }))
                            {
                                if (string.IsNullOrEmpty(tM) == false)
                                {
                                    if (tMacroList.Contains(tM) == false)
                                    {
                                        tMacroList.Add(tM);
                                    }
                                    if (tMacroListOriginal.Contains(tM) == false)
                                    {
                                        tMacroListOriginal.Add(tM);
                                    }
                                    if (AllMacrosOriginal.Contains(tM) == false)
                                    {
                                        AllMacrosOriginal.Add(tM);
                                    }
                                }
                            }
                            ActiveGroupMacroDefine.Add(tBuildTargetGroup, tMacroList);
                            ActiveGroupMacroDefineOriginal.Add(tBuildTargetGroup, tMacroListOriginal);
                        }
                    }
                }
            }

            EnumTypeList.Clear();
            BoolTypeList.Clear();
            List<Type> tTypeList = new List<Type>();
            // Find all Type of NWDType
            //BTBBenchmark.Start("Launcher() reflexion");
            Type[] tAllTypes = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            // sort and filter by NWDBasis (NWDTypeClass subclass)
            Type[] tAllEnumTypes = (from System.Type type in tAllTypes
                                    where type.IsSubclassOf(typeof(MDEDataTypeEnum))
                                    select type).ToArray();
            foreach (Type tType in tAllEnumTypes)
            {
                if (tType.IsGenericType == false)
                {
                    MDEDataTypeEnum tEnum = Activator.CreateInstance(tType, null) as MDEDataTypeEnum;
                    EnumTypeList.Add(tEnum);
                }
            }
            foreach (MDEDataTypeEnum tEnum in EnumTypeList)
            {
                List<string> tArrayMacro = tEnum.StringValuesArray();
                foreach (string tM in tArrayMacro)
                {
                    if (AllMacrosOriginal.Contains(tM))
                    {
                        AllMacrosOriginal.Remove(tM);
                    }
                }
            }
            Type[] tAllBoolTypes = (from System.Type type in tAllTypes
                                    where type.IsSubclassOf(typeof(NWDMacroDefinerBool))
                                    select type).ToArray();
            foreach (Type tType in tAllBoolTypes)
            {
                if (tType.IsGenericType == false)
                {
                    NWDMacroDefinerBool tBool = Activator.CreateInstance(tType, null) as NWDMacroDefinerBool;
                    BoolTypeList.Add(tBool);
                }
            }
            foreach (NWDMacroDefinerBool tBool in BoolTypeList)
            {
                List<string> tArrayMacro = tBool.StringValuesArray();
                foreach (string tM in tArrayMacro)
                {
                    if (AllMacrosOriginal.Contains(tM))
                    {
                        AllMacrosOriginal.Remove(tM);
                    }
                }
            }
            AllMacrosOriginal.Remove(MDEMacroDefine.kMacro);
            AllMacrosOriginal.Sort();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void DefineGUI()
        {
            if (TitleStyle == null)
            {
                TitleStyle = new GUIStyle(EditorStyles.boldLabel);
                TitleStyle.alignment = TextAnchor.MiddleCenter;

                ButtonStyle = new GUIStyle(EditorStyles.miniButton);

                Style = new GUIStyle(GUI.skin.label);
                Style.richText = true;
                Style.wordWrap = true;
                Style.alignment = TextAnchor.MiddleCenter;

                StyleBold = new GUIStyle(GUI.skin.label);
                StyleBold.richText = true;
                StyleBold.wordWrap = true;
                StyleBold.fontStyle = FontStyle.Bold;
                StyleBold.alignment = TextAnchor.MiddleCenter;

                StyleItalic = new GUIStyle(GUI.skin.label);
                StyleItalic.richText = true;
                StyleItalic.wordWrap = true;
                StyleItalic.fontStyle = FontStyle.Italic;

                StyleImage = new GUIStyle(GUI.skin.label);
                StyleImage.imagePosition = ImagePosition.ImageOnly;
                StyleImage.alignment = TextAnchor.MiddleCenter;

                ImageContent = new GUIContent(ImageLogo);

            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnPreventGUI()
        {
            DefineGUI();

            NWDGUILayout.Title("Macro Define for project");
            //if (waiting == false)
            //{
            BuildTargetGroup tBuildTargetGroupActiveNow = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
            Color tBackground = GUI.backgroundColor;
            bool tReload = false;
            ScroolPoint = GUILayout.BeginScrollView(ScroolPoint);
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical();
            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(MDEConstants.ManagementWidth));
            GUILayout.Label(MDEConstants.Management, TitleStyle);
            GUILayout.Space(10.0F);
            GUILayout.EndVertical();
            GUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.Width(MDEConstants.ManagementWidth));
            foreach (string tM in AllMacrosOriginal)
            {
                if (GUILayout.Button(MDEConstants.Remove, ButtonStyle, GUILayout.Height(MDEConstants.RowHeight)))
                {
                    AllMacrosOriginal.Remove(tM);
                    //WritePref();
                    GUIUtility.ExitGUI();
                }
            }
            GUILayout.Space(10.0F);
            GUILayout.EndVertical();
            GUILayout.EndVertical();
            foreach (BuildTargetGroup tBuildTargetGroup in ActiveGroup)
            {
                GUILayout.BeginVertical();
                if (tBuildTargetGroupActiveNow == tBuildTargetGroup)
                {
                    GUI.backgroundColor = Color.red;
                }
                GUILayout.BeginVertical(EditorStyles.helpBox);
                GUILayout.Label(tBuildTargetGroup.ToString(), TitleStyle);
                GUILayout.Space(10.0F);
                GUILayout.EndVertical();
                GUILayout.BeginVertical(EditorStyles.helpBox);
                if (ActiveGroupMacroDefine.ContainsKey(tBuildTargetGroup))
                {
                    foreach (string tM in AllMacrosOriginal)
                    {
                        bool tResult = ActiveGroupMacroDefine[tBuildTargetGroup].Contains(tM);
                        tResult = GUILayout.Toggle(tResult, tM, GUILayout.Height(MDEConstants.RowHeight));
                        if (tResult == true)
                        {
                            if (ActiveGroupMacroDefine[tBuildTargetGroup].Contains(tM) == false)
                            {
                                ActiveGroupMacroDefine[tBuildTargetGroup].Add(tM);
                            }
                        }
                        else
                        {
                            if (ActiveGroupMacroDefine[tBuildTargetGroup].Contains(tM) == true)
                            {
                                ActiveGroupMacroDefine[tBuildTargetGroup].Remove(tM);
                            }
                        }
                    }
                    GUILayout.Space(10.0F);
                    GUILayout.EndVertical();

                    if (EnumTypeList.Count > 0)
                    {
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                        GUILayout.Label(MDEConstants.EnumArea, TitleStyle);
                        foreach (MDEDataTypeEnum tEnum in EnumTypeList)
                        {
                            List<string> tArrayMacro = tEnum.StringValuesArray();
                            List<string> tArrayRepresentation = tEnum.RepresentationValuesArray();
                            int tIndex = 0;
                            List<string> tToRemove = new List<string>();
                            foreach (string tMM in tArrayMacro)
                            {
                                if (ActiveGroupMacroDefine[tBuildTargetGroup].Contains(tMM))
                                {
                                    tToRemove.Add(tMM);
                                    tIndex = tArrayMacro.IndexOf(tMM);
                                }
                            }
                            foreach (string tR in tToRemove)
                            {
                                ActiveGroupMacroDefine[tBuildTargetGroup].Remove(tR);
                            }
                            //GUILayout.Label(tEnum.GetTitle());
                            tIndex = EditorGUILayout.Popup(tEnum.GetTitle(), tIndex, tArrayRepresentation.ToArray());
                            if (tIndex >= 0)
                            {
                                if (tArrayMacro[tIndex] != MDEConstants.NONE)
                                {
                                    ActiveGroupMacroDefine[tBuildTargetGroup].Add(tArrayMacro[tIndex]);
                                }
                            }
                            GUILayout.Space(5.0F);
                        }
                    GUILayout.EndVertical();
                    }
                    if (BoolTypeList.Count > 0)
                    {
                    GUILayout.BeginVertical(EditorStyles.helpBox);
                        GUILayout.Label(MDEConstants.BoolArea, TitleStyle);

                        foreach (NWDMacroDefinerBool tBool in BoolTypeList)
                        {
                            List<string> tArrayMacro = tBool.StringValuesArray();
                            if (tArrayMacro.Count == 2)
                            {
                                int tIndex = 0;
                                List<string> tToRemove = new List<string>();
                                foreach (string tMM in tArrayMacro)
                                {
                                    if (ActiveGroupMacroDefine[tBuildTargetGroup].Contains(tMM))
                                    {
                                        tToRemove.Add(tMM);
                                        tIndex = tArrayMacro.IndexOf(tMM);
                                    }
                                }
                                foreach (string tR in tToRemove)
                                {
                                    ActiveGroupMacroDefine[tBuildTargetGroup].Remove(tR);
                                }
                                //GUILayout.Label(tBool.GetTitle());
                                bool tSelected = false;
                                if (tIndex == 1)
                                {
                                    tSelected = true;
                                }
                                tSelected = EditorGUILayout.Toggle(new GUIContent(tBool.GetTitle(), tArrayMacro[1]), tSelected);
                                if (tSelected == false)
                                {
                                    tIndex = 0;
                                }
                                else
                                {
                                    tIndex = 1;
                                }
                                if (tIndex >= 0)
                                {
                                    if (tArrayMacro[tIndex] != MDEConstants.NONE)
                                    {
                                        ActiveGroupMacroDefine[tBuildTargetGroup].Add(tArrayMacro[tIndex]);
                                    }
                                }
                                GUILayout.Space(5.0F);
                            }
                        }
                        GUILayout.EndVertical();
                    }
                }
                else
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label(MDEConstants.ErrorInLoading, TitleStyle);
                    if (GUILayout.Button(MDEConstants.Reload))
                    {
                        tReload = true;
                    }
                    GUILayout.FlexibleSpace();
                }

                // add owner macro
                if (ActiveGroupMacroDefine[tBuildTargetGroup].Contains(MDEMacroDefine.kMacro) == false)
                {
                    ActiveGroupMacroDefine[tBuildTargetGroup].Add(MDEMacroDefine.kMacro);
                }
                ActiveGroupMacroDefine[tBuildTargetGroup].Sort();
                // for debug
                GUILayout.BeginVertical(EditorStyles.helpBox);
                GUILayout.Label(MDEConstants.Result, TitleStyle);
                GUILayout.TextArea(string.Join("; ", ActiveGroupMacroDefine[tBuildTargetGroup]));
                GUILayout.EndVertical();
                GUILayout.EndVertical();
                GUI.backgroundColor = tBackground;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
            if (GUILayout.Button(MDEConstants.Reload))
            {
                Load();
            }



            NWDGUILayout.Section("Macro Define informations");
            NWDGUILayout.Informations("If you want create new macro, use generic from "+typeof(MDEDataTypeEnumGeneric<>).Name+ " or " + typeof(MDEDataTypeBoolGeneric<>).Name + "!");

            NWDGUILayout.Section("Macro add");

            GUILayout.Space(10.0F);
            GUILayout.Label(MDEConstants.NewMacroArea, EditorStyles.boldLabel);
            NewMacro = EditorGUILayout.TextField(MDEConstants.NewMacro, NewMacro);
            NewMacro = NewMacro.ToUpper();
            NewMacro = NewMacro.Replace(";", "");
            NewMacro = NewMacro.Replace(" ", "_");
            NewMacro = NewMacro.Replace("-", "_");
            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(NewMacro));
            if (GUILayout.Button(MDEConstants.NewMacroButton))
            {
                if (AllMacrosOriginal.Contains(NewMacro) == false)
                {
                    AllMacrosOriginal.Add(UnixCleaner(NewMacro));
                    AllMacrosOriginal.Sort();
                }
                //WritePref();
                NewMacro = string.Empty;
            }
            EditorGUI.EndDisabledGroup();

            NWDGUILayout.Section("Macro Define save");

            GUILayout.Label(MDEConstants.SaveArea, EditorStyles.boldLabel);
            bool tCheckModified = false;

            foreach (BuildTargetGroup tBuildTargetGroup in ActiveGroup)
            {
                if (ActiveGroupMacroDefine.ContainsKey(tBuildTargetGroup))
                {
                    foreach (string tNewMacro in ActiveGroupMacroDefine[tBuildTargetGroup])
                    {
                        if (ActiveGroupMacroDefineOriginal[tBuildTargetGroup].Contains(tNewMacro) == false)
                        {
                            tCheckModified = true;
                            break;
                        }
                    }
                    if (waiting == false)
                    {
                        foreach (string tOldMacro in ActiveGroupMacroDefineOriginal[tBuildTargetGroup])
                        {
                            if (ActiveGroupMacroDefine[tBuildTargetGroup].Contains(tOldMacro) == false)
                            {
                                tCheckModified = true;
                                break;
                            }
                        }
                    }
                }
            }
            NWDGUI.BeginRedArea();
            EditorGUI.BeginDisabledGroup(!tCheckModified);
            //if (GUILayout.Button(MDEConstants.Save))
            if (GUILayout.Button(NWDConstants.K_APP_CONFIGURATION_SAVE_BUTTON))
            {
                if (EditorUtility.DisplayDialog(MDEConstants.AlertTitle, MDEConstants.AlertMessage, MDEConstants.AlertOK, MDEConstants.AlertCancel))
                {
                    waiting = false;
                    WritePref();
                    foreach (BuildTargetGroup tBuildTargetGroupSet in ActiveGroup)
                    {
                        string tSymbos = string.Join(";", ActiveGroupMacroDefine[tBuildTargetGroupSet]);
                        //Debug.Log("SetScriptingDefineSymbolsForGroup " + tBuildTargetGroupSet.ToString() + " " + tSymbos);
                        PlayerSettings.SetScriptingDefineSymbolsForGroup(tBuildTargetGroupSet, tSymbos);
                    }
                }
                NWDEditorWindow.GenerateCSharpFile();
            }
            EditorGUI.EndDisabledGroup();
            NWDGUI.EndRedArea();

            if (tReload)
            {
                Load();
            }
            //}
            //else
            //{
            //    GUILayout.FlexibleSpace();
            //    GUILayout.Label("... recompile in progress ...", TitleStyle);
            //    GUILayout.FlexibleSpace();
            //    AssetDatabase.Refresh();
            //}
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
