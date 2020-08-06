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
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        public virtual void  ErrorRegenerate()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string  AddonPhpPreCalculate(NWDAppEnvironment sEnvironment)
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string  AddonPhpPostCalculate(NWDAppEnvironment sEnvironment)
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string  AddonPhpGetCalculate(NWDAppEnvironment sEnvironment)
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string  AddonPhpSpecialCalculate(NWDAppEnvironment sEnvironment)
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string  AddonPhpEngineCalculate(NWDAppEnvironment sEnvironment)
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string  AddonPhpFunctions(NWDAppEnvironment sEnvironment)
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif