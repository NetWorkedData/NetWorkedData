//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
//#define NWD_LOG
//#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Build;
//=====================================================================================================================
namespace NetWorkedData.MacroDefine
{
    [InitializeOnLoad]
    /// <summary>
    /// Macro define can find if kMacro is set in the settings and add it if necessary.
    /// This class auto run at build project.
    /// You can use kMacro in precompile definition.
    /// </summary>
    public class MDEMacroDefine : IActiveBuildTargetChanged
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The macro to check in this project. It's tag in settings. The you can use #if define xxxxx #endif in your 
        /// code.
        /// </summary>
        public const string kMacro = "NET_WORKED_DATA";
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The shared instance used for this class.
        /// </summary>
        static MDEMacroDefine kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes the MDEMacroDefine class. 
        /// Instance kSharedInstance to use it when method OnActiveBuildTargetChanged must be invoked
        /// </summary>
        static MDEMacroDefine()
        {
            if (kSharedInstance == null)
            {
                kSharedInstance = new MDEMacroDefine();
                kSharedInstance.InstallMacroAll();
                //kSharedInstance.OnChangedPlatform();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the callback order for IActiveBuildTargetChanged
        /// </summary>
        /// <value>The callback order.</value>
        public int callbackOrder { get { return 0; } }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Raises the active build target changed event.
        /// </summary>
        /// <param name="previousTarget">Previous target.</param>
        /// <param name="newTarget">New target.</param>
        public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
        {
            OnChangedPlatform();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Raises the changed platform event.
        /// </summary>
        public void OnChangedPlatform()
        {
            InstallMacro(EditorUserBuildSettings.selectedBuildTargetGroup);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Installs the macro.
        /// </summary>
        /// <param name="sBuildTarget">S build target.</param>
        public void InstallMacro(BuildTargetGroup sBuildTarget)
        {
            if (PlayerSettings.GetScriptingDefineSymbolsForGroup(sBuildTarget).Contains(kMacro) == false)
            {
                PlayerSettings.SetScriptingDefineSymbolsForGroup(sBuildTarget, PlayerSettings.GetScriptingDefineSymbolsForGroup(sBuildTarget) + ";" + kMacro);
                if (PlayerSettings.GetScriptingDefineSymbolsForGroup(sBuildTarget).Contains(kMacro) == false)
                {
                    NWDDebug.Warning("Fail to install macro " + kMacro + " in " + sBuildTarget + " player settings!");
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Installs the macro in all build target.
        /// </summary>
        public void InstallMacroAll()
        {
            // List all target
            List<BuildTargetGroup> tActiveGroup = new List<BuildTargetGroup>();
            Array tBuildTargetArray = Enum.GetValues(typeof(BuildTarget));
            foreach (BuildTarget tBuildTarget in tBuildTargetArray)
            {
                BuildTargetGroup tBuildTargetGroup = BuildPipeline.GetBuildTargetGroup(tBuildTarget);
                if (tBuildTargetGroup != BuildTargetGroup.Unknown)
                {
                    if (BuildPipeline.IsBuildTargetSupported(tBuildTargetGroup, tBuildTarget))
                    {
                        if (tActiveGroup.Contains(tBuildTargetGroup) == false)
                        {
                            tActiveGroup.Add(tBuildTargetGroup);
                        }
                    }
                }
            }
            foreach (BuildTargetGroup tBuildTarget in tActiveGroup)
            {
                if (tBuildTarget != BuildTargetGroup.Unknown)
                {
                    InstallMacro(tBuildTarget);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================
#endif
