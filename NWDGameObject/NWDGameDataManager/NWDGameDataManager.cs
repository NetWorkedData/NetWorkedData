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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;
using System.Threading;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWD game data manager.
    /// The GameObject monobehaviour connection
    /// Use in game to acces to specific method and create a simulate singleton instance 
    /// The sigleton instance was connect to :
    /// Data Manager
    /// App Configuration
    /// Notification Center
    /// </summary>
    //[ExecuteInEditMode] // We use this only in playmode so don't attribut ExecuteInEditMode
    public partial class NWDGameDataManager : NWDCallBackDataLoadOnly
    {
        //-------------------------------------------------------------------------------------------------------------
        //public NWDNetworkState NetworkStatut = NWDNetworkState.Unknow;
        //-------------------------------------------------------------------------------------------------------------
        public NWEScreenGauge LoadingDatasGauge;
        //-------------------------------------------------------------------------------------------------------------
        public NWEScreenGauge OperationGauge;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// SharedInstanceAsSingleton. Class to create a ShareInstance use as Singleton by All NWDGameDataManager
        /// Declare as private, all NWDGameDataManager instance call this class's shared instance.
        /// So, instances connected to private sharedinstance, if I don't create otehr instance, nobody can : I have a singleton simulation
        /// Of course it's a patch, but it's working!
        /// </summary>
        //-------------------------------------------------------------------------------------------------------------
        //[ExecuteInEditMode] // I do that to run this object in edit mode too (on scene)
        //-------------------------------------------------------------------------------------------------------------
        private class SharedInstanceAsSingleton
        {
            //-------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// The instance's counter.
            /// </summary>
            static int InstanceCounter = 0;
            //-------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// Log the counter of instance of this Class.
            /// </summary>
            static void InstanceCounterLog()
            {
                //Debug.LogVerbose ("(NWDGameDataManager SharedInstanceAsSingleton number of instance : "+InstanceCounter+")");
            }
            //-------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// The shared instance as singleton.
            /// </summary>
            static public SharedInstanceAsSingleton kSharedInstaceAsSingleton = new SharedInstanceAsSingleton();
            //-------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// The data manager.
            /// </summary>
            //public NWDDataManager DataManager;
            ///// <summary>
            ///// The app configuration.
            ///// </summary>
            //public NWDAppConfiguration AppConfiguration;
            ///// <summary>
            ///// The notification center.
            ///// </summary>
            //public NWENotificationManager NotificationCenter;
            //-------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// The initialization state.
            /// </summary>
            private bool Initialized = false;
            //-------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// The data need update state.
            /// </summary>
            //public bool DataWasUpdated;
            //-------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// Initializes a new instance of the <see cref="NetWorkedData.NWDGameDataManager+SharedInstanceAsSingleton"/> class.
            /// </summary>
            public SharedInstanceAsSingleton()
            {
                Interlocked.Increment(ref InstanceCounter);
                if (Initialized == false)
                {
                    // memorize the shared instance
                    //DataManager = NWDDataManager.SharedInstance();
                    //AppConfiguration = NWDAppConfiguration.SharedInstance();
                    //NotificationCenter = NWENotificationManager.SharedInstance();
                    //// ready to launch database
                    //DataManager.ConnectToDatabase();
                    // finish init
                    Initialized = true;
                    //Debug.LogVerbose ("NWDGameDataManager SharedInstanceAsSingleton InitInstance finished ("+InstanceCounter+")");
                }
                InstanceCounterLog();
            }
            //-------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// Releases unmanaged resources and performs other cleanup operations before the
            /// <see cref="NetWorkedData.NWDGameDataManager+SharedInstanceAsSingleton"/> is reclaimed by garbage collection.
            /// </summary>
            ~SharedInstanceAsSingleton()
            {
                Interlocked.Decrement(ref InstanceCounter);
                //Debug.LogVerbose ("NWDGameDataManager SharedInstanceAsSingleton destroy ... IT NEVER POSSIBLE IN RUNTIME");
                //if (NotificationCenter != null)
                //{
                //    NotificationCenter.RemoveAll();
                //    NotificationCenter = null;
                //}
                InstanceCounterLog();
            }
            //-------------------------------------------------------------------------------------------------------------
            /// <summary>
            /// Determines if Singleton is active.
            /// </summary>
            /// <returns><c>true</c> if is active; otherwise, <c>false</c>.</returns>
            public static bool IsActive()
            {
                return (kSharedInstaceAsSingleton != null);
            }
            //-------------------------------------------------------------------------------------------------------------
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the data manager.
        /// </summary>
        /// <value>The data manager.</value>
        //public NWDDataManager DataManager
        //{
        //    get; private set;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// Gets the app configuration.
        ///// </summary>
        ///// <value>The app configuration.</value>
        //public NWDAppConfiguration AppConfiguration
        //{
        //    get; private set;
        //}
        //-------------------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// Gets the notification center.
        ///// </summary>
        ///// <value>The notification center.</value>
        //public NWENotificationManager NotificationCenter
        //{
        //    get; private set;
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="NetWorkedData.NWDGameDataManager"/> class.
        /// </summary>
        public NWDGameDataManager()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the
        /// <see cref="NetWorkedData.NWDGameDataManager"/> is reclaimed by garbage collection.
        /// </summary>
        ~NWDGameDataManager()
        {
            //Debug.LogVerbose ("NWDGameDataManager Destroyed");
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Set needs the synchronize data to true.
        /// </summary>
        //public void NeedSynchronizeData()
        //{
        //    SharedInstanceAsSingleton.kSharedInstaceAsSingleton.DataWasUpdated = true;
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///// Player's account reference.
        ///// </summary>
        ///// <returns>The account reference.</returns>
        //public string PlayerAccountReference()
        //{
        //    return AppConfiguration.SelectedEnvironment().PlayerAccountReference;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public NWDErrorBlock ErrorDelegate;
        ////-------------------------------------------------------------------------------------------------------------
        //public void ErrorManagement(NWDError sError)
        //{
        //    if (ErrorDelegate != null)
        //    {
        //        ErrorDelegate(sError);
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //// Test network connection
        //public void NetworkStatutChange(NWDNetworkState sNewNetWorkStatut)
        //{
        //    if (sNewNetWorkStatut != NetworkStatut)
        //    {
        //        NetworkStatut = sNewNetWorkStatut;
        //        if (NetworkStatut == NWDNetworkState.OffLine)
        //        {
        //            NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_NETWORK_OFFLINE, null));
        //        }
        //        else if (NetworkStatut == NWDNetworkState.OnLine)
        //        {
        //            NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_NETWORK_ONLINE, null));
        //        }
        //        else
        //        {
        //            NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_NETWORK_UNKNOW, null));
        //        }
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        public NWDNetworkCheck NetWorkCheck;
        //-------------------------------------------------------------------------------------------------------------
        public void TestNetWork(NWDNetworkFinishDelegate sNetworkTestFinishedBlock = null)
        {
            if (NetWorkCheck != null)
            {
                NetWorkCheck.TestNetwork(sNetworkTestFinishedBlock);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDNetworkState HasNetWork()
        {
            NWDNetworkState rReturn = NWDNetworkState.Unknow;
            if (NetWorkCheck != null)
            {
                rReturn = NetWorkCheck.NetworkState;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool DatasLoaded()
        {
            return NWDDataManager.SharedInstance().DataLoaded();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The unity singleton. (destroy other instance)
        /// </summary>
        private static NWDGameDataManager kUnitySingleton = null;
        /// <summary>
        /// The application stand by.
        /// </summary>
        private bool ApplicationStandBy = false;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Raises the runtime method load event.
        /// </summary>
        [RuntimeInitializeOnLoadMethod]
        static void RuntimeInitializeOnLoad()
        {
            //Debug.Log("NWDGameDataManager RuntimeInitializeOnLoad");
            UnitySingleton();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Unity the singleton game object.
        /// </summary>
        /// <returns>The singleton game object.</returns>
        public static GameObject UnitySingletonGameObject()
        {
            return UnitySingleton().gameObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Unity singleton.
        /// </summary>
        /// <returns>The singleton.</returns>
        public static NWDGameDataManager UnitySingleton()
        {
            //Debug.Log("NWDGameDataManager UnitySingleton");
            if (kUnitySingleton == null)
            {

                // check real Singleton
                NWDGameDataManager[] tOtherInstances = FindObjectsOfType<NWDGameDataManager>();

                List<NWDGameDataManager> tOtherList = new List<NWDGameDataManager>();

                foreach (NWDGameDataManager tOtherInstance in tOtherInstances)
                {
                    tOtherList.Add(tOtherInstance);
                }
                if (tOtherList.Count == 0)
                {
                    // I need to create singleton
                    GameObject tObjToSpawn;
                    //spawn object
                    tObjToSpawn = new GameObject("NWDGameDataManagerUnitySingleton");
                    //Add Components
                    // Add component NWDGameDatManager to easly manage data from NWD
                    NWDGameDataManager tGameDataManager = tObjToSpawn.AddComponent<NWDGameDataManager>();
                    // Add thread utility
                    /* NWDDataManagerMainThread tGameDataManagerMainThread = */
                    tObjToSpawn.AddComponent<NWDDataManagerMainThread>();
                    // Add component to check user online
                    /*NWDUserNetWorkingScript tUserNetWorkingScript = */
                    tObjToSpawn.AddComponent<NWDUserNetWorkingScript>();
                    // Add component to check the network state
                    NWDNetworkCheck tNetWorkCheckScript = tObjToSpawn.AddComponent<NWDNetworkCheck>();
                    tGameDataManager.NetWorkCheck = tNetWorkCheckScript;

                    // keep k_Singleton
                    //kUnitySingleton = tObjToSpawn.GetComponent<NWDGameDataManager>();
                    kUnitySingleton = tGameDataManager;

                }
                else if (tOtherList.Count == 1)
                {
                    kUnitySingleton = tOtherList[0];
                    while (kUnitySingleton.transform.childCount > 0)
                    {
#if UNITY_EDITOR
                        if (EditorApplication.isPlaying == true)
                        {
                            Destroy(kUnitySingleton.transform.GetChild(0).gameObject);
                        }
                        else
                        {
                            DestroyImmediate(kUnitySingleton.transform.GetChild(0).gameObject);
                        }
#else
                        Destroy(kUnitySingleton.transform.GetChild(0).gameObject);
#endif
                    }
                }
                else
                {
                    while (tOtherList.Count > 0)
                    {
                        NWDGameDataManager tLast = tOtherList[0];
                        tOtherList.Remove(tLast);
#if UNITY_EDITOR
                        if (EditorApplication.isPlaying == true)
                        {
                            //Debug.Log("NWDGameDataManager A");
                            Destroy(tLast.gameObject);
                        }
                        else
                        {
                            //Debug.Log("NWDGameDataManager B");
                            DestroyImmediate(tLast.gameObject);
                        }
#else
                        Debug.Log("NWDGameDataManager C");
                        Destroy(tLast.gameObject);
#endif
                    }

                }
            }
            return kUnitySingleton;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Awake this instance.
        /// </summary>
        void Awake()
        {
            //Debug.Log("NWDGameDataManager Awake ()");
            SceneManager.sceneLoaded += OnSceneLoaded;
            //Debug.Log("<b>NWDGameDataManager Awake() </b>");
            //if (Application.isEditor == true)
            //{
            //    if (Application.isPlaying == true)
            //    {
            //        Debug.Log("<b>NWDGameDataManager Awake()</b> <color=green>I AM IN EDITOR</color> BUT <color=green>MODE PLAYER IS PLAYING</color>  ");
            //    }
            //    else
            //    {
            //        Debug.Log("<b>NWDGameDataManager Awake()</b> <color=green>I AM IN EDITOR</color> AND <color=red>MODE PLAYER IS NOT PLAYING</color> ");
            //    }
            //}
            //else
            //{
            //    Debug.Log("<b>NWDGameDataManager Awake()</b> <color=r-red>I AM NOT IN EDITOR</color>");
            //}

            //NWDToolbox.EditorAndPlaying("NWDGameDataManager Awake()");
            //Debug.LogVerbose ("NWDGameDataManager Awake");
            //Check if there is already an instance
            if (kUnitySingleton == null)
            {
                //if not, set it to this.
                kUnitySingleton = this;
#if UNITY_EDITOR
                if (Application.isEditor && Application.isPlaying == true)
                {
                    DontDestroyOnLoad(gameObject);
                }
#else
                DontDestroyOnLoad(gameObject);
#endif
            }
            //If instance already exists:
            else if (kUnitySingleton != this)
            {
                //Destroy this, this enforces our singleton pattern so there can only be one instance.
#if UNITY_EDITOR
                DestroyImmediate(this.gameObject);
#else
                Destroy(this.gameObject);
#endif
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            switch (NWDLauncher.CompileAs())
            {
                case NWDCompileType.Editor:
                    {
                        Debug.Log("<b>NWDGameDataManager Scene Loaded()</b> <color=green>I AM IN EDITOR</color> AND <color=red>PLAYMODE IS NOT PLAYING</color> ");

                    }
                    break;
                case NWDCompileType.PlayMode:
                    {
                        Debug.Log("<b>NWDGameDataManager Scene Loaded()</b> <color=green>I AM IN EDITOR</color> BUT <color=green>PLAYMODE IS PLAYING</color>  ");

                    }
                    break;
                case NWDCompileType.Runtime:
                    {
                        Debug.Log("<b>NWDGameDataManager Scene Loaded()</b> <color=red>I AM NOT IN EDITOR</color> AND <color=green>MODE RUNTIME IS PLAYING</color> ");
                    }
                    break;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// On enable. call every times is necessary.
        /// </summary>
        //void OnEnable()
        //{
        //    base.OnEnable();
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Start this instance. Called once.
        /// </summary>
        void Start()
        {
            //Debug.Log("NWDGameDataManager Start()");
            NWEScreenGauge[] tAllGauges = FindObjectsOfType<NWEScreenGauge>();
            Debug.Log("NWDGameDataManager Start() find " + tAllGauges.Length + " NWEScreenGauge");
            foreach (NWEScreenGauge tNWEScreenGauge in tAllGauges)
            {
                tNWEScreenGauge.IsVisible = false;
                if (tNWEScreenGauge.UseForLauncher == true)
                {
                    LoadingDatasGauge = tNWEScreenGauge;
                }
                if (tNWEScreenGauge.UseForSync == true)
                {
                    OperationGauge = tNWEScreenGauge;
                }
            }
            if (LoadingDatasGauge == null)
            {
                LoadingDatasGauge = gameObject.AddComponent<NWEScreenGaugeComplex>();
                LoadingDatasGauge.IsVisible = false;
            }
            if (LoadingDatasGauge == null)
            {
                OperationGauge = gameObject.AddComponent<NWEScreenGaugeComplex>();
                OperationGauge.IsVisible = false;
            }
            if (NWDLauncher.GetState() != NWDStatut.NetWorkedDataReady)
            {
                //Debug.LogWarning("NWD => not finish ... need load async!");
                if (LoadingDatasGauge != null)
                {
                    LoadingDatasGauge.IsVisible = true;
                }
            }
            LaunchRuntimeAsync();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public void ReloadAllDatas()
        //{
        //    StartCoroutine(NWDDataManager.SharedInstance().AsyncReloadAllObjectsEditor());
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public void ReloadAllDatasEditor()
        //{

        //}
        //-------------------------------------------------------------------------------------------------------------
        protected void OnGUI()
        {
            //Debug.Log ("NWDGameDataManager OnGUI()");
        }
        //-------------------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// Raises the destroy event.
        ///// </summary>
        //void OnDisable()
        //{
        //    base.OnDisable();
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Raises the application focus event.
        /// </summary>
        /// <param name="sHasFocus">If set to <c>true</c> s has focus.</param>
        protected void OnApplicationFocus(bool sHasFocus)
        {
            ApplicationStandBy = !sHasFocus;
            if (ApplicationStandBy == false)
            {
                //Debug.Log("OnApplicationFocus Focus is ON");
                //NWDNews.InstallAllNotifications(false);
            }
            else
            {
                //Debug.Log("OnApplicationFocus Focus is OFF");
                //NWDNews.InstallAllNotifications(true);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Raises the application pause event.
        /// </summary>
        /// <param name="sPauseStatus">If set to <c>true</c> s pause status.</param>
        protected void OnApplicationPause(bool sPauseStatus)
        {
            //NWDLauncher.OnApplicationPause(sPauseStatus);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void LauncherEngineReady(NWENotification sNotification, bool sPreloadDatas)
        {
            //Debug.Log("<color=red>!!!!!</color><color=orange> LauncherEngineReady</color>" + " state : " + NWDLauncher.GetState().ToString());
            if (LoadingDatasGauge != null)
            {
                LoadingDatasGauge.Show(true);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void LauncherEditorReady(NWENotification sNotification, bool sPreloadDatas)
        {
            //Debug.Log("<color=red>!!!!!</color><color=orange>LauncherEditorReady</color>");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void LauncherAccountReady(NWENotification sNotification, bool sPreloadDatas)
        {
            //Debug.Log("<color=red>!!!!!</color><color=orange>LauncherAccountReady</color>");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void LauncherStep(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            //Debug.Log("<color=red>!!!!!</color><color=orange>LauncherStep</color>");
            if (LoadingDatasGauge != null)
            {
                LoadingDatasGauge.SetValue(sPurcent);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void LauncherNetWorkdeDataReady(NWENotification sNotification, bool sPreloadDatas)
        {
            //Debug.Log("<color=red>!!!!!</color><color=orange>DataStartLoading</color>");
            if (LoadingDatasGauge != null)
            {
                LoadingDatasGauge.Show(false);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
