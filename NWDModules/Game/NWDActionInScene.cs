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
        NWDActionConnection ActionReference;
        public UnityEvent ActionTOEvent;
        //-------------------------------------------------------------------------------------------------------------
        // Use this for initialization
        void Start()
        {
            Debug.Log("NWDActionInScene Start()");
            BTBNotificationBlock tListener = delegate (BTBNotification sNotification)
            {
                RunAction();
            };
            NWDAction tAction = ActionReference.GetObject();
            if (tAction!=null)
            {
                BTBNotificationManager.SharedInstance().AddObserver(this, tAction.ActionName, tListener);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void RunAction()
        {
            Debug.Log("NWDActionInScene RunAction()");
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnDestroy()
        {
            Debug.Log("NWDActionInScene OnDestroy()");
            BTBNotificationManager.SharedInstance().RemoveObserverEveryWhere(this);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================