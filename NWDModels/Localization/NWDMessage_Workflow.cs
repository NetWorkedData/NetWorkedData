//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDMessage : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDMessage()
        {
            //Debug.Log("NWDMessage Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDMessage(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDMessage Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PostNotification(NWDUserNotificationDelegate sValidationbBlock = null, NWDUserNotificationDelegate sCancelBlock = null)
        {
            NWDUserNotification tNotification = new NWDUserNotification(this, sValidationbBlock, sCancelBlock);
            tNotification.Post();
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        public static NWDMessage EditorCreateIfNotExists(string sReferenceKey, string sBaseTitle, string sBaseDescription, string sBaseValidation, string sBaseCancel)
        {
            NWDMessage rReturn = NWDBasisHelper.GetRawDataByReference<NWDMessage>(sReferenceKey);
            if (rReturn == null)
            {
                rReturn = NWDBasisHelper.NewDataWithReference<NWDMessage>(sReferenceKey);
                rReturn.Title.AddBaseString(sBaseTitle);
                rReturn.Description.AddBaseString(sBaseDescription);
                rReturn.Validation.AddBaseString(sBaseValidation);
                rReturn.Cancel.AddBaseString(sBaseCancel);
                rReturn.InternalKey = sReferenceKey; 
                rReturn.SaveData();
            }
            return rReturn;
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        public static NWDMessage GetByReference(string sReferenceKey)
        {
            NWDMessage rReturn = NWDBasisHelper.GetRawDataByReference<NWDMessage>(sReferenceKey);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================