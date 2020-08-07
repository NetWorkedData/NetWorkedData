//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
//#define NWD_LOG
//#define NWD_BENCHMARK
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
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
//=====================================================================================================================
using UnityEditor;
using NetWorkedData.NWDEditor;
//=====================================================================================================================
namespace NetWorkedData.MacroDefine
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class MDEMacroDefineEditor : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        List<BuildTargetGroup> ActiveGroup = new List<BuildTargetGroup>();
        Dictionary<BuildTargetGroup, List<string>> ActiveGroupMacroDefine = new Dictionary<BuildTargetGroup, List<string>>();
        Dictionary<BuildTargetGroup, List<string>> ActiveGroupMacroDefineOriginal = new Dictionary<BuildTargetGroup, List<string>>();
        List<MDEDataTypeEnum> EnumTypeList = new List<MDEDataTypeEnum>();
        List<NWDMacroDefinerBool> BoolTypeList = new List<NWDMacroDefinerBool>();
        List<string> AllMacrosOriginal = new List<string>();
        string NewMacro = string.Empty;
        Vector2 ScroolPoint;
        bool Modified = false;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance.
        /// </summary>
        private static MDEMacroDefineEditor kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the SharedInstance or instance one
        /// </summary>
        /// <returns></returns>
        public static MDEMacroDefineEditor SharedInstance()
        {
            //NWDBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(MDEMacroDefineEditor)) as MDEMacroDefineEditor;
            }
            //NWDBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Show the SharedInstance of Editor Configuration Manager Window and focus on.
        /// </summary>
        /// <returns></returns>
        public static MDEMacroDefineEditor SharedInstanceFocus()
        {
            //NWDBenchmark.Start();
            SharedInstance().ShowUtility();
            SharedInstance().Focus();
            //NWDBenchmark.Finish();
            return kSharedInstance;
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
            //waiting = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Start()
        {
            //waiting = false;
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
            Modified = false;
            ActiveGroup.Clear();
            ActiveGroupMacroDefine.Clear();
            ActiveGroupMacroDefineOriginal.Clear();
            NewMacro = string.Empty;
            ReadPref();
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
            AllMacrosOriginal.Sort();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnPreventGUI()
        {
            NWDGUILayout.Title("Macro Define for project");
            BuildTargetGroup tBuildTargetGroupActiveNow = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
            bool tReload = false;
            ScroolPoint = GUILayout.BeginScrollView(ScroolPoint);
            GUILayout.Space(10.0F);
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.MinWidth(MDEConstants.ManagementWidth), GUILayout.ExpandWidth(true));
            GUILayout.BeginVertical();
            GUILayout.Label(MDEConstants.Management, NWDGUI.KTableHeaderStatut);
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            foreach (string tM in AllMacrosOriginal)
            {
                if (GUILayout.Button(MDEConstants.Remove, GUILayout.Height(MDEConstants.RowHeight)))
                {
                    AllMacrosOriginal.Remove(tM);
                    foreach (BuildTargetGroup tBuildTargetGroup in ActiveGroup)
                    {
                        if (ActiveGroupMacroDefine[tBuildTargetGroup].Contains(tM) == true)
                        {
                            ActiveGroupMacroDefine[tBuildTargetGroup].Remove(tM);
                        }
                    }
                    Modified = true;
                    GUIUtility.ExitGUI();
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndVertical();
            foreach (BuildTargetGroup tBuildTargetGroup in ActiveGroup)
            {
                GUILayout.BeginVertical(GUILayout.MaxWidth(260));
                if (tBuildTargetGroupActiveNow == tBuildTargetGroup)
                {
                    NWDGUI.BeginRedArea();
                }
                GUILayout.BeginVertical();
                GUILayout.Label(tBuildTargetGroup.ToString(), NWDGUI.KTableHeaderStatut);
                GUILayout.EndVertical();
                GUILayout.BeginVertical();
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
                    GUILayout.EndVertical();
                    if (EnumTypeList.Count > 0)
                    {
                        GUILayout.BeginVertical();
                        GUILayout.Label(MDEConstants.EnumArea, NWDGUI.KTableHeaderStatut);
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
                        GUILayout.BeginVertical();
                        GUILayout.Label(MDEConstants.BoolArea, NWDGUI.KTableHeaderStatut);

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
                    GUILayout.Label(MDEConstants.ErrorInLoading, NWDGUI.KTableHeaderStatut);
                    if (GUILayout.Button(MDEConstants.Reload))
                    {
                        tReload = true;
                    }
                    GUILayout.FlexibleSpace();
                }
                ActiveGroupMacroDefine[tBuildTargetGroup].Sort();
                GUILayout.BeginVertical(EditorStyles.helpBox);
                GUILayout.Label(MDEConstants.Result, NWDGUI.kInspectorReferenceCenter);
                GUILayout.TextArea(string.Join(";\n", ActiveGroupMacroDefine[tBuildTargetGroup]));
                GUILayout.EndVertical();
                GUILayout.EndVertical();
                NWDGUI.EndRedArea();
            }
            GUILayout.EndHorizontal();
            GUILayout.EndScrollView();
            if (GUILayout.Button(MDEConstants.Reload))
            {
                tReload = true;
                //Load();
            }
            NWDGUILayout.Section("Macro Define informations");
            NWDGUILayout.Informations("If you want create new macro, use generic from " + typeof(MDEDataTypeEnumGeneric<>).Name + " or " + typeof(MDEDataTypeBoolGeneric<>).Name + "!");
            NWDGUILayout.Section("Macro add");
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
                NewMacro = string.Empty;
            }
            EditorGUI.EndDisabledGroup();
            NWDGUILayout.Section("Macro Define save");
            GUILayout.Label(MDEConstants.SaveArea, EditorStyles.boldLabel);
            foreach (BuildTargetGroup tBuildTargetGroup in ActiveGroup)
            {
                if (ActiveGroupMacroDefine.ContainsKey(tBuildTargetGroup))
                {
                    foreach (string tNewMacro in ActiveGroupMacroDefine[tBuildTargetGroup])
                    {
                        if (ActiveGroupMacroDefineOriginal[tBuildTargetGroup].Contains(tNewMacro) == false)
                        {
                            Modified = true;
                            break;
                        }
                    }
                    foreach (string tOldMacro in ActiveGroupMacroDefineOriginal[tBuildTargetGroup])
                    {
                        if (ActiveGroupMacroDefine[tBuildTargetGroup].Contains(tOldMacro) == false)
                        {
                            Modified = true;
                            break;
                        }
                    }
                }
            }
            NWDGUI.BeginRedArea();
            EditorGUI.BeginDisabledGroup(!Modified);
            if (GUILayout.Button(NWDConstants.K_APP_CONFIGURATION_SAVE_BUTTON))
            {
                if (EditorUtility.DisplayDialog(MDEConstants.AlertTitle, MDEConstants.AlertMessage, MDEConstants.AlertOK, MDEConstants.AlertCancel))
                {
                    WritePref();
                    foreach (BuildTargetGroup tBuildTargetGroupSet in ActiveGroup)
                    {
                        string tSymbos = string.Join(";", ActiveGroupMacroDefine[tBuildTargetGroupSet]);
                        PlayerSettings.SetScriptingDefineSymbolsForGroup(tBuildTargetGroupSet, tSymbos);
                    }
                    Modified = false;
                }
                NWDEditorWindow.GenerateCSharpFile();
            }
            EditorGUI.EndDisabledGroup();
            NWDGUI.EndRedArea();
            if (tReload)
            {
                Load();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
