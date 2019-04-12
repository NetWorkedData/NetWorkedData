// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:26:0
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Reflection;

//using UnityEngine;

//#if UNITY_EDITOR
//using UnityEditor;
//#endif

//using SQLite4Unity3d;

//using BasicToolBox;

////=====================================================================================================================
//namespace NetWorkedData
//{
//	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
//	{
//        //-------------------------------------------------------------------------------------------------------------
//		static bool cFileMustBeWrite = false;
//        //-------------------------------------------------------------------------------------------------------------
//		public static void MustUpdateFile()
//        {
//            Debug.Log("NWDBasis<K> MustUpdateFile()");
//			cFileMustBeWrite=true;
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public static void ForceWriteFile()
//        {
//            Debug.Log("NWDBasis<K> ForceWriteFile()");
//            cFileMustBeWrite = true;
//            WriteFile();
//        }
//		//-------------------------------------------------------------------------------------------------------------
//		public static void WriteFile()
//		{
//            Debug.Log("NWDBasis<K> WriteFile()");
//            //BTBBenchmark.Start();
//			if (cFileMustBeWrite==true)
//			{
//                string rFinal = "";
//                Type tType = ClassType();
//                List<string> tPropertiesList = DataAssemblyPropertiesList();
//                //BTBBenchmark.Increment(ObjectsList.Count());
//			foreach(K tObject in ObjectsList)
//			{
//                    rFinal+= tObject.DataLinearization(tType, tPropertiesList, true);
//			}
//			}         
//            cFileMustBeWrite = false;
//            //BTBBenchmark.Finish();
//		}      
//        //-------------------------------------------------------------------------------------------------------------
//        static void DataEss()
//        {
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public string DataLinearization(Type tType, List<string> tPropertiesList, bool sAsssemblyAsCSV = true)
//        {
//            string rReturn = "";

//            //Debug.Log("DATA ASSEMBLY  initial count =  " + tPropertiesList.Count.ToString());
//            foreach (string tPropertieName in tPropertiesList)
//            {
//                PropertyInfo tProp = tType.GetProperty(tPropertieName);
//                if (tProp != null)
//                {
//                    Type tTypeOfThis = tProp.PropertyType;

//                    string tValueString = "";

//                    object tValue = tProp.GetValue(this, null);
//                    if (tValue == null)
//                    {
//                        tValue = "";
//                    }
//                    tValueString = tValue.ToString();
//                    if (tTypeOfThis.IsEnum)
//                    {
//                        //Debug.Log("this prop  " + tTypeOfThis.Name + " is an enum");
//                        int tInt = (int)tValue;
//                        tValueString = tInt.ToString();
//                    }
//                    if (tTypeOfThis == typeof(bool))
//                    {
//                        //Debug.Log ("REFERENCE " + Reference + " AC + " + AC + " : " + tValueString);
//                        if (tValueString == "False")
//                        {
//                            tValueString = "0";
//                        }
//                        else
//                        {
//                            tValueString = "1";
//                        }
//                    }
//                    if (sAsssemblyAsCSV == true)
//                    {
//                        rReturn += NWDToolbox.TextCSVProtect(tValueString) + NWDConstants.kStandardSeparator;
//                    }
//                    else
//                    {
//                        rReturn += NWDToolbox.TextCSVProtect(tValueString);
//                    }
//                }
//            }
//            if (sAsssemblyAsCSV == true)
//            {
//                rReturn = Reference + NWDConstants.kStandardSeparator +
//                DM + NWDConstants.kStandardSeparator +
//                DS + NWDConstants.kStandardSeparator +
//                DevSync + NWDConstants.kStandardSeparator +
//                PreprodSync + NWDConstants.kStandardSeparator +
//                ProdSync + NWDConstants.kStandardSeparator +
//                // Todo Add WebServiceVersion ?
//                //WebServiceVersion + NWDConstants.kStandardSeparator +
//                rReturn +
//                Integrity;
//                //Debug.Log("DATA ASSEMBLY  CSV count =  " + (tPropertiesList.Count+7).ToString());
//            }
//            else
//            {
//                rReturn = Reference +
//                DM +
//                rReturn;
//            }
//            return rReturn;
//        }
//		//-------------------------------------------------------------------------------------------------------------
//        public static void ReadFile()
//        {
//            Debug.Log("NWDBasis<K> ReadFile()");
//        }
//        //-------------------------------------------------------------------------------------------------------------
//	}
//    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//}
////=====================================================================================================================