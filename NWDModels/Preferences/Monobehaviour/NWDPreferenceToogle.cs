//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:50:21
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

//=====================================================================================================================
//
// ideMobi copyright 2018
// All rights reserved by ideMobi
//
//=====================================================================================================================

using UnityEngine;
//using BasicToolBox;

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
        [Header ("Activation with preference boolean")]
        public GameObject[] ObjectsActivationSync;
        public GameObject[] ObjectsActivationInversed;
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            PreferenceApply();
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnEnable()
        {
            PreferenceApply();
            NWENotificationManager.SharedInstance().AddObserverForAll(this, NWDPreferenceKey.K_PREFERENCE_CHANGED_KEY, delegate (NWENotification sNotification)
            {
                if (sNotification.Sender == PreferenceKeyConnection.GetReachableData())
                {
                    PreferenceApply();
                }
            });
        }
        //-------------------------------------------------------------------------------------------------------------
        void PreferenceApply()
        {
            bool tToogle = PreferenceKeyConnection.GetBool();
            foreach (GameObject tG in ObjectsActivationSync)
            {
                if (tG != null)
                {
                    tG.SetActive(tToogle);
                }
            }
            foreach (GameObject tG in ObjectsActivationInversed)
            {
                if (tG != null)
                {
                    tG.SetActive(!tToogle);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnDisable()
        {
            NWENotificationManager.SharedInstance().RemoveObserverEveryWhere(this);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================