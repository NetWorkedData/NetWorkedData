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
#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDataManager
    {
        //-------------------------------------------------------------------------------------------------------------
        public void CreateErrorsAndMessagesAllClasses()
        {
            NWDBenchmark.Start();
            string tProgressBarTitle = "NetWorkedData Create error";
            float tCountClass = ClassTypeList.Count + 1;
            float tOperation = 1;
            EditorUtility.DisplayProgressBar(tProgressBarTitle, "Create general error index", tOperation / tCountClass);
            tOperation++;
            foreach (Type tType in ClassTypeList)
            {
                EditorUtility.DisplayProgressBar(tProgressBarTitle, "Create " + tType.Name + " errors and messages", tOperation / tCountClass);
                tOperation++;
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                tHelper.ErrorRegenerate();
            }
            NWDDataManager.SharedInstance().DataQueueExecute();
            EditorUtility.DisplayProgressBar(tProgressBarTitle, "Finish", 1.0F);
            EditorUtility.ClearProgressBar();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CreatePHPAllClass(NWDAppEnvironment sEnvironment ,bool sIncrement = true, bool sWriteOnDisk = true)
        {
            NWDBenchmark.Start();
            if (sIncrement == true)
            {
                NWDAppConfiguration.SharedInstance().WebBuildMax++;
                NWDAppConfiguration.SharedInstance().WebBuild = NWDAppConfiguration.SharedInstance().WebBuildMax;
                NWDAppConfiguration.SharedInstance().WSList.Add(NWDAppConfiguration.SharedInstance().WebBuildMax, true);
            }
            else
            {
                string tWebServiceFolder = NWDAppConfiguration.SharedInstance().WebServiceFolder();
                string tOwnerServerFolderPath = NWDToolbox.FindOwnerServerFolder();
                if (AssetDatabase.IsValidFolder(tOwnerServerFolderPath + "/" + tWebServiceFolder) == false)
                {
                    AssetDatabase.DeleteAsset(tOwnerServerFolderPath);
                }
            }
            sEnvironment.CreatePHP(NWDDataManager.SharedInstance().ClassTypeList, true, sWriteOnDisk);
            NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppEnvironment.SelectedEnvironment());
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CreatePHPAllClass(bool sIncrement = true, bool sWriteOnDisk = true)
        {
            NWDBenchmark.Start();
            if (sIncrement == true)
            {
                NWDAppConfiguration.SharedInstance().WebBuildMax++;
                NWDAppConfiguration.SharedInstance().WebBuild = NWDAppConfiguration.SharedInstance().WebBuildMax;
                NWDAppConfiguration.SharedInstance().WSList.Add(NWDAppConfiguration.SharedInstance().WebBuildMax, true);
            }
            else
            {
                string tWebServiceFolder = NWDAppConfiguration.SharedInstance().WebServiceFolder();
                string tOwnerServerFolderPath = NWDToolbox.FindOwnerServerFolder();
                if (AssetDatabase.IsValidFolder(tOwnerServerFolderPath + "/" + tWebServiceFolder) == false)
                {
                    AssetDatabase.DeleteAsset(tOwnerServerFolderPath);
                }
            }
            foreach (NWDAppEnvironment tEnvironement in NWDAppConfiguration.SharedInstance().AllEnvironements())
            {
                tEnvironement.CreatePHP(NWDDataManager.SharedInstance().ClassTypeList, true, sWriteOnDisk);
            }
            NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppEnvironment.SelectedEnvironment());
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ModelResetAllClass()
        {
            NWDBenchmark.Start();
            string tProgressBarTitle = "NetWorkedData Models Resets";
            float tCountClass = ClassTypeList.Count + 1;
            float tOperation = 1;
            EditorUtility.DisplayProgressBar(tProgressBarTitle, "Reset Model index", tOperation / tCountClass);
            tOperation++;
            foreach (Type tType in ClassTypeList)
            {
                EditorUtility.DisplayProgressBar(tProgressBarTitle, "Reset " + tType.Name + " model", tOperation / tCountClass);
                tOperation++;
                //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_ModelReset); 
                NWDBasisHelper.FindTypeInfos(tType).DeleteOldsModels();
            }
            EditorUtility.DisplayProgressBar(tProgressBarTitle, "Finish", 1.0F);
            EditorUtility.ClearProgressBar();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ExportWebSites()
        {
            NWDBenchmark.Start();
            string tPath = EditorUtility.SaveFolderPanel("Export WebSite(s)", "", "NetWorkedDataServer");
            string tFolder = NWDAppConfiguration.SharedInstance().WebFolder;
            if (tPath != null)
            {
                if (tPath.Length != 0)
                {
                    if (Directory.Exists(tPath + "/" + tFolder + "_AllVersions") == false)
                    {
                        Directory.CreateDirectory(tPath + "/" + tFolder + "_AllVersions");
                    }
                    if (Directory.Exists(tPath + "/" + tFolder + "_AllVersions") == true)
                    {
                        string tOwnerFolderServer = NWDToolbox.FindOwnerServerFolder();
                        NWDToolbox.ExportCopyFolderFiles(tOwnerFolderServer + "/", tPath + "/" + tFolder + "_AllVersions");
                    }
                }
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif