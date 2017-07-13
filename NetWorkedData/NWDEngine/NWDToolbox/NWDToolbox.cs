using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using BasicToolBox;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	public class NWDToolbox
	{
		#region class method

		//-------------------------------------------------------------------------------------------------------------
		public static string TextProtect(string sText)
		{
			string rText = sText;
			rText = rText.Replace (NWDConstants.kFieldSeparatorA, NWDConstants.kFieldSeparatorASubstitute);
			rText = rText.Replace (NWDConstants.kFieldSeparatorB, NWDConstants.kFieldSeparatorBSubstitute);
			rText = rText.Replace (NWDConstants.kFieldSeparatorC, NWDConstants.kFieldSeparatorCSubstitute);

			return rText;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static string TextUnprotect(string sText)
		{
			string rText = sText;
			rText = rText.Replace (NWDConstants.kFieldSeparatorASubstitute, NWDConstants.kFieldSeparatorA);
			rText = rText.Replace (NWDConstants.kFieldSeparatorBSubstitute, NWDConstants.kFieldSeparatorB);
			rText = rText.Replace (NWDConstants.kFieldSeparatorCSubstitute, NWDConstants.kFieldSeparatorC);
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
			rText = rText.Replace (NWDConstants.kStandardSeparator, NWDConstants.kStandardSeparatorSubstitute);

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
			rText = rText.Replace (NWDConstants.kStandardSeparatorSubstitute, NWDConstants.kStandardSeparator);
			return rText;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Bools to string.
		/// </summary>
		/// <returns>The value of sBoolean to numerical string "0" if false, "1" if true.</returns>
		/// <param name="sBoolean">If set to <c>true</c> s boolean.</param>
		public static string BoolToNumericalString (bool sBoolean)
		{
			if (sBoolean == false) {
				return "0";
			} else {
				return "1";
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Return random string with length = sLength and char random in "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -_()[]{},;:!".
		/// </summary>
		/// <returns>The string.</returns>
		/// <param name="sLength">length.</param>
		public static string RandomString (int sLength)
		{
			string rReturn = "";
			//const string tChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -_()[]{}%,?;.:!&";
			const string tChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -_()[]{},;:!";
			int tCharLenght = tChars.Length;
			while (rReturn.Length < sLength) {
				rReturn += tChars [UnityEngine.Random.Range (0, tCharLenght)];
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Return random string with length = sLength and char random in "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_".
		/// </summary>
		/// <returns>The string unix.</returns>
		/// <param name="sLength">length.</param>
		public static string RandomStringUnix (int sLength)
		{
			string rReturn = "";
			const string tChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-_";
			int tCharLenght = tChars.Length;
			while (rReturn.Length < sLength) {
				rReturn += tChars [UnityEngine.Random.Range (0, tCharLenght)];
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Clean the salt use in webservice (remove not alphanumeric char)
		/// </summary>
		/// <returns>The cleaner.</returns>
		/// <param name="sString">S string.</param>
		public static string SaltCleaner (string sString)
		{
			//Regex rgx = new Regex ("[^a-zA-Z0-9 -\\_\\(\\)\\[\\]\\{\\}\\%\\,\\?\\;\\.\\:\\!\\&]");
			Regex rgx = new Regex ("[^a-zA-Z0-9 -\\_\\(\\)\\[\\]\\{\\}\\,\\;\\:\\!]");
			return rgx.Replace (sString, "");
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Splits the string by Camel Case format.
		/// </summary>
		/// <returns>The camel case.</returns>
		/// <param name="input">Input.</param>
		public static string SplitCamelCase (string input)
		{
			string rReturn = Regex.Replace (input, "([A-Z])", " $1", RegexOptions.ECMAScript).Trim ();
			rReturn = rReturn.Replace ("_", "");
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Return timestamp from unix 1970 january 01.
		/// </summary>
		public static int Timestamp ()
		{
            //int rUnixCurrentTime = (int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            DateTime tUnixStartTime = new DateTime (1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			int rUnixCurrentTime = (int)(DateTime.UtcNow - tUnixStartTime).TotalSeconds;
			return rUnixCurrentTime;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Return DateTime from timestamp unix 1970 january 01.
		/// </summary>
		/// <returns>The timestamp to date time.</returns>
		/// <param name="sTimeStamp">timestamp.</param>
		public static DateTime TimeStampToDateTime (double sTimeStamp)
		{
            DateTime rDateTime = new DateTime (1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			rDateTime = rDateTime.AddSeconds (sTimeStamp).ToLocalTime ();
			return rDateTime;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endregion

		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Copy all folder directories to destination
		/// </summary>
		/// <param name="sFromFolder">from folder.</param>
		/// <param name="sToFolder">to folder.</param>
		/// <param name="sImport">If set to <c>true</c> import in unity.</param>
		public static void CopyFolderDirectories (string sFromFolder, string sToFolder, bool sImport = true)
		{
			Debug.Log ("copy folder from = " + sFromFolder + " to " + sToFolder);
			string[] tSubFoldersArray = AssetDatabase.GetSubFolders (sFromFolder);
			foreach (string tSubFolder in tSubFoldersArray) {
				string tSubFolderLast = tSubFolder.Replace (sFromFolder + "/", "");

				Debug.Log ("copy sub folder from = " + tSubFolder + " to " + sToFolder + "/" + tSubFolderLast);

				if (AssetDatabase.IsValidFolder (sToFolder + "/" + tSubFolderLast) == false) {
					AssetDatabase.CreateFolder (sToFolder, tSubFolderLast);
				}
				CopyFolderDirectories (sFromFolder + "/" + tSubFolderLast, sToFolder + "/" + tSubFolderLast);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Copy all folder files to destination
		/// </summary>
		/// <param name="sFromFolder">from folder.</param>
		/// <param name="sToFolder">to folder.</param>
		/// <param name="sImport">If set to <c>true</c> import in unity.</param>
		public static void CopyFolderFiles (string sFromFolder, string sToFolder, bool sImport = true)
		{
			//Debug.Log ("copy files from = " + sFromFolder + " to " + sToFolder);
			DirectoryInfo tDirectory = new DirectoryInfo (sFromFolder);
			FileInfo[] tInfo = tDirectory.GetFiles ("*.*");
			foreach (FileInfo tFile in tInfo) {
				//Debug.Log ("find file = " + tFile.Name + " with extension = " + tFile.Extension);
				string tNewPath = sToFolder + "/" + tFile.Name;
				if (File.Exists (tNewPath)) {
					File.Delete (tNewPath);
				}
				if (tFile.Extension != ".meta" && (tFile.Name != ".DS_Store")) {
					tFile.CopyTo (tNewPath);
					if (sImport == true) {
						AssetDatabase.ImportAsset (tNewPath);
					}
				}
			}
			string[] tSubFoldersArray = AssetDatabase.GetSubFolders (sFromFolder);
			foreach (string tSubFolder in tSubFoldersArray) {
				string tSubFolderLast = tSubFolder.Replace (sFromFolder + "/", "");
				if (AssetDatabase.IsValidFolder (sToFolder + "/" + tSubFolderLast) == false) {
					AssetDatabase.CreateFolder (sToFolder, tSubFolderLast);
				}
				CopyFolderFiles (sFromFolder + "/" + tSubFolderLast, sToFolder + "/" + tSubFolderLast);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Exports the folder to destinations (use for web export)
		/// </summary>
		/// <param name="sFromFolder">from folder.</param>
		/// <param name="sToFolder">to folder.</param>
		public static void ExportCopyFolderFiles (string sFromFolder, string sToFolder)
		{
			//Debug.Log ("copy files from = " + sFromFolder + " to " + sToFolder);
			DirectoryInfo tDirectory = new DirectoryInfo (sFromFolder);
			FileInfo[] tInfo = tDirectory.GetFiles ("*.*");
			foreach (FileInfo tFile in tInfo) {
				//Debug.Log ("find file = " + tFile.Name + " with extension = " + tFile.Extension);
				string tNewPath = sToFolder + "/" + tFile.Name.Replace ("dot_htaccess.txt",".htaccess");
				if (File.Exists (tNewPath)) {
					File.Delete (tNewPath);
				}
				if (tFile.Extension != ".meta" && (tFile.Name != ".DS_Store")) {
					tFile.CopyTo (tNewPath);
				}
			}
			string[] tSubFoldersArray = AssetDatabase.GetSubFolders (sFromFolder);
			foreach (string tSubFolder in tSubFoldersArray) {
				string tSubFolderLast = tSubFolder.Replace (sFromFolder + "/", "");
				if (Directory.Exists (sToFolder + "/" + tSubFolderLast) == false) {
					Directory.CreateDirectory (sToFolder + "/" + tSubFolderLast);
				}
				ExportCopyFolderFiles (sFromFolder + "/" + tSubFolderLast, sToFolder + "/" + tSubFolderLast);
			}
		}
        #endif
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Generate unique Temporary USER ID
        /// </summary>
        public static string GenerateUniqueID()
        {
            string rReturn = "";
            int tUnixCurrentTime = Timestamp();
            int tTime = tUnixCurrentTime - 1492710000;
            rReturn = "ACC-" + tTime.ToString() + "-" + UnityEngine.Random.Range(1000000, 9999999).ToString() + UnityEngine.Random.Range(1000000, 9999999).ToString() + "T";
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
        /// Generate Admin hash
        /// </summary>
        /// <param name="sAdminKey">ADMIN ID</param>
        /// <param name="sFrequence">refresh frequency</param>
        public static string GenerateAdminHash(string sAdminKey, int sFrequence)
        {
            return BTBSecurityTools.GenerateSha(sAdminKey + GenerateSALT(sFrequence), BTBSecurityShaTypeEnum.Sha1);
        }
    }
}
//=====================================================================================================================
