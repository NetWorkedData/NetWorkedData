//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.Build;

//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public class NWDBuildPreProcess : IPreprocessBuild {
		//-------------------------------------------------------------------------------------------------------------
		public int callbackOrder { get { return 0; } }
		//-------------------------------------------------------------------------------------------------------------
		public void OnPreprocessBuild(BuildTarget target, string path)
		{
			//Debug.Log("NWDBuildPreProcess OnPreprocessBuild for target " + target + " at path " + path);
			bool tProd = false;
			if (EditorUtility.DisplayDialog("Choose your environment to build", "Be sure to choose the good environment before build your app", "Production", "PreProduction"))
			{
				tProd = true;
			}
			if (tProd == true)
			{
				//Debug.Log("NWDBuildPreProcess !!! PRODUCTION BUILD");
				NWDAppConfiguration.SharedInstance().GenerateCSharpFile (NWDAppConfiguration.SharedInstance().ProdEnvironment);
				NWDAppConfiguration.SharedInstance().ProdEnvironment.Selected = true;
				NWDAppConfiguration.SharedInstance().PreprodEnvironment.Selected = false;
				NWDAppConfiguration.SharedInstance().DevEnvironment.Selected = false;
			}
			else
			{
				//Debug.Log("NWDBuildPreProcess PRE PRODUCTION BUILD");
				NWDAppConfiguration.SharedInstance().GenerateCSharpFile (NWDAppConfiguration.SharedInstance().PreprodEnvironment);
				NWDAppConfiguration.SharedInstance().ProdEnvironment.Selected = false;
				NWDAppConfiguration.SharedInstance().PreprodEnvironment.Selected = true;
				NWDAppConfiguration.SharedInstance().DevEnvironment.Selected = false;
			}

			NWDVersion.UpdateVersionBundle ();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif