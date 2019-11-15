//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:20
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAppEnvironment
    {
        //-------------------------------------------------------------------------------------------------------------
        public string GetRuntimeExtensionValueForKey(NWDAppEnvironmentRuntimeDefineEnum sKey)
        {
            string rReturn = string.Empty;
            if (RuntimeDefineDictionary.ContainsKey(sKey.ToLong()) == true)
            {
                rReturn = RuntimeDefineDictionary[sKey.ToLong()];
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif