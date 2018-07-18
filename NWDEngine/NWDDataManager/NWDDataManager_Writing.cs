//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.IO;
using UnityEngine;
using System.Threading;
using System.Text.RegularExpressions;
using SQLite4Unity3d;
using BasicToolBox;
#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDWritingMode in Database.
    /// </summary>
    public enum NWDWritingMode : int
    {
        /// <summary>
        /// The data is writing in main thread now.
        /// </summary>
        MainThread, // Main Thread
        /// <summary>
        /// The data is writing in background thread now. 
        /// Can be concurrence by main thread an webservice!
        /// </summary>
        PoolThread, // Pool Thread
        /// <summary>
        /// The data is pull in queue in main thread. 
        /// The data will be writing directly on QueueExecution.
        /// </summary>
        QueuedMainThread, // Main Thread In Queue
        /// <summary>
        /// The data is pull in queue in background thread. 
        /// The data will be writing in background on QueueExecution. 
        /// Can be concurrence by main thread an webservice!
        /// </summary>
        QueuedPoolThread, // Pool Thread In Queue
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDWritingState the current mode of object. If it free you can change the state.
    /// </summary>
    public enum NWDWritingState : int
    {
        /// <summary>
        /// Free writing, You can change its mode
        /// </summary>
        Free,
        /// <summary>
        /// The data is writing in main thread now.
        /// </summary>
        MainThread, // Main Thread
        /// <summary>
        /// The data is writing in background thread now. 
        /// Can be concurrence by main thread an webservice!
        /// </summary>
        PoolThread, // Pool Thread
        /// <summary>
        /// The data is pull in queue in main thread. 
        /// The data will be writing directly on QueueExecution.
        /// </summary>
        MainThreadInQueue, // Main Thread In Queue
        /// <summary>
        /// The data is pull in queue in background thread. 
        /// The data will be writing in background on QueueExecution. 
        /// Can be concurrence by main thread an webservice!
        /// </summary>
        PoolThreadInQueue, // Pool Thread In Queue
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDWritingPending : int
    {
        /// <summary>
        /// Unknow pending...
        /// </summary>
        Unknow,
        /// <summary>
        /// New Object in memory. Pending writing in database.
        /// </summary>
        NewInMemory,
        /// <summary>
        /// Updated Object in memory. Pending writing in database.
        /// </summary>
        UpdateInMemory,
        /// <summary>
        /// Deleted Object in memory. Pending writing in database.
        /// </summary>
        DeleteInMemory,
        /// <summary>
        /// Object in database. You have the same representation.
        /// </summary>
        InDataBase,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDataManager
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The List of Data to Insert by queue in main thread
        /// </summary>
        public List<object> kInsertDataQueueMain = new List<object>();
        /// <summary>
        /// The List of Data to Update by queue in main thread
        /// </summary>
        public List<object> kUpdateDataQueueMain = new List<object>();
        /// <summary>
        /// The List of Data to Delete by queue in main thread
        /// </summary>
        public List<object> kDeleteDataQueueMain = new List<object>();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The List of Data to Insert by queue in pool thread
        /// </summary>
        public List<object> kInsertDataQueuePool = new List<object>();
        /// <summary>
        /// The List of Data to Update by queue in main thread
        /// </summary>
        public List<object> kUpdateDataQueuePool = new List<object>();
        /// <summary>
        /// The List of Data to Delete by queue in main thread
        /// </summary>
        public List<object> kDeleteDataQueuePool = new List<object>();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The List of Data in pending to write
        /// </summary>
        public List<object> kDataInWriting = new List<object>();
        //-------------------------------------------------------------------------------------------------------------
        #region Queue Data
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Datas in writing process.
        /// </summary>
        /// <returns><c>true</c>, if in writing process was dataed, <c>false</c> otherwise.</returns>
        public bool DataInWritingProcess()
        {
            BTBBenchmark.Start();
            bool rReturn = false;
            if (kDataInWriting.Count > 0)
            {
                rReturn = true;
            }
            BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Execute Queues
        /// </summary>
        public void DataQueueExecute()
        {
            BTBBenchmark.Start();
            DataQueueMainExecute();
            DataQueuePoolExecute();
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Execute the Queue on main thread
        /// </summary>
        public void DataQueueMainExecute()
        {
            BTBBenchmark.Start();
            InsertDataQueueExecute();
            UpdateDataQueueExecute();
            DeleteDataQueueExecute();
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Execute the Queue on pool thread
        /// </summary>
        public void DataQueuePoolExecute()
        {
            BTBBenchmark.Start();
            List<object> tInsertDataQueuePool = new List<object>(kInsertDataQueuePool);
            List<object> tUpdateDataQueuePool = new List<object>(kUpdateDataQueuePool);
            List<object> tDeleteDataQueuePool = new List<object>(kDeleteDataQueuePool);
            kInsertDataQueuePool = new List<object>();
            kUpdateDataQueuePool = new List<object>();
            kDeleteDataQueuePool = new List<object>();
            ThreadPool.QueueUserWorkItem(DataQueuePoolThread, new object[] {
                tInsertDataQueuePool,
                tUpdateDataQueuePool,
                tDeleteDataQueuePool
            });
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Execute the Queue on pool thread by ThreadPool.QueueUserWorkItem()
        /// </summary>
        private void DataQueuePoolThread(object sState)
        {
            BTBBenchmark.Start();
            object[] tParam = sState as object[];
            List<object> tInsertDataQueue = tParam[0] as List<object>;
            List<object> tUpdateDataQueue = tParam[1] as List<object>;
            List<object> tDeleteDataQueue = tParam[2] as List<object>;
            InsertDataQueuePoolExecute(tInsertDataQueue);
            UpdateDataQueuePoolExecute(tUpdateDataQueue);
            DeleteDataQueuePoolExecute(tDeleteDataQueue);
            BTBBenchmark.Finish();
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
        public void InsertData(object sObject, NWDWritingMode sWritingMode = NWDWritingMode.QueuedMainThread)
        {
            BTBBenchmark.Start();
            switch (sWritingMode)
            {
                case NWDWritingMode.MainThread:
                    {
                        Type tType = sObject.GetType();
                        var tMethodInfo = tType.GetMethod("InsertDataProceed", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                        if (tMethodInfo != null)
                        {
                            tMethodInfo.Invoke(sObject, null);
                        }
                        var tMethodFinish = tType.GetMethod("InsertDataFinish", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                        if (tMethodFinish != null)
                        {
                            tMethodFinish.Invoke(sObject, null);
                        }
                        BTBNotificationManager.SharedInstance().PostNotification(sObject, NWDNotificationConstants.K_DATA_LOCAL_INSERT);
                    }
                    break;
                case NWDWritingMode.PoolThread:
                    {
                        ThreadPool.QueueUserWorkItem(InsertDataPool, sObject );
                    }
                    break;
                case NWDWritingMode.QueuedMainThread:
                    {
                        if (kInsertDataQueueMain.Contains(sObject) == false)
                        {
                            kInsertDataQueueMain.Add(sObject);
                        }
                    }
                    break;
                case NWDWritingMode.QueuedPoolThread:
                    {
                        if (kInsertDataQueuePool.Contains(sObject) == false)
                        {
                            kInsertDataQueuePool.Add(sObject);
                        }
                    }
                    break;
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Insert data in database on pool thread.
        /// </summary>
        /// <param name="sObject">S object.</param>
        private void InsertDataPool(object sObject)
        {
            BTBBenchmark.Start();
            Type tType = sObject.GetType();
            var tMethodInfo = tType.GetMethod("InsertDataProceed", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            if (tMethodInfo != null)
            {
                tMethodInfo.Invoke(sObject, null);
            }
            var tMethodFinish = tType.GetMethod("InsertDataFinish", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            if (tMethodFinish != null)
            {
                tMethodFinish.Invoke(sObject, null);
            }
            BTBNotificationManager.SharedInstance().PostNotification(sObject, NWDNotificationConstants.K_DATA_LOCAL_INSERT);
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Insert data queue execute.
        /// </summary>
        private void InsertDataQueueExecute()
        {
            BTBBenchmark.Start();
            if (kInsertDataQueueMain.Count > 0)
            {
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.BeginTransaction();
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.BeginTransaction();
                foreach (object tObject in kInsertDataQueueMain)
                {
                    Type tType = tObject.GetType();
                    var tMethodInfo = tType.GetMethod("InsertDataProceed", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                    if (tMethodInfo != null)
                    {
                        tMethodInfo.Invoke(tObject, null);
                    }
                }
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.Commit();
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.Commit();
                foreach (object tObject in kInsertDataQueueMain)
                {
                    Type tType = tObject.GetType();
                    var tMethodFinish = tType.GetMethod("InsertDataFinish", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                    if (tMethodFinish != null)
                    {
                        tMethodFinish.Invoke(tObject, null);
                    }
                }
                kInsertDataQueueMain = new List<object>();
                BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_LOCAL_INSERT);
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Insert data queue pool execute.
        /// </summary>
        /// <param name="sInsertDataQueuePool">S insert data queue pool.</param>
        private void InsertDataQueuePoolExecute(List<object> sInsertDataQueuePool)
        {
            BTBBenchmark.Start();
            if (sInsertDataQueuePool.Count > 0)
            {
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.BeginTransaction();
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.BeginTransaction();
                foreach (object tObject in sInsertDataQueuePool)
                {
                    Type tType = tObject.GetType();
                    var tMethodInfo = tType.GetMethod("InsertDataProceed", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                    if (tMethodInfo != null)
                    {
                        tMethodInfo.Invoke(tObject, null);
                    }
                }
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.Commit();
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.Commit();
                foreach (object tObject in sInsertDataQueuePool)
                {
                    Type tType = tObject.GetType();
                    var tMethodFinish = tType.GetMethod("InsertDataFinish", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                    if (tMethodFinish != null)
                    {
                        tMethodFinish.Invoke(tObject, null);
                    }
                }
                BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_LOCAL_INSERT);
            }
            sInsertDataQueuePool = null;
            BTBBenchmark.Finish();
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
        public void UpdateData(object sObject, NWDWritingMode sWritingMode = NWDWritingMode.QueuedMainThread)
        {
            BTBBenchmark.Start();
            switch (sWritingMode)
            {
                case NWDWritingMode.MainThread:
                    {
                        Type tType = sObject.GetType();
                        var tMethodInfo = tType.GetMethod("UpdateDataProceed", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                        if (tMethodInfo != null)
                        {
                            tMethodInfo.Invoke(sObject, null);
                        }
                        var tMethodFinish = tType.GetMethod("UpdateDataFinish", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                        if (tMethodFinish != null)
                        {
                            tMethodFinish.Invoke(sObject, null);
                        }
                        BTBNotificationManager.SharedInstance().PostNotification(sObject, NWDNotificationConstants.K_DATA_LOCAL_UPDATE);
                    }
                    break;
                case NWDWritingMode.PoolThread:
                    {
                        ThreadPool.QueueUserWorkItem(UpdateDataPool, sObject);
                    }
                    break;
                case NWDWritingMode.QueuedMainThread:
                    {
                        if (kUpdateDataQueueMain.Contains(sObject) == false)
                        {
                            kUpdateDataQueueMain.Add(sObject);
                        }
                    }
                    break;
                case NWDWritingMode.QueuedPoolThread:
                    {
                        if (kUpdateDataQueuePool.Contains(sObject) == false)
                        {
                            kUpdateDataQueuePool.Add(sObject);
                        }
                    }
                    break;
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Update data in database on pool thread.
        /// </summary>
        /// <param name="sObject">S object.</param>
        private void UpdateDataPool(object sObject)
        {
            BTBBenchmark.Start();
            Type tType = sObject.GetType();
            var tMethodInfo = tType.GetMethod("UpdateDataProceed", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            if (tMethodInfo != null)
            {
                tMethodInfo.Invoke(sObject, null);
            }
            var tMethodFinish = tType.GetMethod("UpdateDataFinish", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            if (tMethodFinish != null)
            {
                tMethodFinish.Invoke(sObject, null);
            }
            BTBNotificationManager.SharedInstance().PostNotification(sObject, NWDNotificationConstants.K_DATA_LOCAL_UPDATE);
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Update data queue execute.
        /// </summary>
        private void UpdateDataQueueExecute()
        {
            BTBBenchmark.Start();
            if (kUpdateDataQueueMain.Count > 0)
            {
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.BeginTransaction();
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.BeginTransaction();
                foreach (object tObject in kUpdateDataQueueMain)
                {
                    Type tType = tObject.GetType();
                    var tMethodInfo = tType.GetMethod("UpdateDataProceed", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                    if (tMethodInfo != null)
                    {
                        tMethodInfo.Invoke(tObject, null);
                    }
                }
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.Commit();
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.Commit();
                foreach (object tObject in kUpdateDataQueueMain)
                {
                    Type tType = tObject.GetType();
                    var tMethodFinish = tType.GetMethod("UpdateDataFinish", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                    if (tMethodFinish != null)
                    {
                        tMethodFinish.Invoke(tObject, null);
                    }
                }
                kUpdateDataQueueMain = new List<object>();
                BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_LOCAL_UPDATE);
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Update data queue pool execute.
        /// </summary>
        /// <param name="sUpdateDataQueuePool">S update data queue pool.</param>
        private void UpdateDataQueuePoolExecute(List<object> sUpdateDataQueuePool)
        {
            BTBBenchmark.Start();
            if (sUpdateDataQueuePool.Count > 0)
            {
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.BeginTransaction();
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.BeginTransaction();
                foreach (object tObject in sUpdateDataQueuePool)
                {
                    Type tType = tObject.GetType();
                    var tMethodInfo = tType.GetMethod("UpdateDataProceed", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                    if (tMethodInfo != null)
                    {
                        tMethodInfo.Invoke(tObject, null);
                    }
                }
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.Commit();
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.Commit();
                foreach (object tObject in sUpdateDataQueuePool)
                {
                    Type tType = tObject.GetType();
                    var tMethodFinish = tType.GetMethod("UpdateDataFinish", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                    if (tMethodFinish != null)
                    {
                        tMethodFinish.Invoke(tObject, null);
                    }
                }
                BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_LOCAL_UPDATE);
            }
            sUpdateDataQueuePool = null;
            BTBBenchmark.Finish();
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
        public void DeleteData(object sObject, NWDWritingMode sWritingMode = NWDWritingMode.QueuedMainThread)
        {
            BTBBenchmark.Start();
            switch (sWritingMode)
            {
                case NWDWritingMode.MainThread:
                    {
                        Type tType = sObject.GetType();
                        var tMethodInfo = tType.GetMethod("DeleteDataProceed", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                        if (tMethodInfo != null)
                        {
                            tMethodInfo.Invoke(sObject, null);
                        }
                        var tMethodFinish = tType.GetMethod("DeleteDataFinish", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                        if (tMethodFinish != null)
                        {
                            tMethodFinish.Invoke(sObject, null);
                        }
                        BTBNotificationManager.SharedInstance().PostNotification(sObject, NWDNotificationConstants.K_DATA_LOCAL_DELETE);
                    }
                    break;
                case NWDWritingMode.PoolThread:
                    {
                        ThreadPool.QueueUserWorkItem(DeleteDataPool, sObject);
                    }
                    break;
                case NWDWritingMode.QueuedMainThread:
                    {
                        if (kDeleteDataQueueMain.Contains(sObject) == false)
                        {
                            kDeleteDataQueueMain.Add(sObject);
                        }
                    }
                    break;
                case NWDWritingMode.QueuedPoolThread:
                    {
                        if (kDeleteDataQueuePool.Contains(sObject) == false)
                        {
                            kDeleteDataQueuePool.Add(sObject);
                        }
                    }
                    break;
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Delete data pool.
        /// </summary>
        /// <param name="sObject">S object.</param>
        private void DeleteDataPool(object sObject)
        {
            BTBBenchmark.Start();
            Type tType = sObject.GetType();
            var tMethodInfo = tType.GetMethod("DeleteDataProceed", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            if (tMethodInfo != null)
            {
                tMethodInfo.Invoke(sObject, null);
            }
            var tMethodFinish = tType.GetMethod("DeleteDataFinish", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            if (tMethodFinish != null)
            {
                tMethodFinish.Invoke(sObject, null);
            }
            BTBNotificationManager.SharedInstance().PostNotification(sObject, NWDNotificationConstants.K_DATA_LOCAL_DELETE);
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Delete data queue execute.
        /// </summary>
        private void DeleteDataQueueExecute()
        {
            BTBBenchmark.Start();
            if (kDeleteDataQueueMain.Count > 0)
            {
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.BeginTransaction();
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.BeginTransaction();
                foreach (object tObject in kDeleteDataQueueMain)
                {
                    Type tType = tObject.GetType();
                    var tMethodInfo = tType.GetMethod("DeleteDataProceed", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                    if (tMethodInfo != null)
                    {
                        tMethodInfo.Invoke(tObject, null);
                    }
                }
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.Commit();
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.Commit();
                foreach (object tObject in kDeleteDataQueueMain)
                {
                    Type tType = tObject.GetType();
                    var tMethodFinish = tType.GetMethod("DeleteDataFinish", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                    if (tMethodFinish != null)
                    {
                        tMethodFinish.Invoke(tObject, null);
                    }
                }
                kDeleteDataQueueMain = new List<object>();
                BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_LOCAL_DELETE);
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Delete data queue pool execute.
        /// </summary>
        /// <param name="sDeleteDataQueuePool">S delete data queue pool.</param>
        private void DeleteDataQueuePoolExecute(List<object> sDeleteDataQueuePool)
        {
            BTBBenchmark.Start();
            if (sDeleteDataQueuePool.Count > 0)
            {
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.BeginTransaction();
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.BeginTransaction();
                foreach (object tObject in sDeleteDataQueuePool)
                {
                    Type tType = tObject.GetType();
                    var tMethodInfo = tType.GetMethod("DeleteDataProceed", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                    if (tMethodInfo != null)
                    {
                        tMethodInfo.Invoke(tObject, null);
                    }
                }
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.Commit();
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.Commit();
                foreach (object tObject in kDeleteDataQueueMain)
                {
                    Type tType = tObject.GetType();
                    var tMethodFinish = tType.GetMethod("DeleteDataFinish", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                    if (tMethodFinish != null)
                    {
                        tMethodFinish.Invoke(tObject, null);
                    }
                }
                BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_LOCAL_DELETE);
            }
            sDeleteDataQueuePool = null;
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion Delete Data
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The data was loaded from database.
        /// </summary>
        private bool FromDatabase = false;
        //-------------------------------------------------------------------------------------------------------------
        #region Lock Data
        /// <summary>
        /// The current state of the writing for this object.
        /// </summary>
        private NWDWritingState WritingState = NWDWritingState.Free;
        /// <summary>
        /// The writing lock counter. If lock is close the number is the number of lock!
        /// </summary>
        private int WritingLocksCounter = 0;
        /// <summary>
        /// The writing pending.
        /// </summary>
        private NWDWritingPending WritingPending = NWDWritingPending.Unknow;
        //-------------------------------------------------------------------------------------------------------------
        public NWDWritingPending DatabasePending()
        {
            return WritingPending;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Writing lock close once more.
        /// </summary>
        private void WritingLockAdd()
        {
            BTBBenchmark.Start();
            WritingLocksCounter++;
            if (NWDDataManager.SharedInstance().kDataInWriting.Contains(this) == false)
            {
                NWDDataManager.SharedInstance().kDataInWriting.Add(this);
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Writing lock open once. If lock =0 then the object can change writing mode.
        /// </summary>
        private void WritingLockRemove()
        {
            BTBBenchmark.Start();
            WritingLocksCounter--;
            if (WritingLocksCounter == 0)
            {
                WritingState = NWDWritingState.Free;
                if (NWDDataManager.SharedInstance().kDataInWriting.Contains(this) == false)
                {
                    NWDDataManager.SharedInstance().kDataInWriting.Remove(this);
                }
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion Lock Data
        #region New Data
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// New data.
        /// </summary>
        /// <returns>The data.</returns>
        /// <param name="sWritingMode">S writing mode.</param>
        static public NWDBasis<K> NewData(NWDWritingMode sWritingMode = NWDWritingMode.QueuedMainThread)
        {
            return NewDataWithReference(null, true, sWritingMode);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// New data with reference.
        /// </summary>
        /// <returns>The data with reference.</returns>
        /// <param name="sReference">S reference.</param>
        /// <param name="sAutoDate">If set to <c>true</c> s auto date.</param>
        /// <param name="sWritingMode">S writing mode.</param>
        static public NWDBasis<K> NewDataWithReference(string sReference, bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.QueuedMainThread)
        {
            BTBBenchmark.Start();
            NWDBasis<K> rReturnObject = null;
            if (ClassType() != null)
            {
                rReturnObject = (NWDBasis<K>)Activator.CreateInstance(ClassType(), new object[] { false });
                rReturnObject.InstanceInit();
                if (sReference == null || sReference == "")
                {
                    rReturnObject.Reference = rReturnObject.NewReference();
                }
                else
                {
                    rReturnObject.Reference = sReference;
                }
                foreach (PropertyInfo tPropInfo in PropertiesAccountDependent())
                {
                    //Debug.Log("try to insert automatically the account reference in the NWDAccount connection property : " + tPropInfo.Name);
                    NWDReferenceType<NWDAccount> tAtt = new NWDReferenceType<NWDAccount>();
                    tAtt.Value = NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
                    tPropInfo.SetValue(rReturnObject, tAtt, null);
                }
                rReturnObject.Initialization();
                rReturnObject.InsertData(sAutoDate, sWritingMode);
            }
            else
            {
                Debug.LogWarning("ClassType() is null for " + ClassNamePHP());
            }
            BTBBenchmark.Finish();
            return rReturnObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion New Data
        #region Duplicate Data
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Duplicate data.
        /// </summary>
        /// <returns>The data.</returns>
        /// <param name="sAutoDate">If set to <c>true</c> s auto date.</param>
        /// <param name="sWritingMode">S writing mode.</param>
        public NWDBasis<K> DuplicateData(bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.QueuedMainThread)
        {
            BTBBenchmark.Start();
            NWDBasis<K> rReturnObject = null;
            if (TestIntegrity() == true)
            {
                if (ClassType() != null)
                {
                    rReturnObject = (NWDBasis<K>)Activator.CreateInstance(ClassType(), new object[] { false });
                    rReturnObject.InstanceInit();
                    foreach (PropertyInfo tPropInfo in PropertiesAccountDependent())
                    {
                        NWDReferenceType<NWDAccount> tAtt = new NWDReferenceType<NWDAccount>();
                        tAtt.Value = NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
                        tPropInfo.SetValue(rReturnObject, tAtt, null);
                    }
                    rReturnObject.Initialization();
                    rReturnObject.CopyData(this);
                    rReturnObject.Reference = rReturnObject.NewReference();
                    // Change internal key by addding  "copy xxx"
                    string tOriginalKey = "" + InternalKey;
                    string tPattern = "\\(COPY [0-9]*\\)";
                    string tReplacement = "";
                    Regex tRegex = new Regex(tPattern);
                    tOriginalKey = tRegex.Replace(tOriginalKey, tReplacement);
                    tOriginalKey = tOriginalKey.TrimEnd();
                    // init search
                    int tCounter = 1;
                    string tCopy = tOriginalKey + " (COPY " + tCounter + ")";
                    // search available internal key
                    while (NWDBasis<K>.InternalKeyExists(tCopy) == true)
                    {
                        tCounter++;
                        tCopy = tOriginalKey + " (COPY " + tCounter + ")";
                    }
                    // set found internalkey
                    rReturnObject.InternalKey = tCopy;
                    // Update Data! become it's not a real insert but a copy!
                    rReturnObject.UpdateDataOperation(sAutoDate);
                    // Insert Data as new Data!
                    rReturnObject.AddonDuplicateMe();
                    rReturnObject.InsertData(sAutoDate, sWritingMode);
                }
                else
                {
                    Debug.LogWarning("ClassType() is null for " + ClassNamePHP());
                }
            }
            BTBBenchmark.Finish();
            return rReturnObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Test if InternalKey exists.
        /// </summary>
        /// <returns><c>true</c>, if key exists was internaled, <c>false</c> otherwise.</returns>
        /// <param name="sInternalKey">S internal key.</param>
        public static bool InternalKeyExists(string sInternalKey)
        {
            BTBBenchmark.Start();
            bool rReturn = false;
            K[] tArray = GetAllObjects(null);
            foreach (K tObject in tArray)
            {
                if (tObject.InternalKey == sInternalKey)
                {
                    rReturn = true;
                    break;
                }
            }
            BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Copy the data.
        /// </summary>
        /// <param name="sOriginal">S original.</param>
        public void CopyData(NWDBasis<K> sOriginal)
        {
            BTBBenchmark.Start();
            string[] tKey = SLQAssemblyOrderArray();
            Type tType = ClassType();
            foreach (string tPropertyString in tKey)
            {
                PropertyInfo tPropertyInfo = tType.GetProperty(tPropertyString, BindingFlags.Public | BindingFlags.Instance);
                Type tTypeOfThis = tPropertyInfo.PropertyType;
                object tValue = tPropertyInfo.GetValue(sOriginal, null);
                if (tValue == null)
                {
                    tValue = "";
                }
                string tValueString = string.Copy(tValue.ToString()); // force to copy new object
                if (tTypeOfThis.IsEnum)
                {
                    int tTemp = 0;
                    int.TryParse(tValueString, out tTemp);
                    tPropertyInfo.SetValue(this, tTemp, null);
                }
                else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataType)))
                {
                    BTBDataType tTemp = Activator.CreateInstance(tTypeOfThis) as BTBDataType;
                    tTemp.SetString(tValueString);
                    tPropertyInfo.SetValue(this, tTemp, null);
                }
                else if (tTypeOfThis == typeof(String) || tTypeOfThis == typeof(string))
                {
                    tPropertyInfo.SetValue(this, tValueString, null);
                }
                else if (tTypeOfThis == typeof(bool))
                {
                    bool tValueInsert = false;
                    int tTemp = 0;
                    int.TryParse(tValueString, out tTemp);
                    if (tTemp > 0)
                    {
                        tValueInsert = true;
                    }
                    tPropertyInfo.SetValue(this, tValueInsert, null);
                }
                else if (tTypeOfThis == typeof(int))
                {
                    int tTemp = 0;
                    int.TryParse(tValueString, out tTemp);
                    tPropertyInfo.SetValue(this, tTemp, null);
                }
                else if (tTypeOfThis == typeof(Int16))
                {
                    Int16 tTemp = 0;
                    Int16.TryParse(tValueString, out tTemp);
                    tPropertyInfo.SetValue(this, tTemp, null);
                }
                else if (tTypeOfThis == typeof(Int32))
                {
                    Int32 tTemp = 0;
                    Int32.TryParse(tValueString, out tTemp);
                    tPropertyInfo.SetValue(this, tTemp, null);
                }
                else if (tTypeOfThis == typeof(Int64))
                {
                    Int64 tTemp = 0;
                    Int64.TryParse(tValueString, out tTemp);
                    tPropertyInfo.SetValue(this, tTemp, null);
                }
                else if (tTypeOfThis == typeof(float))
                {
                    float tTemp = 0;
                    float.TryParse(tValueString, out tTemp);
                    tPropertyInfo.SetValue(this, tTemp, null);
                }
                else if (tTypeOfThis == typeof(double))
                {
                    double tTemp = 0;
                    double.TryParse(tValueString, out tTemp);
                    tPropertyInfo.SetValue(this, tTemp, null);
                }
                else if (tTypeOfThis == typeof(Single))
                {
                    Single tTemp = 0;
                    Single.TryParse(tValueString, out tTemp);
                    tPropertyInfo.SetValue(this, tTemp, null);
                }
                else if (tTypeOfThis == typeof(Double))
                {
                    Double tTemp = 0;
                    Double.TryParse(tValueString, out tTemp);
                    tPropertyInfo.SetValue(this, tTemp, null);
                }
                else if (tTypeOfThis == typeof(Decimal))
                {
                    Decimal tTemp = 0;
                    Decimal.TryParse(tValueString, out tTemp);
                    tPropertyInfo.SetValue(this, tTemp, null);
                }
                else
                {
                }
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion Duplicate Data
        #region Insert Data
        //-------------------------------------------------------------------------------------------------------------
        public void InsertDataProceed()
        {
            BTBBenchmark.Start();
            if (AccountDependent())
            {
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.Insert(this);
            }
            else
            {
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.Insert(this);
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InsertDataFinish()
        {
            BTBBenchmark.Start();
            WritingLockRemove();
            WritingPending = NWDWritingPending.InDataBase;
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool InsertData(bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.QueuedMainThread)
        {
            BTBBenchmark.Start();
            bool rReturn = false;
            // We can only insert free object
            if (WritingState == NWDWritingState.Free)
            {
                switch (WritingState)
                {
                    case NWDWritingState.Free:
                        // ok you can change the mode
                        break;
                    case NWDWritingState.MainThread:
                        // strange ... not possible.
                        break;
                    case NWDWritingState.MainThreadInQueue:
                        // you can use only this too because writing can be concomitant
                        sWritingMode = NWDWritingMode.QueuedMainThread;
                        Debug.LogWarning("Object can't bypass the preview writing mode. Waiting this object will free.");
                        break;
                    case NWDWritingState.PoolThread:
                        // you can use only this too because writing can be concomitant
                        sWritingMode = NWDWritingMode.PoolThread;
                        Debug.LogWarning("Object can't bypass the preview writing mode. Waiting this object will free.");
                        break;
                    case NWDWritingState.PoolThreadInQueue:
                        // you can use only this too because writing can be concomitant
                        sWritingMode = NWDWritingMode.QueuedPoolThread;
                        Debug.LogWarning("Object can't bypass the preview writing mode. Waiting this object will free.");
                        break;
                }
                if (ObjectsByReferenceList.Contains(this.Reference) == false)
                {
                    bool tDoInsert = true;
                    switch (sWritingMode)
                    {
                        case NWDWritingMode.MainThread:
                            break;
                        case NWDWritingMode.QueuedMainThread:
                            if (NWDDataManager.SharedInstance().kInsertDataQueueMain.Contains(this))
                            {
                                tDoInsert = false;
                            }
                            break;
                        case NWDWritingMode.PoolThread:
                            break;
                        case NWDWritingMode.QueuedPoolThread:
                            if (NWDDataManager.SharedInstance().kInsertDataQueuePool.Contains(this))
                            {
                                tDoInsert = false;
                            }
                            break;
                    }
                    if (tDoInsert == true)
                    {
                        WritingLockAdd();
                        InsertDataOperation(sAutoDate);
                        WritingPending = NWDWritingPending.NewInMemory;
                        AddObjectInListOfEdition(this);
                        NWDDataManager.SharedInstance().InsertData(this, sWritingMode);
                        rReturn = true;
                    }
                }
                else
                {
                    // error this reference allready exist
                    UpdateDataIfModified(sAutoDate, sWritingMode);
                }
#if UNITY_EDITOR
                NWDDataManager.SharedInstance().RepaintWindowsInManager(typeof(K));
#endif
            }
            BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InsertDataOperation(bool sAutoDate = true)
        {
            BTBBenchmark.Start();
            if (sAutoDate == true)
            {
                this.DC = NWDToolbox.Timestamp();
                this.DM = NWDToolbox.Timestamp();
            }
            NWDVersionType tVersion = new NWDVersionType();
            tVersion.SetString("0.00.00");
            this.MinVersion = tVersion;
            this.WebServiceVersion = WebServiceVersionToUse();
            this.DS = 0;
            this.DevSync = 0;
            this.PreprodSync = 0;
            this.ProdSync = 0;
            this.ServerHash = "";
            this.ServerLog = "";
            this.AddonInsertMe();
            this.UpdateIntegrity();
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion Insert Data
        #region Update Data
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateDataProceed()
        {
            BTBBenchmark.Start();
            if (AccountDependent())
            {
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.Update(this);
            }
            else
            {
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.Update(this);
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateDataFinish()
        {
            BTBBenchmark.Start();
            WritingLockRemove();
            WritingPending = NWDWritingPending.InDataBase;
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateData(bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.QueuedMainThread)
        {
            BTBBenchmark.Start();
            switch (WritingState)
            {
                case NWDWritingState.Free:
                    // ok you can change the mode
                    break;
                case NWDWritingState.MainThread:
                    // strange ... not possible.
                    break;
                case NWDWritingState.MainThreadInQueue:
                    // you can use only this too because writing can be concomitant
                    sWritingMode = NWDWritingMode.QueuedMainThread;
                    Debug.LogWarning("Object can't bypass the preview writing mode. Waiting this object will free.");
                    break;
                case NWDWritingState.PoolThread:
                    // you can use only this too because writing can be concomitant
                    sWritingMode = NWDWritingMode.PoolThread;
                    Debug.LogWarning("Object can't bypass the preview writing mode. Waiting this object will free.");
                    break;
                case NWDWritingState.PoolThreadInQueue:
                    // you can use only this too because writing can be concomitant
                    sWritingMode = NWDWritingMode.QueuedPoolThread;
                    Debug.LogWarning("Object can't bypass the preview writing mode. Waiting this object will free.");
                    break;
            }
            bool tDoUpdate = true;
            switch (sWritingMode)
            {
                case NWDWritingMode.MainThread:
                    break;
                case NWDWritingMode.QueuedMainThread:
                    if (NWDDataManager.SharedInstance().kUpdateDataQueueMain.Contains(this))
                    {
                        tDoUpdate = false;
                    }
                    break;
                case NWDWritingMode.PoolThread:
                    break;
                case NWDWritingMode.QueuedPoolThread:
                    if (NWDDataManager.SharedInstance().kUpdateDataQueuePool.Contains(this))
                    {
                        tDoUpdate = false;
                    }
                    break;
            }
            if (tDoUpdate == true)
            {
                WritingLockAdd();
                this.AddonUpdateMe(); // call override method
                UpdateDataOperation(sAutoDate);
                WritingPending = NWDWritingPending.UpdateInMemory;
                UpdateObjectInListOfEdition(this);
                NWDDataManager.SharedInstance().UpdateData(this, sWritingMode);
                this.AddonUpdatedMe(); // call override method
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateDataOperation(bool sAutoDate = true)
        {
            BTBBenchmark.Start();
            // so object is prepared to be update
            if (sAutoDate == true)
            {
                this.DM = NWDToolbox.Timestamp();
            }
            // this.DS = 0;
            // prevent DevSync lock
            if (this.DevSync >= 0)
            {
                this.DevSync = 0;
            }
            // prevent PreprodSync lock
            if (this.PreprodSync >= 0)
            {
                this.PreprodSync = 0;
            }
            // prevent ProdSync lock
            if (this.ProdSync >= 0)
            {
                this.ProdSync = 0;
            }
            // reset Hash server
            this.ServerHash = "";
            // Update WebServiceVersion
            int tWS = WebServiceVersionToUse();
            if (this.WebServiceVersion != tWS)
            {
                this.AddonVersionMe(); // call override method
                this.WebServiceVersion = WebServiceVersionToUse();
            }
            this.UpdateIntegrity();
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool UpdateDataIfModified(bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.QueuedMainThread)
        {
            BTBBenchmark.Start();
            bool tReturn = false;
            if (this.Integrity != this.IntegrityValue())
            {
                tReturn = true;
                UpdateData(sAutoDate, sWritingMode);
            }
            BTBBenchmark.Finish();
            return tReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void EnableData(NWDWritingMode sWritingMode = NWDWritingMode.QueuedMainThread)
        {
            BTBBenchmark.Start();
            this.AC = true;
            this.AddonEnableMe(); // call override method
            this.UpdateData(true, sWritingMode);
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DisableData(NWDWritingMode sWritingMode = NWDWritingMode.QueuedMainThread)
        {
            BTBBenchmark.Start();
            this.DD = NWDToolbox.Timestamp();
            this.AC = false;
            this.AddonDisableMe(); // call override method
            this.UpdateData(true, sWritingMode);
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void TrashData(NWDWritingMode sWritingMode = NWDWritingMode.QueuedMainThread)
        {
            BTBBenchmark.Start();
            int tTimestamp = NWDToolbox.Timestamp();
            this.DD = tTimestamp;
            this.XX = tTimestamp;
            this.AC = false;
            this.AddonTrashMe(); // call override method
            this.UpdateData(true, sWritingMode);
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UnTrashData(NWDWritingMode sWritingMode = NWDWritingMode.QueuedMainThread)
        {
            BTBBenchmark.Start();
            this.XX = 0;
            this.AC = false;
            this.AddonUnTrashMe(); // call override method
            this.UpdateData(true, sWritingMode);
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion Update Data
        #region Delete Data
        //-------------------------------------------------------------------------------------------------------------
        private void DeleteDataProceed()
        {
            BTBBenchmark.Start();
            if (AccountDependent())
            {
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.Delete(this);
            }
            else
            {
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.Delete(this);
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void DeleteDataFinish()
        {
            BTBBenchmark.Start();
            WritingLockRemove();
            WritingPending = NWDWritingPending.InDataBase;
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void DeleteData(NWDWritingMode sWritingMode = NWDWritingMode.QueuedMainThread)
        {
            BTBBenchmark.Start();
            switch (WritingState)
            {
                case NWDWritingState.Free:
                    // ok you can change the mode
                    break;
                case NWDWritingState.MainThread:
                    // strange ... not possible.
                    break;
                case NWDWritingState.MainThreadInQueue:
                    // you can use only this too because writing can be concomitant
                    sWritingMode = NWDWritingMode.QueuedMainThread;
                    Debug.LogWarning("Object can't bypass the preview writing mode. Waiting this object will free.");
                    break;
                case NWDWritingState.PoolThread:
                    // you can use only this too because writing can be concomitant
                    sWritingMode = NWDWritingMode.PoolThread;
                    Debug.LogWarning("Object can't bypass the preview writing mode. Waiting this object will free.");
                    break;
                case NWDWritingState.PoolThreadInQueue:
                    // you can use only this too because writing can be concomitant
                    sWritingMode = NWDWritingMode.QueuedPoolThread;
                    Debug.LogWarning("Object can't bypass the preview writing mode. Waiting this object will free.");
                    break;
            }
            bool tDoDelete = true;
            switch (sWritingMode)
            {
                case NWDWritingMode.MainThread:
                    break;
                case NWDWritingMode.QueuedMainThread:
                    if (NWDDataManager.SharedInstance().kDeleteDataQueueMain.Contains(this))
                    {
                        tDoDelete = false;
                    }
                    break;
                case NWDWritingMode.PoolThread:
                    break;
                case NWDWritingMode.QueuedPoolThread:
                    if (NWDDataManager.SharedInstance().kDeleteDataQueuePool.Contains(this))
                    {
                        tDoDelete = false;
                    }
                    break;
            }
            if (tDoDelete == true)
            {
                if (NWDDataManager.SharedInstance().kUpdateDataQueueMain.Contains(this))
                {
                    NWDDataManager.SharedInstance().kUpdateDataQueueMain.Remove(this);
                    WritingLockRemove();
                }
                if (NWDDataManager.SharedInstance().kInsertDataQueueMain.Contains(this))
                {
                    NWDDataManager.SharedInstance().kInsertDataQueueMain.Remove(this);
                    WritingLockRemove();
                }
                if (NWDDataManager.SharedInstance().kUpdateDataQueuePool.Contains(this))
                {
                    NWDDataManager.SharedInstance().kUpdateDataQueuePool.Remove(this);
                    WritingLockRemove();
                }
                if (NWDDataManager.SharedInstance().kInsertDataQueuePool.Contains(this))
                {
                    NWDDataManager.SharedInstance().kInsertDataQueuePool.Remove(this);
                    WritingLockRemove();
                }
                WritingLockAdd();
                this.AddonDeleteMe(); // call override method
                RemoveObjectInListOfEdition(this);
                WritingPending = NWDWritingPending.DeleteInMemory;
                NWDDataManager.SharedInstance().DeleteData(this, sWritingMode);
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion Update Data
        #region Load Data
        //-------------------------------------------------------------------------------------------------------------
        public void LoadedFromDatabase()
        {
            BTBBenchmark.Start();
            FromDatabase = true;
            WritingPending = NWDWritingPending.InDataBase;
            AddObjectInListOfEdition(this);
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void LoadAllDatas()
        {
            BTBBenchmark.Start();
            SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
            if (AccountDependent())
            {
                tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
            }
            IEnumerable tEnumerable = tSQLiteConnection.Table<K>().OrderBy(x => x.InternalKey);
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
                foreach (NWDBasis<K> tData in tEnumerable)
                {
                    tData.LoadedFromDatabase();
                }
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion Load Data
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================