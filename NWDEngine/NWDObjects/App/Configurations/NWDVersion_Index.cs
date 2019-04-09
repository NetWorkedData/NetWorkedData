//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
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

using BasicToolBox;

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
    public partial class NWDVersion : NWDBasis<NWDVersion>
    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWDVersion FindMaxVersionByEnvironment(NWDAppEnvironment sEnvironment)
        {
            //Debug.Log("NWDVersion FindMaxVersionByEnvironment()");
            NWDVersion tVersion = null;
            string tVersionString = "0.00.00";
            int tVersionInt = 0;
            int.TryParse(tVersionString.Replace(".", string.Empty), out tVersionInt);
            if (NWDVersion.BasisHelper() != null)
            {
                foreach (NWDVersion tVersionObject in NWDVersion.BasisHelper().Datas)
                {
                    if (tVersionObject.TestIntegrity() == true && tVersionObject.AC == true && tVersionObject.Buildable == true)
                    {
                        if ((NWDAppConfiguration.SharedInstance().DevEnvironment == sEnvironment && tVersionObject.ActiveDev == true) ||
                            (NWDAppConfiguration.SharedInstance().PreprodEnvironment == sEnvironment && tVersionObject.ActivePreprod == true) ||
                            (NWDAppConfiguration.SharedInstance().ProdEnvironment == sEnvironment && tVersionObject.ActiveProd == true))
                        {
                            int tVersionInteger = 0;
                            int.TryParse(tVersionObject.Version.ToString().Replace(".", string.Empty), out tVersionInteger);
                            if (tVersionInt < tVersionInteger)
                            {
                                tVersionInt = tVersionInteger;
                                tVersionString = tVersionObject.Version.ToString();
                                tVersion = tVersionObject;
                            }
                        }
                    }
                }
            }
            return tVersion;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDVersion Current()
        {
            //Debug.Log("NWDVersion Current()");
            return CurrentByEnvironment(NWDAppEnvironment.SelectedEnvironment());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDVersion CurrentByEnvironment(NWDAppEnvironment sEnvironment)
        {
            //Debug.Log("NWDVersion CurrentByEnvironment()");
            NWDVersion tVersion = null;
            foreach (NWDVersion tVersionObject in NWDVersion.BasisHelper().Datas)
            {
                if (tVersionObject.TestIntegrity() == true && tVersionObject.AC == true && tVersionObject.Buildable == true)
                {
                    if ((NWDAppConfiguration.SharedInstance().DevEnvironment == sEnvironment && tVersionObject.ActiveDev == true) ||
                        (NWDAppConfiguration.SharedInstance().PreprodEnvironment == sEnvironment && tVersionObject.ActivePreprod == true) ||
                        (NWDAppConfiguration.SharedInstance().ProdEnvironment == sEnvironment && tVersionObject.ActiveProd == true))
                    {
                        if (Application.version == tVersionObject.Version.ToString())
                        {
                            tVersion = tVersionObject;
                        }
                    }
                }
            }
            return tVersion;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================