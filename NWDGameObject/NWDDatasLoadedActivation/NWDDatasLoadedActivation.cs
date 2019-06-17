//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:45:41
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

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
        public bool UseCanvas = true;
        public bool UseChildGameObjects = false;
        //-------------------------------------------------------------------------------------------------------------
        void DataNotLoaded()
        {
            //Debug.Log("NWDAutolocalized DataIsLoaded()");
            BTBNotificationManager tNotificationManager = BTBNotificationManager.SharedInstance();
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_LOADED);
            if (UseCanvas == true)
            {
                Canvas tCanvas = GetComponent<Canvas>();
                if (tCanvas != null)
                {
                    tCanvas.enabled = ActiveDatasNotLoaded;
                }
            }
            if (UseChildGameObjects == true)
            {
                foreach (Transform tChild in transform)
                {
                    tChild.gameObject.SetActive(ActiveDatasNotLoaded);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void DataIsLoaded()
        {
            //Debug.Log("NWDAutolocalized DataIsLoaded()");
            if (UseCanvas == true)
            {
                Canvas tCanvas = GetComponent<Canvas>();
                if (tCanvas != null)
                {
                    tCanvas.enabled = ActiveDatasLoaded;
                }
            }
            if (UseChildGameObjects == true)
            {
                foreach (Transform tChild in transform)
                {
                    tChild.gameObject.SetActive(ActiveDatasLoaded);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void Awake()
        {
            //Debug.Log("NWDAutolocalized Awake()");
            if (NWDDataManager.SharedInstance().DataAccountLoaded == false)
            {
                //Debug.Log("NWDAutolocalized Awake() install observer");
                DataNotLoaded();
                BTBNotificationManager tNotificationManager = BTBNotificationManager.SharedInstance();
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_LOADED, delegate (BTBNotification sNotification)
                {
                    DataIsLoaded();
                    tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_LOADED);
                });
            }
            else
            {
                //Debug.Log("NWDAutolocalized Awake() DataAccountLoaded already loaded!");
                DataIsLoaded();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnDestroy()
        {
            //Debug.Log("NWDAutolocalized OnDestroy()");
            BTBNotificationManager tNotificationManager = BTBNotificationManager.SharedInstance();
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_LOADED);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
