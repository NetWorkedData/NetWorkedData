//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:26:2
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections.Generic;
using System.Reflection;
using BasicToolBox;
using SQLite4Unity3d;
using UnityEngine;

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
        #region EDITOR       
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public static List<K> GetEditorDatasList()
        {
            //BTBBenchmark.Start();
            List<K> rReturn = BasisHelper().Datas as List<K>;
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K[] GetEditorDatas()
        {
            //BTBBenchmark.Start();
            K[] rReturn = BasisHelper().Datas.ToArray() as K[];
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K GetEditorDataByReference(string sReference, bool sTryOnDisk = false)
        {
            //BTBBenchmark.Start();
            K rReturn = null;
            if (BasisHelper().DatasByReference.ContainsKey(sReference))
            {
                rReturn = BasisHelper().DatasByReference[sReference] as K;
            }
            else
            {
                if (sTryOnDisk == true)
                {
                    // TODO : Lag connection : look for quick solution
                    rReturn = LoadDataByReference(sReference);
                }
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K[] GetEditorDatasByInternalKey(string sInternalKey)
        {
            //BTBBenchmark.Start();
            List<K> rReturn;
            if (BasisHelper().DatasByInternalKey.ContainsKey(sInternalKey))
            {
                rReturn = BasisHelper().DatasByInternalKey[sInternalKey] as List<K>;
            }
            else
            {
                rReturn = new List<K>();
            }
            //BTBBenchmark.Finish();
            return rReturn.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K GetEditorFirstDataByInternalKey(string sInternalKey)
        {
            //BTBBenchmark.Start();
            K rReturn = null;
            K[] rDatas = GetEditorDatasByInternalKey(sInternalKey);
            if (rDatas.Length > 0)
            {
                rReturn = rDatas[0];
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region RAW
        //-------------------------------------------------------------------------------------------------------------
        public static List<K> GetRawDatasList()
        {
            return QuickFilterDatas(BasisHelper().Datas, null, null);
        }
        //-------------------------------------------------------------------------------------------------------------
        // ANCIEN GetAllObjects()
        public static K[] GetRawDatas()
        {
            //BTBBenchmark.Start();
            K[] rReturn = GetRawDatasList().ToArray() as K[];
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K GetRawFirstData()
        {
            //BTBBenchmark.Start();
            K rReturn = null;
            K[] rDatas = GetRawDatas();
            if (rDatas.Length > 0)
            {
                rReturn = rDatas[0];
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K GetRawDataByReference(string sReference, bool sTryOnDisk = false)
        {
            //BTBBenchmark.Start();
            K rReturn = null;
            if (BasisHelper().DatasByReference.ContainsKey(sReference))
            {
                rReturn = BasisHelper().DatasByReference[sReference] as K;
            }
            else
            {
                if (sTryOnDisk == true)
                {
                    // TODO : Lag connection : look for quick solution
                    rReturn = LoadDataByReference(sReference);
                }
            }
            rReturn = QuickFilter(rReturn, null, null);
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K[] GetRawDatasByInternalKey(string sInternalKey)
        {
            //BTBBenchmark.Start();
            List<K> rReturn;
            if (BasisHelper().DatasByInternalKey.ContainsKey(sInternalKey))
            {
                rReturn = BasisHelper().DatasByInternalKey[sInternalKey] as List<K>;
            }
            else
            {
                rReturn = new List<K>();
            }
            rReturn = QuickFilterDatas(rReturn as List<NWDTypeClass>, null, null);
            //BTBBenchmark.Finish();
            return rReturn.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K GetRawFirstDataByInternalKey(string sInternalKey)
        {
            //BTBBenchmark.Start();
            K rReturn = null;
            K[] rDatas = GetRawDatasByInternalKey(sInternalKey);
            if (rDatas.Length > 0)
            {
                rReturn = rDatas[0];
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        #region CORPORATE Get datas for by account and by gamesave
        //-------------------------------------------------------------------------------------------------------------
        public static List<K> GetCorporateDatasList(string sAccountReference = null, NWDGameSave sGameSave = null)
        {
            return QuickFilterDatas(BasisHelper().Datas, sAccountReference, sGameSave);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K[] GetCorporateDatas(string sAccountReference = null, NWDGameSave sGameSave = null)
        {
            return QuickFilterDatas(BasisHelper().Datas, sAccountReference, sGameSave).ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K GetCorporateFirstData(string sAccountReference = null, NWDGameSave sGameSave = null)
        {
            //BTBBenchmark.Start();
            K rReturn = null;
            K[] rDatas = GetCorporateDatas(sAccountReference, sGameSave);
            if (rDatas.Length > 0)
            {
                rReturn = rDatas[0];
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K GetCorporateDataByReference(string sReference, string sAccountReference = null, NWDGameSave sGameSave = null, bool sTryOnDisk = false)
        {
            //BTBBenchmark.Start();
            K rReturn = null;
            if (BasisHelper().DatasByReference.ContainsKey(sReference))
            {
                rReturn = BasisHelper().DatasByReference[sReference] as K;
            }
            else
            {
                if (sTryOnDisk == true)
                {
                    // TODO : Lag connection : look for quick solution
                    rReturn = LoadDataByReference(sReference);
                }
            }
            rReturn = QuickFilter(rReturn, sAccountReference, sGameSave);
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K[] GetCorporateDatasByInternalKey(string sInternalKey, string sAccountReference = null, NWDGameSave sGameSave = null)
        {
            //BTBBenchmark.Start();
            List<K> rReturn;
            if (BasisHelper().DatasByInternalKey.ContainsKey(sInternalKey))
            {
                rReturn = BasisHelper().DatasByInternalKey[sInternalKey] as List<K>;
            }
            else
            {
                rReturn = new List<K>();
            }
            rReturn = QuickFilterDatas(rReturn as List<NWDTypeClass>, sAccountReference, sGameSave);
            //BTBBenchmark.Finish();
            return rReturn.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K GetCorporateFirstDataByInternalKey(string sInternalKey, string sAccountReference = null, NWDGameSave sGameSave = null)
        {
            //BTBBenchmark.Start();
            K rReturn = null;
            K[] rDatas = GetCorporateDatasByInternalKey(sInternalKey);
            if (rDatas.Length > 0)
            {
                rReturn = rDatas[0];
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        #region REACHABLE Get datas for my account and my gamesave
        //-------------------------------------------------------------------------------------------------------------
        public static List<K> GetReachableDatasList(bool sLimitByGameSave = true)
        {
            //BTBBenchmark.Start();
            NWDGameSave tGameSave = null;
            if (sLimitByGameSave == true)
            {
                tGameSave = NWDGameSave.CurrentData();
            }
            //BTBBenchmark.Finish();
            return GetCorporateDatasList(NWDAccount.CurrentReference(), tGameSave);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K[] GetReachableDatas(bool sLimitByGameSave = true)
        {
            //BTBBenchmark.Start();
            NWDGameSave tGameSave = null;
            if (sLimitByGameSave == true)
            {
                tGameSave = NWDGameSave.CurrentData();
            }
            //BTBBenchmark.Finish();
            return GetCorporateDatas(NWDAccount.CurrentReference(), tGameSave);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K GetReachableFirstData(bool sLimitByGameSave = true)
        {
            //BTBBenchmark.Start();
            NWDGameSave tGameSave = null;
            if (sLimitByGameSave == true)
            {
                tGameSave = NWDGameSave.CurrentData();
            }
            //BTBBenchmark.Finish();
            return GetCorporateFirstData(NWDAccount.CurrentReference(), tGameSave);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K GetReachableDataByReference(string sReference, bool sLimitByGameSave = true, bool sTryOnDisk = false)
        {
            //BTBBenchmark.Start();
            NWDGameSave tGameSave = null;
            if (sLimitByGameSave == true)
            {
                tGameSave = NWDGameSave.CurrentData();
            }
            //BTBBenchmark.Finish();
            return GetCorporateDataByReference(sReference, NWDAccount.CurrentReference(), tGameSave, sTryOnDisk);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K[] GetReacheableDatasByInternalKey(string sInternalKey, bool sLimitByGameSave = true)
        {
            //BTBBenchmark.Start();
            NWDGameSave tGameSave = null;
            if (sLimitByGameSave == true)
            {
                tGameSave = NWDGameSave.CurrentData();
            }
            //BTBBenchmark.Finish();
            return GetCorporateDatasByInternalKey(sInternalKey, NWDAccount.CurrentReference(), tGameSave);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K GetReacheableFirstDataByInternalKey(string sInternalKey, bool sLimitByGameSave = true)
        {
            //BTBBenchmark.Start();
            NWDGameSave tGameSave = null;
            if (sLimitByGameSave == true)
            {
                tGameSave = NWDGameSave.CurrentData();
            }
            K rReturn = GetCorporateFirstDataByInternalKey(sInternalKey, NWDAccount.CurrentReference(), tGameSave);
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion

        #region PRIVATE FILTER
        //-------------------------------------------------------------------------------------------------------------
        private static K QuickFilter(K sData, string sAccountReference = null, NWDGameSave sGameSave = null)
        {
            //BTBBenchmark.Start();
            K rReturn = null;
            if (sData != null)
            {
                //if (sData.IsTrashed() == false || sData.IsEnable() == true || sData.TestIntegrityResult == true)
                if (sData.IsEnable() == true)
                {
                    bool tInsert = true;
                    if (BasisHelper().kAccountDependent)
                    {
                        if (sGameSave != null)
                        {
                            // test game save if necessary
                            if (BasisHelper().GameSaveMethod != null && sGameSave != null)
                            {
                                string tGameIndex = sGameSave.Reference;
                                var tValue = BasisHelper().ClassGameDependentProperties.GetValue(sData, null);
                                if (tValue == null)
                                {
                                    tValue = string.Empty;
                                }
                                string tSaveIndex = BasisHelper().GameSaveMethod.Invoke(tValue, null) as string;
                                if (tSaveIndex != tGameIndex)
                                {
                                    tInsert = false;
                                }
                            }
                        }
                        if (tInsert == true && string.IsNullOrEmpty(sAccountReference) == false)
                        {
                            tInsert = false; // research by default false and true when found first solution
                            foreach (KeyValuePair<PropertyInfo, MethodInfo> tInfos in BasisHelper().AccountMethodDico)
                            {
                                var tValue = tInfos.Key.GetValue(sData, null);
                                string tAccountValue = tInfos.Value.Invoke(tValue, null) as string;
                                if (tAccountValue.Contains(sAccountReference))
                                {
                                    tInsert = true;
                                    break; // I fonud one solution! this user can see this informations
                                }
                            }
                        }
                    }
                    if (tInsert == true)
                    {
                        rReturn = sData;
                    }
                }
                else
                {

                }
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private static List<K> QuickFilterDatas(List<NWDTypeClass> sDatasArray, string sAccountReference = null, NWDGameSave sGameSave = null)
        {
            //BTBBenchmark.Start();
            List<K> rList = new List<K>();
            //Debug.Log("chercher les data ");
            if (sDatasArray != null)
            {
                foreach (K tData in sDatasArray)
                {
                    K tDataReturn = QuickFilter(tData, sAccountReference, sGameSave);
                    if (tDataReturn != null)
                    {
                        rList.Add(tDataReturn);
                    }
                }
            }
            else
            {
                //Debug.Log("chercher les data a un tableau vide");
            }
            //BTBBenchmark.Finish();
            return rList;
        }
        //-------------------------------------------------------------------------------------------------------------
        //private static K[] FilterDatas(List<NWDTypeClass> sDatasArray,
        //                        string sAccountReference = null, // use default account
        //                        NWDGameSave sGameSave = null,// use default gamesave
        //                        NWDSwitchTrashed sTrashed = NWDSwitchTrashed.NoTrashed,
        //                        NWDSwitchEnable sEnable = NWDSwitchEnable.Enable,
        //                        NWDSwitchIntegrity sIntegrity = NWDSwitchIntegrity.Integrity
        //                        )
        //{
        //    List<K> rList = new List<K>();
        //    //Debug.Log("chercher les data ");
        //    if (sDatasArray != null)
        //    {
        //        if (BasisHelper().kAccountDependent)
        //        {
        //            // autofill sAccountReference if necessary
        //            if (string.IsNullOrEmpty(sAccountReference))
        //            {
        //                sAccountReference = NWDAccount.CurrentReference();
        //            }
        //            //Debug.Log("chercher les data pour " + sAccountReference + " ");
        //        }
        //        if (BasisHelper().ClassGameSaveDependent)
        //        {
        //            if (sGameSave == null)
        //            {
        //                sGameSave = NWDGameSave.SelectCurrentDataForAccount(sAccountReference);
        //            }
        //            //Debug.Log("chercher les data pour " + sAccountReference + " Dans la gamesave " + sGameSave.Reference);
        //        }


        //        foreach (K tDatas in sDatasArray)
        //        {
        //            bool tInsert = true;

        //            switch (sTrashed)
        //            {
        //                case NWDSwitchTrashed.NoTrashed:
        //                    {
        //                        if (tDatas.IsTrashed() == true)
        //                        {
        //                            tInsert = false;
        //                        }
        //                    }
        //                    break;
        //                case NWDSwitchTrashed.Trashed:
        //                    {
        //                        if (tDatas.IsTrashed() == false)
        //                        {
        //                            tInsert = false;
        //                        }
        //                    }
        //                    break;
        //            }

        //            switch (sEnable)
        //            {
        //                case NWDSwitchEnable.Disable:
        //                    {
        //                        if (tDatas.IsEnable() == true)
        //                        {
        //                            tInsert = false;
        //                        }
        //                    }
        //                    break;
        //                case NWDSwitchEnable.Enable:
        //                    {
        //                        if (tDatas.IsEnable() == false)
        //                        {
        //                            tInsert = false;
        //                        }
        //                    }
        //                    break;
        //            }

        //            switch (sIntegrity)
        //            {
        //                case NWDSwitchIntegrity.Cracked:
        //                    {
        //                        if (tDatas.TestIntegrity() == true)
        //                        {
        //                            tInsert = false;
        //                        }
        //                    }
        //                    break;
        //                case NWDSwitchIntegrity.Integrity:
        //                    {
        //                        if (tDatas.TestIntegrity() == false)
        //                        {
        //                            tInsert = false;
        //                        }
        //                    }
        //                    break;
        //            }
        //            if (tInsert == true)
        //            {
        //                if (BasisHelper().kAccountDependent)
        //                {
        //                    // test game save if necessary
        //                    if (BasisHelper().GameSaveMethod != null && sGameSave != null)
        //                    {
        //                        string tGameIndex = sGameSave.Reference;
        //                        var tValue = BasisHelper().ClassGameDependentProperties.GetValue(tDatas, null);
        //                        if (tValue == null)
        //                        {
        //                            tValue = string.Empty;
        //                        }
        //                        string tSaveIndex = BasisHelper().GameSaveMethod.Invoke(tValue, null) as string;
        //                        if (tSaveIndex != tGameIndex)
        //                        {
        //                            tInsert = false;
        //                        }
        //                    }
        //                    if (tInsert == true)
        //                    {
        //                        tInsert = false; // research by default false and true when found first solution
        //                        foreach (KeyValuePair<PropertyInfo, MethodInfo> tInfos in BasisHelper().AccountMethodDico)
        //                        {
        //                            var tValue = tInfos.Key.GetValue(tDatas, null);
        //                            string tAccountValue = tInfos.Value.Invoke(tValue, null) as string;
        //                            if (tAccountValue.Contains(sAccountReference))
        //                            {
        //                                tInsert = true;
        //                                break; // I fonud one solution! this user can see this informations
        //                            }
        //                        }
        //                    }
        //                }
        //                else
        //                {

        //                }
        //            }
        //            if (tInsert == true)
        //            {
        //                rList.Add(tDatas);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        //Debug.Log("chercher les data a un tableau vide");
        //    }
        //    return rList.ToArray();
        //}
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        #region LOAD PARTIAL

        //-------------------------------------------------------------------------------------------------------------
        private static K LoadDataByReference(string sReference)
        {
            Debug.Log("LoadDataByReference(" + sReference + ")");
            BTBBenchmark.Start();
            K rReturn = null;
            NWDBasisHelper tTypeInfos = NWDBasisHelper.FindTypeInfos(ClassType());
            tTypeInfos = BasisHelper();
            if (tTypeInfos.DatasByReference.ContainsKey(sReference) == false)
            {
                SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
                if (AccountDependent())
                {
                    tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
                }
                if (tSQLiteConnection != null)
                {
                    if (tSQLiteConnection.IsValid())
                    {
                        List<K> tSelect = tSQLiteConnection.Query<K>("SELECT * FROM " + tTypeInfos.ClassNamePHP + " WHERE `" + NWDToolbox.PropertyName(() => FictiveData().Reference) + "` = '" + sReference + "';");
                        if (tSelect != null)
                        {
                            foreach (NWDBasis<K> tItem in tSelect)
                            {
                                rReturn = tItem as K;
                                tItem.LoadedFromDatabase();
#if UNITY_EDITOR
                                tItem.RowAnalyze();
#endif
                            }
                        }
                    }
                }
            }
            BTBBenchmark.Finish();
#if UNITY_EDITOR
            BasisHelper().FilterTableEditor();
            BasisHelper().RepaintTableEditor();
#endif
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void LoadDataToSync(NWDAppEnvironment sEnvironment)
        {
            BTBBenchmark.Start();
            NWDBasisHelper tTypeInfos = NWDBasisHelper.FindTypeInfos(ClassType());
            tTypeInfos = BasisHelper();
            SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
            if (AccountDependent())
            {
                tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
            }
            if (tSQLiteConnection != null)
            {
                if (tSQLiteConnection.IsValid())
                {
                    //SQLiteCommand tCommand = tSQLiteConnection.CreateCommand("SELECT `Reference` FROM " + tTypeInfos.ClassNamePHP + " WHERE `"+ sEnvironment.Environment + "Sync` = '0' OR `"+ sEnvironment.Environment + "Sync` = '1';");
                    //List<string> tSelect = tCommand.ExecuteQuery<string>();
                    string tQuery = "SELECT `" + NWDToolbox.PropertyName(() => FictiveData().Reference) + "` FROM " + tTypeInfos.ClassNamePHP + " WHERE `" + sEnvironment.Environment + "Sync` = '0' OR `" + sEnvironment.Environment + "Sync` = '1';";
                    //Debug.Log(tQuery);
                    SQLiteCommand tCreateCommand = tSQLiteConnection.CreateCommand(tQuery);
                    List<NWDTypeClassReference> tSelect = tCreateCommand.ExecuteQuery<NWDTypeClassReference>();
                    if (tSelect != null)
                    {
                        foreach (NWDTypeClassReference tReference in tSelect)
                        {
                            //Debug.Log("tReference = " + tReference.Reference);
                            if (tReference.Reference != null)
                            {
                                GetRawDataByReference(tReference.Reference);
                            }
                        }
                    }
                }
            }
            BTBBenchmark.Finish();
#if UNITY_EDITOR
            BasisHelper().FilterTableEditor();
            BasisHelper().RepaintTableEditor();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        #region SELECT
        //-------------------------------------------------------------------------------------------------------------
        public static K[] SelectDatasWhereRequest(string sWhere = "`AC`=1;")
        {
            BTBBenchmark.Start();
            List<K> rResult = new List<K>();
            NWDBasisHelper tTypeInfos = NWDBasisHelper.FindTypeInfos(ClassType());
            tTypeInfos = BasisHelper();
            SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
            if (AccountDependent())
            {
                tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
            }
            if (tSQLiteConnection != null)
            {
                if (tSQLiteConnection.IsValid())
                {
                    string tQuery = "SELECT `" + NWDToolbox.PropertyName(() => FictiveData().Reference) + "` FROM " + tTypeInfos.ClassNamePHP + " WHERE " + sWhere;
                    Debug.Log(tQuery);
                    SQLiteCommand tCreateCommand = tSQLiteConnection.CreateCommand(tQuery);
                    List<NWDTypeClassReference> tSelect = tCreateCommand.ExecuteQuery<NWDTypeClassReference>();
                    if (tSelect != null)
                    {
                        foreach (NWDTypeClassReference tReference in tSelect)
                        {
                            if (tReference.Reference != null)
                            {
                                K tData = GetRawDataByReference(tReference.Reference, true);
                                if (tData != null)
                                {
                                    rResult.Add(tData);
                                }
                            }
                        }
                    }
                }
            }
            BTBBenchmark.Finish();
#if UNITY_EDITOR
            BasisHelper().FilterTableEditor();
            BasisHelper().RepaintTableEditor();
#endif
            return rResult.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        #region TOOLS
        //-------------------------------------------------------------------------------------------------------------
        public override bool IsReacheableByGameSave(NWDGameSave sGameSave)
        {
            bool rReturn = false;
            if (BasisHelper().GameSaveMethod != null)
            {
                string tGameIndex = "";
                if (sGameSave != null)
                {
                    tGameIndex = sGameSave.Reference;
                }
                var tValue = BasisHelper().ClassGameDependentProperties.GetValue(this, null);
                string tSaveIndex = BasisHelper().GameSaveMethod.Invoke(tValue, null) as string;
                if (tSaveIndex == tGameIndex)
                {
                    rReturn = true;
                }
            }
            else
            {
                rReturn = true;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool VisibleByGameSave(string sGameSaveReference)
        {
            bool rReturn = false;
            if (BasisHelper().GameSaveMethod != null)
            {
                var tValue = BasisHelper().ClassGameDependentProperties.GetValue(this, null);
                if (tValue != null)
                {
                    string tSaveIndex = BasisHelper().GameSaveMethod.Invoke(tValue, null) as string;
                    if (tSaveIndex == sGameSaveReference)
                    {
                        rReturn = true;
                    }
                }
            }
            else
            {
                rReturn = true;
            }
            return rReturn;
        }
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override bool VisibleByAccountByEqual(string sAccountReference)
        {
            bool rReturn = false;
            if (AccountDependent())
            {
                foreach (KeyValuePair<PropertyInfo, MethodInfo> tInfos in BasisHelper().AccountMethodDico)
                {
                    var tValue = tInfos.Key.GetValue(this, null);
                    if (tValue != null)
                    {
                        string tAccount = tInfos.Value.Invoke(tValue, null) as string;
                        if (tAccount == sAccountReference)
                        {
                            rReturn = true;
                            break; // I fonud one solution! this user can see this informations
                        }
                    }
                }
            }
            else
            {
                // non account dependency return acces is true.
                rReturn = true;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool VisibleByAccount(string sAccountReference)
        {
            bool rReturn = false;
            if (AccountDependent())
            {
                foreach (KeyValuePair<PropertyInfo, MethodInfo> tInfos in BasisHelper().AccountMethodDico)
                {
                    var tValue = tInfos.Key.GetValue(this, null);
                    if (tValue != null)
                    {
                        string tAccount = tInfos.Value.Invoke(tValue, null) as string;
                        if (tAccount.Contains(sAccountReference))
                        {
                            rReturn = true;
                            break; // I fonud one solution! this user can see this informations
                        }
                    }
                }
            }
            else
            {
                // non account dependency return acces is true.
                rReturn = true;
            }
            return rReturn;
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        public override bool IsReacheableByAccount(string sAccountReference = null)
        {
            bool rReturn = false;
            if (AccountDependent())
            {
                // is account dependency : get all value and test
                if (sAccountReference == null || sAccountReference == "") // TODO : replace by  string.IsNullOrEmpty
                {
                    sAccountReference = NWDAccount.CurrentReference();
                }
                foreach (KeyValuePair<PropertyInfo, MethodInfo> tInfos in BasisHelper().AccountMethodDico)
                {
                    var tValue = tInfos.Key.GetValue(this, null);
                    if (tValue != null)
                    {
                        string tAccount = tInfos.Value.Invoke(tValue, null) as string;
                        if (tAccount.Contains(sAccountReference))
                        {
                            rReturn = true;
                            break; // I fonud one solution! this user can see this informations
                        }
                    }
                }
            }
            else
            {
                // non account dependency return acces is true.
                rReturn = true;
            }
            return rReturn;
        }//-------------------------------------------------------------------------------------------------------------
         //#if UNITY_EDITOR
         //        public static Dictionary<string, string> EditorDatasMenu()
         //        {
         //            return BasisHelper().EditorDatasMenu;
         //        }
         //#endif
         //-------------------------------------------------------------------------------------------------------------
         /// <summary>
         /// News the get all datas. IT S A GLOBAL ACCESS!!!!
         /// </summary>
         /// <returns>The get all datas.</returns>
         /// AllDatas
        //public static K[] NEW_GetAllDatas()
        //{
        //    return Datas().Datas.ToArray() as K[];
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// News the get all datas. IT S A GLOBAL ACCESS!!!!
        /// </summary>
        /// <returns>The get all datas.</returns>
        //private static K[] NEW_GetAllDatasByInternalKey(string sInternalKey)
        //{
        //    K[] rReturn = new K[0];
        //    if (Datas().DatasByInternalKey.ContainsKey(sInternalKey) == true)
        //    {
        //        rReturn = Datas().DatasByInternalKey[sInternalKey].ToArray() as K[];
        //    }
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //[NWDAliasMethod(NWDConstants.M_IndexAll)]
        //public static void IndexAll()
        //{
        //    NWDBasisHelper tTypeInfos = NWDBasisHelper.FindTypeInfos(ClassType());
        //    foreach (NWDTypeClass tObject in tTypeInfos.Datas)
        //    {
        //        tObject.Index();
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        //        [NWDAliasMethod(NWDConstants.M_LoadFromDatabase)]
        //        public static void LoadFromDatabase()
        //        {
        //            NWDBasisHelper tTypeInfos = NWDBasisHelper.FindTypeInfos(ClassType());
        //            tTypeInfos = BasisHelper();
        //#if UNITY_EDITOR
        //            tTypeInfos.RowAnalyzed = false;
        //#endif
        //            tTypeInfos.ResetDatas();
        //            SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
        //            if (AccountDependent())
        //            {
        //                tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
        //            }
        //            if (tSQLiteConnection != null)
        //            {
        //                if (tSQLiteConnection.IsValid())
        //                {
        //                    List<K> tSelect = tSQLiteConnection.Query<K>("SELECT * FROM " + tTypeInfos.ClassNamePHP);
        //                    int tCount = 0;
        //                    // Prepare the datas
        //                    if (tSelect != null)
        //                    {
        //                        foreach (NWDBasis<K> tItem in tSelect)
        //                        {
        //                            tCount++;
        //                            tItem.LoadedFromDatabase();
        //                        }
        //                    }
        //                }
        //            }
        //            //Debug.Log("NWDBasis<K> LoadFromDatabase() tEnumerable tCount :" + tCount.ToString());
        //#if UNITY_EDITOR
        //            BasisHelper().New_FilterTableEditor();
        //            BasisHelper().New_RepaintTableEditor();
        //#endif
        //    //BTBBenchmark.Finish("LoadFromDatabase " + tTypeInfos.ClassNamePHP);
        //}
        //-------------------------------------------------------------------------------------------------------------
        //        public static void UnloadDataByReference(string sReference)
        //        {
        //            //Debug.Log("UnloadDataByReference(" + sReference + ")");
        //            NWDBasisHelper tTypeInfos = NWDBasisHelper.FindTypeInfos(ClassType());
        //            tTypeInfos = BasisHelper();
        //            if (tTypeInfos.DatasByReference.ContainsKey(sReference))
        //            {
        //                NWDTypeClass tData = tTypeInfos.DatasByReference[sReference];
        //                tData.Desindex(); // call override method
        //                tTypeInfos.RemoveData(tData);
        //                tData.Delete();
        //            }
        //#if UNITY_EDITOR
        //            BasisHelper().New_FilterTableEditor();
        //            BasisHelper().New_RepaintTableEditor();
        //#endif
        //}
        //-------------------------------------------------------------------------------------------------------------
        public override bool DataIntegrityState()
        {
            return TestIntegrity();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public override bool TrashState()
        //{
        //    if (XX > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        public override void TrashAction()
        {
            TrashData();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public override bool EnableState()
        //{
        //    return AC;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public override bool ReachableState()
        //{
        //    return IsReacheableByAccount();
        //}
        //-------------------------------------------------------------------------------------------------------------

        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    class NWDFinderTEST
    {
        //-------------------------------------------------------------------------------------------------------------
        static void FindDatas()
        {
            NWDExample.GetRawDatas();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

}
//=====================================================================================================================