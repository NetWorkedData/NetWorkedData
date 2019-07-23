//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:26:11
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        static K sFictive;
        //-------------------------------------------------------------------------------------------------------------
        public static K FictiveData()
        {
            //BTBBenchmark.Start();
            if (sFictive == null)
            {
                sFictive = NewDataWithReference("FICTIVE");
                sFictive.DeleteData();
            }
            //BTBBenchmark.Finish();
            return sFictive;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static int CSV_IndexOf(string sPropertyName, int sWebBuilt = -1)
        {
            return BasisHelper().CSV_IndexOf(sPropertyName, sWebBuilt);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string SLQSelect(int sWebBuilt = -1)
        {
            return BasisHelper().SLQSelect(sWebBuilt);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void UpdateDataFromWeb(NWDAppEnvironment sEnvironment,
                                      string[] sDataArray,
                                      NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            // Determine the default mode
            sWritingMode = NWDAppConfiguration.WritingMode(sWritingMode);
            // Force update with CVS value

            FillDataFromWeb(sEnvironment, sDataArray); // good value are inside

            BasisHelper().UpdateData(this);
            //Data waiting for queue to finish the process
            if (NWDAppEnvironment.SelectedEnvironment().ThreadPoolForce == false)
            {
                if (sWritingMode == NWDWritingMode.PoolThread)
                {
                    sWritingMode = NWDWritingMode.MainThread;
                }
                else if (sWritingMode == NWDWritingMode.QueuedPoolThread)
                {
                    sWritingMode = NWDWritingMode.QueuedMainThread;
                }
            }
            // Compare with actual State of object and force to follow the actual state
            switch (WritingState)
            {
                case NWDWritingState.Free:
                    // ok you can change the mode
                    break;
                case NWDWritingState.MainThread:
                    // strange ... not possible.
                    break;
                case NWDWritingState.MainThreadInQueue:
                    // you can use only this too because writing can be concomitant
                    sWritingMode = NWDWritingMode.QueuedMainThread;
                    Debug.LogWarning("Object can't bypass the preview writing mode. Waiting this object will free.");
                    break;
                case NWDWritingState.PoolThread:
                    // you can use only this too because writing can be concomitant
                    sWritingMode = NWDWritingMode.PoolThread;
                    Debug.LogWarning("Object can't bypass the preview writing mode. Waiting this object will free.");
                    break;
                case NWDWritingState.PoolThreadInQueue:
                    // you can use only this too because writing can be concomitant
                    sWritingMode = NWDWritingMode.QueuedPoolThread;
                    Debug.LogWarning("Object can't bypass the preview writing mode. Waiting this object will free.");
                    break;
            }
            bool tDoUpdate = true;
            switch (sWritingMode)
            {
                case NWDWritingMode.MainThread:
                    break;
                case NWDWritingMode.QueuedMainThread:
                    if (NWDDataManager.SharedInstance().kUpdateDataQueueMain.Contains(this))
                    {
                        tDoUpdate = false;
                    }
                    break;
                case NWDWritingMode.PoolThread:
                    break;
                case NWDWritingMode.QueuedPoolThread:
                    if (NWDDataManager.SharedInstance().kUpdateDataQueuePool.Contains(this))
                    {
                        tDoUpdate = false;
                    }
                    break;
            }
            if (tDoUpdate == true)
            {
                WritingLockAdd();
                WritingPending = NWDWritingPending.UpdateInMemory;
                NWDDataManager.SharedInstance().UpdateData(this, sWritingMode);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void FillDataFromWeb(NWDAppEnvironment sEnvironment, string[] sDataArray)
        {
            // TODO Determine WebService Model
            // TODO USe the good model to Update
            int tModel = BasisHelper().GetWebModelValueFromCSV(sDataArray);


            // FORCE TO ENGLISH FORMAT!
            //Thread.CurrentThread.CurrentCulture = NWDConstants.FormatCountry;

            //Debug.Log("UpdateWithCSV ref " + Reference);
            // get key order assembly of cvs
            string[] tKey = BasisHelper().PropertiesOrderArray(tModel).ToArray();
            // get values 
            string[] tValue = sDataArray;
            // Short circuit the sync date
            // not replace the date from the other environment
            //if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            //{
            //    tValue[4] = PreprodSync.ToString();
            //    tValue[5] = ProdSync.ToString();
            //}
            //else if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            //{
            //    tValue[3] = DevSync.ToString();
            //    tValue[5] = ProdSync.ToString();
            //}
            //else if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            //{
            //    tValue[3] = DevSync.ToString();
            //    tValue[4] = PreprodSync.ToString();
            //}
            // process to insertion
            Type tType = ClassType();
            for (int tI = 0; tI < tKey.Count(); tI++)
            {
                if (tValue.Count() > tI)
                {
                    PropertyInfo tPropertyInfo = tType.GetProperty(tKey[tI], BindingFlags.Public | BindingFlags.Instance);
                    if (tPropertyInfo != null)
                    {
                        Type tTypeOfThis = tPropertyInfo.PropertyType;
                        string tValueString = tValue[tI] as string;

                        if (tTypeOfThis.IsEnum)
                        {
                            // sign = (NWDAppEnvironmentPlayerStatut)Enum.Parse(typeof(NWDAppEnvironmentPlayerStatut), data["sign"].ToString(), true);
                            int tValueInsert = 0;
                            int.TryParse(tValueString, out tValueInsert);
                            tPropertyInfo.SetValue(this, tValueInsert, null);
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataType)))
                        {
                            BTBDataType tObject = Activator.CreateInstance(tTypeOfThis) as BTBDataType;
                            tObject.SetValue(tValueString);
                            tPropertyInfo.SetValue(this, tObject, null);
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeInt)))
                        {
                            BTBDataTypeInt tObject = Activator.CreateInstance(tTypeOfThis) as BTBDataTypeInt;
                            long tTemp = 0;
                            long.TryParse(tValueString, out tTemp);
                            tObject.SetLong(tTemp);
                            tPropertyInfo.SetValue(this, tObject, null);
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeFloat)))
                        {
                            BTBDataTypeFloat tObject = Activator.CreateInstance(tTypeOfThis) as BTBDataTypeFloat;
                            double tTemp = 0;
                            double.TryParse(tValueString, out tTemp);
                            tObject.SetDouble(tTemp);
                            tPropertyInfo.SetValue(this, tObject, null);
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeEnum)))
                        {
                            BTBDataTypeEnum tObject = Activator.CreateInstance(tTypeOfThis) as BTBDataTypeEnum;
                            long tTemp = 0;
                            long.TryParse(tValueString, out tTemp);
                            tObject.SetLong(tTemp);
                            tPropertyInfo.SetValue(this, tObject, null);
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeMask)))
                        {
                            BTBDataTypeMask tObject = Activator.CreateInstance(tTypeOfThis) as BTBDataTypeMask;
                            long tTemp = 0;
                            long.TryParse(tValueString, out tTemp);
                            tObject.SetLong(tTemp);
                            tPropertyInfo.SetValue(this, tObject, null);
                        }
                        // Do for Standard type
                        else if (tTypeOfThis == typeof(String) || tTypeOfThis == typeof(string))
                        {
                            tPropertyInfo.SetValue(this, tValueString, null);
                        }
                        else if (tTypeOfThis == typeof(bool))
                        {
                            bool tValueInsert = false;
                            int tTemp = 0;
                            int.TryParse(tValueString, out tTemp);
                            if (tTemp > 0)
                            {
                                tValueInsert = true;
                            }
                            tPropertyInfo.SetValue(this, tValueInsert, null);
                        }
                        else if (tTypeOfThis == typeof(int))
                        {
                            int tValueInsert = 0;
                            int.TryParse(tValueString, out tValueInsert);
                            tPropertyInfo.SetValue(this, tValueInsert, null);
                        }
                        else if (tTypeOfThis == typeof(long))
                        {
                            long tValueInsert = 0;
                            long.TryParse(tValueString, out tValueInsert);
                            tPropertyInfo.SetValue(this, tValueInsert, null);
                        }
                        else if (tTypeOfThis == typeof(float))
                        {
                            // TODO: bug with dot, comma
                            //Debug.Log("float tValueString = " + tValueString);
                            float tValueInsert = 0;
                            float.TryParse(tValueString, out tValueInsert);
                            tPropertyInfo.SetValue(this, tValueInsert, null);
                        }
                        else if (tTypeOfThis == typeof(double))
                        {
                            // TODO: bug with dot, comma
                            //Debug.Log("float tValueString = " + tValueString);
                            double tValueInsert = 0;
                            double.TryParse(tValueString, out tValueInsert);
                            tPropertyInfo.SetValue(this, tValueInsert, null);
                        }
                        else
                        {

                        }
                    }
                }
            }
            AddonUpdatedMeFromWeb();
            ReIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string CSVAssemblyHead()
        {
            return string.Join(NWDConstants.kStandardSeparator, BasisHelper().PropertiesOrderArray(-1).ToArray());
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string CSVAssembly()
        {
            string[] tValues = Assembly();
            return string.Join(NWDConstants.kStandardSeparator, tValues);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string IntegrityAssembly()
        {
            string[] tValues = Assembly();
            tValues[2] = "";
            tValues[3] = "";
            tValues[4] = "";
            tValues[5] = "";
            tValues[tValues.Length - 1] = "";
            return string.Join("", tValues);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string[] Assembly(/*int sWebBuilt = -1*/)
        {
            List<string> rReturnList = new List<string>();
            Type tType = ClassType();
            List<string> tPropertiesList = BasisHelper().PropertiesOrderArray(-1);
            foreach (string tPropertieName in tPropertiesList)
            {
                PropertyInfo tProp = tType.GetProperty(tPropertieName);
                if (tProp != null)
                {
                    Type tTypeOfThis = tProp.PropertyType;
                    string tValueString = string.Empty;
                    object tValue = tProp.GetValue(this, null);
                    if (tValue == null)
                    {
                        tValue = string.Empty;
                    }
                    else
                    {
                        if (tTypeOfThis.IsEnum)
                        {
                            int tInt = (int)tValue;
                            tValueString = tInt.ToString();
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataType)))
                        {
                            tValueString = tValue.ToString();
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeInt)))
                        {
                            tValueString = NWDToolbox.LongToString(((BTBDataTypeInt)tValue).Value);
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeFloat)))
                        {
                            tValueString = NWDToolbox.DoubleToString(((BTBDataTypeFloat)tValue).Value);
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeEnum)))
                        {
                            tValueString = NWDToolbox.LongToString(((BTBDataTypeEnum)tValue).Value);
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(BTBDataTypeMask)))
                        {
                            tValueString = NWDToolbox.LongToString(((BTBDataTypeMask)tValue).Value);
                        }
                        else if (tTypeOfThis == typeof(String) || tTypeOfThis == typeof(string))
                        {
                            tValueString = tValue.ToString();
                        }
                        else if (tTypeOfThis == typeof(bool))
                        {
                            tValueString = NWDToolbox.BoolToIntString((bool)tValue);
                        }
                        else if (tTypeOfThis == typeof(int))
                        {
                            tValueString = NWDToolbox.IntToString((int)tValue);
                        }
                        else if (tTypeOfThis == typeof(long))
                        {
                            tValueString = NWDToolbox.LongToString((long)tValue);
                        }
                        else if (tTypeOfThis == typeof(float))
                        {
                            tValueString = NWDToolbox.FloatToString((float)tValue);
                        }
                        else if (tTypeOfThis == typeof(double))
                        {
                            tValueString = NWDToolbox.DoubleToString((double)tValue);
                        }
                        else
                        {
                            tValueString = tValue.ToString();
                        }
                    }
                    tValueString = NWDToolbox.TextCSVProtect(tValueString);
                    rReturnList.Add(tValueString);
                }
            }
            return rReturnList.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================