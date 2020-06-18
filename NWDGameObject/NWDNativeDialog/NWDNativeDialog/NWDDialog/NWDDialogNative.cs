using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public delegate void NWDDialogOnCompleteBlock(NWDMessageState state);
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDDialogNative
    {
        //-------------------------------------------------------------------------------------------------------------
        const string K_OK = "YES";
        const string K_NOK = "NO";
        //-------------------------------------------------------------------------------------------------------------
        public static NWDDialogNative Dialog(NWDMessage sMessage, NWDDialogOnCompleteBlock sCompleteBlock = null)
        {
            NWDDialogNative rDialog = new NWDDialogNative(
                sMessage.Title.GetLocalString(),
                sMessage.Description.GetLocalString(),
                sMessage.Validation.GetLocalString(),
                sMessage.Cancel.GetLocalString(),
                sCompleteBlock);
            return rDialog;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDDialogNative Dialog(string sTitle, string sMessage, string sOK = K_OK, string sNOK = K_NOK, NWDDialogOnCompleteBlock sCompleteBlock = null)
        {
            NWDDialogNative rDialog = new NWDDialogNative(sTitle, sMessage, sOK, sNOK, sCompleteBlock);
            return rDialog;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDDialogNative(string sTitle, string sMessage, string sOK = K_OK, string sNOK = K_NOK)
        {
            Initialization(sTitle, sMessage, sOK, sNOK, null);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDDialogNative(string sTitle, string sMessage, string sOK, string sNOK, NWDDialogOnCompleteBlock sCompleteBlock = null)
        {
            Initialization(sTitle, sMessage, sOK,sNOK, sCompleteBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        private void Initialization(string sTitle, string sMessage, string sOK, string sNOK, NWDDialogOnCompleteBlock sCompleteBlock = null)
        {
#if UNITY_EDITOR
            if (EditorUtility.DisplayDialog(sTitle, sMessage, sOK, sNOK) == true)
            {
                if (sCompleteBlock != null)
                {
                    sCompleteBlock(NWDMessageState.OK);
                }
            }
            else
            {
                if (sCompleteBlock != null)
                {
                    sCompleteBlock(NWDMessageState.NOK);
                }
            }
#else
#if UNITY_IPHONE
            NWDDialogIOS.Create(        sTitle, sMessage, sOK, sNOK, sCompleteBlock);
#elif UNITY_ANDROID
            NWDDialogAndroid.Create(    sTitle, sMessage, sOK, sNOK, sCompleteBlock);
#elif UNITY_STANDALONE_OSX
            NWDDialogOSX.Create(        sTitle, sMessage, sOK, sNOK, sCompleteBlock);
#elif UNITY_STANDALONE_WIN
            NWDDialogWin.Create(        sTitle, sMessage, sOK, sNOK, sCompleteBlock);
#elif UNITY_STANDALONE_LINUX
            Debug.Log("ALERT " +        sTitle +" " + sMessage);
#else
            Debug.Log("ALERT " +        sTitle +" " + sMessage);
#endif

#endif
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
