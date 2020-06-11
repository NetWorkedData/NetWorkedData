//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using BasicToolBox;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDDataManagerMainThread in an static class method with a shareinstance to detect in the main thread the database writing. 
    /// It's not the perfect expected, but unity is mono thread. We haven't another solution :-/ 
    /// </summary>
#if UNITY_EDITOR
    [ExecuteInEditMode]
    [InitializeOnLoad]
#endif
    public class NWDDataManagerMainThread : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Insert completed count the basic insert in transition.
        /// </summary>
        static private int InsertCompleted = 0;
        /// <summary>
        /// Update completed count the basic insert in transition.
        /// </summary>
        static private int UpdateCompleted = 0;
        /// <summary>
        /// Delete completed count the basic insert in transition.
        /// </summary>
        static private int DeleteCompleted = 0;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The list insert completed count the list of class in transition
        /// </summary>
        static private List<List<Type>> ListInsertCompleted = new List<List<Type>>();
        /// <summary>
        /// The list update completed count the list of class in transition
        /// </summary>
        static private List<List<Type>> ListUpdateCompleted = new List<List<Type>>();
        /// <summary>
        /// The list delete completed count the list of class in transition
        /// </summary>
        static private List<List<Type>> ListDeleteCompleted = new List<List<Type>>();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The share instance object use in editor and player.
        /// </summary>
        private static NWDDataManagerMainThread kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes the <see cref="T:NetWorkedData.NWDDataThreadResult"/> class.
        /// </summary>
        static NWDDataManagerMainThread()
        {
#if UNITY_EDITOR
#else
            if (kSharedInstance == null)
            {
                kSharedInstance = (NWDDataManagerMainThread)Activator.CreateInstance(typeof(NWDDataManagerMainThread));
                //kSharedInstance = new NWDDataManagerMainThread();
            }
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="T:NetWorkedData.NWDDataThreadResult"/> class.
        /// </summary>
        public NWDDataManagerMainThread()
        {
#if UNITY_EDITOR
            EditorApplication.update += Update;
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the type of the insert.
        /// </summary>
        /// <param name="sType">S type.</param>
        static public void AddInsertType(Type sType)
        {
            lock (ListInsertCompleted)
            {
                List<Type> tTypeList = new List<Type>();
                tTypeList.Add(sType);
                ListInsertCompleted.Add(tTypeList);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the type of the update.
        /// </summary>
        /// <param name="sType">S type.</param>
        static public void AddUpdateType(Type sType)
        {
            lock (ListUpdateCompleted)
            {
                List<Type> tTypeList = new List<Type>();
                tTypeList.Add(sType);
                ListUpdateCompleted.Add(tTypeList);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the type of the delete.
        /// </summary>
        /// <param name="sType">S type.</param>
        static public void AddDeleteType(Type sType)
        {
            lock (ListDeleteCompleted)
            {
                List<Type> tTypeList = new List<Type>();
                tTypeList.Add(sType);
                ListDeleteCompleted.Add(tTypeList);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the insert type list.
        /// </summary>
        /// <param name="sTypeList">S type list.</param>
        static public void AddInsertTypeList( List<Type> sTypeList)
        {
            lock (ListInsertCompleted)
            {
                ListInsertCompleted.Add(sTypeList);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the update type list.
        /// </summary>
        /// <param name="sTypeList">S type list.</param>
        static public void AddUpdateTypeList(List<Type> sTypeList)
        {
            lock (ListUpdateCompleted)
            {
                ListUpdateCompleted.Add(sTypeList);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the delete type list.
        /// </summary>
        /// <param name="sTypeList">S type list.</param>
        static public void AddDeleteTypeList(List<Type> sTypeList)
        {
            lock (ListDeleteCompleted)
            {
                ListDeleteCompleted.Add(sTypeList);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the insert.
        /// </summary>
        static public void AddInsert()
        {
            InsertCompleted++;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the update.
        /// </summary>
        static public void AddUpdate()
        {
            UpdateCompleted++;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the delete.
        /// </summary>
        static public void AddDelete()
        {
            DeleteCompleted++;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        // Update is called once per frame
        /// </summary>
        void Update()
        {
            //Debug.Log("NWDDataManagerMainThread Update()");
            // not necessary to lock int
            if (InsertCompleted > 0)
            {
                //Debug.Log("NWDDataManagerMainThread Update() InsertCompleted detected!");
                InsertCompleted--;
                NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DATA_LOCAL_INSERT);
            }
            // not necessary to lock int
            if (UpdateCompleted > 0)
            {
                //Debug.Log("NWDDataManagerMainThread Update() UpdateCompleted detected!");
                UpdateCompleted--;
                NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DATA_LOCAL_UPDATE);
            }
            // not necessary to lock int
            if (DeleteCompleted > 0)
            {
                //Debug.Log("NWDDataManagerMainThread Update() DeleteCompleted detected!");
                DeleteCompleted--;
                NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DATA_LOCAL_DELETE);
            }
            // necessary to lock list
            lock (ListInsertCompleted)
            {
                if (ListInsertCompleted.Count>0)
                {
                    //Debug.Log("NWDDataManagerMainThread Update() ListInsertCompleted detected!");
                    List<Type> tTypeList =  ListInsertCompleted[0];
                    ListInsertCompleted.RemoveAt(0);
                    NWENotificationManager.SharedInstance().PostNotification(tTypeList, NWDNotificationConstants.K_DATA_LOCAL_INSERT);
#if UNITY_EDITOR
                    foreach (Type tType in tTypeList)
                        {
                        NWDDataManager.SharedInstance().RepaintWindowsInManager(tType);
                    }
#endif
                }
            }
            // necessary to lock list
            lock (ListUpdateCompleted)
            {
                if (ListUpdateCompleted.Count > 0)
                {
                    //Debug.Log("NWDDataManagerMainThread Update() ListUpdateCompleted detected!");
                    List<Type> tTypeList = ListUpdateCompleted[0];
                    ListUpdateCompleted.RemoveAt(0);
                    NWENotificationManager.SharedInstance().PostNotification(tTypeList, NWDNotificationConstants.K_DATA_LOCAL_UPDATE);
#if UNITY_EDITOR
                    foreach (Type tType in tTypeList)
                    {
                        NWDDataManager.SharedInstance().RepaintWindowsInManager(tType);
                    }
#endif
                }
            }
            // necessary to lock list
            lock (ListDeleteCompleted)
            {
                if (ListDeleteCompleted.Count > 0)
                {
                    //Debug.Log("NWDDataManagerMainThread Update() ListDeleteCompleted detected!");
                    List<Type> tTypeList = ListDeleteCompleted[0];
                    ListDeleteCompleted.RemoveAt(0);
                    NWENotificationManager.SharedInstance().PostNotification(tTypeList, NWDNotificationConstants.K_DATA_LOCAL_DELETE);
#if UNITY_EDITOR
                    foreach (Type tType in tTypeList)
                    {
                        NWDDataManager.SharedInstance().RepaintWindowsInManager(tType);
                    }
#endif
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
