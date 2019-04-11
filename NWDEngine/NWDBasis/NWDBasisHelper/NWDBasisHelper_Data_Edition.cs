//=====================================================================================================================
//
// ideMobi copyright 2019 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
using SQLite4Unity3d;
using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        public const string K_EDITOR_LAST_TYPE_KEY = "K_EDITOR_LAST_TYPE_KEY_5fdshjktr";
        public const string K_EDITOR_LAST_REFERENCE_KEY = "K_EDITOR_LAST_REFERENCE_KEY_ed5f5dtr";
        //-------------------------------------------------------------------------------------------------------------
       public NWDTypeClass New_RestaureObjectInEdition()
        {
            string tTypeEdited = EditorPrefs.GetString(K_EDITOR_LAST_TYPE_KEY);
            string tLastReferenceEdited = EditorPrefs.GetString(K_EDITOR_LAST_REFERENCE_KEY);
            NWDTypeClass rObject = New_ObjectInEditionReccord(tTypeEdited, tLastReferenceEdited);
            if (rObject != null)
            {
                New_SetObjectInEdition(rObject);
            }
            return rObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTypeClass New_ObjectInEditionReccord(string sClassPHP, string sReference)
        {
            NWDTypeClass rObject = null;
            if (!string.IsNullOrEmpty(sClassPHP) && !string.IsNullOrEmpty(sReference))
            {
                if (sClassPHP == ClassNamePHP)
                {
                    NWDTypeClass tObj = New_GetDataByReference(sReference);
                    rObject = tObj;
                }
            }
            return rObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void New_SaveObjectInEdition()
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
        public void New_SetObjectInEdition(NWDTypeClass sObject, bool sResetStack = true, bool sFocus = true)
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
            New_SaveObjectInEdition();
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool New_IsObjectInEdition(NWDTypeClass sObject)
        {
            bool rReturn = false;
            if (NWDDataInspector.ObjectInEdition() == sObject)
            {
                rReturn = true;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================