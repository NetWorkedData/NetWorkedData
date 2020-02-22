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

using UnityEngine;

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
            NWENotificationManager tNotificationManager = NWENotificationManager.SharedInstance();
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NETWORKEDDATA_READY);
            if (UseCanvas == true)
            {
                Canvas tCanvas = GetComponent<Canvas>();
                if (tCanvas == null)
                {
                    tCanvas = gameObject.AddComponent<Canvas>();
                }
                tCanvas.enabled = ActiveDatasNotLoaded;
            }
            if (UseChildGameObjects == true)
            {
                foreach (Transform tChild in transform)
                {
                    tChild.gameObject.SetActive(ActiveDatasNotLoaded);
                }
            }
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_NETWORKEDDATA_READY, delegate (NWENotification sNotification)
            {
                DataIsLoaded();
            });
        }
        //-------------------------------------------------------------------------------------------------------------
        void DataIsLoaded()
        {
            NWENotificationManager tNotificationManager = NWENotificationManager.SharedInstance();
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NETWORKEDDATA_READY);
            if (UseCanvas == true)
            {
                Canvas tCanvas = GetComponent<Canvas>();
                if (tCanvas == null)
                {
                    tCanvas = gameObject.AddComponent<Canvas>();
                }
                tCanvas.enabled = ActiveDatasLoaded;
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
        void OnEnable()
        {
            if (NWDLauncher.GetState() != NWDStatut.NetWorkedDataReady)
            {
                Debug.Log("Data not loaded");
                DataNotLoaded();
            }
            else
            {
                Debug.Log("Data allready loaded");
                DataIsLoaded();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnDestroy()
        {
            NWENotificationManager tNotificationManager = NWENotificationManager.SharedInstance();
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NETWORKEDDATA_READY);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
