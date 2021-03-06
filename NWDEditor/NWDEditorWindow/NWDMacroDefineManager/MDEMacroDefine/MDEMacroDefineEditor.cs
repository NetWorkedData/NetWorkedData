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
    public class MDEMacroDefineEditorContent : NWDEditorWindowContent
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Scroll position in window
        /// </summary>
        private static Vector2 _kScrollPosition;
        //-------------------------------------------------------------------------------------------------------------
        List<BuildTargetGroup> ActiveGroup = new List<BuildTargetGroup>();
        Dictionary<BuildTargetGroup, List<string>> ActiveGroupMacroDefine = new Dictionary<BuildTargetGroup, List<string>>();
        Dictionary<BuildTargetGroup, List<string>> ActiveGroupMacroDefineOriginal = new Dictionary<BuildTargetGroup, List<string>>();
        List<NWDMacroDefiner> EnumTypeList = new List<NWDMacroDefiner>();
        List<NWDMacroDefiner> BoolTypeList = new List<NWDMacroDefiner>();
        Dictionary<string, List<NWDMacroDefiner>> MacroTypeList = new Dictionary<string, List<NWDMacroDefiner>>();
        List<string> AllMacrosOriginal = new List<string>();
        string NewMacro = string.Empty;
        Vector2 ScroolPoint;
        bool Modified = false;

        public List<string> AllMacros = new List<string>();
        Dictionary<NWDMacroDefiner, int> EnumTypeListGloball = new Dictionary<NWDMacroDefiner, int>();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance for deamon class.
        /// </summary>
        private static MDEMacroDefineEditorContent _kSharedInstanceContent;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the <see cref="_kSharedInstanceContent"/> or instance one
        /// </summary>
        /// <returns></returns>
        public static MDEMacroDefineEditorContent SharedInstance()
        {
            //NWDBenchmark.Start();
            if (_kSharedInstanceContent == null)
            {
                _kSharedInstanceContent = new MDEMacroDefineEditorContent();
            }
            //NWDBenchmark.Finish();
            return _kSharedInstanceContent;
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
        public void Load()
        {
            AllMacros.Clear();
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
                                    if (AllMacros.Contains(tM) == false)
                                    {
                                        AllMacros.Add(tM);
                                    }
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
            MacroTypeList.Clear();
            List<Type> tTypeList = new List<Type>();
            // Find all Type of NWDType
            Type[] tAllTypes = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
            // sort and filter by NWDBasis (NWDTypeClass subclass)
            Type[] tAllEnumTypes = (from System.Type type in tAllTypes
                                    where type.IsSubclassOf(typeof(NWDMacroDefiner))
                                    select type).ToArray();
            foreach (Type tType in tAllEnumTypes)
            {
                //Debug.Log("test " + tType.Name + " base = " + tType.BaseType.Name);
                if (tType.IsGenericType == false)
                {
                    //Debug.Log("test " + tType.Name + " is not genereic");
                    if (tType.IsSubclassOf(typeof(NWDMacroEnumDefiner)))
                    {
                        NWDMacroDefiner tEnum = Activator.CreateInstance(tType, null) as NWDMacroDefiner;
                        NWDMacroEnumDefiner tEnumAs = tEnum as NWDMacroEnumDefiner;
                        foreach (string tM in tEnumAs.StringValuesArray())
                        {
                            if (AllMacros.Contains(tM) == false)
                            {
                                AllMacros.Add(tM);
                            }
                        }
                        foreach (string tM in tEnumAs.StringValuesArrayAdd())
                        {
                            if (AllMacros.Contains(tM) == false)
                            {
                                AllMacros.Add(tM);
                            }
                        }
                        string tGroup = tEnum.GetGroup();
                        if (string.IsNullOrEmpty(tGroup))
                        {
                            EnumTypeList.Add(tEnum);
                        }
                        else
                        {
                            if (MacroTypeList.ContainsKey(tGroup) == false)
                            {
                                MacroTypeList.Add(tGroup, new List<NWDMacroDefiner>());
                            }
                            MacroTypeList[tGroup].Add(tEnum);
                            MacroTypeList[tGroup].Sort((tA, tB) => tA.GetOrder().CompareTo(tB.GetOrder()));
                        }
                        List<string> tArrayMacro = tEnum.StringValuesArray();
                        foreach (string tM in tArrayMacro)
                        {
                            if (AllMacrosOriginal.Contains(tM))
                            {
                                AllMacrosOriginal.Remove(tM);
                            }
                        }
                    }
                    if (tType.IsSubclassOf(typeof(NWDMacroBoolDefiner)))
                    {
                        NWDMacroDefiner tBool = Activator.CreateInstance(tType, null) as NWDMacroDefiner;
                        NWDMacroBoolDefiner tEnumAs = tBool as NWDMacroBoolDefiner;
                        foreach (string tM in tEnumAs.StringValuesArray())
                        {
                            if (AllMacros.Contains(tM) == false)
                            {
                                AllMacros.Add(tM);
                            }
                        }
                        foreach (string tM in tEnumAs.StringValuesArrayAdd())
                        {
                            if (AllMacros.Contains(tM) == false)
                            {
                                AllMacros.Add(tM);
                            }
                        }
                        string tGroup = tBool.GetGroup();
                        if (string.IsNullOrEmpty(tGroup))
                        {
                            BoolTypeList.Add(tBool);
                        }
                        else
                        {
                            if (MacroTypeList.ContainsKey(tGroup) == false)
                            {
                                MacroTypeList.Add(tGroup, new List<NWDMacroDefiner>());
                            }
                            MacroTypeList[tGroup].Add(tBool);
                            MacroTypeList[tGroup].Sort((tA, tB) => tA.GetOrder().CompareTo(tB.GetOrder()));
                        }
                        List<string> tArrayMacro = tBool.StringValuesArray();
                        foreach (string tM in tArrayMacro)
                        {
                            if (AllMacrosOriginal.Contains(tM))
                            {
                                AllMacrosOriginal.Remove(tM);
                            }
                        }
                    }
                }
            }
            AllMacrosOriginal.Sort();
            AllMacros.Sort();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnEnable(NWDEditorWindow sEditorWindow)
        {
            Load();
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
            //NWDBenchmark.Start();
            NWDGUILayout.Title("Macro Define for project");
            BuildTargetGroup tBuildTargetGroupActiveNow = BuildPipeline.GetBuildTargetGroup(EditorUserBuildSettings.activeBuildTarget);
            //bool tReload = false;
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

                GUILayout.BeginHorizontal(GUILayout.Height(MDEConstants.RowHeight));

                GUILayout.Label(tM, GUILayout.Width(EditorGUIUtility.labelWidth));
                if (GUILayout.Button(MDEConstants.Remove))
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

                if (GUILayout.Button(MDEConstants.EnableAll))
                {
                    Modified = ChangeThisMacroAll(tM, true);
                }
                if (GUILayout.Button(MDEConstants.DisableAll))
                {
                    Modified = ChangeThisMacroAll(tM, false);
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndVertical();




















            if (MacroTypeList.Count > 0)
            {
                foreach (KeyValuePair<string, List<NWDMacroDefiner>> tKeyValue in MacroTypeList)
                {
                    GUILayout.BeginVertical();
                    GUILayout.Label(tKeyValue.Key, NWDGUI.KTableHeaderStatut);
                    foreach (NWDMacroDefiner tMacro in tKeyValue.Value)
                    {
                        if (tMacro.GetType().IsSubclassOf(typeof(NWDMacroEnumDefiner)))
                        {
                            GUILayout.BeginHorizontal(GUILayout.Height(MDEConstants.RowHeight));
                            List<string> tArrayMacro = tMacro.StringValuesArray();
                            List<string> tArrayMacroAdd = tMacro.StringValuesArrayAdd();
                            List<string> tArrayRepresentation = tMacro.RepresentationValuesArray();
                            if (EnumTypeListGloball.ContainsKey(tMacro) == false)
                            {
                                EnumTypeListGloball.Add(tMacro, 0);
                            }
                            int tIndex = EnumTypeListGloball[tMacro];
                            tIndex = EditorGUILayout.Popup(tMacro.GetTitle(), tIndex, tArrayRepresentation.ToArray());
                            EnumTypeListGloball[tMacro] = tIndex;
                            if (GUILayout.Button(MDEConstants.ChangeAll, GUILayout.Height(NWDGUI.kPopupStyle.fixedHeight)))
                            {
                                foreach (string tMM in tArrayMacro)
                                {
                                    ChangeThisMacroAll(tArrayMacro[1], false);
                                }
                                if (tIndex >= 0)
                                {
                                    //if (tArrayMacro[tIndex] != MDEConstants.NONE)
                                    {
                                        ChangeThisMacroAll(tArrayMacro[tIndex], true);
                                        string[] sList = tArrayMacroAdd[tIndex].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (string tAdd in sList)
                                        {
                                            ChangeThisMacroAll(tAdd, true);
                                        }
                                    }
                                }
                            }
                            GUILayout.EndHorizontal();
                            //GUILayout.Space(5.0F);
                        }
                        if (tMacro.GetType().IsSubclassOf(typeof(NWDMacroBoolDefiner)))
                        {
                            List<string> tArrayMacro = tMacro.StringValuesArray();
                            List<string> tArrayMacroAdd = tMacro.StringValuesArrayAdd();
                            if (tArrayMacro.Count == 2)
                            {
                                GUILayout.BeginHorizontal(GUILayout.Height(MDEConstants.RowHeight));
                                GUILayout.Label(tMacro.GetTitle(), GUILayout.Width(EditorGUIUtility.labelWidth));
                                if (GUILayout.Button(MDEConstants.EnableAll))
                                {
                                    ChangeThisMacroAll(tArrayMacro[1], true);
                                    string[] sList = tArrayMacroAdd[1].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                    foreach (string tAdd in sList)
                                    {
                                        ChangeThisMacroAll(tAdd, true);
                                    }
                                }
                                if (GUILayout.Button(MDEConstants.DisableAll))
                                {
                                    ChangeThisMacroAll(tArrayMacro[1], false);
                                }
                                GUILayout.EndHorizontal();
                                //GUILayout.Space(7.0F);
                            }
                        }
                    }
                    GUILayout.EndVertical();
                }
            }

            if (EnumTypeList.Count > 0)
            {
                GUILayout.BeginVertical();
                GUILayout.Label(MDEConstants.EnumArea, NWDGUI.KTableHeaderStatut);
                EnumTypeList.Sort((tA, tB) => string.Compare(tA.GetTitle(), tB.GetTitle(), StringComparison.Ordinal));
                foreach (NWDMacroDefiner tEnum in EnumTypeList)
                {
                    GUILayout.BeginHorizontal(GUILayout.Height(MDEConstants.RowHeight));
                    List<string> tArrayMacro = tEnum.StringValuesArray();
                    List<string> tArrayMacroAdd = tEnum.StringValuesArrayAdd();
                    List<string> tArrayRepresentation = tEnum.RepresentationValuesArray();
                    if (EnumTypeListGloball.ContainsKey(tEnum) == false)
                    {
                        EnumTypeListGloball.Add(tEnum, 0);
                    }
                    int tIndex = EnumTypeListGloball[tEnum];
                    tIndex = EditorGUILayout.Popup(tEnum.GetTitle(), tIndex, tArrayRepresentation.ToArray());
                    EnumTypeListGloball[tEnum] = tIndex;
                    if (GUILayout.Button(MDEConstants.ChangeAll, GUILayout.Height(NWDGUI.kPopupStyle.fixedHeight)))
                    {
                        foreach (string tMM in tArrayMacro)
                        {
                            ChangeThisMacroAll(tArrayMacro[1], false);
                        }
                        if (tIndex >= 0)
                        {
                            //if (tArrayMacro[tIndex] != MDEConstants.NONE)
                            {
                                ChangeThisMacroAll(tArrayMacro[tIndex], true);
                                string[] sList = tArrayMacroAdd[tIndex].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                foreach (string tAdd in sList)
                                {
                                    ChangeThisMacroAll(tAdd, true);
                                }
                            }
                        }
                    }
                    GUILayout.EndHorizontal();
                    //GUILayout.Space(5.0F);
                }
                GUILayout.EndVertical();
            }
            if (BoolTypeList.Count > 0)
            {
                GUILayout.BeginVertical();
                GUILayout.Label(MDEConstants.BoolArea, NWDGUI.KTableHeaderStatut);
                BoolTypeList.Sort((tA, tB) => string.Compare(tA.GetTitle(), tB.GetTitle(), StringComparison.Ordinal));
                foreach (NWDMacroDefiner tBool in BoolTypeList)
                {
                    List<string> tArrayMacro = tBool.StringValuesArray();
                    List<string> tArrayMacroAdd = tBool.StringValuesArrayAdd();
                    if (tArrayMacro.Count == 2)
                    {
                        GUILayout.BeginHorizontal(GUILayout.Height(MDEConstants.RowHeight));
                        GUILayout.Label(tBool.GetTitle(), GUILayout.Width(EditorGUIUtility.labelWidth));
                        if (GUILayout.Button(MDEConstants.EnableAll))
                        {
                            ChangeThisMacroAll(tArrayMacro[1], true);
                            string[] sList = tArrayMacroAdd[1].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                            foreach (string tAdd in sList)
                            {
                                ChangeThisMacroAll(tAdd, true);
                            }
                        }
                        if (GUILayout.Button(MDEConstants.DisableAll))
                        {
                            ChangeThisMacroAll(tArrayMacro[1], false);
                        }
                        GUILayout.EndHorizontal();
                        //GUILayout.Space(7.0F);
                    }
                }
                GUILayout.EndVertical();
            }
















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

                    if (MacroTypeList.Count > 0)
                    {
                        foreach (KeyValuePair<string, List<NWDMacroDefiner>> tKeyValue in MacroTypeList)
                        {
                            GUILayout.BeginVertical();
                            GUILayout.Label(tKeyValue.Key, NWDGUI.KTableHeaderStatut);
                            foreach (NWDMacroDefiner tMacro in tKeyValue.Value)
                            {
                                if (tMacro.GetType().IsSubclassOf(typeof(NWDMacroEnumDefiner)))
                                {
                                    List<string> tArrayMacro = tMacro.StringValuesArray();
                                    List<string> tArrayMacroAdd = tMacro.StringValuesArrayAdd();
                                    List<string> tArrayRepresentation = tMacro.RepresentationValuesArray();
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
                                    tIndex = EditorGUILayout.Popup(tMacro.GetTitle(), tIndex, tArrayRepresentation.ToArray(), GUILayout.Height(MDEConstants.RowHeight));
                                    if (tIndex >= 0)
                                    {
                                        if (tArrayMacro[tIndex] != MDEConstants.NONE)
                                        {
                                            //ActiveGroupMacroDefine[tBuildTargetGroup].Add(tArrayMacro[tIndex]);
                                            string[] sList = tArrayMacroAdd[tIndex].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                            foreach (string tAdd in sList)
                                            {
                                                if (ActiveGroupMacroDefine[tBuildTargetGroup].Contains(tAdd) == false)
                                                {
                                                    ActiveGroupMacroDefine[tBuildTargetGroup].Add(tAdd);
                                                }
                                            }
                                        }
                                    }
                                    //GUILayout.Space(5.0F);
                                }
                                if (tMacro.GetType().IsSubclassOf(typeof(NWDMacroBoolDefiner)))
                                {
                                    List<string> tArrayMacro = tMacro.StringValuesArray();
                                    List<string> tArrayMacroAdd = tMacro.StringValuesArrayAdd();
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
                                        tSelected = EditorGUILayout.Toggle(new GUIContent(tMacro.GetTitle(), tArrayMacroAdd[1]), tSelected, GUILayout.Height(MDEConstants.RowHeight));
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
                                                string[] sList = tArrayMacroAdd[tIndex].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                                foreach (string tAdd in sList)
                                                {
                                                    if (ActiveGroupMacroDefine[tBuildTargetGroup].Contains(tAdd) == false)
                                                    {
                                                        ActiveGroupMacroDefine[tBuildTargetGroup].Add(tAdd);
                                                    }
                                                }
                                            }
                                        }
                                        //GUILayout.Space(5.0F);
                                    }
                                }
                            }
                            GUILayout.EndVertical();
                        }
                    }

                    if (EnumTypeList.Count > 0)
                    {
                        GUILayout.BeginVertical();
                        GUILayout.Label(MDEConstants.EnumArea, NWDGUI.KTableHeaderStatut);
                        EnumTypeList.Sort((tA, tB) => string.Compare(tA.GetTitle(), tB.GetTitle(), StringComparison.Ordinal));
                        foreach (NWDMacroDefiner tEnum in EnumTypeList)
                        {
                            List<string> tArrayMacro = tEnum.StringValuesArray();
                            List<string> tArrayMacroAdd = tEnum.StringValuesArrayAdd();
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
                            tIndex = EditorGUILayout.Popup(tEnum.GetTitle(), tIndex, tArrayRepresentation.ToArray(), GUILayout.Height(MDEConstants.RowHeight));
                            if (tIndex >= 0)
                            {
                                if (tArrayMacro[tIndex] != MDEConstants.NONE)
                                {
                                    //ActiveGroupMacroDefine[tBuildTargetGroup].Add(tArrayMacro[tIndex]);
                                    string[] sList = tArrayMacroAdd[tIndex].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                    foreach (string tAdd in sList)
                                    {
                                        if (ActiveGroupMacroDefine[tBuildTargetGroup].Contains(tAdd) == false)
                                        {
                                            ActiveGroupMacroDefine[tBuildTargetGroup].Add(tAdd);
                                        }
                                    }
                                }
                            }
                            //GUILayout.Space(5.0F);
                        }
                        GUILayout.EndVertical();
                    }
                    if (BoolTypeList.Count > 0)
                    {
                        GUILayout.BeginVertical();
                        GUILayout.Label(MDEConstants.BoolArea, NWDGUI.KTableHeaderStatut);
                        BoolTypeList.Sort((tA, tB) => string.Compare(tA.GetTitle(), tB.GetTitle(), StringComparison.Ordinal));
                        foreach (NWDMacroDefiner tBool in BoolTypeList)
                        {
                            List<string> tArrayMacro = tBool.StringValuesArray();
                            List<string> tArrayMacroAdd = tBool.StringValuesArrayAdd();
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
                                tSelected = EditorGUILayout.Toggle(new GUIContent(tBool.GetTitle(), tArrayMacroAdd[1]), tSelected, GUILayout.Height(MDEConstants.RowHeight));
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
                                        string[] sList = tArrayMacroAdd[tIndex].Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                        foreach (string tAdd in sList)
                                        {
                                            if (ActiveGroupMacroDefine[tBuildTargetGroup].Contains(tAdd) == false)
                                            {
                                                ActiveGroupMacroDefine[tBuildTargetGroup].Add(tAdd);
                                            }
                                        }
                                    }
                                }
                                //GUILayout.Space(5.0F);
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
                        Load();
                        GUIUtility.ExitGUI();
                        //tReload = true;
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
                Load();
                GUIUtility.ExitGUI();
                //tReload = true;
                //Load();
            }
            //NWDGUILayout.Section("Macro Define informations");
            //NWDGUILayout.Informations("If you want create new macro, use generic from " + typeof(MDEDataTypeEnumGeneric<>).Name + " or " + typeof(MDEDataTypeBoolGeneric<>).Name + "!");
            NWDGUILayout.Section("Add custom macro");
            //GUILayout.Label(MDEConstants.NewMacroArea, EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            NewMacro = EditorGUILayout.TextField(MDEConstants.NewMacro, NewMacro);
            NewMacro = NewMacro.ToUpper();
            NewMacro = NewMacro.Replace(";", "");
            NewMacro = NewMacro.Replace(" ", "_");
            NewMacro = NewMacro.Replace("-", "_");
            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(NewMacro));
            if (GUILayout.Button(MDEConstants.NewMacroButton, GUILayout.Width(NWDGUI.KTableSearchWidth)))
            {
                if (AllMacrosOriginal.Contains(NewMacro) == false)
                {
                    AllMacrosOriginal.Add(UnixCleaner(NewMacro));
                    AllMacrosOriginal.Sort();
                    Modified = true;
                    GUI.FocusControl(null);
                }
                NewMacro = string.Empty;
            }
            EditorGUI.EndDisabledGroup();
            NWDGUILayout.LittleSpace();
            GUILayout.EndHorizontal();
            NWDGUILayout.Line();
            NWDGUILayout.LittleSpace();
            //NWDGUILayout.Section("Macro Define save");
            //GUILayout.Label(MDEConstants.SaveArea, EditorStyles.boldLabel);
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
                //NWDEditorWindow.GenerateCSharpFile();
            }
            EditorGUI.EndDisabledGroup();
            NWDGUI.EndRedArea();
            NWDGUILayout.BigSpace();
            //if (tReload)
            //{
            //    Load();
            //}
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddMacro(string sMacro, bool sActiveDefault = true)
        {
            Load();
            string tNewMacro = sMacro.ToUpper();
            tNewMacro = tNewMacro.Replace(";", "");
            tNewMacro = tNewMacro.Replace(" ", "_");
            tNewMacro = tNewMacro.Replace("-", "_");
            AllMacrosOriginal.Add(UnixCleaner(tNewMacro));
            foreach (BuildTargetGroup tBuildTargetGroup in ActiveGroup)
            {
                if (sActiveDefault)
                {
                    if (ActiveGroupMacroDefine[tBuildTargetGroup].Contains(tNewMacro) == false)
                    {
                        ActiveGroupMacroDefine[tBuildTargetGroup].Add(tNewMacro);
                    }
                }
            }
            WritePref();
            foreach (BuildTargetGroup tBuildTargetGroupSet in ActiveGroup)
            {
                string tSymbos = string.Join(";", ActiveGroupMacroDefine[tBuildTargetGroupSet]);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(tBuildTargetGroupSet, tSymbos);
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        public bool ChangeThisMacroAll(string sMacro, bool sActive)
        {
            if (sActive == true)
            {
                Debug.Log("will add " + sMacro);
            }
            else
            {
                Debug.Log("will remove " + sMacro);
            }
            bool rReturnModified = false;
            foreach (BuildTargetGroup tBuildTargetGroup in ActiveGroup)
            {
                if (sActive == true)
                {
                    if (ActiveGroupMacroDefine[tBuildTargetGroup].Contains(sMacro) == false)
                    {
                        ActiveGroupMacroDefine[tBuildTargetGroup].Add(sMacro);
                        rReturnModified = true;
                    }
                }
                else
                {
                    if (ActiveGroupMacroDefine[tBuildTargetGroup].Contains(sMacro) == true)
                    {
                        ActiveGroupMacroDefine[tBuildTargetGroup].Remove(sMacro);
                        rReturnModified = true;
                    }
                }
            }
            return rReturnModified;
        }
        //-------------------------------------------------------------------------------------------------------------
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class MDEMacroDefineEditor : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string TutorialLink(string sLink = "")
        {
            return NWDConstants.K_NWDURL + "macro-define/";
        }
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
                kSharedInstance = EditorWindow.GetWindow(typeof(MDEMacroDefineEditor), ShowAsWindow()) as MDEMacroDefineEditor;
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
            //SharedInstance().ShowUtility();
            SharedInstance().ShowMe();
            SharedInstance().Focus();
            //NWDBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnEnable()
        {
            TitleInit(NWDConstants.K_APP_MACRO_DEFINE_TITLE, typeof(MDEMacroDefineEditor));
            MDEMacroDefineEditorContent.SharedInstance().Load();
            NormalizeWidth = 1100;
            NormalizeHeight = 1060;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnPreventGUI()
        {
            NWDGUI.LoadStyles();
            MDEMacroDefineEditorContent.SharedInstance().OnPreventGUI(position);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
