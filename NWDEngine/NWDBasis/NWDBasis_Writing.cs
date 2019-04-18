// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:26:25
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================
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
        ///// <summary>
        ///// The current state of the writing for this object.
        ///// </summary>
        //private NWDWritingState WritingState = NWDWritingState.Free;
        ///// <summary>
        ///// The writing lock counter. If lock is close the number is the number of lock!
        ///// </summary>
        //private int WritingLocksCounter = 0;
        ///// <summary>
        ///// The writing pending.
        ///// </summary>
        //private NWDWritingPending WritingPending = NWDWritingPending.Unknow;
        ////-------------------------------------------------------------------------------------------------------------
        //public NWDWritingPending DatabasePending()
        //{
        //    return WritingPending;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// Writing lock close once more.
        ///// </summary>
        //private void WritingLockAdd()
        //{
        //    //BTBBenchmark.Start();
        //    WritingLocksCounter++;
        //    if (NWDDataManager.SharedInstance().kDataInWriting.Contains(this) == false)
        //    {
        //        NWDDataManager.SharedInstance().kDataInWriting.Add(this);
        //    }
        //    //BTBBenchmark.Finish();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// Writing lock open once. If lock =0 then the object can change writing mode.
        ///// </summary>
        //private void WritingLockRemove()
        //{
        //    //BTBBenchmark.Start();
        //    WritingLocksCounter--;
        //    if (WritingLocksCounter == 0)
        //    {
        //        WritingState = NWDWritingState.Free;
        //        if (NWDDataManager.SharedInstance().kDataInWriting.Contains(this) == false)
        //        {
        //            NWDDataManager.SharedInstance().kDataInWriting.Remove(this);
        //        }
        //    }
        //    //BTBBenchmark.Finish();
        //}
        //-------------------------------------------------------------------------------------------------------------
        #endregion Lock Data
        #region New Data
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// New data.
        /// </summary>
        /// <returns>The data.</returns>
        /// <param name="sWritingMode">S writing mode.</param>
        static public K NewData(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            //BTBBenchmark.Start();
            K rReturnObject = NewDataWithReference(null, true, sWritingMode);
            //BTBBenchmark.Finish();
            return rReturnObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        //static public T NewData<T>(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        //{
        //    //BTBBenchmark.Start();
        //    T rReturnObject = NewDataWithReference(null, true, sWritingMode) as T;
        //    //BTBBenchmark.Finish();
        //    return rReturnObject;
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// New data with reference.
        /// </summary>
        /// <returns>The data with reference.</returns>
        /// <param name="sReference">S reference.</param>
        /// <param name="sAutoDate">If set to <c>true</c> s auto date.</param>
        /// <param name="sWritingMode">S writing mode.</param>
        static public K NewDataWithReference(string sReference, bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            //BTBBenchmark.Start();
            K rReturnObject = null;
            if (ClassType() != null)
            {
                rReturnObject = (K)Activator.CreateInstance(ClassType(), new object[] { false });
                rReturnObject.InstanceInit();
                if (sReference == null || sReference == string.Empty)
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
                Debug.LogWarning("ClassType() is null for " + BasisHelper().ClassNamePHP);
            }
            //BTBBenchmark.Finish();
            return rReturnObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void PropertiesAutofill()
        {
            foreach (PropertyInfo tPropInfo in PropertiesAccountDependent())
            {
                //Debug.Log("try to insert automatically the account reference in the NWDAccount connection property : " + tPropInfo.Name);
                NWDReferenceType<NWDAccount> tAtt = new NWDReferenceType<NWDAccount>();
                tAtt.Value = NWDAccount.CurrentReference();
                tPropInfo.SetValue(this, tAtt, null);
            }
            if (BasisHelper().ClassGameSaveDependent == true)
            {
                NWDReferenceType<NWDGameSave> tAtt = new NWDReferenceType<NWDGameSave>();
                if (NWDGameSave.CurrentData() != null)
                {
                    tAtt.SetReference(NWDGameSave.CurrentData().Reference);
                }
                PropertyInfo tPropInfo = BasisHelper().ClassGameDependentProperties;
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
        public override NWDTypeClass Base_DuplicateData(bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            return DuplicateData(sAutoDate, sWritingMode) as NWDTypeClass;
        }
        //-------------------------------------------------------------------------------------------------------------
        public K DuplicateData(bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            //BTBBenchmark.Start();
            K rReturnObject = null;
            if (TestIntegrity() == true)
            {
                if (ClassType() != null)
                {
                    rReturnObject = (K)Activator.CreateInstance(ClassType(), new object[] { false });
                    rReturnObject.InstanceInit();
                    //rReturnObject.PropertiesAutofill();
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
                    string tOriginalKey = string.Empty + InternalKey;
                    string tPattern = "\\(COPY [0-9]*\\)";
                    string tReplacement = string.Empty;
                    Regex tRegex = new Regex(tPattern);
                    tOriginalKey = tRegex.Replace(tOriginalKey, tReplacement);
                    tOriginalKey = tOriginalKey.TrimEnd();
                    // init search
                    int tCounter = 1;
                    string tCopy = tOriginalKey + " (COPY " + tCounter + ")";
                    // search available internal key
                    while (BasisHelper().DatasByInternalKey.ContainsKey(tCopy) == true)
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
                    rReturnObject.ReIndex();
                    rReturnObject.InsertData(sAutoDate, sWritingMode);
                }
                else
                {
                    Debug.LogWarning("ClassType() is null for " + BasisHelper().ClassNamePHP);
                }
            }
            //BTBBenchmark.Finish();
            return rReturnObject;
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
        public void CopyData(NWDTypeClass sOriginal)
        {
            //BTBBenchmark.Start();
            string[] tKey = BasisHelper().New_SLQAssemblyOrderArray();
            Type tType = ClassType();
            foreach (string tPropertyString in tKey)
            {
                PropertyInfo tPropertyInfo = tType.GetProperty(tPropertyString, BindingFlags.Public | BindingFlags.Instance);
                Type tTypeOfThis = tPropertyInfo.PropertyType;
                object tValue = tPropertyInfo.GetValue(sOriginal, null);
                if (tValue == null)
                {
                    tValue = string.Empty;
                }
                string tValueString = string.Copy(tValue.ToString()); // force to copy new object
                if (tTypeOfThis.IsEnum)
                {
                    //int tTemp = 0;
                    //int.TryParse(tValueString, out tTemp);
                    tPropertyInfo.SetValue(this, tValue, null);
                }
                else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataType)))
                {
                    BTBDataType tTemp = Activator.CreateInstance(tTypeOfThis) as BTBDataType;
                    tTemp.Value = ((BTBDataType)tValue).Value;
                    //tTemp.SetValue(tValueString);
                    tPropertyInfo.SetValue(this, tTemp, null);
                }
                else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeInt)))
                {
                    BTBDataTypeInt tTemp = Activator.CreateInstance(tTypeOfThis) as BTBDataTypeInt;
                    //long tTempInt = 0;
                    //long.TryParse(tValueString, out tTempInt);
                    //tTemp.SetLong(tTempInt);
                    tTemp.Value = ((BTBDataTypeInt)tValue).Value;
                    tPropertyInfo.SetValue(this, tTemp, null);
                }
                else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeFloat)))
                {
                    BTBDataTypeFloat tTemp = Activator.CreateInstance(tTypeOfThis) as BTBDataTypeFloat;
                    //double tTempDouble = 0;
                    //double.TryParse(tValueString, out tTempDouble);
                    //tTemp.SetDouble(tTempDouble);
                    tTemp.Value = ((BTBDataTypeFloat)tValue).Value;
                    tPropertyInfo.SetValue(this, tTemp, null);
                }
                else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeEnum)))
                {
                    BTBDataTypeEnum tTemp = Activator.CreateInstance(tTypeOfThis) as BTBDataTypeEnum;
                    //long tTempDouble = 0;
                    //long.TryParse(tValueString, out tTempDouble);
                    //tTemp.SetLong(tTempDouble);
                    tTemp.Value = ((BTBDataTypeEnum)tValue).Value;
                    tPropertyInfo.SetValue(this, tTemp, null);
                }
                else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeMask)))
                {
                    BTBDataTypeMask tTemp = Activator.CreateInstance(tTypeOfThis) as BTBDataTypeMask;
                    //long tTempDouble = 0;
                    //long.TryParse(tValueString, out tTempDouble);
                    //tTemp.SetLong(tTempDouble);
                    tTemp.Value = ((BTBDataTypeMask)tValue).Value;
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
                else if (tTypeOfThis == typeof(long))
                {
                    long tTemp = 0;
                    long.TryParse(tValueString, out tTemp);
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
        public bool InsertData(bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            //BTBBenchmark.Start();
            // Determine the default mode
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
            //NWDVersionType tVersion = new NWDVersionType();
            //tVersion.SetValue("0.00.00");
            //this.MinVersion = tVersion;

            //NWDVersionType tMaxVersion = new NWDVersionType();
            //tMaxVersion.SetString("99.99.99");
            //this.MaxVersion = tMaxVersion;

            int tWebModelToUse = WebModelToUse();
            //Debug.Log(" set from " + this.WebModel + " To " + tWebModelToUse);
            WebModel = tWebModelToUse;

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
            this.ServerHash = string.Empty;
            this.ServerLog = string.Empty;
            this.UpdateIntegrity();
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void InsertDataProceed()
        {
            //BTBBenchmark.Start();
            // Debug.Log("NWDBasis<K> InsertDataProceed()");
            if (AccountDependent())
            {
                if (NWDDataManager.SharedInstance().SQLiteConnectionAccountIsValid())
                {
                    NWDDataManager.SharedInstance().SQLiteConnectionAccount.Insert(this);
                }
            }
            else
            {
                if (NWDDataManager.SharedInstance().SQLiteConnectionEditorIsValid())
                {

                    NWDDataManager.SharedInstance().SQLiteConnectionEditor.Insert(this);
                }
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void InsertDataProceedWithTransaction()
        {
            //BTBBenchmark.Start();
            Debug.Log("NWDBasis<K> InsertDataProceedWithTransaction()");
            if (AccountDependent())
            {
                if (NWDDataManager.SharedInstance().SQLiteConnectionAccountIsValid())
                {
                    NWDDataManager.SharedInstance().SQLiteConnectionAccount.BeginTransaction();
                    NWDDataManager.SharedInstance().SQLiteConnectionAccount.Insert(this);
                    NWDDataManager.SharedInstance().SQLiteConnectionAccount.Commit();
                }
            }
            else
            {
                if (NWDDataManager.SharedInstance().SQLiteConnectionEditorIsValid())
                {

                    NWDDataManager.SharedInstance().SQLiteConnectionEditor.BeginTransaction();
                    NWDDataManager.SharedInstance().SQLiteConnectionEditor.Insert(this);
                    NWDDataManager.SharedInstance().SQLiteConnectionEditor.Commit();
                }
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void InsertDataFinish()
        {
            //BTBBenchmark.Start();
            //Debug.Log("NWDBasis<K> InsertDataFinish()");
            WritingLockRemove();
            WritingPending = NWDWritingPending.InDatabase;
#if UNITY_EDITOR
            RowAnalyze();
#endif 
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
        public override void UpdateData(bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.MainThread, bool sWebServiceUpgrade = true, bool sWithCallBack = true)
        {
            #if UNITY_EDITOR
            //            RecalulateHeight = true;
            NWDNodeEditor.ReAnalyzeIfNecessary(this);
            #endif
            //BTBBenchmark.Start();
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
            //Debug.Log("update ... index method count = "+ BasisHelper().IndexInsertMethodList.Count);
            //UpdateObjectInListOfEdition(this);

            this.ReIndex();

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
            this.ServerHash = string.Empty;
            // Update WebServiceVersion
            if (sWebServiceUpgrade == true)
            {
                int tWS = WebModelToUse();
                if (this.WebModel != tWS)
                {
                    //this.AddonVersionMe(); // call override method
                    //Debug.Log(" set from " + WebModel + " To " + tWS);
                    this.WebModel = tWS;
                }
            }
            this.UpdateIntegrity();
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void UpdateDataProceed()
        {
            //BTBBenchmark.Start();
            //Debug.Log("NWDBasis<K> UpdateDataProceed()");
            if (AccountDependent())
            {
                if (NWDDataManager.SharedInstance().SQLiteConnectionAccountIsValid())
                {
                    NWDDataManager.SharedInstance().SQLiteConnectionAccount.Update(this);
                }
            }
            else
            {
                if (NWDDataManager.SharedInstance().SQLiteConnectionEditorIsValid())
                {

                    NWDDataManager.SharedInstance().SQLiteConnectionEditor.Update(this);
                }
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void UpdateDataProceedWithTransaction()
        {
            //BTBBenchmark.Start();
            //Debug.Log("NWDBasis<K> UpdateDataProceedWithTransaction()");
            if (AccountDependent())
            {
                if (NWDDataManager.SharedInstance().SQLiteConnectionAccountIsValid())
                {
                    NWDDataManager.SharedInstance().SQLiteConnectionAccount.BeginTransaction();
                    NWDDataManager.SharedInstance().SQLiteConnectionAccount.Update(this);
                    NWDDataManager.SharedInstance().SQLiteConnectionAccount.Commit();
                }
            }
            else
            {
                if (NWDDataManager.SharedInstance().SQLiteConnectionEditorIsValid())
                {
                    NWDDataManager.SharedInstance().SQLiteConnectionEditor.BeginTransaction();
                    NWDDataManager.SharedInstance().SQLiteConnectionEditor.Update(this);
                    NWDDataManager.SharedInstance().SQLiteConnectionEditor.Commit();
                }
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void UpdateDataFinish()
        {
            //BTBBenchmark.Start();
            //Debug.Log("NWDBasis<K> UpdateDataFinish()");
            WritingLockRemove();
            WritingPending = NWDWritingPending.InDatabase;
#if UNITY_EDITOR
            RowAnalyze();
#endif 
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool UpdateDataIfModifiedWithoutCallBack()
        {
            //BTBBenchmark.Start();
            bool tReturn = false;
            if (this.Integrity != this.IntegrityValue())
            {
                tReturn = true;
                UpdateData(true, NWDWritingMode.ByDefaultLocal , false, false);
            }
            //BTBBenchmark.Finish();
            return tReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool UpdateDataIfModified(bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
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
        public override void EnableData(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            //BTBBenchmark.Start();
            this.AC = true;
            this.AddonEnableMe(); // call override method
            this.UpdateData(true, sWritingMode);
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DisableData(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            //BTBBenchmark.Start();
            this.DD = NWDToolbox.Timestamp();
            this.AC = false;
            this.AddonDisableMe(); // call override method
            this.UpdateData(true, sWritingMode);
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void TrashData(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
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
        public override void UnTrashData(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
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
        public override void DeleteData(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
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
                this.Desindex(); // call override method
                DeleteDataOperation();
                BasisHelper().RemoveData(this);
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
        public override void DeleteDataProceed()
        {
            //BTBBenchmark.Start();
            //Debug.Log("NWDBasis<K> DeleteDataProceed()");
            if (AccountDependent())
            {
                if (NWDDataManager.SharedInstance().SQLiteConnectionAccountIsValid())
                {
                    NWDDataManager.SharedInstance().SQLiteConnectionAccount.Delete(this);
                }
            }
            else
            {
                if (NWDDataManager.SharedInstance().SQLiteConnectionEditorIsValid())
                {
                    NWDDataManager.SharedInstance().SQLiteConnectionEditor.Delete(this);
                }
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DeleteDataProceedWithTransaction()
        {
            //BTBBenchmark.Start();
            //Debug.Log("NWDBasis<K> DeleteDataProceedWithTransaction()");
            if (AccountDependent())
            {
if (NWDDataManager.SharedInstance().SQLiteConnectionAccountIsValid())
                {
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.BeginTransaction();
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.Delete(this);
                NWDDataManager.SharedInstance().SQLiteConnectionAccount.Commit();
}
            }
            else
            {
                if (NWDDataManager.SharedInstance().SQLiteConnectionEditorIsValid())
                {
                    NWDDataManager.SharedInstance().SQLiteConnectionEditor.BeginTransaction();
                    NWDDataManager.SharedInstance().SQLiteConnectionEditor.Delete(this);
                    NWDDataManager.SharedInstance().SQLiteConnectionEditor.Commit();
                }
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DeleteDataFinish()
        {
            //BTBBenchmark.Start();
            //Debug.Log("NWDBasis<K> DeleteDataFinish()");
            WritingLockRemove();
            WritingPending = NWDWritingPending.InDatabase;
#if UNITY_EDITOR
            RowAnalyze();
#endif 
            //BTBBenchmark.Finish();
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
            //BTBBenchmark.Start();
            InDatabase = true;
            FromDatabase = true;
            WritingPending = NWDWritingPending.InDatabase;
            //AddonLoadedMe();
#if UNITY_EDITOR

#else
            WebserviceVersionCheckMe();
#endif
            //foreach (MethodInfo tMethod in BasisHelper().IndexInsertMethodList)
            //{
            //    tMethod.Invoke(this, null);
            //}
            //AddonIndexMe();
            BasisHelper().AddData(this);
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion Load Data
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================