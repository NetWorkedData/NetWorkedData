//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:16
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWBBenchmarkResult : NWDBasisBundled
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        static public void ClassMethodExample()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public void InstanceMethodExample()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWBBenchmarkResult CurrentData()
        {
            NWBBenchmarkResult tInfos = null;
            if (NWDBasisHelper.FindTypeInfos(typeof(NWBBenchmarkResult)).IsLoaded())
            {
                //NWDAccountInfos tInfos = FindFirstDataByAccount(NWDAccount.CurrentReference(), true);
                tInfos = NWDBasisHelper.GetRawDataByReference<NWBBenchmarkResult>(NWDAccount.CurrentReference());
                if (tInfos == null)
                {
                    tInfos = NWDBasisHelper.NewDataWithReference<NWBBenchmarkResult>(NWDAccount.CurrentReference());
                    tInfos.SaveData();
                }
            }
            return tInfos;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //--------------------------------------------------------------------------------------------------------------
        #region Instance Initialization
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
            base.Initialization();
            // do something when object will be inserted
            // TODO verif if method is call in good place in good timing
        }

        //-------------------------------------------------------------------------------------------------------------
        public void BenchmarkNow() // INIT YOUR INSTANCE WITH THIS METHOD
        {
            HistoricDate = new NWDDateTimeType();
            HistoricDate.SetCurrentDateTime();
            Builder = NWDAppConfiguration.SharedInstance().BuilderUser;
            CompileOn = NWDAppConfiguration.SharedInstance().CompileOn;
            CompileFor = Application.platform.ToString();
            OSVersion = SystemInfo.operatingSystem;
            CompileWith = Application.unityVersion;
            Device = SystemInfo.deviceModel;
            PreloadDatas = NWDLauncher.GetPreload();
            PreloadFaster = NWDAppConfiguration.SharedInstance().LauncherFaster;
            BenchmarkStep = NWDLauncher.ActiveBenchmark;
            LaunchUnity = NWDLauncher.TimeStart;
            SQLSecure = NWDDataManager.SharedInstance().IsSecure();
            SQLVersion = NWDDataManager.SharedInstance().GetVersion();
            LauchNetworkedData = NWDLauncher.TimeNWDFinish;
            CopyDatabase = NWDLauncher.CopyDatabase;
            LauchFinal = NWDLauncher.TimeFinish;
            RowsLoaded = NWDDataManager.SharedInstance().RowsCounterOp;
            RowsMemoryIndexed = NWDDataManager.SharedInstance().MethodCounterOp;
            MethodMemoryIndexation = NWDDataManager.SharedInstance().MethodCounterOp;
            //InternalKey = HistoricDate.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss") + " " + Account.GetReference();
            InternalKey = HistoricDate.ToDateTime().ToString("yyyy-MM-dd HH:mm:ss")+" " + Device + " "+ OSVersion;
            InternalDescription = LauchNetworkedData.ToString("F3")+"s for "+ RowsLoaded.ToString()+ " rows";
            SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================