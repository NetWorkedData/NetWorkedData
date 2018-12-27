using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NetWorkedData;
using BasicToolBox;
using System;

//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
public class NWDPreferenceToogle : MonoBehaviour
{
    //-------------------------------------------------------------------------------------------------------------
    public NWDPreferenceKeyConnection PreferenceKeyConnection;
    public GameObject ObjectToToogle;
    public MonoBehaviour BehaviourToToogle;
    //-------------------------------------------------------------------------------------------------------------
    void Awake()
    {
        PreferenceApply();
    }
    //-------------------------------------------------------------------------------------------------------------
    void Update()
    {
    }
    //-------------------------------------------------------------------------------------------------------------
    private void OnEnable()
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
    private void PreferenceApply()
    {
        bool tToogle = PreferenceKeyConnection.GetBool();
        if (ObjectToToogle != null)
        {
            ObjectToToogle.SetActive(tToogle);
        };
        if (BehaviourToToogle != null)
        {
            BehaviourToToogle.enabled = tToogle;
        };
    }
    //-------------------------------------------------------------------------------------------------------------
    private void OnDisable()
    {
        BTBNotificationManager.SharedInstance().RemoveObserverEveryWhere(this);
    }
    //-------------------------------------------------------------------------------------------------------------
}
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
