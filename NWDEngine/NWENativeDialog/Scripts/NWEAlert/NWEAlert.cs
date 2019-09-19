using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public delegate void NWEAlertOnCompleteBlock(NWEMessageState state);
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWEAlert
    {
        //-------------------------------------------------------------------------------------------------------------
        const string K_OK = "OK";
        //-------------------------------------------------------------------------------------------------------------
        public static NWEAlert Alert(string sTitle, string sMessage, string sOK = K_OK, NWEAlertOnCompleteBlock sCompleteBlock = null)
        {
            NWEAlert rReturn = new NWEAlert(sTitle, sMessage, sOK, null);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWEAlert(string sTitle, string sMessage, string sOK = K_OK)
        {
            Initialization(sTitle, sMessage, sOK, null);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWEAlert(string sTitle, string sMessage, string sOK, NWEAlertOnCompleteBlock sCompleteBlock = null )
        {
            Initialization(sTitle, sMessage, sOK,sCompleteBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        private void Initialization(string sTitle, string sMessage, string sOK, NWEAlertOnCompleteBlock sCompleteBlock = null)
        {
#if UNITY_EDITOR
            if (EditorUtility.DisplayDialog(sTitle, sMessage, sOK) == true)
            {
                if (sCompleteBlock != null)
                {
                    sCompleteBlock(NWEMessageState.OK);
                }
            }
#else
#if UNITY_IPHONE
            NWEAlertIOS.Create(sTitle, sMessage, sOK, sCompleteBlock);
#elif UNITY_ANDROID
            NWEAlertAndroid.Create(sTitle, sMessage, sOK, sCompleteBlock);
#elif UNITY_STANDALONE_OSX
            NWEAlertOSX.Create(sTitle, sMessage, sOK, sCompleteBlock);
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
