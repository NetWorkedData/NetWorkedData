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
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDServerDatas : NWDBasisUnsynchronize
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDServerDatas()
        {
            //Debug.Log("NWDServerConfig Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDServerDatas(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDServerConfig Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
            base.Initialization();
            string tRandom = NWDToolbox.RandomStringAlpha(3).ToLower();
            UserMax = 200000;
            InternalKey = "Unused config";
            MySQLUser = "dbuser" + tRandom;
            MySQLPort = 3306;
            MySQLBase = "NetWorkedData" + tRandom;
            External = true;
            PhpMyAdmin = true;
            // no sync please
            DevSync = -1;
            PreprodSync = -1;
            ProdSync = -1;
            //AccountRangeStart = 000;
            //AccountRangeEnd = 999;
        }

        //-------------------------------------------------------------------------------------------------------------
        public void CheckInformations()
        {
            //PropertiesPrevent();
            MySQLUser = NWDToolbox.UnixCleaner(MySQLUser);
            MySQLBase = NWDToolbox.UnixCleaner(MySQLBase);
            if (Range < 1)
            {
                Range = 1; // 0 is reserved by old system without cluster
            }
            int tNexRange = Range;
            bool tTest = false;
            while (tTest == false)
            {
                tTest = true;
                foreach (NWDServerDatas tServerDatas in BasisHelper().Datas)
                {
                    if (tServerDatas != this)
                    {
                        //if (tServerDatas.IsEnable()) // use all servers, not only active ? Modify in usage.
                        {
                            if (tServerDatas.Range == tNexRange)
                            {
                                tNexRange++;
                                tTest = false;
                                break;
                            }
                        }
                    }
                }
            }
            if (ServerEditorOriginal==null)
            {
                ServerEditorOriginal = new NWDReferenceType<NWDServerDatas>();
            }
            if (ServerEditorOriginal.GetRawData() == this)
            {
                ServerEditorOriginal.SetData(null);
            }
            Range = tNexRange;
            if (UserMax < -1)
            {
                UserMax = -1;
            }
            if (UserMax > 500000)
            {
                UserMax = 500000;
            }
            // check for range
            tTest = false;

            
            // determine free range :
            List<int> MinRangeList = new List<int>();
            List<int> MaxRangeList = new List<int>();
            /*
            List<NWDServerDatas> tRangeUsed = new List<NWDServerDatas>();
            foreach (NWDServerDatas tServerDatas in BasisHelper().Datas)
            {
                if (tServerDatas != this)
                {
                    tRangeUsed.Add(tServerDatas);
                    if (tServerDatas.RangeMin < kRangeMin)
                    {
                        tServerDatas.RangeMin = kRangeMin;
                    }
                    if (tServerDatas.RangeMin >= kRangeMax)
                    {
                        tServerDatas.RangeMin = kRangeMax - 1;
                    }
                    if (tServerDatas.RangeMax <= RangeMin)
                    {
                        tServerDatas.RangeMax = RangeMin + 1;
                    }
                    if (tServerDatas.RangeMax > kRangeMax)
                    {
                        tServerDatas.RangeMax = kRangeMax;
                    }
                    tServerDatas.UpdateDataIfModified();
                }
            }
            // sort by range and create free range 
            tRangeUsed.Sort((x, y) => x.RangeMin.CompareTo(y.RangeMin));
            int tCountRange = tRangeUsed.Count + 1;
            MinRangeList.Add(kRangeMin);
            foreach (NWDServerDatas tServerDatas in tRangeUsed)
            {
                MaxRangeList.Add(tServerDatas.RangeMin);
                MinRangeList.Add(tServerDatas.RangeMax);
            }
            MaxRangeList.Add(kRangeMax);

            tTest = false;
            for (int tI = 0; tI < MinRangeList.Count; tI++)
            {
                if (RangeMin > MinRangeList[tI] && RangeMin < MaxRangeList[tI] && RangeMax > MinRangeList[tI] && RangeMax < MaxRangeList[tI] && RangeMax > RangeMin)
                {
                    tTest = true;
                    break;
                }
            }
            if (tTest == false)
            {
                // use the first free range ?
                for (int tI = 0; tI < MinRangeList.Count; tI++)
                {
                    if (MinRangeList[tI] != MaxRangeList[tI])
                    {
                        RangeMin = MinRangeList[tI];
                        RangeMax = MaxRangeList[tI];
                        tTest = true;
                        break;
                    }
                }
            }
            
    */
            if (RangeMin < kRangeMin)
            {
                RangeMin = kRangeMin;
            }
            if (RangeMin >= kRangeMax)
            {
                RangeMin = kRangeMax - 1;
            }

            if (RangeMax <= RangeMin)
            {
                RangeMax = RangeMin + 1;
            }
            if (RangeMax > kRangeMax)
            {
                RangeMax = kRangeMax;
            }
            /*
            if (tTest == false)
            {
                // ok ... no place ... dont use this database
                RangeMin = 0;
                RangeMax = 0;
            }
            */

            List<string> tDescription = new List<string>();
            if (Dev == true)
            {
                tDescription.Add(NWDAppConfiguration.SharedInstance().DevEnvironment.Environment);
            }
            if (Preprod == true)
            {
                tDescription.Add(NWDAppConfiguration.SharedInstance().PreprodEnvironment.Environment);
            }
            if (Prod == true)
            {
                tDescription.Add(NWDAppConfiguration.SharedInstance().ProdEnvironment.Environment);
            }
            InternalDescription = string.Join(" / ", tDescription);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDServerDatabaseAuthentication GetServerDatabase(NWDAppEnvironment sEnvironment)
        {
            NWDServerDatabaseAuthentication rReturn = null;
            if ((sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment && Dev == true) ||
                (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment && Preprod == true) ||
                (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment && Prod == true))
            {
                rReturn = new NWDServerDatabaseAuthentication(InternalKey, Reference, Range.ToString(), RangeMin, RangeMax, UserMax.ToString(), NWDToolbox.TextUnprotect(MySQLIP.GetValue()), MySQLPort, NWDToolbox.TextUnprotect(MySQLBase), NWDToolbox.TextUnprotect(MySQLUser), NWDToolbox.TextUnprotect(MySQLSecurePassword.Decrypt()));
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDServerDatabaseAuthentication[] GetAllConfigurationServerDatabase(NWDAppEnvironment sEnvironment)
        {
            List<NWDServerDatabaseAuthentication> rReturn = new List<NWDServerDatabaseAuthentication>();
            //rReturn.Add(GetConfigurationServerDatabase(sEnvironment));
            foreach (NWDServerDatas tServerDatabase in NWDBasisHelper.GetRawDatas<NWDServerDatas>())
            {
                NWDServerDatabaseAuthentication tConn = tServerDatabase.GetServerDatabase(sEnvironment);
                if (tConn != null)
                {
                    rReturn.Add(tConn);
                }
            }
            return rReturn.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
