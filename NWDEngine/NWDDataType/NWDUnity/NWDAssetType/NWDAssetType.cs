//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:28:49
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

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
using System.IO;

using UnityEngine;

using SQLite4Unity3d;

//using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField]
    public class NWDAssetType : NWDUnityType
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The asset's path protection (**xxxx/xxx/xxxx.fr**) to find the start and end of Path.
        /// beacuase if you want replace xx.trc That replace too xxxxxx.trc. With delimiter the replace become 
        /// **xx.trc** to replace and **xxxxx.trc** id protected!
        /// **AAA/   in **AAA/xx.trc** to replace and *AAA/ by */BBB is possible!
        /// </summary>
        public static string kAssetDelimiter = "**";
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Change the asset path.
        /// </summary>
        /// <returns><c>true</c>, if asset path was changed, <c>false</c> otherwise.</returns>
        /// <param name="sOldPath">S old path.</param>
        /// <param name="sNewPath">S new path.</param>
        //[NWDAliasMethod(NWDConstants.M_ChangeAssetPath)]
        public bool ChangeAssetPath(string sOldPath, string sNewPath)
        {
            //Debug.Log("NWEDataType ChangeAssetPath " + sOldPath + " to " + sNewPath + " in Value = " + Value);
            bool rChange = false;
            if (string.IsNullOrEmpty(Value))
            { }
            else
            {
                if (Value.Contains(sOldPath))
                {
                    //Value = Value.Replace (kAssetDelimiter+sOldPath+kAssetDelimiter, kAssetDelimiter+sNewPath+kAssetDelimiter);
                    Value = Value.Replace(kAssetDelimiter + sOldPath, kAssetDelimiter + sNewPath); // complient with folder change
                    rChange = true;
                    //Debug.Log("NWEDataType ChangeAssetPath YES I DID");
                }
            }
            return rChange;
        }
        //-------------------------------------------------------------------------------------------------------------
        public GameObject ToAssetAsync(GameObject sInterim, NWDOperationAssetDelegate sDelegate)
        {
            string tPath = Value.Replace(NWDAssetType.kAssetDelimiter, string.Empty);
            tPath = NWEPathResources.PathAbsoluteToPathDB(tPath);
            NWDOperationAsset tOperation = NWDOperationAsset.AddOperation(tPath, sInterim, false, sDelegate);
            return tOperation.Interim;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================