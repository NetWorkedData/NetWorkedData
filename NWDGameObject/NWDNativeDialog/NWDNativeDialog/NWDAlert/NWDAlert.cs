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
using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public delegate void NWDAlertOnCompleteBlock(NWDMessageState state);
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDAlert
    {
        //-------------------------------------------------------------------------------------------------------------
        const string K_OK = "OK";
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAlert Alert(NWDMessage sMessage, NWDAlertOnCompleteBlock sCompleteBlock = null)
        {
            NWDAlert rReturn = new NWDAlert(
                sMessage.Title.GetLocalString(),
                sMessage.Description.GetLocalString(),
                sMessage.Validation.GetLocalString(),
                sCompleteBlock);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAlert Alert(NWDError sError, NWDAlertOnCompleteBlock sCompleteBlock = null)
        {
            NWDAlert rReturn = new NWDAlert(
                sError.Title.GetLocalString(),
                sError.Description.GetLocalString(),
                sError.Validation.GetLocalString(),
                sCompleteBlock);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAlert Alert(string sTitle, string sMessage, string sOK = K_OK, NWDAlertOnCompleteBlock sCompleteBlock = null)
        {
            NWDAlert rReturn = new NWDAlert(sTitle, sMessage, sOK, sCompleteBlock);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAlert(string sTitle, string sMessage, string sOK = K_OK)
        {
            Initialization(sTitle, sMessage, sOK, null);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAlert(string sTitle, string sMessage, string sOK, NWDAlertOnCompleteBlock sCompleteBlock = null )
        {
            Initialization(sTitle, sMessage, sOK,sCompleteBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        private void Initialization(string sTitle, string sMessage, string sOK, NWDAlertOnCompleteBlock sCompleteBlock = null)
        {
#if UNITY_EDITOR
            if (EditorUtility.DisplayDialog(sTitle, sMessage, sOK) == true)
            {
                if (sCompleteBlock != null)
                {
                    sCompleteBlock(NWDMessageState.OK);
                }
            }
#else
#if UNITY_IPHONE
            NWDAlertIOS.Create(sTitle, sMessage, sOK, sCompleteBlock);
#elif UNITY_ANDROID
            NWDAlertAndroid.Create(sTitle, sMessage, sOK, sCompleteBlock);
#elif UNITY_STANDALONE_OSX
            NWDAlertOSX.Create(sTitle, sMessage, sOK, sCompleteBlock);
#elif UNITY_STANDALONE_WIN
            NWDAlertWin.Create(sTitle, sMessage, sOK, sCompleteBlock);
#elif UNITY_STANDALONE_LINUX
            Debug.Log("ALERT " + sTitle +" " + sMessage);
#else
            Debug.Log("ALERT " + sTitle +" " + sMessage);
#endif

#endif
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
