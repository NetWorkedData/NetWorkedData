//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:45:48
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
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
        private NWDErrorNotification ActualErrorNotification;
        //-------------------------------------------------------------------------------------------------------------
        public NWDErrorConnection ErroForTest;
        //-------------------------------------------------------------------------------------------------------------
        public void TestError()
        {
            Debug.Log("NWDErrorController ErroForTest()");
            NWDError tError = ErroForTest.GetData();
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
                ErrorList.Add(sNotification);
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
                    ActualErrorNotification = ActualNotification.Datas as NWDErrorNotification;
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
                        Validation.text = ActualErrorNotification.Validation();
                    }
                    bool tShow = true;
                    switch (ActualErrorNotification.Type())
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
                        case NWDErrorType.UnityEditor:
                        case NWDErrorType.LogWarning:
                        case NWDErrorType.LogVerbose:
                            {
                                tShow = false;
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
            switch (ActualErrorNotification.Type())
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
                        // Stop application!
                        Application.Quit(9);
                    }
                    break;
            }
            ActualErrorNotification.Close();
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
        }
        //-------------------------------------------------------------------------------------------------------------
        protected void RemoveObserver()
        {
            Debug.Log("NWDErrorController RemoveObserver()");
            NWENotificationManager tNotifManager = NWENotificationManager.SharedInstance();
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_ERROR);
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
