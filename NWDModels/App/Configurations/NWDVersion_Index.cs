//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:49
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
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using SQLite4Unity3d;

using ZXing;
using ZXing.QrCode;
using UnityEngine.Networking;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDVersion : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return the Max Version Data in database.
        /// </summary>
        /// <returns></returns>
        public static NWDVersion SelectMaxRecheableData()
        {
            NWDVersion tVersion = null;
            int tVersionInt = -1;
            if (NWDBasisHelper.BasisHelper<NWDVersion>() != null)
            {
                foreach (NWDVersion tVersionObject in NWDBasisHelper.BasisHelper<NWDVersion>().Datas)
                {
                    if (tVersionInt < tVersionObject.Version.ToInt())
                    {
                        tVersionInt = tVersionObject.Version.ToInt();
                        tVersion = tVersionObject;
                    }
                }
            }
            return tVersion;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return the Max Version Data in database for specific environment, active, buildable and valid.
        /// </summary>
        /// <param name="sEnvironment"></param>
        /// <returns></returns>
        public static NWDVersion SelectMaxRecheableDataForEnvironment(NWDAppEnvironment sEnvironment)
        {
            NWDVersion tVersion = null;
            int tVersionInt = -1;
            if (NWDBasisHelper.BasisHelper<NWDVersion>() != null)
            {
                foreach (NWDVersion tVersionObject in NWDBasisHelper.BasisHelper<NWDVersion>().Datas)
                {
                    if (tVersionObject.IntegrityIsValid() == true && tVersionObject.AC == true && tVersionObject.Buildable == true)
                    {
                        if ((NWDAppConfiguration.SharedInstance().DevEnvironment == sEnvironment && tVersionObject.ActiveDev == true) ||
                            (NWDAppConfiguration.SharedInstance().PreprodEnvironment == sEnvironment && tVersionObject.ActivePreprod == true) ||
                            (NWDAppConfiguration.SharedInstance().ProdEnvironment == sEnvironment && tVersionObject.ActiveProd == true))
                        {
                            if (tVersionInt < tVersionObject.Version.ToInt())
                            {
                                tVersionInt = tVersionObject.Version.ToInt();
                                tVersion = tVersionObject;
                            }
                        }
                    }
                }
            }
            return tVersion;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the current Version for selected environment, active, buildable and valid.
        /// </summary>
        /// <returns></returns>
        public static NWDVersion CurrentData()
        {
            return SelectMaxRecheableDataForEnvironment(NWDAppEnvironment.SelectedEnvironment());
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the current Version for specific environment, active, buildable and valid.
        /// </summary>
        /// <param name="sEnvironment"></param>
        /// <returns></returns>
        public static NWDVersion CurrentData(NWDAppEnvironment sEnvironment)
        {
            return SelectMaxRecheableDataForEnvironment(sEnvironment);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the current Version for selected environment, active, buildable and valid.
        /// </summary>
        /// <param name="sEnvironment"></param>
        /// <returns></returns>
        public static List<NWDVersion> SelectReacheableDatasForEnvironment(NWDAppEnvironment sEnvironment)
        {
            List<NWDVersion> rReturn = new List<NWDVersion>();
            foreach (NWDVersion tVersionObject in NWDBasisHelper.BasisHelper<NWDVersion>().Datas)
            {
                if (tVersionObject.IntegrityIsValid() == true && tVersionObject.AC == true && tVersionObject.Buildable == true)
                {
                    if ((NWDAppConfiguration.SharedInstance().DevEnvironment == sEnvironment && tVersionObject.ActiveDev == true) ||
                        (NWDAppConfiguration.SharedInstance().PreprodEnvironment == sEnvironment && tVersionObject.ActivePreprod == true) ||
                        (NWDAppConfiguration.SharedInstance().ProdEnvironment == sEnvironment && tVersionObject.ActiveProd == true))
                    {
                        if (Application.version == tVersionObject.Version.ToString())
                        {
                            rReturn.Add(tVersionObject);
                        }
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================