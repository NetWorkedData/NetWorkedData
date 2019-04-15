// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:46:9
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using UnityEngine;
using UnityEngine.UI;
using BasicToolBox;

//#if COLORED_ADVANCED_DEBUG
//using ColoredAdvancedDebug;
//#endif

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
        public NWDParameterConnection ParameterConnection;
        public GameObject PanelShowDebug;
        //-------------------------------------------------------------------------------------------------------------
        public Image CartridgeImage;
        public Image CartridgeImageSecond;
        public Text CartridgeText;
        public Text TextDebug;
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
        public void ParametersTestAction()
        {
            if (ParameterConnection != null)
            {
                NWDParameter tParam = ParameterConnection.GetData();
                if (tParam != null)
                {
                    TextDebug.text = tParam.Name.GetLocalString();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GDPRTestAction()
        {
            NWDGDPR.ExtractAndSave();
            //Debug.Log(NWDGDPR.ExtractAndSave());
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AlertTestAction()
        {
            TextDebug.text = "TEST ALERT : NOK";
            BTBAlert.Alert("Test Alert", "Messsage", "Ok",  delegate (BTBMessageState state) {
            TextDebug.text = "TEST ALERT : OK";
            });
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DialogTestAction()
        {
            TextDebug.text = "TEST DIALOG: NOK";
            BTBDialog.Dialog("Test Dialog", "Choose", "YES", "NO", delegate (BTBMessageState state) {
                if (state == BTBMessageState.OK)
                {
                    TextDebug.text = "TEST DIALOG: YES";
                }
                else if (state == BTBMessageState.NOK)
                {
                    TextDebug.text = "TEST DIALOG: NO";
                }
                else
                {
                    TextDebug.text = "TEST DIALOG: ERROR";
                }
            });
        }
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            Debug.Log("NWDShowDebugPanel Start()");
            if (NWDGameDataManager.UnitySingleton().DatasLoaded() == true)
            {
                UpdateParameterText();
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
        }
        //-------------------------------------------------------------------------------------------------------------
        //#if COLORED_ADVANCED_DEBUG
        //void CADDebugOverlayAddOnCallBack(CADDebugOverlay sDebug)
        //{
        //    sDebug.AddString(NWDDataManager.SharedInstance().Informations());
        //}
        //#endif 
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
            //#if COLORED_ADVANCED_DEBUG
            //CADDebugOverlay.CADDebugOverlayAddOn += CADDebugOverlayAddOnCallBack;
            //#endif

            if (CartridgeImage != null)
            {
                CartridgeImage.color = NWDAppEnvironment.SelectedEnvironment().CartridgeColor;
            }
            if (CartridgeImageSecond != null)
            {
                NWDVersion tVersion = NWDVersion.CurrentData();
                if (tVersion != null)
                {
                    CartridgeImageSecond.color = tVersion.Cartridge.GetColor();
                }
            }
                if (CartridgeText != null)
            {
                CartridgeText.text = NWDAppEnvironment.SelectedEnvironment().Environment + " " + Application.version+
                    " (" + Application.systemLanguage + ">" + NWDDataManager.SharedInstance().PlayerLanguage + ") " + 
                    " WS" + NWDAppConfiguration.SharedInstance().WebBuild.ToString("0000")+
                    " ©Unity3D " + Application.unityVersion + string.Empty;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnDestroy()
        {
            BTBNotificationManager.SharedInstance().RemoveObserverEveryWhere(this);
            //#if COLORED_ADVANCED_DEBUG
            //CADDebugOverlay.CADDebugOverlayAddOn -= CADDebugOverlayAddOnCallBack;
            //#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateParameterText()
        {
            ParametersTestAction();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDatasLoaded(BTBNotification sNotification)
        {
            Debug.Log("NWDShowDebugPanel NotificationDatasLoaded()");
            UpdateParameterText();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDatasWebUpdate(BTBNotification sNotification)
        {
            Debug.Log("NWDShowDebugPanel NotificationDatasWebUpdate()");
            UpdateParameterText();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationDownloadSuccessed (BTBNotification sNotification)
        {
            Debug.Log("NWDShowDebugPanel NotificationWebOperationDownloadSuccessed()"); 
            UpdateParameterText();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationLanguageChanged(BTBNotification sNotification)
        {
            Debug.Log("NWDShowDebugPanel NotificationWebOperationDownloadSuccessed()");
            UpdateParameterText();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================