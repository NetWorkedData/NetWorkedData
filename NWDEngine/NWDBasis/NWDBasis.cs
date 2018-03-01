//=====================================================================================================================
//
// ideMobi copyright 2017 
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
    /// <summary>
    /// NWD basis the base object of the NetWorkedData framework. This object can be synchronize, modified, update, connect with other NWDBasis <K> Object, etc.
    /// And this object's class can be connect with GameObject by properties autogenerate
    /// </summary>
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        //public bool NWDInserted = false;
        [PrimaryKey, AutoIncrement, NWDNotEditable]
        public int ID
        {
            get; set;
        }

        [Indexed("DefaultIndex", 0)]
        [Indexed("EditorIndex", 0)]
        [Indexed("AccountIndex", 0)]
        [NWDNotEditable]
        public string Reference
        {
            get; set;
        }
        [Indexed("DefaultIndex", 0)]
        [Indexed("EditorIndex", 0)]
        [Indexed("AccountIndex", 0)]
        [NWDNotEditable]
        public NWDVersionType MinVersion
        {
            get; set;
        }
        [Indexed("DefaultIndex", 0)]
        [Indexed("EditorIndex", 0)]
        [Indexed("AccountIndex", 0)]
        [NWDNotEditable]
        public string ReferenceVersionned
        {
            get; set;
        }

        [Indexed("InternalIndex", 0)]
        [Indexed("EditorIndex", 0)]
        public string InternalKey
        {
            get; set;
        }
        public override string InternalKeyValue()
        {
            return InternalKey;
        }
        public string InternalDescription
        {
            get; set;
        }
        public override string InternalDescriptionValue()
        {
            return InternalDescription;
        }
        public string Preview
        {
            get; set;
        }

        [Indexed("DefaultIndex", 1)]
        [Indexed("AccountIndex", 0)]
        public bool AC
        {
            get; set;
        }

        [NWDNotEditable]
        public int DC
        {
            get; set;
        }

        [Indexed("DefaultIndex", 3)]
        [Indexed("EditorIndex", 0)]
        [Indexed("AccountIndex", 0)]
        [NWDNotEditable]
        public int DM
        {
            get; set;
        }
        [NWDNotEditable]
        public int DD
        {
            get; set;
        }
        [NWDNotEditable]
        public int XX
        {
            get; set;
        }
        [NWDNotEditable]
        public string Integrity
        {
            get; set;
        }
        [NWDNotEditable]
        public int DS
        {
            get; set;
        }
        [NWDNotEditable]
        public int DevSync
        {
            get; set;
        }
        [NWDNotEditable]
        public int PreprodSync
        {
            get; set;
        }
        [NWDNotEditable]
        public int ProdSync
        {
            get; set;
        }
        public int Tag
        {
            get; set;
        }
        [NWDNotEditable]
        public string ServerHash
        {
            get; set;
        }
        [NWDNotEditable]
        public string ServerLog
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string m_SearchInternalName = "";
        public static string m_SearchInternalDescription = "";
        public static NWDBasisTag m_SearchTag = NWDBasisTag.NoTag;
        public static Vector2 m_ScrollPositionCard;
        public static bool mSearchShowing = false;
        //-------------------------------------------------------------------------------------------------------------
        public static List<string> ObjectsInEditorTableKeyList = new List<string>();
        public static List<string> ObjectsInEditorTableList = new List<string>();
        public static List<bool> ObjectsInEditorTableSelectionList = new List<bool>();
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Reorders the name of the list of management by.
        /// Just reload the list for the pop-up menu 
        /// </summary>
        public static void ReorderListOfManagementByName()
        {
            // apply all modifications 
            NWDDataManager.SharedInstance.UpdateQueueExecute();
            // must be more efficient
            LoadTableEditor();
            FilterTableEditor();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void IntegritySelection()
        {
            foreach (string tObjectReference in ObjectsInEditorTableList)
            {
                int tIndex = ObjectsByReferenceList.IndexOf(tObjectReference);
                if (ObjectsInEditorTableSelectionList[tIndex] == true)
                {
                    // I test Integrity
                    NWDBasis<K> tObject = ObjectsList[tIndex] as NWDBasis<K>;
                    if (tObject.TestIntegrity() == false || tObject.XX > 0)
                    {
                        ObjectsInEditorTableSelectionList[tIndex] = false;
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SelectAllObjectInTableList()
        {
            ObjectsInEditorTableSelectionList.ForEach(delegate (bool tSelection)
            {
                tSelection = false;
            });
            foreach (string tObjectReference in ObjectsInEditorTableList)
            {
                //Debug.Log ("select ref " + tObjectReference);
                int tIndex = ObjectsByReferenceList.IndexOf(tObjectReference);
                ObjectsInEditorTableSelectionList[tIndex] = true;
            }
            IntegritySelection();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void DeselectAllObjectInTableList()
        {
            ObjectsInEditorTableSelectionList.ForEach(delegate (bool tSelection)
            {
                tSelection = false;
            });
            foreach (string tObjectReference in ObjectsInEditorTableList)
            {
                //Debug.Log ("select ref " + tObjectReference);
                int tIndex = ObjectsByReferenceList.IndexOf(tObjectReference);
                ObjectsInEditorTableSelectionList[tIndex] = false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void InverseSelectionOfAllObjectInTableList()
        {
            foreach (NWDBasis<K> tObject in ObjectsList)
            {
                if (ObjectsInEditorTableList.Contains(tObject.Reference))
                {
                    int tIndex = ObjectsByReferenceList.IndexOf(tObject.Reference);
                    ObjectsInEditorTableSelectionList[tIndex] = !ObjectsInEditorTableSelectionList[tIndex];
                }
                else
                {
                    int tIndex = ObjectsByReferenceList.IndexOf(tObject.Reference);
                    ObjectsInEditorTableSelectionList[tIndex] = false;
                }
            }
            IntegritySelection();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SelectAllObjectDisableInTableList()
        {
            foreach (NWDBasis<K> tObject in ObjectsList)
            {
                if (ObjectsInEditorTableList.Contains(tObject.Reference))
                {
                    int tIndex = ObjectsByReferenceList.IndexOf(tObject.Reference);
                    ObjectsInEditorTableSelectionList[tIndex] = !tObject.AC;
                }
                else
                {
                    int tIndex = ObjectsByReferenceList.IndexOf(tObject.Reference);
                    ObjectsInEditorTableSelectionList[tIndex] = false;
                }
            }
            IntegritySelection();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static IEnumerable<K> SelectForEditionObjects(string sInternalKey, string sInternalDescription, NWDBasisTag sTag)
        {
            SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance.SQLiteConnectionEditor;
            if (AccountDependent())
            {
                tSQLiteConnection = NWDDataManager.SharedInstance.SQLiteConnectionAccount;
            }

            if ((sInternalKey == null || sInternalKey == "") && (sInternalDescription == null || sInternalDescription == "") && (int)sTag < 0)
            {
                //Debug.Log ("no filter");
                return tSQLiteConnection.Table<K>().OrderBy(x => x.InternalKey);
            }
            else
            {
                if (sInternalKey != null && sInternalKey != "" && sInternalDescription != null && sInternalDescription != "")
                {
                    //Debug.Log ("name + description filter");
                    if ((int)sTag >= 0)
                    {
                        return tSQLiteConnection.Table<K>().Where(x => x.InternalKey.Contains(sInternalKey) && x.InternalDescription.Contains(sInternalDescription) && x.Tag.Equals((int)sTag)).OrderBy(x => x.InternalKey);
                    }
                    else
                    {
                        return tSQLiteConnection.Table<K>().Where(x => x.InternalKey.Contains(sInternalKey) && x.InternalDescription.Contains(sInternalDescription)).OrderBy(x => x.InternalKey);
                    }
                }
                else if (sInternalKey != null && sInternalKey != "")
                {
                    //Debug.Log ("name filter");

                    if ((int)sTag >= 0)
                    {
                        return tSQLiteConnection.Table<K>().Where(x => x.InternalKey.Contains(sInternalKey) && x.Tag.Equals((int)sTag)).OrderBy(x => x.InternalKey);
                    }
                    else
                    {
                        return tSQLiteConnection.Table<K>().Where(x => x.InternalKey.Contains(sInternalKey)).OrderBy(x => x.InternalKey);
                    }

                }
                else if (sInternalDescription != null && sInternalDescription != "")
                {
                    //Debug.Log ("description filter");

                    if ((int)sTag >= 0)
                    {
                        return tSQLiteConnection.Table<K>().Where(x => x.InternalDescription.Contains(sInternalDescription) && x.Tag.Equals((int)sTag)).OrderBy(x => x.InternalKey);
                    }
                    else
                    {
                        return tSQLiteConnection.Table<K>().Where(x => x.InternalDescription.Contains(sInternalDescription)).OrderBy(x => x.InternalKey);
                    }

                }
                else if ((int)sTag >= 0)
                {
                    //Debug.Log ("description filter");
                    return tSQLiteConnection.Table<K>().Where(x => x.Tag.Equals((int)sTag)).OrderBy(x => x.InternalKey);

                }
            }
            return null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void FilterTableEditor()
        {
            //Debug.Log("FilterTableEditor()");
            //			Debug.Log ("m_SearchInternalName = " + m_SearchInternalName);
            // change filter, remove selection
            ObjectsInEditorTableSelectionList.ForEach(delegate (bool tSelection)
            {
                tSelection = false;
            });
            // change results
            ObjectsInEditorTableList = new List<string>();
            IEnumerable tEnumerable = SelectForEditionObjects(m_SearchInternalName, m_SearchInternalDescription, m_SearchTag);
            if (tEnumerable != null)
            {
                foreach (NWDBasis<K> tItem in tEnumerable)
                {
                    bool tAdd = true;
                    if (tItem.TestIntegrity() == false && m_ShowIntegrityError == false)
                    {
                        tAdd = false;
                    }
                    if (tItem.AC == true && m_ShowEnable == false)
                    {
                        tAdd = false;
                    }
                    if (tItem.AC == false && m_ShowDisable == false)
                    {
                        tAdd = false;
                    }
                    if (tItem.XX > 0 && m_ShowTrashed == false)
                    {
                        tAdd = false;
                    }
                    if (tAdd == true)
                    {
                        ObjectsInEditorTableList.Add(tItem.Reference);
                    }
                }
            }

            foreach (NWDBasis<K> tObject in ObjectsList)
            {
                if (ObjectsInEditorTableList.Contains(tObject.Reference))
                {
                    // I keep the actual selection 
                }
                else
                {
                    int tIndex = ObjectsByReferenceList.IndexOf(tObject.Reference);
                    ObjectsInEditorTableSelectionList[tIndex] = false;
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        public static void AddObjectInListOfEdition(NWDBasis<K> sObject) // TO DO Rename … AddObjectInListOfManagment
        {
            if (ObjectsList.Contains(sObject) == false)
            {
                sObject.AddonLoadedMe();
                ObjectsList.Add(sObject);
                ObjectsByReferenceList.Add(sObject.Reference);
                ObjectsByKeyList.Add(sObject.InternalKey);
#if UNITY_EDITOR
                ObjectsInEditorTableKeyList.Add(sObject.InternalKey + " <" + sObject.Reference + ">");
                ObjectsInEditorTableList.Add(sObject.Reference);
                ObjectsInEditorTableSelectionList.Add(false);
#endif
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RemoveObjectInListOfEdition(NWDBasis<K> sObject) // TO DO Rename … RemoveObjectFromListOfManagment
        {
            if (ObjectsList.Contains(sObject) == true)
            {
                sObject.AddonUnloadMe();
                int tIndex = ObjectsList.IndexOf(sObject);
                ObjectsList.RemoveAt(tIndex);
                ObjectsByReferenceList.RemoveAt(tIndex);
                ObjectsByKeyList.RemoveAt(tIndex);
#if UNITY_EDITOR
                ObjectsInEditorTableKeyList.RemoveAt(tIndex);
                ObjectsInEditorTableList.Remove(sObject.Reference);
                ObjectsInEditorTableSelectionList.RemoveAt(tIndex);
#endif
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void UpdateObjectInListOfEdition(NWDBasis<K> sObject) // TO DO Rename … UpdateObjectInListOfManagment
        {
            if (ObjectsList.Contains(sObject) == true)
            {
                int tIndex = ObjectsList.IndexOf(sObject);
                // ObjectsList don't change
                ObjectsByReferenceList.RemoveAt(tIndex);
                ObjectsByReferenceList.Insert(tIndex, sObject.Reference);
                ObjectsByKeyList.RemoveAt(tIndex);
                ObjectsByKeyList.Insert(tIndex, sObject.InternalKey);
#if UNITY_EDITOR
                ObjectsInEditorTableKeyList.RemoveAt(tIndex);
                ObjectsInEditorTableKeyList.Insert(tIndex, sObject.InternalKey + " <" + sObject.Reference + ">");
                ObjectsInEditorTableList.RemoveAt(tIndex);
                ObjectsInEditorTableList.Insert(tIndex, sObject.Reference);
                // ObjectsInEditorTableSelectionList don't change
#endif
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void LoadTableEditor()
        {
            //Debug.Log ("LoadTableEditor ##########");

            SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance.SQLiteConnectionEditor;
            if (AccountDependent())
            {
                tSQLiteConnection = NWDDataManager.SharedInstance.SQLiteConnectionAccount;
            }


#if UNITY_EDITOR
            IEnumerable tEnumerable = tSQLiteConnection.Table<K>().OrderBy(x => x.InternalKey);
#else
            //TODO Modify request for release
			//IEnumerable tEnumerable = NWDDataManager.SharedInstance.SQLiteConnection.Table<K> ().Where (x => x.AC.Equals (bool.TrueString)).OrderBy(x => x.InternalKey);
			IEnumerable tEnumerable = tSQLiteConnection.Table<K> ().OrderBy(x => x.InternalKey);
            //TODO Add restriction of AccountReference is AccountReference Exist
#endif

            ObjectsList = new List<object>();
            ObjectsByReferenceList = new List<string>();
            ObjectsByKeyList = new List<string>();

#if UNITY_EDITOR
            ObjectsInEditorTableKeyList = new List<string>();
            ObjectsInEditorTableSelectionList = new List<bool>();
            ObjectsInEditorTableList = new List<string>();
#endif

            if (tEnumerable != null)
            {
                foreach (NWDBasis<K> tItem in tEnumerable)
                {
                    AddObjectInListOfEdition(tItem);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================