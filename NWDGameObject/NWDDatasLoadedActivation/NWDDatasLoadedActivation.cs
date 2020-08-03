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
#endif
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
                if (tCanvas == null)
                {
                    tCanvas = gameObject.AddComponent<CanvasGroup>();
                }
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
                //Debug.Log("Data not loaded");
                DataNotLoaded();
            }
            else
            {
                //Debug.Log("Data allready loaded");
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
