//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

using SQLite4Unity3d;

using UnityEngine;
using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    /// <summary>
    /// NWD basis. Integrity tools
    /// </summary>
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Class Methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Determines hash sum of specified strToEncrypt.
        /// </summary>
        /// <returns>Integrity value</returns>
        /// <param name="strToEncrypt">String to encrypt.</param>
        public static string HashSum(string strToEncrypt)
        {
            // force to utf-8
            System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
            byte[] bytes = ue.GetBytes(strToEncrypt);
            // encrypt bytes
            System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashBytes = md5.ComputeHash(bytes);
            // Convert the encrypted bytes back to a string (base 16)
            string hashString = "";
            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }
            // remove the DataAssemblySeparator from hash (prevent error in networking transimission)
            hashString = hashString.Replace(NWDConstants.kStandardSeparator, "");
            // return value
            return hashString.PadLeft(24, '0');
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Recalculates all integrities for all objects in ObjectsList.
        /// </summary>
        public static void RecalculateAllIntegrities()
        {
            //loop
            foreach (NWDBasis<K> tObject in ObjectsList)
            {
                // update integrity value
                tObject.UpdateIntegrity();
                // force to write object in database
                tObject.UpdateMe();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance Methods
        //-------------------------------------------------------------------------------------------------------------
        public void NotNullChecker()
        {
            //Debug.Log("NotNullChecker()");
            Type tType = ClassType();
            List<string> tPropertiesList = PropertiesOrderArray();
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
                        } else if (tProp.PropertyType.IsEnum) {
#else
                        } else if (tProp.PropertyType.GetTypeInfo().IsEnum) {
#endif
                            tProp.SetValue(this, 0 , null);
                        }
                        else if (tProp.PropertyType == typeof(byte[]) ||
                                 tProp.PropertyType == typeof(Guid) ||
                                 tProp.PropertyType == typeof(string))
                        {
                            tProp.SetValue(this, "", null);
                        }
                        else if (tProp.PropertyType.IsSubclassOf(typeof(BTBDataType)))
                        {
                            Debug.Log("must implement "+tProp.Name + " value");
                            tValue = Activator.CreateInstance(tProp.PropertyType) as object;
                            tProp.SetValue(this, tValue, null);
                        }
                        else
                        {
                        }
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Updates the integrity. Set the integrity value of object's data in the field Integrity.
        /// </summary>
        public void UpdateIntegrity()
        {
            NotNullChecker();
            Integrity = IntegrityValue();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Tests the integrity.
        /// </summary>
        /// <returns><c>true</c>, if integrity is validated, <c>false</c> if integrity is not validate.</returns>
        public bool TestIntegrity()
        {
            bool rReturn = false;
            if (NWDAppConfiguration.SharedInstance().RowDataIntegrity == true)
            {
                // test integrity
                if (Integrity == IntegrityValue())
                {
                    rReturn = true;
                }
            }
            else
            {
                rReturn = true;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Integrity value for this object's data.
        /// </summary>
        /// <returns>The value.</returns>
        public string IntegrityValue()
        {
            //Debug.Log("DataAssembly() = "+DataAssembly()+"");
            // use salts to interfere with data assembly in the hash sum computing
            return HashSum(PrefSaltA() + DataAssembly() + PrefSaltB());
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================