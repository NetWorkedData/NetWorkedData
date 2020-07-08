//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#endif
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWENotificationObserversList
	{
		//-------------------------------------------------------------------------------------------------------------
		public List<NWENotificationObserver> ObserversProperties = new List<NWENotificationObserver> ();
		//-------------------------------------------------------------------------------------------------------------
		public void AddObserver (object sObserver, string sNotificationKey, object sSender, NWENotificationBlock sBlockToUse)
		{
			bool tExists = false;
			foreach (NWENotificationObserver tItem in ObserversProperties) 
			{
				if (tItem.Observer == sObserver && tItem.Sender == sSender)
				{
					tExists = true;
					break;
				}
			}
			if (tExists == false) 
			{
				NWENotificationObserver tItem = new NWENotificationObserver (sObserver, sNotificationKey, sSender, sBlockToUse);
				ObserversProperties.Add (tItem);	
			} 
//			else 
//			{
//				//				Debug.Log ("AddObserver " +  sNotificationKey + " already exists");
//			}

		}
		//-------------------------------------------------------------------------------------------------------------
		public void RemoveObserver (object sObserver, string sNotificationKey, object sSender, bool sRemoveAll)
		{
			List<NWENotificationObserver> tObserversToRemove = new List<NWENotificationObserver> (); 
			foreach (NWENotificationObserver tItem in ObserversProperties) 
			{
				if (tItem.Observer == sObserver && (tItem.Sender == sSender || sRemoveAll == true))
				{
					tObserversToRemove.Add (tItem);
				}
            }
            //Debug.Log("have  " + ObserversProperties.Count + " observer");
            //Debug.Log("will remove " + tObserversToRemove.Count +" observer");
			foreach (NWENotificationObserver tItem in tObserversToRemove) 
			{
				ObserversProperties.Remove (tItem);
			}
            //Debug.Log("rest  " + ObserversProperties.Count + " observer");
        }
		//-------------------------------------------------------------------------------------------------------------
		public void SendNotification (NWENotification sNotification)
        {
            //Debug.Log("have  " + ObserversProperties.Count + " observer");
            //Debug.Log("will poste " + ObserversProperties.Count + " observer");

            // I do copy to prevent remove from delegate in block
            List<NWENotificationObserver> tObserversProperties = new List<NWENotificationObserver>(ObserversProperties);
            // use the copy to send notification
            foreach (NWENotificationObserver tItem in tObserversProperties) 
			{
				if (tItem.Sender == sNotification.Sender || tItem.Sender==null)
				{
					if (tItem.BlockToUse != null) 
					{
						tItem.BlockToUse (sNotification);	
					} 
					//					else {
					//						Type tType = tItem.Observer.GetType ();
					//						var tMethodInfo = tType.GetMethod (tItem.NotificationKey, BindingFlags.Public | BindingFlags.Instance);
					//						if (tMethodInfo != null) {
					//							tMethodInfo.Invoke (tItem.Observer, new object[]{ sNotification });
					//						} 
					//						else {
					//							Debug.Log ("For class " + tType.Name + " the instance method is not implemented or public. Add instance method : public void " + tItem.NotificationKey + "(IDENotification sNotification) {}");
					//						}
					//					}
				}
			}
            tObserversProperties = null;

        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================