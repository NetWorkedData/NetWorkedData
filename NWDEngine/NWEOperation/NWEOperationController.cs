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
	/// NWE operation controller.
	/// this classe control all NWEOPerations... each by each by  Queue
	/// </summary>
	public partial class NWEOperationController
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// All Queues controller disponible in this controller.
		/// </summary>
		public Dictionary<string,NWEOperationQueue> Controller = new Dictionary<string,NWEOperationQueue> ();
		/// <summary>
		/// The default name of the queue.
		/// </summary>
		public string DefaultQueueName = "default";
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Operation in progress.
		/// </summary>
		/// <returns><c>true</c>, if in progress was operationed, <c>false</c> otherwise.</returns>
		/// <param name="sQueueName">queue name.</param>
		public bool OperationInProgress (string sQueueName = null)
		{
            //Debug.Log ("OperationInProgress in " + sQueueName + "");
			if (sQueueName == null) {
				sQueueName = DefaultQueueName;
			}
			return Controller [sQueueName].SynchronizeInProgress;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Add an operation.
		/// </summary>
		/// <param name="sOperation">operation.</param>
		/// <param name="sPriority">If set to <c>true</c> priority.</param>
		public void AddOperation (NWEOperation sOperation, bool sPriority)
		{
           // Debug.Log ("AddOperation in sOperation.QueueName = " + sOperation.QueueName);
			if (Controller.ContainsKey (sOperation.QueueName) == false) {
				Controller.Add (sOperation.QueueName, new NWEOperationQueue ());
			}
			sOperation.Parent = this;
			sOperation.ParentQueue = Controller [sOperation.QueueName];
			if (sPriority == true) {
				if (Controller [sOperation.QueueName].Operations.Contains (sOperation)) {
					Controller [sOperation.QueueName].Operations.Remove (sOperation);
					Controller [sOperation.QueueName].Operations.Insert (0, sOperation);
				} else {
					Controller [sOperation.QueueName].Operations.Insert (0, sOperation);
				}
                //Debug.Log ("AddOperation in " + sOperation.Environment.Environment + " priority ");
            }
            else {

				if (Controller [sOperation.QueueName].Operations.Contains (sOperation) == false) {
					Controller [sOperation.QueueName].Operations.Add (sOperation);
				}
                //Debug.Log ("AddOperation in " + sOperation.Environment.Environment + " ");
            }
            sOperation.Statut = NWEOperationState.InQueue;
            //Debug.Log ("AddOperation in " + sOperation.Environment.Environment + " count  = " + Controller [sOperation.Environment].Operations.Count);

            if (Controller [sOperation.QueueName].ActualOperation == null) {
                //Debug.Log ("AddOperation in " + sOperation.Environment.Environment + " directly execute ");
                Controller[sOperation.QueueName].ActualOperation = sOperation;
				sOperation.Execute ();
				Controller [sOperation.QueueName].SynchronizeInProgress = true;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Determines whether this instance cancel operation the specified sOperation.
		/// </summary>
		/// <returns><c>true</c> if this instance cancel operation the specified sOperation; otherwise, <c>false</c>.</returns>
		/// <param name="sOperation">operation.</param>
		public void CancelOperation (NWEOperation sOperation)
		{
            //Debug.Log ("CancelOperation in " + sOperation.QueueName + "");
			if (sOperation.QueueName == null) {
				sOperation.QueueName = DefaultQueueName;
			}
			if (Controller.ContainsKey (sOperation.QueueName) == false) {
				Controller.Add (sOperation.QueueName, new NWEOperationQueue ());
			}
			if (Controller [sOperation.QueueName].ActualOperation == sOperation) {
				Controller [sOperation.QueueName].ActualOperation.Cancel ();
				Controller [sOperation.QueueName].Operations.Remove (sOperation);
				Controller [sOperation.QueueName].ActualOperation.DestroyThisOperation ();
			} else {
				Controller [sOperation.QueueName].Operations.Remove (sOperation);
				Controller [sOperation.QueueName].ActualOperation.DestroyThisOperation ();
			}
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReplayOperation(string sQueueName = null)
        {
            if (Controller[sQueueName].ActualOperation != null)
            {
                Controller[sQueueName].ActualOperation.Execute();
            }
        }
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Next operation.
		/// </summary>
		/// <param name="sQueueName">S queue name.</param>
		public void NextOperation (string sQueueName = null)
		{
            //Debug.Log ("NextOperation in " + sQueueName + "");
			if (sQueueName == null) {
				sQueueName = DefaultQueueName;
			}
			if (Controller.ContainsKey (sQueueName) == false) {
				Controller.Add (sQueueName, new NWEOperationQueue ());
			}
			bool tOk = false;
			if (Controller [sQueueName].ActualOperation == null) {
				tOk = true;
                //Debug.Log ("NextOperation in " + sEnvironment.Environment + " no actaul operation!");
            }
            else {
                //Debug.Log ("NextOperation in " + sEnvironment.Environment + " test if old operation is finish");
                if (Controller [sQueueName].ActualOperation.IsFinish == true) {
                    //Debug.Log ("NextOperation in " + sEnvironment.Environment + " before remove count  = " + Controller [sEnvironment].Operations.Count);
                    Controller[sQueueName].Operations.Remove (Controller [sQueueName].ActualOperation);
                    //Debug.Log ("NextOperation in " + sEnvironment.Environment + " after remove count  = " + Controller [sEnvironment].Operations.Count);
                    Controller[sQueueName].ActualOperation.DestroyThisOperation ();
					Controller [sQueueName].ActualOperation = null;
					tOk = true;
                    //Debug.Log ("NextOperation in " + sEnvironment.Environment + " old operation is destroy! count  = " + Controller [sEnvironment].Operations.Count);
                }
            }

            //Debug.Log ("NextOperation in " + sEnvironment.Environment + " count  = " + Controller [sEnvironment].Operations.Count);
            if (tOk == true) {
				if (Controller [sQueueName].Operations.Count > 0) {

					Controller [sQueueName].ActualOperation = Controller [sQueueName].Operations [0];
					Controller [sQueueName].ActualOperation.Execute ();
                    //Debug.Log ("NextOperation in " + sEnvironment.Environment + " new actual operation is :" + Controller [sEnvironment].ActualOperation.name);
                }
                else {
					Controller [sQueueName].SynchronizeInProgress = false;
				}
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Infos about specified Queue.
		/// </summary>
		/// <param name="sQueueName">S queue name.</param>
		public void Infos (string sQueueName = null)
		{
            //Debug.Log ("Infos in " + sQueueName + "");
			if (sQueueName == null) {
				sQueueName = DefaultQueueName;
			}
			if (Controller.ContainsKey (sQueueName) == false) {
				Controller.Add (sQueueName, new NWEOperationQueue ());
			}
            //Debug.Log ("Infos in " + sEnvironment.Environment + " count  = " + Controller [sEnvironment].Operations.Count);
            if (Controller [sQueueName].ActualOperation != null) {
                //Debug.Log ("Infos in " + sEnvironment.Environment + " operation in progress = " + Controller [sEnvironment].ActualOperation.name);
            }
            else {
                //Debug.Log ("Infos in " + sEnvironment.Environment + " No operation in progress");
            }
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Flush the specified Queue.
		/// </summary>
		/// <param name="sQueueName">S queue name.</param>
		public void Flush (string sQueueName = null)
		{
            //Debug.Log ("Flush in " + sQueueName + "");
			if (sQueueName == null) {
				sQueueName = DefaultQueueName;
			}
			if (Controller.ContainsKey (sQueueName) == false) {
				Controller.Add (sQueueName, new NWEOperationQueue ());
			}
			foreach (NWEOperation tOperation in Controller [sQueueName].Operations) {
				tOperation.DestroyThisOperation ();
			}
			Controller [sQueueName].Operations.RemoveAll (RemoveAllPredicate);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Removes all predicate.
		/// </summary>
		/// <returns><c>true</c>, if all predicate was removed, <c>false</c> otherwise.</returns>
		/// <param name="sOperation">S operation.</param>
		bool RemoveAllPredicate (NWEOperation sOperation)
		{
			return true; 
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
