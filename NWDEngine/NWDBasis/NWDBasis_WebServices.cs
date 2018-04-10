﻿//=====================================================================================================================
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
        public static string SynchronizeKeyData = "data";
        public static string SynchronizeKeyDataCount = "rowCount";
        public static string SynchronizeKeyClean = "clean";
        public static string SynchronizeKeyTimestamp = "sync";
        public static string SynchronizeKeyLastTimestamp = "last";
        public static string SynchronizeKeyInWaitingTimestamp = "waiting";
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
        public virtual int WebServiceVersionToUse()
        {
            int tWebBuildUsed = NWDAppConfiguration.SharedInstance().WebBuild;
            if (NWDAppConfiguration.SharedInstance().kLastWebBuildClass.ContainsKey(ClassType()))
            {
                tWebBuildUsed = NWDAppConfiguration.SharedInstance().kLastWebBuildClass[ClassType()];
            }
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
                return sEnvironment.PlayerAccountReference + PrefBaseKey() + sEnvironment.Environment + SynchronizeKeyLastTimestamp;
            }
            else
            {
                return PrefBaseKey() + sEnvironment.Environment + SynchronizeKeyLastTimestamp;
            }
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
        /// <summary>
        /// Synchronizations the get last timestamp.
        /// </summary>
        /// <returns>The get last timestamp.</returns>
        public static int SynchronizationGetLastTimestamp(NWDAppEnvironment sEnvironment)
        {
            int rReturn = sEnvironment.BuildTimestamp;
#if UNITY_EDITOR
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
        public static NWDBasis<K> SynchronizationInsertInBase(NWDAppEnvironment sEnvironment, string[] sDataArray)
        {
            // Debug.Log("SynchronizationInsertInBase ");
            string tReference = GetReferenceValueFromCSV(sDataArray);
            NWDBasis<K> tObject = InstanceByReference(tReference);
            if (tObject == null)
            {
                //  Debug.Log("SynchronizationInsertInBase NEW OBJECT DETECTED");
                // TODO : IS RELATIONSHIP OBJECT?
                // IF NOT ... INSERT IN DATABASE
                tObject = NewInstanceFromCSV(sEnvironment, sDataArray);
                AddObjectInListOfEdition(tObject);
            }
            else
            {
                // test if Modification is older than actual object
                //  Debug.Log("SynchronizationInsertInBase JUST UPDATE OBJECT DETECTED");
                // if (tObject.DM <= GetDMValueFromCSV(sDataArray))
                //  {
                //      Debug.Log("OBJECT IS OLDER THAN SYNC RECEIPT");
                tObject.UpdateWithCSV(sEnvironment, sDataArray);
                // }
                // else
                // {
                //     Debug.LogWarning("OBJECT IS MORE RECENT THAN SYNC RECEIPT!!!!!!!!!!!!!!");
                // }
            }
            #if UNITY_EDITOR
            tObject.ErrorCheck();
            #endif
            return tObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations the insert in memory.
        /// </summary>
        /// <param name="sDataArray">S data array.</param>
        public static void SynchronizationInsertInMemory(NWDAppEnvironment sEnvironment, string[] sDataArray)
        {
            //Debug.Log ("SynchronizationInsertInMemory ");
            // if NWDject.reference allready in memory I must replace this object and distroy old object
            NWDBasis<K> tFindObject = null;
            string tReference = GetReferenceValueFromCSV(sDataArray);
            foreach (NWDBasis<K> tObject in ObjectsList)
            {
                if (tObject.Reference == tReference)
                {
                    tFindObject = tObject;
                }
            }
            if (tFindObject != null)
            {
                if (tFindObject.DM <= GetDMValueFromCSV(sDataArray))
                {
                    tFindObject.UpdateWithCSV(sEnvironment, sDataArray);
                }
            }
            else
            {
                tFindObject = NewInstanceFromCSV(sEnvironment, sDataArray);
                tFindObject.UpdateWithCSV(sEnvironment, sDataArray);
                AddObjectInListOfEdition(tFindObject);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations the try to use.
        /// </summary>
        /// <returns>The try to use.</returns>
        /// <param name="sData">S data.</param>
        /// <param name="sForceToUse">If set to <c>true</c> s force to use.</param>
        public static NWDBasis<K> SynchronizationTryToUse(NWDAppEnvironment sEnvironment, string sData, bool sForceToUse = false)
        {
            //Debug.Log("SynchronizationTryToUse ");
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
                Debug.Log("SynchronizationTryToUse INTEGRITY IS FALSE");
            }
            if (tIntegrityTest == true || sForceToUse == true)
            {
                rReturn = SynchronizationInsertInBase(sEnvironment, tDataArray);
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
        public static Dictionary<string, object> SynchronizationPushData(NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, bool sForceAll, bool sClean = false)
        {
            //Debug.Log("NWDBasis SynchronizationPushData() " + ClassName());
            SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
            if (AccountDependent())
            {
                tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
            }
            // create respond object
            Dictionary<string, object> rSend = new Dictionary<string, object>();
            // create dictionnary for this tablename and insert in the respond
            Dictionary<string, object> rSendDatas = new Dictionary<string, object>();
            rSend.Add(TableName(), rSendDatas);
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
                //tResults = tSQLiteConnection.Table<K>().Where(x => x.DM >= tLastSynchronization);
                foreach (K tO in ObjectsList)
                {
                    if (tO.DM >= tLastSynchronization)
                    {
                        tResults.Add(tO);
                    }
                }
            }
            else if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            {
                //tResults = tSQLiteConnection.Table<K>().Where(x => x.DevSync == 0);
                foreach (K tO in ObjectsList)
                {
                    if (tO.DevSync == 0)
                    {
                        tResults.Add(tO);
                    }
                }
            }
            else if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {
                //tResults = tSQLiteConnection.Table<K>().Where(x => x.PreprodSync == 0);
                foreach (K tO in ObjectsList)
                {
                    if (tO.PreprodSync == 0)
                    {
                        tResults.Add(tO);
                    }
                }
            }
            else if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                //tResults = tSQLiteConnection.Table<K>().Where(x => x.ProdSync == 0);
                foreach (K tO in ObjectsList)
                {
                    if (tO.ProdSync == 0)
                    {
                        tResults.Add(tO);
                    }
                }
            }
            if (tResults != null)
            {
                foreach (NWDBasis<K> tItem in tResults)
                {
                    // I test integrity and if object is locked (only editable by editor)
                    if (tItem.TestIntegrity() && tItem.IsLockedObject() == false)
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
                                tDatas.Add(tItem.DataAssembly(true));
                            }
                        }
                        else
                        {
                            // Fake playing mode
                            sInfos.RowPushCounter++;
                            tDatas.Add(tItem.DataAssembly(true));
                        }
#else
                        // not in editor : playing mode only
                        if (tItem.IsReacheableByAccount())
                        {
                            tDatas.Add(tItem.DataAssembly(true));
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
            if (sClean == true)
            {
                rSendDatas.Add(SynchronizeKeyClean, sClean.ToString());
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
        public static string SynchronizationPullData(NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, NWDOperationResult sData)
        {
            //Debug.Log("NWDBasis SynchronizationPullData() " + ClassName());
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
                SynchronizationSetNewTimestamp(sEnvironment, tTimestampServer);
                // now i need get only datas for this class tablename
                string tTableName = TableName();
                // Ok I need to compute all datas for this Class tablename
                if (sData.param.ContainsKey(tTableName))
                {
                    //#if UNITY_EDITOR
                    sInfos.ClassPullCounter++;
                    //#endif
                    List<object> tListOfRows = sData.param[tTableName] as List<object>;

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

                            NWDBasis<K> tObject = SynchronizationTryToUse(sEnvironment, tCsvValueString, tForceToUse);

                            // trash this object ?
                            FlushTrash(tObject);
                        }

                        rReturn = "YES";

#if UNITY_EDITOR
                        FilterTableEditor();
                        RepaintTableEditor();
                        NWDDataInspector.ShareInstance().Repaint();
#endif
                    }
                }
            }

            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Synchronization WebServices
        //-------------------------------------------------------------------------------------------------------------
        //public static List<Type> OverrideClasseInThisSync()
        //{
        //    return new List<Type> { typeof(K)/*, typeof(NWDUserNickname), etc*/ };
        //}
        //-------------------------------------------------------------------------------------------------------------
        public static List<Type> ClasseInThisSync()
        {
            var tMethodInfo = ClassType().GetMethod("OverrideClasseInThisSync", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (tMethodInfo != null)
            {
                return tMethodInfo.Invoke(null, null) as List<Type>;
            }
            else
            {
                return new List<Type> { typeof(K) };
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SynchronizeThisClasse(bool sForce = false)
        {
            if (sForce == true)
            {
                NWDDataManager.SharedInstance().AddWebRequestSynchronization(ClasseInThisSync());
            }
            else
            {
                NWDDataManager.SharedInstance().AddWebRequestSynchronizationForce(ClasseInThisSync());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SynchronizeThisClasseWithBlock(bool sForce = false,
                                                          BTBOperationBlock sSuccessBlock = null,
                                                          BTBOperationBlock sErrorBlock = null,
                                                          BTBOperationBlock sCancelBlock = null,
                                                          BTBOperationBlock sProgressBlock = null,
                                                          bool sPriority = false,
                                                          NWDAppEnvironment sEnvironment = null)
        {
            if (sForce == true)
            {
                NWDDataManager.SharedInstance().AddWebRequestSynchronizationWithBlock(ClasseInThisSync(),
                                                                                     sSuccessBlock,
                                                                                     sErrorBlock,
                                                                                     sCancelBlock,
                                                                                     sProgressBlock,
                                                                                     sPriority,
                                                                                     sEnvironment);
            }
            else
            {
                NWDDataManager.SharedInstance().AddWebRequestSynchronizationForceWithBlock(ClasseInThisSync(),
                                                                                     sSuccessBlock,
                                                                                     sErrorBlock,
                                                                                     sCancelBlock,
                                                                                     sProgressBlock,
                                                                                     sPriority,
                                                                                     sEnvironment);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations from web service.
        /// </summary>
        /// <param name="sForceAll">If set to <c>true</c> s force all.</param>
        public static bool SynchronizationFromWebServiceForce(NWDAppEnvironment sEnvironment)
        {
            bool rReturn = false;
#if UNITY_EDITOR
            //NWDAppEnvironmentSync.SharedInstance().StartProcess(sEnvironment);
            if (Application.isPlaying == true)
            {
                NWDDataManager.SharedInstance().AddWebRequestSynchronizationForce(ClasseInThisSync(), true, sEnvironment);
            }
            else
            {
                NWDEditorMenu.EnvironementSync().SynchronizationForce(ClasseInThisSync(), sEnvironment);
            }
#else
				NWDDataManager.SharedInstance().AddWebRequestSynchronizationForce (new List<Type>{ClassType ()}, true, sEnvironment);
#endif
            rReturn = true;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations from web service.
        /// </summary>
        /// <param name="sForceAll">If set to <c>true</c> s force all.</param>
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
                NWDEditorMenu.EnvironementSync().Synchronization(ClasseInThisSync(), sEnvironment);
            }
#else
				NWDDataManager.SharedInstance().AddWebRequestSynchronization (new List<Type>{ClassType ()}, true, sEnvironment);
#endif
            rReturn = true;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Synchronizations from web service.
        /// </summary>
        /// <param name="sForceAll">If set to <c>true</c> s force all.</param>
        public static bool SynchronizationFromWebServiceClean(NWDAppEnvironment sEnvironment)
        {
            bool rReturn = false;
#if UNITY_EDITOR
            //NWDAppEnvironmentSync.SharedInstance().StartProcess(sEnvironment);
            if (Application.isPlaying == true)
            {
                NWDEditorMenu.EnvironementSync().SynchronizationClean(ClasseInThisSync(), sEnvironment);
            }
            else
            {
                NWDDataManager.SharedInstance().AddWebRequestSynchronizationClean(ClasseInThisSync(), true, sEnvironment);
            }
#else
			NWDDataManager.SharedInstance().AddWebRequestSynchronizationClean (new List<Type>{ClassType ()}, true, sEnvironment);
#endif
            rReturn = true;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Special delete user
        //-------------------------------------------------------------------------------------------------------------
        public static void DeleteUser(NWDAppEnvironment sEnvironment)
        {
            if (AccountDependent() == true)
            {
                // reset last sync to zero
                SynchronizationSetNewTimestamp(sEnvironment, 0); // set to 0 ... only for data AccountDependent, so that's not affect the not connected data (game's data)
                                                                 // delete all datas for this user
                foreach (NWDBasis<K> tObject in ObjectsList)
                {
                    if (tObject.IsReacheableByAccount(NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference))
                    {
                        tObject.DeleteMe();
                    }
                }
                // need to reload this data now : to remove all tObjects from memory!
                LoadTableEditor();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================