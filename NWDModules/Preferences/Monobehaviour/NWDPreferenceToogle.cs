//=====================================================================================================================
//
// ideMobi copyright 2018
// All rights reserved by ideMobi
//
//=====================================================================================================================

using UnityEngine;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// </summary>
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDPreferenceToogle : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDPreferenceKeyConnection PreferenceKeyConnection;
        public GameObject ObjectToToogle;
        public MonoBehaviour BehaviourToToogle;
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            PreferenceApply();
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnEnable()
        {
            BTBNotificationManager.SharedInstance().AddObserverForAll(this, NWDPreferenceKey.K_PREFERENCE_CHANGED_KEY, delegate (BTBNotification sNotification)
            {
                if (sNotification.Sender == PreferenceKeyConnection.GetObject())
                {
                    PreferenceApply();
                }
            });
        }
        //-------------------------------------------------------------------------------------------------------------
        void PreferenceApply()
        {
            bool tToogle = PreferenceKeyConnection.GetBool();
            if (ObjectToToogle != null)
            {
                ObjectToToogle.SetActive(tToogle);
            }
            if (BehaviourToToogle != null)
            {
                BehaviourToToogle.enabled = tToogle;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnDisable()
        {
            BTBNotificationManager.SharedInstance().RemoveObserverEveryWhere(this);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================