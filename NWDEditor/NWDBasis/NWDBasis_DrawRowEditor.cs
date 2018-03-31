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
using System.IO;
using System.Reflection;

using UnityEngine;
using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
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
		public static float kRowHeight = 30.0f;
		public static float kRowHeightImage = 20.0f;
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
		public static float kSyncWidth = 40.0f;
		public static float kDevSyncWidth = 40.0f;
		public static float kPreprodSyncWidth = 40.0f;
		public static float kProdSyncWidth = 40.0f;
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

			GUIStyle tStyleLeft = new GUIStyle (EditorStyles.boldLabel);
			tStyleLeft.alignment = TextAnchor.MiddleLeft;

			GUIStyle tStyleCenter = new GUIStyle (EditorStyles.boldLabel);
			tStyleCenter.alignment = TextAnchor.MiddleCenter;

			GUIStyle tStyleRight = new GUIStyle (EditorStyles.boldLabel);
			tStyleRight.alignment = TextAnchor.MiddleRight;

			GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_SELECT, tStyleLeft, GUILayout.Width(kSelectWidth));
			GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_ID, tStyleLeft, GUILayout.Width(kIDWidth));
			GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_PREFAB, tStyleLeft, GUILayout.Width(kPrefabWidth));
			GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_DESCRIPTION, tStyleLeft, GUILayout.MinWidth(kDescriptionMinWidth));

			GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_SYNCHRO, tStyleCenter, GUILayout.Width(kSyncWidth));
			GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_DEVSYNCHRO, tStyleCenter, GUILayout.Width(kDevSyncWidth));
			GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_PREPRODSYNCHRO, tStyleCenter, GUILayout.Width(kPreprodSyncWidth));
			GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_PRODSYNCHRO,tStyleCenter, GUILayout.Width(kProdSyncWidth));

			GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_STATUT, tStyleCenter, GUILayout.Width(kActiveWidth));
			GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_REFERENCE +" ", tStyleRight, GUILayout.Width(kReferenceWidth));
			Rect tRectLine = new Rect (tRect.x-kHeaderHeight, tRect.y+kHeaderHeight+kHeaderLineStroke, 4096.0f, kHeaderLineStroke);
			EditorGUI.DrawRect (tRectLine, kHeaderColorLine);
			GUILayout.EndHorizontal ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public void RowInformation (Rect sRect)
		{
            // Draw internal informations
            var tStyleLeft = new GUIStyle(EditorStyles.label);
            tStyleLeft.alignment = TextAnchor.MiddleLeft;
            var tStyleCenter = new GUIStyle(EditorStyles.label);
            tStyleCenter.alignment = TextAnchor.MiddleCenter;
            var tStyleRight = new GUIStyle(EditorStyles.label);
            tStyleRight.alignment = TextAnchor.MiddleRight;

			// prepare text
			Rect rRectColored = new Rect (sRect.x-5, sRect.y, sRect.width+1024, sRect.height);

            // check error in data 
            string tIsInError = "";
            //IsInErrorCheck();
            if (InError==true)
            {
                tIsInError = NWDConstants.K_WARNING;
            }


            string tString = "<size=13><color=red>"+tIsInError+"</color><b>" + InternalKey + "</b></size>     <i>(" + InternalDescription + ")</i> ";
            // to check the versioning active this line
            //tString+= "minversion = '" + MinVersion.ToString()+"'";

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


            // TOO LONG TOO LAG ?
            //Texture2D tTextureOfClass = TextureOfClass();
            //if (tTextureOfClass != null)
            //{
            //    //GUILayout.Label(tTextureOfClass, tStyleCenter, GUILayout.Width(kPreprodSyncWidth), GUILayout.Height(kRowHeightImage));
            //    //GUILayout.DrawTexture(new Rect(tX + tWidth / 2.0F - 16, tY, 32, 32), tTextureOfClass);
            //}


			if (TestIntegrity () == false) {
				EditorGUI.DrawRect (rRectColored, kRowColorError);
				ObjectsInEditorTableSelectionList [tIndex] = false;
				GUILayout.Label ("!!!", GUILayout.Width(kSelectWidth));
				sStateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_ERROR;
				sStateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_INTEGRITY_ERROR;
				tString = "<color=#a52a2aff>" +tString +"</color>";
			} else if (XX > 0) {
				EditorGUI.DrawRect (rRectColored, kRowColorTrash);
				ObjectsInEditorTableSelectionList [tIndex] = false;
				GUILayout.Label ("   ", GUILayout.Width(kSelectWidth));
				sStateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_TRASH;
				tString = "<color=#444444ff>" +tString +"</color>";
			} else {
				if (AC == false) {
					EditorGUI.DrawRect (rRectColored, kRowColorDisactive);
					sStateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_DISACTIVE;
					tString = "<color=#555555ff>" +tString +"</color>";
				}
				ObjectsInEditorTableSelectionList [tIndex] = EditorGUILayout.ToggleLeft ("", ObjectsInEditorTableSelectionList [tIndex], GUILayout.Width(kSelectWidth));
			}

			GUILayout.Label (ID.ToString (), GUILayout.Width(kIDWidth));
			GUILayout.Label (" ", GUILayout.Width(kPrefabWidth));
			Rect tRectPreview = GUILayoutUtility.GetLastRect ();
			GUIStyle tStyleForInfos = new GUIStyle (EditorStyles.label);
			tStyleForInfos.richText = true;
			GUILayout.Label (tString, tStyleForInfos, GUILayout.MinWidth(kDescriptionMinWidth));
			tStyleLeft.alignment = TextAnchor.MiddleRight;
			// Draw Sync State
			Texture2D tImageSync = kImageRed; 
			if (DS > 0) {
				tImageSync = kImageGreen; 
			}
			GUILayout.Label(tImageSync,tStyleCenter,GUILayout.Width (kSyncWidth),GUILayout.Height (kRowHeightImage));
			// Draw Dev Sync State
			Texture2D tImageDevSync = kImageRed; 
			if (DevSync > 0) {
				tImageDevSync = kImageGreen;
			}
			GUILayout.Label(tImageDevSync, tStyleCenter,GUILayout.Width (kDevSyncWidth),GUILayout.Height (kRowHeightImage));
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
			if (NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Contains (ClassType ())) {
				tDisableProd = true;
			}
			if (AccountDependent () == true) {
				tDisableProd = true;
			}

			GUILayout.Label(tImagePreprodSync,tStyleCenter,GUILayout.Width (kPreprodSyncWidth),GUILayout.Height (kRowHeightImage));
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
			GUILayout.Label(tImageProdSync,tStyleCenter,GUILayout.Width (kProdSyncWidth),GUILayout.Height (kRowHeightImage));
			// Draw State
			GUILayout.Label (sStateInfos,tStyleCenter, GUILayout.Width (kActiveWidth));
			// Draw Reference
			GUILayout.Label (tStringReference, tStyleRight, GUILayout.Width(kReferenceWidth));
			// Draw prefab preview
			Texture2D tTexture2D = AssetPreview.GetAssetPreview (tObject);
			if (tTexture2D != null) {
				EditorGUI.DrawPreviewTexture (new Rect (tRectPreview.x, tRectPreview.y-3, kPrefabWidth, kPrefabWidth), tTexture2D);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public Rect DrawRowInEditor (Vector2 sMouseClickPosition, EditorWindow sEditorWindow, bool sSelectAndClick)
		{
			float tWidthUsed = sEditorWindow.position.width+20; //4096.0f
//			float tWidthUsed =4096.0f;
			// start line
			GUILayout.Space(kRowHeightSpace);
			GUILayout.BeginHorizontal (GUILayout.Height (kRowHeight));
			GUILayout.Label ("", GUILayout.Width(kOriginWidth));
			Rect tRect = GUILayoutUtility.GetLastRect ();
			// determine rect to select and draw
			Rect rRect = new Rect (tRect.x, tRect.y-5, tWidthUsed, kRowHeight+5);
			Rect rRectColored = new Rect (tRect.x-5, tRect.y-5, tWidthUsed+1024, kRowHeight+5);
			// determine rect to analyze
			Rect rRectAnalyze = new Rect (tRect.x-10, tRect.y-5, tWidthUsed, kRowHeight+10);
			// check click in rect
			if (rRectAnalyze.Contains (sMouseClickPosition)) 
			{
                // verif if object is in error
                ErrorCheck();
				//NWDDataManager.SharedInstance().UpdateQueueExecute(); // update execute in another place!? in NWDDataManager.SharedInstance() destroyed?
				GUI.FocusControl (null);
				SetObjectInEdition (this);
				if (sSelectAndClick==true) {
					if (XX == 0 && TestIntegrity ()) {
						int tIndex = ObjectsByReferenceList.IndexOf (Reference);
						ObjectsInEditorTableSelectionList [tIndex] = !ObjectsInEditorTableSelectionList [tIndex];
						Event.current.Use ();
					}
				}
				sEditorWindow.Focus ();
			}
			// check if object is in edition and draw color rect overlay
			if (IsObjectInEdition (this) == true)
			{
				EditorGUI.DrawRect (rRectColored, kRowColorSelected);
			}
			// draw informations
			RowInformation (rRect);
			// draw line to delimit the rect
			tRect = new Rect (tRect.x-kRowOutMarge, tRect.y+kRowHeight, tWidthUsed+1024, kRowLineStroke);
			EditorGUI.DrawRect (tRect, kRowColorLine);
			// finish line
			GUILayout.EndHorizontal ();
			return rRect;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================