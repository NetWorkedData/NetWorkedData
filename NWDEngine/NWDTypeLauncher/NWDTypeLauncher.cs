//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:48
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================



using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
//using BasicToolBox;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDTypeLauncherType
    {
        //-------------------------------------------------------------------------------------------------------------
        public static void PreCompiledData()
        {
            
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDTypeLauncher is the first class launch in the NetWorkedData lib. It's used to determine the class model and interaction.
    /// </summary>
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDTypeLauncher : NWDTypeLauncherType
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The lib is launching.
        /// </summary>
        //public static bool IsLaunching = false;
        /// <summary>
        /// The lib is launched.
        /// </summary>
        //public static bool IsLaunched = false;
        /// <summary>
        /// All Types array.
        /// </summary>
        public static Type[] AllTypes;
        //-------------------------------------------------------------------------------------------------------------
        //public static int CodePinTentative = 0;
        //public static string CodePinValue;
        //public static string CodePinValueConfirm;
        //public static bool CodePinNeeded = false;
        //public static bool CodePinCreationNeeded = false;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes the <see cref="T:NetWorkedData.NWDTypeLauncher"/> class.
        /// </summary>
        static NWDTypeLauncher()
        {
            //NWDDebug.Log("NWDTypeLauncher Static Class Constructor()");
            Launcher();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="T:NetWorkedData.NWDTypeLauncher"/> class.
        /// </summary>
        public NWDTypeLauncher()
        {
            //NWDDebug.Log("NWDTypeLauncher Instance Constructor NWDTypeLauncher()");
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Launcher this daemon lib.
        /// </summary>
        public static void Launcher()
        {
            NWDLauncher.Launch();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Runs the launcher.
        /// </summary>
#if UNITY_EDITOR
        [InitializeOnLoadMethod()]
#endif
        public static void RunLauncher()
        {
#if UNITY_EDITOR
            // to clear error ProgressBar 
            EditorUtility.ClearProgressBar();
#endif
            // FORCE TO ENGLISH FORMAT!
            Thread.CurrentThread.CurrentCulture = NWDConstants.FormatCountry;
            // this class deamon is launch at start ... Read all classes, install all classes deamon and load all datas
            //NWDDebug.Log("NWDTypeLauncher RunLauncher()");
            // not double lauch
            // not double launching!
            if (NWDLauncher.GetState() == NWDStatut.EngineLaunching)
            {
                NWEBenchmark.Start("### LAUNCH CLASSES DECLARE");
                // craeta a list to reccord all classes
                List<Type> tTypeList = new List<Type>();
                // Find all Type of NWDType
                //NWEBenchmark.Start("Launcher() reflexion");

                Dictionary<Type, Type> BasisToHelperList = new Dictionary<Type, Type>();
                List<Type> BasisTypeList = new List<Type>();
                Type[] tAllTypes = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
                // sort and filter by NWDBasis (NWDTypeClass subclass)
                Type[] tAllNWDTypes = (from System.Type type in tAllTypes where type.IsSubclassOf(typeof(NWDTypeClass)) select type).ToArray();
                Type[] tAllHelperDTypes = (from System.Type type in tAllTypes where type.IsSubclassOf(typeof(NWDBasisHelper)) select type).ToArray();
                foreach (Type tType in tAllNWDTypes)
                {
                    if (tType != typeof(NWDBasis))
                    {
                        BasisTypeList.Add(tType);
                        foreach (Type tPossibleHelper in tAllHelperDTypes)
                        {
                            if (tPossibleHelper.ContainsGenericParameters == false)
                            {
                                if (tPossibleHelper.BaseType.GenericTypeArguments.Contains(tType))
                                {
                                    BasisToHelperList.Add(tType, tPossibleHelper);
                                    break;
                                }
                            }
                        }
                        if (BasisToHelperList.ContainsKey(tType) == false)
                        {
                            BasisToHelperList.Add(tType, typeof(NWDBasisHelper));
                        }
                    }
                }
                //NWEBenchmark.Finish("Launcher() reflexion");
                NWEBenchmark.Start("Launcher() loop");
                foreach (Type tType in tAllNWDTypes)
                {
                    //Debug.Log(" **********************************************************");
                    if (tType != typeof(NWDBasis))
                    {
                        // not the NWDBasis because it's generic class
                            tTypeList.Add(tType);
                            NWDBasisHelper tHelper = NWDBasisHelper.Declare(tType, BasisToHelperList[tType]);
                    }
                    //Debug.Log(" **********************************************************");
                }
                AllTypes = tTypeList.ToArray();
                NWEBenchmark.Finish("Launcher() loop", true, " count class : " + tTypeList.Count);
                NWDAppConfiguration.SharedInstance().RestaureTypesConfigurations();
                NWDDataManager.SharedInstance().ClassEditorExpected = NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Count();
                NWDDataManager.SharedInstance().ClassAccountExpected = NWDDataManager.SharedInstance().mTypeAccountDependantList.Count();
                NWDDataManager.SharedInstance().ClassExpected = NWDDataManager.SharedInstance().ClassEditorExpected + NWDDataManager.SharedInstance().ClassAccountExpected;

                NWEBenchmark.Finish("### LAUNCH CLASSES DECLARE", true, " with "+ tAllNWDTypes.Length + " classes");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================