#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;

using BasicToolBox;

using UnityEditor;

//=====================================================================================================================

namespace NetWorkedData
{
	[InitializeOnLoad]
	public class NWDAssetWatcher : UnityEditor.AssetModificationProcessor
	{
		static List<string> kExtensionsWatchedList = new List<string>(){".prefab", ""};
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
			if (kExtensionsWatchedList.Contains (tExtension)) {
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
			if (kExtensionsWatchedList.Contains (tExtension)) {
				NWDDataManager.SharedInstance.ChangeAssetPath (sOldPath, "");
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif