//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
//#define NWD_LOG
//#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.IO;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDDataInspector : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDTypeClass mObjectInEdition;
        public List<NWDTypeClass> mObjectsList = new List<NWDTypeClass>();
        public int ActualIndex = 0;
        public bool RemoveActualFocus = true;
        //-------------------------------------------------------------------------------------------------------------
        static NWDDataInspector kShareInstance;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDDataInspector ShareInstance()
        {
            NWDBenchmark.Start();
            if (kShareInstance == null)
            {
                EditorWindow tWindow = EditorWindow.GetWindow(typeof(NWDDataInspector));
                tWindow.Show();
                kShareInstance = (NWDDataInspector)tWindow;
                kShareInstance.minSize = new Vector2(250, 540);
                kShareInstance.maxSize = new Vector2(600, 2048);
            }
            NWDBenchmark.Finish();
            return kShareInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ActiveInspector()
        {
            NWDBenchmark.Start();
            ShareInstance().Show();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ActiveRepaint()
        {
            NWDBenchmark.Start();
            if (kShareInstance != null)
            {
                kShareInstance.Repaint();
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void InspectNetWorkedDataPreview()
        {
            NWDBenchmark.Start();
            ShareInstance().DataPreview();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Refresh()
        {
            NWDBenchmark.Start();
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWDDataInspector));
            foreach (NWDDataInspector tWindow in tWindows)
            {
                tWindow.Repaint();
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DataPreview()
        {
            NWDBenchmark.Start();
            ActualIndex--;
            if (ActualIndex < 0)
            {
                ActualIndex = 0;
            }
            if (mObjectsList.Count > ActualIndex)
            {
                NWDTypeClass tTarget = mObjectsList[ActualIndex];
                mObjectInEdition = tTarget;
                Repaint();
                RemoveActualFocus = true;
                Focus();
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void InspectNetWorkedDataNext()
        {
            NWDBenchmark.Start();
            ShareInstance().DataNext();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DataNext()
        {
            NWDBenchmark.Start();
            ActualIndex++;
            if (ActualIndex >= mObjectsList.Count)
            {
                ActualIndex = 0;
            }
            if (mObjectsList.Count > ActualIndex)
            {
                NWDTypeClass tTarget = mObjectsList[ActualIndex];
                mObjectInEdition = tTarget;
                Repaint();
                RemoveActualFocus = true;
                Focus();
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool InspectNetWorkedPreview()
        {
            return ShareInstance().Preview();
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool Preview()
        {
            return (ActualIndex > 0);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool InspectNetWorkedNext()
        {
            return ShareInstance().Next();
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool Next()
        {
            return (ActualIndex < mObjectsList.Count - 1);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void InspectNetWorkedData(NWDTypeClass sTarget, bool sResetStack = true, bool sFocus = true)
        {
            NWDBenchmark.Start();
            if (sTarget != null)
            {
                if (NWDBasisHelper.FindTypeInfos(sTarget.GetType()).AllDatabaseIsLoaded())
                {
                    if (ShareInstance().mObjectInEdition != sTarget)
                    {
                        ShareInstance().Data(sTarget, sResetStack, sFocus);
                        NWDDataManager.SharedInstance().RepaintWindowsInManager(sTarget.GetType());
                    }
                }
            }
            else
            {
                ShareInstance().Data(null, true, sFocus);
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Data(NWDTypeClass sTarget, bool sResetStack = true, bool sFocus = true)
        {
            NWDBenchmark.Start();
            if (sTarget != null)
            {
                if (NWDBasisHelper.FindTypeInfos(sTarget.GetType()).AllDatabaseIsLoaded())
                {
                    NWDTypeClass tTarget = sTarget as NWDTypeClass;
                    tTarget.ErrorCheck();

                    if (sResetStack == true)
                    {
                        mObjectsList = new List<NWDTypeClass>();
                    }
                    else
                    {
                        if (mObjectsList.Count > ActualIndex)
                        {
                            mObjectsList.RemoveRange(ActualIndex + 1, mObjectsList.Count - ActualIndex - 1);
                        }
                    }
                    ActualIndex = mObjectsList.Count;
                    mObjectsList.Add(sTarget);
                    mObjectInEdition = sTarget;
                    Repaint();
                    RemoveActualFocus = sFocus;
                    if (sFocus == true)
                    {
                        Focus();
                    }
                    NWDNodeEditor.Refresh();
                }
            }
            else
            {
                mObjectsList = new List<NWDTypeClass>();
                mObjectInEdition = null;
                Repaint();
                RemoveActualFocus = sFocus;
                if (sFocus == true)
                {
                    Focus();
                }
                NWDNodeEditor.Refresh();
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDTypeClass ObjectInEdition()
        {
            return ShareInstance().mObjectInEdition;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnEnable()
        {
            NWDBenchmark.Start();
            // set min size
            NormalizeWidth = 300;
            NormalizeHeight = 500;
            // set title
            TitleInit(NWDConstants.K_APP_SYNC_INSPECTOR_TITLE, typeof(NWDDataInspector));
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnPreventGUI()
        {
            NWDBenchmark.Start();
            minSize = new Vector2(NWDGUI.kNodeCardWidth, NWDGUI.kNodeCardHeight);
            maxSize = new Vector2(NWDGUI.kNodeCardWidth * 2, NWDGUI.kNodeCardHeight * 2);
            if (RemoveActualFocus == true)
            {
                GUI.FocusControl(null);
                RemoveActualFocus = false;
            }
            if (mObjectInEdition == null)
            {
               GUI.Label(new Rect(0, 0, position.width, position.height), NWDConstants.K_EDITOR_NO_DATA_SELECTED, NWDGUI.kInspectorNoData);
            }
            else
            {
                mObjectInEdition.DrawEditor(new Rect(0, 0, position.width, position.height), true, null, this);
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
