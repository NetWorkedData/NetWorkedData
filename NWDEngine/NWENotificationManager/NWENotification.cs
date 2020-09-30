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
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWENotification
	{
		//-------------------------------------------------------------------------------------------------------------
		public string NotificationKey;
		public object Sender;
		public object Datas;
		//-------------------------------------------------------------------------------------------------------------
		public NWENotification (string sNotificationKey,object sSender)
		{
			NotificationKey = sNotificationKey;
			Sender = sSender;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWENotification (string sNotificationKey,object sSender, object sDatas)
		{
			NotificationKey = sNotificationKey;
			Sender = sSender;
			Datas = sDatas;
		}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
