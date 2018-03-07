//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using UnityEngine;
using UnityEngine.UI;
using ColoredAdvancedDebug;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// 
    /// </summary>
    public class ShowLog : MonoBehaviour
	{
		//-------------------------------------------------------------------------------------------------------------
		public Button ButtonShowLog;
        public Button ButtonShowAccount;
		//-------------------------------------------------------------------------------------------------------------
		// Use this for initialization
		void Start ()
        {
            // Show log button
            Text tText = ButtonShowLog.GetComponentInChildren<Text>();
            ButtonShowLog.gameObject.SetActive(NWDConfiguration.GetBool(tText.text, true));
            tText.text = NWDConfiguration.GetLocalString (tText.text, tText.text);

            // Show account button
            tText = ButtonShowAccount.GetComponentInChildren<Text>();
            ButtonShowAccount.gameObject.SetActive(NWDConfiguration.GetBool(tText.text, true));
            tText.text = NWDConfiguration.GetLocalString(tText.text, tText.text);
        }
        //-------------------------------------------------------------------------------------------------------------
        void CADDebugOverlayAddOnCallBack(CADDebugOverlay sDebug)
        {
            Debug.Log("SceneController CADDebugOverlayAddOnCallBack()");
            Debug.Log("SceneController Add this string " + NWDDataManager.SharedInstance().Informations());
            sDebug.AddString(NWDDataManager.SharedInstance().Informations());
        }
        //-------------------------------------------------------------------------------------------------------------
        void Awake()
        {
            // Add notification for NetWokedData update
            NWDDataManager.SharedInstance().InformationsUpdate();
            NWDDataManager.SharedInstance().NotificationCenter.AddObserver(this, NWDNotificationConstants.K_DATAS_UPDATED, delegate (BTBNotification sNotification) {
                NWDDataManager.SharedInstance().InformationsUpdate();
            });

            // Add callback to 
            CADDebugOverlay.CADDebugOverlayAddOn += CADDebugOverlayAddOnCallBack;
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnDestroy()
        {
            NWDDataManager.SharedInstance().NotificationCenter.RemoveObserverEveryWhere(this);
            CADDebugOverlay.CADDebugOverlayAddOn -= CADDebugOverlayAddOnCallBack;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================