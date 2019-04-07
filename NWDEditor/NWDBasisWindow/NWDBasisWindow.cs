//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BasicToolBox;
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
        public Type[] mTabTypeList = new Type[] { typeof(NWDItem) };
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
        public  bool TabSort = false;

        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes the <see cref="NWDEditor.NWDBasisWindow"/> class.
        /// </summary>
        static NWDBasisWindow()
        {
            //Debug.Log ("NWDBasisWindow clas constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="NWDEditor.NWDBasisWindow"/> class.
        /// </summary>
        public NWDBasisWindow()
        {
            //Debug.Log ("NWDBasisWindow basic construtor");
        }
        //-------------------------------------------------------------------------------------------------------------
        //public Texture FromGizmos(string sName)
        //{
        //    Texture rTexture = NWDFindPackage.PackageEditorTexture(sName+".png");
        //    if (rTexture == null)
        //    {
        //        string[] sGUIDs = AssetDatabase.FindAssets("" + sName + " t:texture");
        //        foreach (string tGUID in sGUIDs)
        //        {
        //            //Debug.Log("TextureOfClass GUID " + tGUID);
        //            string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
        //            string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
        //            //Debug.Log("tPathFilename = " + tPathFilename);
        //            if (tPathFilename.Equals(sName))
        //            {
        //                //Debug.Log("TextureOfClass " + tPath);
        //                rTexture = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
        //            }
        //        }
        //    }
        //    return rTexture;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public NWDBasisWindow(string sTitleKey = BTBConstants.K_EMPTY_STRING, string sDescriptionKey = BTBConstants.K_EMPTY_STRING, Type[] sTabTypeList = null)
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
            // Add to detect mouse event and redraw the widow
            if (EditorWindow.focusedWindow == this && EditorWindow.mouseOverWindow == this)
            {
                this.Repaint();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Call on enable this window
        /// </summary>
        public void DefineTab()
        {
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
            // return result in array
            if (TabSort == true)
            {
                tTabContentList.Sort((GUIContent x, GUIContent y) => string.Compare(x.text, y.text, StringComparison.OrdinalIgnoreCase));
            }
            mTabContentList = tTabContentList.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateDatas()
        {
            foreach (Type tType in mTabTypeList)
            {
                MethodInfo tMethodInfo = NWDAliasMethod.GetMethod(tType, NWDConstants.M_ApplyAllModifications, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    tMethodInfo.Invoke(null, null);
                }
            }
            this.Repaint();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Call on enable this window
        /// </summary>
        void OnEnable()
        {
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
                    mTabTypeList = NWDDataManager.SharedInstance().mTypeList.ToArray();
                    TabSort = true;
                }
                else
                {
                    mTabTypeList = tNWDBasisWindowParamAttribute.TypeList;
                }
                // create the title content
                titleContent = new GUIContent();
                titleContent.text = mTitleKey;
                titleContent.tooltip = mDescriptionKey; // not working :-(
                titleContent.image = IconOfWindow;
                // redefine the TabBar navigation
                DefineTab();
            }
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
            //			ApplyUpdate();
            NWDDataManager.SharedInstance().DataQueueExecute();
            GUI.FocusControl(null);
            NWDBasisClassInspector tBasisClassInspector = ScriptableObject.CreateInstance<NWDBasisClassInspector>();
            tBasisClassInspector.mTypeInEdition = sClassType;
            //			tBasisClassInspector.mWindowInEdition = this;
            Selection.activeObject = tBasisClassInspector;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Call when draw this window, once per frame
        /// </summary>
        public void OnGUI()
        {
            NWDGUI.LoadStyles();
            // prepare the style
            // get width of window
            //float tWidthUsed = EditorGUIUtility.currentViewWidth;


            float tWidthUsed = position.width;
            // determine height
            float tHeight = NWDGUI.KTAB_BAR_HEIGHT;
            //tHeight+=tHelpBoxStyle.CalcHeight (new GUIContent (mDescriptionKey), tWidthUsed);
            //if (mTabContentList.Length > 1)
            //{
            //    if (tWidthUsed > TabsTotalWidthExpected)
            //    {
            //        tHeight += tToolbarStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidthUsed);
            //    }
            //    else
            //    {
            //        tHeight += tPopupStyle.CalcHeight(new GUIContent(BTBConstants.K_A), tWidthUsed);
            //    }
            //    tHeight += NWDGUI.kFieldMarge * 2;
            //}
            // draw background for toolbar


            EditorGUI.DrawRect(new Rect(0, 0, tWidthUsed, tHeight), NWDGUI.KTAB_BAR_BACK_COLOR);
            EditorGUI.DrawRect(new Rect(0, tHeight, tWidthUsed, 1.0F), NWDGUI.KTAB_BAR_LINE_COLOR);
            EditorGUI.DrawRect(new Rect(0, tHeight + 1, tWidthUsed, 1.0F), NWDGUI.KTAB_BAR_HIGHLIGHT_COLOR);

            //if (mDescriptionKey != "")
            //{
            //    EditorGUILayout.HelpBox(mDescriptionKey, MessageType.None);
            //}
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
                    Rect tRectTab = new Rect(NWDGUI.kFieldMarge, NWDGUI.kFieldMarge, tWidthUsed - NWDGUI.kFieldMarge * 2, tHeight - NWDGUI.kFieldMarge*2);
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
                }
                mTabSelected = tTabSelected;
                NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_DrawInEditor, null, new object[] { this, tAutoselect });
            }

        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
