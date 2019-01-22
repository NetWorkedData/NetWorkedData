//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using BasicToolBox;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDTypeLauncher is the first class launch in the NetWorkedData lib. It's used to determine the class model and interaction.
    /// </summary>
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class NWDTypeLauncher
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The lib is launching.
        /// </summary>
        public static bool IsLaunching = false;
        /// <summary>
        /// The lib is launched.
        /// </summary>
        public static bool IsLaunched = false;
        /// <summary>
        /// The datas are loaded.
        /// </summary>
        public static bool DataLoaded = false;
        /// <summary>
        /// Classes expected.
        /// </summary>
        public static int ClassesExpected = 0;
        /// <summary>
        /// Classes NWDbasis generic data loaded.
        /// </summary>
        public static int ClassesDataLoaded = 0;
        /// <summary>
        /// All Types array.
        /// </summary>
        public static Type[] AllTypes;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes the <see cref="T:NetWorkedData.NWDTypeLauncher"/> class.
        /// </summary>
        static NWDTypeLauncher()
        {
            //NWDDebug.Log("NWDTypeLauncher Static Class Constructor()");
            Launcher();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="T:NetWorkedData.NWDTypeLauncher"/> class.
        /// </summary>
        public NWDTypeLauncher()
        {
            //NWDDebug.Log("NWDTypeLauncher Instance Constructor NWDTypeLauncher()");
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Launcher this daemon lib.
        /// </summary>
        public static void Launcher()
        {
            if (IsLaunched == false && IsLaunching == false)
            {
                RunLauncher();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Runs the launcher.
        /// </summary>
#if UNITY_EDITOR
        [InitializeOnLoadMethod()]
#endif
        public static void RunLauncher()
        {
            // FORCE TO ENGLISH FORMAT!
            Thread.CurrentThread.CurrentCulture = NWDConstants.FormatCountry;
            // this class deamon is launch at start ... Read all classes, install all classes deamon and load all datas
            //NWDDebug.Log("NWDTypeLauncher RunLauncher()");
            // not double lauch
            // not double launching!
            if (IsLaunched == false && IsLaunching == false)
            {
                // lauching in progress
                IsLaunching = true;
                // benchmark
                BTBBenchmark.Start();
                // start to reccord memory usage
                long tStartMemory = System.GC.GetTotalMemory(true);
                BTBBenchmark.Start("Launcher() Engine");
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
                //Debug.Log("#LAUNCHER# NWDDataManager.SharedInstance().mTypeAccountDependantList count = " + NWDDataManager.SharedInstance().mTypeAccountDependantList.Count());
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
                // reccord the memory score!
                long tMiddleMemory = System.GC.GetTotalMemory(true);
                // Loaded data 
                if (DataLoaded == false)
                {
                    bool tEditorByPass = false;
#if UNITY_EDITOR
                    if (Application.isEditor == true)
                    {
                        Debug.Log(" I AM IN EDITOR");
                        if (Application.isPlaying == true)
                        {
                            Debug.Log(" <color=green>I AM IN EDITOR</color> BUT <color=green>MODE PLAYER IS PLAYING</color>  ");
                        }
                        else
                        {
                            Debug.Log(" <color=green>I AM IN EDITOR</color> AND <color=red>MODE PLAYER IS NOT PLAYING</color> ");
                        }
                    }
                    else
                    {
                        Debug.Log(" <color=r-red>I AM NOT IN EDITOR</color>");
                    }

                    if (Application.isEditor && Application.isPlaying == false)
                    {
                        tEditorByPass = true;
                        Debug.Log("NWD => Preload Datas bypass by editor");
                    }
#endif
                    if (NWDAppConfiguration.SharedInstance().PreloadDatas == true || tEditorByPass == true)
                    {
                        BTBBenchmark.Start("Launcher() load Datas");
                        //NWDDebug.Log("NWD => Preload Datas");
                        tShareInstance.ReloadAllObjects();
                        BTBBenchmark.Finish("Launcher() load Datas");
                    }
                }
                // reccord the memory score!
                long tFinishMemory = System.GC.GetTotalMemory(true);
                long tStartMem = tStartMemory / 1024 / 1024;
                long tEngineMemory = (tMiddleMemory - tStartMemory) / 1024 / 1024;
                long tDataMemory = (tFinishMemory - tMiddleMemory) / 1024 / 1024;
                long tMemory = tFinishMemory / 1024 / 1024;
                NWDDebug.Log("#### NWDTypeLauncher Launcher FINISHED engine memory = " + tEngineMemory.ToString() + "Mo");
                NWDDebug.Log("#### NWDTypeLauncher Launcher FINISHED Data memory = " + tDataMemory.ToString() + "Mo");
                NWDDebug.Log("#### NWDTypeLauncher memory = " + tStartMem.ToString() + "Mo => " + tMemory.ToString() + "Mo");
                // finish launch
                IsLaunched = true;
                IsLaunching = false;
                //Debug.Log ("#### NWDTypeLauncher Launcher FINISHED");
                BTBBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================