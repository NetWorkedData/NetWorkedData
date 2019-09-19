#define DEBUG_MODE

using System;
using UnityEngine;
using System.Collections;

#if UNITY_STANDALONE_OSX
using System.Runtime.InteropServices;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
// try with https://www.youtube.com/watch?v=Q2dDK0ulDYY

namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWENativeDialogOSX
    {
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_STANDALONE_OSX
        //-------------------------------------------------------------------------------------------------------------
        public delegate void UnityCallbackDelegate(IntPtr sObjectName, IntPtr sCommandName, IntPtr sCommandData);
        [DllImport("NetWorkedDataOSX")]
        public static extern void ConnectCallback([MarshalAs(UnmanagedType.FunctionPtr)] UnityCallbackDelegate callbackMethod);
        //-------------------------------------------------------------------------------------------------------------
        [DllImport("NetWorkedDataOSX")]
        private static extern void _NWE_ShowDialog(string sTitle, string sMessage, string sOK, string sNOK);
        //-------------------------------------------------------------------------------------------------------------
        [DllImport("NetWorkedDataOSX")]
        private static extern void _NWE_ShowAlert(string sTitle, string sMessage, string sOK);
        //-------------------------------------------------------------------------------------------------------------
        //[DllImport("NetWorkedDataOSX")]
        //private static extern void _NWE_DismissAlert();
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        public static void ShowDialog(string title, string message, string ok, string nok)
        {
            Debug.Log("ShowDialog()");
#if UNITY_STANDALONE_OSX
            _NWE_ShowDialog(title, message, ok, nok);
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ShowAlert(string title, string message, string ok)
        {
            Debug.Log("ShowAlert()");
#if UNITY_STANDALONE_OSX
            _NWE_ShowAlert(title, message, ok);
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
//        public static void DismissAlert()
//        {
//#if UNITY_STANDALONE_OSX
//            _NWE_DismissAlert();
//#endif
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================