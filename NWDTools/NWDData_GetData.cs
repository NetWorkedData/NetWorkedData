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
using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    static public partial class NWDData
    {
        //-------------------------------------------------------------------------------------------------------------
        public static T NewData<T>() where T : NWDTypeClass, new()
        {
            return NWDBasisHelper.NewData<T>();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return <see cref="T"/> datas filtered by enable, trashed, integrity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] GetRawDatas<T>() where T : NWDTypeClass, new()
        {
            return NWDBasisHelper.GetRawDatas<T>();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return <see cref="T"/> datas by reference filtered by enable, trashed, integrity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetRawDataByReference<T>(string sReference, bool sTryOnDisk = false) where T : NWDTypeClass, new()
        {
            return NWDBasisHelper.GetRawDataByReference<T>(sReference, sTryOnDisk);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return <see cref="T"/> datas by internal key filtered by enable, trashed, integrity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] GetRawDatasByInternalKey<T>(string sInternalKey) where T : NWDTypeClass, new()
        {
            return NWDBasisHelper.GetRawDatasByInternalKey<T>(sInternalKey);
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    static public partial class NWDData
    {

        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return <see cref="T"/> datas filtered by current account, current gamesave and enable, trashed, integrity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] GetReachableDatas<T>() where T : NWDTypeClass, new()
        {
            return NWDBasisHelper.GetReachableDatas<T>();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return <see cref="T"/> datas by reference filtered by enable, trashed, integrity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetReachableDataByReference<T>(string sReference, bool sTryOnDisk = false) where T : NWDTypeClass, new()
        {
            return NWDBasisHelper.GetReachableDataByReference<T>(sReference, sTryOnDisk);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return <see cref="T"/> datas by internal key filtered by enable, trashed, integrity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] GetReachableDatasByInternalKey<T>(string sInternalKey) where T : NWDTypeClass, new()
        {
            return NWDBasisHelper.GetRawDatasByInternalKey<T>(sInternalKey);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    static public partial class NWDData
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return <see cref="T"/> datas filtered by account, gamesave and enable, trashed, integrity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] GetCorporateDatas<T>(string sAccountReference, NWDGameSave sGameSave = null) where T : NWDTypeClass, new()
        {
            return NWDBasisHelper.GetCorporateDatas<T>(sAccountReference, sGameSave);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return <see cref="T"/> datas by reference filtered by enable, trashed, integrity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetCorporateDataByReference<T>(string sReference, string sAccountReference, NWDGameSave sGameSave = null, bool sTryOnDisk = false) where T : NWDTypeClass, new()
        {
            return NWDBasisHelper.GetCorporateDataByReference<T>(sReference, sAccountReference, sGameSave, sTryOnDisk);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return <see cref="T"/> datas by internal key filtered by enable, trashed, integrity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] GetCorporateDatasByInternalKey<T>(string sInternalKey, string sAccountReference, NWDGameSave sGameSave = null) where T : NWDTypeClass, new()
        {
            return NWDBasisHelper.GetCorporateDatasByInternalKey<T>(sInternalKey,sAccountReference, sGameSave = null);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
