//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using BasicToolBox;

using UnityEngine;

using SQLite4Unity3d;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	public class NWDBasisWindow <K> : NWDTypeWindow where K : NWDBasisWindow <K>, new()
	{
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
		public Type[] mTabTypeList = new Type[] {typeof(NWDItem)};
		/// <summary>
		/// the array to reccord the name menu of each Type
		/// </summary>
		private string[] mTabList;

		/// <summary>
		/// The tab selected.
		/// </summary>
		private int mTabSelected = 0;

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
		public Texture FromGizmos(string sName)
		{
			return AssetDatabase.LoadAssetAtPath<Texture> (NWDFindPackage.PathOfPackage ("/NWDEditor/NWDNativeImages/"+sName+".png"));
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="NWDEditor.NWDBasisWindow"/> class.
		/// </summary>
		/// <param name="sTitleKey">title.</param>
		/// <param name="sDescriptionKey">description.</param>
		/// <param name="sTabTypeList">Type's list.</param>
			public NWDBasisWindow(string sTitleKey = "", string sDescriptionKey = "", Type[] sTabTypeList = null)
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
		void Update ()
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
		public void DefineTab ()
		{
			NWDDataManager.SharedInstance.ConnectToDatabase ();
			int tCount = mTabTypeList.Length;
			mTabList = new string[tCount];
			int tN = 0;
			foreach( Type tType in mTabTypeList)
			{
//				NWDDataManager.SharedInstance.NotificationCenter.AddObserver (this, NWDNotificationConstants.K_DATAS_UPDATED, delegate (BTBNotification sNotification)
//					{
////						Debug.Log ("###### method invoke");
//						UpdateDatas();
//					}
//				);
				var tMethodInfo = tType.GetMethod("MenuName", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null) 
				{
					var tMenu = tMethodInfo.Invoke(null, null);
					mTabList[tN] = tMenu.ToString();
					tN++;
				}
				// I add this window to window update for this Type of Datas
				NWDDataManager.SharedInstance.AddWindowInManager (this, mTabTypeList);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public void UpdateDatas() {
			foreach (Type tType in mTabTypeList) {
				var tMethodInfo = tType.GetMethod("ApplyAllModifications", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null) 
				{
					tMethodInfo.Invoke(null, null);
				}
			}
			this.Repaint ();

		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Call on enable this window
		/// </summary>
		void OnEnable ()
		{
			NWDDataManager.SharedInstance.UpdateQueueExecute();
			if (typeof(K).GetCustomAttributes (typeof(NWDTypeWindowParamAttribute), true).Length > 0) {
				NWDTypeWindowParamAttribute tNWDBasisWindowParamAttribute = (NWDTypeWindowParamAttribute)typeof(K).GetCustomAttributes (typeof(NWDTypeWindowParamAttribute), true) [0];
				mTitleKey = tNWDBasisWindowParamAttribute.Title;
				IconOfWindow = FromGizmos(tNWDBasisWindowParamAttribute.IconName);
				mDescriptionKey = tNWDBasisWindowParamAttribute.Description;
				if (tNWDBasisWindowParamAttribute.TypeList == null) {
					mTabTypeList = NWDDataManager.SharedInstance.mTypeList.ToArray ();
					Array.Sort(mTabTypeList, (x,y) => String.Compare(x.Name, y.Name));
				} else {
					mTabTypeList = tNWDBasisWindowParamAttribute.TypeList;
				}
				DefineTab ();
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Call on disable this window
		/// </summary>
		void OnDisable ()
		{
			NWDDataManager.SharedInstance.UpdateQueueExecute();
			//ApplyUpdate();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Call on focus on this window
		/// </summary>
		void OnFocus() {
			NWDDataManager.SharedInstance.UpdateQueueExecute();
//			ApplyUpdate();
//			ApplyListUpdate ();
		}
		//-------------------------------------------------------------------------------------------------------------
		void OnLostFocus()
		{
			NWDDataManager.SharedInstance.UpdateQueueExecute();
//			ApplyUpdate();
//			ApplyTableUpdate ();
//			ApplyListUpdate ();
		}
		//-------------------------------------------------------------------------------------------------------------
		void OnDestroy() {
			NWDDataManager.SharedInstance.RemoveWindowFromManager (this);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetClassInEdition(Type sClassType)
		{
//			ApplyUpdate();
			NWDDataManager.SharedInstance.UpdateQueueExecute();
			GUI.FocusControl (null);
			NWDBasisClassInspector tBasisClassInspector = ScriptableObject.CreateInstance<NWDBasisClassInspector>();
			tBasisClassInspector.mTypeInEdition = sClassType;
//			tBasisClassInspector.mWindowInEdition = this;
			Selection.activeObject = tBasisClassInspector;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Call when draw this window, once per frame
		/// </summary>
		public void OnGUI ()
		{

			titleContent = new GUIContent ();
			titleContent.text = mTitleKey;
			if (IconOfWindow == null) 
			{
				IconOfWindow = AssetDatabase.LoadAssetAtPath<Texture> (NWDFindPackage.PathOfPackage ("/NWDEditor/NWDNativeImages/settings.png"));
			}
			if (IconOfWindow != null) {
				titleContent.image = IconOfWindow;
			}

			float tWidthUsed = EditorGUIUtility.currentViewWidth;

			GUIStyle tHelpBoxStyle = new GUIStyle (EditorStyles.helpBox);
			float tHeight = tHelpBoxStyle.CalcHeight (new GUIContent (mDescriptionKey), tWidthUsed);
			GUIStyle tToolbarStyle = new GUIStyle (EditorStyles.toolbar);
			tHeight += tToolbarStyle.CalcHeight (new GUIContent ("A"), tWidthUsed);

			EditorGUI.DrawRect (new Rect (0,0, tWidthUsed, tHeight), new Color (0.6f, 0.6f, 0.6f, 1.0f));
			EditorGUILayout.HelpBox (mDescriptionKey, MessageType.None);

			if (mTabSelected > mTabList.Count ()) {
				mTabSelected = 0;
			}
			// the next selected tab
			int tTabSelected = 0;
			// check if tab ids necessary
			if (mTabList.Length > 1) {
				GUILayout.Space (5.0f);
				Rect tRect = GUILayoutUtility.GetLastRect ();
				EditorGUI.DrawRect (new Rect (tRect.x, tRect.y, tWidthUsed, 35.0f), new Color (0.6f, 0.6f, 0.6f, 1.0f));
				EditorGUI.DrawRect (new Rect (tRect.x, tRect.y + 35.0f, tWidthUsed, 1.0f), new Color (0.5f, 0.5f, 0.5f, 1.0f));
				if (mTabList.Length < 10) {
					tTabSelected = GUILayout.Toolbar (mTabSelected, mTabList);
				} else {
					tTabSelected = EditorGUILayout.Popup (mTabSelected, mTabList);
				}
			}

			bool tAutoselect = false;
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode==KeyCode.Tab && Event.current.shift) {
//				if (Event.current.keyCode==KeyCode.Tab && Event.current.shift) {
				tTabSelected++;
				if (tTabSelected >= mTabList.Length) {
					tTabSelected = 0;
				}
				tAutoselect = true;
				Event.current.Use ();
			}
			// select the good class to show
			if (tTabSelected >= mTabTypeList.Count ()) {
				tTabSelected = 0;
			}
			if (mTabTypeList.Count ()>0) {
				Type tType = mTabTypeList [tTabSelected];
				if (mTabSelected != tTabSelected) {
					SetClassInEdition (tType);
				}
				mTabSelected = tTabSelected;
				GUILayout.Space (5.0f);
				var tMethodInfo = tType.GetMethod (NWDSelector.NWDBasis_DRAW_IN_EDITOR_SELECTOR, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null) {
					//Debug.Log ("I AM FINDING THE DRAWING METHOD");
					tMethodInfo.Invoke (null, new object[]{ this, tAutoselect });
//				tMethodInfo.Invoke(null,null);
				}
			}

		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif
