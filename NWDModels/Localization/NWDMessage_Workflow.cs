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
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
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
