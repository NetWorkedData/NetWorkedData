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
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDConnection is generic abstract class to create connection to NWDBasis generic class by object's reference.
    /// </summary>
    [Serializable]
    public abstract class NWDConnection<K> : NWDBasisConnection where K : NWDBasis, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the object instance referenced.
        /// </summary>
        /// <returns>The object.</returns>
        public void Log()
        {
            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(typeof(K));
            if (tHelper != null)
            {
                K tObject = (K)tHelper.GetDataByReference(Reference);
                if (tObject == null)
                {
                    Debug.Log(" type is " + GetType().Name + " with Generic " + NWDBasisHelper.FindTypeInfos(typeof(K)).ClassNamePHP + " and reference is " + Reference + " BUT IS NULL");
                }
                else
                {
                    Debug.Log(" type is " + GetType().Name + " with Generic " + NWDBasisHelper.FindTypeInfos(typeof(K)).ClassNamePHP + " and reference is " + Reference + " AND EXISTS");
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the object instance referenced.
        /// </summary>
        /// <returns>The object.</returns>
        public K GetRawData()
        {
            return NWDBasisHelper.GetRawDataByReference<K>(Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the object instance referenced if it's corporate by account.
        /// </summary>
        /// <param name="sAccountReference"></param>
        /// <returns></returns>
        public K GetCorporateData(string sAccountReference)
        {
            return NWDBasisHelper.GetCorporateDataByReference<K>(Reference, sAccountReference) as K;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the object instance referenced if it's reacheable by current account.
        /// </summary>
        /// <returns></returns>
        public K GetReachableData()
        {
            return NWDBasisHelper.GetReachableDataByReference<K>(Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        /// <summary>
        /// Get the object instance referenced for the editor mode.
        /// </summary>
        /// <returns></returns>
        public K GetEditorData()
        {
            return NWDBasisHelper.GetEditorDataByReference<K>(Reference);
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Set the object instance by its reference.
        /// </summary>
        /// <param name="sData">S object.</param>
        public void SetData(K sData)
        {
            if (sData != null)
            {
                Reference = sData.Reference;
            }
            else
            {
                Reference = string.Empty;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Object instance creation and reference it automatically.
        /// </summary>
        /// <returns>The object.</returns>
        public K NewData(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            K tData = NWDBasisHelper.NewData<K>(sWritingMode);
            Reference = tData.Reference;
            return tData;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================