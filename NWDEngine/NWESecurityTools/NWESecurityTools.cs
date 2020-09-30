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
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

using NWEMiniJSON;
using UnityEngine.Networking;

//=====================================================================================================================
namespace NetWorkedData
{
    #region Enums
    public enum NWESecurityShaTypeEnum
    {
        Sha1,
        Sha512,
    }
    public enum NWESecurityAesTypeEnum
    {
        Aes128,
    }
    #endregion
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// Security tools, use for SHA, AES (crypte, decrypte) and Base64 (encode, decode)
    /// All methodes are static
    /// </summary>
    /// <example>
    /// How to use:
    /// <code>
    /// //using BasicToolBox;
    /// string tEncoded = NWESecurityTools.Base64Encode("my string to encode!");
    /// </code>
    /// </example>
    public static class NWESecurityTools
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Encode string to Base64
        /// </summary>
        /// <param name="sPlainText">String to encode</param>
        /// <returns>A Base64 encoded string</returns>
        public static string Base64Encode(string sPlainText)
        {
            sPlainText = UnityWebRequest.EscapeURL(sPlainText);
            //sPlainText = WWW.EscapeURL(sPlainText); // protect special char as '+'
            var tPlainTextBytes = Encoding.UTF8.GetBytes(sPlainText); // convert to uft8 bt byte
            return Convert.ToBase64String(tPlainTextBytes, Base64FormattingOptions.None); // return the base64 text
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Decode Base64 to string
        /// </summary>
        /// <param name="sBase64EncodedData">Base64 string</param>
        /// <returns>A string decoded</returns>
        public static string Base64Decode(string sBase64EncodedData)
        {
            var tBase64EncodedBytes = Convert.FromBase64String(sBase64EncodedData);
            return Encoding.UTF8.GetString(tBase64EncodedBytes);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// With a Dictionary of string/object
        /// Step 1:
        /// JSONify the Dictionary
        /// Encode the JSON in Base64 (params)
        /// Step 2:
        /// Concate the JSON and the sKey
        /// Encode the JSON in Base 64 (sig)
        /// Add SHA by using sSha to the sig (cert) 
        /// </summary>
        /// <param name="sParam">dictionary Param</param>
        /// <param name="sKey">string Key</param>
        /// <param name="sSha">SHA</param>
        /// <returns>A new Dictionary with two keys : cert and params</returns>
        public static Dictionary<string, object> AddSha(Dictionary<string, object> sParam, string sKey, NWESecurityShaTypeEnum sSha = NWESecurityShaTypeEnum.Sha1)
        {
            // Convert Dictionary to JSon string
            string tJSon = Json.Serialize(sParam);
            // Encode parameters
            string tParamB64 = Base64Encode(tJSon);
            // Set signature
            string tSig = tJSon + sKey;
            // Encode param & sig
            string tCertB64 = Base64Encode(tSig);
            // Add sha512
            string tCert512 = GenerateSha(tCertB64, sSha);
            // Create a new dictionary
            Dictionary<string, object> rParams = new Dictionary<string, object>();
            rParams.Add("cert", tCert512);
            rParams.Add("params", tParamB64);
            return rParams;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GenerateSha(string sPlainText, NWESecurityShaTypeEnum sSha = NWESecurityShaTypeEnum.Sha1)
        {
            var data = Encoding.ASCII.GetBytes(sPlainText);
            if (sSha == NWESecurityShaTypeEnum.Sha1)
            {
                using (SHA1 shaM = new SHA1Managed())
                {
                    byte[] hash = shaM.ComputeHash(data);
                    var hashedInputStringBuilder = BitConverter.ToString(hash);
                    return hashedInputStringBuilder.Replace(NWEConstants.K_MINUS, string.Empty).ToLower();
                }
            }
            else
            {
                using (SHA512 shaM = new SHA512Managed())
                {
                    byte[] hash = shaM.ComputeHash(data);
                    var hashedInputStringBuilder = BitConverter.ToString(hash);
                    return hashedInputStringBuilder.Replace(NWEConstants.K_MINUS, string.Empty).ToLower();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string AddAes(Dictionary<string, object> sParam, string sKey, string sVector, NWESecurityAesTypeEnum sAes = NWESecurityAesTypeEnum.Aes128)
        {
            // Convert Dictionary to JSon string
            string tJSon = Json.Serialize(sParam);
            // Crypto result
            string rParamB64 = string.Empty;
            // Set AES bits size
            Int32 aesSize = 128;
            // Encrypt the string to an array of bytes.
            byte[] encrypted = InternalCryptAes(tJSon, Encoding.ASCII.GetBytes(sKey), Encoding.ASCII.GetBytes(sVector), aesSize);
            // Encode parameters
            rParamB64 = Convert.ToBase64String(encrypted);
            return rParamB64;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Dictionary<string, object> RemoveAes(string sParam, string sKey, string sVector, NWESecurityAesTypeEnum sAes = NWESecurityAesTypeEnum.Aes128)
        {
            // Decode parameters
            sParam = sParam.Replace("_", "/");
            sParam = sParam.Replace("-", "+");
            byte[] encrypted = Convert.FromBase64String(sParam);
            // Set AES bits size
            Int32 aesSize = 128;
            // Decrypt the string to an array of bytes.
            string tJSon = InternalDecryptAes(encrypted, Encoding.ASCII.GetBytes(sKey), Encoding.ASCII.GetBytes(sVector), aesSize);
            // Convert JSon string to Dictionary
            return Json.Deserialize(tJSon) as Dictionary<string, object>;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string KeyLengthFix(string sKey, int sSize)
        {
            string rReturn = null;
            if (string.IsNullOrEmpty(sKey))
            {
                sKey = string.Empty;
            }
            if (sKey.Length == sSize)
            {
                rReturn = sKey;
            }
            else if (sKey.Length > sSize)
            {
                rReturn = sKey.Substring(0, sSize);
            }
            else
            {
                rReturn = sKey;
                while (rReturn.Length < sSize)
                {
                    rReturn = rReturn + "A";
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CryptAes(string sText, string sKey, string sVector, NWESecurityAesTypeEnum sAes = NWESecurityAesTypeEnum.Aes128)
        {
            string rParamB64 = string.Empty;
            if (string.IsNullOrEmpty(sText) == false)
            {
                // Set AES bits size
                Int32 aesSize = 128;
                string tKey = KeyLengthFix(sKey, 24);
                string tVector = KeyLengthFix(sVector, 16);
                // Encrypt the string to an array of bytes.
                byte[] encrypted = InternalCryptAes(sText, Encoding.ASCII.GetBytes(tKey), Encoding.ASCII.GetBytes(tVector), aesSize);
                // Encode parameters
                rParamB64 = Convert.ToBase64String(encrypted);
            }
            return rParamB64;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string DecryptAes(string sText, string sKey, string sVector, NWESecurityAesTypeEnum sAes = NWESecurityAesTypeEnum.Aes128)
        {
            // Decode parameters
            if (string.IsNullOrEmpty(sText) == false)
            {
                // Set AES bits size
                Int32 aesSize = 128;
                string tKey = KeyLengthFix(sKey, 24);
                string tVector = KeyLengthFix(sVector, 16);
                //Debug.Log("sText = " + sText);
                byte[] encrypted = Convert.FromBase64String(sText);
                // Decrypt the string to an array of bytes.
                return InternalDecryptAes(encrypted, Encoding.ASCII.GetBytes(tKey), Encoding.ASCII.GetBytes(tVector), aesSize);
            }
            else
            {
                return string.Empty;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Generate AES
        /// </summary>
        /// <param name="sPlainText">string Text</param>
        /// <param name="sKey">array byte Key</param>
        /// <param name="sIV">array byte Vector Key</param>
        /// <param name="sAesSize">AES size</param>
        /// <returns>A new array byte crypted from sPlainText using sKey, sIV and sAesSize</returns>
        private static byte[] InternalCryptAes(string sPlainText, byte[] sKey, byte[] sIV, Int32 sAesSize)
        {
            // Check arguments.
            if (sPlainText == null || sPlainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (sKey == null || sKey.Length <= 0)
            {
                throw new ArgumentNullException("Key");
            }
            if (sIV == null || sIV.Length <= 0)
            {
                throw new ArgumentNullException("IV");
            }
            byte[] rEncrypted = null;
            using (Aes aes = new AesManaged())
            {
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.PKCS7;
                aes.KeySize = sAesSize;
                aes.BlockSize = sAesSize;
                aes.Key = sKey;
                aes.IV = sIV;
                // Create a decrytor to perform the stream transform.
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // Write all data to the stream.
                            swEncrypt.Write(sPlainText);
                        }
                        rEncrypted = msEncrypt.ToArray();
                    }
                }
            }
            return rEncrypted;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Decrypte AES
        /// </summary>
        /// <param name="sPlainText">string Text</param>
        /// <param name="sKey">array byte Key</param>
        /// <param name="sIV">array byte vector Key</param>
        /// <param name="sAesSize">AES size</param>
        /// <returns>A new string decrypted from sPlainText using sKey, sIV and sAesSize</returns>
        private static string InternalDecryptAes(byte[] sPlainText, byte[] sKey, byte[] sIV, Int32 sAesSize)
        {
            // Check arguments.
            if (sPlainText == null || sPlainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (sKey == null || sKey.Length <= 0)
            {
                throw new ArgumentNullException("Key");
            }
            if (sIV == null || sIV.Length <= 0)
            {
                throw new ArgumentNullException("IV");
            }
            string rDecrypte = null;
            try
            {
                using (Aes aes = new AesManaged())
                {
                    aes.Mode = CipherMode.ECB;
                    aes.Padding = PaddingMode.PKCS7;
                    aes.KeySize = sAesSize;
                    aes.BlockSize = sAesSize;
                    aes.Key = sKey;
                    aes.IV = sIV;
                    // Create a decrytor to perform the stream transform.
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    // Create the streams used for decryption.
                    using (MemoryStream msDecrypt = new MemoryStream(sPlainText))
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream
                                // and place them in a string.
                                rDecrypte = srDecrypt.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.ToString());
            }
            return rDecrypte;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
