//=====================================================================================================================
//
// ideMobi copyright 2018
// All rights reserved by ideMobi
//
//=====================================================================================================================

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDDatasLoadedActivation : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        public bool ActiveDatasLoaded = true;
        public bool ActiveDatasNotLoaded = false;
        //-------------------------------------------------------------------------------------------------------------
        void DataIsLoaded()
        {
            Debug.Log("NWDAutolocalized DataIsLoaded()");
            BTBNotificationManager tNotificationManager = BTBNotificationManager.SharedInstance();
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_LOADED);
            foreach (Transform tChild in transform)
            {
                tChild.GetComponent<Canvas>().enabled = ActiveDatasLoaded;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void Awake()
        {
            Debug.Log("NWDAutolocalized Awake()");
            if (NWDDataManager.SharedInstance().DataAccountLoaded == false)
            {
                Debug.Log("NWDAutolocalized Awake() install observer");
                foreach (Transform tChild in transform)
                {
                    tChild.GetComponent<Canvas>().enabled = ActiveDatasNotLoaded;
                }
                BTBNotificationManager tNotificationManager = BTBNotificationManager.SharedInstance();
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_LOADED, delegate (BTBNotification sNotification)
                {
                    DataIsLoaded();
                });
            }
            else
            {
                Debug.Log("NWDAutolocalized Awake() DataAccountLoaded already loaded!");
                foreach (Transform tChild in transform)
                {
                    tChild.GetComponent<Canvas>().enabled = ActiveDatasLoaded;
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnDestroy()
        {
            Debug.Log("NWDAutolocalized OnDestroy()");
            BTBNotificationManager tNotificationManager = BTBNotificationManager.SharedInstance();
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_LOADED);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
