//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:26
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
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
            if (NWDDataManager.SharedInstance().DataEditorLoaded == false)
            {
                NWENotificationManager.SharedInstance().AddObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_LOADED, delegate (NWENotification sNotification)
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