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
        protected bool InDatabase = false;
        protected bool FromDatabase = false;
        //-------------------------------------------------------------------------------------------------------------
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
        public override string ReferenceUsedValue()
        {
            return Reference;
        }
        [NWDNotEditable]
        public NWDVersionType MinVersion
        {
            get; set;
        }
        //[NWDNotEditable]
        //public NWDVersionType MaxVersion
        //{
        //    get; set;
        //}
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
        public NWDBasisTag Tag
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


        //public static string m_SearchReference = "";
        //public static string m_SearchInternalName = "";
        //public static string m_SearchInternalDescription = "";
        //public static NWDBasisTag m_SearchTag = NWDBasisTag.NoTag;
        //public static Vector2 m_ScrollPositionCard;
        //public static bool mSearchShowing = false;
        //-------------------------------------------------------------------------------------------------------------
        //public static List<string> ObjectsInEditorTableKeyList = new List<string>();
        //public static List<string> ObjectsInEditorTableList = new List<string>();
        //public static List<bool> ObjectsInEditorTableSelectionList = new List<bool>();
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
            foreach (string tObjectReference in Datas().ObjectsInEditorTableList)
            {
                int tIndex = Datas().ObjectsByReferenceList.IndexOf(tObjectReference);
                if (Datas().ObjectsInEditorTableSelectionList[tIndex] == true)
                {
                    // I test Integrity
                    NWDBasis<K> tObject = Datas().ObjectsList[tIndex] as NWDBasis<K>;
                    if (tObject.TestIntegrity() == false || tObject.XX > 0)
                    {
                        Datas().ObjectsInEditorTableSelectionList[tIndex] = false;
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SelectAllObjectInTableList()
        {
            Datas().ObjectsInEditorTableSelectionList.ForEach(delegate (bool tSelection)
            {
                tSelection = false;
            });
            foreach (string tObjectReference in Datas().ObjectsInEditorTableList)
            {
                //Debug.Log ("select ref " + tObjectReference);
                int tIndex = Datas().ObjectsByReferenceList.IndexOf(tObjectReference);
                Datas().ObjectsInEditorTableSelectionList[tIndex] = true;
            }
            IntegritySelection();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void DeselectAllObjectInTableList()
        {
            Datas().ObjectsInEditorTableSelectionList.ForEach(delegate (bool tSelection)
            {
                tSelection = false;
            });
            foreach (string tObjectReference in Datas().ObjectsInEditorTableList)
            {
                //Debug.Log ("select ref " + tObjectReference);
                int tIndex = Datas().ObjectsByReferenceList.IndexOf(tObjectReference);
                Datas().ObjectsInEditorTableSelectionList[tIndex] = false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void InverseSelectionOfAllObjectInTableList()
        {
            foreach (NWDBasis<K> tObject in Datas().ObjectsList)
            {
                if (Datas().ObjectsInEditorTableList.Contains(tObject.Reference))
                {
                    int tIndex = Datas().ObjectsByReferenceList.IndexOf(tObject.Reference);
                    Datas().ObjectsInEditorTableSelectionList[tIndex] = !Datas().ObjectsInEditorTableSelectionList[tIndex];
                }
                else
                {
                    int tIndex = Datas().ObjectsByReferenceList.IndexOf(tObject.Reference);
                    Datas().ObjectsInEditorTableSelectionList[tIndex] = false;
                }
            }
            IntegritySelection();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SelectAllObjectEnableInTableList()
        {
            foreach (NWDBasis<K> tObject in Datas().ObjectsList)
            {
                if (Datas().ObjectsInEditorTableList.Contains(tObject.Reference))
                {
                    int tIndex = Datas().ObjectsByReferenceList.IndexOf(tObject.Reference);
                    Datas().ObjectsInEditorTableSelectionList[tIndex] = tObject.AC;
                }
                else
                {
                    int tIndex = Datas().ObjectsByReferenceList.IndexOf(tObject.Reference);
                    Datas().ObjectsInEditorTableSelectionList[tIndex] = false;
                }
            }
            IntegritySelection();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SelectAllObjectDisableInTableList()
        {
            foreach (NWDBasis<K> tObject in Datas().ObjectsList)
            {
                if (Datas().ObjectsInEditorTableList.Contains(tObject.Reference))
                {
                    int tIndex = Datas().ObjectsByReferenceList.IndexOf(tObject.Reference);
                    Datas().ObjectsInEditorTableSelectionList[tIndex] = !tObject.AC;
                }
                else
                {
                    int tIndex = Datas().ObjectsByReferenceList.IndexOf(tObject.Reference);
                    Datas().ObjectsInEditorTableSelectionList[tIndex] = false;
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
                return tSQLiteConnection.Table<K>().OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
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
                                                                         ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
                            }
                            else
                            {
                                return tSQLiteConnection.Table<K>().Where(x =>
                                                                          x.Reference.Contains(sReference)
                                                                          && x.InternalKey.Contains(sInternalKey)
                                                                          && x.InternalDescription.Contains(sInternalDescription)
                                                                         ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
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
                                                                         ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
                            }
                            else
                            {
                                return tSQLiteConnection.Table<K>().Where(x => x.Reference.Contains(sReference)
                                                                          && x.InternalKey.Contains(sInternalKey)
                                                                         ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
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
                                                                         ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
                            }
                            else
                            {
                                return tSQLiteConnection.Table<K>().Where(x => x.Reference.Contains(sReference)
                                                                          && x.InternalDescription.Contains(sInternalDescription)
                                                                         ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
                            }
                        }
                        else
                        {
                            if ((int)sTag >= 0)
                            {
                                return tSQLiteConnection.Table<K>().Where(x => x.Reference.Contains(sReference)
                                                                          && x.Tag.Equals((int)sTag)
                                                                         ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
                            }
                            else
                            {
                                return tSQLiteConnection.Table<K>().Where(x => x.Reference.Contains(sReference)
                                                                         ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
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
                                                                         ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
                            }
                            else
                            {
                                return tSQLiteConnection.Table<K>().Where(x =>
                                                                           x.InternalKey.Contains(sInternalKey)
                                                                          && x.InternalDescription.Contains(sInternalDescription)
                                                                         ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
                            }
                        }
                        else
                        {
                            if ((int)sTag >= 0)
                            {
                                return tSQLiteConnection.Table<K>().Where(x =>
                                                                           x.InternalKey.Contains(sInternalKey)
                                                                          && x.Tag.Equals((int)sTag)
                                                                         ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
                            }
                            else
                            {
                                return tSQLiteConnection.Table<K>().Where(x => x.InternalKey.Contains(sInternalKey)
                                                                         ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
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
                                                                         ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
                            }
                            else
                            {
                                return tSQLiteConnection.Table<K>().Where(x => x.InternalDescription.Contains(sInternalDescription)
                                                                         ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
                            }
                        }
                        else
                        {
                            if ((int)sTag >= 0)
                            {
                                return tSQLiteConnection.Table<K>().Where(x => x.Tag.Equals((int)sTag)
                                                                         ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
                            }
                            else
                            {
                                return tSQLiteConnection.Table<K>().OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
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
            Datas().ObjectsInEditorTableSelectionList.ForEach(delegate (bool tSelection)
            {
                tSelection = false;
            });
            // change results
            Datas().ObjectsInEditorTableList = new List<string>();
            //IEnumerable tEnumerable = SelectForEditionObjects(m_SearchReference, m_SearchInternalName, m_SearchInternalDescription, m_SearchTag);

            IEnumerable tEnumerable = SelectForEditionObjects(Datas().m_SearchReference, Datas().m_SearchInternalName, Datas().m_SearchInternalDescription, Datas().m_SearchTag);
            if (tEnumerable != null)
            {
                foreach (NWDBasis<K> tItem in tEnumerable)
                {
                    bool tAdd = true;
                    if (tItem.TestIntegrity() == false && Datas().m_ShowIntegrityError == false)
                    {
                        tAdd = false;
                    }
                    if (tItem.AC == true && Datas().m_ShowEnable == false)
                    {
                        tAdd = false;
                    }
                    if (tItem.AC == false && Datas().m_ShowDisable == false)
                    {
                        tAdd = false;
                    }
                    if (tItem.XX > 0 && Datas().m_ShowTrashed == false)
                    {
                        tAdd = false;
                    }
                    if (tAdd == true)
                    {
                        Datas().ObjectsInEditorTableList.Add(tItem.Reference);
                    }
                }
            }
            foreach (NWDBasis<K> tObject in Datas().ObjectsList)
            {
                if (Datas().ObjectsInEditorTableList.Contains(tObject.Reference))
                {
                    // I keep the actual selection 
                }
                else
                {
                    int tIndex = Datas().ObjectsByReferenceList.IndexOf(tObject.Reference);
                    Datas().ObjectsInEditorTableSelectionList[tIndex] = false;
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
                if (Datas().ObjectsList.Contains(sObject) == false)
                {
                    // upgrade object between the old web service (add properties init, etc.)
                    //sObject.WebserviceVersionCheckMe(); // fait planter la mise à jupr de la table
                    // launch method specific on load object
                    sObject.AddonLoadedMe();
                    // add object in lists 
                    Datas().ObjectsList.Add(sObject);
                    Datas().ObjectsByReferenceList.Add(sObject.Reference);
                    Datas().ObjectsByKeyList.Add(sObject.InternalKey);
#if UNITY_EDITOR
                    sObject.ErrorCheck();

                    // add load object in editor table
                    if (Datas().ObjectsInEditorTableList.Contains(sObject.Reference) == false)
                    {
                        // Active to auto remove on filter
                        // if (sObject.Tag == (int)m_SearchTag)
                        {
                            Datas().ObjectsInEditorTableList.Add(sObject.Reference);
                        }
                    }
                    Datas().ObjectsInEditorTableKeyList.Add(sObject.InternalKey + " <" + sObject.Reference + ">");
                    Datas().ObjectsInEditorTableSelectionList.Add(false);
#endif
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RemoveObjectInListOfEdition(NWDBasis<K> sObject) // TO DO Rename … RemoveObjectFromListOfManagment
        {
            if (Datas().ObjectsList.Contains(sObject) == true)
            {
                sObject.AddonUnloadMe();
                //int tIndexInGame = InGameObjectsList.IndexOf(sObject);
                //InGameObjectsList.RemoveAt(tIndexInGame);
                //InGameObjectsByReference.RemoveAt(tIndexInGame);
                //InGameObjectsByKey.RemoveAt(tIndexInGame);

                int tIndex = Datas().ObjectsList.IndexOf(sObject);
                Datas().ObjectsList.RemoveAt(tIndex);
                Datas().ObjectsByReferenceList.RemoveAt(tIndex);
                Datas().ObjectsByKeyList.RemoveAt(tIndex);
#if UNITY_EDITOR
                int tIndexB = Datas().ObjectsInEditorTableList.IndexOf(sObject.Reference);
                if (tIndexB >= 0 && tIndexB < Datas().ObjectsInEditorTableList.Count())
                {
                    Datas().ObjectsInEditorTableList.RemoveAt(tIndexB);
                }
                Datas().ObjectsInEditorTableKeyList.RemoveAt(tIndex);
                Datas().ObjectsInEditorTableSelectionList.RemoveAt(tIndex);
#endif
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void UpdateObjectInListOfEdition(NWDBasis<K> sObject) // TO DO Rename … UpdateObjectInListOfManagment
        {
            if (Datas().ObjectsList.Contains(sObject) == true)
            {
                int tIndex = Datas().ObjectsList.IndexOf(sObject);
                Datas().ObjectsByReferenceList.RemoveAt(tIndex);
                Datas().ObjectsByReferenceList.Insert(tIndex, sObject.Reference);
                Datas().ObjectsByKeyList.RemoveAt(tIndex);
                Datas().ObjectsByKeyList.Insert(tIndex, sObject.InternalKey);
#if UNITY_EDITOR

                Datas().ObjectsInEditorTableKeyList.RemoveAt(tIndex);
                Datas().ObjectsInEditorTableSelectionList.RemoveAt(tIndex);

                Datas().ObjectsInEditorTableKeyList.Insert(tIndex, sObject.InternalKey + " <" + sObject.Reference + ">");
                Datas().ObjectsInEditorTableSelectionList.Insert(tIndex, false);

                int tIndexB = Datas().ObjectsInEditorTableList.IndexOf(sObject.Reference);
                if (tIndexB >= 0 && tIndexB < Datas().ObjectsInEditorTableList.Count())
                {
                    Datas().ObjectsInEditorTableList.RemoveAt(tIndexB);
                    // Active to auto remove on filter
                   // if (sObject.Tag == (int)m_SearchTag)
                    {
                        Datas().ObjectsInEditorTableList.Insert(tIndexB, sObject.Reference);
                    }
                }
                else
                {
                    if (sObject.Tag == Datas().m_SearchTag)
                    {
                        Datas().ObjectsInEditorTableList.Add(sObject.Reference);
                    }
                }
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
            IEnumerable tEnumerable = tSQLiteConnection.Table<K>().OrderBy(x => x.InternalKey); // Normal using of SQLITE
#else
            //TODO Modify request for release
			//IEnumerable tEnumerable = NWDDataManager.SharedInstance().SQLiteConnection.Table<K> ().Where (x => x.AC.Equals (bool.TrueString)).OrderBy(x => x.InternalKey);
			IEnumerable tEnumerable = tSQLiteConnection.Table<K> ().OrderBy(x => x.InternalKey);
            //TODO Add restriction of AccountReference is AccountReference Exist
#endif

            //InGameObjectsList = new List<object>();
            //InGameObjectsByReference = new List<string>();
            //InGameObjectsByKey = new List<string>();

            Datas().ObjectsList = new List<object>();
            Datas().ObjectsByReferenceList = new List<string>();
            Datas().ObjectsByKeyList = new List<string>();

#if UNITY_EDITOR

            Datas().ObjectsInEditorTableKeyList = new List<string>();
            Datas().ObjectsInEditorTableSelectionList = new List<bool>();
            Datas().ObjectsInEditorTableList = new List<string>();
#endif

            if (tEnumerable != null)
            {
                foreach (NWDBasis<K> tItem in tEnumerable)
                {
                    tItem.InDatabase = true;
                    tItem.LoadedFromDatabase();
                    AddObjectInListOfEdition(tItem);


                }
            }
            //Debug.Log("Load " + ObjectsList.Count + " object(s) in " + ClassName());
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================