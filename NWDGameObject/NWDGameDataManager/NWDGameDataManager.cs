//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SocialPlatforms;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Threading;

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
    [ExecuteInEditMode] // We use this only in playmode so don't attribut ExecuteInEditMode
    public partial class NWDGameDataManager : NWDCallBack //MonoBehaviour 
    {
        //-------------------------------------------------------------------------------------------------------------
        //public NWDNetworkState NetworkStatut = NWDNetworkState.Unknow;
        //-------------------------------------------------------------------------------------------------------------
        public BTBScreenGaugeComplex LoadingDatasGauge;
        //-------------------------------------------------------------------------------------------------------------
        public BTBScreenGaugeComplex OperationGauge;
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
            //public BTBNotificationManager NotificationCenter;
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
                    //NotificationCenter = BTBNotificationManager.SharedInstance();
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
        //public BTBNotificationManager NotificationCenter
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
        public NWDErrorBlock ErrorDelegate;
        //-------------------------------------------------------------------------------------------------------------
        public void ErrorManagement(NWDError sError)
        {
            if (ErrorDelegate != null)
            {
                ErrorDelegate(sError);
            }
        }
        ////-------------------------------------------------------------------------------------------------------------
        //// Test network connection
        //public void NetworkStatutChange(NWDNetworkState sNewNetWorkStatut)
        //{
        //    if (sNewNetWorkStatut != NetworkStatut)
        //    {
        //        NetworkStatut = sNewNetWorkStatut;
        //        if (NetworkStatut == NWDNetworkState.OffLine)
        //        {
        //            BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_NETWORK_OFFLINE, null));
        //        }
        //        else if (NetworkStatut == NWDNetworkState.OnLine)
        //        {
        //            BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_NETWORK_ONLINE, null));
        //        }
        //        else
        //        {
        //            BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_NETWORK_UNKNOW, null));
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
        public bool DatasIsLoaded()
		{
			return NWDTypeLauncher.DataLoaded;
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
                    /* NWDDataManagerMainThread tGameDataManagerMainThread = */tObjToSpawn.AddComponent<NWDDataManagerMainThread>();
                    // Add component to check user online
                    /*NWDUserNetWorkingScript tUserNetWorkingScript = */tObjToSpawn.AddComponent<NWDUserNetWorkingScript>();
                    // Add component to check the network state
                    NWDNetworkCheck tNetWorkCheckScript = tObjToSpawn.AddComponent<NWDNetworkCheck>();
                    tGameDataManager.NetWorkCheck = tNetWorkCheckScript;
                    // Add GUI component to draw some basic element : gauge, spinner, alert... not in canvas! For canvas element use prefab NWDCanvasDataManager
                    BTBScreenGaugeComplex tHealthGaugeComplex = tObjToSpawn.AddComponent<BTBScreenGaugeComplex>();
                    tHealthGaugeComplex.IsVisible = false;
                    BTBScreenGaugeComplex tHealthGaugeComplexB = tObjToSpawn.AddComponent<BTBScreenGaugeComplex>();
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
            if (NWDTypeLauncher.DataLoaded == false)
            {
                //Debug.LogWarning("NWD => Datas ARE NOT LOADED ... load async now");
                ReloadAllDatas();
            }
            else
            {
                // hello every body DATA ARE LOADED
                //BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_LOADED);
                //Debug.LogWarning("NWD => Datas ARE ALLREADY LOADED");
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
            StartCoroutine(NWDDataManager.SharedInstance().AsyncReloadAllObjects());
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
        ///// <summary>
        ///// Raises the destroy event.
        ///// </summary>
        //void OnDestroy()
        //{
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
                //Debug.LogVerbose ("OnApplicationFocus Focus is ON");
            }
            else
            {
                //Debug.LogVerbose ("OnApplicationFocus Focus is OFF");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Raises the application pause event.
        /// </summary>
        /// <param name="sPauseStatus">If set to <c>true</c> s pause status.</param>
        protected void OnApplicationPause(bool sPauseStatus)
        {
            ApplicationStandBy = sPauseStatus;
            if (ApplicationStandBy == false)
            {
                //Debug.LogVerbose ("OnApplicationPause Pause is OFF");
            }
            else
            {
                //Debug.LogVerbose ("OnApplicationPause Pause is ON");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationEngineLaunch(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDatasStartLoading(BTBNotification sNotification, bool sPreloadDatas)
        {
               //Debug.Log("NWD => NWDGameDataManager NOTIFICATION_DATAS_LOADEDNOTIFICATION_DATAS_START_LOADINGNOTIFIED ()");
            if (LoadingDatasGauge != null)
            {
                LoadingDatasGauge.IsVisible = true;
                LoadingDatasGauge.SetHorizontalValue(0.0F);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDatasPartialLoaded(BTBNotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            //Debug.Log("NWD => NWDGameDataManager NOTIFICATION_DATAS_PARTIAL_LOADED NOTIFIED ()");
            if (LoadingDatasGauge != null)
            {
                LoadingDatasGauge.SetHorizontalValue(sPurcent);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDatasLoaded(BTBNotification sNotification, bool sPreloadDatas)
        {
            //Debug.Log("NWD => NWDGameDataManager NOTIFICATION_DATAS_LOADED NOTIFIED ()");
            if (LoadingDatasGauge != null)
            {
                LoadingDatasGauge.IsVisible = false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationLanguageChanged(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDataLocalUpdate(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDataLocalInsert(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDataLocalDelete(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDatasWebUpdate(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationError(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationAccountChanged(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationAccountSessionExpired(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationAccountBanned(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationNetworkOffLine(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationNetworkOnLine(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationNetworkUnknow(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationNetworkCheck(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationError(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationUploadInProgress(BTBNotification sNotification, float sPurcent)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationDownloadInProgress(BTBNotification sNotification, float sPurcent)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationDownloadIsDone(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationDownloadFailed(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationDownloadError(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationDownloadSuccessed(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationGeneric(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
