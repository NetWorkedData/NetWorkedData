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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
//using BasicToolBox;
using NWEMiniJSON;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public delegate void NWDOperationTexture2DDelegate(Texture2D sInterim, Texture2D sResult);
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[ExecuteInEditMode]
    public class NWDOperationTexture2D : NWEOperation
    {
        //-------------------------------------------------------------------------------------------------------------
        public GameObject GameObjectToSpawn;
        public string Path;
        public Texture2D Interim;
        public Texture2D Result;
        public NWDOperationTexture2DDelegate Delegation;
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationTexture2D AddOperation(string sPath, Texture2D sInterim, bool sPriority, NWDOperationTexture2DDelegate sDelegate)
        {
            NWDOperationTexture2D rReturn = NWDOperationTexture2D.Create(sPath, sInterim, sDelegate);
            NWDDataManager.SharedInstance().AssetOperationQueue.AddOperation(rReturn, sPriority);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationTexture2D Create(string sPath, Texture2D sInterim, NWDOperationTexture2DDelegate sDelegate)
        {
            NWDOperationTexture2D rReturn = null;
            GameObject tGameObjectToSpawn = new GameObject(sPath);
#if UNITY_EDITOR
            tGameObjectToSpawn.hideFlags = HideFlags.HideAndDontSave;
#else
            tGameObjectToSpawn.transform.SetParent(NWDGameDataManager.UnitySingleton().transform);
#endif
            rReturn = tGameObjectToSpawn.AddComponent<NWDOperationTexture2D>();
            rReturn.GameObjectToSpawn = tGameObjectToSpawn;
            rReturn.Path = sPath;
            if (sInterim != null)
            {
                rReturn.Interim = Instantiate(sInterim);
            }
            rReturn.Result = null;
            rReturn.Delegation = sDelegate;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Execute()
        {
            StartCoroutine(ExecuteAsync());
        }
        //-------------------------------------------------------------------------------------------------------------
        IEnumerator ExecuteAsync()
        {
            //NWEBenchmark.Start();
            Statut = NWEOperationState.Start;
            //Debug.Log("ExecuteAsync loading for path : " + Path);
            if (!string.IsNullOrEmpty(Path))
            {
                Parent.Controller[QueueName].ActualOperation = this;
                ResourceRequest ResourceRequest = Resources.LoadAsync<Texture2D>(Path);
                //Operation progress
                Statut = NWEOperationState.InProgress;
                // Put Sync in progress
                while (!ResourceRequest.isDone)
                {
                    yield return 0;
                }
                if (ResourceRequest.asset != null)
                {
                    Result = Instantiate(ResourceRequest.asset) as Texture2D;
                }
                if (Delegation != null)
                {
                    Delegation(Interim, Result);
                }
            }
            Finish();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Cancel()
        {
            Statut = NWEOperationState.Cancel;
            IsFinish = true;
            Parent.NextOperation(QueueName);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Finish()
        {
            if (Statut == NWEOperationState.ReStart)
            {
                // I MUST RESTART THE REQUEST BECAUSE BEFORE I WAS TEMPORARY ACCOUNT
                Parent.ReplayOperation(QueueName);
            }
            else
            {
                Statut = NWEOperationState.Finish;
                IsFinish = true;
                Parent.NextOperation(QueueName);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DestroyThisOperation()
        {
            Statut = NWEOperationState.Destroy;
#if UNITY_EDITOR
            DestroyImmediate(GameObjectToSpawn);
#else
            Destroy (GameObjectToSpawn);
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================