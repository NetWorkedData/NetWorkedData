//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:34:8
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================



using System;
//using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++-
    [Serializable]
    public class NWDActionConnection : NWDConnection<NWDAction>
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Tracks the referenced object in the notification.
        /// </summary>
        /// <param name="sObserver">Observer.</param>
        /// <param name="sBlockToUse">S block to use.</param>
        public void TrackBy(object sObserver, NWENotificationBlock sBlockToUse)
        {
            NWDAction tAction = this.GetData();
            if (tAction != null)
            {
                tAction.TrackBy(sObserver, sBlockToUse);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Untrack the referenced object in the notification.
        /// </summary>
        /// <param name="sObserver">Observer.</param>
        public void UnTrackBy(object sObserver)
        {
            NWDAction tAction = this.GetData();
            if (tAction != null)
            {
                tAction.UnTrackBy(sObserver);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================