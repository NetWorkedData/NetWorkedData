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
using System.Collections;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDUserNetWorkingScript : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        public bool IsActive = true;
        //-------------------------------------------------------------------------------------------------------------
        public void CheckNow() // TODO TEST
        {
            //NWDUserNetWorking.NetworkingUpdate();
            // but need to chnage the waiting seconds? 
            StopCoroutine(UserNetworkinUpdate());
            StartCoroutine(UserNetworkinUpdate());
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CheckStart() // TODO TEST
        {
            StartCoroutine(UserNetworkinUpdate());
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CheckStop() // TODO TEST
        {
            StopCoroutine(UserNetworkinUpdate());
        }
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            if (NWDDataManager.SharedInstance().EditorDatabaseLoaded == false)
            {
                NWENotificationManager.SharedInstance().AddObserverForAll(this, NWDNotificationConstants.K_LAUNCHER_EDITOR_READY, delegate (NWENotification sNotification)
                {
                    NWENotificationManager.SharedInstance().RemoveObserverEveryWhere(this);
                    NWDUserNetWorking.PrepareUpdate(0, null);
                });
            }
            else
            {
                NWDUserNetWorking.PrepareUpdate(0, null);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        IEnumerator UserNetworkinUpdate()
        {
            while (true)
            {
                if (IsActive == true)
                {
                    //Debug.Log("NWDUserNetWorkingScript UserNetworkinUpdate()");
                    NWDUserNetWorking.NetworkingUpdate();
                }
                yield return new WaitForSeconds(NWDUserNetWorking.DelayInSeconds());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================