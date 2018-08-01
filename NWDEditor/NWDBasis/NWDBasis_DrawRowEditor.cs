//=====================================================================================================================
//
// ideMobi copyright 2018 
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
using SQLite4Unity3d;
//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
		public static void DrawHeaderInEditor ()
		{
			GUILayout.BeginHorizontal (GUILayout.Height (NWDConstants.kHeaderHeight));
            GUILayout.Label (" ", EditorStyles.boldLabel, GUILayout.Width(NWDConstants.kOriginWidth));
			Rect tRect = GUILayoutUtility.GetLastRect ();
            Rect tRectArea = new Rect (tRect.x-NWDConstants.kHeaderHeight, tRect.y-NWDConstants.kHeaderHeightSpace/2.0f, 4096.0f, NWDConstants.kHeaderHeight+NWDConstants.kHeaderHeightSpace);
            EditorGUI.DrawRect (tRectArea, NWDConstants.kHeaderColorBackground);

			GUIStyle tStyleLeft = new GUIStyle (EditorStyles.boldLabel);
			tStyleLeft.alignment = TextAnchor.MiddleLeft;

			GUIStyle tStyleCenter = new GUIStyle (EditorStyles.boldLabel);
			tStyleCenter.alignment = TextAnchor.MiddleCenter;

			GUIStyle tStyleRight = new GUIStyle (EditorStyles.boldLabel);
			tStyleRight.alignment = TextAnchor.MiddleRight;

            GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_SELECT, tStyleLeft, GUILayout.Width(NWDConstants.kSelectWidth));
            GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_ID, tStyleLeft, GUILayout.Width(NWDConstants.kIDWidth));
            GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_PREFAB, tStyleLeft, GUILayout.Width(NWDConstants.kPrefabWidth));
            GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_DESCRIPTION, tStyleLeft, GUILayout.MinWidth(NWDConstants.kDescriptionMinWidth));

            GUILayout.Label(NWDConstants.K_APP_TABLE_HEADER_DISK, tStyleCenter, GUILayout.Width(NWDConstants.kDiskWidth));

            GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_SYNCHRO, tStyleCenter, GUILayout.Width(NWDConstants.kSyncWidth));
            GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_DEVSYNCHRO, tStyleCenter, GUILayout.Width(NWDConstants.kDevSyncWidth));
            GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_PREPRODSYNCHRO, tStyleCenter, GUILayout.Width(NWDConstants.kPreprodSyncWidth));
            GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_PRODSYNCHRO,tStyleCenter, GUILayout.Width(NWDConstants.kProdSyncWidth));

            GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_STATUT, tStyleCenter, GUILayout.Width(NWDConstants.kActiveWidth));
            GUILayout.Label (NWDConstants.K_APP_TABLE_HEADER_REFERENCE +" ", tStyleRight, GUILayout.Width(NWDConstants.kReferenceWidth));
            Rect tRectLine = new Rect (tRect.x-NWDConstants.kHeaderHeight, tRect.y+NWDConstants.kHeaderHeight+NWDConstants.kHeaderLineStroke, 4096.0f, NWDConstants.kHeaderLineStroke);
            EditorGUI.DrawRect (tRectLine, NWDConstants.kHeaderColorLine);
			GUILayout.EndHorizontal ();
        }
		//-------------------------------------------------------------------------------------------------------------
  //      public float RowInformation (Rect sRect)
		//{
  //          float rReturn = NWDConstants.kRowHeight; 
  //          // Draw internal informations
  //          GUIStyle tStyleLeft = new GUIStyle(EditorStyles.label);
  //          tStyleLeft.alignment = TextAnchor.MiddleLeft;
  //          GUIStyle tStyleCenter = new GUIStyle(EditorStyles.label);
  //          tStyleCenter.alignment = TextAnchor.MiddleCenter;
  //          GUIStyle tStyleRight = new GUIStyle(EditorStyles.label);
  //          tStyleRight.alignment = TextAnchor.MiddleRight;
  //          GUIStyle tStyleForInfos = new GUIStyle(EditorStyles.label);
  //          tStyleForInfos.richText = true;

  //          // check error in data 
  //          string tIsInError = "";
  //          //IsInErrorCheck();
  //          if (InError==true)
  //          {
  //              tIsInError = NWDConstants.K_WARNING;
  //          }


  //          string tString = "<size=13><color=red>"+tIsInError+"</color><b>" + InternalKey + "</b></size>     <i>(" + InternalDescription + ")</i> ";
  //          // to check the versioning active this line
  //          //tString+= "minversion = '" + MinVersion.ToString()+"'";

		//	tString = tString.Replace ("()", "");
		//	string tStringReference = "<" + Reference + ">";
		//	// prepare prefab 
		//	GameObject tObject = null;
		//	if (Preview != null && Preview != "") {
		//		tObject = AssetDatabase.LoadAssetAtPath (Preview, typeof(GameObject)) as GameObject;
		//	}
		//	if (mGameObjectEditor == null) {
		//		mGameObjectEditor = Editor.CreateEditor (tObject);
		//	}
		//	// prepare State infos
		//	string sStateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_OK;
		//	//draw informations
		//	int tIndex = ObjectsByReferenceList.IndexOf (Reference);
		//	// test Integrity, trash, etc. to draw the good color of row
		//	// and draw the toogle to select this row

  //          rReturn = tStyleForInfos.CalcHeight(new GUIContent(tString), NWDConstants.kDescriptionMinWidth);

  //          if (rReturn < NWDConstants.kRowHeight)
  //          {
  //              rReturn = NWDConstants.kRowHeight;
  //          }


  //          // TOO LONG TOO LAG ?
  //          //Texture2D tTextureOfClass = TextureOfClass();
  //          //if (tTextureOfClass != null)
  //          //{
  //          //    //GUILayout.Label(tTextureOfClass, tStyleCenter, GUILayout.Width(kPreprodSyncWidth), GUILayout.Height(kRowHeightImage));
  //          //    //GUILayout.DrawTexture(new Rect(tX + tWidth / 2.0F - 16, tY, 32, 32), tTextureOfClass);
  //          //}

  //          // Test the web service version
  //          WebserviceVersionCheckMe();

  //          // prepare text
  //          Rect rRectColored = new Rect(sRect.x - 5, sRect.y, sRect.width + 1024, rReturn);

		//	if (TestIntegrity () == false) {
  //              EditorGUI.DrawRect (rRectColored, NWDConstants.kRowColorError);
		//		ObjectsInEditorTableSelectionList [tIndex] = false;
  //              GUILayout.Label ("!!!", GUILayout.Width(NWDConstants.kSelectWidth));
		//		sStateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_ERROR;
		//		sStateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_INTEGRITY_ERROR;
		//		tString = "<color=#a52a2aff>" +tString +"</color>";
		//	} else if (XX > 0) {
  //              EditorGUI.DrawRect (rRectColored, NWDConstants.kRowColorTrash);
		//		ObjectsInEditorTableSelectionList [tIndex] = false;
  //              GUILayout.Label ("   ", GUILayout.Width(NWDConstants.kSelectWidth));
		//		sStateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_TRASH;
		//		tString = "<color=#444444ff>" +tString +"</color>";
		//	} else {
		//		if (AC == false) {
  //                  EditorGUI.DrawRect (rRectColored, NWDConstants.kRowColorDisactive);
		//			sStateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_DISACTIVE;
		//			tString = "<color=#555555ff>" +tString +"</color>";
		//		}
  //              ObjectsInEditorTableSelectionList [tIndex] = EditorGUILayout.ToggleLeft ("", ObjectsInEditorTableSelectionList [tIndex], GUILayout.Width(NWDConstants.kSelectWidth));
		//	}

  //          GUILayout.Label (ID.ToString (), GUILayout.Width(NWDConstants.kIDWidth));
  //          GUILayout.Label (" ", GUILayout.Width(NWDConstants.kPrefabWidth));
		//	Rect tRectPreview = GUILayoutUtility.GetLastRect ();
  //          GUILayout.Label (tString, tStyleForInfos, GUILayout.MinWidth(NWDConstants.kDescriptionMinWidth));
		//	tStyleLeft.alignment = TextAnchor.MiddleRight;
		//	// Draw Sync State
  //          Texture2D tImageSync = NWDConstants.kImageRed; 
		//	if (DS > 0) {
  //              tImageSync = NWDConstants.kImageGreen; 
		//	}
  //          GUILayout.Label(tImageSync,tStyleCenter,GUILayout.Width (NWDConstants.kSyncWidth),GUILayout.Height (NWDConstants.kRowHeightImage));
		//	// Draw Dev Sync State
  //          Texture2D tImageDevSync = NWDConstants.kImageRed; 
		//	if (DevSync > 0) {
  //              tImageDevSync = NWDConstants.kImageGreen;
		//	}
  //          GUILayout.Label(tImageDevSync, tStyleCenter,GUILayout.Width (NWDConstants.kDevSyncWidth),GUILayout.Height (NWDConstants.kRowHeightImage));
		//	// Draw Preprod Sync State
  //          Texture2D tImagePreprodSync = NWDConstants.kImageRed; 
		//	if (PreprodSync > 0) {
		//		if (PreprodSync > DevSync) {
  //                  tImagePreprodSync = NWDConstants.kImageGreen;
		//		} else {
  //                  tImagePreprodSync = NWDConstants.kImageOrange;
		//		}
		//	}

		//	bool tDisableProd = false;
		//	if (NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Contains (ClassType ())) {
		//		tDisableProd = true;
		//	}
		//	if (AccountDependent () == true) {
		//		tDisableProd = true;
		//	}

  //          GUILayout.Label(tImagePreprodSync,tStyleCenter,GUILayout.Width (NWDConstants.kPreprodSyncWidth),GUILayout.Height (NWDConstants.kRowHeightImage));
		//	// Draw Prod Sync State
  //          Texture2D tImageProdSync = NWDConstants.kImageRed; 
		//	if (tDisableProd == true) {
  //              tImageProdSync = NWDConstants.kImageForbidden;
		//	}
		//	else
		//	{
		//		if (ProdSync > 0) {
		//			if (ProdSync > DevSync && ProdSync > PreprodSync) {
  //                      tImageProdSync = NWDConstants.kImageGreen;
		//			} else {
  //                      tImageProdSync = NWDConstants.kImageOrange;
		//			}
		//		}
		//	}
  //          GUILayout.Label(tImageProdSync,tStyleCenter,GUILayout.Width (NWDConstants.kProdSyncWidth),GUILayout.Height (NWDConstants.kRowHeightImage));
		//	// Draw State
  //          GUILayout.Label (sStateInfos,tStyleCenter, GUILayout.Width (NWDConstants.kActiveWidth));
		//	// Draw Reference
  //          GUILayout.Label (tStringReference, tStyleRight, GUILayout.Width(NWDConstants.kReferenceWidth));
		//	// Draw prefab preview
		//	Texture2D tTexture2D = AssetPreview.GetAssetPreview (tObject);
		//	if (tTexture2D != null) {
  //              EditorGUI.DrawPreviewTexture (new Rect (tRectPreview.x, tRectPreview.y-3, NWDConstants.kPrefabWidth, NWDConstants.kPrefabWidth), tTexture2D);
		//	}

  //          return rReturn;
		//}
		//-------------------------------------------------------------------------------------------------------------
		public Rect DrawRowInEditor (Vector2 sMouseClickPosition, EditorWindow sEditorWindow, bool sSelectAndClick)
		{
			float tWidthUsed = sEditorWindow.position.width+20; //4096.0f



            float tRowHeight = NWDConstants.kRowHeight;
            // Draw internal informations
            GUIStyle tStyleLeft = new GUIStyle(EditorStyles.label);
            tStyleLeft.alignment = TextAnchor.MiddleLeft;
            GUIStyle tStyleCenter = new GUIStyle(EditorStyles.label);
            tStyleCenter.alignment = TextAnchor.MiddleCenter;
            GUIStyle tStyleRight = new GUIStyle(EditorStyles.label);
            tStyleRight.alignment = TextAnchor.MiddleRight;
            GUIStyle tStyleForInfos = new GUIStyle(EditorStyles.label);
            tStyleForInfos.richText = true;


            // verif if object is in error
            ErrorCheck();
            int tIndex = Datas().ObjectsByReferenceList.IndexOf(Reference);

            // check error in data 
            string tIsInError = "";
            //IsInErrorCheck();
            if (InError == true)
            {
                tIsInError = NWDConstants.K_WARNING;
            }


            string tString = "<size=13><color=red>" + tIsInError + "</color><b>" + InternalKey + "</b></size>     <i>(" + InternalDescription + ")</i> ";
            // to check the versioning active this line
            //tString+= "minversion = '" + MinVersion.ToString()+"'";

            tString = tString.Replace("()", "");
            tRowHeight = tStyleForInfos.CalcHeight(new GUIContent(tString), NWDConstants.kDescriptionMinWidth);

            if (tRowHeight < NWDConstants.kRowHeight)
            {
                tRowHeight = NWDConstants.kRowHeight;
            }

//			float tWidthUsed =4096.0f;
			// start line
            GUILayout.Space(NWDConstants.kRowHeightSpace);
            GUILayout.BeginHorizontal (GUILayout.Height (tRowHeight));
            GUILayout.Label ("", GUILayout.Width(NWDConstants.kOriginWidth));
			Rect tRect = GUILayoutUtility.GetLastRect ();
			// determine rect to select and draw
            Rect rRect = new Rect (tRect.x, tRect.y-5, tWidthUsed, tRowHeight);
            Rect rRectColored = new Rect (tRect.x-5, tRect.y-5, tWidthUsed+1024, tRowHeight+5);
			// determine rect to analyze
            Rect rRectAnalyze = new Rect (tRect.x-10, tRect.y-5, tWidthUsed, tRowHeight+10);
            // check click in rect


			if (rRectAnalyze.Contains (sMouseClickPosition)) 
			{
				//NWDDataManager.SharedInstance().UpdateQueueExecute(); // update execute in another place!? in NWDDataManager.SharedInstance() destroyed?
				GUI.FocusControl (null);
				SetObjectInEdition (this);
				if (sSelectAndClick==true) {
					if (XX == 0 && TestIntegrity ()) {
                        Datas().ObjectsInEditorTableSelectionList[tIndex] = !Datas().ObjectsInEditorTableSelectionList [tIndex];
						Event.current.Use ();
					}
				}
				sEditorWindow.Focus ();
			}
			// check if object is in edition and draw color rect overlay
			if (IsObjectInEdition (this) == true)
			{
                EditorGUI.DrawRect (rRectColored, NWDConstants.kRowColorSelected);
			}
			// draw informations
            //float tHeightInfo = RowInformation (rRect);

            string tStringReference = "<" + Reference + ">";
            // prepare prefab 
            GameObject tObject = null;
            if (Preview != null && Preview != "")
            {
                tObject = AssetDatabase.LoadAssetAtPath(Preview, typeof(GameObject)) as GameObject;
            }
            if (mGameObjectEditor == null)
            {
                mGameObjectEditor = Editor.CreateEditor(tObject);
            }
            // prepare State infos
            string sStateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_OK;
            //draw informations
            //int tIndex = ObjectsByReferenceList.IndexOf(Reference);
            // test Integrity, trash, etc. to draw the good color of row
            // and draw the toogle to select this row




            // TOO LONG TOO LAG ?
            //Texture2D tTextureOfClass = TextureOfClass();
            //if (tTextureOfClass != null)
            //{
            //    //GUILayout.Label(tTextureOfClass, tStyleCenter, GUILayout.Width(kPreprodSyncWidth), GUILayout.Height(kRowHeightImage));
            //    //GUILayout.DrawTexture(new Rect(tX + tWidth / 2.0F - 16, tY, 32, 32), tTextureOfClass);
            //}

            // Test the web service version
            // WebserviceVersionCheckMe();
            if (WebserviceVersionIsValid())
            {
                if (TestIntegrity() == false)
                {
                    EditorGUI.DrawRect(rRectColored, NWDConstants.kRowColorError);
                    Datas().ObjectsInEditorTableSelectionList[tIndex] = false;
                    GUILayout.Label("!!!", GUILayout.Width(NWDConstants.kSelectWidth));
                    //sStateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_ERROR;
                    sStateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_INTEGRITY_ERROR;
                    tString = "<color=#a52a2aff>" + tString + "</color>";
                }
                else if (XX > 0)
                {
                    EditorGUI.DrawRect(rRectColored, NWDConstants.kRowColorTrash);
                    Datas().ObjectsInEditorTableSelectionList[tIndex] = false;
                    GUILayout.Label("   ", GUILayout.Width(NWDConstants.kSelectWidth));
                    sStateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_TRASH;
                    tString = "<color=#444444ff>" + tString + "</color>";
                }
                else
                {
                    if (AC == false)
                    {
                        EditorGUI.DrawRect(rRectColored, NWDConstants.kRowColorDisactive);
                        sStateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_DISACTIVE;
                        tString = "<color=#555555ff>" + tString + "</color>";
                    }
                    Datas().ObjectsInEditorTableSelectionList[tIndex] = EditorGUILayout.ToggleLeft("", Datas().ObjectsInEditorTableSelectionList[tIndex], GUILayout.Width(NWDConstants.kSelectWidth));
                }
            }
            else
            {
                EditorGUI.DrawRect(rRectColored, NWDConstants.kRowColorWarning);
                Datas().ObjectsInEditorTableSelectionList[tIndex] = false;
                GUILayout.Label("!~!", GUILayout.Width(NWDConstants.kSelectWidth));
                sStateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_WEBSERVICE_ERROR;
                tString = "<color=#cc6600ff>" + tString + "</color>";
            }
            GUILayout.Label(ID.ToString(), GUILayout.Width(NWDConstants.kIDWidth));
            GUILayout.Label(" ", GUILayout.Width(NWDConstants.kPrefabWidth));
            Rect tRectPreview = GUILayoutUtility.GetLastRect();
            GUILayout.Label(tString, tStyleForInfos, GUILayout.MinWidth(NWDConstants.kDescriptionMinWidth));
            tStyleLeft.alignment = TextAnchor.MiddleRight;


            // Draw Disk State

            Texture2D tImageDisk = NWDConstants.kImageDiskUnknow;
            if (FromDatabase==true)
            {
                tImageDisk = NWDConstants.kImageDiskDatabase;
            }
            switch (WritingPending)
            {
                case NWDWritingPending.Unknow :
                    tImageDisk = NWDConstants.kImageDiskUnknow;
                    break;
                case NWDWritingPending.UpdateInMemory:
                    tImageDisk = NWDConstants.kImageDiskUpdate;
                    break;
                case NWDWritingPending.InsertInMemory:
                    tImageDisk = NWDConstants.kImageDiskInsert;
                    break;
                case NWDWritingPending.DeleteInMemory:
                    tImageDisk = NWDConstants.kImageDiskDelete;
                    break;
                case NWDWritingPending.InDatabase:
                    tImageDisk = NWDConstants.kImageDiskDatabase;
                    break;
            }

            GUILayout.Label(tImageDisk, tStyleCenter, GUILayout.Width(NWDConstants.kSyncWidth), GUILayout.Height(NWDConstants.kRowHeightImage));

            bool tDisableProd = false;
            if (NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Contains(ClassType()))
            {
                tDisableProd = true;
            }
            if (AccountDependent() == true)
            {
                tDisableProd = true;
            }

            // Draw Sync State
            Texture2D tImageSync = NWDConstants.kImageSyncGeneralWaiting;
            if (DevSync < 0 && PreprodSync < 0 && (ProdSync < 0 || tDisableProd == true))
            {
                tImageSync = NWDConstants.kImageSyncGeneralForbidden;
            }
            else
            {
                if (DS > 0)
                {
                    if (DevSync > 1 && PreprodSync < 1 && ProdSync < 1 && DS == DevSync)
                    {
                        tImageSync = NWDConstants.kImageSyncGeneralSuccessed;
                    }
                    else if (DevSync > 1 && PreprodSync > 1 && ProdSync < 1 && (DS == DevSync || DS==PreprodSync))
                    {
                        tImageSync = NWDConstants.kImageSyncGeneralSuccessed;
                    }
                    else if (DS < DM)
                    {
                        tImageSync = NWDConstants.kImageSyncGeneralForward;
                    }
                    else
                    {
                        tImageSync = NWDConstants.kImageSyncGeneralWaiting;
                    }
                }
            }
            GUILayout.Label(tImageSync, tStyleCenter, GUILayout.Width(NWDConstants.kSyncWidth), GUILayout.Height(NWDConstants.kRowHeightImage));
            // Draw Dev Sync State
            Texture2D tImageDevSync = NWDConstants.kImageSyncRequired;
            if (DevSync == 0)
            {
                tImageDevSync = NWDConstants.kImageSyncRequired;
            }
            else if (DevSync == 1)
            {
                tImageDevSync = NWDConstants.kImageSyncWaiting;
            }
            else if (DevSync > 1)
            {
                tImageDevSync = NWDConstants.kImageSyncSuccessed;
            }
            else if (DevSync == -1)
            {
                tImageDevSync = NWDConstants.kImageSyncForbidden;
            }
            else if (DevSync < -1)
            {
                tImageDevSync = NWDConstants.kImageSyncDanger;
            }
            GUILayout.Label(tImageDevSync, tStyleCenter, GUILayout.Width(NWDConstants.kDevSyncWidth), GUILayout.Height(NWDConstants.kRowHeightImage));
            // Draw Preprod Sync State
            Texture2D tImagePreprodSync = NWDConstants.kImageSyncRequired;

            if (PreprodSync == 0)
            {
                tImagePreprodSync = NWDConstants.kImageSyncRequired;
            }
            else if (PreprodSync == 1)
            {
                tImagePreprodSync = NWDConstants.kImageSyncWaiting;
            }
            else if (PreprodSync > 1)
            {
                if (PreprodSync > DevSync)
                {
                    tImagePreprodSync = NWDConstants.kImageSyncSuccessed;
                }
                else
                {
                    tImagePreprodSync = NWDConstants.kImageSyncForward;
                }
            }
            else if (PreprodSync == -1)
            {
                tImagePreprodSync = NWDConstants.kImageSyncForbidden;
            }
            else if (PreprodSync < -1)
            {
                tImagePreprodSync = NWDConstants.kImageSyncDanger;
            }


            GUILayout.Label(tImagePreprodSync, tStyleCenter, GUILayout.Width(NWDConstants.kPreprodSyncWidth), GUILayout.Height(NWDConstants.kRowHeightImage));
            // Draw Prod Sync State
            Texture2D tImageProdSync = NWDConstants.kImageSyncRequired;
            if (tDisableProd == true)
            {
                tImageProdSync = NWDConstants.kImageSyncForbidden;
            }
            else
            {
                if (ProdSync == 0)
                {
                    tImageProdSync = NWDConstants.kImageSyncRequired;
                }
                else if (ProdSync == 1)
                {
                    tImageProdSync = NWDConstants.kImageSyncWaiting;
                }
                if (ProdSync > 1)
                {
                    if (ProdSync > DevSync && ProdSync > PreprodSync)
                    {
                        tImageProdSync = NWDConstants.kImageSyncSuccessed;
                    }
                    else
                    {
                        tImageProdSync = NWDConstants.kImageSyncForward;
                    }
                }
                else if (ProdSync == -1)
                {
                    tImageProdSync = NWDConstants.kImageSyncForbidden;
                }
                else if (ProdSync < -1)
                {
                    tImageProdSync = NWDConstants.kImageSyncDanger;
                }
            }
            GUILayout.Label(tImageProdSync, tStyleCenter, GUILayout.Width(NWDConstants.kProdSyncWidth), GUILayout.Height(NWDConstants.kRowHeightImage));
            // Draw State
            GUILayout.Label(sStateInfos, tStyleCenter, GUILayout.Width(NWDConstants.kActiveWidth));
            // Draw Reference
            GUILayout.Label(tStringReference, tStyleRight, GUILayout.Width(NWDConstants.kReferenceWidth));
            // Draw prefab preview
            Texture2D tTexture2D = AssetPreview.GetAssetPreview(tObject);
            if (tTexture2D != null)
            {
                EditorGUI.DrawPreviewTexture(new Rect(tRectPreview.x, tRectPreview.y - 3, NWDConstants.kPrefabWidth, NWDConstants.kPrefabWidth), tTexture2D);
            }
			// draw line to delimit the rect
            tRect = new Rect (tRect.x-NWDConstants.kRowOutMarge, tRect.y+tRowHeight+NWDConstants.kRowLineStroke, tWidthUsed+1024, NWDConstants.kRowLineStroke);
            EditorGUI.DrawRect (tRect, NWDConstants.kRowColorLine);
			// finish line
			GUILayout.EndHorizontal ();
			return rRect;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif