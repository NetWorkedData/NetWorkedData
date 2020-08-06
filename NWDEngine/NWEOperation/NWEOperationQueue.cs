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
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[ExecuteInEditMode]
	//-------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// NWE operation queue.
	/// </summary>
	public partial class NWEOperationQueue
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The operations List.
		/// </summary>
		public List<NWEOperation> Operations = new List<NWEOperation> ();
		/// <summary>
		/// The actual operation.
		/// </summary>
		public NWEOperation ActualOperation;
		/// <summary>
		/// The synchronize in progress.
		/// </summary>
		public bool SynchronizeInProgress = false;
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================