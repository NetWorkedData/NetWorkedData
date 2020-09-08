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

using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[ExecuteInEditMode]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	/// <summary>
	/// NWE operation.
	/// </summary>
	public partial class NWEOperation : MonoBehaviour
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The parent.
		/// </summary>
		public NWEOperationController Parent;
		/// <summary>
		/// The parent queue.
		/// </summary>
		public NWEOperationQueue ParentQueue;
		/// <summary>
		/// The name of the queue.
		/// </summary>
		public string QueueName = "default";
		/// <summary>
		/// The statut.
		/// </summary>
		public NWEOperationState Statut = NWEOperationState.NoQueued;
		/// <summary>
		/// The is finish.
		/// </summary>
		public bool IsFinish = false;
		/// <summary>
		/// The error if exists.
		/// </summary>
		public NWEError Error;
		//-------------------------------------------------------------------------------------------------------------
		// block for callback
		/// <summary>
		/// The sucess block.
		/// </summary>
		private NWEOperationBlock SucessBlock;
		/// <summary>
		/// The fail block.
		/// </summary>
		private NWEOperationBlock FailBlock;
		/// <summary>
		/// The cancel block.
		/// </summary>
		private NWEOperationBlock CancelBlock;
		/// <summary>
		/// The progress block.
		/// </summary>
		private NWEOperationBlock ProgressBlock;
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Inits the block.
		/// </summary>
		/// <param name="sSucessBlock">S sucess block.</param>
		/// <param name="sFailBlock">S fail block.</param>
		/// <param name="sCancelBlock">S cancel block.</param>
		/// <param name="sProgressBlock">S progress block.</param>
		protected void InitBlock (
			NWEOperationBlock sSucessBlock = null, 
			NWEOperationBlock sFailBlock = null, 
			NWEOperationBlock sCancelBlock = null, 
			NWEOperationBlock sProgressBlock = null)
		{
			SucessBlock = sSucessBlock;
			FailBlock = sFailBlock;
			CancelBlock = sCancelBlock;
			ProgressBlock = sProgressBlock;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Invoke block "Success".
		/// </summary>
		/// <param name="sProgress">S progress.</param>
		/// <param name="sInfos">S infos.</param>
		protected void SuccessInvoke (float sProgress = 0.0f, NWEOperationResult sInfos = null)
        {
            SucessBlock?.Invoke(this, sProgress, sInfos);
        }
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Invoke block "Fail".
		/// </summary>
		/// <param name="sProgress">S progress.</param>
		/// <param name="sInfos">S infos.</param>
		protected void FailInvoke (float sProgress = 0.0f, NWEOperationResult sInfos = null)
        {
            FailBlock?.Invoke(this, sProgress, sInfos);
        }
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Invoke block "Cancel".
		/// </summary>
		/// <returns><c>true</c> if this instance cancel invoke the specified sProgress sInfos; otherwise, <c>false</c>.</returns>
		/// <param name="sProgress">S progress.</param>
		/// <param name="sInfos">S infos.</param>
		protected void CancelInvoke (float sProgress = 0.0f, NWEOperationResult sInfos = null)
		{
            CancelBlock?.Invoke(this, sProgress, sInfos);
        }
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Invoke block "Progress".
		/// </summary>
		/// <param name="sProgress">S progress.</param>
		/// <param name="sInfos">S infos.</param>
		protected void ProgressInvoke (float sProgress = 0.0f, NWEOperationResult sInfos = null)
		{
            ProgressBlock?.Invoke(this, sProgress, sInfos);
        }
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Execute operation.
		/// </summary>
		public virtual void Execute ()
		{
            ProgressBlock?.Invoke(null);
            Finish ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Cancel operation.
		/// </summary>
		/// <returns><c>true</c> if this instance cancel ; otherwise, <c>false</c>.</returns>
		public virtual void Cancel ()
		{
            CancelBlock?.Invoke(null);
            IsFinish = true;
			Parent.NextOperation (QueueName);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Finish this operation.
		/// </summary>
		public virtual void Finish ()
		{
            SucessBlock?.Invoke(null);
            IsFinish = true;
			Parent.NextOperation (QueueName);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Destroy this operation.
		/// </summary>
		public virtual void DestroyThisOperation ()
		{
		}
		//-------------------------------------------------------------------------------------------------------------
	}
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
