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

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    public partial class NWDBasis : NWDTypeClass
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Class Methods
        //-------------------------------------------------------------------------------------------------------------
//        public static void CopyTable(/*SQLiteConnection BundleSQLiteConnection*/)
//        {

//#if !UNITY_EDITOR
//			 // nothing do to ... update bundle is not possible
//#else
//            if (AccountDependent() == false)
//            {
//                // reset sync timestamp
//                SynchronizationResetTimestamp(NWDAppConfiguration.SharedInstance().DevEnvironment);
//                SynchronizationResetTimestamp(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
//                SynchronizationResetTimestamp(NWDAppConfiguration.SharedInstance().ProdEnvironment);
//                // flush object in memory and drop table of document 
//                //				ResetTable ();
//                //				// load data from BundleSQLiteConnection
//                //				IEnumerable tEnumerable = BundleSQLiteConnection.Table<K> ().OrderBy(x => x.InternalKey);
//                //				if (tEnumerable != null) {
//                //					foreach (NWDBasis tItem in tEnumerable) {
//                //						AddObjectInListOfEdition (tItem);
//                //						NWDDataManager.SharedInstance().InsertObject (tItem);
//                //					}
//                //				}

//            }
//#endif
        //}
        //-------------------------------------------------------------------------------------------------------------
//        [NWDAliasMethod(NWDConstants.M_CleanTable)]
//        public static void CleanTable()
//        {
//            List<object> tObjectsListToDelete = new List<object>();
//            foreach (NWDBasis tObject in BasisHelper().Datas)
//            {
//                //if (tObject.XX > 0 && tObject.DevSync > 0 && tObject.PreprodSync > 0 && tObject.ProdSync > 0)
//                if (tObject.XX > 0)
//                {
//                    tObjectsListToDelete.Add(tObject);
//                }
//            }
//            foreach (NWDBasis tObject in tObjectsListToDelete)
//            {
//                //RemoveObjectInListOfEdition(tObject);
//#if UNITY_EDITOR
//                if (BasisHelper().New_IsObjectInEdition(tObject))
//                {
//                    BasisHelper().New_SetObjectInEdition(null);
//                }
//                //NWDNodeEditor.ReAnalyzeIfNecessary(tObject);
//#endif
//                tObject.DeleteData();
//            }

//#if UNITY_EDITOR
//            //NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
//            BasisHelper().New_RepaintTableEditor();
//#endif
        //    // TODO : remove reference from all tables columns?
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static void PurgeNotCurrentAccountDataFromTable()
        //{
        //    PurgeTable();
        //}
        //-------------------------------------------------------------------------------------------------------------
//        [NWDAliasMethod(NWDConstants.M_PurgeTable)]
//        public static void PurgeTable()
//        {
//            List<object> tObjectsListToDelete = new List<object>();
//            // clean object not mine!
//            foreach (NWDBasis tObject in BasisHelper().Datas)
//            {
//                //if (tObject.XX > 0 && tObject.DevSync > 0 && tObject.PreprodSync > 0 && tObject.ProdSync > 0)
//                if (tObject.IsReacheableByAccount() == false)
//                {
//                    tObjectsListToDelete.Add(tObject);
//                }
//            }

//            foreach (NWDBasis tObject in tObjectsListToDelete)
//            {
//                //RemoveObjectInListOfEdition(tObject);
//#if UNITY_EDITOR
//                if (BasisHelper().New_IsObjectInEdition(tObject))
//                {
//                    BasisHelper().New_SetObjectInEdition(null);
//                }
//                //NWDNodeEditor.ReAnalyzeIfNecessary(tObject);
//#endif
//                tObject.DeleteData();
//            }

//#if UNITY_EDITOR
//            //NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
//            BasisHelper().New_RepaintTableEditor();
//#endif
        //    // TODO : remove reference from all tables columns?
        //}
        //-------------------------------------------------------------------------------------------------------------
        //[NWDAliasMethod(NWDConstants.M_UpdateDataTable)]
        //public static void UpdateDataTable()
        //{
        //    NWDDataManager.SharedInstance().MigrateTable(ClassType(), AccountDependent());
        //    //List<object> tObjectsListToDelete = new List<object>();
        //    foreach (NWDBasis tObject in BasisHelper().Datas)
        //    {
        //        tObject.UpdateData();
        //    }
        //    // TODO : remove reference from all tables columns?
        //}
        //-------------------------------------------------------------------------------------------------------------
        //[NWDAliasMethod(NWDConstants.M_CreateTable)]
        //public static void CreateTable()
        //{
        //    //Debug.Log("<color=orange>CreateTable() "+ ClassType() + " </color>");
        //    NWDDataManager.SharedInstance().CreateTable(ClassType(), AccountDependent());
        //}
        //-------------------------------------------------------------------------------------------------------------
        //        public static void ConnectToDatabase()
        //        {
        //            NWDDataManager.SharedInstance().ConnectToDatabase();
        //		}
        //-------------------------------------------------------------------------------------------------------------
//        [NWDAliasMethod(NWDConstants.M_ResetTable)]
//        public static void ResetTable()
//        {
//            NWDDataManager.SharedInstance().ResetTable(ClassType(), AccountDependent());
//// reload empty datas
//            LoadFromDatabase();
//#if UNITY_EDITOR
        //    // refresh the tables windows
        //    BasisHelper().New_RepaintTableEditor();
        //    #endif
        //}
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
        //public static void EmptyTable()
        //{
        //    NWDDataManager.SharedInstance().EmptyTable(ClassType(), AccountDependent());
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static void DropTable()
        //{
        //    NWDDataManager.SharedInstance().DropTable(ClassType(), AccountDependent());
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static void ReInitializeTable()
        //{
        //    NWDDataManager.SharedInstance().ReInitializeTable(ClassType(), AccountDependent());
        //}
        //-------------------------------------------------------------------------------------------------------------
        //protected static string GenerateNewSalt()
        //{
        //    return NWDToolbox.RandomString(UnityEngine.Random.Range(12, 24));
        //}
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================
