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
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEngine.UI;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDErrorController : NWESingletonUnity<NWDErrorController>
    {
        //-------------------------------------------------------------------------------------------------------------
        public Text Title;
        public Text Description;
        public Text Validation;
        //-------------------------------------------------------------------------------------------------------------
        public Image IconWarning;
        public Image IconAlert;
        //-------------------------------------------------------------------------------------------------------------
        private Animator Anim;
        //-------------------------------------------------------------------------------------------------------------
        private bool ShowPossible = true;
        private List<NWENotification> ErrorList = new List<NWENotification>();
        private NWENotification ActualNotification;
        private NWDUserNotification ActualErrorNotification;
        //-------------------------------------------------------------------------------------------------------------
        public NWDErrorConnection ErroForTest;
        //-------------------------------------------------------------------------------------------------------------
        public void TestError()
        {
            Debug.Log("NWDErrorController ErroForTest()");
            NWDError tError = ErroForTest.GetReachableData();
            if (tError != null)
            {
                tError.ShowAlert("NONE");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Show(NWENotification sNotification)
        {
            Debug.Log("NWDErrorController Show()");
            if (sNotification != null)
            {
                NWDUserNotification tNot = sNotification.Datas as NWDUserNotification;
                if (tNot.Origin() == NWDUserNotificationOrigin.Error)
                {
                    ErrorList.Insert(0, sNotification);
                }
                else
                {
                    ErrorList.Add(sNotification);
                }
            }
            ShowNext();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void ShowNext()
        {
            Debug.Log("NWDErrorController ShowNext()");
            if (ShowPossible == true)
            {
                if (ErrorList.Count > 0)
                {
                    ShowPossible = false;
                    ActualNotification = ErrorList[0];
                    ErrorList.RemoveAt(0);
                    ActualErrorNotification = ActualNotification.Datas as NWDUserNotification;
                    ActualErrorNotification.IsShowing();
                    //Install error
                    if (Title != null)
                    {
                        Title.text = ActualErrorNotification.Title();
                    }
                    if (Description != null)
                    {
                        Description.text = ActualErrorNotification.Description();
                    }
                    if (Validation != null)
                    {
                        Validation.text = ActualErrorNotification.TextValidate();
                    }
                    bool tShow = true;
                    switch (ActualErrorNotification.Origin())
                    {
                        case NWDUserNotificationOrigin.Error:
                            {
                                switch (ActualErrorNotification.ErrorType())
                                {
                                    case NWDErrorType.Alert:
                                        {

                                        }
                                        break;
                                    case NWDErrorType.Critical:
                                        {

                                        }
                                        break;
                                    case NWDErrorType.Upgrade:
                                        {

                                        }
                                        break;
                                    case NWDErrorType.InGame:
                                        {

                                        }
                                        break;
                                    case NWDErrorType.Ignore:
                                        {
                                            // not a error ... perhaps a message
                                        }
                                        break;
                                    case NWDErrorType.UnityEditor:
                                    case NWDErrorType.LogWarning:
                                    case NWDErrorType.LogVerbose:
                                        {
                                            tShow = false;
                                        }
                                        break;
                                }
                            }
                            break;
                        case NWDUserNotificationOrigin.Message:
                        case NWDUserNotificationOrigin.InterMessage:
                            {
                                if (ActualErrorNotification.MessageStyle() == NWDMessageStyle.kNotification)
                                {
                                    if (Validation != null)
                                    {
                                        Validation.text = "......";
                                    }
                                    Anim.SetTrigger("Close");
                                }
                                else
                                {
                                }
                            }
                            break;
                    }
                    if (tShow == true)
                    {
                        if (Anim != null)
                        {
                            Anim.SetTrigger("Open");
                        }
                    }
                    else
                    {
                        // don't show pass to next error
                        CloseIsFinish();
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Close()
        {
            Debug.Log("NWDErrorController Close()");
            if (Anim != null)
            {
                Anim.SetTrigger("Close");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CloseIsFinish()
        {
            Debug.Log("NWDErrorController CloseIsFinish()");
            ShowPossible = true;
            switch (ActualErrorNotification.ErrorType())
            {
                case NWDErrorType.Alert:
                case NWDErrorType.InGame:
                case NWDErrorType.Ignore:
                case NWDErrorType.UnityEditor:
                case NWDErrorType.LogWarning:
                case NWDErrorType.LogVerbose:
                    {
                        ShowNext();
                    }
                    break;

                case NWDErrorType.Critical:
                case NWDErrorType.Upgrade:
                    {
                        // error manage the quit operation
                    }
                    break;
            }
            ActualErrorNotification.Complete();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override NWESingletonRoot DestroyRoot()
        {
            return NWESingletonRoot.GameObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void InitInstance()
        {
            Anim = GetComponent<Animator>();
        }
        //-------------------------------------------------------------------------------------------------------------
        protected void InstallObserver()
        {
            Debug.Log("NWDErrorController InstallObserver()");
            NWENotificationManager tNotifManager = NWENotificationManager.SharedInstance();
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_ERROR, delegate (NWENotification sNotification)
            {
                Show(sNotification);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_MESSAGE, delegate (NWENotification sNotification)
            {
                Show(sNotification);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_NEWS_NOTIFICATION, delegate (NWENotification sNotification)
            {
                Show(sNotification);
            });
        }
        //-------------------------------------------------------------------------------------------------------------
        protected void RemoveObserver()
        {
            Debug.Log("NWDErrorController RemoveObserver()");
            NWENotificationManager tNotifManager = NWENotificationManager.SharedInstance();
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_ERROR);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_MESSAGE);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NEWS_NOTIFICATION);
        }
        //-------------------------------------------------------------------------------------------------------------
        protected void OnEnable()
        {
            Debug.Log("NWDErrorController OnEnable()");
            InstallObserver();
        }
        //-------------------------------------------------------------------------------------------------------------
        protected void OnDisable()
        {
            Debug.Log("NWDErrorController OnDisable()");
            RemoveObserver();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
