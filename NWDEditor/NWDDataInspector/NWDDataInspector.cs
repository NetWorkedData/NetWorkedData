﻿//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public class NWDDataInspector : EditorWindow
	{
		//-------------------------------------------------------------------------------------------------------------
		public object mObjectInEdition;
		public List<object> mObjectsList = new List<object> ();
		public int ActualIndex = 0;
		public bool RemoveActualFocus = true;
        //-------------------------------------------------------------------------------------------------------------
        private GUIContent IconAndTitle;
		//-------------------------------------------------------------------------------------------------------------
		static NWDDataInspector kShareInstance;
		//-------------------------------------------------------------------------------------------------------------
		public static NWDDataInspector ShareInstance ()
		{
			if (kShareInstance == null)
			{
				EditorWindow tWindow = EditorWindow.GetWindow (typeof(NWDDataInspector));
				tWindow.Show ();
				kShareInstance = (NWDDataInspector)tWindow;
				kShareInstance.minSize = new Vector2 (300, 500);
				kShareInstance.maxSize = new Vector2 (600, 2048);
			}
			return kShareInstance;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void ActiveInspector ()
		{
			ShareInstance().Show ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void ActiveRepaint()
		{
			if (kShareInstance != null) {
				kShareInstance.Repaint ();
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void InspectNetWorkedDataPreview () {
			ShareInstance ().DataPreview ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public void DataPreview () {
			ActualIndex--;
			if (ActualIndex < 0) {
				ActualIndex = 0;
			}
			object tTarget = mObjectsList[ActualIndex];
			mObjectInEdition = tTarget;
			Repaint ();
			RemoveActualFocus = true;
			Focus();
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void InspectNetWorkedDataNext () {
			ShareInstance ().DataNext ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public void DataNext () {
			ActualIndex++;
			if (ActualIndex >= mObjectsList.Count) {
				ActualIndex = 0;
			}
			object tTarget = mObjectsList[ActualIndex];
			mObjectInEdition = tTarget;
			Repaint ();
			RemoveActualFocus = true;
			Focus();
		}
		//-------------------------------------------------------------------------------------------------------------
		public static bool InspectNetWorkedPreview () {
			return ShareInstance ().Preview ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public bool Preview () {
			return (ActualIndex > 0);
		}
		//-------------------------------------------------------------------------------------------------------------
		public static bool InspectNetWorkedNext () {
			return ShareInstance ().Next ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public bool Next () {
			return (ActualIndex < mObjectsList.Count-1);
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void InspectNetWorkedData (object sTarget, bool sResetStack = true, bool sFocus=true)
		{
            if (ShareInstance().mObjectInEdition != sTarget)
            {
                ShareInstance().Data(sTarget, sResetStack, sFocus);
                if (sTarget != null)
                {
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(sTarget.GetType());
                }
            }
		}
		//-------------------------------------------------------------------------------------------------------------
		public void Data (object sTarget, bool sResetStack = true, bool sFocus=true)
		{
			if (sResetStack == true) {
				mObjectsList = new List<object> ();
			} else {
				if (mObjectsList.Count > ActualIndex) {
					mObjectsList.RemoveRange (ActualIndex + 1, mObjectsList.Count - ActualIndex - 1);
				}
			}
			ActualIndex = mObjectsList.Count;
			mObjectsList.Add (sTarget);
			mObjectInEdition = sTarget;
			Repaint ();
			RemoveActualFocus = sFocus;
			if (sFocus == true) {
				Focus ();
			}
            NWDNodeEditor.ReDraw();
//			GUI.FocusControl (NWDConstants.K_CLASS_FOCUS_ID);
		}
		//-------------------------------------------------------------------------------------------------------------
		public static object ObjectInEdition ()
		{
			return ShareInstance ().mObjectInEdition;
		}
		//-------------------------------------------------------------------------------------------------------------
		// Use this for initialization
		void Start ()
		{
			//Debug.Log ("Start");
		}
		//-------------------------------------------------------------------------------------------------------------
		public void OnEnable ()
		{
			//Debug.Log ("OnEnable");

		}
		//-------------------------------------------------------------------------------------------------------------
//		public void Update ()
//		{
//			Debug.Log ("Update");
		//		}
		//-------------------------------------------------------------------------------------------------------------
		void OnDestroy() {
			
		}
		//-------------------------------------------------------------------------------------------------------------
		public void OnGUI ()
		{
			if (RemoveActualFocus == true) {
				GUI.FocusControl (null);
				RemoveActualFocus = false;
			}
            if (IconAndTitle == null)
            {
                IconAndTitle = new GUIContent();
                IconAndTitle.text = NWDConstants.K_APP_SYNC_INSPECTOR_TITLE;
                if (IconAndTitle.image == null)
                {
                    string[] sGUIDs = AssetDatabase.FindAssets("NWDDataInspector t:texture");
                    foreach (string tGUID in sGUIDs)
                    {
                        //Debug.Log("TextureOfClass GUID " + tGUID);
                        string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                        string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                        //Debug.Log("tPathFilename = " + tPathFilename);
                        if (tPathFilename.Equals("NWDDataInspector"))
                        {
                            //Debug.Log("TextureOfClass " + tPath);
                            IconAndTitle.image = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                        }
                    }
                }
                titleContent = IconAndTitle;
            }
//			ScrollPosition = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height), ScrollPosition, new Rect(0, 0, position.width, position.height*2));
			if (mObjectInEdition == null)
			{
			} 
			else 
			{
				Type tType = mObjectInEdition.GetType ();
                // TODO : Change to remove invoke!
                MethodInfo tMethodInfo = NWDAliasMethod.GetMethodPublicInstance(tType, NWDConstants.M_DrawObjectEditor);
                //var tMethodInfo = tType.GetMethod ("DrawObjectEditor", BindingFlags.Public | BindingFlags.Instance);
				if (tMethodInfo != null) 
				{
					tMethodInfo.Invoke (mObjectInEdition, new object[]{position, true});
				}
			}
//			GUI.EndScrollView();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif