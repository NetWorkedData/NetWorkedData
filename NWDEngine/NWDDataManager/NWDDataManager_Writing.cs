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
        InsertInMemory,
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
        InDatabase,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDataManager
    {
        //-------------------------------------------------------------------------------------------------------------
        static public void Main()
        {
            Debug.Log("NWDDataManager Main");
        }
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
            List<object> tInsertDataQueueMain = new List<object>(kInsertDataQueueMain);
            List<object> tUpdateDataQueueMain = new List<object>(kUpdateDataQueueMain);
            List<object> tDeleteDataQueueMain = new List<object>(kDeleteDataQueueMain);
            kInsertDataQueueMain = new List<object>();
            kUpdateDataQueueMain = new List<object>();
            kDeleteDataQueueMain = new List<object>();
            InsertDataQueueExecute(tInsertDataQueueMain);
            UpdateDataQueueExecute(tUpdateDataQueueMain);
            DeleteDataQueueExecute(tDeleteDataQueueMain);
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
            InsertDataQueueExecute(tInsertDataQueue);
            UpdateDataQueueExecute(tUpdateDataQueue);
            DeleteDataQueueExecute(tDeleteDataQueue);
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
        public void InsertData(object sObject, NWDWritingMode sWritingMode = NWDWritingMode.MainThread)
        {
            //BTBBenchmark.Start();
            switch (sWritingMode)
            {
                case NWDWritingMode.MainThread:
                    {
                        Debug.Log("NWDDataManager InsertData() MainThread");
                        InsertDataExecute(sObject);
                    }
                    break;
                case NWDWritingMode.PoolThread:
                    {
                        Debug.Log("NWDDataManager InsertData() PoolThread");
#if UNITY_EDITOR
                        RepaintWindowForData(sObject.GetType());
#endif
                        ThreadPool.QueueUserWorkItem(InsertDataExecute, sObject);
                    }
                    break;
                case NWDWritingMode.QueuedMainThread:
                    {
                        Debug.Log("NWDDataManager InsertData() QueuedMainThread");
                        if (kInsertDataQueueMain.Contains(sObject) == false)
                        {
#if UNITY_EDITOR
                            RepaintWindowForData(sObject.GetType());
#endif
                            kInsertDataQueueMain.Add(sObject);
                        }
                    }
                    break;
                case NWDWritingMode.QueuedPoolThread:
                    {
                        Debug.Log("NWDDataManager InsertData() QueuedPoolThread");
                        if (kInsertDataQueuePool.Contains(sObject) == false)
                        {
#if UNITY_EDITOR
                            RepaintWindowForData(sObject.GetType());
#endif
                            kInsertDataQueuePool.Add(sObject);
                        }
                    }
                    break;
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Insert data in database on pool thread.
        /// </summary>
        /// <param name="sObject">S object.</param>
        private void InsertDataExecute(object sObject)
        {
            //BTBBenchmark.Start();
            Type tType = sObject.GetType();
            //NWDDataManager.SharedInstance().SQLiteConnectionAccount.BeginTransaction();
            //NWDDataManager.SharedInstance().SQLiteConnectionEditor.BeginTransaction();
            var tMethodInfo = tType.GetMethod("InsertDataProceedWithTransaction", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            if (tMethodInfo != null)
            {
                tMethodInfo.Invoke(sObject, null);
            }
            //NWDDataManager.SharedInstance().SQLiteConnectionAccount.Commit();
            //NWDDataManager.SharedInstance().SQLiteConnectionEditor.Commit();
            var tMethodFinish = tType.GetMethod("InsertDataFinish", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            if (tMethodFinish != null)
            {
                tMethodFinish.Invoke(sObject, null);
            }
            InsertDataCompleted(sObject);

            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Insert data queue pool execute.
        /// </summary>
        /// <param name="sInsertDataQueuePool">S insert data queue pool.</param>
        private void InsertDataQueueExecute(List<object> sInsertDataQueuePool)
        {
            //BTBBenchmark.Start();
            Debug.Log("InsertDataQueueExecute with " + sInsertDataQueuePool.Count + " Object(s)");
            List<Type> tTypeList = new List<Type>();
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
                    if (tTypeList.Contains(tType) == false)
                    {
                        tTypeList.Add(tType);
                    };
                    var tMethodFinish = tType.GetMethod("InsertDataFinish", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                    if (tMethodFinish != null)
                    {
                        tMethodFinish.Invoke(tObject, null);
                    }
                }
                InsertDataQueueCompleted(tTypeList);
            }
            sInsertDataQueuePool = null;
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void InsertDataCompleted(object sObject)
        {
            //BTBBenchmark.Start();
            NWDDataManagerMainThread.AddInsertType(sObject.GetType());
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void InsertDataQueueCompleted(List<Type> sTypeList)
        {
            //BTBBenchmark.Start();
            NWDDataManagerMainThread.AddInsertTypeList(sTypeList);
            //BTBBenchmark.Finish();
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
        public void UpdateData(object sObject, NWDWritingMode sWritingMode = NWDWritingMode.MainThread)
        {
            //BTBBenchmark.Start();
            switch (sWritingMode)
            {
                case NWDWritingMode.MainThread:
                    {
                        Debug.Log("NWDDataManager UpdateData() MainThread");
                        UpdateDataExecute(sObject);
                        BTBNotificationManager.SharedInstance().PostNotification(sObject, NWDNotificationConstants.K_DATA_LOCAL_UPDATE);
                    }
                    break;
                case NWDWritingMode.PoolThread:
                    {
                        Debug.Log("NWDDataManager UpdateData() PoolThread");
#if UNITY_EDITOR
                        RepaintWindowForData(sObject.GetType());
#endif
                        ThreadPool.QueueUserWorkItem(UpdateDataExecute, sObject);

                    }
                    break;
                case NWDWritingMode.QueuedMainThread:
                    {
                        Debug.Log("NWDDataManager UpdateData() QueuedMainThread");
                        if (kUpdateDataQueueMain.Contains(sObject) == false)
                        {
#if UNITY_EDITOR
                            RepaintWindowForData(sObject.GetType());
#endif
                            kUpdateDataQueueMain.Add(sObject);
                        }
                    }
                    break;
                case NWDWritingMode.QueuedPoolThread:
                    {
                        Debug.Log("NWDDataManager UpdateData() QueuedPoolThread");
                        if (kUpdateDataQueuePool.Contains(sObject) == false)
                        {
#if UNITY_EDITOR
                            RepaintWindowForData(sObject.GetType());
#endif
                            kUpdateDataQueuePool.Add(sObject);
                        }
                    }
                    break;
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Update data in database on pool thread.
        /// </summary>
        /// <param name="sObject">S object.</param>
        private void UpdateDataExecute(object sObject)
        {
            //BTBBenchmark.Start();
            Type tType = sObject.GetType();
            //NWDDataManager.SharedInstance().SQLiteConnectionAccount.BeginTransaction();
            //NWDDataManager.SharedInstance().SQLiteConnectionEditor.BeginTransaction();
            var tMethodInfo = tType.GetMethod("UpdateDataProceed", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            if (tMethodInfo != null)
            {
                tMethodInfo.Invoke(sObject, null);
            }
            //NWDDataManager.SharedInstance().SQLiteConnectionAccount.Commit();
            //NWDDataManager.SharedInstance().SQLiteConnectionEditor.Commit();
            var tMethodFinish = tType.GetMethod("UpdateDataFinish", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            if (tMethodFinish != null)
            {
                tMethodFinish.Invoke(sObject, null);
            }
            UpdateDataCompleted(sObject);
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Update data queue pool execute.
        /// </summary>
        /// <param name="sUpdateDataQueuePool">S update data queue pool.</param>
        private void UpdateDataQueueExecute(List<object> sUpdateDataQueuePool)
        {
            //BTBBenchmark.Start();
            Debug.Log("UpdateDataQueueExecute with " + sUpdateDataQueuePool.Count + " Object(s)");
            List<Type> tTypeList = new List<Type>();
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
                    if (tTypeList.Contains(tType) == false)
                    {
                        tTypeList.Add(tType);
                    };
                    var tMethodFinish = tType.GetMethod("UpdateDataFinish", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                    if (tMethodFinish != null)
                    {
                        tMethodFinish.Invoke(tObject, null);
                    }
                }
                UpdateDataQueueCompleted(tTypeList);
            }
            sUpdateDataQueuePool = null;
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void UpdateDataCompleted(object sObject)
        {
            //BTBBenchmark.Start();
            NWDDataManagerMainThread.AddUpdateType(sObject.GetType());
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void UpdateDataQueueCompleted(List<Type> sTypeList)
        {
            //BTBBenchmark.Start();
            NWDDataManagerMainThread.AddUpdateTypeList(sTypeList);
            //BTBBenchmark.Finish();
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
        public void DeleteData(object sObject, NWDWritingMode sWritingMode = NWDWritingMode.MainThread)
        {
            //BTBBenchmark.Start();
            switch (sWritingMode)
            {
                case NWDWritingMode.MainThread:
                    {
                        Debug.Log("NWDDataManager DeleteData() MainThread");
                        DeleteDataExecute(sObject);
                        BTBNotificationManager.SharedInstance().PostNotification(sObject, NWDNotificationConstants.K_DATA_LOCAL_DELETE);
                    }
                    break;
                case NWDWritingMode.PoolThread:
                    {
                        Debug.Log("NWDDataManager DeleteData() PoolThread");
#if UNITY_EDITOR
                        RepaintWindowForData(sObject.GetType());
#endif
                        ThreadPool.QueueUserWorkItem(DeleteDataExecute, sObject);
                    }
                    break;
                case NWDWritingMode.QueuedMainThread:
                    {
                        Debug.Log("NWDDataManager DeleteData() QueuedMainThread");
                        if (kDeleteDataQueueMain.Contains(sObject) == false)
                        {
#if UNITY_EDITOR
                            RepaintWindowForData(sObject.GetType());
#endif
                            kDeleteDataQueueMain.Add(sObject);
                        }
                    }
                    break;
                case NWDWritingMode.QueuedPoolThread:
                    {
                        Debug.Log("NWDDataManager DeleteData() QueuedPoolThread");
                        if (kDeleteDataQueuePool.Contains(sObject) == false)
                        {
#if UNITY_EDITOR
                            RepaintWindowForData(sObject.GetType());
#endif
                            kDeleteDataQueuePool.Add(sObject);
                        }
                    }
                    break;
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Delete data pool.
        /// </summary>
        /// <param name="sObject">S object.</param>
        private void DeleteDataExecute(object sObject)
        {
            //BTBBenchmark.Start();
            Type tType = sObject.GetType();
            //NWDDataManager.SharedInstance().SQLiteConnectionAccount.BeginTransaction();
            //NWDDataManager.SharedInstance().SQLiteConnectionEditor.BeginTransaction();
            var tMethodInfo = tType.GetMethod("DeleteDataProceed", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            if (tMethodInfo != null)
            {
                tMethodInfo.Invoke(sObject, null);
            }
            //NWDDataManager.SharedInstance().SQLiteConnectionAccount.Commit();
            //NWDDataManager.SharedInstance().SQLiteConnectionEditor.Commit();
            var tMethodFinish = tType.GetMethod("DeleteDataFinish", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
            if (tMethodFinish != null)
            {
                tMethodFinish.Invoke(sObject, null);
            }
            DeleteDataCompleted(sObject);
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Delete data queue pool execute.
        /// </summary>
        /// <param name="sDeleteDataQueuePool">S delete data queue pool.</param>
        private void DeleteDataQueueExecute(List<object> sDeleteDataQueuePool)
        {
            //BTBBenchmark.Start();
            Debug.Log("DeleteDataQueueExecute with " + sDeleteDataQueuePool.Count + " Object(s)");
            List<Type> tTypeList = new List<Type>();
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
                    if (tTypeList.Contains(tType) == false)
                    {
                        tTypeList.Add(tType);
                    };
                    var tMethodFinish = tType.GetMethod("DeleteDataFinish", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                    if (tMethodFinish != null)
                    {
                        tMethodFinish.Invoke(tObject, null);
                    }
                }
                DeleteDataQueueCompleted(tTypeList);
            }
            sDeleteDataQueuePool = null;
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void DeleteDataCompleted(object sObject)
        {
            //BTBBenchmark.Start();
            NWDDataManagerMainThread.AddDeleteType(sObject.GetType());
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void DeleteDataQueueCompleted(List<Type> sTypeList)
        {
            //BTBBenchmark.Start();
            NWDDataManagerMainThread.AddDeleteTypeList(sTypeList);
            //BTBBenchmark.Finish();
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
            //BTBBenchmark.Start();
            WritingLocksCounter++;
            if (NWDDataManager.SharedInstance().kDataInWriting.Contains(this) == false)
            {
                NWDDataManager.SharedInstance().kDataInWriting.Add(this);
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Writing lock open once. If lock =0 then the object can change writing mode.
        /// </summary>
        private void WritingLockRemove()
        {
            //BTBBenchmark.Start();
            WritingLocksCounter--;
            if (WritingLocksCounter == 0)
            {
                WritingState = NWDWritingState.Free;
                if (NWDDataManager.SharedInstance().kDataInWriting.Contains(this) == false)
                {
                    NWDDataManager.SharedInstance().kDataInWriting.Remove(this);
                }
            }
            //BTBBenchmark.Finish();
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
        static public K NewData(NWDWritingMode sWritingMode = NWDWritingMode.MainThread)
        {
            //BTBBenchmark.Start();
            K rReturnObject = NewDataWithReference(null, true, sWritingMode);
            //BTBBenchmark.Finish();
            return rReturnObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// New data with reference.
        /// </summary>
        /// <returns>The data with reference.</returns>
        /// <param name="sReference">S reference.</param>
        /// <param name="sAutoDate">If set to <c>true</c> s auto date.</param>
        /// <param name="sWritingMode">S writing mode.</param>
        static public K NewDataWithReference(string sReference, bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.QueuedMainThread)
        {
            //BTBBenchmark.Start();
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
            //BTBBenchmark.Finish();
            return rReturnObject as K;
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
        public K DuplicateData(bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.MainThread)
        {
            //BTBBenchmark.Start();
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
                    int tDC = rReturnObject.DC; // memorize date of dupplicate
                    string tReference = rReturnObject.NewReference(); // create reference for dupplicate
                    rReturnObject.CopyData(this); // copy data
                    // restore the DC and Reference 
                    rReturnObject.Reference = tReference;
                    rReturnObject.DC = tDC;
                    // WARNING ... copy generate an error in XX ? 
                    // but copy the DD XX and AC from this
                    rReturnObject.DD = DD;
                    rReturnObject.XX = XX;
                    rReturnObject.AC = AC;
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
            //BTBBenchmark.Finish();
            return rReturnObject as K;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Test if InternalKey exists.
        /// </summary>
        /// <returns><c>true</c>, if key exists was internaled, <c>false</c> otherwise.</returns>
        /// <param name="sInternalKey">S internal key.</param>
        public static bool InternalKeyExists(string sInternalKey)
        {
            //BTBBenchmark.Start();
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
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Copy the data.
        /// </summary>
        /// <param name="sOriginal">S original.</param>
        public void CopyData(NWDBasis<K> sOriginal)
        {
            //BTBBenchmark.Start();
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
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion Duplicate Data
        #region Insert Data
        //-------------------------------------------------------------------------------------------------------------
        public bool InsertData(bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.MainThread)
        {
            //BTBBenchmark.Start();
            bool rReturn = false;
            // Verif if Systeme can use the thread (option in Environment)
            if (NWDAppEnvironment.SelectedEnvironment().ThreadPoolSQLActive == false)
            {
                if (sWritingMode == NWDWritingMode.PoolThread)
                {
                    sWritingMode = NWDWritingMode.MainThread;
                }
                else if (sWritingMode == NWDWritingMode.QueuedPoolThread)
                {
                    sWritingMode = NWDWritingMode.QueuedMainThread;
                }
            }
            // Compare with actual State of object and force to follow the actual state
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
                        rReturn = true;
                        this.AddonInsertMe();
                        InsertDataOperation(sAutoDate);
                        AddObjectInListOfEdition(this);
                        WritingLockAdd();
                        WritingPending = NWDWritingPending.InsertInMemory;
                        NWDDataManager.SharedInstance().InsertData(this, sWritingMode);
                    }
                }
                else
                {
                    // error this reference allready exist
                    UpdateDataIfModified(sAutoDate, sWritingMode);
                }
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InsertDataOperation(bool sAutoDate = true)
        {
            //BTBBenchmark.Start();
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
            this.UpdateIntegrity();
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InsertDataProceed()
        {
            //BTBBenchmark.Start();
            if (AccountDependent())
            {
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.Insert(this);
            }
            else
            {
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.Insert(this);
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InsertDataProceedWithTransaction()
        {
            //BTBBenchmark.Start();
            if (AccountDependent())
            {
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.BeginTransaction();
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.Insert(this);
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.Commit();
            }
            else
            {
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.BeginTransaction();
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.Insert(this);
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.Commit();
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InsertDataFinish()
        {
            //BTBBenchmark.Start();
            WritingLockRemove();
            WritingPending = NWDWritingPending.InDatabase;
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion Insert Data
        #region Update Data
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateData(bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.MainThread)
        {
            //BTBBenchmark.Start();
            // Verif if Systeme can use the thread (option in Environment)
            if (NWDAppEnvironment.SelectedEnvironment().ThreadPoolSQLActive == false)
            {
                if (sWritingMode == NWDWritingMode.PoolThread)
                {
                    sWritingMode = NWDWritingMode.MainThread;
                }
                else if (sWritingMode == NWDWritingMode.QueuedPoolThread)
                {
                    sWritingMode = NWDWritingMode.QueuedMainThread;
                }
            }
            // Compare with actual State of object and force to follow the actual state
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
                this.AddonUpdateMe(); // call override method
                UpdateDataOperation(sAutoDate);
                this.AddonUpdatedMe(); // call override method
                UpdateObjectInListOfEdition(this);
                WritingLockAdd();
                WritingPending = NWDWritingPending.UpdateInMemory;
                NWDDataManager.SharedInstance().UpdateData(this, sWritingMode);
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateDataOperation(bool sAutoDate = true)
        {
            //BTBBenchmark.Start();
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
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateDataProceed()
        {
            //BTBBenchmark.Start();
            if (AccountDependent())
            {
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.Update(this);
            }
            else
            {
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.Update(this);
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateDataProceedWithTransaction()
        {
            //BTBBenchmark.Start();
            if (AccountDependent())
            {
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.BeginTransaction();
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.Update(this);
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.Commit();
            }
            else
            {
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.BeginTransaction();
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.Update(this);
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.Commit();
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateDataFinish()
        {
            //BTBBenchmark.Start();
            WritingLockRemove();
            WritingPending = NWDWritingPending.InDatabase;
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool UpdateDataIfModified(bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.MainThread)
        {
            //BTBBenchmark.Start();
            bool tReturn = false;
            if (this.Integrity != this.IntegrityValue())
            {
                tReturn = true;
                UpdateData(sAutoDate, sWritingMode);
            }
            //BTBBenchmark.Finish();
            return tReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void EnableData(NWDWritingMode sWritingMode = NWDWritingMode.MainThread)
        {
            //BTBBenchmark.Start();
            this.AC = true;
            this.AddonEnableMe(); // call override method
            this.UpdateData(true, sWritingMode);
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DisableData(NWDWritingMode sWritingMode = NWDWritingMode.MainThread)
        {
            //BTBBenchmark.Start();
            this.DD = NWDToolbox.Timestamp();
            this.AC = false;
            this.AddonDisableMe(); // call override method
            this.UpdateData(true, sWritingMode);
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void TrashData(NWDWritingMode sWritingMode = NWDWritingMode.MainThread)
        {
            //BTBBenchmark.Start();
            int tTimestamp = NWDToolbox.Timestamp();
            this.DD = tTimestamp;
            this.XX = tTimestamp;
            this.AC = false;
            this.AddonTrashMe(); // call override method
            this.UpdateData(true, sWritingMode);
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UnTrashData(NWDWritingMode sWritingMode = NWDWritingMode.MainThread)
        {
            //BTBBenchmark.Start();
            this.XX = 0;
            this.AC = false;
            this.AddonUnTrashMe(); // call override method
            this.UpdateData(true, sWritingMode);
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion Update Data
        #region Delete Data
        //-------------------------------------------------------------------------------------------------------------
        public void DeleteData(NWDWritingMode sWritingMode = NWDWritingMode.MainThread)
        {
            //BTBBenchmark.Start();
            // Verif if Systeme can use the thread (option in Environment)
            if (NWDAppEnvironment.SelectedEnvironment().ThreadPoolSQLActive == false)
            {
                if (sWritingMode == NWDWritingMode.PoolThread)
                {
                    sWritingMode = NWDWritingMode.MainThread;
                }
                else if (sWritingMode == NWDWritingMode.QueuedPoolThread)
                {
                    sWritingMode = NWDWritingMode.QueuedMainThread;
                }
            }
            // Compare with actual State of object and force to follow the actual state
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
                this.AddonDeleteMe(); // call override method
                DeleteDataOperation();
                RemoveObjectInListOfEdition(this);
                WritingLockAdd();
                WritingPending = NWDWritingPending.DeleteInMemory;
                NWDDataManager.SharedInstance().DeleteData(this, sWritingMode);
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DeleteDataOperation()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DeleteDataProceed()
        {
            //BTBBenchmark.Start();
            if (AccountDependent())
            {
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.Delete(this);
            }
            else
            {
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.Delete(this);
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DeleteDataProceedWithTransaction()
        {
            //BTBBenchmark.Start();
            if (AccountDependent())
            {
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.BeginTransaction();
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.Delete(this);
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.Commit();
            }
            else
            {
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.BeginTransaction();
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.Delete(this);
                NWDDataManager.SharedInstance().SQLiteConnectionEditor.Commit();
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DeleteDataFinish()
        {
            //BTBBenchmark.Start();
            WritingLockRemove();
            WritingPending = NWDWritingPending.InDatabase;
#if UNITY_EDITOR
            RepaintTableEditor();
#endif
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion Update Data
        #region Load Data
        //-------------------------------------------------------------------------------------------------------------
        public void LoadedFromDatabase()
        {
            //BTBBenchmark.Start();
            FromDatabase = true;
            WritingPending = NWDWritingPending.InDatabase;
            AddObjectInListOfEdition(this);
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void LoadAllDatas()
        {
            //BTBBenchmark.Start();
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
#if UNITY_EDITOR
            RepaintTableEditor();
#endif
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion Load Data
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================