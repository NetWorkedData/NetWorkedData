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
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDSplitDirection
    {
        Horizontal,
        Vertical
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    class NWDSplitArea
    {
        //-------------------------------------------------------------------------------------------------------------
        private static uint LineWidth = 1;
        //-------------------------------------------------------------------------------------------------------------
        public static void SetGlobalLineWidth(uint sWidth = 1)
        {
            if (sWidth > 0)
            {
                LineWidth = sWidth;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static uint LineOffset = 0;
        //-------------------------------------------------------------------------------------------------------------
        public static void SetGlobalLineOffset(uint sOffset = 0)
        {
            LineOffset = sOffset;
        }
        //-------------------------------------------------------------------------------------------------------------
        static NWDSplitArea ActiveZone = null;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// range 0-1
        /// </summary>
        public float Split = 0.5f;
        /// <summary>
        /// 50 to prevent scroolview with bar
        /// </summary>
        public float Min = 50.0f;
        //-------------------------------------------------------------------------------------------------------------
        private bool Init = false;
        private NWDSplitDirection Direction = NWDSplitDirection.Horizontal;
        private Rect Origin;
        private Rect AreaOne;
        private Rect AreaTwo;
        private Rect Line;
        private float Width = 0.50f;
        private float Offset = 0.0f;
        private Rect LineAction;
        private int ActionOffset = 4;
        private EditorWindow WindowToUse;
        private string PrefKey;
        private bool Resizable = true;
        private bool AreaOneDraw = false;
        private bool AreaTwoDraw = false;
        private Color AreaOneColor = Color.black;
        private Color AreaTwoColor = Color.black;
        //-------------------------------------------------------------------------------------------------------------
        public NWDSplitArea(NWDSplitDirection sDirection = NWDSplitDirection.Vertical, string sPrefKey = null)
        {
            Width = (float)LineWidth / 2.0f;
            Offset = LineOffset;
            Direction = sDirection;
            if (string.IsNullOrEmpty(sPrefKey) == false)
            {
                PrefKey = sPrefKey;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetResizable(bool sResizable = true)
        {
            Resizable = sResizable;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetLineOffset(int sOffset = 0)
        {
            Offset = Mathf.Abs(sOffset);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetLineWidth(int sWidth = 1)
        {
            if (sWidth > 0)
            {
                Width = ((float)sWidth) / 2.0f;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetLineLarge()
        {
            SetLineWidth(2);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetLineThin()
        {
            SetLineWidth(1);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetColorAreaOne(Color sColor)
        {
            AreaOneDraw = true;
            AreaOneColor = sColor;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetColorAreaTwo(Color sColor)
        {
            AreaTwoDraw = true;
            AreaTwoColor = sColor;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetColorAreaOne()
        {
            AreaOneDraw = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetColorAreaTwo()
        {
            AreaTwoDraw = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void BeginAreaOne(int sMarge = 0)
        {
            GUILayout.BeginArea(GetAreaOne(sMarge));
        }
        //-------------------------------------------------------------------------------------------------------------
        public void EndAreaOne()
        {
            GUILayout.EndArea();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void BeginAreaTwo(int sMarge = 0)
        {
            GUILayout.BeginArea(GetAreaTwo(sMarge));
        }
        //-------------------------------------------------------------------------------------------------------------
        public void EndAreaTwo()
        {
            GUILayout.EndArea();
        }
        //-------------------------------------------------------------------------------------------------------------
        public Rect GetAreaOne(int sMarge = 0)
        {
            if (sMarge != 0)
            {
                return Reducer(AreaOne, sMarge);
            }
            else
            {
                return AreaOne;
            }

        }
        //-------------------------------------------------------------------------------------------------------------
        public Rect GetAreaTwo(int sMarge = 0)
        {
            if (sMarge != 0)
            {
                return Reducer(AreaTwo, sMarge);
            }
            else
            {
                return AreaTwo;
            }

        }
        //-------------------------------------------------------------------------------------------------------------
        private Rect Reducer(Rect sRect, int sMarge)
        {
            return new Rect(sRect.x + sMarge, sRect.y + sMarge, sRect.width - sMarge * 2.0f, sRect.height - sMarge * 2.0f);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResizeSplit(EditorWindow sEditorWind, Rect sOrigin)
        {
            WindowToUse = sEditorWind;
            Origin = sOrigin;
            if (Direction == NWDSplitDirection.Vertical)
            {
                float tVA = Origin.width * Split - Width;
                float tVB = Origin.width * (1 - Split) - Width;
                if (tVA < Min)
                {
                    tVA = Min;
                    tVB = Origin.width - tVA - Width * 2;
                }
                if (tVB < Min)
                {
                    tVB = Min;
                    tVA = Origin.width - tVB - Width * 2;
                }
                AreaOne = new Rect(Origin.x, Origin.y, tVA, Origin.height);
                AreaTwo = new Rect(Origin.x + tVA + Width * 2, Origin.y, tVB + Width, Origin.height);
                Line = new Rect(Origin.x + tVA, Origin.y + Offset, Width * 2, Origin.height - Offset * 2);
            }
            else
            {
                float tVA = Origin.height * Split - Width;
                float tVB = Origin.height * (1 - Split) - Width;
                if (tVA < Min)
                {
                    tVA = Min;
                    tVB = Origin.height - tVA - Width * 2;
                }
                if (tVB < Min)
                {
                    tVB = Min;
                    tVA = Origin.height - tVB - Width * 2;
                }
                AreaOne = new Rect(Origin.x, Origin.y, Origin.width, tVA);
                AreaTwo = new Rect(Origin.x, Origin.y + tVA + Width * 2, Origin.width, tVB + Width);
                Line = new Rect(Origin.x + Offset, Origin.y + tVA, Origin.width - Offset * 2, Width * 2);
            }
            LineAction = Reducer(Line, -ActionOffset);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnGUI(EditorWindow sEditorWind)
        {
            OnGUI(sEditorWind, new Rect(0, 0, sEditorWind.position.width, sEditorWind.position.height));
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnGUI(EditorWindow sEditorWind, Rect sOrigin)
        {
            if (Init == false)
            {
                Init = true;
                if (string.IsNullOrEmpty(PrefKey) == false)
                {
                    Split = EditorPrefs.GetFloat(PrefKey, Split);
                }
            }
            ResizeSplit(sEditorWind, sOrigin);
            if (Resizable == true)
            {
                if (Direction == NWDSplitDirection.Vertical)
                {
                    EditorGUIUtility.AddCursorRect(LineAction, MouseCursor.SplitResizeLeftRight);
                }
                else
                {
                    EditorGUIUtility.AddCursorRect(LineAction, MouseCursor.SplitResizeUpDown);
                }
                if (Event.current.type == EventType.MouseDown && LineAction.Contains(Event.current.mousePosition))
                {
                    ActiveZone = this;
                }
                if (Event.current.type == EventType.MouseUp)
                {
                    if (string.IsNullOrEmpty(PrefKey) == false)
                    {
                        EditorPrefs.SetFloat(PrefKey, Split);
                    }
                    ActiveZone = null;
                }
                if (Event.current.type == EventType.MouseDrag && ActiveZone == this)
                {
                    if (Direction == NWDSplitDirection.Vertical)
                    {
                        Split = (Event.current.mousePosition.x - Origin.x) / Origin.width;
                        ResizeSplit(WindowToUse, Origin);
                    }
                    else
                    {
                        Split = (Event.current.mousePosition.y - Origin.y) / Origin.height;
                        ResizeSplit(WindowToUse, Origin);
                    }
                    WindowToUse.Repaint();
                }
                if (AreaOneDraw == true) { EditorGUI.DrawRect(AreaOne, AreaOneColor); }
                if (AreaTwoDraw == true) { EditorGUI.DrawRect(AreaTwo, AreaTwoColor); }
                if (EditorGUIUtility.isProSkin)
                {
                    EditorGUI.DrawRect(Line, Color.black);
                }
                else
                {
                    EditorGUI.DrawRect(Line, Color.gray);
                }
            }
            else
            {
                if (AreaOneDraw == true) { EditorGUI.DrawRect(AreaOne, AreaOneColor); }
                if (AreaTwoDraw == true) { EditorGUI.DrawRect(AreaTwo, AreaTwoColor); }
                if (EditorGUIUtility.isProSkin)
                {
                    EditorGUI.DrawRect(Line, new Color(0.5f, 0.5f, 0.5f, 0.5f));
                }
                else
                {
                    EditorGUI.DrawRect(Line, new Color(0.5f, 0.5f, 0.5f, 0.5f));
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDEditorWindowReimport : AssetPostprocessor
    {
        //-------------------------------------------------------------------------------------------------------------
        void OnPreprocessAsset()
        {
            NWDBenchmark.Start();
            NWDEditorWindow.OnBeforeAssemblyReload();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnPostprocessAsset()
        {
            NWDBenchmark.Start();
            NWDEditorWindow.OnAfterAssemblyReload();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            NWDBenchmark.Start();
            NWDEditorWindow.OnAfterAssemblyReload();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDEditorWindowContent
    {
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnEnable(NWDEditorWindow sEditorWindow)
        {
            throw new Exception("override OnEnable()");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnDisable(NWDEditorWindow sEditorWindow)
        {
            throw new Exception("override OnDisable()");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnPreventGUI(Rect sRect)
        {
            //GUILayout.Label("OOOOOKKKKKKKKKK GOOD IMPLEMENTATION");
            //throw new Exception("override OnPreventGUI() in place of OnGUI");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDEditorWindow : EditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        private static List<NWDEditorWindow> AllWindowsList = new List<NWDEditorWindow>();
        //-------------------------------------------------------------------------------------------------------------
        private GUIContent IconAndTitleCompile;
        private GUIContent IconAndTitle;
        private bool ProSkinActive;
        private string EditorTitle;
        private Type EditorType;
        private bool TitleIsInit = false;
        private bool Recompile = false;
        //-------------------------------------------------------------------------------------------------------------
        public static void OnBeforeAssemblyReload()
        {
            //Debug.Log("Before Assembly Reload");
            NWDBenchmark.Start();
            if (NWDProjectPrefs.GetBool(NWDConstants.K_EDITOR_SHOW_COMPILE, true) == true)
            {
                if (AllWindowsList != null)
                {
                    foreach (NWDEditorWindow tWindow in AllWindowsList)
                    {
                        if (tWindow != null)
                        {
                            tWindow.Recompile = true;
                            tWindow.Repaint();
                        }
                    }
                }
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void OnAfterAssemblyReload()
        {
            //Debug.Log("After Assembly Reload");
            NWDBenchmark.Start();
            if (NWDProjectPrefs.GetBool(NWDConstants.K_EDITOR_SHOW_COMPILE, true) == true)
            {
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
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SkinChange()
        {
            NWDBenchmark.Start();
            if (EditorGUIUtility.isProSkin != ProSkinActive || TitleIsInit == false)
            {
                NWDBenchmark.PrefReload();
                TitleEnable();
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void TitleInit(string tTitle, Type tType = null)
        {
            NWDBenchmark.Start();
            EditorTitle = tTitle;
            EditorType = tType;
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void TitleEnable()
        {
            NWDBenchmark.Start();
            if (string.IsNullOrEmpty(EditorTitle) == false)
            {
                TitleIsInit = true;
                ProSkinActive = EditorGUIUtility.isProSkin;
                if (IconAndTitle == null)
                {
                    IconAndTitle = new GUIContent();
                }
                IconAndTitle.text = EditorTitle;

                // find texture by default
                string tIconName = EditorGUIUtility.isProSkin ? typeof(NWDEditorWindow).Name + "_pro" : typeof(NWDEditorWindow).Name;
                //NWDBenchmark.Step(true, tIconName);
                string[] sGUIDs = AssetDatabase.FindAssets(tIconName + " t:texture");
                foreach (string tGUID in sGUIDs)
                {
                    string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                    string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                    if (tPathFilename.Equals(tIconName))
                    {
                        IconAndTitle.image = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                        break;
                    }
                }
                if (EditorType != null)
                {
                    // find texture by in parameters
                    tIconName = EditorGUIUtility.isProSkin ? EditorType.Name + "_pro" : EditorType.Name;
                    //NWDBenchmark.Step(true, tIconName);
                    sGUIDs = AssetDatabase.FindAssets(tIconName + " t:texture");
                    foreach (string tGUID in sGUIDs)
                    {
                        string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                        string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                        if (tPathFilename.Equals(tIconName))
                        {
                            IconAndTitle.image = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                        }
                    }
                }
                titleContent = IconAndTitle;

                IconAndTitleCompile = new GUIContent();
                string tIconNameCompile = EditorGUIUtility.isProSkin ? typeof(NWDEditorWindow).Name + "Compile_pro" : typeof(NWDEditorWindow).Name + "Compile";
                string[] sGUIDCompiles = AssetDatabase.FindAssets(tIconNameCompile + " t:texture");
                foreach (string tGUID in sGUIDCompiles)
                {
                    string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                    string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                    if (tPathFilename.Equals(tIconNameCompile))
                    {
                        IconAndTitleCompile.image = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                        break;
                    }
                }
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ChangeTitleIcon(Texture sIcon)
        {
            IconAndTitle.image = sIcon;
            titleContent.image = sIcon;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void GenerateCSharpFile()
        {
            NWDBenchmark.Start();
            OnBeforeAssemblyReload();
            NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDEditorWindow()
        {
            //NWDBenchmark.Start();
            if (AllWindowsList.Contains(this) == false)
            {
                AllWindowsList.Add(this);
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        ~NWDEditorWindow()
        {
            //NWDBenchmark.Start();
            if (AllWindowsList.Contains(this) == false)
            {
                AllWindowsList.Remove(this);
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnGUI()
        {
            NWDBenchmark.Start();
            SkinChange();
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
                GUILayout.Label(IconAndTitleCompile, NWDGUI.kIconCenterStyle);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                //GUILayout.BeginHorizontal();
                //GUILayout.FlexibleSpace();
                //GUILayout.Label("NeWeeDy!");
                //GUILayout.FlexibleSpace();
                //GUILayout.EndHorizontal();

                GUILayout.Space(10.0F);
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("...compile in progress...");
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
            }
            // if (EditorGUIUtility.isProSkin == true)
            {
                float tLogoSize = NWDGUI.kTitleStyle.fixedHeight;
                GUI.Label(new Rect(position.width - tLogoSize, 0, tLogoSize, tLogoSize), NWDGUI.kNetWorkedDataLogoContent);
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnPreventGUI()
        {
            throw new Exception("override OnPreventGUI() in place of OnGUI");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDExmpleSplitView : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        static NWDExmpleSplitView TestWindow = null;
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem("TEST/test split", false, 21)]
        static public void AndroidShowActivityIndicatorOnLoading()
        {
            if (TestWindow == null)
            {
                TestWindow = EditorWindow.GetWindow(typeof(NWDExmpleSplitView)) as NWDExmpleSplitView;
                TestWindow.TitleInit("SplitTest");
            }
            TestWindow.Show();
            TestWindow.Focus();
        }
        //-------------------------------------------------------------------------------------------------------------
        NWDSplitArea SplitOne;
        NWDSplitArea SplitTwo;
        NWDSplitArea SplitThree;
        //NWDSpliArea SplitOne = new NWDSpliArea(NWDSplitDirection.Vertical, "eee");
        //NWDSpliArea SplitTwo = new NWDSpliArea(NWDSplitDirection.Horizontal, "kkkk");
        //NWDSpliArea SplitThree = new NWDSpliArea(NWDSplitDirection.Vertical, "ooooo");
        Vector2 ScrollA = Vector2.zero;
        Vector2 ScrollB = Vector2.zero;
        //-------------------------------------------------------------------------------------------------------------
        private void OnEnable()
        {
            //NWDSplitArea.SetGlobalLineWidth(0);
            //NWDSplitArea.SetGlobalLineOffset(4);
            SplitOne = new NWDSplitArea(NWDSplitDirection.Vertical, "eee");
            SplitTwo = new NWDSplitArea(NWDSplitDirection.Horizontal, "kkkk");
            SplitThree = new NWDSplitArea(NWDSplitDirection.Vertical, "ooooo");

            //SplitOne.SetLineWidth(2);
            //SplitOne.SetLineOffset(2);
            //SplitOne.SetResizable(false);
            //SplitOne.Split = 0.5F;
            SplitOne.Min = SplitThree.Min * 2;
            //SplitThree.SetColorAreaOne(Color.blue);
            //SplitThree.SetColorAreaTwo(Color.red);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnPreventGUI()
        {
            SplitOne.OnGUI(this);
            SplitTwo.OnGUI(this, SplitOne.GetAreaTwo());
            SplitThree.OnGUI(this, SplitTwo.GetAreaOne());

            SplitOne.BeginAreaOne();
            NWDAppConfigurationManagerContent.SharedInstance().OnPreventGUI(SplitOne.GetAreaOne());
            //NWDGUILayout.Title("jjjj");
            //NWDGUILayout.Section("jjjj");
            //NWDGUILayout.SubSection("jjjj");
            //GUILayout.Button("Click me");
            //GUILayout.Button("Or me");
            //NWDGUILayout.Section("jjjj");
            //NWDGUILayout.SubSection("jjjj");
            //GUILayout.Button("Click me");
            //GUILayout.Button("Or me");
            //NWDGUILayout.SubSection("jjjj");
            //GUILayout.Button("Click me");
            //GUILayout.Button("Or me");
            //NWDGUILayout.SubSection("jjjj");
            //NWDGUILayout.Informations("jjjj");
            //NWDGUILayout.Separator();
            //NWDGUILayout.Informations("jjjj");
            SplitOne.EndAreaOne();

            SplitTwo.BeginAreaTwo();
            //NWDGUILayout.Title("jjjj");
            //GUILayout.Button("Click me");
            //GUILayout.Button("Or me");

            NWDProjectCredentialsManagerContent.SharedInstance().OnPreventGUI(SplitTwo.GetAreaTwo());
            //NWDProjectConfigurationManagerContent.SharedInstance().OnPreventGUI(SplitTwo.GetAreaTwo());
            //NWDLocalizationConfigurationManagerContent.SharedInstance().OnPreventGUI(SplitTwo.GetAreaTwo());
            //NWDAppEnvironmentConfigurationManagerContent.SharedInstance().OnPreventGUI(SplitTwo.GetAreaTwo());
            //NWDAppEnvironmentChooserContent.SharedInstance().OnPreventGUI(SplitTwo.GetAreaTwo());
            SplitTwo.EndAreaTwo();

            SplitThree.BeginAreaOne();
            ScrollB = GUILayout.BeginScrollView(ScrollB);
            NWDGUILayout.Title("jjjj");
            GUILayout.Label("jjjj");
            NWDGUILayout.Section("jjjj");
            NWDGUILayout.SubSection("jjjj");
            NWDGUILayout.WarningBox("jjjj");
            NWDGUI.BeginRedArea();
            GUILayout.Button("Click me");
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            NWDGUI.EndRedArea();
            NWDGUI.BeginColorArea(Color.blue);
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            NWDGUI.EndColorArea();
            GUILayout.EndScrollView();
            SplitThree.EndAreaOne();

            SplitThree.BeginAreaTwo();
            NWDGUILayout.Title("jjjj");
            ScrollA = GUILayout.BeginScrollView(ScrollA);
            GUILayout.Button("Click me");
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            if (NWDGUILayout.AlertBoxButton("alert box", "ok")) { }
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            NWDGUILayout.ErrorBox("error box");
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            NWDGUILayout.WarningBox("warning box");
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            NWDGUILayout.HelpBox("help box");
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            GUILayout.EndScrollView();
            SplitThree.EndAreaTwo();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
