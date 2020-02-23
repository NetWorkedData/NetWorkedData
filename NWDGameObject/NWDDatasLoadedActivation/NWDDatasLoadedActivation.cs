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
    public enum NWDDatasLoadedActivationState
    {
        ActiveWhenDatasNotLoaded,
        ActiveWhenDatasLoaded
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDDatasLoadedActivation : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDDatasLoadedActivationState ActiveState = NWDDatasLoadedActivationState.ActiveWhenDatasLoaded;
        public bool UseCanvasGroup = true;
        public bool UseChildGameObjects = false;
        //-------------------------------------------------------------------------------------------------------------
        void DataNotLoaded()
        {
            NWENotificationManager tNotificationManager = NWENotificationManager.SharedInstance();
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NETWORKEDDATA_READY);
            if (UseCanvasGroup == true)
            {
                CanvasGroup tCanvas = GetComponent<CanvasGroup>();
                if (tCanvas == null)
                {
                    tCanvas = gameObject.AddComponent<CanvasGroup>();
                }
                if (ActiveState == NWDDatasLoadedActivationState.ActiveWhenDatasNotLoaded)
                {
                    tCanvas.alpha = 1;
                    tCanvas.interactable = true;
                }
                else
                {
                    tCanvas.alpha = 0;
                    tCanvas.interactable = false;
                }
            }
            if (UseChildGameObjects == true)
            {
                foreach (Transform tChild in transform)
                {
                    if (ActiveState == NWDDatasLoadedActivationState.ActiveWhenDatasNotLoaded)
                    {
                        tChild.gameObject.SetActive(true);
                    }
                    else
                    {
                        tChild.gameObject.SetActive(false);
                    }
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
            if (UseCanvasGroup == true)
            {
                CanvasGroup tCanvas = GetComponent<CanvasGroup>();
                if (ActiveState == NWDDatasLoadedActivationState.ActiveWhenDatasLoaded)
                {
                    tCanvas.alpha = 1;
                    tCanvas.interactable = true;
                }
                else
                {
                    tCanvas.alpha = 0;
                    tCanvas.interactable = false;
                }
            }
            if (UseChildGameObjects == true)
            {
                foreach (Transform tChild in transform)
                {
                    if (ActiveState == NWDDatasLoadedActivationState.ActiveWhenDatasLoaded)
                    {
                        tChild.gameObject.SetActive(true);
                    }
                    else
                    {
                        tChild.gameObject.SetActive(false);
                    }
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
