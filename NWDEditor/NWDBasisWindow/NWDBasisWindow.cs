//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:22:23
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
//using BasicToolBox;
using UnityEngine;
using SQLite4Unity3d;
using System.IO;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDBasisWindow<K> : NWDTypeWindow where K : NWDBasisWindow<K>, new()
    {
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
        /// <summary>
        /// The title of window
        /// </summary>
        public string mTitleKey = "X Edition";
        /// <summary>
        /// The icon of window.
        /// </summary>
        public Texture IconOfWindow;
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
        int TabsTotalWidthExpected = 0;
        int TabWidthMax = 0;
        /// <summary>
        /// The tab selected.
        /// </summary>
        private int mTabSelected = 0;
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
            //NWEBenchmark.Start();
            // Add to detect mouse event and redraw the widow
            if (EditorWindow.focusedWindow == this && EditorWindow.mouseOverWindow == this)
            {
                this.Repaint();
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Call on enable this window
        /// </summary>
        public void DefineTab()
        {
            //NWEBenchmark.Start();
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
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Call on enable this window
        /// </summary>
        void OnEnable()
        {
            //NWEBenchmark.Start();
            NWDDataManager.SharedInstance().DataQueueExecute();
            if (typeof(K).GetCustomAttributes(typeof(NWDTypeWindowParamAttribute), true).Length > 0)
            {
                NWDTypeWindowParamAttribute tNWDBasisWindowParamAttribute = (NWDTypeWindowParamAttribute)typeof(K).GetCustomAttributes(typeof(NWDTypeWindowParamAttribute), true)[0];
                mTitleKey = tNWDBasisWindowParamAttribute.Title;
                //IconOfWindow = FromGizmos(tNWDBasisWindowParamAttribute.IconName);
                if (tNWDBasisWindowParamAttribute.IconName == null)
                {
                    tNWDBasisWindowParamAttribute.IconName = typeof(K).Name;
                }
                IconOfWindow = NWDFindPackage.EditorTexture(tNWDBasisWindowParamAttribute.IconName);
                if (IconOfWindow == null)
                {
                    IconOfWindow = NWDFindPackage.PackageEditorTexture("settings.png");
                }
                mDescriptionKey = tNWDBasisWindowParamAttribute.Description;
                if (tNWDBasisWindowParamAttribute.TypeList == null)
                {
                    List<Type> tTabTypeList = new List<Type>();

                    List<NWDBasisHelper> tHelperList = new List<NWDBasisHelper>();
                    foreach (Type tType in NWDDataManager.SharedInstance().mTypeList)
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

                foreach (Type tType in NWDDataManager.SharedInstance().mTypeLoadedList)
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

                // create the title content
                titleContent = new GUIContent();
                titleContent.text = mTitleKey;
                titleContent.tooltip = mDescriptionKey; // not working :-(
                titleContent.image = IconOfWindow;
                // redefine the TabBar navigation
                DefineTab();
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Call on disable this window
        /// </summary>
        void OnDisable()
        {
            NWDDataManager.SharedInstance().DataQueueExecute();
            //ApplyUpdate();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Call on focus on this window
        /// </summary>
        void OnFocus()
        {
            NWDDataManager.SharedInstance().DataQueueExecute();
            //			ApplyUpdate();
            //			ApplyListUpdate ();
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnLostFocus()
        {
            NWDDataManager.SharedInstance().DataQueueExecute();
            //			ApplyUpdate();
            //			ApplyTableUpdate ();
            //			ApplyListUpdate ();
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnDestroy()
        {
            // TODO : check why error on recreate desktop space
            NWDDataManager.SharedInstance().RemoveWindowFromManager(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetClassInEdition(Type sClassType)
        {
            //NWEBenchmark.Start();
            //			ApplyUpdate();
            NWDDataManager.SharedInstance().DataQueueExecute();
            GUI.FocusControl(null);
            NWDBasisClassInspector tBasisClassInspector = ScriptableObject.CreateInstance<NWDBasisClassInspector>();
            tBasisClassInspector.mTypeInEdition = sClassType;
            //			tBasisClassInspector.mWindowInEdition = this;
            Selection.activeObject = tBasisClassInspector;
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Call when draw this window, once per frame
        /// </summary>
        public override void OnPreventGUI()
        {
            //NWEBenchmark.Start();
            NWDGUI.LoadStyles();

            float tWidthUsed = position.width;
            // determine height
            float tHeight = NWDGUI.KTAB_BAR_HEIGHT;


            EditorGUI.DrawRect(new Rect(0, 0, tWidthUsed, tHeight), NWDGUI.KTAB_BAR_BACK_COLOR);
            EditorGUI.DrawRect(new Rect(0, tHeight, tWidthUsed, 1.0F), NWDGUI.KTAB_BAR_LINE_COLOR);
            EditorGUI.DrawRect(new Rect(0, tHeight + 1, tWidthUsed, 1.0F), NWDGUI.KTAB_BAR_HIGHLIGHT_COLOR);

            //if (mDescriptionKey != "")
            //{
            //    EditorGUILayout.HelpBox(mDescriptionKey, MessageType.None);
            //}
            if (mTabContentList != null)
            {
                if (mTabSelected > mTabContentList.Count())
                {
                    mTabSelected = 0;
                }
                // the next selected tab
                int tTabSelected = 0;
                // check if tab ids necessary
                if (mTabContentList.Length > 1)
                {
                    if (tWidthUsed > TabsTotalWidthExpected)
                    {
                        Rect tRectTab = new Rect(NWDGUI.kFieldMarge, NWDGUI.kFieldMarge, tWidthUsed - NWDGUI.kFieldMarge * 2, tHeight - NWDGUI.kFieldMarge * 2);
                        tTabSelected = GUI.Toolbar(tRectTab, mTabSelected, mTabContentList, NWDGUI.KTableClassToolbar);
                    }
                    else
                    {
                        Rect tRectTab = new Rect(NWDGUI.kFieldMarge, NWDGUI.kFieldMarge, tWidthUsed - NWDGUI.kFieldMarge * 2, tHeight - NWDGUI.kFieldMarge * 2);
                        tTabSelected = EditorGUI.Popup(tRectTab, mTabSelected, mTabContentList, NWDGUI.KTableClassPopup);
                    }
                }

                bool tAutoselect = false;
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Tab && Event.current.shift)
                {
                    //				if (Event.current.keyCode==KeyCode.Tab && Event.current.shift) {
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
                        // POUR ACTIVER LA CLASSE DAN L'INSPECTOR 

                        //SetClassInEdition (tType);
                        NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                        tHelper.LoadEditorPrefererences();
                    }
                    mTabSelected = tTabSelected;
                    //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_DrawInEditor, null, new object[] { this, tAutoselect });

                    NWDBasisHelper.FindTypeInfos(tType).DrawInEditor(this, tAutoselect);


                }
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void SelectTab(Type tType)
        {
            if (mTabTypeList.Contains(tType))
            {
                mTabSelected = Array.IndexOf(mTabTypeList, tType);
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
