//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using SQLite4Unity3d;
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Change the asset path in all object of this class.
        /// </summary>
        /// <param name="sOldPath">old path.</param>
        /// <param name="sNewPath">new path.</param>
        public static void ChangeAssetPath(string sOldPath, string sNewPath)
        {
            //Debug.Log (ClassName () +" ChangeAssetPath " + sOldPath + " to " + sNewPath);
            if (AssetDependent() == true)
            {
                foreach (NWDBasis<K> tObject in NWDBasis<K>.Datas().Datas)
                {
                    tObject.ChangeAssetPathMe(sOldPath, sNewPath);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Change the asset path in this object.
        /// </summary>
        /// <param name="sOldPath">old path.</param>
        /// <param name="sNewPath">new path.</param>
        public virtual void ChangeAssetPathMe(string sOldPath, string sNewPath)
        {
            //Debug.Log (ClassName () +" ChangeAssetPathMe " + sOldPath + " to " + sNewPath);
            if (TestIntegrity() == true)
            {
                bool tUpdate = false;
                if (Preview != null)
                {
                    if (Preview.Contains(sOldPath))
                    {
                        Preview = Preview.Replace(sOldPath, sNewPath);
                        tUpdate = true;
                    }
                }
                foreach (var tProp in PropertiesAssetDependent())
                {
                    Type tTypeOfThis = tProp.PropertyType;
                    NWDAssetType tValueStruct = (NWDAssetType)tProp.GetValue(this, null);
                    if (tValueStruct != null)
                    {
                        if (tValueStruct.ChangeAssetPath(sOldPath, sNewPath))
                        {
                            tUpdate = true;
                        }
                    }
                }
                if (tUpdate == true)
                {
                    UpdateData(true, NWDWritingMode.ByDefaultLocal);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif