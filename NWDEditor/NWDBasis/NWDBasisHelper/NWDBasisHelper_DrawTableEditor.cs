//=====================================================================================================================
//
// ideMobi copyright 2019 
// All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
using SQLite4Unity3d;
using BasicToolBox;
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        public string New_GetReferenceOfDataInEdition()
        {
            string rReturn = null;
            NWDTypeClass tObject = NWDDataInspector.ObjectInEdition() as NWDTypeClass;
            if (tObject != null)
            {
                rReturn = string.Copy(tObject.Reference);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        public void New_RestaureDataInEditionByReference(string sReference)
        {
            NWDTypeClass tObject = null;
            if (sReference != null)
            {
                if (DatasByReference.ContainsKey(sReference))
                {
                    tObject = DatasByReference[sReference];
                }
                if (tObject != null)
                {
                    if (EditorTableDatas.Contains(tObject))
                    {
                        New_SetObjectInEdition(tObject);
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public  void New_SelectAllObjectInTableList()
        {
            List<NWDTypeClass> tListToUse = new List<NWDTypeClass>();
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
            {
                tListToUse.Add(tKeyValue.Key);
            }
            foreach (NWDTypeClass tObject in tListToUse)
            {
                EditorTableDatasSelected[tObject] = true;
            }
            //IntegritySelection();
        }
        //-------------------------------------------------------------------------------------------------------------
        public  void New_DeselectAllObjectInTableList()
        {
            List<NWDTypeClass> tListToUse = new List<NWDTypeClass>();
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
            {
                tListToUse.Add(tKeyValue.Key);
            }
            foreach (NWDTypeClass tObject in tListToUse)
            {
                EditorTableDatasSelected[tObject] = false;
            }
            //IntegritySelection();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void New_InverseSelectionOfAllObjectInTableList()
        {
            List<NWDTypeClass> tListToUse = new List<NWDTypeClass>();
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
            {
                tListToUse.Add(tKeyValue.Key);
            }
            foreach (NWDTypeClass tObject in tListToUse)
            {
                EditorTableDatasSelected[tObject] = !EditorTableDatasSelected[tObject];
            }
            //IntegritySelection();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void New_SelectAllObjectEnableInTableList()
        {
            List<NWDTypeClass> tListToUse = new List<NWDTypeClass>();
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
            {
                tListToUse.Add(tKeyValue.Key);
            }
            foreach (NWDTypeClass tObject in tListToUse)
            {
                EditorTableDatasSelected[tObject] = tObject.IsEnable();
            }
            //IntegritySelection();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void New_SelectAllObjectDisableInTableList()
        {
            List<NWDTypeClass> tListToUse = new List<NWDTypeClass>();
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
            {
                tListToUse.Add(tKeyValue.Key);
            }
            foreach (NWDTypeClass tObject in tListToUse)
            {
                EditorTableDatasSelected[tObject] = !tObject.IsEnable();
            }
            //IntegritySelection();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif