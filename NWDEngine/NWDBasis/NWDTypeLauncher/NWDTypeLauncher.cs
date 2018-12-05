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
    public class NWDTypeLauncher
    {
        //-------------------------------------------------------------------------------------------------------------
        public static bool IsLaunching = false;
        public static bool IsLaunched = false;
        public static bool DataLoaded = false;
        public static int ClassesExpected = 0;
        public static int ClassesDataLoaded = 0;
        public static Type[] AllTypes;
        //-------------------------------------------------------------------------------------------------------------
        static NWDTypeLauncher()
        {
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
            // this class deamon is launch at start ... Read all classes, install all classes deamon and load all datas
            BTBBenchmark.Start("Launcher()");
            // not double lauch
            // not double launching!
            if (IsLaunched == false && IsLaunching == false)
            {
                // start to reccord memory usage
                long tStartMemory = System.GC.GetTotalMemory(true);
                BTBBenchmark.Start("Launcher() Engine");
                // lauching in progress
                IsLaunching = true;
                // craeta a list to reccord all classes
                List<Type> tTypeList = new List<Type>();
                // Get ShareInstance of datamanager instance
                NWDDataManager tShareInstance = NWDDataManager.SharedInstance();
                // Find all Type of NWDType
                //BTBBenchmark.Start("Launcher() reflexion");
                Type[] tAllTypes = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
                // sort and filter by NWDBasis (NWDTypeClass subclass)
                Type[] tAllNWDTypes = (from System.Type type in tAllTypes
                                       where type.IsSubclassOf(typeof(NWDTypeClass))
                                       select type).ToArray();
                //BTBBenchmark.Finish("Launcher() reflexion");
                foreach (Type tType in tAllNWDTypes)
                {
                    //tOPerationInProgress++;
                    // not the NWDBasis<K> because it's generic class
                    if (tType.ContainsGenericParameters == false)
                    {
                        // add type in list of class
                        tTypeList.Add(tType);
                        // invoke the ClassDeclare method!
                        var tMethodDeclare = tType.GetMethod("ClassDeclare", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                        if (tMethodDeclare != null)
                        {
                            tMethodDeclare.Invoke(null, null);
                        }
                    }
                }
                AllTypes = tTypeList.ToArray();
                // Notify NWD is launched
                IsLaunched = true;
                Debug.Log("#LAUNCHER# NWDDataManager.SharedInstance().mTypeAccountDependantList count =" + NWDDataManager.SharedInstance().mTypeAccountDependantList.Count());
                // connect to database;
                BTBBenchmark.Finish("Launcher() Engine");
                // Ok engine is launched
                BTBNotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_ENGINE_LAUNCH);
                // start connexion to database file
                BTBBenchmark.Start("Launcher() Connect to Database");
                tShareInstance.ConnectToDatabase();
                BTBBenchmark.Finish("Launcher() Connect to Database");
                // Ok database is connected
                BTBNotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DATABASE_CONNECTED);
                // start to lauch datas from database
                BTBBenchmark.Start("Launcher() load Datas");
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
                BTBBenchmark.Finish("Launcher() load Datas");
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
            BTBBenchmark.Finish("Launcher()");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================