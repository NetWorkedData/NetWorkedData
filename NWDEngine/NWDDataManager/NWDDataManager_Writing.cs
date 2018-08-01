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
            //BTBBenchmark.Start();
            bool rReturn = false;
            if (kDataInWriting.Count > 0)
            {
                rReturn = true;
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Execute Queues
        /// </summary>
        public int DataQueueCounter()
        {
            //BTBBenchmark.Start();
            int rResult = kDataInWriting.Count;
            //BTBBenchmark.Finish();
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Execute Queues
        /// </summary>
        public void DataQueueExecute()
        {
            //BTBBenchmark.Start();
            if (DataInWritingProcess())
            {
                DataQueueMainExecute();
                DataQueuePoolExecute();
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Execute the Queue on main thread
        /// </summary>
        public void DataQueueMainExecute()
        {
            //BTBBenchmark.Start();
            if (DataInWritingProcess())
            {
                List<object> tInsertDataQueueMain = new List<object>(kInsertDataQueueMain);
                List<object> tUpdateDataQueueMain = new List<object>(kUpdateDataQueueMain);
                List<object> tDeleteDataQueueMain = new List<object>(kDeleteDataQueueMain);
                kInsertDataQueueMain = new List<object>();
                kUpdateDataQueueMain = new List<object>();
                kDeleteDataQueueMain = new List<object>();
                InsertDataQueueExecute(tInsertDataQueueMain);
                UpdateDataQueueExecute(tUpdateDataQueueMain);
                DeleteDataQueueExecute(tDeleteDataQueueMain);
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Execute the Queue on pool thread
        /// </summary>
        public void DataQueuePoolExecute()
        {
            //BTBBenchmark.Start();
            if (DataInWritingProcess())
            {
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
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Execute the Queue on pool thread by ThreadPool.QueueUserWorkItem()
        /// </summary>
        private void DataQueuePoolThread(object sState)
        {
            //BTBBenchmark.Start();
            object[] tParam = sState as object[];
            List<object> tInsertDataQueue = tParam[0] as List<object>;
            List<object> tUpdateDataQueue = tParam[1] as List<object>;
            List<object> tDeleteDataQueue = tParam[2] as List<object>;
            InsertDataQueueExecute(tInsertDataQueue);
            UpdateDataQueueExecute(tUpdateDataQueue);
            DeleteDataQueueExecute(tDeleteDataQueue);
            //BTBBenchmark.Finish();
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
                        //Debug.Log("NWDDataManager InsertData() MainThread");
                        InsertDataExecute(sObject);
                    }
                    break;
                case NWDWritingMode.PoolThread:
                    {
                        //Debug.Log("NWDDataManager InsertData() PoolThread");
#if UNITY_EDITOR
                        RepaintWindowForData(sObject.GetType());
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
                            RepaintWindowForData(sObject.GetType());
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
            //Debug.Log("InsertDataQueueExecute with " + sInsertDataQueuePool.Count + " Object(s)");
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
                        //Debug.Log("NWDDataManager UpdateData() MainThread");
                        UpdateDataExecute(sObject);
                        BTBNotificationManager.SharedInstance().PostNotification(sObject, NWDNotificationConstants.K_DATA_LOCAL_UPDATE);
                    }
                    break;
                case NWDWritingMode.PoolThread:
                    {
                        //Debug.Log("NWDDataManager UpdateData() PoolThread");
#if UNITY_EDITOR
                        RepaintWindowForData(sObject.GetType());
#endif
                        ThreadPool.QueueUserWorkItem(UpdateDataExecute, sObject);

                    }
                    break;
                case NWDWritingMode.QueuedMainThread:
                    {
                        //Debug.Log("NWDDataManager UpdateData() QueuedMainThread");
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
                        //Debug.Log("NWDDataManager UpdateData() QueuedPoolThread");
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
            //Debug.Log("UpdateDataQueueExecute with " + sUpdateDataQueuePool.Count + " Object(s)");
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
                        //Debug.Log("NWDDataManager DeleteData() MainThread");
                        DeleteDataExecute(sObject);
                        BTBNotificationManager.SharedInstance().PostNotification(sObject, NWDNotificationConstants.K_DATA_LOCAL_DELETE);
                    }
                    break;
                case NWDWritingMode.PoolThread:
                    {
                        //Debug.Log("NWDDataManager DeleteData() PoolThread");
#if UNITY_EDITOR
                        RepaintWindowForData(sObject.GetType());
#endif
                        ThreadPool.QueueUserWorkItem(DeleteDataExecute, sObject);
                    }
                    break;
                case NWDWritingMode.QueuedMainThread:
                    {
                        //Debug.Log("NWDDataManager DeleteData() QueuedMainThread");
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
                        //Debug.Log("NWDDataManager DeleteData() QueuedPoolThread");
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
            //Debug.Log("DeleteDataQueueExecute with " + sDeleteDataQueuePool.Count + " Object(s)");
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
}
//=====================================================================================================================