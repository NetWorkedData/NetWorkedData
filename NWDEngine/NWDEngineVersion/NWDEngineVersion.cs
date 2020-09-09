//=====================================================================================================================
// Copyright NetWorkedData ideMobi 2020
// NWD Autogenerate script created by Jean-François CONTART 
//=====================================================================================================================
#if NWD_VERBOSE
#if UNITY_EDITOR
//#define NWD_LOG
//#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
//#undef NWD_LOG
//#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
	{
		//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
		/// <summary>
		/// Just use as abstract for version reccord
		/// </summary>
		public abstract class NWDEngineVersion
			{
				//-------------------------------------------------------------------------------------------------------------
				/// <summary>
				/// Engine version hard (as constant)
				/// </summary>
				public const string Version = "0.82.20083104";
				//-------------------------------------------------------------------------------------------------------------
			}
		//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	}
//=====================================================================================================================