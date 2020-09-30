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
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDEditorCoroutine
    {
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        private struct NWDYieldConstruction
        {
            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            enum NWDEditorCoroutineDataType : byte
            {
                None = 0,
                EditorCoroutine = 1,
            }
            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            struct NWDEditorCoroutineData
            {
                public NWDEditorCoroutineDataType DataType;
                public object DataObject;
            }
            //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            NWDEditorCoroutineData ProcessorData;
            //-------------------------------------------------------------------------------------------------------------
            public void Set(object sData)
            {
                NWDBenchmark.Start();
                if (sData != ProcessorData.DataObject)
                {
                    Type tType = sData.GetType();
                    NWDEditorCoroutineDataType tDataType = NWDEditorCoroutineDataType.None;
                    if (tType == typeof(NWDEditorCoroutine))
                    {
                        tDataType = NWDEditorCoroutineDataType.EditorCoroutine;
                    }
                    ProcessorData = new NWDEditorCoroutineData { DataObject = sData, DataType = tDataType };
                }
                NWDBenchmark.Finish();
            }
            //-------------------------------------------------------------------------------------------------------------
            public bool MoveNext(IEnumerator sEnumerator)
            {
                NWDBenchmark.Start();
                bool tAdvancement = false;
                switch (ProcessorData.DataType)
                {
                    case NWDEditorCoroutineDataType.EditorCoroutine:
                        {
                            tAdvancement = (ProcessorData.DataObject as NWDEditorCoroutine).OperationIsDone;
                        }
                        break;
                    default:
                        {
                            tAdvancement = ProcessorData.DataObject == sEnumerator.Current;
                        }
                        break;
                }
                if (tAdvancement)
                {
                    ProcessorData = default(NWDEditorCoroutineData);
                    return sEnumerator.MoveNext();
                }
                NWDBenchmark.Finish();
                return true;
            }
            //-------------------------------------------------------------------------------------------------------------
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        internal WeakReference Owner;
        IEnumerator Routine;
        NWDYieldConstruction YieldConstruction;
        bool OperationIsDone;
        //-------------------------------------------------------------------------------------------------------------
        static Stack<IEnumerator> kStack = new Stack<IEnumerator>(32);
        //-------------------------------------------------------------------------------------------------------------
        internal NWDEditorCoroutine(IEnumerator sRoutine, object sOwner)
        {
            NWDBenchmark.Start();
            YieldConstruction = new NWDYieldConstruction();
            if (sOwner != null)
            {
                Owner = new WeakReference(sOwner);
            }
            else
            {
                Owner = null;
            }
            Routine = sRoutine;
            EditorApplication.update += Next;
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        internal void Next()
        {
            NWDBenchmark.Start();
            if (Owner != null && !Owner.IsAlive)
            {
                EditorApplication.update -= Next;
                return;
            }
            else
            {
                OperationIsDone = !ProcessMove(Routine);
                if (OperationIsDone)
                {
                    EditorApplication.update -= Next;
                }
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private bool ProcessMove(IEnumerator sEnumerator)
        {
            NWDBenchmark.Start();
            IEnumerator root = sEnumerator;
            while (sEnumerator.Current as IEnumerator != null)
            {
                kStack.Push(sEnumerator);
                sEnumerator = sEnumerator.Current as IEnumerator;
            }
            YieldConstruction.Set(sEnumerator.Current);
            bool result = YieldConstruction.MoveNext(sEnumerator);
            while (kStack.Count > 1)
            {
                if (!result)
                {
                    result = kStack.Pop().MoveNext();
                }
                else
                {
                    kStack.Clear();
                }
            }
            if (kStack.Count > 0 && !result && root == kStack.Pop())
            {
                result = root.MoveNext();
            }
            NWDBenchmark.Finish();
            return result;
        }
        //-------------------------------------------------------------------------------------------------------------
        internal void Stop()
        {
            NWDBenchmark.Start();
            EditorApplication.update -= Next;
            Owner = null;
            Routine = null;
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
