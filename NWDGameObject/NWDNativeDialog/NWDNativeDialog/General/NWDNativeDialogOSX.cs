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
using System.Collections;
#if UNITY_STANDALONE_OSX
using System.Runtime.InteropServices;
#endif
#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDNativeDialogOSX
    {
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_STANDALONE_OSX
        //-------------------------------------------------------------------------------------------------------------
        [DllImport("NWDOSX")]
        private static extern int _NWD_ShowDialog(string sTitle, string sMessage, string sOK, string sNOK);
        //-------------------------------------------------------------------------------------------------------------
        [DllImport("NWDOSX")]
        private static extern int _NWD_ShowAlert(string sTitle, string sMessage, string sOK);
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        public static int ShowDialog(string title, string message, string yes, string no)
        {
#if UNITY_STANDALONE_OSX
            return _NWD_ShowDialog(title, message, yes, no);
#else
            return -1;
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public static int ShowAlert(string title, string message, string ok)
        {
#if UNITY_STANDALONE_OSX
            return _NWD_ShowAlert(title, message, ok);
#else
            return -1;
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================