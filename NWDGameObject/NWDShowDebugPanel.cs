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
using UnityEngine;
using UnityEngine.UI;
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
        //[SerializeField] private Button ButtonShowLog = null;
        //[SerializeField] private Button ButtonShowAccount = null;
        //[SerializeField] private Button ButtonAddStats = null;
        //[SerializeField] private Button ButtonReloadDatas = null;
        [SerializeField] private NWDParameterConnection ParameterConnection = null;
        [SerializeField] private GameObject PanelShowDebug = null;
        //-------------------------------------------------------------------------------------------------------------
        [SerializeField] private Image CartridgeImage = null;
        [SerializeField] private Image CartridgeImageSecond = null;
        [SerializeField] private Text CartridgeText = null;
        [SerializeField] private Text TextDebug = null;
        //-------------------------------------------------------------------------------------------------------------
        const string K_NWD_SHOW_DEBUG_PANEL = "NWDShowDebugPanel";
        //-------------------------------------------------------------------------------------------------------------
        public void ShowHidePanel()
        {
            if (PanelShowDebug.activeInHierarchy)
            {
                PanelShowDebug.SetActive(false);
                //PlayerPrefs.SetInt(K_NWD_SHOW_DEBUG_PANEL, 0);
            }
            else
            {
                PanelShowDebug.SetActive(true);
                //PlayerPrefs.SetInt(K_NWD_SHOW_DEBUG_PANEL, 1);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ParametersTestAction()
        {
            Debug.Log("NWDShowDebugPanel ParametersTestAction()");

            foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeLoadedList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                Debug.Log(" class " + tHelper.ClassNamePHP + " row nb =" + tHelper.Datas.Count);
            }

            if (ParameterConnection != null)
            {
                Debug.Log("NWDShowDebugPanel ParametersTestAction() ParameterConnection not null ParameterConnection.reference = " + ParameterConnection.Reference);
                NWDParameter tParam = ParameterConnection.GetReachableData();
                if (tParam != null)
                {
                    Debug.Log("NWDShowDebugPanel ParametersTestAction() tParam not null get local string = " + tParam.Name.GetLocalString());
                    TextDebug.text = tParam.Name.GetLocalString();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateParameterText()
        {
            ParametersTestAction();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GDPRTestAction()
        {
#if NWD_RGPD
            NWDGDPR.ExtractAndSave();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AlertTestAction()
        {
            Debug.Log("NWDShowDebugPanel AlertTestAction()");
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DialogTestAction()
        {
            Debug.Log("NWDShowDebugPanel DialogTestAction()");
        }
        //-------------------------------------------------------------------------------------------------------------
        // PRIVATE METHOD
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            Debug.Log("NWDShowDebugPanel Start()");
            if (NWDGameDataManager.UnitySingleton().DatasLoaded() == true)
            {
                UpdateParameterText();
            }

            /*int tShowPanel = PlayerPrefs.GetInt(K_NWD_SHOW_DEBUG_PANEL, 0);
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
            }*/
        }
        //-------------------------------------------------------------------------------------------------------------
        void Awake()
        {
            // Add notification for NetWokedData update
            NWDDataManager.SharedInstance().InformationsUpdate();
#if NWD_CRUD_NOTIFICATION
            NWENotificationManager.SharedInstance().AddObserverForAll(this, NWDNotificationConstants.K_DATAS_WEB_UPDATE, delegate (NWENotification sNotification)
            {
                NWDDataManager.SharedInstance().InformationsUpdate();
            });
#endif

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
                CartridgeText.text = NWDAppEnvironment.SelectedEnvironment().Environment + " " + Application.version +
                    " (" + Application.systemLanguage + ">" + NWDDataManager.SharedInstance().PlayerLanguage + ") " +
                    " WS" + NWDAppConfiguration.SharedInstance().WebBuild.ToString("0000") +
                    " ©Unity3D " + Application.unityVersion + string.Empty;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnDestroy()
        {
            NWENotificationManager.SharedInstance().RemoveObserverEveryWhere(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        // OVERRIDE
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDatasLoaded(NWENotification sNotification)
        {
            Debug.Log("NWDShowDebugPanel NotificationDatasLoaded()");
            UpdateParameterText();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDatasWebUpdate(NWENotification sNotification)
        {
            Debug.Log("NWDShowDebugPanel NotificationDatasWebUpdate()");
            UpdateParameterText();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationDownloadSuccessed(NWENotification sNotification)
        {
            Debug.Log("NWDShowDebugPanel NotificationWebOperationDownloadSuccessed()");
            UpdateParameterText();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationLanguageChanged(NWENotification sNotification)
        {
            Debug.Log("NWDShowDebugPanel NotificationLanguageChanged()");
            UpdateParameterText();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
