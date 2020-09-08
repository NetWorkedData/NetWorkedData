//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#endif
//MACRO_DEFINE #if NWD_EXAMPLE_MACRO
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
//=====================================================================================================================
using UnityEditor;
using NetWorkedData.NWDEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDExample : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons in edition state of object.
        /// </summary>
        /// <returns><c>true</c>, if object need to be update, <c>false</c> or not not to be update.</returns>
        /// <param name="sNeedBeUpdate">If set to <c>true</c> need be update in enter.</param>
        public override bool AddonEdited(bool sNeedBeUpdate)
        {
            // do base
            bool tNeedBeUpdate =  base.AddonEdited(sNeedBeUpdate);
            if (tNeedBeUpdate == true)
            {
                // do something
            }
            return tNeedBeUpdate;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons editor intreface expected height.
        /// </summary>
        /// <returns>The editor expected height.</returns>
        public override float AddonEditorHeight(float sWidth)
        {
            //EditorMatrixLine = 5;
            //EditorMatrixColunm = 1;
            //return base.AddonEditorHeight(sWidth);
            return LayoutEditorHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons editor interface.
        /// </summary>
        /// <returns>The editor height addon.</returns>
        /// <param name="sRect">S in rect.</param>
        public override void AddonEditor(Rect sRect)
        {
            //GUILayout.BeginArea(sRect);
            //EditorGUILayout.BeginVertical();
            //NWDGUILayout.Separator();
            EditorGUILayout.TextField("jhhjkkhj", "jkhjhkhjk");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            //EditorGUILayout.EndVertical();
            //if (Event.current.type == EventType.Repaint)
            //{
            //    LayoutEditorHeight = GUILayoutUtility.GetLastRect().height;
            //}
            //GUILayout.EndArea();

            //base.AddonEditor(sRect);
            //NWDGUI.Separator(EditorMatrix[0, EditorMatrixIndex]);
            //EditorMatrixIndex++;
            //// Draw Editor addon in matrix
            //if (GUI.Button(EditorMatrix[0, EditorMatrixIndex], "Click me"))
            //{
            //    // do that... 
            //}
            //EditorMatrixIndex++;
            //NWDGUI.Separator(EditorMatrix[0, EditorMatrixIndex]);
            //EditorMatrixIndex++;
            // Draw the interface addon for editor
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the height of node draw.
        /// </summary>
        /// <returns>The on node draw height.</returns>
        public override float AddOnNodeDrawHeight(float sCardWidth)
        {
            EditorNodalMatrixLine = 5;
            EditorNodalMatrixColunm = 1;
            return base.AddOnNodeDrawHeight(sCardWidth);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds node draw.
        /// </summary>
        /// <param name="sRect">S rect.</param>
        public override void AddOnNodeDraw(Rect sRect)
        {
            base.AddOnNodeDraw(sRect);
            // Draw Editor addon in matrix
            if (GUI.Button(EditorNodalMatrix[0, EditorNodalMatrixIndex], "Nodal click me"))
            {
                // do that... 
            }
            EditorNodalMatrixIndex++;
            // Draw the interface addon for editor nodal
            // Or use Layout
            GUILayout.BeginArea(sRect);
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.Label("jjj");
            GUILayout.EndArea();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool AddonErrorFound()
        {
            bool rReturnErrorFound = base.AddonErrorFound();
            // check if you found error in Data values.
            // normal way is return false!
            return rReturnErrorFound;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
//MACRO_DEFINE #endif //NWD_EXAMPLE_MACRO
