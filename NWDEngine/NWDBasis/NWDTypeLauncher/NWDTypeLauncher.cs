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

using UnityEngine;

using BasicToolBox;
using UnityEngine.SceneManagement;
using System.Threading;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDTypeLauncher
    {
        //private static NWDTypeLauncher InitialLaucher = new NWDTypeLauncher();
        //-------------------------------------------------------------------------------------------------------------
        public static bool IsLaunching = false;// to protect dupplicate launch editor/player
        public static bool IsLaunched = false;// to protect dupplicate launch editor/player
        public static bool DataLoaded = false;
        public static int ClassesExpected = 0;
        public static int ClassesDataLoaded = 0;
        public static Type[] AllTypes;
        //-------------------------------------------------------------------------------------------------------------
        static NWDTypeLauncher()
        {
            //Debug.Log("NWDTypeLauncher Class Constructor NWDTypeLauncher()");
            //Debug.Log("screen Screen.width = " + Screen.width.ToString());
            //Debug.Log("screen Screen.height = " + Screen.height.ToString());
            //if (Application.isPlaying == true)
            //{
            //    Debug.Log("create a scene ? ");
            //    //Scene tSceneLoader = SceneManager.CreateScene("NetWorkedDataTemporaryScene");
            //    //SceneManager.SetActiveScene(tSceneLoader);
            //}
            //else
            //{
            //    Debug.Log("Application is not playing");
            //}
            Launcher();
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTypeLauncher()
        {
            //Debug.Log("NWDTypeLauncher Instance Constructor NWDTypeLauncher()");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Launcher()
        {
            long tStartMemory = System.GC.GetTotalMemory(true);

            //Debug.Log("NWDTypeLauncher Launcher()");
            //double tStartTimestamp =  BTBDateHelper.ConvertToTimestamp(DateTime.Now);
            if (IsLaunched == false && IsLaunching == false)
            {
                List<Type> tTypeList = new List<Type>();
                IsLaunching = true;
                // Get ShareInstance
                NWDDataManager tShareInstance = NWDDataManager.SharedInstance();
                // connect to database;
                tShareInstance.ConnectToDatabase();
                // Find all Type of NWDType
                Type[] tAllTypes = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
                Type[] tAllNWDTypes = (from System.Type type in tAllTypes
                                       where type.IsSubclassOf(typeof(NWDTypeClass))
                                       select type).ToArray();
                // Force launch and register class type
                int tTrigrammeAbstract = 111;
                //int tNumberOfClasses = tAllNWDTypes.Count ();
                int tIndexOfActualClass = 0;

                //int tOperationsNeeded = tAllNWDTypes.Count();
                //int tOPerationInProgress = 0;

                foreach (Type tType in tAllNWDTypes)
                {
                    //tOPerationInProgress++;
                    tTrigrammeAbstract++;
                    if (tType.ContainsGenericParameters == false)
                    {
                        tTypeList.Add(tType);
                        //Debug.Log ("FIND tType = " + tType.Name);
                        string tTrigramme = tTrigrammeAbstract.ToString();
                        if (tType.GetCustomAttributes(typeof(NWDClassTrigrammeAttribute), true).Length > 0)
                        {
                            NWDClassTrigrammeAttribute tTrigrammeAttribut = (NWDClassTrigrammeAttribute)tType.GetCustomAttributes(typeof(NWDClassTrigrammeAttribute), true)[0];
                            tTrigramme = tTrigrammeAttribut.Trigramme;
                            if (tTrigramme == null || tTrigramme == "")
                            {
                                tTrigramme = tTrigrammeAbstract.ToString();
                            }
                        }
                        bool tServerSynchronize = true;
                        if (tType.GetCustomAttributes(typeof(NWDClassServerSynchronizeAttribute), true).Length > 0)
                        {
                            NWDClassServerSynchronizeAttribute tServerSynchronizeAttribut = (NWDClassServerSynchronizeAttribute)tType.GetCustomAttributes(typeof(NWDClassServerSynchronizeAttribute), true)[0];
                            tServerSynchronize = tServerSynchronizeAttribut.ServerSynchronize;
                        }
                        string tDescription = "no description";
                        if (tType.GetCustomAttributes(typeof(NWDClassDescriptionAttribute), true).Length > 0)
                        {
                            NWDClassDescriptionAttribute tDescriptionAttribut = (NWDClassDescriptionAttribute)tType.GetCustomAttributes(typeof(NWDClassDescriptionAttribute), true)[0];
                            tDescription = tDescriptionAttribut.Description;
                            if (tDescription == null || tDescription == "")
                            {
                                tDescription = "empty description";
                            }
                        }
                        string tMenuName = tType.Name + " menu";
                        if (tType.GetCustomAttributes(typeof(NWDClassMenuNameAttribute), true).Length > 0)
                        {
                            NWDClassMenuNameAttribute tMenuNameAttribut = (NWDClassMenuNameAttribute)tType.GetCustomAttributes(typeof(NWDClassMenuNameAttribute), true)[0];
                            tMenuName = tMenuNameAttribut.MenuName;
                            if (tMenuName == null || tMenuName == "")
                            {
                                tMenuName = tType.Name + " menu";
                            }
                        }
                        var tMethodDeclare = tType.GetMethod("ClassDeclare", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                        if (tMethodDeclare != null)
                        {
                            tMethodDeclare.Invoke(null, new object[] { tServerSynchronize, tTrigramme, tMenuName, tDescription });
                        }

                        /* DEBUG */
                        //var tMethodInfo = tType.GetMethod ("ClassInformations", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                        //                        if (tMethodInfo != null) {
                        //                            tMethodInfo.Invoke (null, new object[]{ "Launcher " });
                        //                        }
                        /* DEBUG */
                        //double tTimeStamp = BTBDateHelper.ConvertToTimestamp(DateTime.Now);
                        //Debug.Log("tOperationsNeeded = " + tOPerationInProgress.ToString() + "/" + tOperationsNeeded.ToString() + " at "+ tTimeStamp.ToString());
                    }
                    tIndexOfActualClass++;
                }

                //foreach (Type tType in NWDDataManager.SharedInstance().mTypeSynchronizedList)
                //{
                //    Debug.Log("tType in sync " + tType.Name);
                //}

                AllTypes = tTypeList.ToArray();
                //double tFinishTimestamp = BTBDateHelper.ConvertToTimestamp(DateTime.Now);
                //double tDelta = tFinishTimestamp - tStartTimestamp;
                //Debug.Log("NWD => NetWorkeData launch in " + tDelta.ToString() + " seconds");
                // Notify NWD is launch
                IsLaunched = true;
                BTBNotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_ENGINE_LAUNCH);


                long tMiddleMemory = System.GC.GetTotalMemory(true);
                if (DataLoaded == false)
                {
                    if (NWDAppConfiguration.SharedInstance().PreloadDatas == true)
                    {
                        //Debug.Log("NWD => Preload Datas");
                        tShareInstance.ReloadAllObjects();
                    }
                }

                long tFinishMemory = System.GC.GetTotalMemory(true);
                long tStartMem = tStartMemory / 1024 / 1024;
                long tEngineMemory = (tMiddleMemory - tStartMemory) / 1024/ 1024;
                long tDataMemory = (tFinishMemory - tMiddleMemory) / 1024 / 1024;
                long tMemory = tFinishMemory / 1024 / 1024;
                Debug.Log("#### NWDTypeLauncher Launcher FINISHED engine memory = " + tEngineMemory.ToString() + "Mo");
                Debug.Log("#### NWDTypeLauncher Launcher FINISHED Data memory = " + tDataMemory.ToString() + "Mo");
                Debug.Log("#### NWDTypeLauncher memory = " + tStartMem.ToString() + "Mo => " + tMemory.ToString() + "Mo");

            }
            //Debug.Log ("#### NWDTypeLauncher Launcher FINISHED");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================