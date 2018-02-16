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
        Vector2 mLastMousePosition = new Vector2(-1.0F, -1.0F);
        //-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the OnGUI event. Create the interface to enter a new class.
		/// </summary>
		public void OnGUI ()
		{
            Debug.Log("NWDNodeEditor OnGUI");

            Rect tScrollViewRect = new Rect(0, 0, position.width, position.height);
            //EditorGUI.DrawRect(tScrollViewRect, new Color (0.5F,0.5F,0.5F,1.0F));
            mScrollPosition = GUI.BeginScrollView(tScrollViewRect,mScrollPosition,Document.Dimension());
            Document.Draw();
            GUI.EndScrollView();


            // Check if the mouse is above our scrollview.
            if (tScrollViewRect.Contains(Event.current.mousePosition))
            {
                //Debug.Log("NWDNodeEditor event in rect");
                // Only move if we are hold down mouse button, and the mouse is moving.
                if (Event.current.type == EventType.MouseDrag)
                {
                    //Debug.Log("NWDNodeEditor MouseDrag");
                    // Current position
                    Vector2 currPos = Event.current.mousePosition;

                    // Only move if the distance between the last mouse position and the current is less than 50.
                    // Without this it jumps during the drag.
                    if (Vector2.Distance(currPos, mLastMousePosition) < 50)
                    {
                        // Calculate the delta x and y.
                        float x = mLastMousePosition.x - currPos.x;
                        float y = mLastMousePosition.y - currPos.y;

                        // Add the delta moves to the scroll position.
                        mScrollPosition.x += x;
                        mScrollPosition.y += y;
                        Event.current.Use();
                    }
                    // Set the last mouse position to the current mouse position.
                    mLastMousePosition = currPos;
                }
            }
		}
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
