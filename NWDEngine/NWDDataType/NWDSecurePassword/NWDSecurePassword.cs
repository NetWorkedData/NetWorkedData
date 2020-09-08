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
using System.IO;
using UnityEngine;
using System.Text;
using System.Security.Cryptography;
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEditor;
using NetWorkedData.NWDEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField]
    public class NWDSecurePassword : NWEDataType
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDSecurePassword()
        {
            Value = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDSecurePassword(string sValue = "")
        {
            if (string.IsNullOrEmpty(sValue))
            {
                Value = string.Empty;
            }
            else
            {
                Value = sValue;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void BaseVerif()
        {
            // Need to check with a new dictionary each time
            if (string.IsNullOrEmpty(Value))
            {
                Default();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
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
        public void CryptAes(string sPlainText)
        {
            Value = InternalCryptAes(sPlainText);
        }
        //-------------------------------------------------------------------------------------------------------------
        private string InternalCryptAes(string sPlainText)
        {
            string rParamB64 = null;
            if (string.IsNullOrEmpty(sPlainText) == false)
            {
                // Set AES bits size
                Int32 sAesSize = 128;
                string tKey = KeyLengthFix(NWDProjectCredentialsManagerContent.Password, 24);
                string tVector = KeyLengthFix(NWDProjectCredentialsManagerContent.VectorString, 16);
                // Encrypt the string to an array of bytes.
                byte[] sKey = Encoding.ASCII.GetBytes(tKey);
                byte[] sIV = Encoding.ASCII.GetBytes(tVector);
                Encoding.ASCII.GetBytes(tVector);
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
                // Encode parameters
                rParamB64 = Convert.ToBase64String(rEncrypted);
            }
            return rParamB64;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string Decrypt()
        {
            // Decode parameters
            string rDecrypte = null;
            if (string.IsNullOrEmpty(Value) == false)
            {
                // Set AES bits size
                Int32 sAesSize = 128;
                string tKey = KeyLengthFix(NWDProjectCredentialsManagerContent.Password, 24);
                string tVector = KeyLengthFix(NWDProjectCredentialsManagerContent.VectorString, 16);
                byte[] sPlainText = Convert.FromBase64String(Value);
                byte[] sKey = Encoding.ASCII.GetBytes(tKey);
                byte[] sIV = Encoding.ASCII.GetBytes(tVector);
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
                catch (Exception sException)
                {
                    NWDDebug.Log(sException.ToString());
                }
            }
            return rDecrypte;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight()
        {
            return NWDGUI.kLabelStyle.fixedHeight + NWDGUI.kMiniButtonStyle.fixedHeight * 2 + NWDGUI.kFieldMarge * 3;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = NWEConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            NWDSecurePassword tTemporary = new NWDSecurePassword();
            tTemporary.Value = Value;
            float tX = sPosition.x + EditorGUIUtility.labelWidth;
            float tWidth = sPosition.width - EditorGUIUtility.labelWidth;
            float tTiersWidth = Mathf.Ceil((tWidth + NWDGUI.kFieldMarge) / 2.0F);
            float tTiersWidthB = tTiersWidth - NWDGUI.kFieldMarge;
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            EditorGUI.LabelField(new Rect(sPosition.x, sPosition.y, sPosition.width, NWDGUI.kLabelStyle.fixedHeight), tContent);
            if (string.IsNullOrEmpty(NWDProjectCredentialsManagerContent.Password) == false && string.IsNullOrEmpty(NWDProjectCredentialsManagerContent.VectorString) == false)
            {
                if (string.IsNullOrEmpty(Value))
                {
                    Value = string.Empty;
                }
                string tdecode = Decrypt();
                if (string.IsNullOrEmpty(Value))
                {
                    Value = string.Empty;
                }
                if (tdecode != null || string.IsNullOrEmpty(Value))
                {
                    int tIndentLevel = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 0;
                    tdecode = EditorGUI.TextField(new Rect(tX, sPosition.y, tWidth, NWDGUI.kTextFieldStyle.fixedHeight), tdecode);
                    sPosition.y += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                    if (GUI.Button(new Rect(tX, sPosition.y, tTiersWidthB, NWDGUI.kMiniButtonStyle.fixedHeight), "Test it"))
                    {
                        NWEPassAnalyseWindow.SharedInstance().AnalyzePassword(tdecode);
                    }
                    if (GUI.Button(new Rect(tX + tTiersWidth, sPosition.y, tTiersWidthB, NWDGUI.kMiniButtonStyle.fixedHeight), "Random"))
                    {
                        tdecode = NWDToolbox.RandomStringCypher(24);
                    }
                    string tencode = InternalCryptAes(tdecode);
                    tTemporary.Value = tencode;
                    sPosition.y += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
                    if (GUI.Button(new Rect(tX, sPosition.y, tWidth, NWDGUI.kPopupStyle.fixedHeight), "Credentials window"))
                    {
                        NWDProjectCredentialsManager.SharedInstance().Show();
                        NWDProjectCredentialsManager.SharedInstance().Focus();
                    }
                    sPosition.y += NWDGUI.kMiniButtonStyle.fixedHeight;

                    EditorGUI.indentLevel = tIndentLevel;
                }
                else
                {
                    EditorGUI.LabelField(new Rect(tX, sPosition.y, tWidth, NWDGUI.kLabelStyle.fixedHeight), "Undisclosed secret");
                    sPosition.y += NWDGUI.kLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                    NWDGUI.BeginRedArea();
                    if (GUI.Button(new Rect(tX, sPosition.y, tWidth, NWDGUI.kMiniButtonStyle.fixedHeight), "Reset"))
                    {
                        tTemporary.Value = string.Empty;
                    }
                    sPosition.y += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
                    if (GUI.Button(new Rect(tX, sPosition.y, tWidth, NWDGUI.kMiniButtonStyle.fixedHeight), "Change credentials"))
                    {
                        NWDProjectCredentialsManager.SharedInstance().Show();
                        NWDProjectCredentialsManager.SharedInstance().Focus();
                    }
                    sPosition.y += NWDGUI.kMiniButtonStyle.fixedHeight;
                    NWDGUI.EndRedArea();
                }
            }
            else
            {
                EditorGUI.LabelField(new Rect(tX, sPosition.y, tWidth, NWDGUI.kLabelStyle.fixedHeight), "•••••••••••••");
                sPosition.y += NWDGUI.kLabelStyle.fixedHeight + NWDGUI.kFieldMarge;
                sPosition.y += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
                NWDGUI.BeginRedArea();
                if (GUI.Button(new Rect(tX, sPosition.y, tWidth, NWDGUI.kMiniButtonStyle.fixedHeight), "Need credentials"))
                {
                    NWDProjectCredentialsManager.SharedInstance().Show();
                    NWDProjectCredentialsManager.SharedInstance().Focus();
                }
                sPosition.y += NWDGUI.kMiniButtonStyle.fixedHeight;
                NWDGUI.EndRedArea();
            }
            return tTemporary;

            //NWDPasswordSecure tTemporary = new NWDPasswordSecure();
            //GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            //float tX = sPosition.x + EditorGUIUtility.labelWidth;
            //float tTiersWidth = Mathf.Ceil((sPosition.width - EditorGUIUtility.labelWidth + NWDGUI.kFieldMarge) / 3.0F);
            //float tTiersWidthB = tTiersWidth - NWDGUI.kFieldMarge;
            //EditorGUI.LabelField(new Rect(sPosition.x, sPosition.y, sPosition.width, NWDGUI.kLabelStyle.fixedHeight), tContent);
            //int tIndentLevel = EditorGUI.indentLevel;
            //EditorGUI.indentLevel = 0;
            //tTemporary.Value = EditorGUI.TextField(new Rect(tX, sPosition.y, tTiersWidthB, NWDGUI.kPopupStyle.fixedHeight), Value);
            //sPosition.y += NWDGUI.kPopupStyle.fixedHeight;
            //EditorGUI.LabelField(new Rect(tX, sPosition.y, tTiersWidthB, NWDGUI.kPopupStyle.fixedHeight), NWDPasswordConfigurationManager.Encode(tTemporary.Value));
            //sPosition.y += NWDGUI.kPopupStyle.fixedHeight;

            ////if (string.IsNullOrEmpty(NWDPasswordConfigurationManager.Password) == false && string.IsNullOrEmpty(NWDPasswordConfigurationManager.VectorString) == false)
            ////{
            ////    string tDecode = NWDPasswordConfigurationManager.Decode(tTemporary.Value);
            ////    tDecode = EditorGUI.TextField(new Rect(tX, sPosition.y, tTiersWidthB, NWDGUI.kPopupStyle.fixedHeight), tDecode);
            ////    if (GUI.Button(new Rect(tX + tTiersWidth * 1, sPosition.y, tTiersWidthB, NWDGUI.kPopupStyle.fixedHeight), "test"))
            ////    {
            ////        NWEPassAnalyseWindow.SharedInstance().AnalyzePassword(tDecode);
            ////    }
            ////    if (GUI.Button(new Rect(tX + tTiersWidth * 2, sPosition.y, tTiersWidthB, NWDGUI.kPopupStyle.fixedHeight), "rand"))
            ////    {
            ////        tDecode = NWDToolbox.RandomStringCypher(24);
            ////    }
            ////    tTemporary.Value = NWDPasswordConfigurationManager.Encode(tDecode);
            ////}
            ////else
            ////{
            ////    EditorGUI.PasswordField(new Rect(tX, sPosition.y, tTiersWidthB, NWDGUI.kPopupStyle.fixedHeight), tTemporary.Value);
            ////    //NWDPasswordConfigurationManager.SharedInstance().Focus();
            ////}
            //EditorGUI.indentLevel = tIndentLevel;
            //return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
