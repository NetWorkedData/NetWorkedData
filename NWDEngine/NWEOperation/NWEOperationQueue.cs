

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