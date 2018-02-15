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
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	/// <summary>
	/// NWD editor new class. Can create a new classes based on NWDExample automatically from the form generated in this editor window.
	/// </summary>
    public class NWDNodeEditor : EditorWindow
	{
        //-------------------------------------------------------------------------------------------------------------
        private NWDNodeDocument Document = new NWDNodeDocument();
        //-------------------------------------------------------------------------------------------------------------
        public NWDNodeEditor()
        {
            this.autoRepaintOnSceneChange = false;
            this.wantsMouseEnterLeaveWindow = false;
            this.wantsMouseMove = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetSelection(NWDTypeClass sSelection)
        {
            Document.SetData(sSelection);
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
        public static Vector2 mScrollPosition = Vector2.zero;
        //-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the OnGUI event. Create the interface to enter a new class.
		/// </summary>
		public void OnGUI ()
		{
            Debug.Log("NWDNodeEditor OnGUI");
            mScrollPosition = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height),mScrollPosition,Document.Dimension());
            Document.Draw();
            GUI.EndScrollView();
		}
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
