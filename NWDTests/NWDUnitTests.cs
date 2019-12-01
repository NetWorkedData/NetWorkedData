//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:24:53
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

using UnityEngine;

using SQLite4Unity3d;
//using BasicToolBox;


#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDUnitTests
    {
        //-------------------------------------------------------------------------------------------------------------
        const int kUnitTestDC = 123456789;
        const string kDescriptionMark = "For UnitTest only";
        //-------------------------------------------------------------------------------------------------------------
        public static T NewData<T>() where T : NWDTypeClass, new()
        {
            T rObject = NWDBasisHelper.NewData<T>();
            rObject.InternalKey = "UnitTest " + NWDToolbox.RandomStringCypher(8);
            rObject.InternalDescription = kDescriptionMark;
            rObject.DevSync = -1;
            rObject.PreprodSync = -1;
            rObject.ProdSync = -1;
            rObject.DC = kUnitTestDC;
            return rObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void CleanUnitTests()
        {
            List<NWDTypeClass> tToDelete = new List<NWDTypeClass>();
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeLoadedList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                foreach (NWDTypeClass tObject in tHelper.Datas)
                {
                    if (tObject.DC == kUnitTestDC
                        && tObject.DevSync == -1
                        && tObject.PreprodSync == -1
                        && tObject.ProdSync == -1
                        && tObject.InternalDescription == kDescriptionMark)
                    {
                        tToDelete.Add(tObject);
                    }
                }
            }
            foreach (NWDTypeClass tObject in tToDelete)
            {
                tObject.DeleteData(NWDWritingMode.MainThread);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================