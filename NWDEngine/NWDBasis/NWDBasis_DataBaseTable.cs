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
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Class Methods
        //-------------------------------------------------------------------------------------------------------------
        public static void CopyTable(/*SQLiteConnection BundleSQLiteConnection*/)
        {

#if !UNITY_EDITOR
			 // nothing do to ... update bundle is not possible
#else
            if (AccountDependent() == false)
            {
                // reset sync timestamp
                SynchronizationResetTimestamp(NWDAppConfiguration.SharedInstance().DevEnvironment);
                SynchronizationResetTimestamp(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
                SynchronizationResetTimestamp(NWDAppConfiguration.SharedInstance().ProdEnvironment);
                // flush object in memory and drop table of document 
                //				ResetTable ();
                //				// load data from BundleSQLiteConnection
                //				IEnumerable tEnumerable = BundleSQLiteConnection.Table<K> ().OrderBy(x => x.InternalKey);
                //				if (tEnumerable != null) {
                //					foreach (NWDBasis<K> tItem in tEnumerable) {
                //						AddObjectInListOfEdition (tItem);
                //						NWDDataManager.SharedInstance().InsertObject (tItem);
                //					}
                //				}

            }
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void CleanTable()
        {
            List<object> tObjectsListToDelete = new List<object>();
            foreach (NWDBasis<K> tObject in Datas().Datas)
            {
                //if (tObject.XX > 0 && tObject.DevSync > 0 && tObject.PreprodSync > 0 && tObject.ProdSync > 0)
                if (tObject.XX > 0)
                {
                    tObjectsListToDelete.Add(tObject);
                }
            }
            foreach (NWDBasis<K> tObject in tObjectsListToDelete)
            {
                //RemoveObjectInListOfEdition(tObject);
#if UNITY_EDITOR
                if (IsObjectInEdition(tObject))
                {
                    SetObjectInEdition(null);
                }
                //NWDNodeEditor.ReAnalyzeIfNecessary(tObject);
#endif
                tObject.DeleteData();
            }

#if UNITY_EDITOR
            //NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
            RepaintTableEditor();
#endif
            // TODO : remove reference from all tables columns?
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void PurgeNotCurrentAccountDataFromTable()
        {
            PurgeTable();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void PurgeTable()
        {
            List<object> tObjectsListToDelete = new List<object>();
            // clean object not mine!
            foreach (NWDBasis<K> tObject in Datas().Datas)
            {
                //if (tObject.XX > 0 && tObject.DevSync > 0 && tObject.PreprodSync > 0 && tObject.ProdSync > 0)
                if (tObject.IsReacheableByAccount() == false)
                {
                    tObjectsListToDelete.Add(tObject);
                }
            }

            foreach (NWDBasis<K> tObject in tObjectsListToDelete)
            {
                //RemoveObjectInListOfEdition(tObject);
#if UNITY_EDITOR
                if (IsObjectInEdition(tObject))
                {
                    SetObjectInEdition(null);
                }
                //NWDNodeEditor.ReAnalyzeIfNecessary(tObject);
#endif
                tObject.DeleteData();
            }

#if UNITY_EDITOR
            //NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
            RepaintTableEditor();
#endif
            // TODO : remove reference from all tables columns?
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void UpdateDataTable()
        {
            NWDDataManager.SharedInstance().MigrateTable(ClassType(), AccountDependent());
            //List<object> tObjectsListToDelete = new List<object>();
            foreach (NWDBasis<K> tObject in Datas().Datas)
            {
                tObject.UpdateData();
            }
            // TODO : remove reference from all tables columns?
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void CreateTable()
        {
            NWDDataManager.SharedInstance().CreateTable(ClassType(), AccountDependent());
        }
        //-------------------------------------------------------------------------------------------------------------
        //        public static void ConnectToDatabase()
        //        {
        //            NWDDataManager.SharedInstance().ConnectToDatabase();
        //		}
        //-------------------------------------------------------------------------------------------------------------
        public static void ResetTable()
        {
            NWDDataManager.SharedInstance().ResetTable(ClassType(), AccountDependent());
// reload empty datas
            LoadFromDatabase();
            #if UNITY_EDITOR
            // refresh the tables windows
            RepaintTableEditor();
            #endif
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        //public static void PopulateTable(int sNumberOfRow)
        //{
        //    for (int tI = 0; tI < sNumberOfRow; tI++)
        //    {
        //        NewData();
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        public static void EmptyTable()
        {
            NWDDataManager.SharedInstance().EmptyTable(ClassType(), AccountDependent());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void DropTable()
        {
            NWDDataManager.SharedInstance().DropTable(ClassType(), AccountDependent());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ReInitializeTable()
        {
            NWDDataManager.SharedInstance().ReInitializeTable(ClassType(), AccountDependent());
        }
        //-------------------------------------------------------------------------------------------------------------
        protected static string GenerateNewSalt()
        {
            return NWDToolbox.RandomString(UnityEngine.Random.Range(12, 24));
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================