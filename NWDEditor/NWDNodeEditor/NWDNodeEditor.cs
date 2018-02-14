//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using System;
using System.Reflection;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
	/// <summary>
	/// NWD editor new class. Can create a new classes based on NWDExample automatically from the form generated in this editor window.
	/// </summary>
    public class NWDNodeEditor : EditorWindow
	{
        //-------------------------------------------------------------------------------------------------------------
        private NWDTypeClass Selection;
        //-------------------------------------------------------------------------------------------------------------
        public void SetSelection(NWDTypeClass sSelection)
        {
            Selection = sSelection;
            Repaint();
        }
        //-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the enable event.
		/// </summary>
		public void OnEnable ()
		{
            titleContent = new GUIContent ("NWDNodeEditor");
		}
        //-------------------------------------------------------------------------------------------------------------
        public static Vector2 m_NodeEditorScrollPosition = Vector2.zero;
        //-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the OnGUI event. Create the interface to enter a new class.
		/// </summary>
		public void OnGUI ()
		{

            m_NodeEditorScrollPosition = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height),m_NodeEditorScrollPosition, new Rect(0, 0, 1024,1024));

            titleContent = new GUIContent ("NWDNodeEditor");
            if (Selection != null)
            {
                Type tType = Selection.GetType();
                var tMethodInfo = tType.GetMethod("DrawNode", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    tMethodInfo.Invoke(Selection, null);
                }


                //GUI.Label(new Rect(10, 10, 100, 100), "Object");
                //if (GUI.Button(new Rect(10, 20, 100, 100), "edit"))
                //{


                //    Type tType = Selection.GetType();
                //    var tMethodInfo = tType.GetMethod("SetObjectInEdition", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                //    if (tMethodInfo != null)
                //    {
                //        tMethodInfo.Invoke(null, new object[] { Selection ,true, true});
                //    }
                //    //NWDBasis<K>.SetObjectInEdition(Selection);
                //}
            }
            else
            {
                GUI.Label(new Rect(10, 10, 100, 100), "NO Object");
            }

            GUI.EndScrollView();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif
