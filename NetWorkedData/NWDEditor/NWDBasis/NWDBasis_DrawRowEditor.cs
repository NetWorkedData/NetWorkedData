using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	public partial  class NWDBasis <K> where K : NWDBasis <K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		// Header design
		public static float kHeaderHeight = 20.0f;
		public static float kHeaderHeightSpace = 6.0f;
		public static float kHeaderLineStroke = 1.0f;
		public static Color kHeaderColorBackground = new Color (0.0f, 0.0f, 0.0f, 0.35f);
		public static Color kHeaderColorLine = new Color (0.0f, 0.0f, 0.0f, 0.55f);
		//-------------------------------------------------------------------------------------------------------------
		// Row design
		public static float kRowOutMarge = 25.0f;
		public static float kRowHeight = 25.0f;
		public static float kRowHeightSpace = 5.0f;
		//static Color kRowColorNormal = new Color (0.0f, 0.0f, 0.0f, 0.30f);
		public static Color kRowColorSelected = new Color (0.55f, 0.55f, 1.00f, 0.25f);
		public static Color kRowColorError = new Color (1.00f, 0.00f, 0.00f, 0.55f);
		public static Color kRowColorTrash = new Color (0.00f, 0.00f, 0.00f, 0.45f);
		public static Color kRowColorDisactive = new Color (0.00f, 0.00f, 0.00f, 0.35f);
		public static float kRowLineStroke = 2.0f;
		public static Color kRowColorLine = new Color (0.0f, 0.0f, 0.0f, 0.25f);
		//-------------------------------------------------------------------------------------------------------------
		// Columns Size
		public static float kOriginWidth = 1.0f;
		public static float kSelectWidth = 20.0f;
		public static float kIDWidth = 30.0f;
		public static float kPrefabWidth = 30.0f;
		public static float kDescriptionMinWidth = 200.0f;
		public static float kSyncWidth = 30.0f;
		public static float kDevSyncWidth = 30.0f;
		public static float kPreprodSyncWidth = 30.0f;
		public static float kProdSyncWidth = 30.0f;
		public static float kActiveWidth = 70.0f;
		public static float kReferenceWidth = 230.0f;
		//-------------------------------------------------------------------------------------------------------------
		// Icons for Sync
		static public Texture2D kImageRed = AssetDatabase.LoadAssetAtPath<Texture2D> (NWDFindPackage.PathOfPackage ("/NWDEditor/NWDNativeImages/NWDRed.psd"));
		static public Texture2D kImageGreen = AssetDatabase.LoadAssetAtPath<Texture2D> (NWDFindPackage.PathOfPackage ("/NWDEditor/NWDNativeImages/NWDGreen.psd"));
		static public Texture2D kImageOrange = AssetDatabase.LoadAssetAtPath<Texture2D> (NWDFindPackage.PathOfPackage ("/NWDEditor/NWDNativeImages/NWDOrange.psd"));
		static public Texture2D kImageForbidden = AssetDatabase.LoadAssetAtPath<Texture2D> (NWDFindPackage.PathOfPackage ("/NWDEditor/NWDNativeImages/NWDForbidden.psd"));
		//-------------------------------------------------------------------------------------------------------------
		public static void DrawHeaderInEditor ()
		{
			GUILayout.BeginHorizontal (GUILayout.Height (kHeaderHeight));
			GUILayout.Label (" ", EditorStyles.boldLabel, GUILayout.Width(kOriginWidth));
			Rect tRect = GUILayoutUtility.GetLastRect ();
			Rect tRectArea = new Rect (tRect.x-kHeaderHeight, tRect.y-kHeaderHeightSpace/2.0f, 4096.0f, kHeaderHeight+kHeaderHeightSpace);
			EditorGUI.DrawRect (tRectArea, kHeaderColorBackground);
			GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_SELECT, EditorStyles.boldLabel, GUILayout.Width(kSelectWidth));
			GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_ID, EditorStyles.boldLabel, GUILayout.Width(kIDWidth));
			GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_PREFAB, EditorStyles.boldLabel, GUILayout.Width(kPrefabWidth));
			GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_DESCRIPTION, EditorStyles.boldLabel, GUILayout.MinWidth(kDescriptionMinWidth));
			GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_SYNCHRO, EditorStyles.boldLabel, GUILayout.Width(kSyncWidth));
			GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_DEVSYNCHRO, EditorStyles.boldLabel, GUILayout.Width(kDevSyncWidth));
			GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_PREPRODSYNCHRO, EditorStyles.boldLabel, GUILayout.Width(kPreprodSyncWidth));
			GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_PRODSYNCHRO, EditorStyles.boldLabel, GUILayout.Width(kProdSyncWidth));
			GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_STATUT, EditorStyles.boldLabel, GUILayout.Width(kActiveWidth));
			GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_REFERENCE, EditorStyles.boldLabel, GUILayout.Width(kReferenceWidth));
			Rect tRectLine = new Rect (tRect.x-kHeaderHeight, tRect.y+kHeaderHeight+kHeaderLineStroke, 4096.0f, kHeaderLineStroke);
			EditorGUI.DrawRect (tRectLine, kHeaderColorLine);
			GUILayout.EndHorizontal ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public void RowInformation (Rect sRect)
		{ 
			// prepare text
			string tString = "<size=13><b>" + InternalKey + "</b></size>     <i>(" + InternalDescription + ")</i>";
			tString = tString.Replace ("()", "");
			string tStringReference = "<" + Reference + ">";
			// prepare prefab 
			GameObject tObject = null;
			if (Preview != null && Preview != "") {
				tObject = AssetDatabase.LoadAssetAtPath (Preview, typeof(GameObject)) as GameObject;
			}
			if (mGameObjectEditor == null) {
				mGameObjectEditor = Editor.CreateEditor (tObject);
			}
			// prepare State infos
			string sStateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_OK;
			//draw informations
			int tIndex = ObjectsByReferenceList.IndexOf (Reference);
			// test Integrity, trash, etc. to draw the good color of row
			// and draw the toogle to select this row
			if (TestIntegrity () == false) {
				EditorGUI.DrawRect (sRect, kRowColorError);
				ObjectsInEditorTableSelectionList [tIndex] = false;
				GUILayout.Label ("!!!", GUILayout.Width(kSelectWidth));
				sStateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_ERROR;
				tString = "<color=#a52a2aff>" +tString +"</color>";
			} else if (XX > 0) {
				EditorGUI.DrawRect (sRect, kRowColorTrash);
				ObjectsInEditorTableSelectionList [tIndex] = false;
				GUILayout.Label ("   ", GUILayout.Width(kSelectWidth));
				sStateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_TRASH;
				tString = "<color=#444444ff>" +tString +"</color>";
			} else {
				if (AC == false) {
					EditorGUI.DrawRect (sRect, kRowColorDisactive);
					sStateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_DISACTIVE;
					tString = "<color=#555555ff>" +tString +"</color>";
				}
				ObjectsInEditorTableSelectionList [tIndex] = EditorGUILayout.ToggleLeft ("", ObjectsInEditorTableSelectionList [tIndex], GUILayout.Width(kSelectWidth));
			}
			// Draw internal informations
			var tStyle = GUI.skin.GetStyle("Label");
			tStyle.alignment = TextAnchor.MiddleLeft;
			var tStyleCenter = GUI.skin.GetStyle("Label");
			tStyleCenter.alignment = TextAnchor.MiddleCenter;
			var tStyleRight = GUI.skin.GetStyle("Label");
			tStyleRight.alignment = TextAnchor.MiddleRight;
			GUILayout.Label (ID.ToString (), GUILayout.Width(kIDWidth));
			GUILayout.Label (" ", GUILayout.Width(kPrefabWidth));
			Rect tRectPreview = GUILayoutUtility.GetLastRect ();
			GUIStyle tStyleForInfos = new GUIStyle (EditorStyles.label);
			tStyleForInfos.richText = true;
			GUILayout.Label (tString, tStyleForInfos, GUILayout.MinWidth(kDescriptionMinWidth));
			tStyle.alignment = TextAnchor.MiddleRight;
			// Draw Sync State
			Texture2D tImageSync = kImageRed; 
			if (DS > 0) {
				tImageSync = kImageGreen; 
			}
			GUILayout.Label(tImageSync,GUILayout.Width (kSyncWidth),GUILayout.Height (kSyncWidth/2.0f));
			// Draw Dev Sync State
			Texture2D tImageDevSync = kImageRed; 
			if (DevSync > 0) {
				tImageDevSync = kImageGreen;
			}
			GUILayout.Label(tImageDevSync,GUILayout.Width (kSyncWidth),GUILayout.Height (kSyncWidth/2.0f));
			// Draw Preprod Sync State
			Texture2D tImagePreprodSync = kImageRed; 
			if (PreprodSync > 0) {
				if (PreprodSync > DevSync) {
					tImagePreprodSync = kImageGreen;
				} else {
					tImagePreprodSync = kImageOrange;
				}
			}

			bool tDisableProd = false;
			if (NWDDataManager.SharedInstance.mTypeUnSynchronizedList.Contains (ClassType ())) {
				tDisableProd = true;
			}
			if (AccountDependent () == true) {
				tDisableProd = true;
			}

			GUILayout.Label(tImagePreprodSync,GUILayout.Width (kSyncWidth),GUILayout.Height (kSyncWidth/2.0f));
			// Draw Prod Sync State
			Texture2D tImageProdSync = kImageRed; 
			if (tDisableProd == true) {
				tImageProdSync = kImageForbidden;
			}
			else
			{
				if (ProdSync > 0) {
					if (ProdSync > DevSync && ProdSync > PreprodSync) {
						tImageProdSync = kImageGreen;
					} else {
						tImageProdSync = kImageOrange;
					}
				}
			}
			GUILayout.Label(tImageProdSync,GUILayout.Width (kSyncWidth),GUILayout.Height (kSyncWidth/2.0f));
			// Draw State
			GUILayout.Label (sStateInfos,tStyleCenter, GUILayout.Width (kActiveWidth));
			// Draw Reference
			GUILayout.Label (tStringReference, tStyleRight, GUILayout.Width(kReferenceWidth));
			// Draw prefab preview
			Texture2D tTexture2D = AssetPreview.GetAssetPreview (tObject);
			if (tTexture2D != null) {
				EditorGUI.DrawPreviewTexture (new Rect (tRectPreview.x, tRectPreview.y, 20.0f, 20.0f), tTexture2D);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public Rect DrawRowInEditor (Vector2 sMouseClickPosition)
		{
			// start line
			GUILayout.Space(kRowHeightSpace);
			GUILayout.BeginHorizontal (GUILayout.Height (kRowHeight));
			GUILayout.Label ("", GUILayout.Width(kOriginWidth));
			Rect tRect = GUILayoutUtility.GetLastRect ();
			// determine rect to select and draw
			Rect rRect = new Rect (tRect.x-10, tRect.y-5, 4096.0f, kRowHeight+5);
			// determine rect to analyze
			Rect rRectAnalyze = new Rect (tRect.x-10, tRect.y-5, 4096.0f, kRowHeight+10);
			// check click in rect
			if (rRectAnalyze.Contains (sMouseClickPosition)) 
			{
				NWDDataManager.SharedInstance.UpdateQueueExecute();
				GUI.FocusControl (null);
				SetObjectInEdition (this);
			}
			// check if object is in edition and draw color rect overlay
			if (IsObjectInEdition (this) == true)
			{
				EditorGUI.DrawRect (rRect, kRowColorSelected);
			}
			// draw informations
			RowInformation (rRect);
			// draw line to delimit the rect
			tRect = new Rect (tRect.x-kRowOutMarge, tRect.y+kRowHeight, 4096.0f, kRowLineStroke);
			EditorGUI.DrawRect (tRect, kRowColorLine);
			// finish line
			GUILayout.EndHorizontal ();
			return rRect;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
	}
}
//=====================================================================================================================