//=====================================================================================================================
//
// ideMobi copyright 2017 
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
	[InitializeOnLoad]
	public class NWDAssetWatcher : UnityEditor.AssetModificationProcessor
	{
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
		static NWDAssetWatcher ()
		{
		}
		//-------------------------------------------------------------------------------------------------------------
		public static AssetMoveResult OnWillMoveAsset (string sOldPath, string sNewPath)
		{
			AssetMoveResult rReturn = AssetMoveResult.DidNotMove;
			string tExtension = Path.GetExtension (sOldPath);
			BTBDebug.Log ("OnWillMoveAsset " + sOldPath + " to " + sNewPath);
			if (kExtensionsWatchedList.Contains (tExtension.ToLower ())) {
				NWDDataManager.SharedInstance.ChangeAssetPath (sOldPath, sNewPath);
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static AssetDeleteResult OnWillDeleteAsset (string sOldPath, RemoveAssetOptions e)
		{
			AssetDeleteResult rReturn = AssetDeleteResult.DidNotDelete;
			BTBDebug.Log ("OnWillDeleteAsset " + sOldPath + "");
			string tExtension = Path.GetExtension (sOldPath);
			if (kExtensionsWatchedList.Contains (tExtension.ToLower ())) {
				NWDDataManager.SharedInstance.ChangeAssetPath (sOldPath, "");
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif