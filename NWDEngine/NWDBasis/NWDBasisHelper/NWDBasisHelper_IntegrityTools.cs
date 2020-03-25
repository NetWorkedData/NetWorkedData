//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:24:57
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

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        System.Text.UTF8Encoding ueEncoding = new System.Text.UTF8Encoding();
        static System.Security.Cryptography.MD5CryptoServiceProvider md5Provider = new System.Security.Cryptography.MD5CryptoServiceProvider();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Determines hash sum of specified strToEncrypt.
        /// </summary>
        /// <returns>Integrity value</returns>
        /// <param name="strToEncrypt">String to encrypt.</param>
        ///
        // TODO : read https://jacksondunstan.com/articles/3206 and change algorthyme ?!
        // TODO : read https://www.php.net/manual/fr/function.hash.php for php !?
        public string HashSum(string strToEncrypt)
        {
            // force to utf-8
            //System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
            byte[] bytes = ueEncoding.GetBytes(strToEncrypt);
            // encrypt bytes
            //System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashBytes = md5Provider.ComputeHash(bytes);
            // Convert the encrypted bytes back to a string (base 16)
            string hashString = string.Empty;
            for (int i = 0; i < hashBytes.Length; i++)
            {
                hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');
            }
            // remove the DataAssemblySeparator from hash (prevent error in networking transimission)
            hashString = hashString.Replace(NWDConstants.kStandardSeparator, string.Empty);
            // return value
            return hashString.PadLeft(24, '0');
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RecalculateAllIntegrities()
        {
            //loop
            foreach (NWDTypeClass tData in Datas)
            {
                // update integrity value
                tData.UpdateIntegrity();
                // force to write object in database
                tData.UpdateData();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================