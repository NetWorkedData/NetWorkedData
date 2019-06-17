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
            "", // for folder change
		};
		//-------------------------------------------------------------------------------------------------------------
		static NWDAssetWatcher ()
		{
		}
		//-------------------------------------------------------------------------------------------------------------
		public static AssetMoveResult OnWillMoveAsset (string sOldPath, string sNewPath)
        {
            //BTBBenchmark.Start();
            AssetMoveResult rReturn = AssetMoveResult.DidNotMove;
            if (sOldPath.Contains(NWD.K_Resources))
            {
                string tExtension = Path.GetExtension(sOldPath);
                if (kExtensionsWatchedList.Contains(tExtension.ToLower()))
                {
                    NWDDataManager.SharedInstance().ChangeAssetPath(sOldPath, sNewPath);
                }
            }
            //BTBBenchmark.Finish();
            return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static AssetDeleteResult OnWillDeleteAsset (string sOldPath, RemoveAssetOptions sUnused)
        {
            //BTBBenchmark.Start();
            AssetDeleteResult rReturn = AssetDeleteResult.DidNotDelete;
            if (sOldPath.Contains(NWD.K_Resources))
            {
                string tExtension = Path.GetExtension(sOldPath);
                if (kExtensionsWatchedList.Contains(tExtension.ToLower()))
                {
                    NWDDataManager.SharedInstance().ChangeAssetPath(sOldPath, "");
                }
            }
            //BTBBenchmark.Finish();
            return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif