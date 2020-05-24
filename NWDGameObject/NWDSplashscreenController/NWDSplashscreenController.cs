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

        public Text BundleVersion;
        public Text EngineVersion;
        public Text FramePerSeconds;

        public Button NextButton;
        public Button SyncCounterButton;
        public Button SyncSomeAccountButton;
        public Button SyncSomeEditorButton;
        public Button SyncAllButton;

        public Text BenchmarkText;
        public Text AccountText;
        public Text RequestTokenText;

        public TextMeshPro[] CountersArray;
        public Text SynchroTest;
        public NWDLocalizationConnection SynchroTextTest;
        //-------------------------------------------------------------------------------------------------------------
        string LastSyncResultLog = string.Empty;
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
            if (NWDLauncher.GetState() > NWDStatut.ClassRestaureFinish)
            {
                if (AccountText != null)
                {
                    AccountText.text = NWDAccount.CurrentReference();
                }
                if (BenchmarkText != null)
                {
                    BenchmarkText.text = NWDEngineBenchmark.GetWatch() + " " + LastSyncResultLog;
                }

                if (FramePerSeconds != null)
                {
                    float msec = deltaTime * 1000.0f;
                    float fps = 1.0f / deltaTime;
                    FramePerSeconds.text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
                }
                foreach (TextMeshPro Counter in CountersArray)
                {
                    if (Counter != null)
                    {
                        NWDAccountPreference tAccountPreference = NWDAccountPreference.GetByInternalKeyOrCreate("Test", new NWDMultiType(0));
                        if (tAccountPreference != null)
                        {
                            Counter.text = tAccountPreference.Value.GetIntValue().ToString();
                        }
                    }
                }
                if (RequestTokenText != null)
                {
                    string tRequestToken = NWDAppConfiguration.SharedInstance().SelectedEnvironment().RequesToken;
                    if (string.IsNullOrEmpty(tRequestToken))
                    {
                        RequestTokenText.text = "--- empty request token ---";
                    }
                    else
                    {
                        RequestTokenText.text = NWDAppConfiguration.SharedInstance().SelectedEnvironment().RequesToken;
                    }
                }
                if (SynchroTextTest != null && SynchroTest != null)
                {
                    SynchroTest.text = SynchroTextTest.GetLocalString("error ?!");
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void WebLog(string sTag, NWEOperationResult sInfos)
        {
            if (sInfos != null)
            {
                if (sInfos.GetType() == typeof(NWDOperationResult))
                {
                    NWDOperationResult LastInfos = sInfos as NWDOperationResult;
                    double tDurationNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.FinishDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.PrepareDateTime)) / 1000.0F;
                    double tPrepareNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.WebDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.PrepareDateTime)) / 1000.0F;
                    double tUploadNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.UploadedDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.WebDateTime)) / 1000.0F;
                    double tDowloadNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.DownloadedDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.UploadedDateTime)) / 1000.0F;
                    double tComputeNetMilliseconds = (NWDToolbox.TimestampMilliseconds(LastInfos.FinishDateTime) - NWDToolbox.TimestampMilliseconds(LastInfos.DownloadedDateTime)) / 1000.0F;
                    LastSyncResultLog = "\n<b>" + sTag + "</b>:" +
                        "duration : " + tDurationNetMilliseconds.ToString("#0.000") + "s " +
                        "(network = " + (tUploadNetMilliseconds + tDowloadNetMilliseconds).ToString("#0.000") + " s) " +
                        "row pushed " + LastInfos.RowPushCounter.ToString() + "," +
                        "row pulled " + LastInfos.RowPullCounter.ToString() + "";
                }
                else
                {
                    LastSyncResultLog = "\n<b>" + sTag + "</b>:" + ": type error for infos";
                }
            }
            else
            {
                LastSyncResultLog = "\n<b>" + sTag + "</b>:" + ": null infos";
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ChangeLanguage(string sCode)
        {
            NWDDataManager.SharedInstance().AccountLanguageSave(sCode);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AlertTest()
        {
            NWDAlert.Alert(NWDError.GetErrorDomainCode("TEST", "001"), delegate (NWDMessageState sState)
            {
                if (sState == NWDMessageState.OK)
                {
                    Debug.Log("AlertTest() close with NWDMessageState.OK");
                }
                else
                {
                    Debug.LogWarning("AlertTest() close with BAD NWDMessageState ");
                }
            });
        }
        //-------------------------------------------------------------------------------------------------------------
        public void FakeToken()
        {
            NWDDialog.Dialog(NWDMessage.GetByReference("FakeToken"), delegate (NWDMessageState sState)
            {
                if (sState == NWDMessageState.OK)
                {
                    Debug.Log("FakeToken() close with NWDMessageState.OK");
                    NWDAppConfiguration.SharedInstance().SelectedEnvironment().RequesToken = "Fakeee" + NWDToolbox.RandomStringUnix(24);
                }
                else if (sState == NWDMessageState.NOK)
                {
                    Debug.Log("FakeToken() close with NWDMessageState.NOK");
                }
                else
                {
                    Debug.LogWarning("FakeToken() close with BAD NWDMessageState");
                }

            });
        }
        //-------------------------------------------------------------------------------------------------------------
        public void FakeAccount()
        {
            NWDDialog.Dialog(NWDMessage.GetByReference("FakeAccount"), delegate (NWDMessageState sState)
            {
                if (sState == NWDMessageState.OK)
                {
                    Debug.Log("FakeAccount() close with NWDMessageState.OK");
                    NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference = NWDAccount.K_ACCOUNT_PREFIX_TRIGRAM + "-1234-" + NWDToolbox.RandomStringNumeric(8) + "-" + NWDToolbox.RandomStringNumeric(6) + "C";
                }
                else if (sState == NWDMessageState.NOK)
                {
                    Debug.Log("FakeAccount() close with NWDMessageState.NOK");
                }
                else
                {
                    Debug.LogWarning("FakeAccount() close with BAD NWDMessageState");
                }

            });
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetSession()
        {
            NWDAppConfiguration.SharedInstance().SelectedEnvironment().ResetPreferences();
        }
            //-------------------------------------------------------------------------------------------------------------
            public void SyncAll()
        {
            Debug.Log("<color=red>!!!!!</color><color=orange>SyncAll</color>" + " state : " + NWDLauncher.GetState().ToString());
            NWDDataManager.SharedInstance().AddWebRequestAllSynchronizationWithBlock(
                delegate (NWEOperation sOperation, float sProgress, NWEOperationResult sInfos)
            {
                WebLog("Sync All", sInfos);
            },
            delegate (NWEOperation sOperation, float sProgress, NWEOperationResult sInfos)
            {
                NWDOperationResult tInfos = sInfos as NWDOperationResult;
                NWDAlert.Alert(tInfos.errorDesc);
            });
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SyncCounterAccount()
        {
            Debug.Log("<color=red>!!!!!</color><color=orange>SyncCounterAccount</color>" + " state : " + NWDLauncher.GetState().ToString());
            // get an fictive Preference for this account
            NWDAccountPreference tAccountPreference = NWDAccountPreference.GetByInternalKeyOrCreate("Test", new NWDMultiType(0));
            // increment this value
            tAccountPreference.Value.SetIntValue(tAccountPreference.Value.GetIntValue() + 1);
            // save modification
            tAccountPreference.SaveData();
            // Please sync these classes on cluster 
            NWDDataManager.SharedInstance().AddWebRequestSynchronizationWithBlock(new List<Type>(){
                typeof(NWDAccountPreference),
            },
            delegate (NWEOperation sOperation, float sProgress, NWEOperationResult sInfos)
            {
                WebLog("Sync Counter Account", sInfos);
            },
            delegate (NWEOperation sOperation, float sProgress, NWEOperationResult sInfos)
            {
                NWDOperationResult tInfos = sInfos as NWDOperationResult;
                NWDAlert.Alert(tInfos.errorDesc);
            }
            );
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
            NWDDataManager.SharedInstance().AddWebRequestSynchronizationWithBlock(new List<Type>(){
                typeof(NWDAccountInfos),
                typeof(NWDAccountAvatar),
                typeof(NWDAccountNickname),
                typeof(NWDAccountPreference),
            },
            delegate (NWEOperation sOperation, float sProgress, NWEOperationResult sInfos)
            {
                WebLog("Sync Some Account", sInfos);
            },
            delegate (NWEOperation sOperation, float sProgress, NWEOperationResult sInfos)
            {
                NWDOperationResult tInfos = sInfos as NWDOperationResult;
                NWDAlert.Alert(tInfos.errorDesc);
            }
            );
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SyncSomeEditor()
        {
            Debug.Log("<color=red>!!!!!</color><color=orange>SyncSomeEditor</color>" + " state : " + NWDLauncher.GetState().ToString());
            NWDDataManager.SharedInstance().AddWebRequestSynchronizationWithBlock(new List<Type>() {
                typeof(NWDItem),
                typeof(NWDCategory),
                typeof(NWDFamily),
                typeof(NWDKeyword),
                typeof(NWDLocalization),
            },
            delegate (NWEOperation sOperation, float sProgress, NWEOperationResult sInfos)
            {
                WebLog("Sync Some Editor", sInfos);
            },
            delegate (NWEOperation sOperation, float sProgress, NWEOperationResult sInfos)
            {
                NWDOperationResult tInfos = sInfos as NWDOperationResult;
                NWDAlert.Alert(tInfos.errorDesc);
            }
            );
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
