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
    public delegate void NWDPinCodeDelegate(string sCodePin);
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDPinCodeController : NWESingletonUnity<NWDPinCodeController>
    {
        //-------------------------------------------------------------------------------------------------------------
        public GameObject PinPanel;
        public int RandomButton = 9;
        public int PinLenght = 4;
        public NWDPinCodeDelegate PinCodeDelegate;
        public string PinCode = null;
        public bool CodeField = true;
        //-------------------------------------------------------------------------------------------------------------
        public void ButtonReset(Button sButton)
        {
            Debug.Log("ButtonReset()");
            PinCode = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ButtonCallback(Button sButton)
        {
            Debug.Log("ButtonCallback()");
            if (CodeField == true)
            {
                PinCode = PinCode + sButton.GetComponentInChildren<Text>().text;
                Debug.Log("ButtonCallback() PinCode = " + PinCode);
                if (PinCode.Length == PinLenght)
                {
                    CodeField = false;
                    Close();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public Text Title;
        public Text Description;
        //-------------------------------------------------------------------------------------------------------------
        private Animator Anim;
        //-------------------------------------------------------------------------------------------------------------
        private bool ShowPossible = true;
        //-------------------------------------------------------------------------------------------------------------
        public void Show(NWDPinCodeDelegate sDelegate)
        {
            if (PinPanel != null)
            {
                if (PinPanel.transform.childCount > 3)
                {
                    for (int tR = 0; tR < RandomButton; tR++)
                    {
                        int tIndex = Random.Range(0, PinPanel.transform.childCount);
                        PinPanel.transform.GetChild(tIndex).SetAsFirstSibling();
                        //PinPanel.transform.ch
                    }
                }
            }
            PinCodeDelegate = sDelegate;
            PinCode = string.Empty;
            if (ShowPossible == true)
            {
                ShowPossible = false;
                if (Anim != null)
                {
                    Anim.SetTrigger("Open");
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void Close()
        {
            Debug.Log("NWDPinCodeController Close()");
            if (Anim != null)
            {
                Anim.SetTrigger("Close");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CloseIsFinish()
        {
            Debug.Log("NWDPinCodeController CloseIsFinish()");
            ShowPossible = true;
            CodeField = true;
            PinCodeDelegate(PinCode);
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
            Debug.Log("NWDPinCodeController InstallObserver()");
            NWENotificationManager tNotifManager = NWENotificationManager.SharedInstance();
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_REQUEST, delegate (NWENotification sNotification)
            {
                Show(delegate (string sCodePin)
                {
                    NWDGameDataManager.UnitySingleton().PinCodeInsert(sCodePin, sCodePin);
                });

            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_NEEDED, delegate (NWENotification sNotification)
            {
                Show(delegate (string sCodePin)
                {
                    NWDGameDataManager.UnitySingleton().PinCodeInsert(sCodePin, sCodePin);
                });

            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_FAIL, delegate (NWENotification sNotification)
            {
                Show(delegate (string sCodePin)
                {
                    NWDGameDataManager.UnitySingleton().PinCodeInsert(sCodePin, sCodePin);
                });

            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_STOP, delegate (NWENotification sNotification)
            {

            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_SUCCESS, delegate (NWENotification sNotification)
            {

            });
        }
        //-------------------------------------------------------------------------------------------------------------
        protected void RemoveObserver()
        {
            Debug.Log("NWDPinCodeController RemoveObserver()");
            NWENotificationManager tNotifManager = NWENotificationManager.SharedInstance();
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_REQUEST);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_NEEDED);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_FAIL);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_STOP);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_SUCCESS);
        }
        //-------------------------------------------------------------------------------------------------------------
        protected void OnEnable()
        {
            Debug.Log("NWDPinCodeController OnEnable()");
            CodeField = true;
            InstallObserver();
        }
        //-------------------------------------------------------------------------------------------------------------
        protected void OnDisable()
        {
            Debug.Log("NWDPinCodeController OnDisable()");
            CodeField = false;
            RemoveObserver();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
