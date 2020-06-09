//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:22:27
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using System;

using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDEditorBuildEnvironment : int
    {
        Ask = 0,
        Dev = 2,
        Preprod = 3,
        Prod = 1
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
            return (NWDEditorBuildEnvironment)NWDEditorPrefs.GetInt("EditorBuildEnvironment");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SetEditorBuildEnvironment(NWDEditorBuildEnvironment sValue)
        {
            NWDEditorPrefs.SetInt("EditorBuildEnvironment", (int)sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDEditorBuildRename GetEditoBuildRename()
        {
            return (NWDEditorBuildRename)NWDEditorPrefs.GetInt("EditorBuildRename");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SetEditorBuildRename(NWDEditorBuildRename sValue)
        {
            NWDEditorPrefs.SetInt("EditorBuildRename", (int)sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDEditorBuildDatabaseUpdate GetEditorBuildDatabaseUpdate()
        {
            return (NWDEditorBuildDatabaseUpdate)NWDEditorPrefs.GetInt("EditorBuildDatabaseUpdate");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SetEditorBuildDatabaseUpdate(NWDEditorBuildDatabaseUpdate sValue)
        {
            NWDEditorPrefs.SetInt("EditorBuildDatabaseUpdate", (int)sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public int callbackOrder { get { return 0; } }
        //-------------------------------------------------------------------------------------------------------------
        public void OnPreprocessBuild(BuildReport report)
        {
            //NWEBenchmark.Start();
            Debug.Log("NWDBuildPreProcess OnPreprocessBuild");

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
                                                                            "Production", "Development", "PreProduction") + 1;
                //  ----->----->---->---->----->----->----->----->----->       1          2           3
            }
            else if(GetEditoBuildEnvironment() == NWDEditorBuildEnvironment.Prod)
            {
                tResultEnvironment = 1;
            }
            else if (GetEditoBuildEnvironment() == NWDEditorBuildEnvironment.Preprod)
            {
                tResultEnvironment = 3;
            }
            else if (GetEditoBuildEnvironment() == NWDEditorBuildEnvironment.Dev)
            {
                tResultEnvironment = 2;
            }
            // Prebuild 
            if (tResultEnvironment == 1)
            {
                tName = NWDAppConfiguration.SharedInstance().ProdEnvironment.AppName;
                tHisto = NWDAppConfiguration.SharedInstance().ProdEnvironment.PreProdTimeFormat;
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

            // change the build environment 
            if (tResultEnvironment == 1)
            {
                //Debug.Log("NWDBuildPreProcess PRODUCTION BUILD");
                // update build timestamp
                if (tBuild == true)
                {
                    //Debug.LogWarning("NWDBuildPreProcess Update the build timestamp in NetWorkedData lib !!!");
                    NWDAppConfiguration.SharedInstance().ProdEnvironment.BuildTimestamp = tTimeStamp;
                    //NWDAppConfiguration.SharedInstance().PreprodEnvironment.BuildTimestamp = tTimeStamp;
                    //NWDAppConfiguration.SharedInstance().DevEnvironment.BuildTimestamp = tTimeStamp;
                }
                //Debug.Log("NWDBuildPreProcess !!! PRODUCTION BUILD");
                NWDAppConfiguration.SharedInstance().ProdEnvironment.BuildDate = tFuturBuildDate;
                // reccord modif
                NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().ProdEnvironment);
                // reselect environment
                NWDAppConfiguration.SharedInstance().ProdEnvironment.Selected = true;
                NWDAppConfiguration.SharedInstance().PreprodEnvironment.Selected = false;
                NWDAppConfiguration.SharedInstance().DevEnvironment.Selected = false;
            }
            else if (tResultEnvironment == 3)
            {
                //Debug.Log("NWDBuildPreProcess PRE PRODUCTION BUILD");
                // update build timestamp
                if (tBuild == true)
                {
                    //Debug.LogWarning("NWDBuildPreProcess Update the build timestamp in NetWorkedData lib !!!");
                    //NWDAppConfiguration.SharedInstance().ProdEnvironment.BuildTimestamp = tTimeStamp;
                    NWDAppConfiguration.SharedInstance().PreprodEnvironment.BuildTimestamp = tTimeStamp;
                    //NWDAppConfiguration.SharedInstance().DevEnvironment.BuildTimestamp = tTimeStamp;
                }
                NWDAppConfiguration.SharedInstance().PreprodEnvironment.BuildDate = tFuturBuildDate;
                // reccord modif
                NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
                // reselect environment
                NWDAppConfiguration.SharedInstance().ProdEnvironment.Selected = false;
                NWDAppConfiguration.SharedInstance().PreprodEnvironment.Selected = true;
                NWDAppConfiguration.SharedInstance().DevEnvironment.Selected = false;
            }
            else if (tResultEnvironment == 2)
            {
                //Debug.Log("NWDBuildPreProcess DEVELOPMENT BUILD");
                // update build timestamp
                if (tBuild == true)
                {
                    //Debug.LogWarning("NWDBuildPreProcess Update the build timestamp in NetWorkedData lib !!!");
                    //NWDAppConfiguration.SharedInstance().ProdEnvironment.BuildTimestamp = tTimeStamp;
                    //NWDAppConfiguration.SharedInstance().PreprodEnvironment.BuildTimestamp = tTimeStamp;
                    NWDAppConfiguration.SharedInstance().DevEnvironment.BuildTimestamp = tTimeStamp;
                }
                NWDAppConfiguration.SharedInstance().DevEnvironment.BuildDate = tFuturBuildDate;
                // reccord modif
                NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().DevEnvironment);
                // reselect environment
                NWDAppConfiguration.SharedInstance().ProdEnvironment.Selected = false;
                NWDAppConfiguration.SharedInstance().PreprodEnvironment.Selected = false;
                NWDAppConfiguration.SharedInstance().DevEnvironment.Selected = true;
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif