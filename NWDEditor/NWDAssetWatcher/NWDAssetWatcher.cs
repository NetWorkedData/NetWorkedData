//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:23
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [InitializeOnLoad]
    public class NWDAssetWatcher : UnityEditor.AssetModificationProcessor
    {
        //-------------------------------------------------------------------------------------------------------------
        static List<string> kExtensionsWatchedList = new List<string>() {
            ".prefab",
            ".tga",
            ".mat",
            ".jpg",
            ".png",
            ".jpeg",
            ".mp3",
            ".mp4",
            ".aiff",
            ".aif",
            ".jpeg",
        };
        //-------------------------------------------------------------------------------------------------------------
        static NWDAssetWatcher()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        static bool ContaintsResource(string sOldPath)
        {
            bool tMove = false;
            if (AssetDatabase.IsValidFolder(sOldPath))
            {
                // ok is a folder
                if (sOldPath.Contains(NWD.K_Resources))
                {
                    tMove = true;
                }
                else
                {
                    foreach (string tFile in Directory.GetFiles(sOldPath))
                    {
                        string tExtension = Path.GetExtension(tFile);
                        if (kExtensionsWatchedList.Contains(tExtension.ToLower()))
                        {
                            tMove = true;
                            break;
                        }
                    }
                    if (tMove == false)
                    {
                        foreach (string tFolder in Directory.GetDirectories(sOldPath))
                        {
                            if (ContaintsResource(tFolder))
                            {
                                tMove = true;
                                break;
                            }
                        }
                    }
                }

            }
            else
            {
                string tExtension = Path.GetExtension(sOldPath);
                if (kExtensionsWatchedList.Contains(tExtension.ToLower()))
                {
                    tMove = true;
                }
            }
            return tMove;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static AssetMoveResult OnWillMoveAsset(string sOldPath, string sNewPath)
        {
            NWEBenchmark.Start();
            AssetMoveResult rReturn = AssetMoveResult.DidNotMove;
            if (ContaintsResource(sOldPath) == true)
            {
                NWDDataManager.SharedInstance().ChangeAssetPath(sOldPath, sNewPath);
            }
            NWEBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static AssetDeleteResult OnWillDeleteAsset(string sOldPath, RemoveAssetOptions sUnused)
        {
            //NWEBenchmark.Start();
            AssetDeleteResult rReturn = AssetDeleteResult.DidNotDelete;
            if (ContaintsResource(sOldPath) == true)
            {
                NWDDataManager.SharedInstance().ChangeAssetPath(sOldPath, string.Empty);
            }
            //NWEBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif