//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
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
    /// <summary>
    /// NWD Asset watcher. This Class look after assets which are changed in unity and send to the manager the new paths of assets
    /// </summary>
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
            AssetMoveResult rReturn = AssetMoveResult.DidNotMove;
            if (sOldPath.Contains("Resources"))
            {
                string tExtension = Path.GetExtension(sOldPath);
                //UnityEngine.Debug.Log ("OnWillMoveAsset " + sOldPath + " to " + sNewPath);
                if (kExtensionsWatchedList.Contains(tExtension.ToLower()))
                {
                    NWDDataManager.SharedInstance().ChangeAssetPath(sOldPath, sNewPath);
                }
            }
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static AssetDeleteResult OnWillDeleteAsset (string sOldPath, RemoveAssetOptions sUnused)
        {
            AssetDeleteResult rReturn = AssetDeleteResult.DidNotDelete;
            if (sOldPath.Contains("Resources"))
            {
                //UnityEngine.Debug.Log ("OnWillDeleteAsset " + sOldPath + "");
                string tExtension = Path.GetExtension(sOldPath);
                if (kExtensionsWatchedList.Contains(tExtension.ToLower()))
                {
                    NWDDataManager.SharedInstance().ChangeAssetPath(sOldPath, "");
                }
            }
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif