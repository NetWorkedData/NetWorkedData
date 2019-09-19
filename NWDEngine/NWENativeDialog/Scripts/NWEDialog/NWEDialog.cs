using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public delegate void NWEDialogOnCompleteBlock(NWEMessageState state);
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWEDialog
    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWEDialog Dialog(string sTitle, string sMessage, string sOK = "YES", string sNOK = "NO", NWEDialogOnCompleteBlock sCompleteBlock = null)
        {
            NWEDialog rDialog = new NWEDialog(sTitle, sMessage, sOK, sNOK, sCompleteBlock);
            return rDialog;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWEDialog(string sTitle, string sMessage, string sOK = "YES", string sNOK = "NO")
        {
            Initialization(sTitle, sMessage, sOK, sNOK, null);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWEDialog(string sTitle, string sMessage, string sOK, string sNOK, NWEDialogOnCompleteBlock sCompleteBlock = null)
        {
            Initialization(sTitle, sMessage, sOK,sNOK, sCompleteBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        private void Initialization(string sTitle, string sMessage, string sOK, string sNOK, NWEDialogOnCompleteBlock sCompleteBlock = null)
        {
#if UNITY_EDITOR
            if (EditorUtility.DisplayDialog(sTitle, sMessage, sOK, sNOK) == true)
            {
                if (sCompleteBlock != null)
                {
                    sCompleteBlock(NWEMessageState.OK);
                }
            }
            else
            {
                if (sCompleteBlock != null)
                {
                    sCompleteBlock(NWEMessageState.NOK);
                }
            }
#else
#if UNITY_IPHONE
            NWEDialogIOS.Create(sTitle, sMessage, sOK, sNOK, sCompleteBlock);
#elif UNITY_ANDROID
            NWEDialogAndroid.Create(sTitle, sMessage, sOK, sNOK, sCompleteBlock);
#elif UNITY_STANDALONE_OSX
            NWEDialogOSX.Create(sTitle, sMessage, sOK, sNOK, sCompleteBlock);
#elif UNITY_STANDALONE_WIN
            Debug.Log("ALERT " + sTitle +" " + sMessage);
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
