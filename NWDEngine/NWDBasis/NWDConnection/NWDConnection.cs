//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:25:36
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

//using BasicToolBox;

using SQLite4Unity3d;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDConnection is generic class to create connection to NWDBasis generic class by object's reference.
    /// </summary>
    [Serializable]
    public class NWDConnection<K> : NWDBasisConnection where K : NWDBasis, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the object instance referenced.
        /// </summary>
        /// <returns>The object.</returns>
        public void Log()
        {
            Debug.Log(" type is " + GetType().Name + " with Generic " + NWDBasisHelper.FindTypeInfos(typeof(K)).ClassNamePHP + " and reference is " + Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the object instance referenced.
        /// </summary>
        /// <returns>The object.</returns>
        [Obsolete("Use GetReachableData()")]
        public K GetData()
        {
            return NWDBasisHelper.GetReachableDataByReference<K>(Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public K GetReachableData()
        {
            return NWDBasisHelper.GetReachableDataByReference<K>(Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public K GetRawData()
        {
            return NWDBasisHelper.GetRawDataByReference<K>(Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public K GetCorporateData()
        {
            return NWDBasisHelper.GetCorporateDataByReference<K>(Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
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