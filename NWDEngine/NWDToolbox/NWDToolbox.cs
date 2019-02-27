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
using System.Text.RegularExpressions;

using BasicToolBox;

using UnityEngine;
using System.Text;
using System.Security.Cryptography;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDToolbox
    {
        //-------------------------------------------------------------------------------------------------------------
        #region class method
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Protect the text for the separator usage.
        /// </summary>
        /// <returns>The protect text.</returns>
        /// <param name="sText">text.</param>
        public static string TextProtect(string sText)
        {
            string rText = sText;
            rText = rText.Replace(NWDConstants.kFieldSeparatorA, NWDConstants.kFieldSeparatorASubstitute);
            rText = rText.Replace(NWDConstants.kFieldSeparatorB, NWDConstants.kFieldSeparatorBSubstitute);
            rText = rText.Replace(NWDConstants.kFieldSeparatorC, NWDConstants.kFieldSeparatorCSubstitute);
            // new adds
            rText = rText.Replace(NWDConstants.kFieldSeparatorD, NWDConstants.kFieldSeparatorDSubstitute);
            rText = rText.Replace(NWDConstants.kFieldSeparatorE, NWDConstants.kFieldSeparatorESubstitute);

            return rText;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Unprotect the text for the separator usage.
        /// </summary>
        /// <returns>The unprotect text.</returns>
        /// <param name="sText">text.</param>
        public static string TextUnprotect(string sText)
        {
            string rText = sText;
            rText = rText.Replace(NWDConstants.kFieldSeparatorASubstitute, NWDConstants.kFieldSeparatorA);
            rText = rText.Replace(NWDConstants.kFieldSeparatorBSubstitute, NWDConstants.kFieldSeparatorB);
            rText = rText.Replace(NWDConstants.kFieldSeparatorCSubstitute, NWDConstants.kFieldSeparatorC);
            // new adds
            rText = rText.Replace(NWDConstants.kFieldSeparatorDSubstitute, NWDConstants.kFieldSeparatorD);
            rText = rText.Replace(NWDConstants.kFieldSeparatorESubstitute, NWDConstants.kFieldSeparatorE);
            return rText;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Protect the text for CSV export.
        /// </summary>
        /// <returns>The CSV protect.</returns>
        /// <param name="sText">S text.</param>
        public static string TextCSVProtect(string sText)
        {
            string rText = sText;
            rText = rText.Replace(NWDConstants.kStandardSeparator, NWDConstants.kStandardSeparatorSubstitute);

            return rText;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Unprotect the text from CSV import.
        /// </summary>
        /// <returns>The CSV unprotect.</returns>
        /// <param name="sText">S text.</param>
        public static string TextCSVUnprotect(string sText)
        {
            string rText = sText;
            rText = rText.Replace(NWDConstants.kStandardSeparatorSubstitute, NWDConstants.kStandardSeparator);
            return rText;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Bools to string.
        /// </summary>
        /// <returns>The value of sBoolean to numerical string "0" if false, "1" if true.</returns>
        /// <param name="sBoolean">If set to <c>true</c> s boolean.</param>
        public static string BoolToNumericalString(bool sBoolean)
        {
            if (sBoolean == false)
            {
                return "0";
            }
            else
            {
                return "1";
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return random string with length = sLength and char random in "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -_()[]{},;:!".
        /// </summary>
        /// <returns>The string.</returns>
        /// <param name="sLength">length.</param>
        public static string RandomString(int sLength)
        {
            string rReturn = string.Empty;
            //const string tChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -_()[]{}%,?;.:!&";
            const string tChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            int tCharLenght = tChars.Length;
            while (rReturn.Length < sLength)
            {
                rReturn += tChars[UnityEngine.Random.Range(0, tCharLenght)];
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string RandomStringCypher(int sLength)
        {
            string rReturn = string.Empty;
            const string tChars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ" +
            //"()[]{}" +
            //"+=_" +
            //"#$%&" +
            //"<^>" +
            //".!?:;" +
            "0123456789" +
            "";
            int tCharLenght = tChars.Length;
            while (rReturn.Length < sLength)
            {
                rReturn += tChars[UnityEngine.Random.Range(0, tCharLenght)];
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return random string with length = sLength and char random in "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_".
        /// </summary>
        /// <returns>The string unix.</returns>
        /// <param name="sLength">length.</param>
        public static string RandomStringAlpha(int sLength)
        {
            string rReturn = string.Empty;
            const string tChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            int tCharLenght = tChars.Length;
            while (rReturn.Length < sLength)
            {
                rReturn += tChars[UnityEngine.Random.Range(0, tCharLenght)];
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return random string with length = sLength and char random in "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_".
        /// </summary>
        /// <returns>The string unix.</returns>
        /// <param name="sLength">length.</param>
        public static string RandomStringUnix(int sLength)
        {
            string rReturn = string.Empty;
            const string tChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
            int tCharLenght = tChars.Length;
            while (rReturn.Length < sLength)
            {
                rReturn += tChars[UnityEngine.Random.Range(0, tCharLenght)];
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string URLCleaner(string sString)
        {
            //Regex rgx = new Regex ("[^a-zA-Z0-9 -\\_\\(\\)\\[\\]\\{\\}\\%\\,\\?\\;\\.\\:\\!\\&]");
            Regex rgx = new Regex("[^a-zA-Z0-9-_\\:\\/\\.]");
            return rgx.Replace(sString, string.Empty);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string EmailCleaner(string sString)
        {
            //Regex rgx = new Regex ("[^a-zA-Z0-9 -\\_\\(\\)\\[\\]\\{\\}\\%\\,\\?\\;\\.\\:\\!\\&]");
            Regex rgx = new Regex("[^a-zA-Z0-9-_@\\.]");
            return rgx.Replace(sString, string.Empty);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string UnixCleaner(string sString)
        {
            //Regex rgx = new Regex ("[^a-zA-Z0-9 -\\_\\(\\)\\[\\]\\{\\}\\%\\,\\?\\;\\.\\:\\!\\&]");
            Regex rgx = new Regex("[^a-zA-Z0-9-_]");
            return rgx.Replace(sString, string.Empty);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string SaltCleaner(string sString)
        {
            //Regex rgx = new Regex ("[^a-zA-Z0-9 -\\_\\(\\)\\[\\]\\{\\}\\%\\,\\?\\;\\.\\:\\!\\&]");
            Regex rgx = new Regex("[^a-zA-Z0-9 -_\\(\\)\\[\\]\\,\\;\\:\\!\\.]");
            return rgx.Replace(sString, string.Empty);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Splits the string by Camel Case format.
        /// </summary>
        /// <returns>The camel case.</returns>
        /// <param name="input">Input.</param>
        public static string SplitCamelCase(string input)
        {
            string rReturn = Regex.Replace(input, "([A-Z])", " $1", RegexOptions.ECMAScript).Trim();
            rReturn = rReturn.Replace("_", string.Empty);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return timestamp from unix 1970 january 01.
        /// </summary>
        public static int Timestamp(DateTime sDateTime)
        {
            DateTime tUnixStartTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            int rUnixCurrentTime = (int)(sDateTime - tUnixStartTime).TotalSeconds;
            return rUnixCurrentTime;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return timestamp milliseconds from unix 1970 january 01.
        /// </summary>
        public static double TimestampMilliseconds(DateTime sDateTime)
        {
            DateTime tUnixStartTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            double rUnixCurrentTime = (double)(sDateTime - tUnixStartTime).TotalMilliseconds;
            return rUnixCurrentTime;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return timestamp from unix 1970 january 01.
        /// </summary>
        public static int Timestamp()
        {
            //int rUnixCurrentTime = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            DateTime tUnixStartTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            int rUnixCurrentTime = (int)(DateTime.UtcNow - tUnixStartTime).TotalSeconds;
            return rUnixCurrentTime;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return DateTime from timestamp unix 1970 january 01.
        /// </summary>
        /// <returns>The timestamp to date time.</returns>
        /// <param name="sTimeStamp">timestamp.</param>
        public static DateTime TimeStampToDateTime(int sTimeStamp)
        {
            DateTime rDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            rDateTime = rDateTime.AddSeconds(sTimeStamp).ToLocalTime();
            return rDateTime;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return DateTime from timestamp unix 1970 january 01.
        /// </summary>
        /// <returns>The timestamp to date time.</returns>
        /// <param name="sTimeStamp">timestamp.</param>
        public static DateTime TimeStampToDateTime(double sTimeStamp)
        {
            DateTime rDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            rDateTime = rDateTime.AddSeconds(sTimeStamp).ToLocalTime();
            return rDateTime;
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Copy all folder directories to destination
        /// </summary>
        /// <param name="sFromFolder">from folder.</param>
        /// <param name="sToFolder">to folder.</param>
        /// <param name="sImport">If set to <c>true</c> import in unity.</param>
        public static void CopyFolderDirectories(string sFromFolder, string sToFolder, bool sImport = true)
        {
            //Debug.Log ("copy folder from = " + sFromFolder + " to " + sToFolder);
            string[] tSubFoldersArray = AssetDatabase.GetSubFolders(sFromFolder);
            foreach (string tSubFolder in tSubFoldersArray)
            {
                string tSubFolderLast = tSubFolder.Replace(sFromFolder + "/", string.Empty);

                //Debug.Log ("copy sub folder from = " + tSubFolder + " to " + sToFolder + "/" + tSubFolderLast);

                if (AssetDatabase.IsValidFolder(sToFolder + "/" + tSubFolderLast) == false)
                {
                    AssetDatabase.CreateFolder(sToFolder, tSubFolderLast);
                }
                CopyFolderDirectories(sFromFolder + "/" + tSubFolderLast, sToFolder + "/" + tSubFolderLast);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Copy all folder files to destination
        /// </summary>
        /// <param name="sFromFolder">from folder.</param>
        /// <param name="sToFolder">to folder.</param>
        /// <param name="sImport">If set to <c>true</c> import in unity.</param>
        public static void CopyFolderFiles(string sFromFolder, string sToFolder, bool sImport = true)
        {
            //Debug.Log ("copy files from = " + sFromFolder + " to " + sToFolder);
            DirectoryInfo tDirectory = new DirectoryInfo(sFromFolder);
            FileInfo[] tInfo = tDirectory.GetFiles("*.*");
            foreach (FileInfo tFile in tInfo)
            {
                //Debug.Log ("find file = " + tFile.Name + " with extension = " + tFile.Extension);
                string tNewPath = sToFolder + "/" + tFile.Name;
                if (File.Exists(tNewPath))
                {
                    File.Delete(tNewPath);
                }
                if (tFile.Extension != ".meta" && (tFile.Name != ".DS_Store"))
                {
                    tFile.CopyTo(tNewPath);
                    if (sImport == true)
                    {
                        AssetDatabase.ImportAsset(tNewPath);
                    }
                }
            }
            DirectoryInfo[] tSubFoldersArray = tDirectory.GetDirectories();
            foreach (DirectoryInfo tSubFolder in tSubFoldersArray)
            {
                //Debug.Log ("find subfolder Name = " + tSubFolder.Name);
                string tSubFolderLast = tSubFolder.Name;
                if (AssetDatabase.IsValidFolder(sToFolder + "/" + tSubFolderLast) == false)
                {
                    AssetDatabase.CreateFolder(sToFolder, tSubFolderLast);
                }
                CopyFolderFiles(sFromFolder + "/" + tSubFolderLast, sToFolder + "/" + tSubFolderLast);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exports the folder to destinations (use for web export)
        /// </summary>
        /// <param name="sFromFolder">from folder.</param>
        /// <param name="sToFolder">to folder.</param>
        public static void ExportCopyFolderFiles(string sFromFolder, string sToFolder)
        {
            //Debug.Log ("copy files from = " + sFromFolder + " to " + sToFolder);
            DirectoryInfo tDirectory = new DirectoryInfo(sFromFolder);
            FileInfo[] tInfo = tDirectory.GetFiles("*.*");
            foreach (FileInfo tFile in tInfo)
            {
                //Debug.Log ("find file = " + tFile.Name + " with extension = " + tFile.Extension);
                string tNewPath = sToFolder + "/" + tFile.Name.Replace(NWD.K_DOT_HTACCESS, NWD.K_HTACCESS);
                if (File.Exists(tNewPath))
                {
                    File.Delete(tNewPath);
                }
                if (tFile.Extension != ".meta" && tFile.Name != ".DS_Store" && tFile.Name != ".xcodeproj")
                {
                    tFile.CopyTo(tNewPath);
                }
            }
            DirectoryInfo[] tSubFoldersArray = tDirectory.GetDirectories();
            foreach (DirectoryInfo tSubFolder in tSubFoldersArray)
            {
                //Debug.Log ("find subfolder Name = " + tSubFolder.Name);
                string tSubFolderLast = tSubFolder.Name;
                // if (!tSubFolderLast.Contains(".xcodeproj"))
                {
                    if (Directory.Exists(sToFolder + "/" + tSubFolderLast) == false)
                    {
                        Directory.CreateDirectory(sToFolder + "/" + tSubFolderLast);
                    }
                    ExportCopyFolderFiles(sFromFolder + "/" + tSubFolderLast, sToFolder + "/" + tSubFolderLast);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Generate unique Temporary USER ID
        /// </summary>
        public static string GenerateUniqueID()
        {
            string rReturn = string.Empty;
            int tUnixCurrentTime = Timestamp();
            int tTime = tUnixCurrentTime - 1492710000;
            rReturn = "ACC-" + tTime.ToString() + BTBConstants.K_MINUS + UnityEngine.Random.Range(1000000, 9999999).ToString() + UnityEngine.Random.Range(1000000, 9999999).ToString() + "T";
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Generate unique SALT
        /// </summary>
        /// <param name="sFrequence">refresh frequency</param>
        public static string GenerateSALT(int sFrequence)
        {
            int tUnixTimestamp = Timestamp();
            if (sFrequence <= 0 || sFrequence >= tUnixTimestamp)
            {
                sFrequence = 600;
            }
            int rSalt = (tUnixTimestamp - (tUnixTimestamp % sFrequence));
            return rSalt.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Generate unique SALT
        /// </summary>
        /// <param name="sFrequence">refresh frequency</param>
        public static string GenerateSALTOutlined(int sFrequence, int sOutline = 0)
        {
            int tUnixTimestamp = Timestamp();
            if (sFrequence <= 0 || sFrequence >= tUnixTimestamp)
            {
                sFrequence = 600;
            }
            int rSalt = (tUnixTimestamp - (tUnixTimestamp % sFrequence) - (sOutline % sFrequence));
            return rSalt.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Generate Admin hash
        /// </summary>
        /// <param name="sAdminKey">ADMIN ID</param>
        /// <param name="sFrequence">refresh frequency</param>
        public static string GenerateAdminHash(string sAdminKey, int sFrequence)
        {
            return BTBSecurityTools.GenerateSha(sAdminKey + GenerateSALT(sFrequence), BTBSecurityShaTypeEnum.Sha1);
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        public static string CSharpFormat(string sString)
        {
            StringBuilder rReturn = new StringBuilder();
            int tIndentCount = 0;
            string[] tLines = sString.Split(new string[] { "\n", "\r" }, StringSplitOptions.None);
            foreach (string tLine in tLines)
            {
                if (tLine.Contains("{"))
                {
                    tIndentCount++;
                }
                if (tLine.Contains("}"))
                {
                    tIndentCount--;
                }
                for (int i = 0; i < tIndentCount; i++)
                {
                    rReturn.Append("\t");
                }
                //rReturn.Append(tLine.Replace("\t",""));
                rReturn.Append(tLine);
                rReturn.Append(NWD.K_ReturnLine);
                if (tLine.Contains("{"))
                {
                    tIndentCount++;
                }
                if (tLine.Contains("}"))
                {
                    tIndentCount--;
                }
            }
            return rReturn.ToString().TrimEnd(new char[] { '\n', '\r' });
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string FindOwnerServerFolder()
        {
            string tPath = NWDToolbox.FindClassFolder("NWDFindOwnerServer", "NetWorkedDataServer");
            if (AssetDatabase.IsValidFolder(tPath + "/Editor") == false)
            {
                AssetDatabase.CreateFolder(tPath, "Editor");
                AssetDatabase.ImportAsset(tPath + "/Editor");
            }
            return tPath + "/Editor";
        }
        //------------------------------------------------------------------------------------------------------------- 
        public static string FindOwnerClassesFolder()
        {
            return NWDToolbox.FindClassFolder("NWDFindOwnerClasses", "NetWorkedDataClasses");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string FindOwnerConfigurationFolder()
        {
            return NWDToolbox.FindClassFolder("NWDFindOwnerConfiguration", "NetWorkedDataConfiguration");
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Find the folder with FindClassName (ScriptableObject) or create the folder and the script FindClassName to find the folder if not found.
        /// </summary>
        /// <returns>The class folder.</returns>
        /// <param name="sFindClassName">S find class name.</param>
        /// <param name="sDefaultFolder">S default folder.</param>
        public static string FindClassFolder(string sFindClassName, string sDefaultFolder)
        {
            string tEngineRoot = "Assets";
            string tFolder = sDefaultFolder;
            string tEngineRootFolder = tEngineRoot + "/" + tFolder;

            bool tFindClassesFolder = false;
            if (Type.GetType("NetWorkedData." + sFindClassName) != null)
            {
                // TODO : Change to remove invoke!
                tFindClassesFolder = true;
                Type tFindClassesType = Type.GetType("NetWorkedData." + sFindClassName);
                var tMethodInfo = tFindClassesType.GetMethod("PathOfPackage", BindingFlags.Public | BindingFlags.Static);
                if (tMethodInfo != null)
                {
                    tEngineRoot = tMethodInfo.Invoke(null, new object[] { string.Empty }) as string;
                }
                tEngineRootFolder = tEngineRoot; // root is directly the good path of final folder
            }
            if (AssetDatabase.IsValidFolder(tEngineRootFolder) == false)
            {
                AssetDatabase.CreateFolder(tEngineRoot, tFolder);
                AssetDatabase.ImportAsset(tEngineRootFolder);
            }

            // TODO : rewrite with builderstring and NWD, NWDConstants
            if (tFindClassesFolder == false)
            {
                string tFindClassesClass = string.Empty +
                                           "using System.Collections;\n" +
                                           "using System.Collections.Generic;\n" +
                                           "using System.IO;\n" +
                                           "\n" +
                                           "using UnityEngine;\n" +
                                           "\n" +
                                           "#if UNITY_EDITOR\n" +
                                           "using UnityEditor;\n" +
                                           "\n" +
                                           "//=====================================================================================================================\n" +
                                           "namespace NetWorkedData\n" +
                                           "{\n" +
                                           "	/// <summary>\n" +
                                           "	/// Find package path class.\n" +
                                           "	/// Use the ScriptableObject to find the path of this package\n" +
                                           "	/// </summary>\n" +
                                           "	public class " + sFindClassName + " : ScriptableObject\n" +
                                           "	{\n" +
                                           "		/// <summary>\n" +
                                           "		/// The script file path.\n" +
                                           "		/// </summary>\n" +
                                           "		public string ScriptFilePath;\n" +
                                           "		/// <summary>\n" +
                                           "		/// The script folder.\n" +
                                           "		/// </summary>\n" +
                                           "		public string ScriptFolder;\n" +
                                           "		/// <summary>\n" +
                                           "		/// The script folder from assets.\n" +
                                           "		/// </summary>\n" +
                                           "		public string ScriptFolderFromAssets;\n" +
                                           "		/// <summary>\n" +
                                           "		/// The shared instance.\n" +
                                           "		/// </summary>\n" +
                                           "		private static " + sFindClassName + " kSharedInstance;\n" +
                                           "		//-------------------------------------------------------------------------------------------------------------\n" +
                                           "		/// <summary>\n" +
                                           "		/// Ascencor to shared instance.\n" +
                                           "		/// </summary>\n" +
                                           "		/// <returns>The shared instance.</returns>\n" +
                                           "		public static " + sFindClassName + " SharedInstance ()\n" +
                                           "		{\n" +
                                           "			if (kSharedInstance == null) {\n" +
                                           "				kSharedInstance = ScriptableObject.CreateInstance (\"" + sFindClassName + "\") as " + sFindClassName + ";\n" +
                                           "				kSharedInstance.ReadPaths ();\n" +
                                           "			}\n" +
                                           "			return kSharedInstance; \n" +
                                           "		}\n" +
                                           "		//-------------------------------------------------------------------------------------------------------------\n" +
                                           "		/// <summary>\n" +
                                           "		/// Reads the paths.\n" +
                                           "		/// </summary>\n" +
                                           "		public void ReadPaths ()\n" +
                                           "		{\n" +
                                           "			MonoScript tMonoScript = MonoScript.FromScriptableObject (this);\n" +
                                           "			ScriptFilePath = AssetDatabase.GetAssetPath (tMonoScript);\n" +
                                           "			FileInfo tFileInfo = new FileInfo (ScriptFilePath);\n" +
                                           "			ScriptFolder = tFileInfo.Directory.ToString ();\n" +
                                           "			ScriptFolder = ScriptFolder.Replace (\"\\\\\", \"/\");\n" +
                                           "			ScriptFolderFromAssets = \"Assets\"+ScriptFolder.Replace (Application.dataPath, \"\");\n" +
                                           "		}\n" +
                                           "		//-------------------------------------------------------------------------------------------------------------\n" +
                                           "		/// <summary>\n" +
                                           "		/// Packages the path.\n" +
                                           "		/// </summary>\n" +
                                           "		/// <returns>The path.</returns>\n" +
                                           "		/// <param name=\"sAddPath\">S add path.</param>\n" +
                                           "		public static string PathOfPackage (string sAddPath=\"\")\n" +
                                           "		{\n" +
                                           "			return SharedInstance ().ScriptFolderFromAssets + sAddPath;\n" +
                                           "		}\n" +
                                           "		//-------------------------------------------------------------------------------------------------------------\n" +
                                           "	}\n" +
                                           "}\n" +
                                           "//=====================================================================================================================\n" +
                                           "#endif";
                File.WriteAllText(tEngineRootFolder + "/" + sFindClassName + ".cs", tFindClassesClass);
                // force to import this file by Unity3D
                AssetDatabase.ImportAsset(tEngineRootFolder + "/" + sFindClassName + ".cs");
            }
            return tEngineRootFolder;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------

        #endregion

        //-------------------------------------------------------------------------------------------------------------

    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
