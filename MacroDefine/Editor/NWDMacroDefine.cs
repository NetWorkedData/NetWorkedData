using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;

//=====================================================================================================================
namespace NetWorkedDataMacroDefine
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[InitializeOnLoad]
	public partial class NWDMacroDefine :  IActiveBuildTargetChanged
	{
		//-------------------------------------------------------------------------------------------------------------
		static NWDMacroDefine kSharedInstance = new NWDMacroDefine("NET_WORKED_DATA", true);
        //-------------------------------------------------------------------------------------------------------------
        string Macro;
        bool Install;
		//-------------------------------------------------------------------------------------------------------------
		public NWDMacroDefine (string sMacro, bool sInstall)
		{
            Macro = sMacro;
            Install = sInstall;
            OnChangedPlatform();
		}
		//-------------------------------------------------------------------------------------------------------------
		public int callbackOrder { get { return 0; } }
		//-------------------------------------------------------------------------------------------------------------
		public void OnActiveBuildTargetChanged (BuildTarget previousTarget, BuildTarget newTarget)
		{
			OnChangedPlatform ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public void OnChangedPlatform ()
		{
			InstallMacro (EditorUserBuildSettings.selectedBuildTargetGroup);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void InstallMacro (BuildTargetGroup sBuildTarget)
		{
            List<string> tMacroList = new List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(sBuildTarget).Split(new char[] { ';' }));
            if (Install == true)
            {
               if (tMacroList.Contains(Macro)==false)
                {
                    tMacroList.Add(Macro);
                }
            }
            else
            {
                if (tMacroList.Contains(Macro)==true)
                {
                    tMacroList.Remove(Macro);
                }
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(sBuildTarget,string.Join(";",tMacroList));
		}
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
