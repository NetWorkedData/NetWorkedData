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
using System.Linq;
using System.Reflection;
using System.IO;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField]
	public class NWDUnityType : NWEDataType
	{
        //-------------------------------------------------------------------------------------------------------------
        public object GetMethodResult(string sMethodName, Type sClass, string sUsingName, Type[] sParamTypes = null, object[] sParamValues = null)
        {
            object rReturn = null;

            Type tAssemblyType = sClass.Assembly.GetType(sUsingName);

            sParamTypes = sParamTypes ?? new Type[] { };
            MethodInfo tMethod = tAssemblyType.GetMethod(sMethodName, sParamTypes);
            if (tMethod != null)
            {
                sParamValues = sParamValues ?? new object[] { };
                rReturn = tMethod.Invoke(null, BindingFlags.Static | BindingFlags.Public, null, sParamValues, null);
            }
            else
            {
                Debug.LogWarning("Method '" + sMethodName + "' not found!");
            }

            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
