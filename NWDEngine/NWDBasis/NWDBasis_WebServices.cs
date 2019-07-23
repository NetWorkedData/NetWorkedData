//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:26:21
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
using System.IO;
using System.Reflection;
using SQLite4Unity3d;
using BasicToolBox;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public override bool IsSynchronized()
        {
            int tD = 0;
            if (NWDAppConfiguration.SharedInstance().IsDevEnvironement())
            {
                tD = DevSync;
            }
            else if (NWDAppConfiguration.SharedInstance().IsPreprodEnvironement())
            {
                tD = PreprodSync;
            }
            else if (NWDAppConfiguration.SharedInstance().IsProdEnvironement())
            {
                tD = ProdSync;
            }

            if (tD > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override int WebModelToUse()
        {
            int tWebBuildUsed = BasisHelper().LastWebBuild;
            return tWebBuildUsed;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static NWDBasis<K> SynchronizationInsertInBase(NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, string[] sDataArray)
        //{
        //    return BasisHelper().New_SynchronizationInsertInBase( sInfos,  sEnvironment, sDataArray) as NWDBasis<K>;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static NWDBasis<K> SynchronizationTryToUse(NWDOperationResult sInfos, NWDAppEnvironment sEnvironment, string sData, bool sForceToUse = false)
        //{
        //    return BasisHelper().SynchronizationTryToUse( sInfos,  sEnvironment,  sData,  sForceToUse) as NWDBasis<K>;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public static void SynchronizationFromWebService(BTBOperationBlock sSuccessBlock = null,
         BTBOperationBlock sErrorBlock = null,
         BTBOperationBlock sCancelBlock = null,
         BTBOperationBlock sProgressBlock = null,
         bool sForce = false,
         bool sPriority = false)
        {
            BasisHelper().SynchronizationFromWebService(sSuccessBlock, sErrorBlock, sCancelBlock, sProgressBlock, sForce, sPriority);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool PullFromWebService(NWDAppEnvironment sEnvironment)
        {
            return BasisHelper().PullFromWebService(sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool PullFromWebServiceForce(NWDAppEnvironment sEnvironment)
        {
            return BasisHelper().PullFromWebServiceForce(sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool SynchronizationFromWebServiceForce(NWDAppEnvironment sEnvironment)
        {
            return BasisHelper().SynchronizationFromWebServiceForce(sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool SynchronizationFromWebService(NWDAppEnvironment sEnvironment)
        {
            return BasisHelper().SynchronizationFromWebService(sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool SynchronizationFromWebServiceClean(NWDAppEnvironment sEnvironment)
        {
            return BasisHelper().SynchronizationFromWebServiceClean(sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        public static bool SynchronizationFromWebServiceSpecial(NWDAppEnvironment sEnvironment, NWDOperationSpecial sSpecial)
        {
            return BasisHelper().SynchronizationFromWebServiceSpecial( sEnvironment,  sSpecial);
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================