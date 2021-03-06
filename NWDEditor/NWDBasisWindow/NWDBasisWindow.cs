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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using System.IO;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDBasisWindow<K> : NWDTypeWindow where K : NWDBasisWindow<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string TutorialLink(string sLink = "")
        {
            return NWDConstants.K_NWDURL + "table-manager/";
        }
        //-------------------------------------------------------------------------------------------------------------
        protected static K SharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        public static void ShowWindow()
        {
            if (SharedInstance == null)
            {
                Type[] tAllTypes = Assembly.GetExecutingAssembly().GetTypes();
                Type[] tAllNWDTypes = (from System.Type type in tAllTypes
                                       where (
                                       type.BaseType != null &&
                                       type.BaseType.IsGenericType &&
                                       type.BaseType.GetGenericTypeDefinition() == typeof(NWDBasisWindow<>)
                                       )
                                       select type).ToArray();
                SharedInstance = EditorWindow.GetWindow<K>(tAllNWDTypes);
            }
            SharedInstance.Show(true);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ShowWindow(Type sType)
        {
            ShowWindow();
            SharedInstance.SelectTab(sType);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ShowAndFocusWindow()
        {
            ShowWindow();
            SharedInstance.Focus();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Detach in window"), false, DetachAsWindow);
            menu.AddItem(new GUIContent("Normalize size window " + NormalizeWidth.ToString("0") + "x" + NormalizeHeight.ToString("0")), false, NormalizeSize);
            menu.AddItem(new GUIContent("Free size window " + position.width.ToString("0") + "x" + position.height.ToString("0") + ""), false, FreeSize);
            menu.AddSeparator("");
            menu.AddItem(new GUIContent("Visualize script"), false, ScriptOpener, this.GetType());
            if (string.IsNullOrEmpty(TutorialLink()) == false)
            {
                menu.AddItem(new GUIContent("Tutorial online"), false, TutorialOnline);
            }
            menu.AddSeparator("");
            foreach (Type tType in mTabTypeList)
            {
                NWDBasisHelper tTypeInfos = NWDBasisHelper.FindTypeInfos(tType);
                menu.AddItem(new GUIContent("Visualize script of class "+ tTypeInfos.ClassNamePHP+ ""), false, ScriptOpener, tTypeInfos.ClassType);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DetachAsWindow()
        {
            SharedInstance = new K();
#if NWD_DEVELOPER
            SharedInstance.NormalizeSize();
#endif
            SharedInstance.ShowUtility();
            SharedInstance.Focus();
            SharedInstance.RemoveFieldFocus = true;
            Close();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The title of window
        /// </summary>
        public string mTitleKey = "X Edition";
        /// <summary>
        /// The description of window.
        /// </summary>
        public string mDescriptionKey = "X Edition, You can add, change, remove the …";
        /// <summary>
        /// The type's list of the tables managed in this windows
        /// </summary>
        public List<Type> mTabTypeList = new List<Type>();
        /// <summary>
        /// the array to reccord the name menu of each Type
        /// </summary>
        private GUIContent[] mTabContentList;
        private GUIContent[] mPanelContentList;
        int TabsTotalWidthExpected = 0;
        int TabWidthMax = 0;
        /// <summary>
        /// The tab selected.
        /// </summary>
        private int mTabSelected = 0;
        NWDSplitArea SplitArea = new NWDSplitArea(NWDSplitDirection.Vertical, "DataTablePanel");
        NWDBasisHelperPanel mPanelSelected = NWDBasisHelperPanel.Data;
        //-------------------------------------------------------------------------------------------------------------
        static NWDBasisWindow()
        {
            //Debug.Log ("NWDBasisWindow clas constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDBasisWindow()
        {
            //Debug.Log ("NWDBasisWindow basic construtor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDBasisWindow(string sTitleKey = NWEConstants.K_EMPTY_STRING, string sDescriptionKey = NWEConstants.K_EMPTY_STRING, List<Type> sTabTypeList = null)
        {
            //Debug.Log ("NWDBasisWindow advanced constructor");
            this.mTitleKey = sTitleKey;
            this.mDescriptionKey = sDescriptionKey;
            this.mTabTypeList = sTabTypeList;
            this.DefineTab();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Call on update once per frame
        /// </summary>
        void Update()
        {
            this.wantsMouseMove = true;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Call on inspector update
        /// </summary>
        void OnInspectorUpdate()
        {
            //NWDBenchmark.Start();
            // Add to detect mouse event and redraw the widow
            if (EditorWindow.focusedWindow == this && EditorWindow.mouseOverWindow == this)
            {
                this.Repaint();
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Call on enable this window
        /// </summary>
        public void DefineTab()
        {
            //NWDBenchmark.Start();
            // force to check database connection
            //NWDDataManager.SharedInstance().ConnectToDatabase ();
            // prepare futur list results
            TabsTotalWidthExpected = 0;
            TabWidthMax = 0;
            int tCounter = 0;
            List<GUIContent> tTabContentList = new List<GUIContent>();
            // check all type 
            foreach (Type tType in mTabTypeList)
            {
                NWDBasisHelper tTypeInfos = NWDBasisHelper.FindTypeInfos(tType);
                if (tTypeInfos != null)
                {
                    // add informations for tab list
                    if (TabWidthMax < tTypeInfos.ClassMenuName.Length)
                    {
                        TabWidthMax = tTypeInfos.ClassMenuName.Length;
                    }
                    if (tTypeInfos.ClassMenuNameContent == null)
                    {
                        tTypeInfos.ClassMenuNameContent = new GUIContent(tTypeInfos.ClassMenuName, tTypeInfos.TextureOfClass(), tTypeInfos.ClassDescription);
                    }
                    tTabContentList.Add(tTypeInfos.ClassMenuNameContent);
                    // I add this window to window update for this Type of Datas
                    NWDDataManager.SharedInstance().AddWindowInManager(this, mTabTypeList);
                    tCounter++;
                }
            }
            TabsTotalWidthExpected = TabWidthMax * tCounter * 8;
            mTabContentList = tTabContentList.ToArray();

            List<GUIContent> tPanelContentList = new List<GUIContent>();
            foreach (string tEnumName in Enum.GetNames(typeof(NWDBasisHelperPanel)))
            {
                tPanelContentList.Add(new GUIContent(tEnumName));
            }
            mPanelContentList = tPanelContentList.ToArray();
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Call on enable this window
        /// </summary>
        void OnEnable()
        {
            //NWDBenchmark.Start();
            // set default size
            NormalizeWidth = 1400;
            NormalizeHeight = 1000;

            SplitArea.Min = 300;
            SplitArea.Split = 0.75F;
            SplitArea.ResizeSplit(this, position);
            NWDDataManager.SharedInstance().DataQueueExecute();
            if (typeof(K).GetCustomAttributes(typeof(NWDTypeWindowParamAttribute), true).Length > 0)
            {
                NWDTypeWindowParamAttribute tNWDBasisWindowParamAttribute = (NWDTypeWindowParamAttribute)typeof(K).GetCustomAttributes(typeof(NWDTypeWindowParamAttribute), true)[0];
                mTitleKey = tNWDBasisWindowParamAttribute.Title;
                mDescriptionKey = tNWDBasisWindowParamAttribute.Description;
                if (tNWDBasisWindowParamAttribute.TypeList == null)
                {
                    List<Type> tTabTypeList = new List<Type>();

                    List<NWDBasisHelper> tHelperList = new List<NWDBasisHelper>();
                    foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeList)
                    {
                        tHelperList.Add(NWDBasisHelper.FindTypeInfos(tType));
                    }
                    tHelperList.Sort((NWDBasisHelper x, NWDBasisHelper y) => string.Compare(x.ClassMenuName, y.ClassMenuName, StringComparison.OrdinalIgnoreCase));
                    foreach (NWDBasisHelper tNWDBasisHelper in tHelperList)
                    {
                        tTabTypeList.Add(tNWDBasisHelper.ClassType);
                    }
                    mTabTypeList = tTabTypeList;
                }
                else
                {
                    mTabTypeList.Clear();
                    foreach (Type tType in tNWDBasisWindowParamAttribute.TypeList)
                    {
                        mTabTypeList.Add(tType);
                    }
                }
                foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeLoadedList)
                {
                    foreach (NWDWindowOwnerAttribute tOnwer in tType.GetCustomAttributes(typeof(NWDWindowOwnerAttribute), true))
                    {
                        if (tOnwer.WindowType != null)
                        {
                            if (tOnwer.WindowType == this.GetType())
                            {
                                mTabTypeList.Add(tType);
                            }
                        }
                    }
                }
                TitleInit(mTitleKey, typeof(K));
                // redefine the TabBar navigation
                DefineTab();
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Call on disable this window
        /// </summary>
        void OnDisable()
        {
            NWDDataManager.SharedInstance().DataQueueExecute();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Call on focus on this window
        /// </summary>
        void OnFocus()
        {
            NWDDataManager.SharedInstance().DataQueueExecute();
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnLostFocus()
        {
            NWDDataManager.SharedInstance().DataQueueExecute();
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnDestroy()
        {
            NWDDataManager.SharedInstance().RemoveWindowFromManager(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetClassInEdition(Type sClassType)
        {
            //NWDBenchmark.Start();
            NWDDataManager.SharedInstance().DataQueueExecute();
            GUI.FocusControl(null);
            NWDBasisClassInspector tBasisClassInspector = ScriptableObject.CreateInstance<NWDBasisClassInspector>();
            tBasisClassInspector.mTypeInEdition = sClassType;
            Selection.activeObject = tBasisClassInspector;
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Call when draw this window, once per frame
        /// </summary>
        public override void OnPreventGUI()
        {
            //NWDBenchmark.Start();
            NWDGUI.LoadStyles();

            // draw bar top
            EditorGUI.DrawRect(new Rect(0, 0, position.width, NWDGUI.KTAB_BAR_HEIGHT), NWDGUI.KTAB_BAR_BACK_COLOR);
            EditorGUI.DrawRect(new Rect(0, NWDGUI.KTAB_BAR_HEIGHT, position.width, 1.0F), NWDGUI.KTAB_BAR_LINE_COLOR);
            EditorGUI.DrawRect(new Rect(0, NWDGUI.KTAB_BAR_HEIGHT + 1, position.width, 1.0F), NWDGUI.KTAB_BAR_HIGHLIGHT_COLOR);
            // split windows
            SplitArea.OnGUI(this);
            // define rect to use 
            Rect tAreaTableOrign = SplitArea.GetAreaOne();
            Rect tAreaPanelOrigin = SplitArea.GetAreaTwo();
            // define rect without bar top
            Rect tAreaTable = SplitArea.GetAreaOne();
            Rect tAreaPanel = SplitArea.GetAreaTwo();
            tAreaPanel.y += NWDGUI.KTAB_BAR_HEIGHT;
            tAreaPanel.height -= NWDGUI.KTAB_BAR_HEIGHT;
            tAreaTable.y += NWDGUI.KTAB_BAR_HEIGHT;
            tAreaTable.height -= NWDGUI.KTAB_BAR_HEIGHT;
            Rect tRectPanel = new Rect(tAreaPanelOrigin.x + NWDGUI.kFieldMarge, NWDGUI.kFieldMarge, tAreaPanelOrigin.width - NWDGUI.kFieldMarge * 1 - NWDGUI.kTitleStyle.fixedHeight, NWDGUI.KTAB_BAR_HEIGHT - NWDGUI.kFieldMarge * 2);
            // select the panel mode
            mPanelSelected = (NWDBasisHelperPanel)GUI.Toolbar(tRectPanel, (int)mPanelSelected, mPanelContentList, NWDGUI.KTableClassToolbar);
            // select the basishelper to use
            if (mTabContentList != null)
            {
                if (mTabSelected > mTabContentList.Count())
                {
                    mTabSelected = 0;
                }
                // the next selected tab
                int tTabSelected = 0;
                // check if tab ids necessary
                if (mTabContentList.Length > 0)
                {
                    if (tAreaTableOrign.width > TabsTotalWidthExpected)
                    {
                        Rect tRectTab = new Rect(NWDGUI.kFieldMarge, NWDGUI.kFieldMarge, tAreaTableOrign.width - NWDGUI.kFieldMarge * 2, NWDGUI.KTAB_BAR_HEIGHT - NWDGUI.kFieldMarge * 2);
                        tTabSelected = GUI.Toolbar(tRectTab, mTabSelected, mTabContentList, NWDGUI.KTableClassToolbar);
                    }
                    else
                    {
                        Rect tRectTab = new Rect(NWDGUI.kFieldMarge, NWDGUI.kFieldMarge, tAreaTableOrign.width - NWDGUI.kFieldMarge * 2, NWDGUI.KTAB_BAR_HEIGHT - NWDGUI.kFieldMarge * 2);
                        tTabSelected = EditorGUI.Popup(tRectTab, mTabSelected, mTabContentList, NWDGUI.KTableClassPopup);
                    }
                }

                bool tAutoselect = false;
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Tab && Event.current.shift)
                {
                    tTabSelected++;
                    if (tTabSelected >= mTabContentList.Length)
                    {
                        tTabSelected = 0;
                    }
                    tAutoselect = true;
                    Event.current.Use();
                }
                // select the good class to show
                if (tTabSelected >= mTabTypeList.Count())
                {
                    tTabSelected = 0;
                }
                if (mTabTypeList.Count() > 0)
                {
                    Type tType = mTabTypeList[tTabSelected];
                    if (mTabSelected != tTabSelected)
                    {
                        NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                        tHelper.LoadEditorPrefererences();
                        mPanelSelected = NWDBasisHelper.FindTypeInfos(tType).PanelActivate;
                        GUI.FocusControl(null);
                    }
                    mTabSelected = tTabSelected;
                    // DRAW TABLE!
                    SplitArea.BeginAreaOne();
                    NWDBasisHelper.FindTypeInfos(tType).DrawInEditor(this, tAreaTable, tAreaTableOrign, tAutoselect);
                    SplitArea.EndAreaOne();
                    // DRAW PANEL!
                    NWDBasisHelper.FindTypeInfos(tType).ActivePanel(mPanelSelected);
                    switch (mPanelSelected)
                    {
                        case NWDBasisHelperPanel.Infos:
                            {
                                NWDBasisHelper.FindTypeInfos(tType).DrawPanelInfos(tAreaPanel);
                            }
                            break;
                        case NWDBasisHelperPanel.Actions:
                            {
                                NWDBasisHelper.FindTypeInfos(tType).DrawPanelAction(tAreaPanel);
                            }
                            break;
                        case NWDBasisHelperPanel.Sync:
                            {
                                NWDBasisHelper.FindTypeInfos(tType).DrawPanelSync(tAreaPanel);
                            }
                            break;
                        case NWDBasisHelperPanel.Data:
                            {
                                NWDBasisHelper.FindTypeInfos(tType).DrawPanelData(tAreaPanel);
                            }
                            break;
                    }
                }
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void SelectTab(Type tType)
        {
            if (mTabTypeList.Contains(tType))
            {
                mTabSelected = mTabTypeList.IndexOf(tType);
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                tHelper.LoadEditorPrefererences();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
