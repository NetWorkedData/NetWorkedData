//=====================================================================================================================
//
//  ideMobi 2020©
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Linq;

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public static partial class NWDLauncher
    {
        //-------------------------------------------------------------------------------------------------------------
        const string NWDLauncher_Runtime_Async = "NWDLauncher_Runtime_Async";
        //-------------------------------------------------------------------------------------------------------------
        private static void Launch_Runtime_Async()
        {
            NWEBenchmark.Start(NWDLauncher_Runtime_Async);
            // lauch engine
            Engine_Runtime_Async();
            // declare models
            Declare_Runtime_Async();
            // restaure models' param
            Restaure_Runtime_Async();
            // connect editor
            Connect_Editor_Runtime_Async();
            // load editor data
            LoadData_Editor_Runtime_Async();
            // index all data editor
            Index_Editor_Runtime_Async();
            // need account pincode
            //PinCode_Account_Runtime_Sync(string sPinCode);
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Engine_Runtime_Async()
        {
            NWEBenchmark.Start();
            Thread.CurrentThread.CurrentCulture = NWDConstants.FormatCountry;
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Declare_Runtime_Async()
        {
            NWEBenchmark.Start();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Restaure_Runtime_Async()
        {
            NWEBenchmark.Start();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Connect_Editor_Runtime_Async()
        {
            NWEBenchmark.Start();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void LoadData_Editor_Runtime_Async()
        {
            NWEBenchmark.Start();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Index_Editor_Runtime_Async()
        {
            NWEBenchmark.Start();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void PinCode_Account_Runtime_Async(string sPinCode)
        {
            NWEBenchmark.Start();
            // connect account
            Connect_Account_Runtime_Async();
            // load account data account
            LoadData_Account_Runtime_Async();
            // index all data
            Index_Account_Runtime_Async();
            // Ready!
            Ready_Runtime_Async();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Connect_Account_Runtime_Async()
        {
            NWEBenchmark.Start();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void LoadData_Account_Runtime_Async()
        {
            NWEBenchmark.Start();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Index_Account_Runtime_Async()
        {
            NWEBenchmark.Start();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Ready_Runtime_Async()
        {
            NWEBenchmark.Start();
            NWEBenchmark.Finish();
            NWEBenchmark.Finish(NWDLauncher_Runtime_Async);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================