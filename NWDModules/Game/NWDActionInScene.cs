using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BasicToolBox;
using UnityEngine.Events;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDActionInScene : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDActionConnection ActionReference;
        public UnityEvent ActionToSceneEvent;
        //-------------------------------------------------------------------------------------------------------------
        // Use this for initialization
        void Start()
        {
            Debug.Log("NWDActionInScene Start()");
            BTBNotificationBlock tListener = delegate (BTBNotification sNotification)
            {
                ReceiptNotification((NWDAction)sNotification.Sender);
            };
            NWDAction tAction = ActionReference.GetObject();
            if (tAction!=null)
            {
                tAction.TrackBy(this, tListener);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PostNotification() // use in demo
        {
            Debug.Log("NWDActionInScene PostNotification()");
            NWDAction tAction = ActionReference.GetObject();
            if (tAction != null)
            {
                tAction.PostNotification();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void ReceiptNotification(NWDAction sAction)
        {
            Debug.Log("NWDActionInScene ReceiptNotification()");
            if (ActionToSceneEvent != null)
            {
                ActionToSceneEvent.Invoke();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DoSceneAction() // use in demo
        {
            Debug.Log("NWDActionInScene DoSceneAction()");
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnDestroy()
        {
            Debug.Log("NWDActionInScene OnDestroy()");
            NWDAction tAction = ActionReference.GetObject();
            if (tAction != null)
            {
                tAction.UnTrackBy(this);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================