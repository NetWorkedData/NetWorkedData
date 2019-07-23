//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:25:5
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
using SQLite4Unity3d;
using BasicToolBox;

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
        //public const string SynchronizeKeyTimestamp = "sync";
        //-------------------------------------------------------------------------------------------------------------
        public NWDTypeClass SynchronizationInsertInBase(NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, string[] sDataArray)
        {
            //Debug.Log("SynchronizationInsertInBase ");
            string tReference = GetReferenceValueFromCSV(sDataArray);
            NWDTypeClass tObject = GetDataByReference(tReference);
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
                string tActualIntegrity =  GetIntegrityValueFromCSV(sDataArray);
                if (tObject.Integrity != tActualIntegrity)
                {
                    // Ok integrity is != I will update data
                    sInfos.RowUpdatedCounter++;
                }
                // BUT I NEED ANY WAY TO REWRITE THE SYNC DATE!!!!
                // TO DO use Unity Editor to switch write  before or not ?
                tObject.UpdateDataFromWeb(sEnvironment, sDataArray);
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

            NWDTypeClass rReturn = null;
            string[] tDataArray = sData.Split(NWDConstants.kStandardSeparator.ToCharArray());
            for (int tI = 0; tI < tDataArray.Length; tI++)
            {
                tDataArray[tI] = NWDToolbox.TextCSVUnprotect(tDataArray[tI]);
            }
            // I need to test the integrity of datas... 
            bool tIntegrityTest =  TestIntegrityValueFromCSV(tDataArray);
            if (tIntegrityTest == false)
            {
#if UNITY_EDITOR
                //Datas().CSVAssemblyOrderArrayPrepare();
                Debug.Log("SynchronizationTryToUse INTEGRITY IS FALSE " + ClassTableName + " \n" + string.Join("|", sData + "\n"));
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
        public Dictionary<string, object> SynchronizationPushData(NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, bool sForceAll, NWDOperationSpecial sSpecial)
        {
            //Debug.Log("NWDBasis SynchronizationPushData() " + ClassName);
            // loads data unloaded but not sync
            if (sSpecial == NWDOperationSpecial.None)
            {
                //TODO Tht lag the webservice
                //LoadDataToSync(sEnvironment);
            }
            // create respond object
            Dictionary<string, object> rSend = new Dictionary<string, object>();
            // create dictionnary for this tablename and insert in the respond
            Dictionary<string, object> rSendDatas = new Dictionary<string, object>();
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
                if (sSpecial != NWDOperationSpecial.Pull)
                {
                    LoadFromDatabase();
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
                    foreach (NWDTypeClass tO in Datas)
                    {
                        if (tO.DevSync == 0 || tO.DevSync == 1 /*|| tO.AddonSyncForce()*/)
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
                    foreach (NWDTypeClass tO in Datas)
                    {
                        if (tO.PreprodSync == 0 || tO.PreprodSync == 1 /*|| tO.AddonSyncForce()*/)
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
                    foreach (NWDTypeClass tO in Datas)
                    {
                        if (tO.ProdSync == 0 || tO.ProdSync == 1 /*|| tO.AddonSyncForce()*/)
                        {
                            tResults.Add(tO);
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
                    rSendDatas.Add(NWD.K_WEB_DATA_ROW_COUNTER, tDatas.Count);
                    rSendDatas.Add(NWD.K_WEB_DATA_KEY, tDatas);
                }
            }
            if (sSpecial != NWDOperationSpecial.None)
            {
                rSendDatas.Add(sSpecial.ToString().ToLower(), "true");
            }
            rSendDatas.Add(NWD.K_WEB_ACTION_SYNC_KEY, tLastSynchronization);
            // return the data
            //Debug.Log ("SynchronizationPushData for table " + TableName () +" rSend = " + rSend.ToString ());
            return rSend;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations the pull data.
        /// </summary>
        /// <param name="sData">S data.</param>
        public string SynchronizationPullData(NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, NWDOperationResult sData, NWDOperationSpecial sSpecial)
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
                                NWDTypeClass tObject = SynchronizationTryToUse(sInfos, sEnvironment, tCsvValueString, tForceToUse);
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
                    else
                    {
                        //if (sEnvironment.LogMode == true)
                        //{
                        //    //Debug.Log(tTableName + " just sync with timestamp ?");
                        //}
                    }
                }
            }
            //BTBBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SynchronizationFromWebService(BTBOperationBlock sSuccessBlock = null,
         BTBOperationBlock sErrorBlock = null,
         BTBOperationBlock sCancelBlock = null,
         BTBOperationBlock sProgressBlock = null,
         bool sForce = false,
         bool sPriority = false)
        {
            if (Application.isPlaying == true)
            {
                NWDOperationWebSynchronisation.AddOperation("Sync " + ClassNamePHP, sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, null, ClasseInThisSync(), sForce, sPriority);
            }
            else
            {
#if UNITY_EDITOR
                NWDAppEnvironmentSync.SharedInstance().OperationSynchro(null,  ClasseInThisSync(), false, false, NWDOperationSpecial.None);
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
                NWDAppEnvironmentSync.SharedInstance().OperationSynchro(sEnvironment,  ClasseInThisSync(), false, false, NWDOperationSpecial.Pull);
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
                NWDAppEnvironmentSync.SharedInstance().OperationSynchro(sEnvironment,  ClasseInThisSync(), true, false, NWDOperationSpecial.Pull);
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
        public bool SynchronizationFromWebServiceForce(NWDAppEnvironment sEnvironment)
        {
            bool rReturn = false;
#if UNITY_EDITOR
            //NWDAppEnvironmentSync.SharedInstance().StartProcess(sEnvironment);
            if (Application.isPlaying == true)
            {
                if (Application.isPlaying == true && kAccountDependent == false)
                {
                    EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                }
                NWDDataManager.SharedInstance().AddWebRequestSynchronizationForce(ClasseInThisSync(), true, sEnvironment);
            }
            else
            {
                NWDAppEnvironmentSync.SharedInstance().OperationSynchro(sEnvironment,  ClasseInThisSync(), true, true, NWDOperationSpecial.None);
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
                //NWDEditorMenu.EnvironementSync().Synchronization(ClasseInThisSync(), sEnvironment);
                NWDAppEnvironmentSync.SharedInstance().OperationSynchro(sEnvironment,  ClasseInThisSync(), false, false, NWDOperationSpecial.None);
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
        public bool SynchronizationFromWebServiceClean(NWDAppEnvironment sEnvironment)
        {
            bool rReturn = false;
#if UNITY_EDITOR
            //NWDAppEnvironmentSync.SharedInstance().StartProcess(sEnvironment);
            if (Application.isPlaying == true)
            {
                //NWDEditorMenu.EnvironementSync().SynchronizationClean(ClasseInThisSync(), sEnvironment);
                NWDAppEnvironmentSync.SharedInstance().OperationSynchro(sEnvironment,  ClasseInThisSync(), false, false, NWDOperationSpecial.Clean);
            }
            else
            {

                //NWDEditorMenu.EnvironementSync().SynchronizationClean(ClasseInThisSync(), sEnvironment);

                NWDAppEnvironmentSync.SharedInstance().OperationSynchro(sEnvironment,  ClasseInThisSync(), false, false, NWDOperationSpecial.Clean);
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
        public bool SynchronizationFromWebServiceSpecial(NWDAppEnvironment sEnvironment, NWDOperationSpecial sSpecial)
        {
            bool rReturn = false;
            //NWDAppEnvironmentSync.SharedInstance().StartProcess(sEnvironment);
            if (Application.isPlaying == true)
            {
            }
            else
            {
                //NWDEditorMenu.EnvironementSync().SynchronizationSpecial(ClasseInThisSync(), sEnvironment, sSpecial);
                NWDAppEnvironmentSync.SharedInstance().OperationSynchro(sEnvironment,  ClasseInThisSync(), false, false, sSpecial);
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