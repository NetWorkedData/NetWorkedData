//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis : NWDTypeClass
    {
        //-------------------------------------------------------------------------------------------------------------
        //public override void PropertiesAutofill()
        //{
        //    // if account dependent then insert the account reference value in all account depedent properties
        //    //NWDBasisHelper tHelper = BasisHelper();
        //    //foreach (PropertyInfo tPropInfo in tHelper.kAccountDependentProperties)
        //    //{
        //    //    NWDReferenceType<NWDAccount> tAtt = new NWDReferenceType<NWDAccount>();
        //    //    tAtt.Value = NWDAccount.CurrentReference();
        //    //    tPropInfo.SetValue(this, tAtt, null);
        //    //}
        //    //// if gamesave dependent then insert the gamesave reference in the good property 
        //    //if (tHelper.ClassGameSaveDependent == true)
        //    //{
        //    //    NWDReferenceType<NWDGameSave> tAtt = new NWDReferenceType<NWDGameSave>();
        //    //    if (NWDGameSave.CurrentData() != null)
        //    //    {
        //    //        tAtt.SetReference(NWDGameSave.CurrentData().Reference);
        //    //    }
        //    //    PropertyInfo tPropInfo = tHelper.ClassGameDependentProperties;
        //    //    tPropInfo.SetValue(this, tAtt, null);
        //    //}
        //}
        //-------------------------------------------------------------------------------------------------------------
        #region Duplicate Data
        //-------------------------------------------------------------------------------------------------------------
        public override NWDTypeClass Base_DuplicateData(bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            return NWDBasisHelper.DuplicateData(this, sAutoDate, sWritingMode) as NWDTypeClass;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Copy the data.
        /// </summary>
        /// <param name="sOriginal">S original.</param>
        public override void CopyData(NWDTypeClass sOriginal)
        {
            //NWEBenchmark.Start();
            string[] tKey = BasisHelper().SLQAssemblyOrderArray();
            Type tType = ClassType();
            foreach (string tPropertyString in tKey)
            {
                PropertyInfo tPropertyInfo = tType.GetProperty(tPropertyString, BindingFlags.Public | BindingFlags.Instance);
                Type tTypeOfThis = tPropertyInfo.PropertyType;
                object tValue = tPropertyInfo.GetValue(sOriginal, null);
                if (tValue != null)
                {
                    string tValueString = string.Copy(tValue.ToString()); // force to copy new object
                    if (tTypeOfThis.IsEnum)
                    {
                        tPropertyInfo.SetValue(this, tValue, null);
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataType)))
                    {
                        NWEDataType tTemp = Activator.CreateInstance(tTypeOfThis) as NWEDataType;
                        tTemp.Value = ((NWEDataType)tValue).Value;
                        tPropertyInfo.SetValue(this, tTemp, null);
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeInt)))
                    {
                        NWEDataTypeInt tTemp = Activator.CreateInstance(tTypeOfThis) as NWEDataTypeInt;
                        tTemp.Value = ((NWEDataTypeInt)tValue).Value;
                        tPropertyInfo.SetValue(this, tTemp, null);
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeFloat)))
                    {
                        NWEDataTypeFloat tTemp = Activator.CreateInstance(tTypeOfThis) as NWEDataTypeFloat;
                        tTemp.Value = ((NWEDataTypeFloat)tValue).Value;
                        tPropertyInfo.SetValue(this, tTemp, null);
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeEnum)))
                    {
                        NWEDataTypeEnum tTemp = Activator.CreateInstance(tTypeOfThis) as NWEDataTypeEnum;
                        tTemp.Value = ((NWEDataTypeEnum)tValue).Value;
                        tPropertyInfo.SetValue(this, tTemp, null);
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeMask)))
                    {
                        NWEDataTypeMask tTemp = Activator.CreateInstance(tTypeOfThis) as NWEDataTypeMask;
                        tTemp.Value = ((NWEDataTypeMask)tValue).Value;
                        tPropertyInfo.SetValue(this, tTemp, null);
                    }
                    else if (tTypeOfThis == typeof(String) || tTypeOfThis == typeof(string))
                    {
                        tPropertyInfo.SetValue(this, tValueString, null);
                    }
                    else if (tTypeOfThis == typeof(bool))
                    {
                        bool tValueInsert = false;
                        int tTemp;
                        int.TryParse(tValueString, out tTemp);
                        if (tTemp > 0)
                        {
                            tValueInsert = true;
                        }
                        tPropertyInfo.SetValue(this, tValueInsert, null);
                    }
                    else if (tTypeOfThis == typeof(int))
                    {
                        int tTemp;
                        int.TryParse(tValueString, out tTemp);
                        tPropertyInfo.SetValue(this, tTemp, null);
                    }
                    else if (tTypeOfThis == typeof(long))
                    {
                        long tTemp;
                        long.TryParse(tValueString, out tTemp);
                        tPropertyInfo.SetValue(this, tTemp, null);
                    }
                    else if (tTypeOfThis == typeof(Int16))
                    {
                        Int16 tTemp;
                        Int16.TryParse(tValueString, out tTemp);
                        tPropertyInfo.SetValue(this, tTemp, null);
                    }
                    else if (tTypeOfThis == typeof(Int32))
                    {
                        Int32 tTemp;
                        Int32.TryParse(tValueString, out tTemp);
                        tPropertyInfo.SetValue(this, tTemp, null);
                    }
                    else if (tTypeOfThis == typeof(Int64))
                    {
                        Int64 tTemp;
                        Int64.TryParse(tValueString, out tTemp);
                        tPropertyInfo.SetValue(this, tTemp, null);
                    }
                    else if (tTypeOfThis == typeof(float))
                    {
                        float tTemp;
                        float.TryParse(tValueString, out tTemp);
                        tPropertyInfo.SetValue(this, tTemp, null);
                    }
                    else if (tTypeOfThis == typeof(double))
                    {
                        double tTemp;
                        double.TryParse(tValueString, out tTemp);
                        tPropertyInfo.SetValue(this, tTemp, null);
                    }
                    else if (tTypeOfThis == typeof(Single))
                    {
                        Single tTemp;
                        Single.TryParse(tValueString, out tTemp);
                        tPropertyInfo.SetValue(this, tTemp, null);
                    }
                    else if (tTypeOfThis == typeof(Double))
                    {
                        Double tTemp;
                        Double.TryParse(tValueString, out tTemp);
                        tPropertyInfo.SetValue(this, tTemp, null);
                    }
                    else if (tTypeOfThis == typeof(Decimal))
                    {
                        Decimal tTemp;
                        Decimal.TryParse(tValueString, out tTemp);
                        tPropertyInfo.SetValue(this, tTemp, null);
                    }
                    else
                    {
                    }
                }
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion Duplicate Data
        #region Insert Data
        //-------------------------------------------------------------------------------------------------------------
        public override bool InsertData(bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            //NWEBenchmark.Start();
            sWritingMode = NWDAppConfiguration.WritingMode(sWritingMode);
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
                if (this.Reference != null)
                {
                    if (BasisHelper().DatasByReference.ContainsKey(this.Reference) == false)
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
                            BasisHelper().AddData(this);

                            this.AddonInsertedMe();
                            //AddObjectInListOfEdition(this);
                            WritingLockAdd();
                            WritingPending = NWDWritingPending.InsertInMemory;
                            NWDDataManager.SharedInstance().InsertData(this, sWritingMode);
                        }
                    }
                    else
                    {
                        // error this reference already exist
                        UpdateDataIfModified(sAutoDate, sWritingMode);
                    }
                }
            }
            //NWEBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InsertDataOperation(bool sAutoDate = true)
        {
            //NWEBenchmark.Start();
#if UNITY_INCLUDE_TESTS
            if (NWDUnitTests.IsTest())
            {
                Tag = NWDBasisTag.UnitTestToDelete;
            }
#endif
            if (sAutoDate == true)
            {
                this.DC = NWDToolbox.Timestamp();
                this.DM = NWDToolbox.Timestamp();
            }
            WebModel = WebModelToUse();
            this.DS = 0;
            //if (AccountDependent() == true)
            if (BasisHelper().TemplateHelper.GetAccountDependent() != NWDTemplateAccountDependent.NoAccountDependent)
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
            ServerHash = string.Empty;
            ServerLog = string.Empty;
            UpdateIntegrity();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void InsertDataProceed()
        {
            //NWEBenchmark.Start();
            // Debug.Log("NWDBasis InsertDataProceed()");
            //if (AccountDependent())
            //if (BasisHelper().TemplateHelper.GetAccountDependent() != NWDTemplateAccountDependent.NoAccountDependent)
            if (BasisHelper().TemplateHelper.GetDeviceDatabase() == NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseAccount)
            {
                if (NWDDataManager.SharedInstance().SQLiteConnectionAccountIsValid())
                {
                    IntPtr stmt = SQLite3.Prepare2(NWDDataManager.SharedInstance().SQLiteDeviceHandle, New_SQLInsertOrReplace());
                    SQLite3.Step(stmt);
                    SQLite3.Finalize(stmt);
                }
            }
            else if (BasisHelper().TemplateHelper.GetDeviceDatabase() == NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseEditor)
            {
                if (NWDDataManager.SharedInstance().SQLiteConnectionEditorIsValid())
                {
                    IntPtr stmt = SQLite3.Prepare2(NWDDataManager.SharedInstance().SQLiteEditorHandle, New_SQLInsertOrReplace());
                    SQLite3.Step(stmt);
                    SQLite3.Finalize(stmt);
                }
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void InsertDataProceedWithTransaction()
        {
            //NWEBenchmark.Start();
            Debug.Log("NWDBasis InsertDataProceedWithTransaction()");
            //if (AccountDependent())
            //if (BasisHelper().TemplateHelper.GetAccountDependent() != NWDTemplateAccountDependent.NoAccountDependent)
            if (BasisHelper().TemplateHelper.GetDeviceDatabase() == NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseAccount)
            {
                if (NWDDataManager.SharedInstance().SQLiteConnectionAccountIsValid())
                {
                    IntPtr stmt = SQLite3.Prepare2(NWDDataManager.SharedInstance().SQLiteDeviceHandle, "BEGIN TRANSACTION");
                    SQLite3.Step(stmt);
                    SQLite3.Finalize(stmt);
                    stmt = SQLite3.Prepare2(NWDDataManager.SharedInstance().SQLiteDeviceHandle, New_SQLInsertOrReplace());
                    SQLite3.Step(stmt);
                    SQLite3.Finalize(stmt);
                    stmt = SQLite3.Prepare2(NWDDataManager.SharedInstance().SQLiteDeviceHandle, "COMMIT");
                    SQLite3.Step(stmt);
                    SQLite3.Finalize(stmt);
                }
            }
            else if (BasisHelper().TemplateHelper.GetDeviceDatabase() == NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseEditor)
            {
                if (NWDDataManager.SharedInstance().SQLiteConnectionEditorIsValid())
                {
                    IntPtr stmt = SQLite3.Prepare2(NWDDataManager.SharedInstance().SQLiteEditorHandle, "BEGIN TRANSACTION");
                    SQLite3.Step(stmt);
                    SQLite3.Finalize(stmt);
                    stmt = SQLite3.Prepare2(NWDDataManager.SharedInstance().SQLiteEditorHandle, New_SQLInsertOrReplace());
                    SQLite3.Step(stmt);
                    SQLite3.Finalize(stmt);
                    stmt = SQLite3.Prepare2(NWDDataManager.SharedInstance().SQLiteEditorHandle, "COMMIT");
                    SQLite3.Step(stmt);
                    SQLite3.Finalize(stmt);
                }
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void InsertDataFinish()
        {
            //NWEBenchmark.Start();
            WritingLockRemove();
            WritingPending = NWDWritingPending.InDatabase;
#if UNITY_EDITOR
            RowAnalyze();
#endif 
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion Insert Data
        #region Update Data
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Save data.
        /// </summary>
        /// <param name="sWritingMode">S writing mode.</param>
        public void SaveData(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            UpdateData(true, sWritingMode);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Save data if modified.
        /// </summary>
        /// <returns><c>true</c>, if data if modified : save, <c>false</c> otherwise.</returns>
        /// <param name="sWritingMode">S writing mode.</param>
        public bool SaveDataIfModified(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
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
        [Obsolete("Use UpdateData()")]
        public void UpdateDataLater()
        {
            UpdateData(true, NWDWritingMode.QueuedMainThread, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        //[Obsolete("Use UpdateData()")]
        public void UpdateData(NWDWritingMode sWritingMode)
        {
            UpdateData(true, sWritingMode, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void UpdateDataEditor()
        {
            DM = NWDToolbox.Timestamp();
            UpdateIntegrity();
            UpdateData(true, NWDWritingMode.ByEditorDefault);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void UpdateData(bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.MainThread, bool sWebServiceUpgrade = true, bool sWithCallBack = true)
        {
            //NWEBenchmark.Start();
            // Determine the default mode
            sWritingMode = NWDAppConfiguration.WritingMode(sWritingMode);
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
            if (sWithCallBack)
            {
                this.AddonUpdateMe(); // call override method
            }
            UpdateDataOperation(sAutoDate, sWebServiceUpgrade);
            if (sWithCallBack)
            {
                this.AddonUpdatedMe(); // call override method
            }
            this.IndexInBase();
            this.IndexInMemory();
            BasisHelper().UpdateData(this);
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
                //UpdateObjectInListOfEdition(this);
                WritingLockAdd();
                WritingPending = NWDWritingPending.UpdateInMemory;
                NWDDataManager.SharedInstance().UpdateData(this, sWritingMode);
            }

#if UNITY_EDITOR
            RowAnalyze();
            NWDDataManager.SharedInstance().RepaintWindowsInManager(this.GetType());
            NWDNodeEditor.ReAnalyzeIfNecessary(this);
#endif
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void UpdateDataOperation(bool sAutoDate = true, bool sWebServiceUpgrade = true)
        {
            //NWEBenchmark.Start();
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
            this.ServerHash = string.Empty;
            // Update WebServiceVersion
            if (sWebServiceUpgrade == true)
            {
                int tWS = WebModelToUse();
                if (this.WebModel != tWS)
                {
                    //Debug.Log(" set from " + WebModel + " To " + tWS);
                    this.WebModel = tWS;
                }
            }
            UpdateIntegrity();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void UpdateDataProceed()
        {
            //NWEBenchmark.Start();
            //Debug.Log("NWDBasis UpdateDataProceed()");
            //if (AccountDependent())
            //if (BasisHelper().TemplateHelper.GetAccountDependent() != NWDTemplateAccountDependent.NoAccountDependent)
            if (BasisHelper().TemplateHelper.GetDeviceDatabase() == NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseAccount)
            {
                if (NWDDataManager.SharedInstance().SQLiteConnectionAccountIsValid())
                {
                    IntPtr stmt = SQLite3.Prepare2(NWDDataManager.SharedInstance().SQLiteDeviceHandle, New_SQLInsertOrReplace());
                    SQLite3.Step(stmt);
                    SQLite3.Finalize(stmt);
                }
            }
            else if (BasisHelper().TemplateHelper.GetDeviceDatabase() == NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseEditor)
            {
                if (NWDDataManager.SharedInstance().SQLiteConnectionEditorIsValid())
                {
                    IntPtr stmt = SQLite3.Prepare2(NWDDataManager.SharedInstance().SQLiteEditorHandle, New_SQLInsertOrReplace());
                    SQLite3.Step(stmt);
                    SQLite3.Finalize(stmt);
                }
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void UpdateDataProceedWithTransaction()
        {
            //NWEBenchmark.Start();
            //Debug.Log("NWDBasis UpdateDataProceedWithTransaction()");
            //if (AccountDependent())
            //if (BasisHelper().TemplateHelper.GetAccountDependent() != NWDTemplateAccountDependent.NoAccountDependent)
            if (BasisHelper().TemplateHelper.GetDeviceDatabase() == NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseAccount)
            {
                if (NWDDataManager.SharedInstance().SQLiteConnectionAccountIsValid())
                {
                    IntPtr stmt = SQLite3.Prepare2(NWDDataManager.SharedInstance().SQLiteDeviceHandle, "BEGIN TRANSACTION");
                    SQLite3.Step(stmt);
                    SQLite3.Finalize(stmt);
                    stmt = SQLite3.Prepare2(NWDDataManager.SharedInstance().SQLiteDeviceHandle, New_SQLInsertOrReplace());
                    SQLite3.Step(stmt);
                    SQLite3.Finalize(stmt);
                    stmt = SQLite3.Prepare2(NWDDataManager.SharedInstance().SQLiteDeviceHandle, "COMMIT");
                    SQLite3.Step(stmt);
                    SQLite3.Finalize(stmt);
                }
            }
            else if (BasisHelper().TemplateHelper.GetDeviceDatabase() == NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseEditor)
            {
                if (NWDDataManager.SharedInstance().SQLiteConnectionEditorIsValid())
                {
                    IntPtr stmt = SQLite3.Prepare2(NWDDataManager.SharedInstance().SQLiteEditorHandle, "BEGIN TRANSACTION");
                    SQLite3.Step(stmt);
                    SQLite3.Finalize(stmt);
                    stmt = SQLite3.Prepare2(NWDDataManager.SharedInstance().SQLiteEditorHandle, New_SQLInsertOrReplace());
                    SQLite3.Step(stmt);
                    SQLite3.Finalize(stmt);
                    stmt = SQLite3.Prepare2(NWDDataManager.SharedInstance().SQLiteEditorHandle, "COMMIT");
                    SQLite3.Step(stmt);
                    SQLite3.Finalize(stmt);
                }
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void UpdateDataFinish()
        {
            //NWEBenchmark.Start();
            //Debug.Log("NWDBasis UpdateDataFinish()");
            WritingLockRemove();
            WritingPending = NWDWritingPending.InDatabase;
#if UNITY_EDITOR
            RowAnalyze();
#endif 
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool UpdateDataIfModifiedWithoutCallBack()
        {
            //NWEBenchmark.Start();
            bool tReturn = false;
            if (this.Integrity != this.IntegrityValue())
            {
                tReturn = true;
                UpdateData(true, NWDWritingMode.ByDefaultLocal, false, false);
            }
            //NWEBenchmark.Finish();
            return tReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool UpdateDataIfModified(bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            //NWEBenchmark.Start();
            bool tReturn = false;
            if (this.Integrity != this.IntegrityValue())
            {
                tReturn = true;
                UpdateData(sAutoDate, sWritingMode);
            }
            //NWEBenchmark.Finish();
            return tReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void EnableData(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            //NWEBenchmark.Start();
            this.AC = true;
            this.AddonEnableMe(); // call override method
            this.UpdateData(true, sWritingMode);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DisableData(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            //NWEBenchmark.Start();
            this.DD = NWDToolbox.Timestamp();
            this.AC = false;
            this.AddonDisableMe(); // call override method
            this.UpdateData(true, sWritingMode);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool TrashData(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            //NWEBenchmark.Start();
            bool rReturn = false;
            if (XX == 0)
            {
                rReturn = true;
                int tTimestamp = NWDToolbox.Timestamp();
                this.DD = tTimestamp;
                this.XX = 1;
                this.AC = false;
                this.AddonTrashMe(); // call override method
                this.UpdateData(true, sWritingMode);
            }
            return rReturn;
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool UnTrashData(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            //NWEBenchmark.Start();
            bool rReturn = false;
            if (XX <= 1)
            {
                rReturn = true;
                this.XX = 0;
                this.AC = false;
                this.AddonUnTrashMe(); // call override method
                this.UpdateData(true, sWritingMode);
            }
            return rReturn;
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion Update Data
        #region Delete Data
        //-------------------------------------------------------------------------------------------------------------
        public override void DeleteData(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            //NWEBenchmark.Start();
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
                this.DeindexInBase(); // call override method
                this.DeindexInMemory(); // call override method
                DeleteDataOperation();
                BasisHelper().RemoveData(this);
                //RemoveObjectInListOfEdition(this);
                WritingLockAdd();
                WritingPending = NWDWritingPending.DeleteInMemory;
                NWDDataManager.SharedInstance().DeleteData(this, sWritingMode);
                this.AddonDeletedMe(); // call override method
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DeleteDataOperation()
        {
            //#if UNITY_INCLUDE_TESTS
            //            Tag = NWDBasisTag.UnitTestToDelete;
            //#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DeleteDataProceed()
        {
            //NWEBenchmark.Start();
            //if (AccountDependent())
            //if (BasisHelper().TemplateHelper.GetAccountDependent() != NWDTemplateAccountDependent.NoAccountDependent)
            if (BasisHelper().TemplateHelper.GetDeviceDatabase() == NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseAccount)
            {
                if (NWDDataManager.SharedInstance().SQLiteConnectionAccountIsValid())
                {
                    IntPtr stmt = SQLite3.Prepare2(NWDDataManager.SharedInstance().SQLiteDeviceHandle, New_SQLDelete());
                    SQLite3.Step(stmt);
                    SQLite3.Finalize(stmt);
                }
            }
            else if (BasisHelper().TemplateHelper.GetDeviceDatabase() == NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseEditor)
            {
                if (NWDDataManager.SharedInstance().SQLiteConnectionEditorIsValid())
                {
                    IntPtr stmt = SQLite3.Prepare2(NWDDataManager.SharedInstance().SQLiteEditorHandle, New_SQLDelete());
                    SQLite3.Step(stmt);
                    SQLite3.Finalize(stmt);
                }
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DeleteDataProceedWithTransaction()
        {
            //NWEBenchmark.Start();
            //Debug.Log("NWDBasis DeleteDataProceedWithTransaction()");
            //if (AccountDependent())
            //if (BasisHelper().TemplateHelper.GetAccountDependent() != NWDTemplateAccountDependent.NoAccountDependent)
            if (BasisHelper().TemplateHelper.GetDeviceDatabase() == NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseAccount)
            {
                if (NWDDataManager.SharedInstance().SQLiteConnectionAccountIsValid())
                {
                    IntPtr stmt = SQLite3.Prepare2(NWDDataManager.SharedInstance().SQLiteDeviceHandle, "BEGIN TRANSACTION");
                    SQLite3.Step(stmt);
                    SQLite3.Finalize(stmt);
                    stmt = SQLite3.Prepare2(NWDDataManager.SharedInstance().SQLiteDeviceHandle, New_SQLDelete());
                    SQLite3.Step(stmt);
                    SQLite3.Finalize(stmt);
                    stmt = SQLite3.Prepare2(NWDDataManager.SharedInstance().SQLiteDeviceHandle, "COMMIT");
                    SQLite3.Step(stmt);
                    SQLite3.Finalize(stmt);
                }
            }
            else if (BasisHelper().TemplateHelper.GetDeviceDatabase() == NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseEditor)
            {
                if (NWDDataManager.SharedInstance().SQLiteConnectionEditorIsValid())
                {
                    IntPtr stmt = SQLite3.Prepare2(NWDDataManager.SharedInstance().SQLiteEditorHandle, "BEGIN TRANSACTION");
                    SQLite3.Step(stmt);
                    SQLite3.Finalize(stmt);
                    stmt = SQLite3.Prepare2(NWDDataManager.SharedInstance().SQLiteEditorHandle, New_SQLDelete());
                    SQLite3.Step(stmt);
                    SQLite3.Finalize(stmt);
                    stmt = SQLite3.Prepare2(NWDDataManager.SharedInstance().SQLiteEditorHandle, "COMMIT");
                    SQLite3.Step(stmt);
                    SQLite3.Finalize(stmt);
                }
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DeleteDataFinish()
        {
            //NWEBenchmark.Start();
            WritingLockRemove();
            WritingPending = NWDWritingPending.InDatabase;
#if UNITY_EDITOR
            RowAnalyze();
#endif 
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion Update Data
        #region Load Data

        public override void AnalyzeData()
        {

#if UNITY_EDITOR
            ErrorCheck();
#endif
            WebserviceVersionCheckMe();
#if UNITY_EDITOR
            RowAnalyze();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void LoadedFromDatabase()
        {
            //NWEBenchmark.Start();
            InDatabase = true;
            FromDatabase = true;
            WritingPending = NWDWritingPending.InDatabase;
            //AddonLoadedMe();
#if UNITY_EDITOR

#else
            WebserviceVersionCheckMe();
#endif
            //ReIndex();
            BasisHelper().AddData(this);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion Load Data
        //-------------------------------------------------------------------------------------------------------------
        public string New_SQLInsertOrReplace()
        {
            if (string.IsNullOrEmpty(Reference))
            {
                return string.Empty;
            }
            NWDBasisHelper tHelper = BasisHelper();
            List<string> tKeys = new List<string>();
            List<string> tValues = new List<string>();
            foreach (PropertyInfo tProp in tHelper.ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                tKeys.Add(tProp.Name);

                Type tTypeOfThis = tProp.PropertyType;
                string tValueString = string.Empty;
                object tValue = tProp.GetValue(this, null);
                if (tValue == null)
                {
                    if (tTypeOfThis.IsEnum)
                    {
                        tValueString = "0";
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataType)))
                    {
                        tValueString = string.Empty;
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeInt)) ||
                        tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeFloat)) ||
                        tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeEnum)) ||
                        tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeMask)))
                    {
                        tValueString = "0";
                    }
                    else if (tTypeOfThis == typeof(String) ||
                        tTypeOfThis == typeof(string))
                    {
                        tValueString = string.Empty;
                    }
                    else if (tTypeOfThis == typeof(bool))
                    {
                        tValueString = NWDToolbox.BoolToIntString(false);
                    }
                    else if (tTypeOfThis == typeof(int) ||
                        tTypeOfThis == typeof(long) ||
                        tTypeOfThis == typeof(float) ||
                        tTypeOfThis == typeof(double))
                    {
                        tValueString = "0";
                    }
                    else
                    {
                        tValueString = string.Empty;
                    }
                }
                else
                {
                    if (tTypeOfThis.IsEnum)
                    {
                        int tInt = (int)tValue;
                        tValueString = tInt.ToString();
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataType)))
                    {
                        tValueString = tValue.ToString().Replace("\"", "\"\"");
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeInt)))
                    {
                        tValueString = NWDToolbox.LongToString(((NWEDataTypeInt)tValue).Value);
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeFloat)))
                    {
                        tValueString = NWDToolbox.DoubleToString(((NWEDataTypeFloat)tValue).Value);
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeEnum)))
                    {
                        tValueString = NWDToolbox.LongToString(((NWEDataTypeEnum)tValue).Value);
                    }
                    else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeMask)))
                    {
                        tValueString = NWDToolbox.LongToString(((NWEDataTypeMask)tValue).Value);
                    }
                    else if (tTypeOfThis == typeof(String) || tTypeOfThis == typeof(string))
                    {
                        tValueString = tValue.ToString().Replace("\"", "\"\"");
                    }
                    else if (tTypeOfThis == typeof(bool))
                    {
                        tValueString = NWDToolbox.BoolToIntString((bool)tValue);
                    }
                    else if (tTypeOfThis == typeof(int))
                    {
                        tValueString = NWDToolbox.IntToString((int)tValue);
                    }
                    else if (tTypeOfThis == typeof(long))
                    {
                        tValueString = NWDToolbox.LongToString((long)tValue);
                    }
                    else if (tTypeOfThis == typeof(float))
                    {
                        tValueString = NWDToolbox.FloatToString((float)tValue);
                    }
                    else if (tTypeOfThis == typeof(double))
                    {
                        tValueString = NWDToolbox.DoubleToString((double)tValue);
                    }
                    else
                    {
                        tValueString = tValue.ToString().Replace("\"", "\"\"");
                    }
                }
                //tValueString = "/*" + tProp.Name + "*/" + tValueString;
                tValues.Add(tValueString);
            }
            string rReturn = "INSERT OR REPLACE INTO `" + tHelper.ClassNamePHP + "` (`" + string.Join("`, `", tKeys) + "`) VALUES (\"" + string.Join("\", \"", tValues) + "\");";
            //Debug.Log(rReturn);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string New_SQLDelete()
        {
            if (string.IsNullOrEmpty(Reference))
            {
                return string.Empty;
            }
            NWDBasisHelper tHelper = BasisHelper();
            string tSignReference = NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference);
            string rReturn = "DELETE FROM `" + tHelper.ClassNamePHP + "` WHERE `" + tSignReference + "` = \"" + Reference.Replace("\"", "\"\"") + "\";";
            //Debug.Log(rReturn);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================