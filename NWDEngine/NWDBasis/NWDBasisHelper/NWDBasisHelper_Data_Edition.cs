//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:24:55
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
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
            string tTypeEdited = EditorPrefs.GetString(K_EDITOR_LAST_TYPE_KEY);
            string tLastReferenceEdited = EditorPrefs.GetString(K_EDITOR_LAST_REFERENCE_KEY);
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
                EditorPrefs.SetString(K_EDITOR_LAST_TYPE_KEY, string.Empty);
                EditorPrefs.SetString(K_EDITOR_LAST_REFERENCE_KEY, string.Empty);
            }
            else
            {
                EditorPrefs.SetString(K_EDITOR_LAST_TYPE_KEY, NWDBasisHelper.FindTypeInfos(tObject.GetType()).ClassNamePHP);
                EditorPrefs.SetString(K_EDITOR_LAST_REFERENCE_KEY, tObject.Reference);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDBasis GetObjectInEdition()
        {
            NWDBasis tSelected = null;
            if (InspectorActions == true)
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
        public void SetObjectInEdition(NWDTypeClass sObject, bool sResetStack = true, bool sFocus = true)
        {

            if (InspectorActions == true)
            {
                mObjectInEdition = sObject;
                if (mObjectInEdition != null)
                {
                    EditorPrefs.SetString(LastSelectedObjectKey(), mObjectInEdition.Reference);
                    foreach (NWDTypeWindow tWindow in NWDDataManager.SharedInstance().EditorWindowsInManager(sObject.GetType()))
                    {
                        tWindow.Focus();
                        tWindow.SelectTab(sObject.GetType());
                        tWindow.Repaint();
                    };
                }
                else
                {
                    EditorPrefs.SetString(LastSelectedObjectKey(), string.Empty);
                }
            }
            else
            {
                GUI.FocusControl(null);
                NWDDataInspector.InspectNetWorkedData(sObject, sResetStack, sFocus);
                if (sObject != null)
                {
                    NWDBasisEditor.ObjectEditorLastType = sObject.GetType();
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(NWDBasisEditor.ObjectEditorLastType);
                }
                else if (NWDBasisEditor.ObjectEditorLastType != null)
                {
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(NWDBasisEditor.ObjectEditorLastType);
                    NWDBasisEditor.ObjectEditorLastType = null;
                }
                SaveObjectInEdition();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool IsObjectInEdition(NWDTypeClass sObject)
        {
            bool rReturn = false;
            if (InspectorActions == true)
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
            if (kAccountDependent == true)
            {
                // reset last sync to zero
                SynchronizationSetNewTimestamp(sEnvironment, 0); // set to 0 ... only for data AccountDependent, so that's not affect the not connected data (game's data)
                                                                 // delete all datas for this user
                foreach (NWDTypeClass tObject in Datas)
                {
                    if (tObject.IsReacheableByAccount(NWDAccount.CurrentReference()))
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
        public void FlushTrash(NWDTypeClass sObject)
        {
            //Debug.Log ("Flush trash ... the delete this object if it's necessary");
#if UNITY_EDITOR
            if (sObject.XX > 0 && sObject.DevSync > 0 && sObject.PreprodSync > 0 && sObject.ProdSync > 0)
            {
                //              Debug.Log (sObject.Reference + "Must be trashed!");
                //              RemoveObjectInListOfEdition (sObject);
                //              if (IsObjectInEdition (sObject)) {
                //                  SetObjectInEdition (null);
                //              }
                //              this.AddonDeleteMe();
                //  NWDDataManager.SharedInstance().DeleteObjectDirect(this, AccountDependent());
            }
#else
            if (sObject.XX > 0)
            {
                //Debug.Log (sObject.Reference + "Must be trashed!");
                //RemoveObjectInListOfEdition (sObject);
                //sObject.AddonDeleteMe();
                //NWDDataManager.SharedInstance().DeleteObjectDirect(sObject, AccountDependent());
            }
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================