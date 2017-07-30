//=====================================================================================================================
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

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
	public class NWDDataInspector : EditorWindow
	{
		//-------------------------------------------------------------------------------------------------------------
		public object mObjectInEdition;
		public List<object> mObjectsList = new List<object> ();
		public int ActualIndex = 0;
		public bool RemoveActualFocus = true;
		//-------------------------------------------------------------------------------------------------------------
//		private Vector2 ScrollPosition = Vector2.zero;
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
			ShareInstance ().Data (sTarget, sResetStack,sFocus);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void Data (object sTarget, bool sResetStack = true, bool sFocus=true)
		{
			if (sResetStack == true) {
				mObjectsList = new List<object> ();
			} else {
				mObjectsList.RemoveRange (ActualIndex + 1, mObjectsList.Count - ActualIndex - 1);
			}
			ActualIndex = mObjectsList.Count;
			mObjectsList.Add (sTarget);
			mObjectInEdition = sTarget;
			Repaint ();
			RemoveActualFocus = sFocus;
			if (sFocus == true) {
				Focus ();
			}
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
			//Debug.Log ("OnGUI");
			titleContent = new GUIContent ();
			titleContent.text = "Inspector";
			titleContent.image = AssetDatabase.LoadAssetAtPath<Texture> (NWDFindPackage.PathOfPackage ("/NWDEditor/NWDNativeImages/NWDIcons_01.png"));

//			ScrollPosition = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height), ScrollPosition, new Rect(0, 0, position.width, position.height*2));
			if (mObjectInEdition == null)
			{
			} 
			else 
			{
				Type tType = mObjectInEdition.GetType ();
				var tMethodInfo = tType.GetMethod ("DrawObjectEditor", BindingFlags.Public | BindingFlags.Instance);
				if (tMethodInfo != null) 
				{
					tMethodInfo.Invoke (mObjectInEdition, new object[]{position, true});
				}
			}
//			GUI.EndScrollView();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif