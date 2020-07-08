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
#endif
//=====================================================================================================================

using System;
using System.Diagnostics;

//=====================================================================================================================
namespace NetWorkedData
{

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDWebBenchmark
    {
        //-------------------------------------------------------------------------------------------------------------
        private Stopwatch Watch = new Stopwatch();
        private double Start_P;
        private double Prepare_U;
        private bool UploadStart = false;
        private double Upload_S;
        private bool UploadFinished = false;
        private double Perform_R;
        private bool DownloadStart = false;
        private double Download_C;
        private bool DownloadFinish = false;
        private double Compute_F;
        private double Final_X;
        //-------------------------------------------------------------------------------------------------------------
        public void Start()
        {
            Watch.Reset();
            Watch.Start();
            Start_P = Watch.ElapsedMilliseconds;
            UploadFinished = false;
            UploadFinished = false;
            DownloadStart = false;
            DownloadFinish = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PrepareIsFinished()
        {
            if (UploadStart == false)
            {
                UploadStart = true;
                Prepare_U = Watch.ElapsedMilliseconds;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UploadIsFinished()
        {
            if (UploadFinished == false)
            {
                UploadFinished = true;
                Upload_S = Watch.ElapsedMilliseconds;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PerformIsFinished()
        {
            if (DownloadStart == false)
            {
                DownloadStart = true;
                Perform_R = Watch.ElapsedMilliseconds;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DownloadIsFinished()
        {
            if (DownloadFinish == false)
            {
                DownloadFinish = true;
                Download_C = Watch.ElapsedMilliseconds;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ComputeIsFinished()
        {
            Compute_F = Watch.ElapsedMilliseconds;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void IsFinished()
        {
            Final_X = Watch.ElapsedMilliseconds;
        }
        //-------------------------------------------------------------------------------------------------------------
        public double GetPrepareTime()
        {
            return Prepare_U - Start_P;
        }
        //-------------------------------------------------------------------------------------------------------------
        public double GetUploadTime()
        {
            return Upload_S - Prepare_U;
        }
        //-------------------------------------------------------------------------------------------------------------
        public double GetPerformTime()
        {
            return Perform_R - Upload_S;
        }
        //-------------------------------------------------------------------------------------------------------------
        public double GetDownloadTime()
        {
            return Download_C - Perform_R;
        }
        //-------------------------------------------------------------------------------------------------------------
        public double GetComputeTime()
        {
            return Compute_F - Download_C;
        }
        //-------------------------------------------------------------------------------------------------------------
        public double GetTotalTime()
        {
            return Final_X - Start_P;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetLogString(float sServerPerform = 0)
        {
            if (sServerPerform != 0)
            {
                return "WebRequest " +
                   " Prepare " + (GetPrepareTime() / 1000.0F).ToString("#0.000") + "s " +
                   " Upload " + (GetUploadTime() / 1000.0F).ToString("#0.000") + "s " +
                   " Perform server * " + (sServerPerform).ToString("#0.000") + "s " +
                   " Download * " + ((GetDownloadTime() + GetPerformTime() ) / 1000.0F - sServerPerform).ToString("#0.000") + "s " +
                   " Compute " + (GetComputeTime() / 1000.0F).ToString("#0.000") + "s " +
                   " <b>Final " + (GetTotalTime() / 1000.0F).ToString("#0.000") + "s</b> " +
                   "";
            }
            else
            {
                return "WebRequest " +
                    " Prepare " + (GetPrepareTime() / 1000.0F).ToString("#0.000") + "s " +
                    " Upload " + (GetUploadTime() / 1000.0F).ToString("#0.000") + "s " +
                    " Perform " + ((GetPerformTime()) / 1000.0F).ToString("#0.000") + "s " +
                    " Download " + ((GetDownloadTime()) / 1000.0F).ToString("#0.000") + "s " +
                    " Compute " + (GetComputeTime() / 1000.0F).ToString("#0.000") + "s " +
                    " <b>Final " + (GetTotalTime() / 1000.0F).ToString("#0.000") + "s</b> " +
                    "";
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================