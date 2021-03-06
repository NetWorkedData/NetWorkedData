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

using System.Text.RegularExpressions;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDTypeClass CreateInstanceQuickly(bool sInsertInNetWorkedData, PropertyInfo[] sPropertyInfo)
        {
            return CreateInstance_Bypass(sInsertInNetWorkedData, false, sPropertyInfo);
        }
        //-------------------------------------------------------------------------------------------------------------
        protected virtual NWDTypeClass CreateInstance_Bypass(bool sInsertInNetWorkedData, bool sStupid, PropertyInfo[] sPropertyInfo)
        {
            NWDTypeClass rReturn = Activator.CreateInstance(ClassType, new object[] { sInsertInNetWorkedData }) as NWDTypeClass;
            rReturn.PropertiesAutofill();
            //foreach (PropertyInfo tPropertyInfo in sPropertyInfo)
            //{
            //    if (tPropertyInfo.PropertyType.IsSubclassOf(typeof(NWEDataType)))
            //    {
            //        NWEDataType tV = Activator.CreateInstance(tPropertyInfo.PropertyType) as NWEDataType;
            //        tPropertyInfo.SetValue(rReturn, tV);
            //    }
            //    else if (tPropertyInfo.PropertyType.IsSubclassOf(typeof(NWEDataTypeInt)))
            //    {
            //        NWEDataTypeInt tV = Activator.CreateInstance(tPropertyInfo.PropertyType) as NWEDataTypeInt;
            //        tPropertyInfo.SetValue(rReturn, tV);
            //    }
            //    else if (tPropertyInfo.PropertyType.IsSubclassOf(typeof(NWEDataTypeEnum)))
            //    {
            //        NWEDataTypeEnum tV = Activator.CreateInstance(tPropertyInfo.PropertyType) as NWEDataTypeEnum;
            //        tPropertyInfo.SetValue(rReturn, tV);
            //    }
            //    else if (tPropertyInfo.PropertyType.IsSubclassOf(typeof(NWEDataTypeMask)))
            //    {
            //        NWEDataTypeMask tV = Activator.CreateInstance(tPropertyInfo.PropertyType) as NWEDataTypeMask;
            //        tPropertyInfo.SetValue(rReturn, tV);
            //    }
            //    else if (tPropertyInfo.PropertyType.IsSubclassOf(typeof(NWEDataTypeFloat)))
            //    {
            //        NWEDataTypeFloat tV = Activator.CreateInstance(tPropertyInfo.PropertyType) as NWEDataTypeFloat;
            //        tPropertyInfo.SetValue(rReturn, tV);
            //    }
            //}
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string PHP_TABLENAME(NWDAppEnvironment sEnvironment)
        {
            return NWDAppConfiguration.SharedInstance().TablePrefixe + sEnvironment.Environment + "_" + TablePrefix + ClassTableName;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTypeClass NewData(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            //ClassInformations("#### test");
            return NewDataWithReference(null, true, sWritingMode);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTypeClass NewDataWithReference(string sReference, bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            NWDTypeClass rReturnObject = null;
            rReturnObject = Activator.CreateInstance(ClassType, new object[] { false }) as NWDTypeClass;
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
            //NWDBenchmark.Finish();
            return rReturnObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static T NewData<T>(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal) where T : NWDTypeClass, new()
        {
            //NWDBasisHelper tHelper = FindTypeInfos(typeof(T));
            //tHelper.ClassInformations("#### test");
            return NewDataWithReference<T>(null, true, sWritingMode);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// New data with reference.
        /// </summary>
        /// <returns>The data with reference.</returns>
        /// <param name="sReference">S reference.</param>
        /// <param name="sAutoDate">If set to <c>true</c> s auto date.</param>
        /// <param name="sWritingMode">S writing mode.</param>
        public static T NewDataWithReference<T>(string sReference, bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal) where T : NWDTypeClass, new()
        {
            //NWDBenchmark.Start();
            NWDBasisHelper tHelper = BasisHelper<T>();
            return tHelper.NewDataWithReference(sReference, sAutoDate, sWritingMode) as T;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string TableNamePHP<T>(NWDAppEnvironment sEnvironment) where T : NWDTypeClass, new()
        {
            NWDBasisHelper tHelper = BasisHelper<T>();
            return tHelper.PHP_TABLENAME(sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string PHP_ClassNameFor<T>() where T : NWDTypeClass, new()
        {
            NWDBasisHelper tHelper = BasisHelper<T>();
            return tHelper.ClassNamePHP;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string PHP_TrigrammeFor<T>() where T : NWDTypeClass, new()
        {
            NWDBasisHelper tHelper = BasisHelper<T>();
            return tHelper.ClassTrigramme;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDBasisHelper BasisHelper<T>() where T : NWDTypeClass, new()
        {
            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(typeof(T));
            if (tHelper == null)
            {
                Debug.LogWarning("ERROR NWDBasisHelper.FindTypeInfos(typeof(K)) NOT RETURN FOR " + typeof(T).Name);
            }
            return tHelper;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string DGPRExtract()
        {
            string rExtract = "{\"" + ClassNamePHP + "\"" + " : [\n\r";
            List<string> tListSerialized = new List<string>();
            List<NWDTypeClass> tListData = new List<NWDTypeClass>();
            string tAccountReference = NWDAccount.CurrentReference();
            NWDGameSave tGameSave = NWDGameSave.CurrentData();
            foreach (NWDTypeClass tData in Datas)
            {
                NWDTypeClass tDataReturn = QuickFilter(tData, tAccountReference, tGameSave);
                if (tDataReturn != null)
                {
                    tListData.Add(tDataReturn);
                }
            }
            foreach (NWDTypeClass tObject in tListData)
            {
                tListSerialized.Add("{ \"csv\" : \"" + tObject.DGPRLinearization(ClassNamePHP) + "\"}");
            }
            rExtract += string.Join(",\n\r", tListSerialized.ToArray());
            rExtract += "\n\r]\n\r}";
            return rExtract;
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        //public static List<T> GetEditorDatasList<T>() where T : NWDTypeClass, new()
        //{
        //    //NWDBenchmark.Start();
        //    List<T> rReturn = BasisHelper<T>().Datas as List<T>;
        //    //NWDBenchmark.Finish();
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static T[] GetEditorDatas<T>() where T : NWDTypeClass, new()
        //{
        //    //NWDBenchmark.Start();
        //    T[] rReturn = BasisHelper<T>().Datas.ToArray() as T[];
        //    //NWDBenchmark.Finish();
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public static T GetEditorDataByReference<T>(string sReference, bool sTryOnDisk = false) where T : NWDTypeClass, new()
        {
            //NWDBenchmark.Start();
            T rReturn = null;
            if (BasisHelper<T>().DatasByReference.ContainsKey(sReference))
            {
                rReturn = BasisHelper<T>().DatasByReference[sReference] as T;
            }
            else
            {
                if (sTryOnDisk == true)
                {
                    BasisHelper<T>().LoadFromDatabaseByReference(sReference, true);
                    if (BasisHelper<T>().DatasByReference.ContainsKey(sReference))
                    {
                        rReturn = BasisHelper<T>().DatasByReference[sReference] as T;
                    }
                }
            }
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static T[] GetEditorDatasByInternalKey<T>(string sInternalKey) where T : NWDTypeClass, new()
        {
            //NWDBenchmark.Start();
            List<T> rReturn;
            if (BasisHelper<T>().DatasByInternalKey.ContainsKey(sInternalKey))
            {
                rReturn = BasisHelper<T>().DatasByInternalKey[sInternalKey] as List<T>;
            }
            else
            {
                rReturn = new List<T>();
            }
            //NWDBenchmark.Finish();
            return rReturn.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static T GetEditorFirstDataByInternalKey<T>(string sInternalKey) where T : NWDTypeClass, new()
        {
            //NWDBenchmark.Start();
            T rReturn = null;
            T[] rDatas = GetEditorDatasByInternalKey<T>(sInternalKey);
            if (rDatas.Length > 0)
            {
                rReturn = rDatas[0];
            }
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        #region RAW
        //-------------------------------------------------------------------------------------------------------------
        public static List<T> GetRawDatasList<T>() where T : NWDTypeClass, new()
        {
            List<NWDTypeClass> tDatas = BasisHelper<T>().Datas;
            return QuickFilterDatas<T>(tDatas, null, null);
        }
        //-------------------------------------------------------------------------------------------------------------
        // ANCIEN GetAllObjects()
        public static T[] GetRawDatas<T>() where T : NWDTypeClass, new()
        {
            //NWDBenchmark.Start();
            T[] rReturn = GetRawDatasList<T>().ToArray() as T[];
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static T GetRawFirstData<T>() where T : NWDTypeClass, new()
        {
            //NWDBenchmark.Start();
            T rReturn = null;
            T[] rDatas = GetRawDatas<T>();
            if (rDatas.Length > 0)
            {
                rReturn = rDatas[0];
            }
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static T GetRawDataByReference<T>(string sReference, bool sTryOnDisk = false) where T : NWDTypeClass, new()
        {
            //NWDBenchmark.Start();
            T rReturn = null;
            if (string.IsNullOrEmpty(sReference) == false)
            {
                NWDBasisHelper tHelper = BasisHelper<T>();
                if (tHelper.DatasByReference.ContainsKey(sReference))
                {
                    //Debug.Log("found in memory reference " + sReference);
                    rReturn = tHelper.DatasByReference[sReference] as T;
                }
                else
                {
                    //Debug.Log("not found in memory reference " + sReference);
                    if (sTryOnDisk == true)
                    {
                        //Debug.Log("try find on disk reference " + sReference);
                        tHelper.LoadFromDatabaseByReference(sReference, true);
                        if (tHelper.DatasByReference.ContainsKey(sReference))
                        {
                            //Debug.Log("not found on disk reference " + sReference);
                            rReturn = tHelper.DatasByReference[sReference] as T;
                        }
                    }
                }
                rReturn = QuickFilter<T>(rReturn, null, null);
            }
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static T[] GetRawDatasByInternalKey<T>(string sInternalKey) where T : NWDTypeClass, new()
        {
            //NWDBenchmark.Start();
            List<NWDTypeClass> tReturn;
            if (BasisHelper<T>().DatasByInternalKey.ContainsKey(sInternalKey))
            {
                tReturn = BasisHelper<T>().DatasByInternalKey[sInternalKey];
            }
            else
            {
                tReturn = new List<NWDTypeClass>();
            }
            List<T> rReturn = QuickFilterDatas<T>(tReturn, null, null);
            //NWDBenchmark.Finish();
            return rReturn.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static T GetRawFirstDataByInternalKey<T>(string sInternalKey) where T : NWDTypeClass, new()
        {
            //NWDBenchmark.Start();
            T rReturn = null;
            T[] rDatas = GetRawDatasByInternalKey<T>(sInternalKey);
            if (rDatas.Length > 0)
            {
                rReturn = rDatas[0];
            }
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        #region CORPORATE Get datas for by account and by gamesave
        //-------------------------------------------------------------------------------------------------------------
        public static List<T> GetCorporateDatasList<T>(string sAccountReference, NWDGameSave sGameSave = null) where T : NWDTypeClass, new()
        {
            List<NWDTypeClass> tDatas = BasisHelper<T>().Datas;
            return QuickFilterDatas<T>(tDatas, sAccountReference, sGameSave);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static T[] GetCorporateDatas<T>(string sAccountReference, NWDGameSave sGameSave = null) where T : NWDTypeClass, new()
        {
            List<NWDTypeClass> tDatas = BasisHelper<T>().Datas;
            return QuickFilterDatas<T>(tDatas, sAccountReference, sGameSave).ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static T GetCorporateFirstData<T>(string sAccountReference, NWDGameSave sGameSave = null) where T : NWDTypeClass, new()
        {
            //NWDBenchmark.Start();
            T rReturn = null;
            T[] rDatas = GetCorporateDatas<T>(sAccountReference, sGameSave);
            if (rDatas.Length > 0)
            {
                rReturn = rDatas[0];
            }
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static T GetCorporateDataByReference<T>(string sReference, string sAccountReference, NWDGameSave sGameSave = null, bool sTryOnDisk = false) where T : NWDTypeClass, new()
        {
            //NWDBenchmark.Start();
            T rReturn = null;
            if (BasisHelper<T>().DatasByReference.ContainsKey(sReference))
            {
                rReturn = BasisHelper<T>().DatasByReference[sReference] as T;
            }
            else
            {
                if (sTryOnDisk == true)
                {
                    BasisHelper<T>().LoadFromDatabaseByReference(sReference, true);
                    if (BasisHelper<T>().DatasByReference.ContainsKey(sReference))
                    {
                        rReturn = BasisHelper<T>().DatasByReference[sReference] as T;
                    }
                }
            }
            rReturn = QuickFilter<T>(rReturn, sAccountReference, sGameSave);
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static T[] GetCorporateDatasByInternalKey<T>(string sInternalKey, string sAccountReference, NWDGameSave sGameSave = null) where T : NWDTypeClass, new()
        {
            //NWDBenchmark.Start();
            List<NWDTypeClass> tReturn;
            if (BasisHelper<T>().DatasByInternalKey.ContainsKey(sInternalKey))
            {
                tReturn = BasisHelper<T>().DatasByInternalKey[sInternalKey];
            }
            else
            {
                tReturn = new List<NWDTypeClass>();
            }
            List<T> rReturn = QuickFilterDatas<T>(tReturn, sAccountReference, sGameSave);
            //NWDBenchmark.Finish();
            return rReturn.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static T GetCorporateFirstDataByInternalKey<T>(string sInternalKey, string sAccountReference, NWDGameSave sGameSave = null) where T : NWDTypeClass, new()
        {
            //NWDBenchmark.Start();
            T rReturn = null;
            T[] rDatas = GetCorporateDatasByInternalKey<T>(sInternalKey, sAccountReference);
            if (rDatas.Length > 0)
            {
                rReturn = rDatas[0];
            }
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        #region REACHABLE Get datas for my account and my gamesave
        //-------------------------------------------------------------------------------------------------------------
        public static List<T> GetReachableDatasList<T>() where T : NWDTypeClass, new()
        {
            return GetCorporateDatasList<T>(NWDAccount.CurrentReference(), NWDGameSave.CurrentData());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static T[] GetReachableDatas<T>() where T : NWDTypeClass, new()
        {
            return GetCorporateDatas<T>(NWDAccount.CurrentReference(), NWDGameSave.CurrentData());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static T GetReachableFirstData<T>() where T : NWDTypeClass, new()
        {
            return GetCorporateFirstData<T>(NWDAccount.CurrentReference(), NWDGameSave.CurrentData());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static T GetReachableDataByReference<T>(string sReference, bool sTryOnDisk = false) where T : NWDTypeClass, new()
        {
            return GetCorporateDataByReference<T>(sReference, NWDAccount.CurrentReference(), NWDGameSave.CurrentData(), sTryOnDisk);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static T[] GetReacheableDatasByInternalKey<T>(string sInternalKey) where T : NWDTypeClass, new()
        {
            return GetCorporateDatasByInternalKey<T>(sInternalKey, NWDAccount.CurrentReference(), NWDGameSave.CurrentData());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static T GetReacheableFirstDataByInternalKey<T>(string sInternalKey) where T : NWDTypeClass, new()
        {
            return GetCorporateFirstDataByInternalKey<T>(sInternalKey, NWDAccount.CurrentReference(), NWDGameSave.CurrentData());
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion

        #region PRIVATE FILTER

        //-------------------------------------------------------------------------------------------------------------
        private NWDTypeClass QuickFilter(NWDTypeClass sData, string sAccountReference = null, NWDGameSave sGameSave = null)
        {
            //NWDBenchmark.Start();
            NWDTypeClass rReturn = null;
            if (sData != null)
            {
                if (sData.IsEnable() == true)
                {
                    string tGameSaveReference = null;
                    if (sGameSave != null)
                    {
                        tGameSaveReference = sGameSave.Reference;
                    }
                    bool tInsert = sData.IsReacheableBy(tGameSaveReference, sAccountReference);

                    //bool tInsert = true;
                    //if (kAccountDependent)
                    //{
                    //    if (sGameSave != null)
                    //    {
                    //        // test game save if necessary
                    //        if (GameSaveMethod != null && sGameSave != null)
                    //        {
                    //            string tGameIndex = sGameSave.Reference;
                    //            var tValue = ClassGameDependentProperties.GetValue(sData, null);
                    //            if (tValue == null)
                    //            {
                    //                tValue = string.Empty;
                    //            }
                    //            string tSaveIndex = GameSaveMethod.Invoke(tValue, null) as string;
                    //            if (tSaveIndex != tGameIndex)
                    //            {
                    //                tInsert = false;
                    //            }
                    //        }
                    //    }
                    //    if (tInsert == true && string.IsNullOrEmpty(sAccountReference) == false)
                    //    {
                    //        tInsert = false; // research by default false and true when found first solution
                    //        foreach (KeyValuePair<PropertyInfo, MethodInfo> tInfos in AccountMethodDico)
                    //        {
                    //            var tValue = tInfos.Key.GetValue(sData, null);
                    //            if (tValue == null)
                    //            {
                    //                tValue = string.Empty;
                    //            }
                    //            string tAccountValue = tInfos.Value.Invoke(tValue, null) as string;
                    //            if (tAccountValue.Contains(sAccountReference))
                    //            {
                    //                tInsert = true;
                    //                break; // I fonud one solution! this user can see this informations
                    //            }
                    //        }
                    //    }
                    //}
                    if (tInsert == true)
                    {
                        rReturn = sData;
                    }
                }
                else
                {

                }
            }
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private static T QuickFilter<T>(T sData, string sAccountReference = null, NWDGameSave sGameSave = null) where T : NWDTypeClass, new()
        {
            T rReturn = null;
            if (sData != null)
            {
                //if (sData.IsTrashed() == false || sData.IsEnable() == true || sData.TestIntegrityResult == true)
                if (sData.IsEnable() == true)
                {
                    if (sData.IntegrityIsValid())
                    {
                        string tGameSaveReference = null;
                        if (sGameSave != null)
                        {
                            tGameSaveReference = sGameSave.Reference;
                        }
                        bool tInsert = sData.IsReacheableBy(tGameSaveReference, sAccountReference);

                        //bool tInsert = true;
                        //if (BasisHelper<T>().kAccountDependent)
                        //{


                        //    if (sGameSave != null)
                        //    {
                        //        // test game save if necessary
                        //        if (BasisHelper<T>().GameSaveMethod != null && sGameSave != null)
                        //        {
                        //            string tGameIndex = sGameSave.Reference;
                        //            var tValue = BasisHelper<T>().ClassGameDependentProperties.GetValue(sData, null);
                        //            if (tValue == null)
                        //            {
                        //                tValue = string.Empty;
                        //            }
                        //            string tSaveIndex = BasisHelper<T>().GameSaveMethod.Invoke(tValue, null) as string;
                        //            if (tSaveIndex != tGameIndex)
                        //            {
                        //                tInsert = false;
                        //            }
                        //        }
                        //    }
                        //    if (tInsert == true && string.IsNullOrEmpty(sAccountReference) == false)
                        //    {
                        //        tInsert = false; // research by default false and true when found first solution
                        //        foreach (KeyValuePair<PropertyInfo, MethodInfo> tInfos in BasisHelper<T>().AccountMethodDico)
                        //        {
                        //            var tValue = tInfos.Key.GetValue(sData, null);
                        //            if (tValue == null)
                        //            {
                        //                tValue = string.Empty;
                        //            }
                        //            string tAccountValue = tInfos.Value.Invoke(tValue, null) as string;
                        //            if (tAccountValue.Contains(sAccountReference))
                        //            {
                        //                tInsert = true;
                        //                break; // I fonud one solution! this user can see this informations
                        //            }
                        //        }
                        //    }
                        //}
                        if (tInsert == true)
                        {
                            rReturn = sData;
                        }
                    }
                }
            }

            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private static List<T> QuickFilterDatas<T>(List<NWDTypeClass> sDatasList, string sAccountReference = null, NWDGameSave sGameSave = null) where T : NWDTypeClass, new()
        {
            List<T> rList = new List<T>();
            if (sDatasList != null)
            {
                //Debug.Log(BasisHelper<T>().ClassNamePHP + " : search data in " + sDatasArray.Count + " rows ");
                foreach (T tData in sDatasList)
                {
                    T tDataReturn = QuickFilter<T>(tData, sAccountReference, sGameSave);
                    if (tDataReturn != null)
                    {
                        rList.Add(tDataReturn);
                    }
                }
            }

            return rList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<T> QuickFilterDatas<T>(List<T> sDatasList, string sAccountReference = null, NWDGameSave sGameSave = null) where T : NWDTypeClass, new()
        {
            List<T> rList = new List<T>();
            if (sDatasList != null)
            {
                foreach (T tData in sDatasList)
                {
                    T tDataReturn = QuickFilter<T>(tData, sAccountReference, sGameSave);
                    if (tDataReturn != null)
                    {
                        rList.Add(tDataReturn);
                    }
                }
            }

            return rList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static T[] QuickFilterDatas<T>(T[] sDatasArray, string sAccountReference = null, NWDGameSave sGameSave = null) where T : NWDTypeClass, new()
        {
            List<T> rList = new List<T>();
            if (sDatasArray != null)
            {
                foreach (T tData in sDatasArray)
                {
                    T tDataReturn = QuickFilter<T>(tData, sAccountReference, sGameSave);
                    if (tDataReturn != null)
                    {
                        rList.Add(tDataReturn);
                    }
                }
            }

            return rList.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static T[] QuickFilterReacheableDatas<T>(T[] sDatasArray) where T : NWDTypeClass, new()
        {
            return QuickFilterDatas<T>(sDatasArray, NWDAccount.CurrentReference(), NWDGameSave.CurrentData());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<T> QuickFilterReacheableDatas<T>(List<T> sDatasList) where T : NWDTypeClass, new()
        {
            return QuickFilterDatas<T>(sDatasList, NWDAccount.CurrentReference(), NWDGameSave.CurrentData());
        }
        //-------------------------------------------------------------------------------------------------------------

        #endregion
        #region LOAD PARTIAL

        //-------------------------------------------------------------------------------------------------------------
        //TODO : rewrite with SQLiteAccountHandle
        //        private static T LoadDataByReference<T>(string sReference) where T : NWDTypeClass, new()
        //        {
        //            Debug.Log("LoadDataByReference(" + sReference + ")");
        //            NWDBenchmark.Start();
        //            T rReturn = null;
        //            NWDBasisHelper tTypeInfos = BasisHelper<T>();
        //            if (tTypeInfos.DatasByReference.ContainsKey(sReference) == false)
        //            {
        //                SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
        //                if (tTypeInfos.kAccountDependent)
        //                {
        //                    tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
        //                }
        //                if (tSQLiteConnection != null)
        //                {
        //                    if (tSQLiteConnection.IsValid())
        //                    {
        //                        List<T> tSelect = tSQLiteConnection.Query<T>("SELECT * FROM " + tTypeInfos.ClassNamePHP + " WHERE `" + NWDToolbox.PropertyName(() => FictiveData<T>().Reference) + "` = '" + sReference + "';");
        //                        if (tSelect != null)
        //                        {
        //                            foreach (T tItem in tSelect)
        //                            {
        //                                rReturn = tItem as T;
        //                                tItem.LoadedFromDatabase();
        //#if UNITY_EDITOR
        //                                tItem.RowAnalyze();
        //#endif
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            NWDBenchmark.Finish();
        //#if UNITY_EDITOR
        //            BasisHelper<T>().FilterTableEditor();
        //            BasisHelper<T>().RepaintTableEditor();
        //#endif
        //            return rReturn;
        //        }
        //-------------------------------------------------------------------------------------------------------------
        //        private static void LoadDataToSync<T>(NWDAppEnvironment sEnvironment) where T : NWDTypeClass, new()
        //        {
        //            NWDBenchmark.Start();
        //            NWDBasisHelper tTypeInfos = BasisHelper<T>();
        //            SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
        //            if (tTypeInfos.kAccountDependent)
        //            {
        //                tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
        //            }
        //            if (tSQLiteConnection != null)
        //            {
        //                if (tSQLiteConnection.IsValid())
        //                {
        //                    //SQLiteCommand tCommand = tSQLiteConnection.CreateCommand("SELECT `Reference` FROM " + tTypeInfos.ClassNamePHP + " WHERE `"+ sEnvironment.Environment + "Sync` = '0' OR `"+ sEnvironment.Environment + "Sync` = '1';");
        //                    //List<string> tSelect = tCommand.ExecuteQuery<string>();
        //                    string tQuery = "SELECT `" + NWDToolbox.PropertyName(() => FictiveData<T>().Reference) + "` FROM " + tTypeInfos.ClassNamePHP + " WHERE `" + sEnvironment.Environment + "Sync` = '0' OR `" + sEnvironment.Environment + "Sync` = '1';";
        //                    //Debug.Log(tQuery);
        //                    SQLiteCommand tCreateCommand = tSQLiteConnection.CreateCommand(tQuery);
        //                    List<NWDTypeClassReference> tSelect = tCreateCommand.ExecuteQuery<NWDTypeClassReference>();
        //                    if (tSelect != null)
        //                    {
        //                        foreach (NWDTypeClassReference tReference in tSelect)
        //                        {
        //                            //Debug.Log("tReference = " + tReference.Reference);
        //                            if (tReference.Reference != null)
        //                            {
        //                                GetRawDataByReference<T>(tReference.Reference);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            NWDBenchmark.Finish();
        //#if UNITY_EDITOR
        //            BasisHelper<T>().FilterTableEditor();
        //            BasisHelper<T>().RepaintTableEditor();
        //#endif
        //        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        #region SELECT
        //-------------------------------------------------------------------------------------------------------------
        //        public static T[] SelectDatasWhereRequest<T>(string sWhere = "`AC`=1;") where T : NWDTypeClass, new()
        //        {
        //            NWDBenchmark.Start();
        //            List<T> rResult = new List<T>();
        //            NWDBasisHelper tTypeInfos = BasisHelper<T>();
        //            SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
        //            if (tTypeInfos.kAccountDependent)
        //            {
        //                tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
        //            }
        //            if (tSQLiteConnection != null)
        //            {
        //                if (tSQLiteConnection.IsValid())
        //                {
        //                    string tQuery = "SELECT `" + NWDToolbox.PropertyName(() => FictiveData<T>().Reference) + "` FROM " + tTypeInfos.ClassNamePHP + " WHERE " + sWhere;
        //                    Debug.Log(tQuery);
        //                    SQLiteCommand tCreateCommand = tSQLiteConnection.CreateCommand(tQuery);
        //                    List<NWDTypeClassReference> tSelect = tCreateCommand.ExecuteQuery<NWDTypeClassReference>();
        //                    if (tSelect != null)
        //                    {
        //                        foreach (NWDTypeClassReference tReference in tSelect)
        //                        {
        //                            if (tReference.Reference != null)
        //                            {
        //                                T tData = GetRawDataByReference<T>(tReference.Reference, true);
        //                                if (tData != null)
        //                                {
        //                                    rResult.Add(tData);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            NWDBenchmark.Finish();
        //#if UNITY_EDITOR
        //            BasisHelper<T>().FilterTableEditor();
        //            BasisHelper<T>().RepaintTableEditor();
        //#endif
        //            return rResult.ToArray();
        //        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        public static T FictiveData<T>() where T : NWDTypeClass, new()
        {
            T rFictive = NewDataWithReference<T>("FICTIVE");
            rFictive.DeleteData();
            return rFictive;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static int CSV_IndexOf<T>(string sPropertyName, int sWebBuilt = -1) where T : NWDTypeClass, new()
        {
            return BasisHelper<T>().CSV_IndexOf(sPropertyName, sWebBuilt);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string SLQSelect<T>(int sWebBuilt = -1) where T : NWDTypeClass, new()
        {
            return BasisHelper<T>().SLQSelect(sWebBuilt);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SynchronizationFromWebService<T>(NWEOperationBlock sSuccessBlock = null,
         NWEOperationBlock sErrorBlock = null,
         NWEOperationBlock sCancelBlock = null,
         NWEOperationBlock sProgressBlock = null,
         bool sForce = false,
         bool sPriority = false) where T : NWDTypeClass, new()
        {
            BasisHelper<T>().SynchronizationFromWebService(sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sForce, sPriority);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static T DuplicateData<T>(T sData, bool sAutoDate = true, NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal) where T : NWDTypeClass, new()
        {
            //NWDBenchmark.Start();
            T rReturnObject = null;
            if (sData != null)
            {
                if (sData.IntegrityIsValid() == true)
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(sData.GetType());
                    rReturnObject = (T)Activator.CreateInstance(tHelper.ClassType, new object[] { false });
                    rReturnObject.InstanceInit();
                    //rReturnObject.PropertiesAutofill();
                    rReturnObject.Initialization();
                    int tDC = rReturnObject.DC; // memorize date of dupplicate
                    string tReference = rReturnObject.NewReference(); // create reference for dupplicate
                    rReturnObject.CopyData(sData); // copy data
                                                   // restore the DC and Reference 
                    rReturnObject.Reference = tReference;
                    rReturnObject.DC = tDC;
                    //#if UNITY_INCLUDE_TESTS
                    //                    rReturnObject.Tag = sData.Tag;
                    //#endif
                    //rReturnObject.Tag = sData.Tag;
                    // WARNING ... copy generate an error in XX ? 
                    // but copy the DD XX and AC from this
                    rReturnObject.DD = sData.DD;
                    rReturnObject.XX = sData.XX;
                    rReturnObject.AC = sData.AC;
                    // Change internal key by addding  "copy xxx"
                    string tOriginalKey = string.Empty + sData.InternalKey;
                    string tPattern = "\\(COPY [0-9]*\\)";
                    string tReplacement = string.Empty;
                    Regex tRegex = new Regex(tPattern);
                    tOriginalKey = tRegex.Replace(tOriginalKey, tReplacement);
                    tOriginalKey = tOriginalKey.TrimEnd();
                    // init search
                    int tCounter = 1;
                    string tCopy = tOriginalKey + " (COPY " + tCounter + ")";
                    // search available internal key

                    while (tHelper.DatasByInternalKey.ContainsKey(tCopy) == true)
                    {
                        tCounter++;
                        tCopy = tOriginalKey + " (COPY " + tCounter + ")";
                    }
                    // set found internalkey
                    rReturnObject.InternalKey = tCopy;
                    // Update Data! become it's not a real insert but a copy!
                    rReturnObject.AddonDuplicateMe();
                    rReturnObject.UpdateDataOperation(sAutoDate);
                    // Insert Data as new Data!
                    rReturnObject.IndexInBase();
                    rReturnObject.IndexInMemory();
                    rReturnObject.AddonDuplicatedMe();
                    rReturnObject.InsertData(sAutoDate, sWritingMode);
                }
                else
                {
                    Debug.LogWarning("Data no integrity, no dupplicate possibility");
                }
            }
            else
            {
                Debug.LogWarning("Data is null, no dupplicate possibility");
            }
            //NWDBenchmark.Finish();
            return rReturnObject;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
