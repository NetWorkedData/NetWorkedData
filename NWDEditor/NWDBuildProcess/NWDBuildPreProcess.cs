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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using System.IO;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDEditorBuildEnvironment : int
    {
        Ask = 0,
        Prod = 1,
        Dev = 2,
        Preprod = 3,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDEditorBuildRename : int
    {
        No = -1,
        Ask = 0,
        Yes = 1,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDEditorBuildDatabaseUpdate : int
    {
        No = -1,
        Ask = 0,
        Yes = 1,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDBuildPreProcess : IPreprocessBuildWithReport
    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWDEditorBuildEnvironment GetEditoBuildEnvironment()
        {
            return (NWDEditorBuildEnvironment)NWDProjectPrefs.GetInt(NWDConstants.K_EDITOR_BUILD_ENVIRONMENT);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDEditorBuildRename GetEditoBuildRename()
        {
            return (NWDEditorBuildRename)NWDProjectPrefs.GetInt(NWDConstants.K_EDITOR_BUILD_RENAME);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDEditorBuildDatabaseUpdate GetEditorBuildDatabaseUpdate()
        {
            return (NWDEditorBuildDatabaseUpdate)NWDProjectPrefs.GetInt(NWDConstants.K_EDITOR_BUILD_DATABASE_UPDATE);
        }
        //-------------------------------------------------------------------------------------------------------------
        public int callbackOrder { get { return 0; } }
        //-------------------------------------------------------------------------------------------------------------
        public void OnPreprocessBuild(BuildReport report)
        {
            NWDBenchmark.Start();

            // prevent error not exist (delete by dev)
            NWDErrorHelper tErrorHelper = NWDBasisHelper.BasisHelper<NWDError>() as NWDErrorHelper;
            tErrorHelper.GenerateBasisError();

            //Force all datas to be write in database
            NWDDataManager.SharedInstance().DataQueueExecute();
            NWDAppConfiguration.SharedInstance().WebBuild = NWDAppConfiguration.SharedInstance().WebBuildMax;

            //Get all infos
            string tName = NWDAppConfiguration.SharedInstance().DevEnvironment.AppName;
            string tHisto = NWDAppConfiguration.SharedInstance().DevEnvironment.PreProdTimeFormat;
            DateTime tDateTime = DateTime.Now;
            int tTimeStamp = NWDToolbox.Timestamp();
            string tFuturBuildDate = tDateTime.ToString("yyyy/MM/dd HH:mm:ss");

            // get environment
            int tResultEnvironment = 0;
            if (GetEditoBuildEnvironment() == NWDEditorBuildEnvironment.Ask)
            {
                tResultEnvironment = EditorUtility.DisplayDialogComplex("Choose your environment to build", "Be sure to choose the good environment before build your app",
                                                                            "Production", // 1
                                                                            "Development", //2
                                                                            "PreProduction" //3
                                                                        ) + 1;
            }
            else if (GetEditoBuildEnvironment() == NWDEditorBuildEnvironment.Prod)
            {
                tResultEnvironment = (int)NWDEditorBuildEnvironment.Prod;
            }
            else if (GetEditoBuildEnvironment() == NWDEditorBuildEnvironment.Dev)
            {
                tResultEnvironment = (int)NWDEditorBuildEnvironment.Dev;
            }
            else if (GetEditoBuildEnvironment() == NWDEditorBuildEnvironment.Preprod)
            {
                tResultEnvironment = (int)NWDEditorBuildEnvironment.Preprod;
            }

            // Prebuild 
            if (tResultEnvironment == 1)
            {
                tName = NWDAppConfiguration.SharedInstance().ProdEnvironment.AppName;
                tHisto = NWDAppConfiguration.SharedInstance().ProdEnvironment.PreProdTimeFormat;
            }
            else if (tResultEnvironment == 2)
            {
                tName = NWDAppConfiguration.SharedInstance().DevEnvironment.AppName;
                tHisto = NWDAppConfiguration.SharedInstance().DevEnvironment.PreProdTimeFormat;
                string tNameFutur = tName;
                if (!string.IsNullOrEmpty(tHisto))
                {
                    tNameFutur = tName + tDateTime.ToString(tHisto);

                    switch (GetEditoBuildRename())
                    {
                        case NWDEditorBuildRename.Ask:
                            {
                                if (EditorUtility.DisplayDialog("Use the versioned name", "Do you want to use the name '" + tNameFutur + "' for your bundle", "Yes", "No"))
                                {
                                    tName = tNameFutur;
                                }
                            }
                            break;
                        case NWDEditorBuildRename.Yes:
                            {
                                tName = tNameFutur;
                            }
                            break;
                    }
                }
            }
            else if (tResultEnvironment == 3)
            {
                tName = NWDAppConfiguration.SharedInstance().PreprodEnvironment.AppName;
                tHisto = NWDAppConfiguration.SharedInstance().PreprodEnvironment.PreProdTimeFormat;
                string tNameFutur = tName;
                if (!string.IsNullOrEmpty(tHisto))
                {
                    tNameFutur = tName + tDateTime.ToString(tHisto);

                    switch (GetEditoBuildRename())
                    {
                        case NWDEditorBuildRename.Ask:
                            {
                                if (EditorUtility.DisplayDialog("Use the versioned name", "Do you want to use the name '" + tNameFutur + "' for your bundle", "Yes", "No"))
                                {
                                    tName = tNameFutur;
                                }
                            }
                            break;
                        case NWDEditorBuildRename.Yes:
                            {
                                tName = tNameFutur;
                            }
                            break;
                    }
                }
            }

            // buildtiestamp update ?
            bool tBuild = true;
            switch (GetEditorBuildDatabaseUpdate())
            {
                case NWDEditorBuildDatabaseUpdate.Ask:
                    {
                        if (EditorUtility.DisplayDialog("Use a your database as new one?", "If you decide to copy your database as new one, that will replace old database on device and insert the sync timestamp to now.", "Use old database", "Copy database"))
                        {
                            tBuild = false;
                        }
                    }
                    break;
                case NWDEditorBuildDatabaseUpdate.No:
                    {
                        tBuild = false;
                    }
                    break;
                case NWDEditorBuildDatabaseUpdate.Yes:
                    {
                        tBuild = true;
                    }
                    break;
            }

            // update vesion of app build from NWDVersion system
            NWDVersion.UpdateVersionBundle();
            PlayerSettings.productName = tName;
            
            // verify if database exists
            string tDatabasePathEditor = /*NWD.K_Assets + "/" + NWD.K_StreamingAssets + "/" +*/ NWDDataManager.SharedInstance().DatabaseEditorName();
            string tDatabasePathBuild = NWD.K_Assets + "/" + NWD.K_StreamingAssets + "/" + NWDDataManager.SharedInstance().DatabaseBuildName();
            if (File.Exists(tDatabasePathBuild) == false)
            {
                Debug.Log("Copy database from editor to build because is first build");
                // if not exist copy for build
                File.Copy(tDatabasePathEditor, tDatabasePathBuild);
            }

            // if build is 
            if (tBuild == true)
            {
                Debug.Log("Copy database from editor to build");
                if (File.Exists(tDatabasePathBuild))
                {
                    File.Delete(tDatabasePathBuild);
                }
                File.Copy(tDatabasePathEditor, tDatabasePathBuild);
            }
            Debug.Log("tTimeStamp use to build is = " + NWEDateHelper.ConvertFromTimestamp(tTimeStamp).ToString("yyyy-MM-dd HH:mm:ss"));

            // Change the build environment            
            bool tEnvProd = false;
            bool tEnvDev = false;
            bool tEnvPreProd = false;

            switch(tResultEnvironment)
            {
                // Prod
                case 1:
                    {
                        NWDAppEnvironment tEnv = NWDAppConfiguration.SharedInstance().ProdEnvironment;
                        if (tBuild == true)
                        {
                            tEnv.BuildTimestamp = tTimeStamp;
                        }
                        tEnv.BuildDate = tFuturBuildDate;
                        tEnvProd = true;
                    }
                    break;
                // Dev
                case 2:
                    {
                        NWDAppEnvironment tEnv = NWDAppConfiguration.SharedInstance().DevEnvironment;
                        if (tBuild == true)
                        {
                            tEnv.BuildTimestamp = tTimeStamp;
                        }
                        tEnv.BuildDate = tFuturBuildDate;
                        tEnvDev = true;
                    }
                    break;
                // Preprod
                case 3:
                    {
                        NWDAppEnvironment tEnv = NWDAppConfiguration.SharedInstance().PreprodEnvironment;
                        if (tBuild == true)
                        {
                            tEnv.BuildTimestamp = tTimeStamp;
                        }
                        tEnv.BuildDate = tFuturBuildDate;
                        tEnvPreProd = true;
                    }
                    break;
                default:
                    break;
            }

            NWDAppConfiguration.SharedInstance().ProdEnvironment.Selected = tEnvProd;
            NWDAppConfiguration.SharedInstance().DevEnvironment.Selected = tEnvDev;
            NWDAppConfiguration.SharedInstance().PreprodEnvironment.Selected = tEnvPreProd;

            NWDEditorWindow.GenerateCSharpFile();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
