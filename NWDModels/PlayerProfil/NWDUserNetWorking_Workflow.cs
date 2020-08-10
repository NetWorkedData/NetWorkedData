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
#if NWD_USER_NETWORKING
using System;
using System.Collections.Generic;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserNetWorking : NWDBasisGameSaveDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserNetWorking()
        {
            //Debug.Log("NWDUserNetWorking Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserNetWorking(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDUserNetWorking Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        private static int UpdateDelayInSeconds = 600;
        static bool Started = false;
        static List<Type> OtherData = new List<Type>();
        //-------------------------------------------------------------------------------------------------------------
        public static float DelayInSeconds()
        {
            return (float)UpdateDelayInSeconds;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void PrepareUpdate(int sUpdateDelayInSeconds, List<Type> sOtherData)
        {
            //Debug.Log("NWDUserNetWorking Static StartUpdate()");
            if (Started == false)
            {
                Started = true;
                if (sUpdateDelayInSeconds >= 60)
                {
                    UpdateDelayInSeconds = sUpdateDelayInSeconds;
                }
                if (sOtherData != null)
                {
                    OtherData = sOtherData;
                }
                if (OtherData.Contains(typeof(NWDUserNetWorking)) == false)
                {
                    OtherData.Add(typeof(NWDUserNetWorking));
                }
                // do something with this class
                NWDUserNetWorking tUserNetWorking = NWDBasisHelper.GetReachableFirstData<NWDUserNetWorking>();
                if (tUserNetWorking == null)
                {
                    tUserNetWorking = NWDBasisHelper.NewData<NWDUserNetWorking>();
                    tUserNetWorking.InsertData();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void NetworkingUpdate()
            {
                //Debug.Log("NWDUserNetWorking Static NetworkingUpdate()");
                Started = true;
            NWDUserNetWorking tUserNetWorking = NWDBasisHelper.GetReachableFirstData<NWDUserNetWorking>();
                if (tUserNetWorking != null)
                {
                    DateTime tDateTime = DateTime.Now;
                    int tTimestampA = (int)NWEDateHelper.ConvertToTimestamp(tDateTime);
                    int tTimestampB = (int)NWEDateHelper.ConvertToTimestamp(tUserNetWorking.NextUpdate.ToDateTime());
                    tUserNetWorking.TotalPlay += UpdateDelayInSeconds - tTimestampB + tTimestampA;
                    tDateTime = tDateTime.AddSeconds(UpdateDelayInSeconds);
                    tUserNetWorking.NextUpdate.SetDateTime(tDateTime);
                    tUserNetWorking.UpdateData();
                    NWDDataManager.SharedInstance().AddWebRequestSynchronization(OtherData, true);
                // use AddWebRequestSynchronizationWithBlock?
                }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void NetworkingOffline()
        {
            //Debug.Log("NWDUserNetWorking Static NetworkingOffline()");
            if (Started == true)
            {
                Started = false;
                NWDUserNetWorking tUserNetWorking = NWDBasisHelper.GetReachableFirstData<NWDUserNetWorking>();
                if (tUserNetWorking != null)
                {
                    tUserNetWorking.Offline();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Offline()
        {
            DateTime tDateTime = DateTime.Now;
            int tTimestampA = (int)NWEDateHelper.ConvertToTimestamp(tDateTime);
            int tTimestampB = (int)NWEDateHelper.ConvertToTimestamp(NextUpdate.ToDateTime());
            TotalPlay += UpdateDelayInSeconds - tTimestampB + tTimestampA;
            NextUpdate.SetDateTime(tDateTime);
            UpdateData();
            //NWDDataManager.SharedInstance().AddWebRequestSynchronization(OtherData, true);
            NWDDataManager.SharedInstance().AddWebRequestSynchronization(new List<Type>() { typeof(NWDUserNetWorking) }, true);
            // use AddWebRequestSynchronizationWithBlock?
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserNetWorkingState Statut()
        {
            NWDUserNetWorkingState rReturn = NWDUserNetWorkingState.Unknow;
            if (Masked == true)
            {
                rReturn = NWDUserNetWorkingState.Masked;
            }
            else
            {
                if (NextUpdate.ToDateTime() > DateTime.Now)
                {
                    if (NotDisturbe == true)
                    {
                        rReturn = NWDUserNetWorkingState.NotDisturbe;
                    }
                    else
                    {
                        rReturn = NWDUserNetWorkingState.OnLine;
                    }
                }
                else
                {
                    rReturn = NWDUserNetWorkingState.OffLine;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif