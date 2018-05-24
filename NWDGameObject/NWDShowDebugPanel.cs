﻿//=====================================================================================================================
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
    public class NWDShowDebugPanel : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        public Button ButtonShowLog;
        public Button ButtonShowAccount;
        public Button ButtonAddStats;
        public Button ButtonReloadDatas;
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
        public void GDPRTestAction()
        {
            Debug.Log(NWDGDPR.ExtractAndSave());
        }
        //-------------------------------------------------------------------------------------------------------------
        // Use this for initialization
        void Start()
        {
            // Show log button
            //Text tText = ButtonShowLog.GetComponentInChildren<Text>();
            //ButtonShowLog.gameObject.SetActive(NWDConfiguration.GetBool(tText.text, true));
            //tText.text = NWDConfiguration.GetLocalString(tText.text, tText.text);

            //// Show account button
            //tText = ButtonShowAccount.GetComponentInChildren<Text>();
            //ButtonShowAccount.gameObject.SetActive(NWDConfiguration.GetBool(tText.text, true));
            //tText.text = NWDConfiguration.GetLocalString(tText.text, tText.text);
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
            BTBNotificationManager.SharedInstance().AddObserver(this, NWDNotificationConstants.K_DATAS_WEB_UPDATE, delegate (BTBNotification sNotification)
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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================