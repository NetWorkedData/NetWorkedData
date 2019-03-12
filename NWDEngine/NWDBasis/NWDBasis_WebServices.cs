//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using SQLite4Unity3d;
using BasicToolBox;
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
        public const string SynchronizeKeyData = "data";
        public const string SynchronizeKeyDataCount = "rowCount";
        public const string SynchronizeKeyTimestamp = "sync";
        public const string SynchronizeKeyLastTimestamp = "last";
        public const string SynchronizeKeyInWaitingTimestamp = "waiting";
        //-------------------------------------------------------------------------------------------------------------
        #region Synchronization informations
        //-------------------------------------------------------------------------------------------------------------
        public bool IsSynchronized()
        {
            int tD = 0;
            if (NWDAppConfiguration.SharedInstance().IsDevEnvironement())
            {
                tD = DevSync;
            }
            else if (NWDAppConfiguration.SharedInstance().IsPreprodEnvironement())
            {
                tD = PreprodSync;
            }
            else if (NWDAppConfiguration.SharedInstance().IsProdEnvironement())
            {
                tD = ProdSync;
            }

            if (tD > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual int WebModelToUse()
        {
            //int tWebBuildUsed = NWDAppConfiguration.SharedInstance().WebBuild;
            //if (NWDAppConfiguration.SharedInstance().kLastWebBuildClass.ContainsKey(ClassType()))
            //{
            //    tWebBuildUsed = NWDAppConfiguration.SharedInstance().kLastWebBuildClass[ClassType()];
            //}

            int tWebBuildUsed = BasisHelper().LastWebBuild;
            return tWebBuildUsed;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Synchronization informations
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations the prefs key.
        /// </summary>
        /// <returns>The prefs key.</returns>
        public static string SynchronizationPrefsKey(NWDAppEnvironment sEnvironment)
        {
            // use the accountReference with prefbase key associated with environement and key time 
            if (AccountDependent())
            {
                return sEnvironment.PlayerAccountReference + BasisHelper().ClassPrefBaseKey + sEnvironment.Environment + SynchronizeKeyLastTimestamp;
            }

            return BasisHelper().ClassPrefBaseKey + sEnvironment.Environment + SynchronizeKeyLastTimestamp;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SynchronizationUpadteTimestamp()
        {
            Debug.Log(BasisHelper().ClassNamePHP + " must be reset the timestamp of last sync to the build tiemstamp");
            SynchronizationResetTimestamp(NWDAppEnvironment.SelectedEnvironment());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SynchronizationResetTimestamp(NWDAppEnvironment sEnvironment)
        {
#if UNITY_EDITOR
            EditorPrefs.SetInt(SynchronizationPrefsKey(sEnvironment), sEnvironment.BuildTimestamp);
#else
			PlayerPrefs.SetInt (SynchronizationPrefsKey(sEnvironment), sEnvironment.BuildTimestamp);
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SynchronizationSetToZeroTimestamp()
        {
#if UNITY_EDITOR
            EditorPrefs.SetInt(SynchronizationPrefsKey(NWDAppConfiguration.SharedInstance().DevEnvironment), 0);
            EditorPrefs.SetInt(SynchronizationPrefsKey(NWDAppConfiguration.SharedInstance().PreprodEnvironment), 0);
            EditorPrefs.SetInt(SynchronizationPrefsKey(NWDAppConfiguration.SharedInstance().ProdEnvironment), 0);
#else
            PlayerPrefs.SetInt (SynchronizationPrefsKey(NWDAppConfiguration.SharedInstance().DevEnvironment), 0);
            PlayerPrefs.SetInt (SynchronizationPrefsKey(NWDAppConfiguration.SharedInstance().PreprodEnvironment), 0);
            PlayerPrefs.SetInt (SynchronizationPrefsKey(NWDAppConfiguration.SharedInstance().ProdEnvironment), 0);
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations the get last timestamp.
        /// </summary>
        /// <returns>The get last timestamp.</returns>
        public static int SynchronizationGetLastTimestamp(NWDAppEnvironment sEnvironment)
        {
            int rReturn = sEnvironment.BuildTimestamp;
#if UNITY_EDITOR
            rReturn = 0;
            if (EditorPrefs.HasKey(SynchronizationPrefsKey(sEnvironment)))
            {
                rReturn = EditorPrefs.GetInt(SynchronizationPrefsKey(sEnvironment));
            }
#else
			if (PlayerPrefs.HasKey(SynchronizationPrefsKey(sEnvironment)))
				{
			rReturn = PlayerPrefs.GetInt (SynchronizationPrefsKey(sEnvironment));
				};
#endif
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations the set new timestamp.
        /// </summary>
        /// <param name="sNewTimestamp">S new timestamp.</param>
        public static void SynchronizationSetNewTimestamp(NWDAppEnvironment sEnvironment, int sNewTimestamp)
        {
#if UNITY_EDITOR
            EditorPrefs.SetInt(SynchronizationPrefsKey(sEnvironment), sNewTimestamp);
#else
            if (AccountDependent() == false)
            {
                if (sNewTimestamp < sEnvironment.BuildTimestamp)
                {
                    sNewTimestamp = sEnvironment.BuildTimestamp;
                }
            }
			PlayerPrefs.SetInt (SynchronizationPrefsKey(sEnvironment), sNewTimestamp);
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Synchronization Objects
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations the insert in base.
        /// </summary>
        /// <returns>The insert in base.</returns>
        /// <param name="sDataArray">S data array.</param>
        public static NWDBasis<K> SynchronizationInsertInBase(NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, string[] sDataArray)
        {
            //Debug.Log("SynchronizationInsertInBase ");
            string tReference = GetReferenceValueFromCSV(sDataArray);
            NWDBasis<K> tObject = GetDataByReference(tReference);
            if (tObject == null)
            {
                //Debug.Log("SynchronizationTryToUse () NEW DATA");
                sInfos.RowAddedCounter++;
                tObject = NewDataFromWeb(sEnvironment, sDataArray, tReference);
                //AddObjectInListOfEdition(tObject);
            }
            else
            {
                //Debug.Log("SynchronizationTryToUse () OLD DATA");
                string tActualIntegrity = GetIntegrityValueFromCSV(sDataArray);
                if (tObject.Integrity != tActualIntegrity)
                {
                    // Ok integrity is != I will update data
                    sInfos.RowUpdatedCounter++;
                }
                // BUT I NEED ANY WAY TO REWRITE THE SYNC DATE!!!!
                // TO DO use Unity Editor to switch write  before or not ?
                tObject.UpdateDataFromWeb(sEnvironment, sDataArray);
                //tObject.UpdateWithCSV(sEnvironment, sDataArray);
            }
#if UNITY_EDITOR
            tObject.ErrorCheck();
#endif
            return tObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations the try to use.
        /// </summary>
        /// <returns>The try to use.</returns>
        /// <param name="sData">S data.</param>
        /// <param name="sForceToUse">If set to <c>true</c> s force to use.</param>
        public static NWDBasis<K> SynchronizationTryToUse(NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, string sData, bool sForceToUse = false)
        {
            //Debug.Log("SynchronizationTryToUse ()");

            NWDBasis<K> rReturn = null;
            string[] tDataArray = sData.Split(NWDConstants.kStandardSeparator.ToCharArray());
            for (int tI = 0; tI < tDataArray.Length; tI++)
            {
                tDataArray[tI] = NWDToolbox.TextCSVUnprotect(tDataArray[tI]);
            }
            // I need to test the integrity of datas... 
            bool tIntegrityTest = TestIntegrityValueFromCSV(tDataArray);
            if (tIntegrityTest == false)
            {
#if UNITY_EDITOR
                //Datas().CSVAssemblyOrderArrayPrepare();
                //Debug.Log("SynchronizationTryToUse INTEGRITY IS FALSE " + Datas().ClassTableName + " \n" + string.Join("|", CSVAssemblyOrderArray()) + "\n" + sData + "\n");
                //EditorUtility.DisplayDialog("SynchronizationTryToUse()", "INTEGRITY IS FALSE", "OK");
#endif
            }
            if (tIntegrityTest == true || sForceToUse == true)
            {
                rReturn = SynchronizationInsertInBase(sInfos, sEnvironment, tDataArray);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Synchronization Push Pull methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations the push data.
        /// </summary>
        /// <returns>The push data.</returns>
        /// <param name="sForceAll">If set to <c>true</c> s force all.</param>
        [NWDAliasMethod(NWDConstants.M_CheckoutPushData)]
        public static Dictionary<string, object> CheckoutPushData(NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, bool sForceAll, NWDOperationSpecial sSpecial = NWDOperationSpecial.None)
        {
            //Debug.Log("NWDBasis CheckoutPushData() " + ClassName());
            //SQLiteConnection tSQLiteConnection = null;
            //if (AccountDependent())
            //{
            //    tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
            //}
            //else
            //{
            //    tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
            //}
            // create respond object
            Dictionary<string, object> rSend = new Dictionary<string, object>();
            // create dictionnary for this tablename and insert in the respond
            Dictionary<string, object> rSendDatas = new Dictionary<string, object>();
            rSend.Add(BasisHelper().ClassTableName, rSendDatas);
            // create List with all object to synchron on the server
            // create List 
            //List<object> tDatas = new List<object>();
            // get last synchro
            int tLastSynchronization = SynchronizationGetLastTimestamp(sEnvironment);
            // I get force or not
            if (sForceAll == true)
            {
                tLastSynchronization = 0;
            }
            //IEnumerable<K> tResults = null;
            if (sSpecial != NWDOperationSpecial.None)
            {
                rSendDatas.Add(sSpecial.ToString().ToLower(), "true");
            }
            rSendDatas.Add(SynchronizeKeyTimestamp, tLastSynchronization);
            // return the data
            //Debug.Log ("SynchronizationPushData for table " + TableName () +" rSend = " + rSend.ToString ());
            return rSend;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations the push data.
        /// </summary>
        /// <returns>The push data.</returns>
        /// <param name="sForceAll">If set to <c>true</c> s force all.</param>
        [NWDAliasMethod(NWDConstants.M_SynchronizationPushData)]
        public static Dictionary<string, object> SynchronizationPushData(NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, bool sForceAll, NWDOperationSpecial sSpecial)
        {
            //Debug.Log("NWDBasis SynchronizationPushData() " + ClassName());
            //SQLiteConnection tSQLiteConnection = null;
            //if (AccountDependent())
            //{
            //    tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
            //}
            //else
            //{
            //    tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
            //}
            // create respond object
            Dictionary<string, object> rSend = new Dictionary<string, object>();
            // create dictionnary for this tablename and insert in the respond
            Dictionary<string, object> rSendDatas = new Dictionary<string, object>();
            rSend.Add(BasisHelper().ClassTableName, rSendDatas);
            // create List with all object to synchron on the server
            // create List 
            List<object> tDatas = new List<object>();
            // get last synchro
            int tLastSynchronization = SynchronizationGetLastTimestamp(sEnvironment);
            // I get all objects 
            //IEnumerable<K> tResults = null;
            List<K> tResults = new List<K>();
            //TODO: BUT IF SYNC = TIME ?
            if (sForceAll == true)
            {
                tLastSynchronization = 0; // ok you force, then, upload and then download ALL datas since 1970 (0)
                if (sSpecial != NWDOperationSpecial.Pull)
                {
                    foreach (K tO in BasisHelper().Datas)
                    {
                        bool tAddEnv = true;
                        if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment && tO.DevSync < 0)
                        {
                            tAddEnv = false;
                        }
                        if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment && tO.PreprodSync < 0)
                        {
                            tAddEnv = false;
                        }
                        if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment && tO.ProdSync < 0)
                        {
                            tAddEnv = false;
                        }
                        if (tAddEnv == true)
                        {
                            //if (tO.DM >= tLastSynchronization)
                            {
                                tResults.Add(tO);
                            }
                        }
                    }
                }
            }
            else if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            {
                //tResults = tSQLiteConnection.Table<K>().Where(x => x.DevSync == 0);
                if (sSpecial == NWDOperationSpecial.None)
                {
                    foreach (K tO in BasisHelper().Datas)
                    {
                        if (tO.DevSync == 0 || tO.DevSync == 1 || tO.AddonSyncForce())
                        {
                            tResults.Add(tO);
                        }
                    }
                }
            }
            else if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {
                //tResults = tSQLiteConnection.Table<K>().Where(x => x.PreprodSync == 0);
                if (sSpecial == NWDOperationSpecial.None)
                {
                    foreach (K tO in BasisHelper().Datas)
                    {
                        if (tO.PreprodSync == 0 || tO.PreprodSync == 1 || tO.AddonSyncForce())
                        {
                            tResults.Add(tO);
                        }
                    }
                }
            }
            else if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                //tResults = tSQLiteConnection.Table<K>().Where(x => x.ProdSync == 0);
                if (sSpecial == NWDOperationSpecial.None)
                {
                    foreach (K tO in BasisHelper().Datas)
                    {
                        if (tO.ProdSync == 0 || tO.ProdSync == 1 || tO.AddonSyncForce())
                        {
                            tResults.Add(tO);
                        }
                    }
                }
            }
            if (tResults != null)
            {
                foreach (NWDBasis<K> tItem in tResults)
                {
                    // I test integrity and if object is locked (only editable by editor)
                    // I add test if WebService is not outgrade for this object 
                    if (tItem.TestIntegrity() && tItem.IsLockedObject() == false && tItem.WebserviceVersionIsValid() == true)
                    {
                        // I need to update the webservice to synchronize !
                        tItem.WebserviceVersionCheckMe(); // use this method is more effectient
                        // LIMIT EXPORT IF I HAVE ACCOUNT RIGHTS !
                        // But Auhtorize if editor ....
                        // TODO Silmplify the reach editor mode ?
#if UNITY_EDITOR
                        // check if is playing or not
                        if (Application.isPlaying == true)
                        {
                            // TODO WARNING  ?
                            if (tItem.IsReacheableByAccount())
                            {
                                sInfos.RowPushCounter++;
                                tDatas.Add(tItem.CSVAssembly());
                            }
                        }
                        else
                        {
                            // Fake playing mode
                            sInfos.RowPushCounter++;
                            tDatas.Add(tItem.CSVAssembly());
                        }
#else
                        // not in editor : playing mode only
                        if (tItem.IsReacheableByAccount())
                        {
                            tDatas.Add(tItem.CSVAssembly());
                        }
#endif 
                    }
                }
                // But I insert the datas only if I had one object or more to insert/update on the server
                if (tDatas.Count > 0)
                {
                    //#if UNITY_EDITOR
                    sInfos.ClassPushCounter++;
                    //#endif
                    rSendDatas.Add(SynchronizeKeyDataCount, tDatas.Count);
                    rSendDatas.Add(SynchronizeKeyData, tDatas);
                }
            }
            if (sSpecial != NWDOperationSpecial.None)
            {
                rSendDatas.Add(sSpecial.ToString().ToLower(), "true");
            }
            rSendDatas.Add(SynchronizeKeyTimestamp, tLastSynchronization);
            // return the data
            //Debug.Log ("SynchronizationPushData for table " + TableName () +" rSend = " + rSend.ToString ());
            return rSend;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations the pull data.
        /// </summary>
        /// <param name="sData">S data.</param>
        [NWDAliasMethod(NWDConstants.M_SynchronizationPullData)]
        public static string SynchronizationPullData(NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, NWDOperationResult sData, NWDOperationSpecial sSpecial)
        {
            //Debug.Log("NWDBasis SynchronizationPullData() " + ClassName());
            //BTBBenchmark.Start();
            //BTBBenchmark.Tag(ClassNamePHP());
            string rReturn = "NO";
            // Ok I receive data ... so I can reccord the last waiting timestamp as the good sync date
            if (sData.isError)
            {
                // error to show on Device
                // Debug.LogWarning("NWDBasis SynchronizationPullData() ERROR IN DATAS FOR" + ClassName());
            }
            else
            {
                int tTimestampServer = sData.timestamp;
                if (sSpecial == NWDOperationSpecial.None)
                {
                    SynchronizationSetNewTimestamp(sEnvironment, tTimestampServer);
                }
                // now i need get only datas for this class tablename
                string tTableName = BasisHelper().ClassTableName;
                // Ok I need to compute all datas for this Class tablename
                if (sData.param.ContainsKey(tTableName))
                {
                    //#if UNITY_EDITOR
                    sInfos.ClassPullCounter++;
                    //#endif
                    List<object> tListOfRows = sData.param[tTableName] as List<object>;
                    //BTBBenchmark.Increment(tListOfRows.Count);
                    if (tListOfRows.Count > 0)
                    {
                        //Debug.Log("NWDBasis SynchronizationPullData() find "+tListOfRows.Count+" row for " + ClassName());
                        foreach (object tCsvValue in tListOfRows)
                        {
                            string tCsvValueString = tCsvValue as string;

                            // I try to use this data to ... insert/update/delete/... ?
                            bool tForceToUse = false;

#if UNITY_EDITOR
                            tForceToUse = true;
#endif
                            sInfos.RowPullCounter++;

                            NWDBasis<K> tObject = SynchronizationTryToUse(sInfos, sEnvironment, tCsvValueString, tForceToUse);

                            // trash this object ?
                            if (tObject != null)
                            {
                                FlushTrash(tObject);
                            }
                        }

                        rReturn = "YES";
#if UNITY_EDITOR
                        //FilterTableEditor();
                        RepaintTableEditor();
                        NWDDataInspector.ShareInstance().Repaint();
#endif
                    }
                }
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Synchronization WebServices
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_ClasseInThisSync)]
        public static List<Type> ClasseInThisSync()
        {
            List<Type> rReturn = null;
            MethodInfo tMethodInfo = NWDAliasMethod.GetMethod(ClassType(), NWDConstants.M_OverrideClasseInThisSync, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (tMethodInfo != null)
            {
                List<Type> tReturn = null;
                tReturn = tMethodInfo.Invoke(null, null) as List<Type>;
                tReturn.Add(typeof(K));
                rReturn = new List<Type>(tReturn.Distinct<Type>());
            }
            else
            {
                rReturn = new List<Type> { typeof(K) };
            }
            //Debug.Log("Return List<Type> containts " + rReturn.Count + " classes");
            return rReturn;
        }

        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations from web service.
        /// </summary>
        public static void SynchronizationFromWebService(BTBOperationBlock sSuccessBlock = null,
         BTBOperationBlock sErrorBlock = null,
         BTBOperationBlock sCancelBlock = null,
         BTBOperationBlock sProgressBlock = null,
         bool sForce = false,
         bool sPriority = false)
        {
            if (Application.isPlaying == true)
            {
                NWDOperationWebSynchronisation.AddOperation("Sync " + BasisHelper().ClassNamePHP , sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, null, ClasseInThisSync(), sForce, sPriority);
            }
            else
            {
#if UNITY_EDITOR
                NWDAppEnvironmentSync.SharedInstance().OperationSynchro(null, ClasseInThisSync(), false, false, NWDOperationSpecial.None);
#endif
            }
        }






        //-------------------------------------------------------------------------------------------------------------
        //public static void SynchronizeThisClasse(bool sForce = false)
        //{
        //    if (sForce == true)
        //    {
        //        NWDDataManager.SharedInstance().AddWebRequestSynchronization(ClasseInThisSync());
        //    }
        //    else
        //    {
        //        NWDDataManager.SharedInstance().AddWebRequestSynchronizationForce(ClasseInThisSync());
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public static void SynchronizeThisClasseWithBlock(bool sForce = false,
        //                                                  BTBOperationBlock sSuccessBlock = null,
        //                                                  BTBOperationBlock sErrorBlock = null,
        //                                                  BTBOperationBlock sCancelBlock = null,
        //                                                  BTBOperationBlock sProgressBlock = null,
        //                                                  bool sPriority = false,
        //                                                  NWDAppEnvironment sEnvironment = null)
        //{
        //    if (sForce == true)
        //    {
        //        NWDDataManager.SharedInstance().AddWebRequestSynchronizationWithBlock(ClasseInThisSync(),
        //                                                                             sSuccessBlock,
        //                                                                             sErrorBlock,
        //                                                                             sCancelBlock,
        //                                                                             sProgressBlock,
        //                                                                             sPriority,
        //                                                                             sEnvironment);
        //    }
        //    else
        //    {
        //        NWDDataManager.SharedInstance().AddWebRequestSynchronizationForceWithBlock(ClasseInThisSync(),
        //                                                                             sSuccessBlock,
        //                                                                             sErrorBlock,
        //                                                                             sCancelBlock,
        //                                                                             sProgressBlock,
        //                                                                             sPriority,
        //                                                                             sEnvironment);
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations from web service.
        /// </summary>
        public static bool PullFromWebService(NWDAppEnvironment sEnvironment)
        {
            bool rReturn = false;
#if UNITY_EDITOR
            //NWDAppEnvironmentSync.SharedInstance().StartProcess(sEnvironment);
            if (Application.isPlaying == true)
            {
                NWDDataManager.SharedInstance().AddWebRequestPull(ClasseInThisSync(), true, sEnvironment);
            }
            else
            {
                //NWDEditorMenu.EnvironementSync().Pull(ClasseInThisSync(), sEnvironment);
                NWDAppEnvironmentSync.SharedInstance().OperationSynchro(sEnvironment, ClasseInThisSync(), false, false, NWDOperationSpecial.Pull);
            }
#else
                NWDDataManager.SharedInstance().AddWebRequestPull (ClasseInThisSync(), true, sEnvironment);
#endif
            rReturn = true;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations from web service.
        /// </summary>
        public static bool PullFromWebServiceForce(NWDAppEnvironment sEnvironment)
        {
            bool rReturn = false;
#if UNITY_EDITOR
            //NWDAppEnvironmentSync.SharedInstance().StartProcess(sEnvironment);
            if (Application.isPlaying == true)
            {
                NWDDataManager.SharedInstance().AddWebRequestPullForce(ClasseInThisSync(), true, sEnvironment);
            }
            else
            {
                //NWDEditorMenu.EnvironementSync().PullForce(ClasseInThisSync(), sEnvironment);
                NWDAppEnvironmentSync.SharedInstance().OperationSynchro(sEnvironment, ClasseInThisSync(), true, false, NWDOperationSpecial.Pull);
            }
#else
            NWDDataManager.SharedInstance().AddWebRequestPullForce (ClasseInThisSync(), true, sEnvironment);
#endif
            rReturn = true;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations from web service.
        /// </summary>
        public static bool SynchronizationFromWebServiceForce(NWDAppEnvironment sEnvironment)
        {
            bool rReturn = false;
#if UNITY_EDITOR
            //NWDAppEnvironmentSync.SharedInstance().StartProcess(sEnvironment);
            if (Application.isPlaying == true)
            {
                if (Application.isPlaying == true && AccountDependent() == false)
                {
                    EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                }
                NWDDataManager.SharedInstance().AddWebRequestSynchronizationForce(ClasseInThisSync(), true, sEnvironment);
            }
            else
            {
                NWDAppEnvironmentSync.SharedInstance().OperationSynchro(sEnvironment, ClasseInThisSync(), true, true, NWDOperationSpecial.None);
                //NWDEditorMenu.EnvironementSync().SynchronizationForce(ClasseInThisSync(), sEnvironment);
            }
#else
				NWDDataManager.SharedInstance().AddWebRequestSynchronizationForce (ClasseInThisSync(), true, sEnvironment);
#endif
            rReturn = true;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations from web service.
        /// </summary>
        public static bool SynchronizationFromWebService(NWDAppEnvironment sEnvironment)
        {
            bool rReturn = false;
#if UNITY_EDITOR
            //NWDAppEnvironmentSync.SharedInstance().StartProcess(sEnvironment);
            if (Application.isPlaying == true)
            {
                NWDDataManager.SharedInstance().AddWebRequestSynchronization(ClasseInThisSync(), true, sEnvironment);
            }
            else
            {
                //NWDEditorMenu.EnvironementSync().Synchronization(ClasseInThisSync(), sEnvironment);
                NWDAppEnvironmentSync.SharedInstance().OperationSynchro(sEnvironment, ClasseInThisSync(), false, false, NWDOperationSpecial.None);
            }
#else
				NWDDataManager.SharedInstance().AddWebRequestSynchronization (ClasseInThisSync(), true, sEnvironment);
#endif
            rReturn = true;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations from web service.
        /// </summary>
        public static bool SynchronizationFromWebServiceClean(NWDAppEnvironment sEnvironment)
        {
            bool rReturn = false;
#if UNITY_EDITOR
            //NWDAppEnvironmentSync.SharedInstance().StartProcess(sEnvironment);
            if (Application.isPlaying == true)
            {
                //NWDEditorMenu.EnvironementSync().SynchronizationClean(ClasseInThisSync(), sEnvironment);
                NWDAppEnvironmentSync.SharedInstance().OperationSynchro(sEnvironment, ClasseInThisSync(), false, false, NWDOperationSpecial.Clean);
            }
            else
            {

                //NWDEditorMenu.EnvironementSync().SynchronizationClean(ClasseInThisSync(), sEnvironment);

                NWDAppEnvironmentSync.SharedInstance().OperationSynchro(sEnvironment, ClasseInThisSync(), false, false, NWDOperationSpecial.Clean);
                //NWDDataManager.SharedInstance().AddWebRequestSynchronizationClean(ClasseInThisSync(), true, sEnvironment);
            }
#else
#endif
            rReturn = true;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations from web service.
        /// </summary>
#if UNITY_EDITOR
        public static bool SynchronizationFromWebServiceSpecial(NWDAppEnvironment sEnvironment, NWDOperationSpecial sSpecial)
        {
            bool rReturn = false;
            //NWDAppEnvironmentSync.SharedInstance().StartProcess(sEnvironment);
            if (Application.isPlaying == true)
            {
            }
            else
            {
                //NWDEditorMenu.EnvironementSync().SynchronizationSpecial(ClasseInThisSync(), sEnvironment, sSpecial);
                NWDAppEnvironmentSync.SharedInstance().OperationSynchro(sEnvironment, ClasseInThisSync(), false, false, sSpecial);
            }
            rReturn = true;
            return rReturn;
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Special delete user
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_DeleteUser)]
        public static void DeleteUser(NWDAppEnvironment sEnvironment)
        {
            if (AccountDependent() == true)
            {
                // reset last sync to zero
                SynchronizationSetNewTimestamp(sEnvironment, 0); // set to 0 ... only for data AccountDependent, so that's not affect the not connected data (game's data)
                                                                 // delete all datas for this user
                foreach (NWDBasis<K> tObject in BasisHelper().Datas)
                {
                    if (tObject.IsReacheableByAccount(NWDAccount.GetCurrentAccountReference()))
                    {
                        tObject.DeleteData();
                    }
                }
                // need to reload this data now : to remove all tObjects from memory!
                //LoadTableEditor();
                LoadFromDatabase();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================