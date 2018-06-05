//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using UnityEngine;
using UnityEngine.UI;
#if COLORED_ADVANCED_DEBUG
using ColoredAdvancedDebug;
#endif
using BasicToolBox;
#if UNITY_EDITOR
using UnityEditor;
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
        private const string K_NWD_SHOW_DEBUG_PANEL = "NWDShowDebugPanel";
        //-------------------------------------------------------------------------------------------------------------
        public void ReloadDatasAction()
        {
            //NWDDataManager.SharedInstance().ReloadAllObjects();
            NWDGameDataManager.UnitySingleton().ReloadAllDatas();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddStatsAction()
        {
            NWDUserStats tStats = NWDUserStats.NewObject();
            tStats.SaveModificationsIfModified();
#if UNITY_EDITOR
            EditorWindow tEditorWindow = EditorWindow.focusedWindow;
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
        void Start()
        {
            Debug.Log("NWDShowDebugPanel Start()");
            if (NWDGameDataManager.UnitySingleton().DatasIsLoaded() == true)
            {
                // loaded (preloaded?) 
                Debug.Log("NWDShowDebugPanel Start() PreLoaded");
                UpdateParameterText();
            }
            else
            {
                Debug.Log("NWDShowDebugPanel Start() Async Loading");
                //load async ?
            }

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

            // Show log button
            //Text tText = ButtonShowLog.GetComponentInChildren<Text>();
            //ButtonShowLog.gameObject.SetActive(NWDParameter.GetBool(tText.text, true));
            //tText.text = NWDParameter.GetLocalString(tText.text, tText.text);

            //// Show account button
            //tText = ButtonShowAccount.GetComponentInChildren<Text>();
            //ButtonShowAccount.gameObject.SetActive(NWDParameter.GetBool(tText.text, true));
            //tText.text = NWDParameter.GetLocalString(tText.text, tText.text);
        }
        //-------------------------------------------------------------------------------------------------------------
        #if COLORED_ADVANCED_DEBUG
        void CADDebugOverlayAddOnCallBack(CADDebugOverlay sDebug)
        {
            //Debug.Log("ShowLog CADDebugOverlayAddOnCallBack()");
            //Debug.Log("ShowLog Add this string " + NWDDataManager.SharedInstance().Informations());
            sDebug.AddString(NWDDataManager.SharedInstance().Informations());
        }
        #endif 
        //-------------------------------------------------------------------------------------------------------------
        void Awake()
        {
            //Debug.Log("NetWorkedData Awake");
            // Add notification for NetWokedData update
            NWDDataManager.SharedInstance().InformationsUpdate();
            BTBNotificationManager.SharedInstance().AddObserverForAll(this, NWDNotificationConstants.K_DATAS_WEB_UPDATE, delegate (BTBNotification sNotification)
            {
                NWDDataManager.SharedInstance().InformationsUpdate();
            });
            // Add callback to 
            #if COLORED_ADVANCED_DEBUG
            //Debug.Log("Add CADDebugOverlayAddOnCallBack()");
            CADDebugOverlay.CADDebugOverlayAddOn += CADDebugOverlayAddOnCallBack;
            #endif
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnDestroy()
        {
            //Debug.Log("NetWorkedData OnDestroy");
            BTBNotificationManager.SharedInstance().RemoveObserverEveryWhere(this);
            #if COLORED_ADVANCED_DEBUG
            //Debug.Log("Remove CADDebugOverlayAddOnCallBack()");
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