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
    class NWDSpliArea
    {
        //-------------------------------------------------------------------------------------------------------------
        static NWDSpliArea ActiveZone = null;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// range 0-1
        /// </summary>
        public float Split = 0.5f;
        public float Min = 50.0f;
        //-------------------------------------------------------------------------------------------------------------
        private bool Init = false;
        private NWDSplitDirection Direction = NWDSplitDirection.Horizontal;
        private Rect Origin;
        private Rect First;
        private Rect Second;
        private Rect Line;
        private Rect LineAction;
        public float Border = 0.50f;
        private float ActionOffset = 3.0f;
        public float LineOffset = 0.0f;
        private EditorWindow EditorWind;
        private string PrefKey;
        public bool Resizable = true;
        private bool AreaOneDraw = false;
        private bool AreaTwoDraw = false;
        private Color AreaOneColor = Color.black;
        private Color AreaTwoColor = Color.black;
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
        public NWDSpliArea(NWDSplitDirection sDirection = NWDSplitDirection.Vertical, string sPrefKey = null)
        {
            Direction = sDirection;
            if (string.IsNullOrEmpty(sPrefKey) == false)
            {
                PrefKey = sPrefKey;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResizeSplit(EditorWindow sEditorWind, Rect sOrigin)
        {
            EditorWind = sEditorWind;
            Origin = sOrigin;
            if (Direction == NWDSplitDirection.Vertical)
            {
                float tVA = Origin.width * Split - Border;
                float tVB = Origin.width * (1 - Split) - Border;
                if (tVA < Min)
                {
                    tVA = Min;
                    tVB = Origin.width - tVA - Border * 2;
                }
                if (tVB < Min)
                {
                    tVB = Min;
                    tVA = Origin.width - tVB - Border * 2;
                }
                First = new Rect(Origin.x, Origin.y, tVA, Origin.height);
                Second = new Rect(Origin.x + tVA + Border * 2, Origin.y, tVB + Border, Origin.height);
                Line = new Rect(Origin.x + tVA, Origin.y + LineOffset, Border * 2, Origin.height - LineOffset * 2);
                LineAction = new Rect(Line.x - ActionOffset, Line.y - ActionOffset, Line.width + ActionOffset * 2, Line.height + ActionOffset * 2);
            }
            else
            {
                float tVA = Origin.height * Split - Border;
                float tVB = Origin.height * (1 - Split) - Border;
                if (tVA < Min)
                {
                    tVA = Min;
                    tVB = Origin.height - tVA - Border * 2;
                }
                if (tVB < Min)
                {
                    tVB = Min;
                    tVA = Origin.height - tVB - Border * 2;
                }
                First = new Rect(Origin.x, Origin.y, Origin.width, tVA);
                Second = new Rect(Origin.x, Origin.y + tVA + Border * 2, Origin.width, tVB + Border);
                Line = new Rect(Origin.x + LineOffset, Origin.y + tVA, Origin.width - LineOffset * 2, Border * 2);
                LineAction = new Rect(Line.x - ActionOffset, Line.y - ActionOffset, Line.width + ActionOffset * 2, Line.height + ActionOffset * 2);
            }
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
                        ResizeSplit(EditorWind, Origin);
                    }
                    else
                    {
                        Split = (Event.current.mousePosition.y - Origin.y) / Origin.height;
                        ResizeSplit(EditorWind, Origin);
                    }
                    EditorWind.Repaint();
                }
                if (AreaOneDraw == true) { EditorGUI.DrawRect(First, AreaOneColor); }
                if (AreaTwoDraw == true) { EditorGUI.DrawRect(Second, AreaTwoColor); }
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
                if (AreaOneDraw == true) { EditorGUI.DrawRect(First, AreaOneColor); }
                if (AreaTwoDraw == true) { EditorGUI.DrawRect(Second, AreaTwoColor); }
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
        public Rect GetAreaOne()
        {
            return First;

        }
        //-------------------------------------------------------------------------------------------------------------
        public Rect GetAreaTwo()
        {
            return Second;

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
        public static void GenerateCSharpFile()
        {
            NWDBenchmark.Start();
            if (NWDProjectPrefs.GetBool(NWDConstants.K_EDITOR_SHOW_COMPILE, true) == true)
            {
                foreach (NWDEditorWindow tWindow in AllWindowsList)
                {
                    tWindow.Recompile = true;
                    tWindow.Repaint();
                }
            }
            NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            NWDBenchmark.Start();
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
    public class NWDExmpleSpiltView : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        static NWDExmpleSpiltView TestWindow = null;
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem("TEST/test split", false, 21)]
        static public void AndroidShowActivityIndicatorOnLoading()
        {
            if (TestWindow == null)
            {
                TestWindow = EditorWindow.GetWindow(typeof(NWDExmpleSpiltView)) as NWDExmpleSpiltView;
                TestWindow.TitleInit("SplitTest");
            }
            TestWindow.Show();
            TestWindow.Focus();
        }
        //-------------------------------------------------------------------------------------------------------------
        NWDSpliArea SplitMe = new NWDSpliArea(NWDSplitDirection.Vertical, "eee");
        NWDSpliArea SplitMeAgain = new NWDSpliArea(NWDSplitDirection.Horizontal, "kkkk");
        NWDSpliArea SplitThree = new NWDSpliArea(NWDSplitDirection.Vertical, "ooooo");
        Vector2 ScrollA = Vector2.zero;
        Vector2 ScrollB = Vector2.zero;
        //-------------------------------------------------------------------------------------------------------------
        private void OnEnable()
        {
            //SplitMeAgain.Resizable = false;
            //SplitMeAgain.Split = 0.25F;
            SplitMe.Min = SplitThree.Min * 2;
            //SplitMeAgain.SetColorAreaOne(Color.blue);
            //SplitMeAgain.SetColorAreaTwo(Color.red);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnPreventGUI()
        {
            SplitMe.OnGUI(this);
            SplitMeAgain.OnGUI(this, SplitMe.GetAreaTwo());
            SplitThree.OnGUI(this, SplitMeAgain.GetAreaOne());


            GUILayout.BeginArea(SplitMe.GetAreaOne());
            GUILayout.Button("Click me");
            GUILayout.Button("Or me");
            GUILayout.EndArea();


            GUILayout.BeginArea(SplitMeAgain.GetAreaTwo());
            GUILayout.Button("Click me");
            GUILayout.Button("Or me");
            GUILayout.EndArea();

            GUILayout.BeginArea(SplitThree.GetAreaOne());
            ScrollB = GUILayout.BeginScrollView(ScrollB);
            GUILayout.Button("Click me");
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            GUILayout.EndScrollView();
            GUILayout.EndArea();

            GUILayout.BeginArea(SplitThree.GetAreaTwo());
            ScrollA= GUILayout.BeginScrollView(ScrollA);
            GUILayout.Button("Click me");
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            GUILayout.Button("Or me");
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
