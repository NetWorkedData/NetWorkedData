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
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEditor;
using NetWorkedData.NWDEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {

#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public const string K_EDITOR_LAST_TYPE_KEY = "K_EDITOR_LAST_TYPE_KEY_5fdshjktr";
        public const string K_EDITOR_LAST_REFERENCE_KEY = "K_EDITOR_LAST_REFERENCE_KEY_ed5f5dtr";
        //-------------------------------------------------------------------------------------------------------------
        public NWDTypeClass RestaureObjectInEdition()
        {
            string tTypeEdited = NWDProjectPrefs.GetString(K_EDITOR_LAST_TYPE_KEY);
            string tLastReferenceEdited = NWDProjectPrefs.GetString(K_EDITOR_LAST_REFERENCE_KEY);
            NWDTypeClass rObject = ObjectInEditionReccord(tTypeEdited, tLastReferenceEdited);
            if (rObject != null)
            {
                SetObjectInEdition(rObject);
            }
            return rObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTypeClass ObjectInEditionReccord(string sClassPHP, string sReference)
        {
            NWDTypeClass rObject = null;
            if (!string.IsNullOrEmpty(sClassPHP) && !string.IsNullOrEmpty(sReference))
            {
                if (sClassPHP == ClassNamePHP)
                {
                    NWDTypeClass tObj = GetDataByReference(sReference);
                    rObject = tObj;
                }
            }
            return rObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SaveObjectInEdition()
        {
            NWDTypeClass tObject = NWDDataInspector.ObjectInEdition() as NWDTypeClass;
            if (tObject == null)
            {
                NWDProjectPrefs.SetString(K_EDITOR_LAST_TYPE_KEY, string.Empty);
                NWDProjectPrefs.SetString(K_EDITOR_LAST_REFERENCE_KEY, string.Empty);
            }
            else
            {
                NWDProjectPrefs.SetString(K_EDITOR_LAST_TYPE_KEY, NWDBasisHelper.FindTypeInfos(tObject.GetType()).ClassNamePHP);
                NWDProjectPrefs.SetString(K_EDITOR_LAST_REFERENCE_KEY, tObject.Reference);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDBasis GetObjectInEdition()
        {
            NWDBasis tSelected = null;
            if (PanelActivate == NWDBasisHelperPanel.Data)
            {
                tSelected = mObjectInEdition as NWDBasis;
            }
            else
            {
                tSelected = NWDDataInspector.ObjectInEdition() as NWDBasis;
            }
            return tSelected;
        }
        //-------------------------------------------------------------------------------------------------------------
        private string LastSelectedObjectKey() { return ClassNamePHP + "_last_selected_ref"; }
        //-------------------------------------------------------------------------------------------------------------
        public void SetObjectInEdition(NWDTypeClass sData, bool sResetStack = true, bool sFocus = true)
        {
            NWDBenchmark.Start();
            //NWDBenchmark.Trace();
            bool tInspectorDataPanel = false;
            if (sData != null)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(sData.GetType());
                if (tHelper != null)
                {
                    tInspectorDataPanel = tHelper.PanelActivate == NWDBasisHelperPanel.Data;
                }
            }

            bool tManaged = false;
            if (tInspectorDataPanel == true)
            {
                //NWDBenchmark.Trace();
                mObjectInEdition = sData;
                if (mObjectInEdition != null)
                {
                    NWDProjectPrefs.SetString(LastSelectedObjectKey(), mObjectInEdition.Reference);
                    NWDTypeWindow tWindowFound = null;
                    foreach (NWDTypeWindow tWindow in NWDDataManager.SharedInstance().EditorWindowsInManager(sData.GetType()))
                    {
                        tWindowFound = tWindow;
                        tWindow.Focus();
                        tWindow.SelectTab(sData.GetType());
                        tWindow.Repaint();
                        tManaged = true;
                    };
                    if (tWindowFound == null)
                    {
                        Debug.LogWarning("No Window found to manage this class : " + sData.GetType().Name);
                    }
                }
                else
                {
                    NWDProjectPrefs.SetString(LastSelectedObjectKey(), string.Empty);
                }
            }
            if (tManaged == false)
            {
                //NWDBenchmark.Trace();
                GUI.FocusControl(null);
                NWDDataInspector.InspectNetWorkedData(sData, sResetStack, sFocus);
                if (sData != null)
                {
                    NWDBasisEditor.ObjectEditorLastType = sData.GetType();
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(NWDBasisEditor.ObjectEditorLastType);
                }
                else if (NWDBasisEditor.ObjectEditorLastType != null)
                {
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(NWDBasisEditor.ObjectEditorLastType);
                    NWDBasisEditor.ObjectEditorLastType = null;
                }
                SaveObjectInEdition();
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool IsObjectInEdition(NWDTypeClass sObject)
        {
            bool rReturn = false;
            if (PanelActivate == NWDBasisHelperPanel.Data)
            {
                if (mObjectInEdition == sObject)
                {
                    rReturn = true;
                }
            }
            else
            {
                if (NWDDataInspector.ObjectInEdition() == sObject)
                {
                    rReturn = true;
                }
            }
            return rReturn;
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        public void DeleteUser(NWDAppEnvironment sEnvironment)
        {
            //if (kAccountDependent == true)
            if (TemplateHelper.GetAccountDependent() != NWDTemplateAccountDependent.NoAccountDependent)
            {
                // reset last sync to zero
                SynchronizationSetNewTimestamp(sEnvironment, 0); // set to 0 ... only for data AccountDependent, so that's not affect the not connected data (game's data)
                                                                 // delete all datas for this user
                foreach (NWDTypeClass tObject in Datas)
                {
                    if (tObject.IsReacheableBy(null, NWDAccount.CurrentReference()))
                    {
                        tObject.DeleteData();
                    }
                }
                // need to reload this data now : to remove all tObjects from memory!
                //LoadTableEditor();
                LoadFromDatabase(string.Empty, true);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        //public NWDTypeClass NewData()
        //{
        //    NWDTypeClass rReturn = Activator.CreateInstance(ClassType, new object[] { true }) as NWDTypeClass;
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //        public void FlushTrash(NWDTypeClass sObject)
        //        {
        //            //Debug.Log ("Flush trash ... the delete this object if it's necessary");
        //#if UNITY_EDITOR
        //            if (sObject.XX > 0 && sObject.DevSync > 0 && sObject.PreprodSync > 0 && sObject.ProdSync > 0)
        //            {
        //                //              Debug.Log (sObject.Reference + "Must be trashed!");
        //                //              RemoveObjectInListOfEdition (sObject);
        //                //              if (IsObjectInEdition (sObject)) {
        //                //                  SetObjectInEdition (null);
        //                //              }
        //                //              this.AddonDeleteMe();
        //                //  NWDDataManager.SharedInstance().DeleteObjectDirect(this, AccountDependent());
        //            }
        //#else
        //            if (sObject.XX > 0)
        //            {
        //                //Debug.Log (sObject.Reference + "Must be trashed!");
        //                //RemoveObjectInListOfEdition (sObject);
        //                //sObject.AddonDeleteMe();
        //                //NWDDataManager.SharedInstance().DeleteObjectDirect(sObject, AccountDependent());
        //            }
        //#endif
        //        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
