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
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================

using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using static UnityEngine.Networking.UnityWebRequest;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDTestController : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        float deltaTime = 0.0f;
        //-------------------------------------------------------------------------------------------------------------
        public bool Log = false;
        public bool AutoClickNextButton = true;

        public Text BundleVersion;
        public Text EngineVersion;
        public Text FramePerSeconds;

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
                UnityEngine.Debug.Log("<color=red>!!!!!</color><color=orange>Start</color>" + " state : " + NWDLauncher.GetState().ToString());
            }
            if (BundleVersion != null)
            {
                BundleVersion.text = "App Version " + Application.version;
            }
            if (EngineVersion != null)
            {
                EngineVersion.text = "NeWeeDy " + NWDEngineVersion.Version;
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
                    BenchmarkText.text = NWDLauncherChronometer.GetWatch() + " " + LastSyncResultLog;
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
                    LastSyncResultLog = "\n<b>" + sTag + " </b>: " + LastInfos.Benchmark.GetLogString(LastInfos.perform) + ", rows pushed " + LastInfos.RowPushCounter + ", rows pulled " + LastInfos.RowPullCounter;
                }
                else
                {
                    LastSyncResultLog = "\n<b>" + sTag + " </b>: " + ": type error for infos";
                }
            }
            else
            {
                LastSyncResultLog = "\n<b>" + sTag + " </b>: " + ": null infos";
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
                    //UnityEngine.Debug.Log("AlertTest() close with NWDMessageState.OK");
                }
                else
                {
                    //UnityEngine.Debug.LogWarning("AlertTest() close with BAD NWDMessageState ");
                }
            });
        }
        //-------------------------------------------------------------------------------------------------------------
        public void FakeToken()
        {
            NWDDialogNative.Dialog(NWDMessage.GetByReference("FakeToken"), delegate (NWDMessageState sState)
            {
                if (sState == NWDMessageState.OK)
                {
                    //UnityEngine.Debug.Log("FakeToken() close with NWDMessageState.OK");
                    NWDAppConfiguration.SharedInstance().SelectedEnvironment().RequesToken = "Fakeee" + NWDToolbox.RandomStringUnix(24);
                }
                else if (sState == NWDMessageState.NOK)
                {
                    //UnityEngine.Debug.Log("FakeToken() close with NWDMessageState.NOK");
                }
                else
                {
                    //UnityEngine.Debug.LogWarning("FakeToken() close with BAD NWDMessageState");
                }

            });
        }
        //-------------------------------------------------------------------------------------------------------------
        public void FakeAccount()
        {
            NWDDialogNative.Dialog(NWDMessage.GetByReference("FakeAccount"), delegate (NWDMessageState sState)
            {
                if (sState == NWDMessageState.OK)
                {
                    //UnityEngine.Debug.Log("FakeAccount() close with NWDMessageState.OK");
                    NWDAppConfiguration.SharedInstance().SelectedEnvironment().SetAccountReference(NWDAccount.K_ACCOUNT_PREFIX_TRIGRAM + "-1234-" + NWDToolbox.RandomStringNumeric(8) + "-" + NWDToolbox.RandomStringNumeric(6) + "C");
                }
                else if (sState == NWDMessageState.NOK)
                {
                    //UnityEngine.Debug.Log("FakeAccount() close with NWDMessageState.NOK");
                }
                else
                {
                    //UnityEngine.Debug.LogWarning("FakeAccount() close with BAD NWDMessageState");
                }

            });
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetSession()
        {
            NWDAppConfiguration.SharedInstance().SelectedEnvironment().ResetPreferences();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void BlankTest()
        {
            //UnityEngine.Debug.Log("<color=red>!!!!!</color><color=orange>Blank Test</color>" + " state : " + NWDLauncher.GetState().ToString());
            NWDDataManager.SharedInstance().AddWebRequestBlankWithBlock(
                delegate (NWEOperation sOperation, float sProgress, NWEOperationResult sInfos)
                {
                    NWDOperationWebBlank tRequest = sOperation as NWDOperationWebBlank;
                    WebLog("Blank Test " + tRequest.Request.url, sInfos);
                },
            delegate (NWEOperation sOperation, float sProgress, NWEOperationResult sInfos)
            {
                NWDOperationResult tInfos = sInfos as NWDOperationResult;
                NWDAlert.Alert(tInfos.errorDesc);
            });
        }
        //-------------------------------------------------------------------------------------------------------------
        public void BlankTestSecond()
        {
            StartCoroutine(Upload());

        }
        IEnumerator Upload()
        {
            Stopwatch Watch = new Stopwatch();
            Watch.Start();
            WWWForm form = new WWWForm();
            form.AddField("myField", "myData");
            string tURLBlank = NWDAppConfiguration.SharedInstance().SelectedEnvironment().GetConfigurationServerHTTPS() + "/"
                + NWDAppConfiguration.SharedInstance().WebServiceFolder() + "/"
                + NWDAppConfiguration.SharedInstance().SelectedEnvironment().Environment + "/" + NWD.K_BLANK_PHP;
            //using (UnityWebRequest www = UnityWebRequest.Post("https://simple.net-worked-data.ovh/NWDSample_0024/Dev/blank.php", form))
            using (UnityWebRequest www = UnityWebRequest.Post(tURLBlank, form))
            {
                yield return www.SendWebRequest();

                if (www.result == Result.ConnectionError || www.result == Result.ProtocolError)
                //if (www.isNetworkError || www.isHttpError)
                {
                    UnityEngine.Debug.Log(www.error);
                    double tResult = (double)(Watch.ElapsedMilliseconds / 1000.0F);
                    LastSyncResultLog = "\n<b> blank test " + tURLBlank + " second ERROR in </b> = " + tResult.ToString("#0.000") + "s /";
                    Watch.Stop();
                }
                else
                {
                    double tResult = (double)(Watch.ElapsedMilliseconds / 1000.0F);
                    LastSyncResultLog = "\n<b> blank test " + tURLBlank + " second </b> = " + tResult.ToString("#0.000") + "s";
                    Watch.Stop();
                    //UnityEngine.Debug.Log("Second upload complete "+tResult.ToString("#0.000")+"s !");
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void BlankTestThird()
        {
            Stopwatch Watch = new Stopwatch();
            Watch.Start();
            WWWForm form = new WWWForm();
            form.AddField("myField", "myData");
            string tURLBlank = NWDAppConfiguration.SharedInstance().SelectedEnvironment().GetConfigurationServerHTTPS() + "/"
                + NWDAppConfiguration.SharedInstance().WebServiceFolder() + "/"
                + NWDAppConfiguration.SharedInstance().SelectedEnvironment().Environment + "/" + NWD.K_BLANK_PHP;
            using (UnityWebRequest www = UnityWebRequest.Post(tURLBlank, form))
            {
                www.SendWebRequest();

                while (www.isDone == false)
                {
                }

                if (www.result == Result.ConnectionError || www.result == Result.ProtocolError)
                //if (www.isNetworkError || www.isHttpError)
                {
                    UnityEngine.Debug.Log(www.error);
                    double tResult = (double)(Watch.ElapsedMilliseconds / 1000.0F);
                    LastSyncResultLog = "\n<b> blank test " + tURLBlank + "third ERROR in </b> = " + tResult.ToString("#0.000") + "s /";
                    Watch.Stop();
                }
                else
                {
                    double tResult = (double)(Watch.ElapsedMilliseconds / 1000.0F);
                    LastSyncResultLog = "\n<b> blank test " + tURLBlank + "third </b> = " + tResult.ToString("#0.000") + "s";
                    Watch.Stop();
                    //UnityEngine.Debug.Log("Third upload complete " + tResult.ToString("#0.000") + "s !");
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SyncAll()
        {
            //UnityEngine.Debug.Log("<color=red>!!!!!</color><color=orange>SyncAll</color>" + " state : " + NWDLauncher.GetState().ToString());
            NWDDataManager.SharedInstance().AddWebRequestAllSynchronizationWithBlock(
                delegate (NWEOperation sOperation, float sProgress, NWEOperationResult sInfos)
                {
                    NWDOperationWebSynchronisation tRequest = sOperation as NWDOperationWebSynchronisation;
                    WebLog("Sync All " + tRequest.Request.url, sInfos);
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
            //UnityEngine.Debug.Log("<color=red>!!!!!</color><color=orange>SyncCounterAccount</color>" + " state : " + NWDLauncher.GetState().ToString());
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
                NWDOperationWebSynchronisation tRequest = sOperation as NWDOperationWebSynchronisation;
                WebLog("Sync Counter Account " + tRequest.Request.url, sInfos);
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
            //UnityEngine.Debug.Log("<color=red>!!!!!</color><color=orange>SyncSomeAccount</color>" + " state : " + NWDLauncher.GetState().ToString());
            // get an fictive Preference for this account
            NWDAccountPreference tAccountPreference = NWDAccountPreference.GetByInternalKeyOrCreate("Test", new NWDMultiType(0));
            // increment this value
            tAccountPreference.Value.SetIntValue(tAccountPreference.Value.GetIntValue() + 1);
            // save modification
            tAccountPreference.SaveData();
            // Please sync these classes on cluster 
            NWDDataManager.SharedInstance().AddWebRequestSynchronizationWithBlock(new List<Type>(){
                typeof(NWDAccountInfos),
#if NWD_ACCOUNT_IDENTITY
                typeof(NWDAccountAvatar),
                typeof(NWDAccountNickname),
#endif
                typeof(NWDAccountPreference),
            },
            delegate (NWEOperation sOperation, float sProgress, NWEOperationResult sInfos)
            {
                NWDOperationWebSynchronisation tRequest = sOperation as NWDOperationWebSynchronisation;
                WebLog("Sync Some Account " + tRequest.Request.url, sInfos);
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
            //UnityEngine.Debug.Log("<color=red>!!!!!</color><color=orange>SyncSomeEditor</color>" + " state : " + NWDLauncher.GetState().ToString());
            NWDDataManager.SharedInstance().AddWebRequestSynchronizationWithBlock(new List<Type>() {
                typeof(NWDItem),
#if NWD_CLASSIFICATION
                typeof(NWDCategory),
                typeof(NWDFamily),
                typeof(NWDKeyword),
#endif
                typeof(NWDLocalization),
            },
            delegate (NWEOperation sOperation, float sProgress, NWEOperationResult sInfos)
            {
                NWDOperationWebSynchronisation tRequest = sOperation as NWDOperationWebSynchronisation;
                WebLog("Sync Some Editor " + tRequest.Request.url, sInfos);
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
