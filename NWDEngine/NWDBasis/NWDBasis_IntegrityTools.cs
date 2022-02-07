//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;



using UnityEngine;
//using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    /// <summary>
    /// NWD basis. Integrity tools
    /// </summary>
    public partial class NWDBasis : NWDTypeClass
    {
        #region Class Methods
        //-------------------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// Determines hash sum of specified strToEncrypt.
        ///// </summary>
        ///// <returns>Integrity value</returns>
        ///// <param name="strToEncrypt">String to encrypt.</param>
        //public static string HashSum(string strToEncrypt)
        //{
        //    // force to utf-8
        //    System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        //    byte[] bytes = ue.GetBytes(strToEncrypt);
        //    // encrypt bytes
        //    System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        //    byte[] hashBytes = md5.ComputeHash(bytes);
        //    // Convert the encrypted bytes back to a string (base 16)
        //    string hashString = string.Empty;
        //    for (int i = 0; i < hashBytes.Length; i++)
        //    {
        //        hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
        //    }
        //    // remove the DataAssemblySeparator from hash (prevent error in networking transimission)
        //    hashString = hashString.Replace(NWDConstants.kStandardSeparator, string.Empty);
        //    // return value
        //    return hashString.PadLeft(24, '0');
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Recalculates all integrities for all objects in ObjectsList.
        /// </summary>
        //public static void RecalculateAllIntegrities()
        //{
        //    //loop
        //    foreach (NWDBasis tObject in BasisHelper().Datas)
        //    {
        //        // update integrity value
        //        tObject.UpdateIntegrity();
        //        // force to write object in database
        //        tObject.UpdateData();
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        #region Instance Methods
        //-------------------------------------------------------------------------------------------------------------
        public void NotNullChecker()
        {
            //Debug.Log("NWDBasis NotNullChecker()");
            Type tType = ClassType();
            //List<string> tPropertiesList = PropertiesOrderArray();
            List<string> tPropertiesList = BasisHelper().SLQIntegrityOrder();
            foreach (string tPropertieName in tPropertiesList)
            {
                PropertyInfo tProp = tType.GetProperty(tPropertieName);
                if (tProp != null)
                {
                    object tValue = tProp.GetValue(this, null);
                    if (tValue == null)
                    {
                        // What the fuck ... NWD want not null value!

                        //Debug.Log(" null detect in " + tProp.Name + " value");

                        if (tProp.PropertyType == typeof(Boolean) ||
                            tProp.PropertyType == typeof(Byte) ||
                            tProp.PropertyType == typeof(UInt16) ||
                            tProp.PropertyType == typeof(SByte) ||
                            tProp.PropertyType == typeof(Int16) ||
                            tProp.PropertyType == typeof(Int32) ||
                            tProp.PropertyType == typeof(UInt32) ||
                            tProp.PropertyType == typeof(Int64) ||
                            tProp.PropertyType == typeof(Single) ||
                            tProp.PropertyType == typeof(Double) ||
                            tProp.PropertyType == typeof(Decimal) ||
                            tProp.PropertyType == typeof(TimeSpan) ||
                            tProp.PropertyType == typeof(DateTime) ||
                            tProp.PropertyType == typeof(DateTimeOffset)
                    )
                        {
                            tProp.SetValue(this, 0, null);
#if !NETFX_CORE
                        }
                        else if (tProp.PropertyType.IsEnum)
                        {
#else
                        } else if (tProp.PropertyType.GetTypeInfo().IsEnum) {
#endif
                            tProp.SetValue(this, 0, null);
                        }
                        else if (tProp.PropertyType == typeof(byte[]) ||
                                 tProp.PropertyType == typeof(Guid) ||
                                 tProp.PropertyType == typeof(string))
                        {
                            tProp.SetValue(this, string.Empty, null);
                        }
                        else if (tProp.PropertyType.IsSubclassOf(typeof(NWEDataType)))
                        {
                            //Debug.Log("must implement "+tProp.Name + " value");
                            tValue = Activator.CreateInstance(tProp.PropertyType) as object;
                            NWEDataType tValueNWEDataType = (NWEDataType)tValue;
                            tValueNWEDataType.Default();
                            tProp.SetValue(this, tValue, null);
                        }
                        else if (tProp.PropertyType.IsSubclassOf(typeof(NWEDataTypeInt)))
                        {
                            //Debug.Log("must implement "+tProp.Name + " value");
                            tValue = Activator.CreateInstance(tProp.PropertyType) as object;
                            NWEDataTypeInt tValueNWEDataType = (NWEDataTypeInt)tValue;
                            tValueNWEDataType.Default();
                            tProp.SetValue(this, tValue, null);
                        }
                        else if (tProp.PropertyType.IsSubclassOf(typeof(NWEDataTypeFloat)))
                        {
                            //Debug.Log("must implement "+tProp.Name + " value");
                            tValue = Activator.CreateInstance(tProp.PropertyType) as object;
                            NWEDataTypeFloat tValueNWEDataType = (NWEDataTypeFloat)tValue;
                            tValueNWEDataType.Default();
                            tProp.SetValue(this, tValue, null);
                        }
                        else if (tProp.PropertyType.IsSubclassOf(typeof(NWEDataTypeEnum)))
                        {
                            //Debug.Log("must implement "+tProp.Name + " value");
                            tValue = Activator.CreateInstance(tProp.PropertyType) as object;
                            NWEDataTypeEnum tValueNWEDataType = (NWEDataTypeEnum)tValue;
                            tValueNWEDataType.Default();
                            tProp.SetValue(this, tValue, null);
                        }
                        else if (tProp.PropertyType.IsSubclassOf(typeof(NWEDataTypeMask)))
                        {
                            //Debug.Log("must implement "+tProp.Name + " value");
                            tValue = Activator.CreateInstance(tProp.PropertyType) as object;
                            NWEDataTypeMask tValueNWEDataType = (NWEDataTypeMask)tValue;
                            tValueNWEDataType.Default();
                            tProp.SetValue(this, tValue, null);
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        if (tProp.PropertyType.IsSubclassOf(typeof(NWEDataType)))
                        {
                            NWEDataType tValueNWEDataType = (NWEDataType)tValue;
                            tValueNWEDataType.BaseVerif();
                            tProp.SetValue(this, tValue, null);
                        }
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void UpdateIntegrity()
        {
            NotNullChecker();
#if NWD_INTEGRITY_NONE
            Integrity = string.Empty;
#else
            Integrity = IntegrityValue();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool IntegrityIsValid()
        {
#if NWD_INTEGRITY_NONE
            return true;
#else
            return Integrity == IntegrityValue();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public string IntegrityValue()
        {
#if NWD_INTEGRITY_NONE
            return string.Empty;
#else
            if (BasisHelper().TemplateHelper.NeedUseAccountSalt())
            {
#if UNITY_EDITOR
                string tSalt = NWDAppConfiguration.SharedInstance().SelectedEnvironment().GetAccountSalt();
                string tAccount = GetEventualAccount();
                if (tAccount != NWDAppConfiguration.SharedInstance().SelectedEnvironment().GetAccountReference())
                {
                    tSalt = string.Empty;
                    if (string.IsNullOrEmpty(tAccount) == false)
                    {
                        NWDAccount tAccountData = NWDBasisHelper.GetRawDataByReference<NWDAccount>(tAccount);
                        if (tAccountData != null)
                        {
                            tSalt = tAccountData.Salt;
                        }
                    }
                }
                return BasisHelper().HashSum(BasisHelper().SaltStart + IntegrityAssembly() + tSalt);
#else
                return BasisHelper().HashSum(BasisHelper().SaltStart + IntegrityAssembly() + NWDAppConfiguration.SharedInstance().SelectedEnvironment().GetAccountSalt());
#endif
            }
            else
            {
                return BasisHelper().HashSum(BasisHelper().SaltStart + IntegrityAssembly() + BasisHelper().SaltEnd);
            }
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
    }
}
//=====================================================================================================================
