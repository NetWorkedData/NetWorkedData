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

using UnityEngine;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

using SQLite4Unity3d;
using System.IO;

//=====================================================================================================================
namespace NetWorkedData
{
	public partial  class NWDBasis <K> where K : NWDBasis <K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		// Table EDITOR

		/// <summary>
		/// The scroll position list.
		/// </summary>
		public static Vector2 m_ScrollPositionList;

		/// <summary>
		/// The number of items per page.
		/// </summary>
		public static int m_ItemPerPage = 10;

		/// <summary>
		/// The number of items per page.
		/// </summary>
		public static int m_ItemPerPageSelection = 0;

		public static string[] m_ItemPerPageOptions = new string[] {
			"20", "30", "40", "50", "100", "200", 
		};

		/// <summary>
		/// The selected page.
		/// </summary>
		public static int m_PageSelected = 0;

		/// <summary>
		/// The selected page.
		/// </summary>
		public static int m_MaxPage = 0;

		/// <summary>
		/// The toogle to list page limit.
		/// </summary>
		//public static int m_ToogleToListPageLimit = 15;


		public static bool m_ShowEnable = true;
		public static bool m_ShowDisable = true;
		public static bool m_ShowTrashed = true;
		public static bool m_ShowIntegrityError = true;

		public static void DrawPagesTab ()
		{
			float tWidth = EditorGUIUtility.currentViewWidth;
//			float tWidth = EditorGUIUtility.fieldWidth;
			float tTabWidth = 35.0f;
			float tPopupWidth = 60.0f;
			//Debug.Log ("tWidth = " + tWidth);
			int tToogleToListPageLimit = (int)Math.Floor (tWidth / tTabWidth);
			//Debug.Log ("tToogleToListPageLimit = " + tToogleToListPageLimit);
//			kObjectsInTableList
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
			m_ItemPerPage = int.Parse (m_ItemPerPageOptions [m_ItemPerPageSelection]);
			float tNumberOfPage = ObjectsInEditorTableList.Count / m_ItemPerPage;
			int tPagesExpected = (int)Math.Floor (tNumberOfPage);
			if (tPagesExpected != 0) {
				if (ObjectsInEditorTableList.Count % (tPagesExpected * m_ItemPerPage) != 0) {
					tPagesExpected++;
				}
			}
			if (m_PageSelected > tPagesExpected - 1) {
				m_PageSelected = tPagesExpected - 1;
			}
			m_MaxPage = tPagesExpected + 1;
			string[] tListOfPagesName = new string[tPagesExpected];
			for (int p = 0; p < tPagesExpected; p++) {
				int tP = p + 1;
				tListOfPagesName [p] = "" + tP.ToString ();
			}
			int t_PageSelected = m_PageSelected;
			if (tPagesExpected == 0 || tPagesExpected == 1) {
				// no choose
				t_PageSelected = 0;
			} else if (tPagesExpected < tToogleToListPageLimit) {
				//m_PageSelected = GUILayout.Toolbar (m_PageSelected, tListOfPagesName, GUILayout.ExpandWidth (true));
				t_PageSelected = GUILayout.Toolbar (m_PageSelected, tListOfPagesName, GUILayout.Width (tPagesExpected * tTabWidth));
			} else {
				t_PageSelected = EditorGUILayout.Popup (m_PageSelected, tListOfPagesName, EditorStyles.popup, GUILayout.Width (tPopupWidth));
			}
			if (m_PageSelected != t_PageSelected) {
				NWDDataManager.SharedInstance.UpdateQueueExecute ();
			}
			m_PageSelected = t_PageSelected;
			GUILayout.FlexibleSpace ();
			GUILayout.EndHorizontal ();
		}

		/// <summary>
		/// Draws the table editor.
		/// </summary>
		public static void DrawTableEditor (EditorWindow sEditorWindow)
		{
			GUIStyle tRightLabel = new GUIStyle (EditorStyles.boldLabel);
			tRightLabel.alignment = TextAnchor.MiddleRight;

			GUIStyle tLeftLabel = new GUIStyle (EditorStyles.boldLabel);
			tLeftLabel.alignment = TextAnchor.MiddleLeft;

			GUIStyle tCenterLabel = new GUIStyle (EditorStyles.boldLabel);
			tCenterLabel.alignment = TextAnchor.MiddleCenter;

//			if (TestSaltValid () == false) {
//				EditorGUILayout.HelpBox (NWDConstants.kAlertSaltShortError, MessageType.Error);
//			}
			if (NWDDataManager.SharedInstance.TestSaltMemorizationForAllClass () == false) {
				EditorGUILayout.HelpBox (NWDConstants.kAlertSaltShortError, MessageType.Error);
			}

			GUILayout.Label (NWDConstants.K_APP_TABLE_SEARCH_ZONE, EditorStyles.boldLabel);
			//EditorGUILayout.BeginScrollView (Vector2.zero, EditorStyles.inspectorFullWidthMargins, GUILayout.ExpandWidth (false), GUILayout.ExpandHeight (false));
			// ===========================================
			GUILayout.BeginHorizontal ();
			// -------------------------------------------
			GUILayout.BeginVertical (GUILayout.Width (300));
			// |||||||||||||||||||||||||||||||||||||||||||
			m_SearchInternalName = EditorGUILayout.TextField (NWDConstants.K_APP_TABLE_SEARCH_NAME, m_SearchInternalName, GUILayout.Width (300));
			m_SearchInternalDescription = EditorGUILayout.TextField (NWDConstants.K_APP_TABLE_SEARCH_DESCRIPTION, m_SearchInternalDescription, GUILayout.Width (300));

			// |||||||||||||||||||||||||||||||||||||||||||
			GUILayout.EndVertical ();
			GUILayout.BeginVertical (GUILayout.Width (120));
			// |||||||||||||||||||||||||||||||||||||||||||
			if (GUILayout.Button (NWDConstants.K_APP_TABLE_SEARCH_REMOVE_FILTER, EditorStyles.miniButton, GUILayout.Width (120))) {
				GUI.FocusControl (null);
				SetObjectInEdition (null);
				m_SearchInternalName = "";
				m_SearchInternalDescription = "";
				FilterTableEditor ();
			}
			if (GUILayout.Button (NWDConstants.K_APP_TABLE_SEARCH_FILTER, EditorStyles.miniButton, GUILayout.Width (120))) {
				GUI.FocusControl (null);
				SetObjectInEdition (null);
				FilterTableEditor ();
			}
			GUILayout.EndVertical ();
			// |||||||||||||||||||||||||||||||||||||||||||
			GUILayout.BeginVertical (GUILayout.Width (120));
			if (GUILayout.Button (NWDConstants.K_APP_TABLE_SEARCH_SORT, EditorStyles.miniButton, GUILayout.Width (120))) {
				GUI.FocusControl (null);
				SetObjectInEdition (null);
				ReorderListOfManagementByName ();
			}
			if (GUILayout.Button (NWDConstants.K_APP_TABLE_SEARCH_RELOAD, EditorStyles.miniButton, GUILayout.Width (120))) {
				GUI.FocusControl (null);
				SetObjectInEdition (null);
				m_SearchInternalName = "";
				m_SearchInternalDescription = "";
//				ReloadAllObjects ();
//				LoadTableEditor ();
				ReorderListOfManagementByName ();
			}
			// |||||||||||||||||||||||||||||||||||||||||||
			GUILayout.EndVertical ();


			GUILayout.BeginVertical (GUILayout.Width (300));
			// |||||||||||||||||||||||||||||||||||||||||||
			bool t_ShowEnable = EditorGUILayout.ToggleLeft ("Show Enable", m_ShowEnable, GUILayout.Width (200));
			if (m_ShowEnable != t_ShowEnable) {
				m_ShowEnable = t_ShowEnable;
				FilterTableEditor ();
			}
			bool t_ShowDisable = EditorGUILayout.ToggleLeft ("Show Disable", m_ShowDisable, GUILayout.Width (200));
			if (m_ShowDisable != t_ShowDisable) {
				m_ShowDisable = t_ShowDisable;
				FilterTableEditor ();
			}
			bool t_ShowTrashed = EditorGUILayout.ToggleLeft ("Show Trashed", m_ShowTrashed, GUILayout.Width (200));
			if (m_ShowTrashed != t_ShowTrashed) {
				m_ShowTrashed = t_ShowTrashed;
				FilterTableEditor ();
			}
			bool t_ShowIntegrityError = EditorGUILayout.ToggleLeft ("Show Integrity error", m_ShowIntegrityError, GUILayout.Width (200));
			if (m_ShowIntegrityError != t_ShowIntegrityError) {
				m_ShowIntegrityError = t_ShowIntegrityError;
				FilterTableEditor ();
			}

			// |||||||||||||||||||||||||||||||||||||||||||
			GUILayout.EndVertical ();



			// -------------------------------------------
			GUILayout.EndHorizontal ();
			EditorGUILayout.HelpBox (NWDConstants.K_APP_TABLE_SHORTCUT_ZONE_A + " " +
				NWDConstants.K_APP_TABLE_SHORTCUT_ZONE_B + " " +
				NWDConstants.K_APP_TABLE_SHORTCUT_ZONE_C, MessageType.Info);
			
			// ===========================================
			DrawPagesTab ();


			DrawHeaderInEditor ();

			m_ScrollPositionList = EditorGUILayout.BeginScrollView (m_ScrollPositionList, EditorStyles.inspectorFullWidthMargins, GUILayout.ExpandWidth (true), GUILayout.ExpandHeight (true));
			// ===========================================

			//m_ItemList.Count

			// EVENT USE
			bool tSelectAndClick = false;
			Vector2 tMousePosition = new Vector2 (-200, -200);
			if (Event.current.type == EventType.MouseDown && Event.current.button == 0) {
				tMousePosition = Event.current.mousePosition;
				if (Event.current.alt == true) {
					BTBDebug.LogVerbose ("alt and select", BTBDebugResult.Success);
					tSelectAndClick = true;
				}
			}

			//
			// TODO: add instruction in tab view
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.S) {
				NWDBasis<K> tSelected = NWDDataInspector.ObjectInEdition () as NWDBasis<K>;
				if (tSelected != null) {
					if (ObjectsByReferenceList.Contains (tSelected.Reference)) {
						if (tSelected.XX == 0 && tSelected.TestIntegrity ()) {
							int tIndex = ObjectsByReferenceList.IndexOf (tSelected.Reference);
							ObjectsInEditorTableSelectionList [tIndex] = !ObjectsInEditorTableSelectionList [tIndex];
							Event.current.Use ();
						}
					}
				}
			}
			// TODO: add instruction in tab view
			// KEY DOWN ARROW
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.DownArrow) {
				//BTBDebug.LogVerbose ("KeyDown DownArrow", BTBDebugResult.Success);
				NWDBasis<K> tSelected = NWDDataInspector.ObjectInEdition () as NWDBasis<K>;
				if (tSelected != null) {
					if (ObjectsInEditorTableList.Contains (tSelected.Reference)) {
						int tIndexSelected = ObjectsInEditorTableList.IndexOf (tSelected.Reference);
						if (tIndexSelected < ObjectsInEditorTableList.Count - 1) {
							string tNextReference = ObjectsInEditorTableList.ElementAt (tIndexSelected + 1);
							int tNextObjectIndex = ObjectsByReferenceList.IndexOf (tNextReference);
							SetObjectInEdition (ObjectsList.ElementAt (tNextObjectIndex));
							float tNumberPage = (tIndexSelected + 1) / m_ItemPerPage;
							int tPageExpected = (int)Math.Floor (tNumberPage);
							m_PageSelected = tPageExpected;
							Event.current.Use ();
							sEditorWindow.Focus ();
						}
					} else {
					}
				}
			}

			// TODO: add instruction in tab view
			// KEY UP ARROW
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.UpArrow) {
				//BTBDebug.LogVerbose ("KeyDown UpArrow", BTBDebugResult.Success);
				NWDBasis<K> tSelected = NWDDataInspector.ObjectInEdition () as NWDBasis<K>;
				if (tSelected != null) {
					if (ObjectsInEditorTableList.Contains (tSelected.Reference)) {
						int tIndexSelected = ObjectsInEditorTableList.IndexOf (tSelected.Reference);
						if (tIndexSelected > 0) {
							string tNextReference = ObjectsInEditorTableList.ElementAt (tIndexSelected - 1);
							int tNextObjectIndex = ObjectsByReferenceList.IndexOf (tNextReference);
							float tNumberPage = (tIndexSelected - 1) / m_ItemPerPage;
							int tPageExpected = (int)Math.Floor (tNumberPage);
							m_PageSelected = tPageExpected;
							SetObjectInEdition (ObjectsList.ElementAt (tNextObjectIndex));
							Event.current.Use ();
							sEditorWindow.Focus ();
						}
					} else {
					}
				}
			}

			float tNumberOfPage = ObjectsInEditorTableList.Count / m_ItemPerPage;
			int tPagesExpected = (int)Math.Floor (tNumberOfPage);

			// TODO: add instruction in tab view
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.RightArrow) {
				//BTBDebug.LogVerbose ("KeyDown RightArrow", BTBDebugResult.Success);
				if (m_PageSelected < tPagesExpected) {
					m_PageSelected++;
					// TODO : reselect first object
					string tNextReference = ObjectsInEditorTableList.ElementAt (m_ItemPerPage * m_PageSelected);
					int tNextObjectIndex = ObjectsByReferenceList.IndexOf (tNextReference);
					SetObjectInEdition (ObjectsList.ElementAt (tNextObjectIndex));
					Event.current.Use ();
					sEditorWindow.Focus ();
				} else {
				}
			}


			// TODO: add instruction in tab view
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.LeftArrow) {
				//BTBDebug.LogVerbose ("KeyDown LeftArrow", BTBDebugResult.Success);
				if (m_PageSelected > 0) {
					m_PageSelected--;
					// TODO : reselect first object
					string tNextReference = ObjectsInEditorTableList.ElementAt (m_ItemPerPage * m_PageSelected);
					int tNextObjectIndex = ObjectsByReferenceList.IndexOf (tNextReference);
					SetObjectInEdition (ObjectsList.ElementAt (tNextObjectIndex));
					Event.current.Use ();
					sEditorWindow.Focus ();
				} else {
				}
			}


			if (m_PageSelected < 0) {
				m_PageSelected = 0;
			}
			if (m_PageSelected > tPagesExpected) {
				m_PageSelected = tPagesExpected;
			}

			for (int i = 0; i < m_ItemPerPage; i++) {
				int tItemIndexInPage = m_ItemPerPage * m_PageSelected + i;
				if (tItemIndexInPage < ObjectsInEditorTableList.Count) {
					string tReference = ObjectsInEditorTableList.ElementAt (tItemIndexInPage);
					int tObjectIndex = ObjectsByReferenceList.IndexOf (tReference);
					if (ObjectsList.Count > tObjectIndex && tObjectIndex >= 0) {
						NWDBasis<K> tObject = (NWDBasis<K>)ObjectsList.ElementAt (tObjectIndex);
						if (tObject != null) {
							tObject.DrawRowInEditor (tMousePosition, sEditorWindow, tSelectAndClick);
						}
					}
				}
			}


			// ===========================================
			EditorGUILayout.EndScrollView ();
			// -------------------------------------------

			GUILayout.Space (5.0f);

			Rect tRect = GUILayoutUtility.GetLastRect ();
			EditorGUI.DrawRect (new Rect (tRect.x - 10.0f, tRect.y, 4096.0f, 1024.0f), new Color (0.0f, 0.0f, 0.0f, 0.10f));
			EditorGUI.DrawRect (new Rect (tRect.x - 10.0f, tRect.y, 4096.0f, 1.0f), new Color (0.0f, 0.0f, 0.0f, 0.30f));


			DrawPagesTab ();




			GUILayout.Space (5.0f);

//			GUILayout.Label ("Management", EditorStyles.boldLabel);

			int tSelectionCount = 0;
			int tActualItems = ObjectsInEditorTableSelectionList.Count;
			for (int tSelect = 0; tSelect < tActualItems; tSelect++) {
				if (ObjectsInEditorTableSelectionList [tSelect] == true) {
					tSelectionCount++;
				}
			}

			GUILayout.BeginHorizontal ();
			// -------------------------------------------
			GUILayout.BeginVertical (GUILayout.Width (120));
			// |||||||||||||||||||||||||||||||||||||||||||
			if (tSelectionCount == 0) {
				GUILayout.Label (NWDConstants.K_APP_TABLE_NO_SELECTED_OBJECT, tCenterLabel);
			} else if (tSelectionCount == 1) {
				GUILayout.Label (NWDConstants.K_APP_TABLE_ONE_SELECTED_OBJECT, tCenterLabel);
			} else {
				GUILayout.Label (tSelectionCount + NWDConstants.K_APP_TABLE_XX_SELECTED_OBJECT, tCenterLabel);
			}

			EditorGUI.BeginDisabledGroup (tSelectionCount == tActualItems);
			if (GUILayout.Button (NWDConstants.K_APP_TABLE_SELECT_ALL, EditorStyles.miniButton)) {
//				for (int tSelect = 0; tSelect < tActualItems; tSelect++) {
//					kObjectsSelectionList [tSelect] = true;
//				}
				SelectAllObjectInTableList ();
			}
			EditorGUI.EndDisabledGroup ();

			EditorGUI.BeginDisabledGroup (tSelectionCount == 0);
			if (GUILayout.Button (NWDConstants.K_APP_TABLE_DESELECT_ALL, EditorStyles.miniButton)) {
//				for (int tSelect = 0; tSelect < tActualItems; tSelect++) {
//					kObjectsSelectionList [tSelect] = false;
//				}
				DeselectAllObjectInTableList ();
			}
			EditorGUI.EndDisabledGroup ();

			if (GUILayout.Button (NWDConstants.K_APP_TABLE_INVERSE, EditorStyles.miniButton)) {
//				for (int tSelect = 0; tSelect < tActualItems; tSelect++) {
//					kObjectsSelectionList [tSelect] = !kObjectsSelectionList [tSelect];
//				}
				InverseSelectionOfAllObjectInTableList ();
			}
			EditorGUI.EndDisabledGroup ();

			if (GUILayout.Button (NWDConstants.K_APP_TABLE_SELECT_DISABLED, EditorStyles.miniButton)) {
				SelectAllObjectDisableInTableList ();
			}

			// |||||||||||||||||||||||||||||||||||||||||||
			GUILayout.EndHorizontal ();
			GUILayout.BeginVertical (GUILayout.Width (120));
			// |||||||||||||||||||||||||||||||||||||||||||
//			GUILayout.Label ("For all selected objects");

			EditorGUI.BeginDisabledGroup (tSelectionCount == 0);
			GUILayout.Label (NWDConstants.K_APP_TABLE_ACTIONS, tCenterLabel);

			if (GUILayout.Button (NWDConstants.K_APP_TABLE_REACTIVE, EditorStyles.miniButton)) {
				for (int tSelect = 0; tSelect < tActualItems; tSelect++) {
					if (ObjectsInEditorTableSelectionList [tSelect] == true) {
						NWDBasis<K> tObject = (NWDBasis<K>)ObjectsList.ElementAt (tSelect);
						tObject.EnableMe ();
						//tObject.AddMeToUpdateQueue();
					}
				}
			}

			if (GUILayout.Button (NWDConstants.K_APP_TABLE_DISACTIVE, EditorStyles.miniButton)) {
				for (int tSelect = 0; tSelect < tActualItems; tSelect++) {
					if (ObjectsInEditorTableSelectionList [tSelect] == true) {
						NWDBasis<K> tObject = (NWDBasis<K>)ObjectsList.ElementAt (tSelect);
						tObject.DisableMe ();
						//tObject.AddMeToUpdateQueue();
					}
				}
			}

			if (GUILayout.Button (NWDConstants.K_APP_TABLE_DUPPLICATE, EditorStyles.miniButton)) {
				NWDBasis<K> tNextObjectSelected = null;
				int tNewObect = 0;
				for (int tSelect = 0; tSelect < tActualItems; tSelect++) {
					if (ObjectsInEditorTableSelectionList [tSelect] == true) {
						tNewObect++;
						NWDBasis<K> tObject = (NWDBasis<K>)ObjectsList.ElementAt (tSelect);
						NWDBasis<K> tNextObject = tObject.DuplicateMe ();
						AddObjectInListOfEdition (tNextObject);
						tNextObjectSelected = tNextObject;
					}
				}
				if (tNewObect != 1) {
					tNextObjectSelected = null;
				}
				SetObjectInEdition (tNextObjectSelected);
				//ReorderListOfManagementByName ();
				m_PageSelected = m_MaxPage * 3;
			}

			if (GUILayout.Button (NWDConstants.K_APP_TABLE_UPDATE, EditorStyles.miniButton)) {
				for (int tSelect = 0; tSelect < tActualItems; tSelect++) {
					if (ObjectsInEditorTableSelectionList [tSelect] == true) {
						NWDBasis<K> tObject = (NWDBasis<K>)ObjectsList.ElementAt (tSelect);
						tObject.UpdateMe ();
					}
				}
			}


			EditorGUI.EndDisabledGroup ();

			// |||||||||||||||||||||||||||||||||||||||||||
			GUILayout.EndVertical ();

			GUILayout.BeginVertical (GUILayout.Width (120));


			EditorGUI.BeginDisabledGroup (tSelectionCount == 0);

			GUILayout.Label (NWDConstants.K_APP_TABLE_DELETE_WARNING, tCenterLabel);
			if (GUILayout.Button (NWDConstants.K_APP_TABLE_DELETE_BUTTON, EditorStyles.miniButton)) {
				string tDialog = "";
				if (tSelectionCount == 0) {
					tDialog = NWDConstants.K_APP_TABLE_DELETE_NO_OBJECT;
				} else if (tSelectionCount == 1) {
					tDialog = NWDConstants.K_APP_TABLE_DELETE_ONE_OBJECT;
				} else {
					tDialog = NWDConstants.K_APP_TABLE_DELETE_X_OBJECTS_A + tSelectionCount + NWDConstants.K_APP_TABLE_DELETE_X_OBJECTS_B;
				}
				if (EditorUtility.DisplayDialog (NWDConstants.K_APP_TABLE_DELETE_ALERT,
					    tDialog,
					    NWDConstants.K_APP_TABLE_DELETE_YES,
					    NWDConstants.K_APP_TABLE_DELETE_NO)) {

					List <object> tListToDelete = new List<object> ();
					for (int tSelect = 0; tSelect < tActualItems; tSelect++) {
						if (ObjectsInEditorTableSelectionList [tSelect] == true) {
							NWDBasis<K> tObject = (NWDBasis<K>)ObjectsList.ElementAt (tSelect);
							tListToDelete.Add ((object)tObject);
						}
					}
					foreach (object tObject in tListToDelete) {
						NWDBasis<K> tObjectToDelete = (NWDBasis<K>)tObject;
						RemoveObjectInListOfEdition (tObjectToDelete);
						tObjectToDelete.DeleteMe ();
					}
					SetObjectInEdition (null);
					NWDDataManager.SharedInstance.RepaintWindowsInManager (ClassType ());
				}
			}


			if (GUILayout.Button (NWDConstants.K_APP_TABLE_TRASH_ZONE, EditorStyles.miniButton)) {
				string tDialog = "";
				if (tSelectionCount == 0) {
					tDialog = NWDConstants.K_APP_TABLE_TRASH_NO_OBJECT;
				} else if (tSelectionCount == 1) {
					tDialog = NWDConstants.K_APP_TABLE_TRASH_ONE_OBJECT;
				} else {
					tDialog = NWDConstants.K_APP_TABLE_TRASH_X_OBJECT_A + tSelectionCount + NWDConstants.K_APP_TABLE_TRASH_X_OBJECT_B;
				}
				if (EditorUtility.DisplayDialog (NWDConstants.K_APP_TABLE_TRASH_ALERT,
					    tDialog,
					    NWDConstants.K_APP_TABLE_TRASH_YES,
					    NWDConstants.K_APP_TABLE_TRASH_NO)) {

					List <object> tListToTrash = new List<object> ();
					for (int tSelect = 0; tSelect < tActualItems; tSelect++) {
						if (ObjectsInEditorTableSelectionList [tSelect] == true) {
							NWDBasis<K> tObject = (NWDBasis<K>)ObjectsList.ElementAt (tSelect);
							tListToTrash.Add ((object)tObject);
						}
					}
					foreach (object tObject in tListToTrash) {
						NWDBasis<K> tObjectToTrash = (NWDBasis<K>)tObject;
						tObjectToTrash.TrashMe ();
					}
					SetObjectInEdition (null);
					//					sEditorWindow.Repaint ();
					NWDDataManager.SharedInstance.RepaintWindowsInManager (ClassType ());
				}
			}

			EditorGUI.EndDisabledGroup ();

			// |||||||||||||||||||||||||||||||||||||||||||
			GUILayout.EndVertical ();

			GUILayout.BeginVertical (GUILayout.Width (120));
			// |||||||||||||||||||||||||||||||||||||||||||

			GUILayout.Label (NWDConstants.K_APP_TABLE_PAGINATION, tCenterLabel);
			//			GUILayout.BeginHorizontal ();
			int t_ItemPerPageSelection = EditorGUILayout.Popup (m_ItemPerPageSelection, m_ItemPerPageOptions, EditorStyles.popup);
			if (t_ItemPerPageSelection != m_ItemPerPageSelection) {
				m_PageSelected = 0;
			}
			m_ItemPerPageSelection = t_ItemPerPageSelection;
			int tRealReference = ObjectsByReferenceList.Count;
			if (tRealReference == 0) {
				GUILayout.Label (NWDConstants.K_APP_TABLE_NO_OBJECT);
			} else if (tRealReference == 1) {
				GUILayout.Label (NWDConstants.K_APP_TABLE_ONE_OBJECT);
			} else {
				GUILayout.Label (tRealReference + NWDConstants.K_APP_TABLE_X_OBJECTS);
			}

			int tResultReference = ObjectsInEditorTableList.Count;
			if (tResultReference == 0) {
				GUILayout.Label (NWDConstants.K_APP_TABLE_NO_OBJECT_FILTERED);
			} else if (tResultReference == 1) {
				GUILayout.Label (NWDConstants.K_APP_TABLE_ONE_OBJECT_FILTERED);
			} else {
				GUILayout.Label (tResultReference + NWDConstants.K_APP_TABLE_X_OBJECTS_FILTERED);
			}

			// |||||||||||||||||||||||||||||||||||||||||||
			GUILayout.EndVertical ();
			GUILayout.FlexibleSpace ();
			GUILayout.BeginVertical (GUILayout.Width (120));
			// |||||||||||||||||||||||||||||||||||||||||||

			GUILayout.Label (NWDConstants.K_APP_TABLE_TOOLS_ZONE, tCenterLabel);
			if (GUILayout.Button (NWDConstants.K_APP_TABLE_SHOW_TOOLS, EditorStyles.miniButton)) {
//				NWDBasisClassInspector tBasisInspector = new NWDBasisClassInspector ();
				NWDBasisClassInspector tBasisInspector = ScriptableObject.CreateInstance<NWDBasisClassInspector> ();
				tBasisInspector.mTypeInEdition = ClassType ();
//				tBasisInspector.mObjectInEdition = null;
//				tBasisInspector.mWindowInEdition = sEditorWindow;
				Selection.activeObject = tBasisInspector;
			}


			// |||||||||||||||||||||||||||||||||||||||||||



			bool tDisableProd = false;
			if (NWDDataManager.SharedInstance.mTypeUnSynchronizedList.Contains (ClassType ())) {
				tDisableProd = true;
			}
			if (AccountDependent () == true) {
				tDisableProd = true;
			}


			GUILayout.EndVertical ();

			GUILayout.BeginVertical (GUILayout.Width (120));

			// SYNCHRONIZATION

			GUILayout.Label (NWDConstants.K_APP_BASIS_CLASS_SYNC, tCenterLabel);

			// SYNCHRO NORMAL

			GUILayout.BeginHorizontal ();
			if (GUILayout.Button (NWDConstants.K_DEVELOPMENT_NAME, EditorStyles.miniButton)) {
				SynchronizationFromWebService (false, NWDAppConfiguration.SharedInstance.DevEnvironment);
			}

			if (GUILayout.Button (NWDConstants.K_PREPRODUCTION_NAME, EditorStyles.miniButton)) {
				SynchronizationFromWebService (false, NWDAppConfiguration.SharedInstance.PreprodEnvironment);
			}
			EditorGUI.BeginDisabledGroup (tDisableProd);
			if (GUILayout.Button (NWDConstants.K_PRODUCTION_NAME, EditorStyles.miniButton)) {
				if (EditorUtility.DisplayDialog (NWDConstants.K_SYNC_ALERT_TITLE,
					    NWDConstants.K_SYNC_ALERT_MESSAGE,
					    NWDConstants.K_SYNC_ALERT_OK,
					    NWDConstants.K_SYNC_ALERT_CANCEL)) {
					SynchronizationFromWebService (false, NWDAppConfiguration.SharedInstance.ProdEnvironment);
				}
			}
			EditorGUI.EndDisabledGroup ();
			GUILayout.EndHorizontal ();


			// FORCE SYNCHRO
			GUILayout.BeginHorizontal ();
			if (GUILayout.Button (NWDConstants.K_DEVELOPMENT_NAME + " force", EditorStyles.miniButton)) {
				SynchronizationFromWebService (true, NWDAppConfiguration.SharedInstance.DevEnvironment);
			}
			if (GUILayout.Button (NWDConstants.K_PREPRODUCTION_NAME + " force", EditorStyles.miniButton)) {
				SynchronizationFromWebService (true, NWDAppConfiguration.SharedInstance.PreprodEnvironment);
			}
			EditorGUI.BeginDisabledGroup (tDisableProd);
			if (GUILayout.Button (NWDConstants.K_PRODUCTION_NAME + " force", EditorStyles.miniButton)) {
				if (EditorUtility.DisplayDialog (NWDConstants.K_SYNC_ALERT_TITLE,
					    NWDConstants.K_SYNC_ALERT_MESSAGE,
					    NWDConstants.K_SYNC_ALERT_OK,
					    NWDConstants.K_SYNC_ALERT_CANCEL)) {
					SynchronizationFromWebService (true, NWDAppConfiguration.SharedInstance.ProdEnvironment);
				}
			}
			EditorGUI.EndDisabledGroup ();
			GUILayout.EndHorizontal ();




//			// FORCE TO TRASH
//			GUILayout.BeginHorizontal ();
//			if (GUILayout.Button (NWDConstants.K_DEVELOPMENT_NAME + " trash", EditorStyles.miniButton)) 
//			{
//				TrashFromWebService (NWDAppConfiguration.SharedInstance.DevEnvironment);
//			}
//			if (GUILayout.Button (NWDConstants.K_PREPRODUCTION_NAME + " trash", EditorStyles.miniButton)) 
//			{
//				TrashFromWebService (NWDAppConfiguration.SharedInstance.PreprodEnvironment);
//			}
//			EditorGUI.BeginDisabledGroup (tDisableProd);
//			if (GUILayout.Button (NWDConstants.K_PRODUCTION_NAME + " trash", EditorStyles.miniButton)) 
//			{
//				if (EditorUtility.DisplayDialog (NWDConstants.K_SYNC_ALERT_TITLE,
//					NWDConstants.K_SYNC_ALERT_MESSAGE,
//					NWDConstants.K_SYNC_ALERT_OK,
//					NWDConstants.K_SYNC_ALERT_CANCEL)) {
//					TrashFromWebService (NWDAppConfiguration.SharedInstance.ProdEnvironment);
//				}
//			}
//			EditorGUI.EndDisabledGroup ();
//			GUILayout.EndHorizontal ();
//
//



			// |||||||||||||||||||||||||||||||||||||||||||
			GUILayout.EndVertical ();

			GUILayout.BeginVertical (GUILayout.Width (120));

			// |||||||||||||||||||||||||||||||||||||||||||
			GUILayout.Label (NWDConstants.K_APP_TABLE_ADD_ZONE, tCenterLabel);
			if (GUILayout.Button (NWDConstants.K_APP_TABLE_ADD_ROW, EditorStyles.miniButton)) {
				// add card editor change
//				NWDBasis<K> tNewObject = NWDBasis<K>.NewInstance ();
//				AddObjectInListOfEdition (tNewObject);
				K tNewObject = NWDBasis<K>.NewObject ();
				m_PageSelected = m_MaxPage * 3;
				SetObjectInEdition (tNewObject);
//				sEditorWindow.Repaint ();
				NWDDataManager.SharedInstance.RepaintWindowsInManager (ClassType ());
			}

			// |||||||||||||||||||||||||||||||||||||||||||
			GUILayout.EndVertical ();
			// -------------------------------------------
			GUILayout.EndHorizontal ();

			GUILayout.Space (10.0f);


			//			GUILayout.Label ("Edit card", EditorStyles.boldLabel);
			//			m_ScrollPositionCard = EditorGUILayout.BeginScrollView (m_ScrollPositionCard, GUILayout.ExpandWidth (true), GUILayout.ExpandHeight (true));
			//			// ===========================================
			//			// ===========================================
			//			EditorGUILayout.EndScrollView ();
		}

		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================