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
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.IO;
using UnityEngine;
using System.Threading;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDWritingPending : int
    {
        /// <summary>
        /// Unknow pending...
        /// </summary>
        Unknow,
        /// <summary>
        /// New Object in memory. Pending writing in database.
        /// </summary>
        InsertInMemory,
        /// <summary>
        /// Updated Object in memory. Pending writing in database.
        /// </summary>
        UpdateInMemory,
        /// <summary>
        /// Deleted Object in memory. Pending writing in database.
        /// </summary>
        DeleteInMemory,
        /// <summary>
        /// Object in database. You have the same representation.
        /// </summary>
        InDatabase,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================