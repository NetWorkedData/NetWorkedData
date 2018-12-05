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
            BTBBenchmark.Start("Launcher()");
            BTBBenchmark.Start("Launcher() Engine");
            // this class deamon is launch at start ... Read all classes, install all classes deamon and load all datas
            // start to reccord the memeories used
            long tStartMemory = System.GC.GetTotalMemory(true);
            // not double lauch
            // not double launching!
            if (IsLaunched == false && IsLaunching == false)
            {
                // lauching in progress
                IsLaunching = true;
                // craeta a list to reccord all classes
                List<Type> tTypeList = new List<Type>();
                // Get ShareInstance of datamanager instance
                NWDDataManager tShareInstance = NWDDataManager.SharedInstance();
                // Find all Type of NWDType
                Type[] tAllTypes = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
                // sort and filter by NWDBasis (NWDTypeClass subclass)
                Type[] tAllNWDTypes = (from System.Type type in tAllTypes
                                       where type.IsSubclassOf(typeof(NWDTypeClass))
                                       select type).ToArray();
                // Force launch and register class type
                //int tTrigrammeAbstract = 111; // refault trigramme
                //int tNumberOfClasses = tAllNWDTypes.Count ();
                //int tIndexOfActualClass = 0;

                //int tOperationsNeeded = tAllNWDTypes.Count();
                //int tOPerationInProgress = 0;

                foreach (Type tType in tAllNWDTypes)
                {
                    //tOPerationInProgress++;
                    //tTrigrammeAbstract++;
                    if (tType.ContainsGenericParameters == false)
                    {
                        tTypeList.Add(tType);
                        ////Debug.Log ("FIND tType = " + tType.Name);
                        //string tTrigramme = tTrigrammeAbstract.ToString();
                        //if (tType.GetCustomAttributes(typeof(NWDClassTrigrammeAttribute), true).Length > 0)
                        //{
                        //    NWDClassTrigrammeAttribute tTrigrammeAttribut = (NWDClassTrigrammeAttribute)tType.GetCustomAttributes(typeof(NWDClassTrigrammeAttribute), true)[0];
                        //    tTrigramme = tTrigrammeAttribut.Trigramme;
                        //    if (tTrigramme == null || tTrigramme == "")
                        //    {
                        //        tTrigramme = tTrigrammeAbstract.ToString();
                        //    }
                        //}
                        //bool tServerSynchronize = true;
                        //if (tType.GetCustomAttributes(typeof(NWDClassServerSynchronizeAttribute), true).Length > 0)
                        //{
                        //    NWDClassServerSynchronizeAttribute tServerSynchronizeAttribut = (NWDClassServerSynchronizeAttribute)tType.GetCustomAttributes(typeof(NWDClassServerSynchronizeAttribute), true)[0];
                        //    tServerSynchronize = tServerSynchronizeAttribut.ServerSynchronize;
                        //}
                        //string tDescription = "no description";
                        //if (tType.GetCustomAttributes(typeof(NWDClassDescriptionAttribute), true).Length > 0)
                        //{
                        //    NWDClassDescriptionAttribute tDescriptionAttribut = (NWDClassDescriptionAttribute)tType.GetCustomAttributes(typeof(NWDClassDescriptionAttribute), true)[0];
                        //    tDescription = tDescriptionAttribut.Description;
                        //    if (tDescription == null || tDescription == "")
                        //    {
                        //        tDescription = "empty description";
                        //    }
                        //}
                        //string tMenuName = tType.Name + " menu";
                        //if (tType.GetCustomAttributes(typeof(NWDClassMenuNameAttribute), true).Length > 0)
                        //{
                        //    NWDClassMenuNameAttribute tMenuNameAttribut = (NWDClassMenuNameAttribute)tType.GetCustomAttributes(typeof(NWDClassMenuNameAttribute), true)[0];
                        //    tMenuName = tMenuNameAttribut.MenuName;
                        //    if (tMenuName == null || tMenuName == "")
                        //    {
                        //        tMenuName = tType.Name + " menu";
                        //    }
                        //}
                    //    var tMethodDeclare = tType.GetMethod("ClassDeclare", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                    //    if (tMethodDeclare != null)
                    //    {
                    //        tMethodDeclare.Invoke(null, new object[] { tServerSynchronize, tTrigramme, tMenuName, tDescription });
                    //}
                    var tMethodDeclare = tType.GetMethod("ClassDeclare", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                    if (tMethodDeclare != null)
                    {
                        tMethodDeclare.Invoke(null, null);
                    }
                }
                    //tIndexOfActualClass++;
                }
                AllTypes = tTypeList.ToArray();
                //double tFinishTimestamp = BTBDateHelper.ConvertToTimestamp(DateTime.Now);
                //double tDelta = tFinishTimestamp - tStartTimestamp;
                //Debug.Log("NWD => NetWorkeData launch in " + tDelta.ToString() + " seconds");
                // Notify NWD is launched
                IsLaunched = true;
                Debug.Log("#LAUNCHER# NWDDataManager.SharedInstance().mTypeAccountDependantList count =" + NWDDataManager.SharedInstance().mTypeAccountDependantList.Count());
                // connect to database;
                BTBBenchmark.Finish("Launcher() Engine");
                BTBBenchmark.Start("Launcher() Connect to Database");
                tShareInstance.ConnectToDatabase();
                BTBBenchmark.Finish("Launcher() Connect to Database");
                BTBBenchmark.Start("Launcher() load Datas");
                // Ok engine is launched
                BTBNotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_ENGINE_LAUNCH);
                // reccord the memory score!
                long tMiddleMemory = System.GC.GetTotalMemory(true);
                // Loaded data 
                if (DataLoaded == false)
                {
                    // But only if preloaddatas is true!
                    if (NWDAppConfiguration.SharedInstance().PreloadDatas == true)
                    {
                        //Debug.Log("NWD => Preload Datas");
                        tShareInstance.ReloadAllObjects();
                    }
                }
                // reccord the memory score!
                long tFinishMemory = System.GC.GetTotalMemory(true);
                long tStartMem = tStartMemory / 1024 / 1024;
                long tEngineMemory = (tMiddleMemory - tStartMemory) / 1024 / 1024;
                long tDataMemory = (tFinishMemory - tMiddleMemory) / 1024 / 1024;
                long tMemory = tFinishMemory / 1024 / 1024;
                Debug.Log("#### NWDTypeLauncher Launcher FINISHED engine memory = " + tEngineMemory.ToString() + "Mo");
                Debug.Log("#### NWDTypeLauncher Launcher FINISHED Data memory = " + tDataMemory.ToString() + "Mo");
                Debug.Log("#### NWDTypeLauncher memory = " + tStartMem.ToString() + "Mo => " + tMemory.ToString() + "Mo");
            }
            //Debug.Log ("#### NWDTypeLauncher Launcher FINISHED");
            BTBBenchmark.Finish("Launcher() load Datas");
            BTBBenchmark.Finish("Launcher()");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================