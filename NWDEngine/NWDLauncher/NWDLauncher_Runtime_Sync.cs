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
        const string NWDLauncher_Runtime_Sync = "NWDLauncher_Runtime_Sync";
        //-------------------------------------------------------------------------------------------------------------
        private static void Launch_Runtime_Sync()
        {
            NWEBenchmark.Start(NWDLauncher_Runtime_Sync);
            // lauch engine
            Engine_Runtime_Sync();
            // declare models
            Declare_Runtime_Sync();
            // restaure models' param
            Restaure_Runtime_Sync();
            // connect editor
            Connect_Editor_Runtime_Sync();
            // load editor data
            LoadData_Editor_Runtime_Sync();
            // index all data editor
            Index_Editor_Runtime_Sync();
            // need account pincode
            //PinCode_Account_Runtime_Sync(string sPinCode);
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Engine_Runtime_Sync()
        {
            NWEBenchmark.Start();
            Thread.CurrentThread.CurrentCulture = NWDConstants.FormatCountry;

            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Declare_Runtime_Sync()
        {
            NWEBenchmark.Start();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Restaure_Runtime_Sync()
        {
            NWEBenchmark.Start();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Connect_Editor_Runtime_Sync()
        {
            NWEBenchmark.Start();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void LoadData_Editor_Runtime_Sync()
        {
            NWEBenchmark.Start();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Index_Editor_Runtime_Sync()
        {
            NWEBenchmark.Start();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void PinCode_Account_Runtime_Sync(string sPinCode)
        {
            NWEBenchmark.Start();
            // connect account
            Connect_Account_Runtime_Sync();
            // load account data account
            LoadData_Account_Runtime_Sync();
            // index all data
            Index_Account_Runtime_Sync();
            // Ready!
            Ready_Runtime_Sync();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Connect_Account_Runtime_Sync()
        {
            NWEBenchmark.Start();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void LoadData_Account_Runtime_Sync()
        {
            NWEBenchmark.Start();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Index_Account_Runtime_Sync()
        {
            NWEBenchmark.Start();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Ready_Runtime_Sync()
        {
            NWEBenchmark.Start();
            NWEBenchmark.Finish();
            NWEBenchmark.Finish(NWDLauncher_Runtime_Sync);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================