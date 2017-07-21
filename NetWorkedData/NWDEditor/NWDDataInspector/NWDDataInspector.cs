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
		public object mObjectInEdition;
		public List<object> mObjectsList = new List<object> ();
		public int ActualIndex = 0;

		static NWDDataInspector kShareInstance;

		public bool RemoveActualFocus = true;
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
		public static void ActiveInspector ()
		{
			ShareInstance().Show ();
		}
		public static void ActiveRepaint()
		{
			if (kShareInstance != null) {
				kShareInstance.Repaint ();
			}
		}
		// TODO: sortir en method d'instance et remmettre les raccpurcis en static
		public static void InspectNetWorkedDataPreview () {
			NWDDataInspector tShareInstance = ShareInstance ();
			tShareInstance.ActualIndex--;
			if (tShareInstance.ActualIndex < 0) {
				tShareInstance.ActualIndex = 0;
			}
			object tTarget = tShareInstance.mObjectsList[tShareInstance.ActualIndex];
			tShareInstance.mObjectInEdition = tTarget;
			tShareInstance.Repaint ();
			tShareInstance.RemoveActualFocus = true;
			tShareInstance.Focus();
		}
		public static void InspectNetWorkedDataNext () {
			NWDDataInspector tShareInstance = ShareInstance ();
			tShareInstance.ActualIndex++;
			if (tShareInstance.ActualIndex >= tShareInstance.mObjectsList.Count) {
				tShareInstance.ActualIndex = 0;
			}
			object tTarget = tShareInstance.mObjectsList[tShareInstance.ActualIndex];
			ShareInstance ().mObjectInEdition = tTarget;
			ShareInstance ().Repaint ();
			ShareInstance ().RemoveActualFocus = true;
			ShareInstance ().Focus();
		}
		public static bool InspectNetWorkedPreview () {
			NWDDataInspector tShareInstance = ShareInstance ();
			return (tShareInstance.ActualIndex > 0);
		}
		public static bool InspectNetWorkedNext () {
			NWDDataInspector tShareInstance = ShareInstance ();
			return (tShareInstance.ActualIndex < tShareInstance.mObjectsList.Count-1);
		}

		public static void InspectNetWorkedData (object sTarget, bool sResetStack = true)
		{
			NWDDataInspector tShareInstance = ShareInstance ();
			if (sResetStack == true) {
				tShareInstance.mObjectsList = new List<object> ();
			} else {
				tShareInstance.mObjectsList.RemoveRange (tShareInstance.ActualIndex + 1, tShareInstance.mObjectsList.Count - tShareInstance.ActualIndex - 1);
			}
			tShareInstance.ActualIndex = tShareInstance.mObjectsList.Count;
			tShareInstance.mObjectsList.Add (sTarget);
			tShareInstance.mObjectInEdition = sTarget;
			tShareInstance.Repaint ();
			tShareInstance.RemoveActualFocus = true;
			tShareInstance.Focus();
//			GUI.FocusControl (NWDConstants.K_CLASS_FOCUS_ID);
		}

		public static object ObjectInEdition ()
		{
			return ShareInstance ().mObjectInEdition;
		}
		// Use this for initialization
		void Start ()
		{
			//Debug.Log ("Start");
		}

		public void OnEnable ()
		{
			//Debug.Log ("OnEnable");
		}

//		public void Update ()
//		{
//			Debug.Log ("Update");
//		}

		void OnDestroy() {
			
		}

		public Vector2 ScrollPosition = Vector2.zero;

		public void OnGUI ()
		{
			if (RemoveActualFocus == true) {
				GUI.FocusControl (null);
				RemoveActualFocus = false;
			}
			//Debug.Log ("OnGUI");
			titleContent = new GUIContent ();
			titleContent.text = "NWD Inspector";

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
	}
}
//=====================================================================================================================
#endif