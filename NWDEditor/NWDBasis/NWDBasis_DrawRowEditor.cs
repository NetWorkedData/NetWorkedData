//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
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
        Texture2D tImageDisk = NWDConstants.kImageDiskUnknow;
        Texture2D tImageSync = NWDConstants.kImageSyncGeneralWaiting;
        Texture2D tImageDevSync = NWDConstants.kImageSyncRequired;
        Texture2D tImagePreprodSync = NWDConstants.kImageSyncRequired;
        Texture2D tImageProdSync = NWDConstants.kImageSyncRequired;
        bool TestIntegrityResult = true;
        bool TestWebserviceVersionIsValid = true;
        string tStringRow = string.Empty;
        //-------------------------------------------------------------------------------------------------------------
        public void RowAnalyze()
        {
            //Debug.Log("RowAnalyze");
            TestIntegrityResult = TestIntegrity();
            TestWebserviceVersionIsValid = WebserviceVersionIsValid();

            string tIsInError = string.Empty;
            //IsInErrorCheck();
            if (InError == true)
            {
                tIsInError = NWDConstants.K_WARNING;
            }


            tStringRow = "<size=13><color=red>" + tIsInError + "</color><b>" + InternalKey + "</b></size>     <i>(" + InternalDescription + ")</i> " +
            "[" + WebModel.ToString() + "/" + WebModelToUse() + "/" + NWDAppConfiguration.SharedInstance().WebBuild + "]";


            // verif if object is in error
            if (FromDatabase == true)
            {
                tImageDisk = NWDConstants.kImageDiskDatabase;
            }
            switch (WritingPending)
            {
                case NWDWritingPending.Unknow:
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
            // Draw Sync State
            bool tDisableProd = false;
            if (NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Contains(ClassType()))
            {
                tDisableProd = true;
            }
            if (AccountDependent() == true)
            {
                tDisableProd = true;
            }

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
                    else if (DevSync > 1 && PreprodSync > 1 && ProdSync < 1 && (DS == DevSync || DS == PreprodSync))
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
        }
        //-------------------------------------------------------------------------------------------------------------
        public Rect DrawRowInEditor (Vector2 sMouseClickPosition, EditorWindow sEditorWindow, bool sSelectAndClick)
		{
			float tWidthUsed = sEditorWindow.position.width+20; //4096.0f

            float tRowHeight = NWDConstants.kRowHeight;
            //// Draw internal informations
            //GUIStyle tStyleLeft = new GUIStyle(EditorStyles.label);
            //tStyleLeft.alignment = TextAnchor.MiddleLeft;
            //GUIStyle tStyleCenter = new GUIStyle(EditorStyles.label);
            //tStyleCenter.alignment = TextAnchor.MiddleCenter;
            //GUIStyle tStyleRight = new GUIStyle(EditorStyles.label);
            //tStyleRight.alignment = TextAnchor.MiddleRight;
            //GUIStyle tStyleForInfos = new GUIStyle(EditorStyles.label);
            //tStyleForInfos.richText = true;

            //int tIndex = Datas().ObjectsByReferenceList.IndexOf(Reference);

            // check error in data 
            string tString = tStringRow;
            // to check the versioning active this line
            //tString+= "minversion = '" + MinVersion.ToString()+"'";

            tString = tString.Replace("()", string.Empty);
            tRowHeight = NWDConstants.kRowStyleForInfos.CalcHeight(new GUIContent(tString), NWDConstants.kDescriptionMinWidth);

            if (tRowHeight < NWDConstants.kRowHeight)
            {
                tRowHeight = NWDConstants.kRowHeight;
            }

//			float tWidthUsed =4096.0f;
			// start line
            GUILayout.Space(NWDConstants.kRowHeightSpace);
            GUILayout.BeginHorizontal (GUILayout.Height (tRowHeight));
            GUILayout.Label (string.Empty, GUILayout.Width(NWDConstants.kOriginWidth));
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
                        //Datas().DatasInEditorSelectionList[tIndex] = !Datas().DatasInEditorSelectionList [tIndex];
                        BasisHelper().EditorTableDatasSelected[this] = !BasisHelper().EditorTableDatasSelected[this];
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

            string tStringReference = "[" + Reference + "]";
            // prepare prefab 
            //GameObject tObject = null;
            //if (Preview != null && Preview != string.Empty)
            //{
            //    tObject = AssetDatabase.LoadAssetAtPath(Preview, typeof(GameObject)) as GameObject;
            //}
            //if (NWDBasisEditor.mGameObjectEditor == null)
            //{
            //    NWDBasisEditor.mGameObjectEditor = Editor.CreateEditor(tObject);
            //}
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
            if (TestWebserviceVersionIsValid)
            {
                if (TestIntegrityResult == false)
                {
                    EditorGUI.DrawRect(rRectColored, NWDConstants.kRowColorError);
                    BasisHelper().EditorTableDatasSelected[this] = false;
                    GUILayout.Label("!!!", GUILayout.Width(NWDConstants.kSelectWidth));
                    //sStateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_ERROR;
                    sStateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_INTEGRITY_ERROR;
                    tString = "<color=#a52a2aff>" + tString + "</color>";
                }
                else if (XX > 0)
                {
                    EditorGUI.DrawRect(rRectColored, NWDConstants.kRowColorTrash);
                    BasisHelper().EditorTableDatasSelected[this] = false;
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
                    BasisHelper().EditorTableDatasSelected[this] = EditorGUILayout.ToggleLeft(string.Empty, BasisHelper().EditorTableDatasSelected[this], GUILayout.Width(NWDConstants.kSelectWidth));
                }
            }
            else
            {
                EditorGUI.DrawRect(rRectColored, NWDConstants.kRowColorWarning);
                BasisHelper().EditorTableDatasSelected[this] = false;
                GUILayout.Label("!~!", GUILayout.Width(NWDConstants.kSelectWidth));
                sStateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_WEBSERVICE_ERROR;
                tString = "<color=#cc6600ff>" + tString + "</color>";
            }
            GUILayout.Label(ID.ToString(), GUILayout.Width(NWDConstants.kIDWidth));
            GUILayout.Label(" ", GUILayout.Width(NWDConstants.kPrefabWidth));
            Rect tRectPreview = GUILayoutUtility.GetLastRect();
            GUILayout.Label(tString, NWDConstants.kRowStyleForInfos, GUILayout.MinWidth(NWDConstants.kDescriptionMinWidth));
            NWDConstants.kRowStyleLeft.alignment = TextAnchor.MiddleRight;


            // Draw Disk State

            GUILayout.Label(tImageDisk, NWDConstants.kRowStyleCenter, GUILayout.Width(NWDConstants.kSyncWidth), GUILayout.Height(NWDConstants.kRowHeightImage));

            GUILayout.Label(tImageSync, NWDConstants.kRowStyleCenter, GUILayout.Width(NWDConstants.kSyncWidth), GUILayout.Height(NWDConstants.kRowHeightImage));
            // Draw Dev Sync State
            GUILayout.Label(tImageDevSync, NWDConstants.kRowStyleCenter, GUILayout.Width(NWDConstants.kDevSyncWidth), GUILayout.Height(NWDConstants.kRowHeightImage));
            // Draw Preprod Sync State


            GUILayout.Label(tImagePreprodSync, NWDConstants.kRowStyleCenter, GUILayout.Width(NWDConstants.kPreprodSyncWidth), GUILayout.Height(NWDConstants.kRowHeightImage));
            // Draw Prod Sync State
            GUILayout.Label(tImageProdSync, NWDConstants.kRowStyleCenter, GUILayout.Width(NWDConstants.kProdSyncWidth), GUILayout.Height(NWDConstants.kRowHeightImage));
            // Draw State
            GUILayout.Label(sStateInfos, NWDConstants.kRowStyleCenter, GUILayout.Width(NWDConstants.kActiveWidth));
            // Draw Reference
            GUILayout.Label(tStringReference, NWDConstants.kRowStyleRight, GUILayout.Width(NWDConstants.kReferenceWidth));
            // Draw prefab preview
            //Texture2D tTexture2D = AssetPreview.GetAssetPreview(tObject);
            //Texture2D tTexture2D = PreviewTexture2D();
            //if (tTexture2D != null)
            //{
            //    EditorGUI.DrawPreviewTexture(new Rect(tRectPreview.x, tRectPreview.y - 3, NWDConstants.kPrefabWidth, NWDConstants.kPrefabWidth), tTexture2D);
            //}
            DrawPreviewTexture2D(new Rect(tRectPreview.x, tRectPreview.y - 3, NWDConstants.kPrefabWidth, NWDConstants.kPrefabWidth));
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