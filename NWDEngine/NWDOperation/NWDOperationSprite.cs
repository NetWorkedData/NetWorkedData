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
    public delegate void NWDOperationSpriteDelegate(Sprite sInterim, Sprite sResult);
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[ExecuteInEditMode]
    public class NWDOperationSprite : NWEOperation
    {
        //-------------------------------------------------------------------------------------------------------------
        public GameObject GameObjectToSpawn;
        public string Path;
        public Sprite Interim;
        public Sprite Result;
        public NWDOperationSpriteDelegate Delegation;
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationSprite AddOperation(string sPath, Sprite sInterim, bool sPriority, NWDOperationSpriteDelegate sDelegate)
        {
            NWDOperationSprite rReturn = NWDOperationSprite.Create(sPath, sInterim, sDelegate);
            NWDDataManager.SharedInstance().AssetOperationQueue.AddOperation(rReturn, sPriority);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationSprite Create(string sPath, Sprite sInterim, NWDOperationSpriteDelegate sDelegate)
        {
            NWDOperationSprite rReturn = null;
            GameObject tGameObjectToSpawn = new GameObject(sPath);
#if UNITY_EDITOR
            tGameObjectToSpawn.hideFlags = HideFlags.HideAndDontSave;
#else
            tGameObjectToSpawn.transform.SetParent(NWDGameDataManager.UnitySingleton().transform);
#endif
            rReturn = tGameObjectToSpawn.AddComponent<NWDOperationSprite>();
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
            //NWDBenchmark.Start();
            Statut = NWEOperationState.Start;
            //Debug.Log("ExecuteAsync loading for path : " + Path);
            if (!string.IsNullOrEmpty(Path))
            {
                Parent.Controller[QueueName].ActualOperation = this;
                ResourceRequest ResourceRequest = Resources.LoadAsync<Sprite>(Path);
                //Operation progress
                Statut = NWEOperationState.InProgress;
                // Put Sync in progress
                while (!ResourceRequest.isDone)
                {
                    yield return 0;
                }
                if (ResourceRequest.asset != null)
                {
                    Result = Instantiate(ResourceRequest.asset) as Sprite;
                }
                if (Delegation != null)
                {
                    Delegation(Interim, Result);
                }
            }
            Finish();
            //NWDBenchmark.Finish();
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