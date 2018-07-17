//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicToolBox;
using UnityEngine.Events;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDActionInScene : NWDCallBack
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDActionConnection ActionReference;
        public UnityEvent ActionToSceneEvent;
        private bool ActionIsInstalled = false;
        //-------------------------------------------------------------------------------------------------------------
        public void PostNotification() // use in demo
        {
            //Debug.Log("NWDActionInScene PostNotification()");
            NWDAction tAction = ActionReference.GetObject();
            if (tAction != null)
            {
                tAction.PostNotification();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void ReceiptNotification(NWDAction sAction)
        {
            //Debug.Log("NWDActionInScene ReceiptNotification()");
            if (ActionToSceneEvent != null)
            {
                ActionToSceneEvent.Invoke();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void InstallAction()
        {
            //Debug.Log("NWDActionInScene InstallAction()");
            if (ActionIsInstalled == false)
            {
                if (NWDTypeLauncher.DataLoaded == true)
                {
                    BTBNotificationBlock tListener = delegate (BTBNotification sNotification)
                    {
                        ReceiptNotification((NWDAction)sNotification.Sender);
                    };
                    NWDAction tAction = ActionReference.GetObject();
                    if (tAction != null)
                    {
                        tAction.TrackBy(this, tListener);
                        ActionIsInstalled = true;
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void RemoveAction()
        {
            //Debug.Log("NWDActionInScene RemoveAction()");
            NWDAction tAction = ActionReference.GetObject();
            if (tAction != null)
            {
                tAction.UnTrackBy(this);
                ActionIsInstalled = false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DoSceneAction() // use in demo
        {
            //Debug.Log("NWDActionInScene DoSceneAction()");
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnDestroy()
        {
            //Debug.Log("NWDActionInScene OnDestroy()");
        }
        //-------------------------------------------------------------------------------------------------------------
        // Use this for initialization
        protected override void OnEnable()
        {
            //Debug.Log("NWDActionInScene OnEnable()");
            base.OnEnable(); // onstall notifi from other 
            InstallAction();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDatasLoaded(BTBNotification sNotification, bool sPreloadDatas)
        {
            //Debug.Log("NWDActionInScene NotificationDatasLoaded()");
            // create your method by override
            InstallAction();
        }
        //-------------------------------------------------------------------------------------------------------------
        protected override void OnDisable()
        {
            //Debug.Log("NWDActionInScene OnDisable()");
            RemoveAction();
            base.OnDisable();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================