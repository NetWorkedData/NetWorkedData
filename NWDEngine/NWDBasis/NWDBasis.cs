//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

// system lib
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

// unity3D lib
using UnityEngine;

// SQLite lib
using SQLite4Unity3d;

// Basic method by ideMobi
using BasicToolBox;

#if UNITY_EDITOR
// unity3D editor lib
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
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
        [Indexed("UpdateIndex", 0)]
        [Indexed("EditorIndex", 0)]
        [Indexed("InternalIndex", 0)]
        [NWDNotEditable]
        public string Reference
        {
            get; set;
        }
        [NWDNotEditable]
        public NWDVersionType MinVersion
        {
            get; set;
        }
        [Indexed("GetIndex", 0)]
        [Indexed("EditorIndex", 0)]
        [NWDNotEditable]
        public int WebServiceVersion
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
        [Indexed("EditorIndex", 0)]
        public bool AC
        {
            get; set;
        }
        [Indexed("EditorIndex", 0)]
        [NWDNotEditable]
        public int DC
        {
            get; set;
        }
        [Indexed("EditorIndex", 0)]
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
        [Indexed("GetIndex", 3)]
        [Indexed("UpdateIndex", 3)]
        public int DevSync
        {
            get; set;
        }
        [NWDNotEditable]
        [Indexed("GetIndex", 2)]
        [Indexed("UpdateIndex", 2)]
        public int PreprodSync
        {
            get; set;
        }
        [NWDNotEditable]
        [Indexed("GetIndex", 1)]
        [Indexed("UpdateIndex", 1)]
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
        [NWDNotEditable]
        public bool InError
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string m_SearchReference = "";
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
            NWDDataManager.SharedInstance().UpdateQueueExecute();
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
        public static void SelectAllObjectEnableInTableList()
        {
            foreach (NWDBasis<K> tObject in ObjectsList)
            {
                if (ObjectsInEditorTableList.Contains(tObject.Reference))
                {
                    int tIndex = ObjectsByReferenceList.IndexOf(tObject.Reference);
                    ObjectsInEditorTableSelectionList[tIndex] = tObject.AC;
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
        public static IEnumerable<K> SelectForEditionObjects(string sReference, string sInternalKey, string sInternalDescription, NWDBasisTag sTag)
        {
            SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
            if (AccountDependent())
            {
                tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
            }
            if (string.IsNullOrEmpty(sReference) && string.IsNullOrEmpty(sInternalKey) && string.IsNullOrEmpty(sInternalDescription) && (int)sTag < 0)
            {
                return tSQLiteConnection.Table<K>().OrderBy(x => x.InternalKey);
            }
            else
            {
                if (!string.IsNullOrEmpty(sReference))
                {
                    //Debug.Log("sReference = " + sReference);
                    if (!string.IsNullOrEmpty(sInternalKey))
                    {
                        if (!string.IsNullOrEmpty(sInternalDescription))
                        {
                            if ((int)sTag >= 0)
                            {
                                return tSQLiteConnection.Table<K>().Where(x =>
                                                                          x.Reference.Contains(sReference)
                                                                          && x.InternalKey.Contains(sInternalKey)
                                                                          && x.InternalDescription.Contains(sInternalDescription)
                                                                          && x.Tag.Equals((int)sTag)
                                                                         ).OrderBy(x => x.InternalKey);
                            }
                            else
                            {
                                return tSQLiteConnection.Table<K>().Where(x =>
                                                                          x.Reference.Contains(sReference)
                                                                          && x.InternalKey.Contains(sInternalKey)
                                                                          && x.InternalDescription.Contains(sInternalDescription)
                                                                         ).OrderBy(x => x.InternalKey);
                            }
                        }
                        else
                        {
                            if ((int)sTag >= 0)
                            {
                                return tSQLiteConnection.Table<K>().Where(x =>
                                                                          x.Reference.Contains(sReference)
                                                                          && x.InternalKey.Contains(sInternalKey)
                                                                          && x.Tag.Equals((int)sTag)
                                                                         ).OrderBy(x => x.InternalKey);
                            }
                            else
                            {
                                return tSQLiteConnection.Table<K>().Where(x => x.Reference.Contains(sReference)
                                                                          && x.InternalKey.Contains(sInternalKey)
                                                                         ).OrderBy(x => x.InternalKey);
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(sInternalDescription))
                        {
                            if ((int)sTag >= 0)
                            {
                                return tSQLiteConnection.Table<K>().Where(x => x.Reference.Contains(sReference)
                                                                          && x.InternalDescription.Contains(sInternalDescription)
                                                                          && x.Tag.Equals((int)sTag)
                                                                         ).OrderBy(x => x.InternalKey);
                            }
                            else
                            {
                                return tSQLiteConnection.Table<K>().Where(x => x.Reference.Contains(sReference)
                                                                          && x.InternalDescription.Contains(sInternalDescription)
                                                                         ).OrderBy(x => x.InternalKey);
                            }
                        }
                        else
                        {
                            if ((int)sTag >= 0)
                            {
                                return tSQLiteConnection.Table<K>().Where(x => x.Reference.Contains(sReference)
                                                                          && x.Tag.Equals((int)sTag)
                                                                         ).OrderBy(x => x.InternalKey);
                            }
                            else
                            {
                                return tSQLiteConnection.Table<K>().Where(x => x.Reference.Contains(sReference)
                                                                         ).OrderBy(x => x.InternalKey);
                            }
                        }
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(sInternalKey))
                    {
                        if (!string.IsNullOrEmpty(sInternalDescription))
                        {
                            if ((int)sTag >= 0)
                            {
                                return tSQLiteConnection.Table<K>().Where(x =>
                                                                           x.InternalKey.Contains(sInternalKey)
                                                                          && x.InternalDescription.Contains(sInternalDescription)
                                                                          && x.Tag.Equals((int)sTag)
                                                                         ).OrderBy(x => x.InternalKey);
                            }
                            else
                            {
                                return tSQLiteConnection.Table<K>().Where(x =>
                                                                           x.InternalKey.Contains(sInternalKey)
                                                                          && x.InternalDescription.Contains(sInternalDescription)
                                                                         ).OrderBy(x => x.InternalKey);
                            }
                        }
                        else
                        {
                            if ((int)sTag >= 0)
                            {
                                return tSQLiteConnection.Table<K>().Where(x =>
                                                                           x.InternalKey.Contains(sInternalKey)
                                                                          && x.Tag.Equals((int)sTag)
                                                                         ).OrderBy(x => x.InternalKey);
                            }
                            else
                            {
                                return tSQLiteConnection.Table<K>().Where(x => x.InternalKey.Contains(sInternalKey)
                                                                         ).OrderBy(x => x.InternalKey);
                            }
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(sInternalDescription))
                        {
                            if ((int)sTag >= 0)
                            {
                                return tSQLiteConnection.Table<K>().Where(x => x.InternalDescription.Contains(sInternalDescription)
                                                                          && x.Tag.Equals((int)sTag)
                                                                         ).OrderBy(x => x.InternalKey);
                            }
                            else
                            {
                                return tSQLiteConnection.Table<K>().Where(x => x.InternalDescription.Contains(sInternalDescription)
                                                                         ).OrderBy(x => x.InternalKey);
                            }
                        }
                        else
                        {
                            if ((int)sTag >= 0)
                            {
                                return tSQLiteConnection.Table<K>().Where(x => x.Tag.Equals((int)sTag)
                                                                         ).OrderBy(x => x.InternalKey);
                            }
                            else
                            {
                                return tSQLiteConnection.Table<K>().OrderBy(x => x.InternalKey);
                            }
                        }
                    }
                }
            }
            //return null;
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
            IEnumerable tEnumerable = SelectForEditionObjects(m_SearchReference, m_SearchInternalName, m_SearchInternalDescription, m_SearchTag);
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
            // test object validity (integer or corrupt)
            bool tObjectIsValid = sObject.TestIntegrity();
            //if (tObjectIsValid == true)
            //{
            //    if (ObjectsList.Contains(sObject) == false)
            //    {
            //        // launch method specific on load object
            //        sObject.AddonLoadedMe();
            //        if (sObject.IsTrashed() == false)
            //        {
            //            if (sObject.IsReacheableByAccount())
            //            {
            //                InGameObjectsList.Add(sObject);
            //                InGameObjectsByReference.Add(sObject.Reference);
            //                InGameObjectsByKey.Add(sObject.InternalKey);
            //            }
            //        }
            //    }
            //}


#if UNITY_EDITOR
            // override the insertion for unity editor
            tObjectIsValid = true;
#endif
            // if integrity is ok insert in ObjectsList
            if (tObjectIsValid == true)
            {
                if (ObjectsList.Contains(sObject) == false)
                {
                    // upgrade object between the old web service (add properties init, etc.)
                    //sObject.WebserviceVersionCheckMe(); // fait planter la mise à jupr de la table
                    // launch method specific on load object
                    sObject.AddonLoadedMe();
                    // add object in lists 
                    ObjectsList.Add(sObject);
                    ObjectsByReferenceList.Add(sObject.Reference);
                    ObjectsByKeyList.Add(sObject.InternalKey);
#if UNITY_EDITOR
                    sObject.ErrorCheck();
                    // add load object in editor table
                    ObjectsInEditorTableKeyList.Add(sObject.InternalKey + " <" + sObject.Reference + ">");
                    ObjectsInEditorTableList.Add(sObject.Reference);
                    ObjectsInEditorTableSelectionList.Add(false);
#endif
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RemoveObjectInListOfEdition(NWDBasis<K> sObject) // TO DO Rename … RemoveObjectFromListOfManagment
        {
            if (ObjectsList.Contains(sObject) == true)
            {
                sObject.AddonUnloadMe();
                //int tIndexInGame = InGameObjectsList.IndexOf(sObject);
                //InGameObjectsList.RemoveAt(tIndexInGame);
                //InGameObjectsByReference.RemoveAt(tIndexInGame);
                //InGameObjectsByKey.RemoveAt(tIndexInGame);

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

                //int tIndexInGame = InGameObjectsList.IndexOf(sObject);
                //// InGameObjectsList doesn't change
                //InGameObjectsList.RemoveAt(tIndexInGame);
                //InGameObjectsList.Insert(tIndexInGame, sObject.Reference);
                //InGameObjectsByKey.RemoveAt(tIndexInGame);
                //InGameObjectsByKey.Insert(tIndexInGame, sObject.InternalKey);


                int tIndex = ObjectsList.IndexOf(sObject);
                // ObjectsList doesn't change
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

            SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
            if (AccountDependent())
            {
                tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
            }


#if UNITY_EDITOR
            IEnumerable tEnumerable = tSQLiteConnection.Table<K>().OrderBy(x => x.InternalKey);
#else
            //TODO Modify request for release
			//IEnumerable tEnumerable = NWDDataManager.SharedInstance().SQLiteConnection.Table<K> ().Where (x => x.AC.Equals (bool.TrueString)).OrderBy(x => x.InternalKey);
			IEnumerable tEnumerable = tSQLiteConnection.Table<K> ().OrderBy(x => x.InternalKey);
            //TODO Add restriction of AccountReference is AccountReference Exist
#endif

            //InGameObjectsList = new List<object>();
            //InGameObjectsByReference = new List<string>();
            //InGameObjectsByKey = new List<string>();

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
            Debug.Log("Load " + ObjectsList.Count + " object(s) in " + ClassName());
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================