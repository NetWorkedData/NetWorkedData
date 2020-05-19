//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDSplashscreenController : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        float deltaTime = 0.0f;
        //-------------------------------------------------------------------------------------------------------------
        public bool Log = false;
        public bool AutoClickNextButton = true;
        public Button NextButton;
        public Button SyncSomeAccountButton;
        public Button SyncSomeEditorButton;
        public Button SyncAllButton;
        public Text AccountText;
        public Text BenchmarkText;
        public Text BundleVersion;
        public Text EngineVersion;
        public Text FramePerSeconds;

        public TextMeshPro Counter;
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
            if (SyncAllButton != null)
            {
                SyncAllButton.gameObject.SetActive(true);
            }
            if (SyncSomeAccountButton != null)
            {
                SyncSomeAccountButton.gameObject.SetActive(true);
            }
            if (SyncSomeEditorButton != null)
            {
                SyncSomeEditorButton.gameObject.SetActive(true);
            }
            if (BundleVersion != null)
            {
                BundleVersion.text = "App Version " + Application.version;
            }
            if (EngineVersion != null)
            {
                EngineVersion.text = "NeWeeDy " + NWDAppConfiguration.Version;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void Update()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnGUI()
        {
            if (AccountText != null)
            {
                AccountText.text = NWDAccount.CurrentReference();
            }
            if (BenchmarkText != null)
            {
                BenchmarkText.text = NWDEngineBenchmark.GetWatch();
            }

            if (FramePerSeconds != null)
            {
                float msec = deltaTime * 1000.0f;
                float fps = 1.0f / deltaTime;
                FramePerSeconds.text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            }
            if (Counter!=null)
            {
                NWDAccountPreference tAccountPreference = NWDAccountPreference.GetByInternalKeyOrCreate("Test", new NWDMultiType(0));
                if (tAccountPreference!=null)
                {
                Counter.text = tAccountPreference.Value.GetIntValue().ToString();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SyncAll()
        {
            Debug.Log("<color=red>!!!!!</color><color=orange>SyncAll</color>" + " state : " + NWDLauncher.GetState().ToString());
            NWDDataManager.SharedInstance().AddWebRequestAllSynchronization();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SyncSomeAccount()
        {
            Debug.Log("<color=red>!!!!!</color><color=orange>SyncSomeAccount</color>" + " state : " + NWDLauncher.GetState().ToString());
            // get an fictive Preference for this account
            NWDAccountPreference tAccountPreference = NWDAccountPreference.GetByInternalKeyOrCreate("Test", new NWDMultiType(0));
            // increment this value
            tAccountPreference.Value.SetIntValue(tAccountPreference.Value.GetIntValue() + 1);
            // save modification
            tAccountPreference.SaveData();
            // Please sync these classes on cluster 
            NWDDataManager.SharedInstance().AddWebRequestSynchronization(new List<Type>(){
                typeof(NWDAccountInfos),
                typeof(NWDAccountAvatar),
                typeof(NWDAccountNickname),
                typeof(NWDAccountPreference),
            });
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SyncSomeEditor()
        {
            Debug.Log("<color=red>!!!!!</color><color=orange>SyncSomeEditor</color>" + " state : " + NWDLauncher.GetState().ToString());
            NWDDataManager.SharedInstance().AddWebRequestSynchronization(new List<Type>() {
                typeof(NWDItem),
                typeof(NWDCategory),
                typeof(NWDFamily),
                typeof(NWDKeyword),
                typeof(NWDLocalization),
            });
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
