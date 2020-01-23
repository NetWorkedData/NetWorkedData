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

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAction : NWDBasis
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
            //NWENotificationManager.SharedInstance().PostNotification(this, ActionName);
            NWENotificationManager.SharedInstance().PostNotification(this, Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void TrackBy(object sObserver, NWENotificationBlock sBlockToUse)
        {
            NWENotificationManager.SharedInstance().AddObserverForAll(sObserver, Reference, sBlockToUse);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UnTrackBy(object sObserver)
        {
            NWENotificationManager.SharedInstance().RemoveObserverForSender(sObserver, Reference, this);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================