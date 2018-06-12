//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using UnityEngine;
using UnityEngine.UI;
using BasicToolBox;

#if COLORED_ADVANCED_DEBUG
using ColoredAdvancedDebug;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// 
    /// </summary>
    public class NWDShowDebugPanel : NWDCallBack
    {
        //-------------------------------------------------------------------------------------------------------------
        public Button ButtonShowLog;
        public Button ButtonShowAccount;
        public Button ButtonAddStats;
        public Button ButtonReloadDatas;
        public NWDParameterConnection ParamOfApp;
        public Text ParamText;
        public GameObject PanelShowDebug;
        //-------------------------------------------------------------------------------------------------------------
        public Text TestAlertText;
        public Image CartridgeImage;
        public Text CartridgeText;
        public Text TextAccount;
        public Text TextReloadData;
        public Text TextAddStat;
        public Text TextGRPD;
        public Text TextShowLog;
        public Text TextTestAlert;
        //-------------------------------------------------------------------------------------------------------------
        private const string K_NWD_SHOW_DEBUG_PANEL = "NWDShowDebugPanel";
        //-------------------------------------------------------------------------------------------------------------
        public void ReloadDatasAction()
        {
            NWDGameDataManager.UnitySingleton().ReloadAllDatas();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddStatsAction()
        {
            NWDUserStats tStats = NWDUserStats.NewObject();
            tStats.SaveModificationsIfModified();
#if UNITY_EDITOR
            UnityEditor.EditorWindow tEditorWindow = UnityEditor.EditorWindow.focusedWindow;
            NWDUserStats.SetObjectInEdition(tStats);
            tEditorWindow.Focus();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ShowHidePanel()
        {
            if (PanelShowDebug.activeInHierarchy)
            {
                PanelShowDebug.SetActive(false);
                PlayerPrefs.SetInt(K_NWD_SHOW_DEBUG_PANEL, 0);
            }
            else
            {
                PanelShowDebug.SetActive(true);
                PlayerPrefs.SetInt(K_NWD_SHOW_DEBUG_PANEL, 1);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GDPRTestAction()
        {
            Debug.Log(NWDGDPR.ExtractAndSave());
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AlertTestAction()
        {
            TestAlertText.text = "TEST ALERT : NOK";
            BTBAlert tMessage = new BTBAlert("Test Alert", "Messsage", "Ok",  delegate (BTBMessageState state) {
                TestAlertText.text = "TEST ALERT : OK";
            });
        }
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            Debug.Log("NWDShowDebugPanel Start()");
            /*if (NWDGameDataManager.UnitySingleton().DatasIsLoaded() == true)
            {
                UpdateParameterText();
            }*/

            int tShowPanel = PlayerPrefs.GetInt(K_NWD_SHOW_DEBUG_PANEL, 0);
            if (tShowPanel == 0)
            {
                if (PanelShowDebug != null)
                {
                    PanelShowDebug.SetActive(false);
                }
            }
            else
            {
                if (PanelShowDebug != null)
                {
                    PanelShowDebug.SetActive(true);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        #if COLORED_ADVANCED_DEBUG
        void CADDebugOverlayAddOnCallBack(CADDebugOverlay sDebug)
        {
            sDebug.AddString(NWDDataManager.SharedInstance().Informations());
        }
        #endif 
        //-------------------------------------------------------------------------------------------------------------
        void Awake()
        {
            // Add notification for NetWokedData update
            NWDDataManager.SharedInstance().InformationsUpdate();
            BTBNotificationManager.SharedInstance().AddObserverForAll(this, NWDNotificationConstants.K_DATAS_WEB_UPDATE, delegate (BTBNotification sNotification)
            {
                NWDDataManager.SharedInstance().InformationsUpdate();
            });

            // Add callback to 
            #if COLORED_ADVANCED_DEBUG
            CADDebugOverlay.CADDebugOverlayAddOn += CADDebugOverlayAddOnCallBack;
#endif

            if (CartridgeImage != null)
            {
                CartridgeImage.color = NWDAppEnvironment.SelectedEnvironment().CartridgeColor;
            }
            if (CartridgeText != null)
            {
                CartridgeText.text = NWDAppEnvironment.SelectedEnvironment().Environment + " " + Application.version + " WS"+ NWDAppConfiguration.SharedInstance().WebBuild.ToString("0000")+" ©Unity3D " + Application.unityVersion + "";
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnDestroy()
        {
            BTBNotificationManager.SharedInstance().RemoveObserverEveryWhere(this);
            #if COLORED_ADVANCED_DEBUG
            CADDebugOverlay.CADDebugOverlayAddOn -= CADDebugOverlayAddOnCallBack;
            #endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateParameterText()
        {
            Debug.Log("NWDShowDebugPanel UpdateParameterText()");
            if (ParamOfApp != null)
            {
                NWDParameter tParam = ParamOfApp.GetObject();
                if (tParam != null && ParamText != null)
                {
                    ParamText.text = ParamOfApp.GetObject().ValueString.GetLocalString();
                }
            }

            // Localize Text
            NWDLocalization.AutoLocalize(TextAccount);
            NWDLocalization.AutoLocalize(TextReloadData);
            NWDLocalization.AutoLocalize(TextAddStat);
            NWDLocalization.AutoLocalize(TextGRPD);
            NWDLocalization.AutoLocalize(TextShowLog);
            NWDLocalization.AutoLocalize(TextTestAlert);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDatasLoaded(BTBNotification sNotification, bool sPreloadDatas)
        {
            Debug.Log("NWDShowDebugPanel NotificationDatasLoaded()");
            // create your method by override
            UpdateParameterText();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDatasWebUpdate(BTBNotification sNotification)
        {
            Debug.Log("NWDShowDebugPanel NotificationDatasWebUpdate()");
            // create your method by override
            UpdateParameterText();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationDownloadSuccessed (BTBNotification sNotification)
        {
            Debug.Log("NWDShowDebugPanel NotificationWebOperationDownloadSuccessed()"); 
            // create your method by override
            UpdateParameterText();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationLanguageChanged(BTBNotification sNotification)
        {
            Debug.Log("NWDShowDebugPanel NotificationWebOperationDownloadSuccessed()");
            // create your method by override
            UpdateParameterText();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================