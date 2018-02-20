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
    /// NWD Node Editor. This editor can edit data as nodal card.
    /// </summary>
    public class NWDNodeEditor : EditorWindow
	{
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The document of deck.
        /// </summary>
        private NWDNodeDocument Document = new NWDNodeDocument();
        //-------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="T:NetWorkedData.NWDNodeEditor"/> class.
        /// </summary>
        public NWDNodeEditor()
        {
            this.autoRepaintOnSceneChange = false;
            this.wantsMouseEnterLeaveWindow = false;
            this.wantsMouseMove = false;
            //Document.SetData(null, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The k node editor shared instance.
        /// </summary>
        public static NWDNodeEditor kNodeEditorSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Shared instance.
        /// </summary>
        public static void SharedInstance()
        {
            kNodeEditorSharedInstance = EditorWindow.GetWindow(typeof(NWDNodeEditor)) as NWDNodeEditor;
            kNodeEditorSharedInstance.Show();
            kNodeEditorSharedInstance.Focus();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the object in node window.
        /// </summary>
        /// <param name="sSelection">S selection.</param>
        public static void SetObjectInNodeWindow(NWDTypeClass sSelection)
        {
            kNodeEditorSharedInstance = EditorWindow.GetWindow(typeof(NWDNodeEditor)) as NWDNodeEditor;
            kNodeEditorSharedInstance.Show();
            //tNodeEditor.ShowUtility();
            kNodeEditorSharedInstance.Focus();
            kNodeEditorSharedInstance.SetSelection(sSelection);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Redraw.
        /// </summary>
        public static void ReDraw()
        {
            if (kNodeEditorSharedInstance != null)
            {
                kNodeEditorSharedInstance.Repaint();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// On destroy.
        /// </summary>
        void OnDestroy()
        {
            //Debug.Log("Destroyed...");
            kNodeEditorSharedInstance = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Updates the node window but prevent.
        /// </summary>
        /// <param name="sSelection">S selection.</param>
        public static void UpdateNodeWindow(NWDTypeClass sSelection)
        {
           // Debug.Log("NWDNodeEditor UpdateNodeWindow");
            if (kNodeEditorSharedInstance != null)
            {
                kNodeEditorSharedInstance.Document.ReAnalyze();
                kNodeEditorSharedInstance.Repaint();
            }

        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Set selection.
        /// </summary>
        /// <param name="sSelection">S selection.</param>
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
            Document.LoadClasses();
            Repaint();
		}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The m scroll position.
        /// </summary>
        public static Vector2 mScrollPosition = Vector2.zero;
        /// <summary>
        /// The m last mouse position.
        /// </summary>
        Vector2 mLastMousePosition = new Vector2(-1.0F, -1.0F);
        //-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the OnGUI event. Create the interface to enter a new class.
		/// </summary>
		public void OnGUI ()
		{
            // Debug.Log("NWDNodeEditor OnGUI");
            NWDConstants.LoadImages();
            NWDConstants.LoadStyles();

            Rect tScrollViewRect = new Rect(0, 0, position.width, position.height);
            //EditorGUI.DrawRect(tScrollViewRect, new Color (0.5F,0.5F,0.5F,1.0F));
            mScrollPosition = GUI.BeginScrollView(tScrollViewRect,mScrollPosition,Document.Dimension());
            Document.Draw(tScrollViewRect);
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
