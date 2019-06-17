//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:34:6
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
using BasicToolBox;
using UnityEngine.Events;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDActionInScene :  MonoBehaviour// NWDCallBack
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDActionConnection ActionReference;
        public UnityEvent ActionToSceneEvent;
        private bool ActionIsInstalled = false;
        //-------------------------------------------------------------------------------------------------------------
        public void PostNotification() // use in demo
        {
            //Debug.Log("NWDActionInScene PostNotification()");
            NWDAction tAction = ActionReference.GetData();
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
                if (NWDDataManager.SharedInstance().DataEditorLoaded == true)
                {
                    BTBNotificationBlock tListener = delegate (BTBNotification sNotification)
                    {
                        ReceiptNotification((NWDAction)sNotification.Sender);
                    };
                    NWDAction tAction = ActionReference.GetData();
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
            NWDAction tAction = ActionReference.GetData();
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
        /*protected override*/ void OnEnable()
        {
            //Debug.Log("NWDActionInScene OnEnable()");
            /*base.OnEnable();*/ // onstall notifi from other 
            InstallAction();
        }
        //-------------------------------------------------------------------------------------------------------------
        public /*override*/ void NotificationDatasLoaded(BTBNotification sNotification, bool sPreloadDatas)
        {
            //Debug.Log("NWDActionInScene NotificationDatasLoaded()");
            // create your method by override
            InstallAction();
        }
        //-------------------------------------------------------------------------------------------------------------
        protected /*override*/ void OnDisable()
        {
            //Debug.Log("NWDActionInScene OnDisable()");
            RemoveAction();
            //base.OnDisable();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================