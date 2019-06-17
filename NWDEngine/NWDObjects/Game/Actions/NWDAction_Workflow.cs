//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:34:10
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================



using System;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAction : NWDBasis<NWDAction>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDAction()
        {
            //Debug.Log("NWDAction Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAction(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDAction Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exampel of implement for instance method.
        /// </summary>
        public void PostNotification()
        {
            //BTBNotificationManager.SharedInstance().PostNotification(this, ActionName);
            BTBNotificationManager.SharedInstance().PostNotification(this, Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void TrackBy(object sObserver, BTBNotificationBlock sBlockToUse)
        {
            BTBNotificationManager.SharedInstance().AddObserverForAll(sObserver, Reference, sBlockToUse);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UnTrackBy(object sObserver)
        {
            BTBNotificationManager.SharedInstance().RemoveObserverForSender(sObserver, Reference, this);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================