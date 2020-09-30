//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
//#define NWD_LOG
//#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
#if NWD_DEVELOPER
#if UNITY_EDITOR
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDEngineVersionEditor
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Engine version temporary reccord
        /// </summary>
        public static string Version = NWDEngineVersion.Version;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Menu string
        /// </summary>
        const string K_MENU_ENGINE_VERSION = NWDEditorMenu.K_DOCUMENTATION + "Version";
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Increment major version tag
        /// </summary>
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_MENU_ENGINE_VERSION + " management/Major number increment", false, 221)]
        public static void MajorVersion()
        {
            NWDBenchmark.Start();
            ChangeVersion(true, false, false);
            GenerateCSharpFile_Editor();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Increment minor version tag
        /// </summary>
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_MENU_ENGINE_VERSION + " management/Minor numnber increment", false, 221)]
        public static void MinorVersion()
        {
            NWDBenchmark.Start();
            ChangeVersion(false, true, false);
            GenerateCSharpFile_Editor();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Increment build tag
        /// </summary>
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_MENU_ENGINE_VERSION + " management/Build number autogenerate", false, 222)]
        public static void BuildVersion()
        {
            NWDBenchmark.Start();
            ChangeVersion(false, false, true);
            GenerateCSharpFile_Editor();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Reccord new version in <see cref="Version"/>
        /// </summary>
        /// <param name="sMajor"></param>
        /// <param name="sMinor"></param>
        /// <param name="sJustBuild"></param>
        private static void ChangeVersion(bool sMajor, bool sMinor, bool sJustBuild)
        {
            NWDBenchmark.Start();
            string tReturn = "";
            string[] tSplitArray = NWDEngineVersion.Version.Split(new char[] { '.' });
            uint tVersion = 0;
            uint tMinorVersion = 0;
            ulong tBuild = 0;
            if (tSplitArray.Length > 0)
            {
                uint.TryParse(tSplitArray[0], out tVersion);
            }
            if (tSplitArray.Length > 1)
            {
                uint.TryParse(tSplitArray[1], out tMinorVersion);
            }
            if (tSplitArray.Length > 2)
            {
                ulong.TryParse(tSplitArray[2], out tBuild);
            }
            if (sMinor == true)
            {
                tMinorVersion++;
                sJustBuild = true;
            }
            if (sMajor == true)
            {
                tVersion++;
                tMinorVersion = 0;
                sJustBuild = true;
            }
            if (sJustBuild == true)
            {
                string tDate = DateTime.Now.ToString("yyMMdd") + "00";
                ulong tBuildDate = 0;
                ulong.TryParse(tDate, out tBuildDate);
                tBuild++;
                if (tBuild < tBuildDate)
                {
                    tBuild = tBuildDate;
                }
            }
            tReturn = tReturn + tVersion.ToString() + "." + tMinorVersion.ToString("00") + "." + tBuild.ToString("00000000");
            Version = tReturn;
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Reccord <see cref="Version"/> value in CSharp file to be compiled as constant value.
        /// </summary>
        private static void GenerateCSharpFile_Editor()
        {
            NWDBenchmark.Start();
            DateTime tTime = DateTime.UtcNow;
            string tDateTimeString = NWDToolbox.DateTimeYYYYMMdd(tTime);
            string tYearString = NWDToolbox.DateTimeYYYY(tTime);
            StringBuilder rReturn = new StringBuilder(string.Empty);
            rReturn.AppendLine("//=====================================================================================================================");
            rReturn.AppendLine(NWD.K_CommentCopyright + tYearString);
            rReturn.AppendLine(NWD.K_CommentCreator);
            rReturn.AppendLine("//=====================================================================================================================");
            rReturn.AppendLine("#if NWD_VERBOSE");
            rReturn.AppendLine("#if UNITY_EDITOR");
            rReturn.AppendLine("//#define NWD_LOG");
            rReturn.AppendLine("//#define NWD_BENCHMARK");
            rReturn.AppendLine("#elif DEBUG");
            rReturn.AppendLine("//#define NWD_LOG");
            rReturn.AppendLine("//#define NWD_BENCHMARK");
            rReturn.AppendLine("#endif");
            rReturn.AppendLine("#else");
            rReturn.AppendLine("#undef NWD_LOG");
            rReturn.AppendLine("#undef NWD_BENCHMARK");
            rReturn.AppendLine("#endif");
            rReturn.AppendLine("//=====================================================================================================================");
            rReturn.AppendLine("using UnityEngine;");
            rReturn.AppendLine("//=====================================================================================================================");
            rReturn.AppendLine("namespace NetWorkedData");
            rReturn.AppendLine("{");
            rReturn.AppendLine("//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            rReturn.AppendLine("/// <summary>");
            rReturn.AppendLine("/// Just use as abstract for version reccord");
            rReturn.AppendLine("/// </summary>");
            rReturn.AppendLine("public abstract class NWDEngineVersion");
            rReturn.AppendLine("{");
            rReturn.AppendLine("//-------------------------------------------------------------------------------------------------------------");
            rReturn.AppendLine("/// <summary>");
            rReturn.AppendLine("/// Engine version hard (as constant)");
            rReturn.AppendLine("/// </summary>");
            rReturn.AppendLine("public const string Version = \"" + Version + "\";");
            rReturn.AppendLine("//-------------------------------------------------------------------------------------------------------------");
            rReturn.AppendLine("}");
            rReturn.AppendLine("//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++");
            rReturn.AppendLine("}");
            rReturn.AppendLine("//=====================================================================================================================");
            string tPathPrivate = NWDFindPackage.PathOfPackage("/NWDEngine/NWDEngineVersion/NWDEngineVersion.cs");
            string rReturnPrivateFormatted = NWDToolbox.CSharpFormat(rReturn.ToString());
            File.WriteAllText(tPathPrivate, rReturnPrivateFormatted);
            try
            {
                AssetDatabase.ImportAsset(tPathPrivate, ImportAssetOptions.ForceUpdate);
                AssetDatabase.ImportAsset(NWDToolbox.FindPrivateConfigurationFolder(), ImportAssetOptions.ForceUpdate);
            }
            catch (IOException eException)
            {
                Debug.LogException(eException);
                throw;
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
#endif
