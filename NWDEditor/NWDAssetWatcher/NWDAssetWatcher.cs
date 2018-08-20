//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System.Collections.Generic;
using System.IO;
using BasicToolBox;
#if UNITY_EDITOR
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
        /// <summary>
        /// The asset's extensions watched list.
        /// </summary>
		static List<string> kExtensionsWatchedList = new List<string> () {
			".prefab", 
			".tga", 
			".mat", 
			".jpg", 
			".png", 
			".jpeg", 
			".mp3", 
			".jpeg", 
			".shader", 
//			".txt",
//			".json",
			"", // for folder change
		};
		//-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes the <see cref="T:NetWorkedData.NWDAssetWatcher"/> class.
        /// </summary>
		static NWDAssetWatcher ()
		{
		}
		//-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// On will move asset.
        /// </summary>
        /// <returns>asset will move .</returns>
        /// <param name="sOldPath">old path.</param>
        /// <param name="sNewPath">new path.</param>
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
        /// <summary>
        /// On will delete asset.
        /// </summary>
        /// <returns>The will delete asset.</returns>
        /// <param name="sOldPath">S old path.</param>
        /// <param name="e">E.</param>
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