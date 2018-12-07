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
using SQLite.Attribute;

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
        //[Indexed("EditorIndex", 0)]
        //[Indexed("InternalIndex", 0)]
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
        //[Indexed("EditorIndex", 0)]
        [NWDNotEditable]
        public int WebServiceVersion
        {
            get; set;
        }
        [Indexed("InternalIndex", 0)]
        //[Indexed("EditorIndex", 0)]
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
        //[Indexed("UpdateIndex", 3)]
        public int DevSync
        {
            get; set;
        }
        [NWDNotEditable]
        [Indexed("GetIndex", 2)]
        //[Indexed("UpdateIndex", 2)]
        public int PreprodSync
        {
            get; set;
        }
        [NWDNotEditable]
        [Indexed("GetIndex", 1)]
        //[Indexed("UpdateIndex", 1)]
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
        static NWDBasis()
        {
            Debug.Log("NWDBasis Static Class Constructor()");
            NWDTypeLauncher.Launcher();
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
#endif
        //-------------------------------------------------------------------------------------------------------------
        //        public static void AddObjectInListOfEdition(NWDBasis<K> sObject) // TO DO Rename … AddObjectInListOfManagment
        //        {

        //            Datas().AddData(sObject);
        //            // test object validity (integer or corrupt)
        //            bool tObjectIsValid = sObject.TestIntegrity();
        //            //if (tObjectIsValid == true)
        //            //{
        //            //    if (ObjectsList.Contains(sObject) == false)
        //            //    {
        //            //        // launch method specific on load object
        //            //        sObject.AddonLoadedMe();
        //            //        if (sObject.IsTrashed() == false)
        //            //        {
        //            //            if (sObject.IsReacheableByAccount())
        //            //            {
        //            //                InGameObjectsList.Add(sObject);
        //            //                InGameObjectsByReference.Add(sObject.Reference);
        //            //                InGameObjectsByKey.Add(sObject.InternalKey);
        //            //            }
        //            //        }
        //            //    }
        //            //}


        //#if UNITY_EDITOR
        //            // override the insertion for unity editor
        //            tObjectIsValid = true;
        //#endif
        //            // if integrity is ok insert in ObjectsList
        //            if (tObjectIsValid == true)
        //            {
        //                if (Datas().ObjectsList.Contains(sObject) == false)
        //                {
        //                    // upgrade object between the old web service (add properties init, etc.)
        //                    //sObject.WebserviceVersionCheckMe(); // fait planter la mise à jupr de la table
        //                    // launch method specific on load object
        //                    sObject.AddonLoadedMe();
        //                    // add object in lists 
        //                    Datas().ObjectsList.Add(sObject);
        //                    Datas().ObjectsByReferenceList.Add(sObject.Reference);
        //                    Datas().ObjectsByKeyList.Add(sObject.InternalKey);
        //#if UNITY_EDITOR
        //                    sObject.ErrorCheck();

        //                    // add load object in editor table
        //                    if (Datas().DatasInEditorReferenceList.Contains(sObject.Reference) == false)
        //                    {
        //                        // Active to auto remove on filter
        //                        // if (sObject.Tag == (int)m_SearchTag)
        //                        {
        //                            Datas().DatasInEditorReferenceList.Add(sObject.Reference);
        //                        }
        //                    }
        //                    Datas().DatasInEditorRowDescriptionList.Add(sObject.InternalKey + " <" + sObject.Reference + ">");
        //                    Datas().DatasInEditorSelectionList.Add(false);
        //#endif
        //                }
        //            }
        //        }
        //        //-------------------------------------------------------------------------------------------------------------
        //        public static void RemoveObjectInListOfEdition(NWDBasis<K> sObject) // TO DO Rename … RemoveObjectFromListOfManagment
        //        {
        //            if (Datas().ObjectsList.Contains(sObject) == true)
        //            {
        //                sObject.AddonUnloadMe();
        //                //int tIndexInGame = InGameObjectsList.IndexOf(sObject);
        //                //InGameObjectsList.RemoveAt(tIndexInGame);
        //                //InGameObjectsByReference.RemoveAt(tIndexInGame);
        //                //InGameObjectsByKey.RemoveAt(tIndexInGame);

        //                int tIndex = Datas().ObjectsList.IndexOf(sObject);
        //                Datas().ObjectsList.RemoveAt(tIndex);
        //                Datas().ObjectsByReferenceList.RemoveAt(tIndex);
        //                Datas().ObjectsByKeyList.RemoveAt(tIndex);
        //#if UNITY_EDITOR
        //                int tIndexB = Datas().DatasInEditorReferenceList.IndexOf(sObject.Reference);
        //                if (tIndexB >= 0 && tIndexB < Datas().DatasInEditorReferenceList.Count())
        //                {
        //                    Datas().DatasInEditorReferenceList.RemoveAt(tIndexB);
        //                }
        //                Datas().DatasInEditorRowDescriptionList.RemoveAt(tIndex);
        //                Datas().DatasInEditorSelectionList.RemoveAt(tIndex);
        //#endif
        //            }
        //        }
        //        //-------------------------------------------------------------------------------------------------------------
        //        public static void UpdateObjectInListOfEdition(NWDBasis<K> sObject) // TO DO Rename … UpdateObjectInListOfManagment
        //        {

        //            Datas().UpdateData(sObject);

        //            if (Datas().ObjectsList.Contains(sObject) == true)
        //            {
        //                int tIndex = Datas().ObjectsList.IndexOf(sObject);
        //                Datas().ObjectsByReferenceList.RemoveAt(tIndex);
        //                Datas().ObjectsByReferenceList.Insert(tIndex, sObject.Reference);
        //                Datas().ObjectsByKeyList.RemoveAt(tIndex);
        //                Datas().ObjectsByKeyList.Insert(tIndex, sObject.InternalKey);
        //#if UNITY_EDITOR

        //                Datas().DatasInEditorRowDescriptionList.RemoveAt(tIndex);
        //                Datas().DatasInEditorSelectionList.RemoveAt(tIndex);

        //                Datas().DatasInEditorRowDescriptionList.Insert(tIndex, sObject.InternalKey + " <" + sObject.Reference + ">");
        //                Datas().DatasInEditorSelectionList.Insert(tIndex, false);

        //                int tIndexB = Datas().DatasInEditorReferenceList.IndexOf(sObject.Reference);
        //                if (tIndexB >= 0 && tIndexB < Datas().DatasInEditorReferenceList.Count())
        //                {
        //                    Datas().DatasInEditorReferenceList.RemoveAt(tIndexB);
        //                    // Active to auto remove on filter
        //                   // if (sObject.Tag == (int)m_SearchTag)
        //                    {
        //                        Datas().DatasInEditorReferenceList.Insert(tIndexB, sObject.Reference);
        //                    }
        //                }
        //                else
        //                {
        //                    if (sObject.Tag == Datas().m_SearchTag)
        //                    {
        //                        Datas().DatasInEditorReferenceList.Add(sObject.Reference);
        //                    }
        //                }
        //#endif
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        //        public static void LoadTableEditor()
        //        {
        //            //Debug.Log ("LoadTableEditor ##########");
        //            LoadFromDatabase();

        //            SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
        //            if (AccountDependent())
        //            {
        //                tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
        //            }


        //#if UNITY_EDITOR
        //            IEnumerable tEnumerable = tSQLiteConnection.Table<K>().OrderBy(x => x.InternalKey); // Normal using of SQLITE
        //#else
        //            //TODO Modify request for release
        //			//IEnumerable tEnumerable = NWDDataManager.SharedInstance().SQLiteConnection.Table<K> ().Where (x => x.AC.Equals (bool.TrueString)).OrderBy(x => x.InternalKey);
        //			IEnumerable tEnumerable = tSQLiteConnection.Table<K> ().OrderBy(x => x.InternalKey);
        //            //TODO Add restriction of AccountReference is AccountReference Exist
        //#endif

        //            //InGameObjectsList = new List<object>();
        //            //InGameObjectsByReference = new List<string>();
        //            //InGameObjectsByKey = new List<string>();

        //            Datas().ObjectsList = new List<object>();
        //            Datas().ObjectsByReferenceList = new List<string>();
        //            Datas().ObjectsByKeyList = new List<string>();

        //#if UNITY_EDITOR

        //            Datas().DatasInEditorRowDescriptionList = new List<string>();
        //            Datas().DatasInEditorSelectionList = new List<bool>();
        //            Datas().DatasInEditorReferenceList = new List<string>();
        //#endif

        //    if (tEnumerable != null)
        //    {
        //        foreach (NWDBasis<K> tItem in tEnumerable)
        //        {
        //            tItem.InDatabase = true;
        //            tItem.LoadedFromDatabase();
        //            //AddObjectInListOfEdition(tItem);


        //        }
        //    }
        //    //Debug.Log("Load " + ObjectsList.Count + " object(s) in " + ClassName());
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================