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
    public partial class NWDItemHelper : NWDHelper<NWDItem>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonActions()
        {
            NWDGUILayout.Section("Addon Actions");
            NWDGUILayout.Label("...no addon actions...");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDItem : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        static int kOwnershipAddValue;
        //-------------------------------------------------------------------------------------------------------------
        static int kOwnershipSetValue;
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonNodal(Rect sRect)
        {
            NWDUserOwnership tOwnership = NWDUserOwnership.FindReachableByItem(this, false);
            EditorGUI.indentLevel = 1;
            if (tOwnership != null)
            {
                EditorGUILayout.LabelField("You have " + tOwnership.Quantity + " '" + this.InternalKey + "' in your ownership!");
            }
            else
            {
                EditorGUILayout.LabelField("You have not '" + this.InternalKey + "' in your ownership!");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonEditor(Rect sRect)
        {
            base.AddonEditor(sRect);
            NWDGUILayout.Title("More actions");
            NWDGUILayout.SubSection("Ownership");
            NWDUserOwnership tOwnership = NWDUserOwnership.FindReachableByItem(this, false);
            if (tOwnership != null)
            {
                EditorGUILayout.LabelField("You have " + tOwnership.Quantity + " " + this.InternalKey + " in your ownership!");
                NWDGUILayout.SubSection("Management");
                if (GUILayout.Button("Select Ownership", NWDGUI.kMiniButtonStyle))
                {
                    NWDDataInspector.InspectNetWorkedData(tOwnership);
                }
                if (GUILayout.Button("Reset to zero", NWDGUI.kMiniButtonStyle))
                {
                    NWDUserOwnership.SetItemToOwnership(this, 0);
                }

                NWDGUILayout.SubSection("Change value");
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();
                kOwnershipSetValue = EditorGUILayout.IntField("value to set", kOwnershipSetValue);
                kOwnershipAddValue = EditorGUILayout.IntField("value to add", kOwnershipAddValue);
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical();
                if (GUILayout.Button("set", NWDGUI.kMiniButtonStyle))
                {
                    NWDUserOwnership.SetItemToOwnership(this, kOwnershipSetValue);
                }
                if (GUILayout.Button("add", NWDGUI.kMiniButtonStyle))
                {
                    NWDUserOwnership.AddItemToOwnership(this, kOwnershipAddValue);
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.BeginVertical();
                if (GUILayout.Button("add 1", NWDGUI.kMiniButtonStyle))
                {
                    NWDUserOwnership.AddItemToOwnership(this, 1);
                }
                if (GUILayout.Button("add 10", NWDGUI.kMiniButtonStyle))
                {
                    NWDUserOwnership.AddItemToOwnership(this, 10);
                }
                if (GUILayout.Button("add 100", NWDGUI.kMiniButtonStyle))
                {
                    NWDUserOwnership.AddItemToOwnership(this, 100);
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginVertical();
                if (GUILayout.Button("remove 1", NWDGUI.kMiniButtonStyle))
                {
                    NWDUserOwnership.AddItemToOwnership(this, -1);
                }
                if (GUILayout.Button("remove 10", NWDGUI.kMiniButtonStyle))
                {
                    NWDUserOwnership.AddItemToOwnership(this, -10);
                }
                if (GUILayout.Button("remove 100", NWDGUI.kMiniButtonStyle))
                {
                    NWDUserOwnership.AddItemToOwnership(this, -100);
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                if (GUILayout.Button("Create Ownership"))
                {
                    NWDUserOwnership.FindReachableByItem(this, true);
                }
            }
        }
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
