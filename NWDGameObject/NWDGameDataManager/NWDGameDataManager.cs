//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:45:48
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
        public NWEScreenGaugeComplex LoadingDatasGauge;
        //-------------------------------------------------------------------------------------------------------------
        public NWEScreenGaugeComplex OperationGauge;
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
                    // Add GUI component to draw some basic element : gauge, spinner, alert... not in canvas! For canvas element use prefab NWDCanvasDataManager
                    NWEScreenGaugeComplex tHealthGaugeComplex = tObjToSpawn.AddComponent<NWEScreenGaugeComplex>();
                    tHealthGaugeComplex.IsVisible = false;
                    NWEScreenGaugeComplex tHealthGaugeComplexB = tObjToSpawn.AddComponent<NWEScreenGaugeComplex>();
                    tHealthGaugeComplexB.IsVisible = false;
                    tGameDataManager.LoadingDatasGauge = tHealthGaugeComplex;
                    tGameDataManager.OperationGauge = tHealthGaugeComplexB;

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
                    Destroy (kUnitySingleton.transform.GetChild(0).gameObject);
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
                    Destroy (tLast.gameObject);
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
                DontDestroyOnLoad (gameObject);
#endif
            }
            //If instance already exists:
            else if (kUnitySingleton != this)
            {
                //Destroy this, this enforces our singleton pattern so there can only be one instance.
#if UNITY_EDITOR
                DestroyImmediate(this.gameObject);
#else
                Destroy (this.gameObject);
#endif
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //Debug.Log("<b>NWDGameDataManager Scene Loaded() </b>");
            if (Application.isEditor == true)
            {
                if (Application.isPlaying == true)
                {
                    Debug.Log("<b>NWDGameDataManager Scene Loaded()</b> <color=green>I AM IN EDITOR</color> BUT <color=green>MODE PLAYER IS PLAYING</color>  ");
                }
                else
                {
                    Debug.Log("<b>NWDGameDataManager Scene Loaded()</b> <color=green>I AM IN EDITOR</color> AND <color=red>MODE PLAYER IS NOT PLAYING</color> ");
                }
            }
            else
            {
                Debug.Log("<b>NWDGameDataManager Scene Loaded()</b> <color=r-red>I AM NOT IN EDITOR</color>");
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
            if (NWDLauncher.GetState() != NWDStatut.NetWorkedDataReady)
            {
                Debug.LogWarning("NWD => not finish ... need load async!");
                if (LoadingDatasGauge != null)
                {
                    LoadingDatasGauge.IsVisible = true;
                }
                Debug.LogWarning("NWD => LaunchResume!");
                NWDLauncher.LaunchResume();
            }
            else
            {
                //Debug.LogWarning("NWD => NWDStatut.NetWorkedDataReady!");
                if (LoadingDatasGauge != null)
                {
                    LoadingDatasGauge.IsVisible = false;
                }
            }
            // push gauge for web opeartion to invisible
            if (OperationGauge != null)
            {
                OperationGauge.IsVisible = false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReloadAllDatas()
        {
            StartCoroutine(NWDDataManager.SharedInstance().AsyncReloadAllObjectsEditor());
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReloadAllDatasEditor()
        {

        }
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
            NWDLauncher.OnApplicationPause(sPauseStatus);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void EngineLaunch(NWENotification sNotification, bool sPreloadDatas)
        {
            Debug.Log("<color=red>!!!!!</color><color=orange> EngineLaunch</color>");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DBEditorConnected(NWENotification sNotification, bool sPreloadDatas)
        {
            Debug.Log("<color=red>!!!!!</color><color=orange>DBEditorConnected</color>");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DBEditorStartAsyncLoading(NWENotification sNotification, bool sPreloadDatas)
        {
            LaunchNext();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataEditorStartLoading(NWENotification sNotification, bool sPreloadDatas)
        {
            Debug.Log("<color=red>!!!!!</color><color=orange>DataEditorStartLoading</color>");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataEditorPartialLoaded(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            Debug.Log("<color=red>!!!!!</color><color=orange>DataEditorPartialLoaded</color>");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataEditorLoaded(NWENotification sNotification, bool sPreloadDatas)
        {
            Debug.Log("<color=red>!!!!!</color><color=orange>DataEditorLoaded</color>");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DBAccountPinCodeRequest(NWENotification sNotification, bool sPreloadDatas)
        {
            Debug.Log("<color=red>!!!!!</color><color=orange>DBAccountPinCodeRequest</color>");
           // PinCodeInsert("111","111");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DBAccountPinCodeSuccess(NWENotification sNotification, bool sPreloadDatas)
        {
            Debug.Log("<color=red>!!!!!</color><color=orange>DBAccountPinCodeSuccess</color>");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DBAccountPinCodeFail(NWENotification sNotification, bool sPreloadDatas)
        {
            Debug.Log("<color=red>!!!!!</color><color=orange>DBAccountPinCodeFail</color>");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DBAccountPinCodeStop(NWENotification sNotification, bool sPreloadDatas)
        {
            Debug.Log("<color=red>!!!!!</color><color=orange>DBAccountPinCodeStop</color>");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DBAccountPinCodeNeeded(NWENotification sNotification, bool sPreloadDatas)
        {
            Debug.Log("<color=red>!!!!!</color><color=orange>DBAccountPinCodeNeeded</color>");
           // PinCodeInsert("111","111");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DBAccountConnected(NWENotification sNotification, bool sPreloadDatas)
        {
            Debug.Log("<color=red>!!!!!</color><color=orange>DBAccountConnected</color>");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DBAccountStartAsyncLoading(NWENotification sNotification, bool sPreloadDatas)
        {
            LaunchNext();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataAccountStartLoading(NWENotification sNotification, bool sPreloadDatas)
        {
            Debug.Log("<color=red>!!!!!</color><color=orange>DataAccountStartLoading</color>");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataAccountPartialLoaded(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            Debug.Log("<color=red>!!!!!</color><color=orange>DataAccountPartialLoaded</color>");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataAccountLoaded(NWENotification sNotification, bool sPreloadDatas)
        {
            Debug.Log("<color=red>!!!!!</color><color=orange>DataAccountLoaded</color>");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataStartLoading(NWENotification sNotification, bool sPreloadDatas)
        {
            Debug.Log("<color=red>!!!!!</color><color=orange>DataStartLoading</color>");
            if (LoadingDatasGauge != null)
            {
                LoadingDatasGauge.IsVisible = true;
                LoadingDatasGauge.SetHorizontalValue(0.0F);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataPartialLoaded(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            Debug.Log("<color=red>!!!!!</color><color=orange>DataPartialLoaded : </color> <color=red>" + sPurcent + "</color>");
            if (LoadingDatasGauge != null)
            {
                LoadingDatasGauge.SetHorizontalValue(sPurcent);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataLoaded(NWENotification sNotification, bool sPreloadDatas)
        {
            Debug.Log("<color=red>!!!!!</color><color=orange>DataLoaded</color>");
            if (LoadingDatasGauge != null)
            {
                LoadingDatasGauge.IsVisible = false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataIndexationStartAsync(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            LaunchNext();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataIndexationStart(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            Debug.Log("<color=red>!!!!!</color><color=orange>DataIndexationStart</color>");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataIndexationStep(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            // create your method by override
            Debug.Log("<color=red>!!!!!</color><color=orange>DataIndexationStep</color>");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataIndexationFinish(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            Debug.Log("<color=red>!!!!!</color><color=orange>DataIndexationFinish</color>");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void EngineReady(NWENotification sNotification, bool sPreloadDatas)
        {
            Debug.Log("<color=red>!!!!!</color><color=orange>DataStartLoading</color>");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
