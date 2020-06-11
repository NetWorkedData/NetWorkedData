//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public abstract partial class NWDApp
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Use to restaure configuration of your App: never override the NetWorkedData engine will do it for you!
        /// </summary>
        /// <returns></returns>
        public virtual bool RestaureConfigurations()
        {
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Use to restaure configuration of your App: never override the NetWorkedData engine will do it for you!
        /// </summary>
        /// <returns></returns>
        public virtual bool RestaureTypesConfigurations()
        {
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Use to do some operation after engine was launched and datas were loaded. You can override in public partial class NWDAppConfiguration 
        /// </summary>
        /// <returns></returns>
        public virtual void Loaded()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
