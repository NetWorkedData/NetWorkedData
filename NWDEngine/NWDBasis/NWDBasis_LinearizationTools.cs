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
//using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    public partial class NWDBasis : NWDTypeClass
    {
        //-------------------------------------------------------------------------------------------------------------
        //public static K FictiveData()
        //{
        //    return NWDBasisHelper.FictiveData<NWDBasisHelper><K>();
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public static int CSV_IndexOf(string sPropertyName, int sWebBuilt = -1)
        //{
        //    return BasisHelper()NWDBasisHelper.CSV_IndexOf<>(sPropertyName, sWebBuilt);
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public static string SLQSelect(int sWebBuilt = -1)
        //{
        //    return BasisHelper().SLQSelect(sWebBuilt);
        //}
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
            int tModel = BasisHelper().GetWebModelValueFromCSV(sDataArray);
            // FORCE TO ENGLISH FORMAT!
            string[] tKey = BasisHelper().PropertiesOrderArray(tModel).ToArray();
            // get values 
            string[] tValue = sDataArray;
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
                            int tValueInsert = 0;
                            int.TryParse(tValueString, out tValueInsert);
                            tPropertyInfo.SetValue(this, tValueInsert, null);
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataType)))
                        {
                            NWEDataType tObject = Activator.CreateInstance(tTypeOfThis) as NWEDataType;
                            tObject.SetValue(tValueString);
                            tPropertyInfo.SetValue(this, tObject, null);
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeInt)))
                        {
                            NWEDataTypeInt tObject = Activator.CreateInstance(tTypeOfThis) as NWEDataTypeInt;
                            long tTemp = 0;
                            long.TryParse(tValueString, out tTemp);
                            tObject.SetLong(tTemp);
                            tPropertyInfo.SetValue(this, tObject, null);
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeFloat)))
                        {
                            NWEDataTypeFloat tObject = Activator.CreateInstance(tTypeOfThis) as NWEDataTypeFloat;
                            double tTemp = 0;
                            double.TryParse(tValueString, out tTemp);
                            tObject.SetDouble(tTemp);
                            tPropertyInfo.SetValue(this, tObject, null);
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeEnum)))
                        {
                            NWEDataTypeEnum tObject = Activator.CreateInstance(tTypeOfThis) as NWEDataTypeEnum;
                            long tTemp = 0;
                            long.TryParse(tValueString, out tTemp);
                            tObject.SetLong(tTemp);
                            tPropertyInfo.SetValue(this, tObject, null);
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeMask)))
                        {
                            NWEDataTypeMask tObject = Activator.CreateInstance(tTypeOfThis) as NWEDataTypeMask;
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
                            float tValueInsert = 0;
                            float.TryParse(tValueString, out tValueInsert);
                            tPropertyInfo.SetValue(this, tValueInsert, null);
                        }
                        else if (tTypeOfThis == typeof(double))
                        {
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
            IndexInMemory();
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
        // TODO Override compiler?
        public virtual string IntegrityAssembly()
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
        // TODO Override compiler?
        public virtual string[] Assembly(/*int sWebBuilt = -1*/)
        {
            List<string> rReturnList = new List<string>();
            Type tType = ClassType();
            //List<string> tPropertiesList = BasisHelper().PropertiesOrderArray(WebModel);
            string[] tPropertiesList = BasisHelper().PropertiesOrderArray(WebModel).ToArray();
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
                        else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataType)))
                        {
                            tValueString = tValue.ToString();
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeInt)))
                        {
                            tValueString = NWDToolbox.LongToString(((NWEDataTypeInt)tValue).Value);
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeFloat)))
                        {
                            tValueString = NWDToolbox.DoubleToString(((NWEDataTypeFloat)tValue).Value);
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeEnum)))
                        {
                            tValueString = NWDToolbox.LongToString(((NWEDataTypeEnum)tValue).Value);
                        }
                        else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeMask)))
                        {
                            tValueString = NWDToolbox.LongToString(((NWEDataTypeMask)tValue).Value);
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