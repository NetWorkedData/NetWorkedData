//=====================================================================================================================
//
//  ideMobi 2020©
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDSplashscreenController : MonoBehaviour 
    {
        //-------------------------------------------------------------------------------------------------------------
        public bool Log = false;
        public bool AutoClickNextButton = true;
        public Button NextButton;
        public Button SyncButton;
        //-------------------------------------------------------------------------------------------------------------
        private void Start()
        {
            if (Log == true)
            {
                Debug.Log("<color=red>!!!!!</color><color=orange>Start</color>" + " state : " + NWDLauncher.GetState().ToString());
            }
            if (NextButton != null)
            {
                NextButton.gameObject.SetActive(true);
                if (AutoClickNextButton == true)
                {
                    NextButton.onClick.Invoke();
                }
            }
            if (SyncButton != null)
            {
                SyncButton.gameObject.SetActive(true);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SyncAll()
        {
            Debug.Log("<color=red>!!!!!</color><color=orange>SyncAll</color>" + " state : " + NWDLauncher.GetState().ToString());
            NWDDataManager.SharedInstance().AddWebRequestAllSynchronization();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
