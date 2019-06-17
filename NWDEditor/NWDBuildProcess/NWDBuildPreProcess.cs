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
    public class NWDBuildPreProcess : IPreprocessBuildWithReport
	{
		//-------------------------------------------------------------------------------------------------------------
		public int callbackOrder { get { return 0; } }
        //-------------------------------------------------------------------------------------------------------------
        public void OnPreprocessBuild(BuildReport report)
        {
            //BTBBenchmark.Start();
            Debug.Log("NWDBuildPreProcess OnPreprocessBuild");
            //Force all datas to be write in database
            NWDDataManager.SharedInstance().DataQueueExecute();

            NWDAppConfiguration.SharedInstance().WebBuild = NWDAppConfiguration.SharedInstance().WebBuildMax;

            //Get all infos
            string tName = NWDAppConfiguration.SharedInstance().DevEnvironment.AppName;
			string tHisto = NWDAppConfiguration.SharedInstance().DevEnvironment.PreProdTimeFormat;
            DateTime tDateTime = DateTime.Now;
            int tTimeStamp = NWDToolbox.Timestamp();
            string tFuturBuildDate = tDateTime.ToString("yyyy/MM/dd HH:mm:ss");
            int tResultEnvironment = EditorUtility.DisplayDialogComplex("Choose your environment to build", "Be sure to choose the good environment before build your app",
                                                                        "Production", "Development", "PreProduction");
            //  ----->----->---->---->----->----->----->----->----->       0           1           2
            if (tResultEnvironment == 0)
			{
				tName = NWDAppConfiguration.SharedInstance().ProdEnvironment.AppName;
				tHisto = NWDAppConfiguration.SharedInstance().ProdEnvironment.PreProdTimeFormat;
			}
            else if (tResultEnvironment == 2)
			{
				tName = NWDAppConfiguration.SharedInstance().PreprodEnvironment.AppName;
				tHisto = NWDAppConfiguration.SharedInstance().PreprodEnvironment.PreProdTimeFormat;
				string tNameFutur = tName;
				if (!string.IsNullOrEmpty(tHisto))
				{
					tNameFutur = tName + tDateTime.ToString(tHisto);
				    if (EditorUtility.DisplayDialog("Use the versioned name", "Do you want to use the name '" + tNameFutur + "' for your bundle", "Yes", "No"))
				    {
					    tName = tNameFutur;
                    }
                }
			}
            else if (tResultEnvironment == 1)
            {
                tName = NWDAppConfiguration.SharedInstance().DevEnvironment.AppName;
                tHisto = NWDAppConfiguration.SharedInstance().DevEnvironment.PreProdTimeFormat;
                string tNameFutur = tName;
                if (!string.IsNullOrEmpty(tHisto))
                {
                    tNameFutur = tName + tDateTime.ToString(tHisto);
                    if (EditorUtility.DisplayDialog("Use the versioned name", "Do you want to use the name '" + tNameFutur + "' for your bundle", "Yes", "No"))
                    {
                        tName = tNameFutur;
                    }
                }
            }
			// buildtiestamp update ?
			bool tBuild = true;
			if (EditorUtility.DisplayDialog("Update the build timestamp?", "Be sure to choose ", "NOT UPDATE", "Update"))
			{
				tBuild = false;
			}
            // update vesion of app build from NWDVersion system
            NWDVersion.UpdateVersionBundle();
            PlayerSettings.productName = tName;

			// change the build environment 
            if (tResultEnvironment == 0)
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
            else if (tResultEnvironment == 2)
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
            else if (tResultEnvironment == 1)
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
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif