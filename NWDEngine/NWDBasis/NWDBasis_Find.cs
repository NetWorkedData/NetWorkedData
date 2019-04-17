// =====================================================================================================================
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
// =====================================================================================================================

using System.Collections.Generic;
using System.Reflection;
using BasicToolBox;
using SQLite4Unity3d;

#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
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
            return BasisHelper().Datas as List<K>;
        }
        //-------------------------------------------------------------------------------------------------------------
        // ANCIEN GetAllObjects()
        public static K[] GetEditorDatas()
        {
            //BTBBenchmark.Start();
            K[] rReturn = BasisHelper().Datas.ToArray() as K[];
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<K> GetEditorDatasByInternalKey(string sInternalKey)
        {
            List<K> rReturn;
            if (BasisHelper().DatasByInternalKey.ContainsKey(sInternalKey))
            {
                rReturn = BasisHelper().DatasByInternalKey[sInternalKey] as List<K>;
            }
            else
            {
                rReturn = new List<K>();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K GetEditorDataByReference(string sReference, bool sTryOnDisk = false)
        {
            K rReturn = null;
            if (BasisHelper().DatasByReference.ContainsKey(sReference))
            {
                rReturn = BasisHelper().DatasByReference[sReference] as K;
            }
            else
            {
                if (sTryOnDisk == true)
                {
                    rReturn = LoadDataByReference(sReference);
                }
            }
            return rReturn;
        }


        //-------------------------------------------------------------------------------------------------------------
#endif
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region RAW
        //-------------------------------------------------------------------------------------------------------------
        public static List<K> RawDatasList()
        {
            return BasisHelper().Datas as List<K>;
        }
        //-------------------------------------------------------------------------------------------------------------
        // ANCIEN GetAllObjects()
        public static K[] RawDatas()
        {
            //BTBBenchmark.Start();
            K[] rReturn = BasisHelper().Datas.ToArray() as K[];
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<K> RawDatasByInternalKey(string sInternalKey)
        {
            List<K> rReturn;
            if (BasisHelper().DatasByInternalKey.ContainsKey(sInternalKey))
            {
                rReturn = BasisHelper().DatasByInternalKey[sInternalKey] as List<K>;
            }
            else
            {
                rReturn = new List<K>();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K RawDataByReference(string sReference, bool sTryOnDisk = false)
        {
            K rReturn = null;
            if (BasisHelper().DatasByReference.ContainsKey(sReference))
            {
                rReturn = BasisHelper().DatasByReference[sReference] as K;
            }
            else
            {
                if (sTryOnDisk == true)
                {
                    // TODO : fait lagguer la connection : trouver une solution
                    rReturn = LoadDataByReference(sReference);
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        #region REACHABLE Get datas for my account and my gamesave
        //-------------------------------------------------------------------------------------------------------------
        public static K[] GetReachableDatas()
        {
            return GetCorporateDatas(NWDAccount.CurrentReference(), NWDGameSave.CurrentData());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K GetReachableFirstData()
        {
            return GetCorporateFirstData(NWDAccount.CurrentReference(), NWDGameSave.CurrentData());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K GetReachableDataByReference(string sReference)
        {
            return GetCorporateDataByReference(sReference, NWDAccount.CurrentReference());
            //return FilterDataByReference(sReference, NWDAccount.CurrentReference(), NWDGameSave.CurrentData());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K[] GetReacheableDatasByInternalKey(string sInternalKey, bool sCreateIfNotExists = false, NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            return GetCorporateDatasByInternalKey(sInternalKey, sCreateIfNotExists, sWritingMode, NWDAccount.CurrentReference(), NWDGameSave.CurrentData());
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        #region CORPORATE Get datas for by account and by gamesave
        //-------------------------------------------------------------------------------------------------------------
        // ANCIEN GetAllObjects()
        /// <summary>
        /// News the get data by reference.
        /// </summary>
        /// <returns>The get data by reference.</returns>
        /// <param name="sReference">S reference.</param>
        public static K GetCorporateDataByReference(string sReference, string sAccountReference = null, NWDGameSave sGameSave = null)
        {
            K rReturn = null;
            if (string.IsNullOrEmpty(sReference) == false)
            {
                if (string.IsNullOrEmpty(sAccountReference))
                {
                    sAccountReference = NWDAccount.CurrentReference();
                }
                if (BasisHelper().DatasByReference != null)
                {
                    if (BasisHelper().DatasByReference.ContainsKey(sReference))
                    {
                        K tObject = BasisHelper().DatasByReference[sReference] as K;
                        if (tObject.IsReacheableByAccount(sAccountReference))
                        {
                            rReturn = tObject;
                        }
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K[] GetCorporateDatas(string sAccountReference = null, // use default account
                                NWDGameSave sGameSave = null, // use default gamesave
                                NWDSwitchTrashed sTrashed = NWDSwitchTrashed.NoTrashed,
                                NWDSwitchEnable sEnable = NWDSwitchEnable.Enable,
                                NWDSwitchIntegrity sIntegrity = NWDSwitchIntegrity.Integrity
                                )
        {
            //BTBBenchmark.Start();
            //Debug.Log("Datas() Datas count = " + Datas().Datas.Count);
            K[] rReturn = FilterDatas(BasisHelper().Datas, sAccountReference, sGameSave, sTrashed, sEnable, sIntegrity);
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K GetCorporateFirstData(string sAccountReference = null, NWDGameSave sGameSave = null)
        {
            K rReturn = null;
            K[] rDatas = GetCorporateDatas(sAccountReference, sGameSave);
            if (rDatas.Length > 0)
            {
                rReturn = rDatas[0];
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K[] GetCorporateDatasByInternalKey(
                                        string sInternalKey,
                                        bool sCreateIfNotExists = false,
            NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal,
                                        string sAccountReference = null,
                                        NWDGameSave sGameSave = null,
                                        NWDSwitchTrashed sTrashed = NWDSwitchTrashed.NoTrashed,
                                        NWDSwitchEnable sEnable = NWDSwitchEnable.Enable,
                                        NWDSwitchIntegrity sIntegrity = NWDSwitchIntegrity.Integrity
                                       )
        {
            List<NWDTypeClass> tTestList = new List<NWDTypeClass>();
            sInternalKey = NWDToolbox.TextProtect(sInternalKey);
            if (BasisHelper().DatasByInternalKey.ContainsKey(sInternalKey) == true)
            {
                tTestList.AddRange(BasisHelper().DatasByInternalKey[sInternalKey]);
            }
            if (BasisHelper().kAccountDependent)
            {
                // autofill sAccountReference if necessary
                if (string.IsNullOrEmpty(sAccountReference))
                {
                    sAccountReference = NWDAccount.CurrentReference();
                }
                //Debug.Log("chercher les data pour " + sAccountReference + " ");
            }
            if (BasisHelper().ClassGameSaveDependent)
            {
                if (sGameSave == null)
                {
                    sGameSave = NWDGameSave.SelectCurrentDataForAccount(sAccountReference);
                }
                //Debug.Log("chercher les data pour " + sAccountReference + " Dans la gamesave " + sGameSave.Reference);
            }

            K[] rArray = FilterDatas(tTestList, sAccountReference, sGameSave, sTrashed, sEnable, sIntegrity);
            if (sCreateIfNotExists == true && rArray.Length == 0)
            {
                //Debug.Log(" must create object !");
                if (sAccountReference == null || sAccountReference == NWDAccount.CurrentReference())
                {
                    if (sGameSave == NWDGameSave.CurrentData())
                    {
                        //Debug.Log("Creat Ok");
                        K rReturn = NewData(sWritingMode);
                        rReturn.InternalKey = sInternalKey;
                        rReturn.UpdateData(true, sWritingMode);
                        rArray = new K[1] { rReturn };
                    }
                    else
                    {
                        Debug.Log("create not possinble in another gamesave!");
                    }
                }
                else
                {
                    Debug.Log("create not possible with another account!");
                }
            }
            return rArray;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K FilterFirstDataByInternalKey(
                                        string sInternalKey,
                                        bool sCreateIfNotExists = false,
                                         NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal,
                                        string sAccountReference = null,
                                        NWDGameSave sGameSave = null,
                                        NWDSwitchTrashed sTrashed = NWDSwitchTrashed.NoTrashed,
                                        NWDSwitchEnable sEnable = NWDSwitchEnable.Enable,
                                        NWDSwitchIntegrity sIntegrity = NWDSwitchIntegrity.Integrity
                                       )
        {
            K rReturn = null;

            K[] rDatas = GetCorporateDatasByInternalKey(sInternalKey, sCreateIfNotExists, sWritingMode, sAccountReference, sGameSave, sTrashed, sEnable, sIntegrity);
            if (rDatas.Length > 0)
            {
                rReturn = rDatas[0];
            }
            return rReturn;
        }

        //-------------------------------------------------------------------------------------------------------------
        private static K[] FilterDatas(List<NWDTypeClass> sDatasArray,
                                string sAccountReference = null, // use default account
                                NWDGameSave sGameSave = null,// use default gamesave
                                NWDSwitchTrashed sTrashed = NWDSwitchTrashed.NoTrashed,
                                NWDSwitchEnable sEnable = NWDSwitchEnable.Enable,
                                NWDSwitchIntegrity sIntegrity = NWDSwitchIntegrity.Integrity
                                )
        {
            List<K> rList = new List<K>();
            //Debug.Log("chercher les data ");
            if (sDatasArray != null)
            {
                if (BasisHelper().kAccountDependent)
                {
                    // autofill sAccountReference if necessary
                    if (string.IsNullOrEmpty(sAccountReference))
                    {
                        sAccountReference = NWDAccount.CurrentReference();
                    }
                    //Debug.Log("chercher les data pour " + sAccountReference + " ");
                }
                if (BasisHelper().ClassGameSaveDependent)
                {
                    if (sGameSave == null)
                    {
                        sGameSave = NWDGameSave.SelectCurrentDataForAccount(sAccountReference);
                    }
                    //Debug.Log("chercher les data pour " + sAccountReference + " Dans la gamesave " + sGameSave.Reference);
                }


                foreach (K tDatas in sDatasArray)
                {
                    bool tInsert = true;

                    switch (sTrashed)
                    {
                        case NWDSwitchTrashed.NoTrashed:
                            {
                                if (tDatas.IsTrashed() == true)
                                {
                                    tInsert = false;
                                }
                            }
                            break;
                        case NWDSwitchTrashed.Trashed:
                            {
                                if (tDatas.IsTrashed() == false)
                                {
                                    tInsert = false;
                                }
                            }
                            break;
                    }

                    switch (sEnable)
                    {
                        case NWDSwitchEnable.Disable:
                            {
                                if (tDatas.IsEnable() == true)
                                {
                                    tInsert = false;
                                }
                            }
                            break;
                        case NWDSwitchEnable.Enable:
                            {
                                if (tDatas.IsEnable() == false)
                                {
                                    tInsert = false;
                                }
                            }
                            break;
                    }

                    switch (sIntegrity)
                    {
                        case NWDSwitchIntegrity.Cracked:
                            {
                                if (tDatas.TestIntegrity() == true)
                                {
                                    tInsert = false;
                                }
                            }
                            break;
                        case NWDSwitchIntegrity.Integrity:
                            {
                                if (tDatas.TestIntegrity() == false)
                                {
                                    tInsert = false;
                                }
                            }
                            break;
                    }
                    if (tInsert == true)
                    {
                        if (BasisHelper().kAccountDependent)
                        {
                            // test game save if necessary
                            if (BasisHelper().GameSaveMethod != null && sGameSave != null)
                            {
                                string tGameIndex = sGameSave.Reference;
                                var tValue = BasisHelper().ClassGameDependentProperties.GetValue(tDatas, null);
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
                            if (tInsert == true)
                            {
                                tInsert = false; // research by default false and true when found first solution
                                foreach (KeyValuePair<PropertyInfo, MethodInfo> tInfos in BasisHelper().AccountMethodDico)
                                {
                                    var tValue = tInfos.Key.GetValue(tDatas, null);
                                    string tAccountValue = tInfos.Value.Invoke(tValue, null) as string;
                                    if (tAccountValue.Contains(sAccountReference))
                                    {
                                        tInsert = true;
                                        break; // I fonud one solution! this user can see this informations
                                    }
                                }
                            }
                        }
                        else
                        {

                        }
                    }
                    if (tInsert == true)
                    {
                        rList.Add(tDatas);
                    }
                }
            }
            else
            {
                //Debug.Log("chercher les data a un tableau vide");
            }
            return rList.ToArray();
        }
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
            BasisHelper().New_FilterTableEditor();
            BasisHelper().New_RepaintTableEditor();
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
                                RawDataByReference(tReference.Reference);
                            }
                        }
                    }
                }
            }
            BTBBenchmark.Finish();
#if UNITY_EDITOR
            BasisHelper().New_FilterTableEditor();
            BasisHelper().New_RepaintTableEditor();
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
                                K tData = RawDataByReference(tReference.Reference, true);
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
            BasisHelper().New_FilterTableEditor();
            BasisHelper().New_RepaintTableEditor();
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
            NWDExample.RawDatas();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

}
//=====================================================================================================================