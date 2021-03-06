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
using System.Collections.Generic;
using UnityEngine;
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEditor;
using NetWorkedData.NWDEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        //public const string SynchronizeKeyTimestamp = "sync";
        //-------------------------------------------------------------------------------------------------------------
        public NWDTypeClass SynchronizationInsertInBase(NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, string[] sDataArray)
        {
            //Debug.Log("SynchronizationInsertInBase ");
            string tReference = GetReferenceValueFromCSV(sDataArray);
            NWDTypeClass tObject = GetDataByReference(tReference);
            if (tObject == null)
            {
                //Debug.Log("SynchronizationTryToUse () NEW DATA Reference " + tReference);
                sInfos.RowAddedCounter++;
                tObject = NewDataFromWeb(sEnvironment, sDataArray, tReference);
                //AddObjectInListOfEdition(tObject);
                //if (kAccountDependent == true)
                if (TemplateHelper.GetAccountDependent() != NWDTemplateAccountDependent.NoAccountDependent)
                {
                    if (tObject.IsTrashed() == true && NWDAppConfiguration.SharedInstance().AutoDeleteTrashDatas == true)
                    {
                        NWDDebug.Log("DATA With Reference " + tObject.Reference + " will be deleted ... just after insert");
                        tObject.DeleteData();
                    }
                }
            }
            else
            {
                //Debug.Log("SynchronizationTryToUse () OLD DATA Reference " + tReference);
                string tActualIntegrity = GetIntegrityValueFromCSV(sDataArray);
                if (tObject.Integrity != tActualIntegrity)
                {
                    // Ok integrity is != I will update data
                    sInfos.RowUpdatedCounter++;
                }
                // BUT I NEED ANY WAY TO REWRITE THE SYNC DATE!!!!
                // TO DO use Unity Editor to switch write  before or not ?
                tObject.UpdateDataFromWeb(sEnvironment, sDataArray);
                //if (kAccountDependent == true)
                if (TemplateHelper.GetAccountDependent() != NWDTemplateAccountDependent.NoAccountDependent)
                {
                    if (tObject.IsTrashed() == true && NWDAppConfiguration.SharedInstance().AutoDeleteTrashDatas == true)
                    {
                        NWDDebug.Log("DATA With Reference " + tObject.Reference + " will be deleted");
                        tObject.DeleteData();
                    }
                }
            }
#if UNITY_EDITOR
            tObject.ErrorCheck();
#endif
            return tObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTypeClass SynchronizationTryToUse(NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, string sData, bool sForceToUse = false)
        {
            //Debug.Log("SynchronizationTryToUse ()");
            //NWDBenchmark.Start();
            NWDTypeClass rReturn = null;
            string[] tDataArray = sData.Split(NWDConstants.kStandardSeparator.ToCharArray());
            for (int tI = 0; tI < tDataArray.Length; tI++)
            {
                tDataArray[tI] = NWDToolbox.TextCSVUnprotect(tDataArray[tI]);
            }
#if NWD_INTEGRITY_NONE
            rReturn = SynchronizationInsertInBase(sInfos, sEnvironment, tDataArray);
#else
            // I need to test the integrity of datas... 
            bool tIntegrityTest = true;
            tIntegrityTest = TestIntegrityValueFromCSV(tDataArray);
            if (tIntegrityTest == true || sForceToUse == true)
            {
                rReturn = SynchronizationInsertInBase(sInfos, sEnvironment, tDataArray);
            }
#endif
            //NWDBenchmark.Finish();
            return rReturn;
        }


        //-------------------------------------------------------------------------------------------------------------
        //public Dictionary<string, object> SynchronizationGetNewData(NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, bool sForceAll)
        //{
        //    //Debug.Log("NWDBasis SynchronizationSignActionData() " + ClassName);
        //    // create respond object
        //    Dictionary<string, object> rSend = new Dictionary<string, object>();
        //    // create dictionnary for this tablename and insert in the respond
        //    Dictionary<string, object> rSendDatas = new Dictionary<string, object>();
        //    rSend.Add(ClassTableName, rSendDatas);
        //    // get last synchro
        //    int tLastSynchronization = SynchronizationGetLastTimestamp(sEnvironment);
        //    if (sForceAll==true)
        //    {
        //        tLastSynchronization = 0;
        //    }
        //    // I get all objects 
        //    rSendDatas.Add(NWD.K_WEB_ACTION_SYNC_KEY, tLastSynchronization);
        //    rSendDatas.Add(NWD.K_WEB_WEBSIGN_KEY, WebServiceSign(LastWebBuild));
        //    // return the data
        //    return rSend;
        //}

        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, object> SynchronizationPushData(NWDOperationResult sInfos,
            NWDAppEnvironment sEnvironment,
            Dictionary<Type, List<string>> sTypeAndReferences,
            bool sForceAll,
            NWDOperationSpecial sSpecial)
        {
            //Debug.Log("NWDBasis SynchronizationPushData() " + ClassName);
            // loads data unloaded but not sync
            if (sSpecial == NWDOperationSpecial.None)
            {
                //TODO Tht lag the webservice
                //LoadDataToSync(sEnvironment);
            }
            bool tSync = true;
            //if (Application.isPlaying && kAccountDependent != true)
            if (Application.isPlaying && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
            {
                tSync = false;
            }
            // create respond object
            Dictionary<string, object> rSend = new Dictionary<string, object>(new StringIndexKeyComparer());
            // create dictionnary for this tablename and insert in the respond
            Dictionary<string, object> rSendDatas = new Dictionary<string, object>(new StringIndexKeyComparer());
            rSend.Add(ClassTableName, rSendDatas);
            // create List with all object to synchron on the server
            // create List 
            List<object> tDatas = new List<object>();
            // get last synchro
            int tLastSynchronization = SynchronizationGetLastTimestamp(sEnvironment);
            // I get all objects 
            //IEnumerable<K> tResults = null;
            List<NWDTypeClass> tResults = new List<NWDTypeClass>();
            //TODO: BUT IF SYNC = TIME ?
            if (sForceAll == true)
            {
                tLastSynchronization = 0; // ok you force, then, upload and then download ALL datas since 1970 (0)
                if (sSpecial == NWDOperationSpecial.None)
                {
                    if (NWDAppConfiguration.SharedInstance().BundleDatas == true)
                    {
                        LoadFromDatabase(string.Empty, false);
                    }
                    else
                    {
                        if (IsLoaded() == true)
                        {
                            LoadFromDatabase(string.Empty, false);
                        }
                    }
                    foreach (NWDTypeClass tO in Datas)
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
                    if (tSync == true)
                    {
                        if (NWDAppConfiguration.SharedInstance().BundleDatas == true)
                        {
                            //if (AllBundleLoaded == false)
                            {
                                LoadFromDatabase("WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DM) + "` >= " + SynchronizationGetLastTimestamp(sEnvironment) + ";", false);
                            }
                        }
                        foreach (NWDTypeClass tO in Datas)
                        {
                            if (tO.DevSync == 0 || tO.DevSync == 1 /*|| tO.AddonSyncForce()*/)
                            {
                                tResults.Add(tO);
                            }
                        }
                    }
                }
            }
            else if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {
                //tResults = tSQLiteConnection.Table<K>().Where(x => x.PreprodSync == 0);
                if (sSpecial == NWDOperationSpecial.None)
                {
                    if (tSync == true)
                    {
                        if (NWDAppConfiguration.SharedInstance().BundleDatas == true)
                        {
                            LoadFromDatabase("WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DM) + "` >= " + SynchronizationGetLastTimestamp(sEnvironment) + ";", false);
                        }
                        foreach (NWDTypeClass tO in Datas)
                        {
                            if (tO.PreprodSync == 0 || tO.PreprodSync == 1 /*|| tO.AddonSyncForce()*/)
                            {
                                tResults.Add(tO);
                            }
                        }
                    }
                }
            }
            else if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                //tResults = tSQLiteConnection.Table<K>().Where(x => x.ProdSync == 0);
                if (sSpecial == NWDOperationSpecial.None)
                {
                    if (tSync == true)
                    {
                        if (NWDAppConfiguration.SharedInstance().BundleDatas == true)
                        {
                            LoadFromDatabase("WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DM) + "` >= " + SynchronizationGetLastTimestamp(sEnvironment) + ";", false);
                        }
                        foreach (NWDTypeClass tO in Datas)
                        {
                            if (tO.ProdSync == 0 || tO.ProdSync == 1 /*|| tO.AddonSyncForce()*/)
                            {
                                tResults.Add(tO);
                            }
                        }
                    }
                }
            }
            if (sSpecial == NWDOperationSpecial.PushReference)
            {
                // addd reference in result
                if (sTypeAndReferences.ContainsKey(ClassType))
                {
                    foreach (string tReference in sTypeAndReferences[ClassType])
                    {
                        NWDTypeClass tObject = GetDataByReference(tReference);
                        if (tObject != null)
                        {
                            LoadFromDatabaseByReference(tReference, false);
                        }
                        if (tObject != null)
                        {
                            tResults.Add(tObject);
                        }
                    }
                }
            }


            if (tResults != null)
            {
                foreach (NWDTypeClass tItem in tResults)
                {
                    // I test integrity and if object is locked (only editable by editor)
                    // I add test if WebService is not outgrade for this object 
                    if (tItem.IntegrityIsValid() && tItem.IsLockedObject() == false && tItem.WebserviceVersionIsValid() == true)
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
                            if (tItem.IsWritableBy(null, NWDAccount.CurrentReference()))
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
                        //if (kAccountDependent == true)
                        if (TemplateHelper.GetAccountDependent() != NWDTemplateAccountDependent.NoAccountDependent)
                            {
                            if (tItem.IsWritableBy(null, NWDAccount.CurrentReference()))
                            {
                                sInfos.RowPushCounter++;
                                tDatas.Add(tItem.CSVAssembly());
                            }
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
                    rSendDatas.Add(NWD.K_WEB_DATA_ROW_COUNTER, tDatas.Count);
                    rSendDatas.Add(NWD.K_WEB_DATA_KEY, tDatas);
                }
            }

            if (sSpecial == NWDOperationSpecial.None)
            {
                rSendDatas.Add(NWD.K_WEB_ACTION_SYNC_KEY, tLastSynchronization);
            }
            else if (sSpecial == NWDOperationSpecial.Pull)
            {
                rSendDatas.Add(NWD.K_WEB_ACTION_SYNC_KEY, 0);
                rSendDatas.Add(sSpecial.ToString().ToLower(), "true");
            }
            else if (sSpecial == NWDOperationSpecial.PullReference)
            {
                if (sTypeAndReferences.ContainsKey(ClassType))
                {
                    rSendDatas.Add(NWD.K_WEB_ACTION_REF_KEY, sTypeAndReferences[ClassType]);
                    rSendDatas.Add(sSpecial.ToString().ToLower(), "true");
                }
            }
            else if (sSpecial == NWDOperationSpecial.PushReference)
            {
                if (sTypeAndReferences.ContainsKey(ClassType))
                {
                    rSendDatas.Add(NWD.K_WEB_ACTION_REF_KEY, sTypeAndReferences[ClassType]);
                    rSendDatas.Add(sSpecial.ToString().ToLower(), "true");
                }
            }
            else
            {
                rSendDatas.Add(sSpecial.ToString().ToLower(), "true");
            }
            rSendDatas.Add(NWD.K_WEB_WEBSIGN_KEY, WebServiceSign(LastWebBuild));
            // return the data
            //Debug.Log ("SynchronizationPushData for table " + TableName () +" rSend = " + rSend.ToString ());
            return rSend;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations the pull data.
        /// </summary>
        /// <param name="sData">S data.</param>
        public bool SynchronizationPullData(NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, NWDOperationResult sData, NWDOperationSpecial sSpecial)
        {
            //Debug.Log("NWDBasis SynchronizationPullData() " + ClassTableName);
            //NWDBenchmark.Start();
            //NWDBenchmark.Tag(ClassNamePHP());
            bool rReturn = false;
            // Ok I receive data ... so I can reccord the last waiting timestamp as the good sync date
            if (sData.isError)
            {
                // error to show on Device
                // Debug.LogWarning("NWDBasis SynchronizationPullData() ERROR IN DATAS FOR" + ClassName());
            }
            else
            {
                // I try to use this data to ... insert/update/delete/... ?
                bool tForceToUse = false;
#if UNITY_EDITOR
                tForceToUse = true;
#endif
                int tTimestampServer = sData.timestamp;
                if (sSpecial == NWDOperationSpecial.None)
                {
                    SynchronizationSetNewTimestamp(sEnvironment, tTimestampServer);
                }
                else
                {
                    // it's not a sync  ... no reccord the timestamp return for last sync
                }
                // now i need get only datas for this class tablename
                string tTableName = ClassTableName;
                // Ok I need to compute all datas for this Class tablename
                if (sData.param.ContainsKey(tTableName))
                {
                    //#if UNITY_EDITOR
                    sInfos.ClassPullCounter++;
                    //#endif
                    Dictionary<string, object> tClassResult = sData.param[tTableName] as Dictionary<string, object>;
                    List<object> tListOfRows = null;
                    if (tClassResult.ContainsKey(NWD.K_WEB_DATA_KEY))
                    {
                        tListOfRows = tClassResult[NWD.K_WEB_DATA_KEY] as List<object>;
                        //NWDBenchmark.Increment(tListOfRows.Count);
                        //Debug.Log("TEST DATAS : " + tListOfRows.Count + " rows");
                        //NWDBenchmark.Start("TEST DATAS");
                        if (tListOfRows.Count > 0)
                        {
                            //Debug.Log("NWDBasis SynchronizationPullData() find "+tListOfRows.Count+" row for " + ClassName());
                            foreach (object tCsvValue in tListOfRows)
                            {
                                string tCsvValueString = tCsvValue as string;

                                sInfos.RowPullCounter++;
                                NWDTypeClass tObject = SynchronizationTryToUse(sInfos, sEnvironment, tCsvValueString, tForceToUse);
                                // trash this object ?
                                //if (tObject != null)
                                //{
                                //    FlushTrash(tObject);
                                //}
                            }

                            rReturn = true;
#if UNITY_EDITOR
                            //FilterTableEditor();
                            RepaintTableEditor();
                            NWDDataInspector.ShareInstance().Repaint();
#endif
                        }
                        //NWDBenchmark.Finish("TEST DATAS");
                    }
                    else
                    {
                        //if (sEnvironment.LogMode == true)
                        //{
                        //    //Debug.Log(tTableName + " just sync with timestamp ?");
                        //}
                    }
                }
            }
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SynchronizationFromWebService(NWEOperationBlock sSuccessBlock = null,
         NWEOperationBlock sErrorBlock = null,
         NWEOperationBlock sCancelBlock = null,
         NWEOperationBlock sProgressBlock = null,
         bool sForce = false,
         bool sPriority = false)
        {
            if (Application.isPlaying == true)
            {
                NWDOperationWebSynchronisation.AddOperation("Sync " + ClassNamePHP, sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, null, ClasseInThisSync(), null, sForce, sPriority);
            }
            else
            {
#if UNITY_EDITOR
                NWDAppEnvironmentSyncContent.SharedInstance().OperationSynchro(null, ClasseInThisSync(), null, false, false, NWDOperationSpecial.None);
#endif
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        public bool PullFromWebService(NWDAppEnvironment sEnvironment)
        {
            bool rReturn = false;
#if UNITY_EDITOR
            if (Application.isPlaying == true)
            {
                NWDDataManager.SharedInstance().AddWebRequestPull(ClasseInThisSync(), true, sEnvironment);
            }
            else
            {
                NWDAppEnvironmentSyncContent.SharedInstance().OperationSynchro(sEnvironment, ClasseInThisSync(), null, false, false, NWDOperationSpecial.Pull);
                NWDOperationWebhook.NewEditorOperation(sEnvironment, "Sync pull operation", ClasseInThisSync(), WebHookType.Sync);
            }
#else
            NWDDataManager.SharedInstance().AddWebRequestPull(ClasseInThisSync(), true, sEnvironment);
#endif
            rReturn = true;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations from web service.
        /// </summary>
        public bool PullFromWebServiceForce(NWDAppEnvironment sEnvironment)
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
                NWDAppEnvironmentSyncContent.SharedInstance().OperationSynchro(sEnvironment, ClasseInThisSync(), null, true, false, NWDOperationSpecial.Pull);
                NWDOperationWebhook.NewEditorOperation(sEnvironment, "Sync pull force datas operation", ClasseInThisSync(), WebHookType.Sync);
            }
#else
            NWDDataManager.SharedInstance().AddWebRequestPullForce(ClasseInThisSync(), true, sEnvironment);
#endif
            rReturn = true;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations from web service.
        /// </summary>
        public bool PullFromWebServiceReferences(NWDAppEnvironment sEnvironment, Dictionary<Type, List<string>> sTypeAndReferences)
        {
            bool rReturn = false;
#if UNITY_EDITOR
            if (Application.isPlaying == true)
            {
                NWDDataManager.SharedInstance().AddWebRequestPullReferences(sTypeAndReferences, true, sEnvironment);
            }
            else
            {

                NWDAppEnvironmentSyncContent.SharedInstance().OperationSynchro(sEnvironment, null, sTypeAndReferences, true, false, NWDOperationSpecial.PullReference);
                NWDOperationWebhook.NewEditorOperation(sEnvironment, "Sync pull references operation", ClasseInThisSync(), WebHookType.Sync);
            }
#else
            NWDDataManager.SharedInstance().AddWebRequestPullReferences(sTypeAndReferences, true, sEnvironment);
#endif
            rReturn = true;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool PushFromWebServiceReferences(NWDAppEnvironment sEnvironment, Dictionary<Type, List<string>> sTypeAndReferences)
        {
            bool rReturn = false;
#if UNITY_EDITOR
            if (Application.isPlaying == true)
            {
                NWDDataManager.SharedInstance().AddWebRequestPushReferences(sTypeAndReferences, true, sEnvironment);
            }
            else
            {

                NWDAppEnvironmentSyncContent.SharedInstance().OperationSynchro(sEnvironment, null, sTypeAndReferences, true, false, NWDOperationSpecial.PushReference);
                NWDOperationWebhook.NewEditorOperation(sEnvironment, "Sync push references operation", ClasseInThisSync(), WebHookType.Sync);
            }
#else
            NWDDataManager.SharedInstance().AddWebRequestPushReferences(sTypeAndReferences, true, sEnvironment);
#endif
            rReturn = true;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations from web service.
        /// </summary>
        public bool SynchronizationFromWebServiceForce(NWDAppEnvironment sEnvironment)
        {
            bool rReturn = false;
#if UNITY_EDITOR
            //NWDAppEnvironmentSync.SharedInstance().StartProcess(sEnvironment);
            if (Application.isPlaying == true)
            {
                //if (Application.isPlaying == true && kAccountDependent == false)
                if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                {
                    EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                }
                NWDDataManager.SharedInstance().AddWebRequestSynchronizationForce(ClasseInThisSync(), true, sEnvironment);
            }
            else
            {
                NWDAppEnvironmentSyncContent.SharedInstance().OperationSynchro(sEnvironment, ClasseInThisSync(), null, true, true, NWDOperationSpecial.None);
                NWDOperationWebhook.NewEditorOperation(sEnvironment, "Sync force datas operation", ClasseInThisSync(), WebHookType.Sync);
            }
#else
            NWDDataManager.SharedInstance().AddWebRequestSynchronizationForce(ClasseInThisSync(), true, sEnvironment);
#endif
            rReturn = true;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations from web service.
        /// </summary>
        public bool SynchronizationFromWebService(NWDAppEnvironment sEnvironment)
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
                NWDAppEnvironmentSyncContent.SharedInstance().OperationSynchro(sEnvironment, ClasseInThisSync(), null, false, false, NWDOperationSpecial.None);
                NWDOperationWebhook.NewEditorOperation(sEnvironment, "Sync datas operation", ClasseInThisSync(), WebHookType.Sync);
            }
#else
            NWDDataManager.SharedInstance().AddWebRequestSynchronization(ClasseInThisSync(), true, sEnvironment);
#endif
            rReturn = true;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations from web service.
        /// </summary>
//#if UNITY_EDITOR
//        public bool SynchronizationFromWebServiceClean(NWDAppEnvironment sEnvironment)
//        {
//            bool rReturn = false;
//            //NWDAppEnvironmentSync.SharedInstance().StartProcess(sEnvironment);
//            if (Application.isPlaying == true)
//            {
//            }
//            else
//            {
//                NWDAppEnvironmentSyncContent.SharedInstance().OperationSynchro(sEnvironment, ClasseInThisSync(), null, false, false, NWDOperationSpecial.Clean);
//                NWDOperationWebhook.NewEditorOperation(sEnvironment, "Clean operation", ClasseInThisSync(), WebHookType.Sync);
//            }
//            rReturn = true;
//            return rReturn;
//        }
//#endif
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations from web service.
        /// </summary>
#if UNITY_EDITOR
        public bool SynchronizationFromWebServiceSpecial(NWDAppEnvironment sEnvironment, NWDOperationSpecial sSpecial)
        {
            bool rReturn = false;
            if (Application.isPlaying == true)
            {
            }
            else
            {
                //NWDEditorMenu.EnvironementSync().SynchronizationSpecial(ClasseInThisSync(), sEnvironment, sSpecial);
                NWDAppEnvironmentSyncContent.SharedInstance().OperationSynchro(sEnvironment, ClasseInThisSync(), null, false, false, sSpecial);
                //NWDOperationWebhook.NewEditorOperation(sEnvironment, ":wrench: Special operation : " + sSpecial.ToString(), ClasseInThisSync(), WebHookType.Sync); ;
                NWDOperationWebhook.NewEditorOperation(sEnvironment, ":hammer_and_wrench: Special operation : " + sSpecial.ToString(), ClasseInThisSync(), WebHookType.Sync); ;
            }
            rReturn = true;
            return rReturn;
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
