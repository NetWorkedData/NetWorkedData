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
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
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
                rReturnObject.PropertiesAutofill();
                rReturnObject.Initialization();
                rReturnObject.InsertData(sAutoDate, sWritingMode);
            }
            else
            {
                Debug.LogWarning("ClassType() is null for " + Datas().ClassNamePHP);
            }
            //BTBBenchmark.Finish();
            return rReturnObject as K;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void PropertiesAutofill()
        {
            foreach (PropertyInfo tPropInfo in PropertiesAccountDependent())
            {
                //Debug.Log("try to insert automatically the account reference in the NWDAccount connection property : " + tPropInfo.Name);
                NWDReferenceType<NWDAccount> tAtt = new NWDReferenceType<NWDAccount>();
                tAtt.Value = NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference;
                tPropInfo.SetValue(this, tAtt, null);
            }
            if (Datas().ClassGameSaveDependent == true)
            {
                NWDReferenceType<NWDGameSave> tAtt = new NWDReferenceType<NWDGameSave>();
                tAtt.SetReference(NWDGameSave.Current().Reference);
                PropertyInfo tPropInfo = Datas().ClassGameDependentProperties;
                tPropInfo.SetValue(this, tAtt, null);
            }
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
                    rReturnObject.PropertiesAutofill();
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
                    while (NWDBasis<K>.NEW_InternalKeyExists(tCopy) == true)
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
                    Debug.LogWarning("ClassType() is null for " + Datas().ClassNamePHP);
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
        //public static bool InternalKeyExists(string sInternalKey)
        //{
        //    //BTBBenchmark.Start();
        //    bool rReturn = false;
        //    K[] tArray = GetAllObjects(null);
        //    foreach (K tObject in tArray)
        //    {
        //        if (tObject.InternalKey == sInternalKey)
        //        {
        //            rReturn = true;
        //            break;
        //        }
        //    }
        //    //BTBBenchmark.Finish();
        //    return rReturn;
        //}
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
            if (NWDAppEnvironment.SelectedEnvironment().ThreadPoolForce == false)
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
                if (Datas().DatasByReference.ContainsKey(this.Reference) == false)
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
                        Datas().AddData(this);
                        //AddObjectInListOfEdition(this);
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

            //NWDVersionType tMaxVersion = new NWDVersionType();
            //tMaxVersion.SetString("99.99.99");
            //this.MaxVersion = tMaxVersion;
            this.WebServiceVersion = WebServiceVersionToUse();
            this.DS = 0;
            if (AccountDependent() == true)
            {
                if (NWDAppConfiguration.SharedInstance().IsDevEnvironement() == true)
                {
                    DevSync = 0;
                    PreprodSync = -1;
                    ProdSync = -1;
                }
                else if (NWDAppConfiguration.SharedInstance().IsPreprodEnvironement() == true)
                {
                    DevSync = -1;
                    PreprodSync = 0;
                    ProdSync = -1;
                }
                else if (NWDAppConfiguration.SharedInstance().IsProdEnvironement() == true)
                {
                    DevSync = -1;
                    PreprodSync = -1;
                    ProdSync = 0;
                }
            }
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
        /// <summary>
        /// Save data.
        /// </summary>
        /// <param name="sWritingMode">S writing mode.</param>
        public void SaveData(NWDWritingMode sWritingMode = NWDWritingMode.MainThread)
        {
            UpdateData(true, sWritingMode);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Save data if modified.
        /// </summary>
        /// <returns><c>true</c>, if data if modified : save, <c>false</c> otherwise.</returns>
        /// <param name="sWritingMode">S writing mode.</param>
        public bool SaveDataIfModified(NWDWritingMode sWritingMode = NWDWritingMode.MainThread)
        {
            bool tReturn = false;
            if (this.Integrity != this.IntegrityValue())
            {
                tReturn = true;
                UpdateData(true, sWritingMode);
            }
            return tReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateData(bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.MainThread, bool sWebServiceUpgrade = true)
        {
            //BTBBenchmark.Start();
            // Verif if Systeme can use the thread (option in Environment)
            if (NWDAppEnvironment.SelectedEnvironment().ThreadPoolForce == false)
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
                UpdateDataOperation(sAutoDate, sWebServiceUpgrade);
                this.AddonUpdatedMe(); // call override method

                Datas().UpdateData(this);
                //UpdateObjectInListOfEdition(this);
                WritingLockAdd();
                WritingPending = NWDWritingPending.UpdateInMemory;
                NWDDataManager.SharedInstance().UpdateData(this, sWritingMode);
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateDataOperation(bool sAutoDate = true, bool sWebServiceUpgrade = true)
        {
            //BTBBenchmark.Start();
            // so object is prepared to be update
            if (sAutoDate == true)
            {
                this.DM = NWDToolbox.Timestamp();
            }
            // this.DS = 0;
            // prevent DevSync lock
            if (this.DevSync > 0)
            {
                this.DevSync = 1;
            }
            // prevent PreprodSync lock
            if (this.PreprodSync > 0)
            {
                this.PreprodSync = 1;
            }
            // prevent ProdSync lock
            if (this.ProdSync > 0)
            {
                this.ProdSync = 1;
            }
            // reset Hash server
            this.ServerHash = "";
            // Update WebServiceVersion
            if (sWebServiceUpgrade == true)
            {
                int tWS = WebServiceVersionToUse();
                if (this.WebServiceVersion != tWS)
                {
                    this.AddonVersionMe(); // call override method
                    this.WebServiceVersion = tWS;
                }
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
            if (NWDAppEnvironment.SelectedEnvironment().ThreadPoolForce == false)
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
                Datas().RemoveData(this);
                //RemoveObjectInListOfEdition(this);
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
            InDatabase = true;
            FromDatabase = true;
            WritingPending = NWDWritingPending.InDatabase;
            AddonLoadedMe();
            Datas().AddData(this);
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
//        public static void LoadAllDatas()
//        {
//            //BTBBenchmark.Start();
//            SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
//            if (AccountDependent())
//            {
//                tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
//            }
//            IEnumerable tEnumerable = tSQLiteConnection.Table<K>().OrderBy(x => x.InternalKey);
//            Datas().ObjectsList = new List<object>();
//            Datas().ObjectsByReferenceList = new List<string>();
//            Datas().ObjectsByKeyList = new List<string>();
//#if UNITY_EDITOR
//            Datas().DatasInEditorRowDescriptionList = new List<string>();
//            Datas().DatasInEditorSelectionList = new List<bool>();
//            Datas().DatasInEditorReferenceList = new List<string>();
//#endif
//            if (tEnumerable != null)
//            {
//                foreach (NWDBasis<K> tData in tEnumerable)
//                {
//                    tData.LoadedFromDatabase();
//                }
//            }
//#if UNITY_EDITOR
//            RepaintTableEditor();
//#endif
        //    //BTBBenchmark.Finish();
        //}
        //-------------------------------------------------------------------------------------------------------------
        #endregion Load Data
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================