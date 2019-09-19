
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// 
    /// </summary>
    public static class NWEPathResources
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string PathAbsoluteToPathDB(string tPath)
        {
            string rReturn = tPath;
            string[] tPathArray = rReturn.Split(new string[] { "Resources/" }, StringSplitOptions.RemoveEmptyEntries);
            if (tPathArray.Length > 0)
            {
                rReturn = tPathArray[tPathArray.Length - 1];
            }
            tPathArray = rReturn.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            if (tPathArray.Length > 0)
            {
                rReturn = tPathArray[0];
            }
            else
            {
                rReturn = string.Empty;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================