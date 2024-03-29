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

using NetWorkedData.NWDORM;
using System;
using System.Collections.Generic;
using System.Threading;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif
//=====================================================================================================================
#if UNITY_EDITOR
using Sqlite = NetWorkedData.Logged.SQLite3; // Have a logged interface for SQLite (Editor only !)
#else
using Sqlite = NetWorkedData.SQLite3;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDataManager
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The List of Data to Insert by queue in main thread
        /// </summary>
        public List<NWDTypeClass> kInsertDataQueueMain = new List<NWDTypeClass>();
        /// <summary>
        /// The List of Data to Update by queue in main thread
        /// </summary>
        public List<NWDTypeClass> kUpdateDataQueueMain = new List<NWDTypeClass>();
        /// <summary>
        /// The List of Data to Delete by queue in main thread
        /// </summary>
        public List<NWDTypeClass> kDeleteDataQueueMain = new List<NWDTypeClass>();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The List of Data to Insert by queue in pool thread
        /// </summary>
        public List<NWDTypeClass> kInsertDataQueuePool = new List<NWDTypeClass>();
        /// <summary>
        /// The List of Data to Update by queue in main thread
        /// </summary>
        public List<NWDTypeClass> kUpdateDataQueuePool = new List<NWDTypeClass>();
        /// <summary>
        /// The List of Data to Delete by queue in main thread
        /// </summary>
        public List<NWDTypeClass> kDeleteDataQueuePool = new List<NWDTypeClass>();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The List of Data in pending to write
        /// </summary>
        public List<NWDTypeClass> kDataInWriting = new List<NWDTypeClass>();
        //-------------------------------------------------------------------------------------------------------------
        #region Queue Data
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Datas in writing process.
        /// </summary>
        /// <returns><c>true</c>, if in writing process was dataed, <c>false</c> otherwise.</returns>
        public bool DataInWritingProcess()
        {
            //NWDBenchmark.Start();
            bool rReturn = false;
            if (kDataInWriting.Count > 0)
            {
                rReturn = true;
            }
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Execute Queues
        /// </summary>
        public int DataQueueCounter()
        {
            //NWDBenchmark.Start();
            int rResult = kDataInWriting.Count;
            //NWDBenchmark.Finish();
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Execute Queues
        /// </summary>
        public void DataQueueExecute()
        {
            //NWDBenchmark.Start();
            if (DataInWritingProcess())
            {
                DataQueueMainExecute();
                DataQueuePoolExecute();
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Execute the Queue on main thread
        /// </summary>
        public void DataQueueMainExecute()
        {
            //NWDBenchmark.Start();
            if (DataInWritingProcess())
            {
                List<NWDTypeClass> tInsertDataQueueMain = new List<NWDTypeClass>(kInsertDataQueueMain);
                List<NWDTypeClass> tUpdateDataQueueMain = new List<NWDTypeClass>(kUpdateDataQueueMain);
                List<NWDTypeClass> tDeleteDataQueueMain = new List<NWDTypeClass>(kDeleteDataQueueMain);
                kInsertDataQueueMain = new List<NWDTypeClass>();
                kUpdateDataQueueMain = new List<NWDTypeClass>();
                kDeleteDataQueueMain = new List<NWDTypeClass>();
                InsertDataQueueExecute(tInsertDataQueueMain);
                UpdateDataQueueExecute(tUpdateDataQueueMain);
                DeleteDataQueueExecute(tDeleteDataQueueMain);
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Execute the Queue on pool thread
        /// </summary>
        public void DataQueuePoolExecute()
        {
            //NWDBenchmark.Start();
            if (DataInWritingProcess())
            {
                List<NWDTypeClass> tInsertDataQueuePool = new List<NWDTypeClass>(kInsertDataQueuePool);
                List<NWDTypeClass> tUpdateDataQueuePool = new List<NWDTypeClass>(kUpdateDataQueuePool);
                List<NWDTypeClass> tDeleteDataQueuePool = new List<NWDTypeClass>(kDeleteDataQueuePool);
                kInsertDataQueuePool = new List<NWDTypeClass>();
                kUpdateDataQueuePool = new List<NWDTypeClass>();
                kDeleteDataQueuePool = new List<NWDTypeClass>();
                ThreadPool.QueueUserWorkItem(DataQueuePoolThread, new object[] {
                tInsertDataQueuePool,
                tUpdateDataQueuePool,
                tDeleteDataQueuePool
            });
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Execute the Queue on pool thread by ThreadPool.QueueUserWorkItem()
        /// </summary>
        private void DataQueuePoolThread(object sState)
        {
            //NWDBenchmark.Start();
            object[] tParam = sState as object[];
            List<NWDTypeClass> tInsertDataQueue = tParam[0] as List<NWDTypeClass>;
            List<NWDTypeClass> tUpdateDataQueue = tParam[1] as List<NWDTypeClass>;
            List<NWDTypeClass> tDeleteDataQueue = tParam[2] as List<NWDTypeClass>;
            InsertDataQueueExecute(tInsertDataQueue);
            UpdateDataQueueExecute(tUpdateDataQueue);
            DeleteDataQueueExecute(tDeleteDataQueue);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion Queue Data
        #region Insert Data
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Insert data in database
        /// </summary>
        /// <param name="sObject">S object.</param>
        /// <param name="sWritingMode">S writing mode.</param>
        public void InsertData(NWDTypeClass sObject, NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            //NWDDebug.Log("NWDDataManager InsertData()");
            //Determine the default mode
            sWritingMode = NWDAppConfiguration.WritingMode(sWritingMode);
            //NWDBenchmark.Start();
            switch (sWritingMode)
            {
                case NWDWritingMode.MainThread:
                    {
                        //Debug.Log("NWDDataManager InsertData() MainThread");
                        InsertDataExecute(sObject);
                    }
                    break;
                case NWDWritingMode.PoolThread:
                    {
                        //Debug.Log("NWDDataManager InsertData() PoolThread");
#if UNITY_EDITOR
                        RepaintWindowsInManager(sObject.GetType());
#endif
                        ThreadPool.QueueUserWorkItem(InsertDataExecute, sObject);
                    }
                    break;
                case NWDWritingMode.QueuedMainThread:
                    {
                        //Debug.Log("NWDDataManager InsertData() QueuedMainThread");
                        if (kInsertDataQueueMain.Contains(sObject) == false)
                        {
#if UNITY_EDITOR
                            RepaintWindowsInManager(sObject.GetType());
#endif
                            kInsertDataQueueMain.Add(sObject);
                        }
                    }
                    break;
                case NWDWritingMode.QueuedPoolThread:
                    {
                        //Debug.Log("NWDDataManager InsertData() QueuedPoolThread");
                        if (kInsertDataQueuePool.Contains(sObject) == false)
                        {
#if UNITY_EDITOR
                            RepaintWindowsInManager(sObject.GetType());
#endif
                            kInsertDataQueuePool.Add(sObject);
                        }
                    }
                    break;
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Insert data in database on pool thread.
        /// </summary>
        /// <param name="sObject">S object.</param>
        private void InsertDataExecute(object sObject)
        {
            //NWDBenchmark.Start();
            NWDTypeClass tObject = sObject as NWDTypeClass;
            tObject.InsertDataProceedWithTransaction();
            tObject.InsertDataFinish();
            InsertDataCompleted(tObject);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Insert data queue pool execute.
        /// </summary>
        /// <param name="sInsertDataQueuePool">S insert data queue pool.</param>
        private void InsertDataQueueExecute(List<NWDTypeClass> sInsertDataQueuePool)
        {
            if (NWDDataManager.SharedInstance().SQLiteConnectionAccountIsValid() && NWDDataManager.SharedInstance().SQLiteConnectionEditorIsValid())
            {
                //NWDBenchmark.Start();
                //NWDDebug.Log("InsertDataQueueExecute with " + sInsertDataQueuePool.Count + " Object(s)");
                List<Type> tTypeList = new List<Type>();
                if (sInsertDataQueuePool.Count > 0)
                {
                    {
                        using ITransaction tDeviceTransaction = NWDDataManager.SharedInstance().DeviceFactory.CreateTransaction();
                        tDeviceTransaction.Begin();
                        using ITransaction tEditorTransaction = NWDDataManager.SharedInstance().EditorFactory.CreateTransaction();
                        tEditorTransaction.Begin();

                        foreach (NWDTypeClass tObject in sInsertDataQueuePool)
                        {
                            tObject.InsertDataProceed(tDeviceTransaction, tEditorTransaction);
                        }

                        tEditorTransaction.Commit();
                        tDeviceTransaction.Commit();
                    }

                    foreach (NWDTypeClass tObject in sInsertDataQueuePool)
                    {
                        Type tType = tObject.GetType();
                        if (tTypeList.Contains(tType) == false)
                        {
                            tTypeList.Add(tType);
                        }
                        tObject.InsertDataFinish();
                    }
                    InsertDataQueueCompleted(tTypeList);
                }
                sInsertDataQueuePool = null;
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void InsertDataCompleted(NWDTypeClass sObject)
        {
            //NWDBenchmark.Start();
            NWDDataManagerMainThread.AddInsertType(sObject.GetType());
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void InsertDataQueueCompleted(List<Type> sTypeList)
        {
            //NWDBenchmark.Start();
            NWDDataManagerMainThread.AddInsertTypeList(sTypeList);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion Insert Data
        #region Update Data
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Update data in database.
        /// </summary>
        /// <param name="sObject">S object.</param>
        /// <param name="sWritingMode">S writing mode.</param>
        public void UpdateData(NWDTypeClass sObject, NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            //Determine the default mode
            sWritingMode = NWDAppConfiguration.WritingMode(sWritingMode);
            //NWDBenchmark.Start();
            switch (sWritingMode)
            {
                case NWDWritingMode.MainThread:
                    {
                        //NWDDebug.Log("NWDDataManager UpdateData() MainThread");
                        UpdateDataExecute(sObject);
#if NWD_CRUD_NOTIFICATION
                        NWENotificationManager.SharedInstance().PostNotification(sObject, NWDNotificationConstants.K_DATA_LOCAL_UPDATE);
#endif
                    }
                    break;
                case NWDWritingMode.PoolThread:
                    {
                        //NWDDebug.Log("NWDDataManager UpdateData() PoolThread");
#if UNITY_EDITOR
                        RepaintWindowsInManager(sObject.GetType());
#endif
                        ThreadPool.QueueUserWorkItem(UpdateDataExecute, sObject);

                    }
                    break;
                case NWDWritingMode.QueuedMainThread:
                    {
                        //NWDDebug.Log("NWDDataManager UpdateData() QueuedMainThread");
                        if (kUpdateDataQueueMain.Contains(sObject) == false)
                        {
#if UNITY_EDITOR
                            RepaintWindowsInManager(sObject.GetType());
#endif
                            kUpdateDataQueueMain.Add(sObject);
                        }
                    }
                    break;
                case NWDWritingMode.QueuedPoolThread:
                    {
                        //NWDDebug.Log("NWDDataManager UpdateData() QueuedPoolThread");
                        if (kUpdateDataQueuePool.Contains(sObject) == false)
                        {
#if UNITY_EDITOR
                            RepaintWindowsInManager(sObject.GetType());
#endif
                            kUpdateDataQueuePool.Add(sObject);
                        }
                    }
                    break;
            }

#if UNITY_EDITOR
            // no auto update data
            if (sObject != null)
            {
                sObject.ErrorCheck();
            }
#endif
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Update data in database on pool thread.
        /// </summary>
        /// <param name="sObject">S object.</param>
        private void UpdateDataExecute(object sObject)
        {
            //NWDBenchmark.Start();
            NWDTypeClass tObject = sObject as NWDTypeClass;
            tObject.UpdateDataProceedWithTransaction();
            tObject.UpdateDataFinish();
            UpdateDataCompleted(tObject);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Update data queue pool execute.
        /// </summary>
        /// <param name="sUpdateDataQueuePool">S update data queue pool.</param>
        private void UpdateDataQueueExecute(List<NWDTypeClass> sUpdateDataQueuePool)
        {
            //NWDBenchmark.Start();
            //NWDDebug.Log("UpdateDataQueueExecute with " + sUpdateDataQueuePool.Count + " Object(s)");
            if (NWDDataManager.SharedInstance().SQLiteConnectionAccountIsValid() && NWDDataManager.SharedInstance().SQLiteConnectionEditorIsValid())
            {
                List<Type> tTypeList = new List<Type>();
                if (sUpdateDataQueuePool.Count > 0)
                {
                    {
                        using ITransaction tDeviceTransaction = NWDDataManager.SharedInstance().DeviceFactory.CreateTransaction();
                        tDeviceTransaction.Begin();
                        using ITransaction tEditorTransaction = NWDDataManager.SharedInstance().EditorFactory.CreateTransaction();
                        tEditorTransaction.Begin();

                        foreach (NWDTypeClass tObject in sUpdateDataQueuePool)
                        {
                            tObject.UpdateDataProceed(tDeviceTransaction, tEditorTransaction);
                        }

                        tEditorTransaction.Commit();
                        tDeviceTransaction.Commit();
                    }

                    foreach (NWDTypeClass tObject in sUpdateDataQueuePool)
                    {
                        Type tType = tObject.GetType();
                        if (tTypeList.Contains(tType) == false)
                        {
                            tTypeList.Add(tType);
                        }
                        tObject.UpdateDataFinish();
                    }
                    UpdateDataQueueCompleted(tTypeList);
                }
                sUpdateDataQueuePool = null;
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void UpdateDataCompleted(NWDTypeClass sObject)
        {
            //NWDBenchmark.Start();
            NWDDataManagerMainThread.AddUpdateType(sObject.GetType());
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void UpdateDataQueueCompleted(List<Type> sTypeList)
        {
            //NWDBenchmark.Start();
            NWDDataManagerMainThread.AddUpdateTypeList(sTypeList);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
#endregion Update Data
#region Delete Data
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Delete data in database.
        /// </summary>
        /// <param name="sObject">S object.</param>
        /// <param name="sWritingMode">S writing mode.</param>
        public void DeleteData(NWDTypeClass sObject, NWDWritingMode sWritingMode = NWDWritingMode.MainThread)
        {
            // Determine the default mode
            sWritingMode = NWDAppConfiguration.WritingMode(sWritingMode);
            //NWDBenchmark.Start();
            switch (sWritingMode)
            {
                case NWDWritingMode.MainThread:
                    {
                        //NWDDebug.Log("NWDDataManager DeleteData() MainThread");
                        DeleteDataExecute(sObject);
#if NWD_CRUD_NOTIFICATION
                        NWENotificationManager.SharedInstance().PostNotification(sObject, NWDNotificationConstants.K_DATA_LOCAL_DELETE);
#endif
                    }
                    break;
                case NWDWritingMode.PoolThread:
                    {
                        //NWDDebug.Log("NWDDataManager DeleteData() PoolThread");
#if UNITY_EDITOR
                        RepaintWindowsInManager(sObject.GetType());
#endif
                        ThreadPool.QueueUserWorkItem(DeleteDataExecute, sObject);
                    }
                    break;
                case NWDWritingMode.QueuedMainThread:
                    {
                        //NWDDebug.Log("NWDDataManager DeleteData() QueuedMainThread");
                        if (kDeleteDataQueueMain.Contains(sObject) == false)
                        {
#if UNITY_EDITOR
                            RepaintWindowsInManager(sObject.GetType());
#endif
                            kDeleteDataQueueMain.Add(sObject);
                        }
                    }
                    break;
                case NWDWritingMode.QueuedPoolThread:
                    {
                        //NWDDebug.Log("NWDDataManager DeleteData() QueuedPoolThread");
                        if (kDeleteDataQueuePool.Contains(sObject) == false)
                        {
#if UNITY_EDITOR
                            RepaintWindowsInManager(sObject.GetType());
#endif
                            kDeleteDataQueuePool.Add(sObject);
                        }
                    }
                    break;
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Delete data pool.
        /// </summary>
        /// <param name="sObject">S object.</param>
        private void DeleteDataExecute(object sObject)
        {
            //NWDBenchmark.Start();
            NWDTypeClass tObject = sObject as NWDTypeClass;
            tObject.DeleteDataProceedWithTransaction();
            tObject.DeleteDataFinish();
            DeleteDataCompleted(tObject);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Delete data queue pool execute.
        /// </summary>
        /// <param name="sDeleteDataQueuePool">S delete data queue pool.</param>
        private void DeleteDataQueueExecute(List<NWDTypeClass> sDeleteDataQueuePool)
        {
            if (NWDDataManager.SharedInstance().SQLiteConnectionAccountIsValid() && NWDDataManager.SharedInstance().SQLiteConnectionEditorIsValid())
            {
                //NWDBenchmark.Start();
                //NWDDebug.Log("DeleteDataQueueExecute with " + sDeleteDataQueuePool.Count + " Object(s)");
                List<Type> tTypeList = new List<Type>();
                if (sDeleteDataQueuePool.Count > 0)
                {
                    {
                        using ITransaction tDeviceTransaction = NWDDataManager.SharedInstance().DeviceFactory.CreateTransaction();
                        tDeviceTransaction.Begin();
                        using ITransaction tEditorTransaction = NWDDataManager.SharedInstance().EditorFactory.CreateTransaction();
                        tEditorTransaction.Begin();

                        foreach (NWDTypeClass tObject in sDeleteDataQueuePool)
                        {
                            tObject.DeleteDataProceed(tDeviceTransaction, tEditorTransaction);
                        }

                        tEditorTransaction.Commit();
                        tDeviceTransaction.Commit();
                    }

                    foreach (NWDTypeClass tObject in kDeleteDataQueueMain)
                    {
                        Type tType = tObject.GetType();
                        if (tTypeList.Contains(tType) == false)
                        {
                            tTypeList.Add(tType);
                        }
                        tObject.DeleteDataFinish();
                    }
                    DeleteDataQueueCompleted(tTypeList);
                }
                sDeleteDataQueuePool = null;
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void DeleteDataCompleted(NWDTypeClass sObject)
        {
            //NWDBenchmark.Start();
            NWDDataManagerMainThread.AddDeleteType(sObject.GetType());
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void DeleteDataQueueCompleted(List<Type> sTypeList)
        {
            //NWDBenchmark.Start();
            NWDDataManagerMainThread.AddDeleteTypeList(sTypeList);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
#endregion Delete Data
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
