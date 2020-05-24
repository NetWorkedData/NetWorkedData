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
    public class NWDNativeDialogWin
    {
        //-------------------------------------------------------------------------------------------------------------
        /*
         [System.Runtime.InteropServices.DllImport("user32.dll")]
         private static extern System.IntPtr GetActiveWindow();

         public static System.IntPtr GetWindowHandle()
         {
             return GetActiveWindow();
         }

         [DllImport("user32.dll", SetLastError = true)]
         static extern int MessageBox(IntPtr hwnd, String lpText, String lpCaption, uint uType);

         /// <summary>
         /// Shows Error alert box with OK button.
         /// </summary>
         /// <param name="text">Main alert text / content.</param>
         /// <param name="caption">Message box title.</param>
         public static void Error(string text, string caption)
         {
             try
             {
                 MessageBox(GetWindowHandle(), text, caption, (uint)(0x00000000L | 0x00000010L));
             }
             catch (Exception ex) { }
         }
        */
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_STANDALONE_WIN
        //-------------------------------------------------------------------------------------------------------------
        public delegate void UnityCallbackDelegate(IntPtr sObjectName, IntPtr sCommandName, IntPtr sCommandData);
        [DllImport("user32.dll")]
        public static extern void ConnectCallback([MarshalAs(UnmanagedType.FunctionPtr)] UnityCallbackDelegate callbackMethod);
        //-------------------------------------------------------------------------------------------------------------
        [DllImport("user32.dll")]
        private static extern void _NWD_ShowDialog(string sTitle, string sMessage, string sOK, string sNOK);
        //-------------------------------------------------------------------------------------------------------------
        [DllImport("user32.dll")]
        private static extern void _NWD_ShowAlert(string sTitle, string sMessage, string sOK);
        //-------------------------------------------------------------------------------------------------------------
        //[DllImport("user32.dll")]
        //private static extern void _NWD_DismissAlert();
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        public static void ShowDialog(string title, string message, string yes, string no)
        {
            //Debug.Log("ShowDialog()");
#if UNITY_STANDALONE_WIN
            _NWD_ShowDialog(title, message, yes, no);
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ShowAlert(string title, string message, string ok)
        {
#if UNITY_STANDALONE_WIN
            _NWD_ShowAlert(title, message, ok);
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        //        public static void DismissAlert()
        //        {
        //#if UNITY_STANDALONE_OSX
        //            _NWD_DismissAlert();
        //#endif
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================