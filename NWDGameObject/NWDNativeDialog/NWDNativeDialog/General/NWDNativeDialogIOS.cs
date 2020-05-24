using UnityEngine;
using System.Collections;

#if (UNITY_IPHONE && !UNITY_EDITOR) || DEBUG_MODE
using System.Runtime.InteropServices;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDNativeDialogIOS
    {
        //-------------------------------------------------------------------------------------------------------------
#if (UNITY_IPHONE && !UNITY_EDITOR) || DEBUG_MODE
        //-------------------------------------------------------------------------------------------------------------
        [DllImport("__Internal")]
        private static extern void _NWD_ShowDialog(string sTitle, string sMessage, string sOK, string sNOK);
        //-------------------------------------------------------------------------------------------------------------
        [DllImport("__Internal")]
        private static extern void _NWD_ShowAlert(string sTitle, string sMessage, string sOK);
        //-------------------------------------------------------------------------------------------------------------
        [DllImport("__Internal")]
        private static extern void _NWD_DismissAlert();
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        public static void ShowDialog(string title, string message, string yes, string no)
        {
#if (UNITY_IPHONE && !UNITY_EDITOR) || DEBUG_MODE
            _NWD_ShowDialog(title, message, yes, no);
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ShowAlert(string title, string message, string ok)
        {
#if (UNITY_IPHONE && !UNITY_EDITOR) || DEBUG_MODE
            _NWD_ShowAlert(title, message, ok);
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void DismissAlert()
        {
#if (UNITY_IPHONE && !UNITY_EDITOR) || DEBUG_MODE
            _NWD_DismissAlert();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================