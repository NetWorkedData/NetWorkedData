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
        public static bool DataEditorConnected = false;
        public static bool DataAccountConnected = false;
        public static bool DataEditorLoaded = false;
        public static bool DataAccountLoaded = false;
        /// <summary>
        /// Classes expected.
        /// </summary>
        public static int ClassesEditorExpected = 0;
        public static int ClassesAccountExpected = 0;
        /// <summary>
        /// Classes NWDbasis generic data loaded.
        /// </summary>
        public static int ClassesEditorDataLoaded = 0;
        public static int ClassesAccountDataLoaded = 0;
        /// <summary>
        /// All Types array.
        /// </summary>
        public static Type[] AllTypes;

        public static int Tentative = 0;
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
#if UNITY_EDITOR
            // to clear error ProgressBar 
            EditorUtility.ClearProgressBar();
#endif
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
                    // not the NWDBasis<K> because it's generic class
                    if (tType.ContainsGenericParameters == false)
                    {
                        // add type in list of class
                        tTypeList.Add(tType);
                        // invoke the ClassDeclare method!
                        MethodInfo tMethodInfo = NWDAliasMethod.GetMethodPublicStaticFlattenHierarchy(tType, NWDConstants.M_ClassDeclare);

                        if (tMethodInfo != null)
                        {
                            tMethodInfo.Invoke(null, null);
                        }
                    }
                }
                AllTypes = tTypeList.ToArray();
                NWDAppConfiguration.SharedInstance().RestaureTypesConfigurations();
                // Notify NWD is launched
                IsLaunched = true;
                // connect to database;
                BTBBenchmark.Finish("Launcher() Engine");
                // Ok engine is launched
                BTBNotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_ENGINE_LAUNCH);
                // start connexion to database file
                DatabaseEditorLauncher();

                // connection to account database 
                DatabaseAccountLauncher();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void DatabaseEditorLauncher()
        {
            if (IsLaunched == true && DataEditorConnected == false && IsLaunching == true)
            {
                // get sharedInstance
                NWDDataManager tShareInstance = NWDDataManager.SharedInstance();
                // connect editor database
                DataEditorConnected = tShareInstance.ConnectToDatabaseEditor();
                //Load datas editor
                DatabaseEditorLoadDatas();
            }
        }//-------------------------------------------------------------------------------------------------------------
        public static void DatabaseEditorLoadDatas()
        {
            BTBBenchmark.Start();
            if (IsLaunched == true && DataEditorConnected == true && IsLaunching == true)
            {
                // Ok database is connected
                BTBNotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DATABASE_CONNECTED);
                // start to lauch datas from database
                // Loaded data 
                if (DataEditorLoaded == false)
                {
                    bool tEditorByPass = false;
#if UNITY_EDITOR
                    if (Application.isEditor && Application.isPlaying == false)
                    {
                        tEditorByPass = true;
                        //Debug.Log("NWD => Preload Datas bypass by editor");
                    }
#endif
                    if (NWDAppConfiguration.SharedInstance().PreloadDatas == true || tEditorByPass == true)
                    {
                        BTBBenchmark.Start("DatabaseEditorLoadDatas() load Datas");
                        NWDDataManager.SharedInstance().ReloadAllObjectsEditor();
                        BTBBenchmark.Finish("DatabaseEditorLoadDatas() load Datas");
                    }
                }
                // finish launch
                DataEditorLoaded = true;
                //Debug.Log ("#### NWDTypeLauncher Launcher FINISHED");
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void DatabaseAccountLauncher()
        {
            BTBBenchmark.Start();
            if (IsLaunched == true && DataAccountConnected == false && IsLaunching == true)
            {
                string tSurProtection = string.Empty;
                if (NWDAppConfiguration.SharedInstance().SurProtected == true)
                {
                    BTBNotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DATABASE_PROTECTION_REQUEST);
#if UNITY_EDITOR
                    NWDDataManagerDialog.ShowDialog("Need code", "Insert your personnal code", MessageType.Warning, delegate (string sValue)
                    {
                        DatabaseAccountConnection(sValue);
                    });
#endif
                }
                else
                {
                    DatabaseAccountConnection(string.Empty);
                }
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void DatabaseAccountConnection(string sSurProtection)
        {
            BTBBenchmark.Start();


            if (IsLaunched == true && DataAccountConnected == false && IsLaunching == true)
            {
                Tentative++;
                // Get ShareInstance of datamanager instance
                NWDDataManager tShareInstance = NWDDataManager.SharedInstance();
                DataAccountConnected = tShareInstance.ConnectToDatabaseAccount(sSurProtection);
                if (DataAccountConnected == false)
                {
                    if (Tentative < NWDAppConfiguration.SharedInstance().ProtectionTentativeMax)
                    {
                        Debug.Log("Database is not openable with this sur protected code! Tentative n°" + Tentative + " : " + sSurProtection);
                        BTBNotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DATABASE_PROTECTION_FAIL);
                        DatabaseAccountLauncher();
                    }
                    else
                    {
                        Debug.Log("Database is not openable max tentative over! Tentative n°" + Tentative);
                        // Kill App || Destroy Database || Call FBI || Vodoo ?
                        // decide yoursel with this notification!
                        BTBNotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DATABASE_PROTECTION_STOP);
                    }
                }
                else
                {
                    Debug.Log("Database is opened with this sur protected code! Tentative n°" + Tentative);
                    BTBNotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DATABASE_PROTECTION_SUCCESS);
                    DatabaseAccountLoadDatas();
                }
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void DatabaseAccountLoadDatas()
        {
            BTBBenchmark.Start();
            if (IsLaunched == true && DataAccountConnected == true && IsLaunching == true)
            {
                // Ok database is connected
                BTBNotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DATABASE_CONNECTED);
                // start to lauch datas from database
                // Loaded data 
                if (DataAccountLoaded == false)
                {
                    bool tEditorByPass = false;
#if UNITY_EDITOR
                    if (Application.isEditor && Application.isPlaying == false)
                    {
                        tEditorByPass = true;
                        //Debug.Log("NWD => Preload Datas bypass by editor");
                    }
#endif
                    if (NWDAppConfiguration.SharedInstance().PreloadDatas == true || tEditorByPass == true)
                    {
                        BTBBenchmark.Start("DatabaseAccountLoadDatas() load Datas");
                        NWDDataManager.SharedInstance().ReloadAllObjectsAccount();
                        BTBBenchmark.Finish("DatabaseAccountLoadDatas() load Datas");
                    }
                }
                // finish launch
                DataAccountLoaded = true;
                IsLaunching = false;
                //Debug.Log ("#### NWDTypeLauncher Launcher FINISHED");
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool DataLoaded()
        {
            bool rReturn = true;
            if (NWDTypeLauncher.DataEditorLoaded == false || NWDTypeLauncher.DataAccountLoaded)
            {
                rReturn = false;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================