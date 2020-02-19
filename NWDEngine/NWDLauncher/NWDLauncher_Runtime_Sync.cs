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
        private static void Launch_Runtime_Sync()
        {
            NWEBenchmark.Start();
            StepSum = 12;
            StepIndex = 0;
            // lauch engine
            Engine_Editor();
            // declare models
            Declare_Editor();
            // restaure models' param
            Restaure_Editor();
            // connect editor
            Connect_Editor_Editor();
            // create table editor
            CreateTable_Editor_Editor();
            // load editor data
            LoadData_Editor_Editor();
            // index all data editor
            Index_Editor_Editor();
            // need account pincode
            Connect_Account_Editor();
            // create table account
            CreateTable_Account_Editor();
            // load account data account
            LoadData_Account_Editor();
            // index all data
            Index_Account_Editor();
            // Ready!
            Ready_Editor();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Engine_Runtime_Sync()
        {
            NWEBenchmark.Start();
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
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================