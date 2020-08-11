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
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
using Renci.SshNet;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDServerDatabaseAuthentication
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Title;
        public string NameID;
        public string Range;
        public string MaxUser;
        public string Host;
        public int Port;
        public string Database;
        public string User;
        public string Password;
        public int RangeMax;
        public int RangeMin;
        //-------------------------------------------------------------------------------------------------------------
        public NWDServerDatabaseAuthentication(string sTitle, string sNameID, string sRange, int sRangeMin, int sRangeMax, string sMaxUser, string sHost, int sPort, string sDatabase, string sUser, string sPassword)
        {
            //----- for debug notion
            Title = sTitle;
            //----- for cluster notion
            NameID = sNameID;
            Range = sRange;
            RangeMin = sRangeMin;
            RangeMax = sRangeMax;
            MaxUser = sMaxUser;
            //-----
            Host = NWDToolbox.CleanDNS(sHost);
            Port = sPort;
            Database = sDatabase;
            User = sUser;
            Password = sPassword;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif