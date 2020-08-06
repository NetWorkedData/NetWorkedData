//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDataManager
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The shared instance of <see cref="NWDDataManager"/>. In ideal project, it's the only instance.
        /// </summary>
        private static NWDDataManager kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The list of classes loaded in engine.
        /// </summary>
        public List<Type> ClassTypeLoadedList = new List<Type>();
        /// <summary>
        /// The list of all classes base on NWDBasis, usable in engine 
        /// </summary>
        public List<Type> ClassTypeList = new List<Type>();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The list of Class <see cref="NWDBasis"/> reccord in the <see cref="NWD.K_DeviceDatabaseName"/>
        /// </summary>
        public List<Type> ClassInDeviceDatabaseList = new List<Type>();
        /// <summary>
        /// The list of Class <see cref="NWDBasis"/> reccord in the <see cref="NWD.K_EditorDatabaseName"/>
        /// </summary>
        public List<Type> ClassInEditorDatabaseList = new List<Type>();
        /// <summary>
        /// The number of Class <see cref="NWDBasis"/> expected in launcher
        /// </summary>
        public int ClassNumberExpected = 0;
        /// <summary>
        /// The number of Class <see cref="NWDBasis"/> expected in launcher for  the <see cref="NWD.K_DeviceDatabaseName"/>
        /// </summary>
        public int ClassInDeviceDatabaseNumberExpected = 0;
        /// <summary>
        /// The number of Class <see cref="NWDBasis"/> expected in launcher for  the <see cref="NWD.K_EditorDatabaseName"/>
        /// </summary>
        public int ClassInEditorDatabaseRumberExpected = 0;
        /// <summary>
        /// The number of Class <see cref="NWDBasis"/> loaded in launcher
        /// </summary>
        public int ClassNumberLoaded = 0;
        /// <summary>
        /// The number of Class <see cref="NWDBasis"/> loaded in launcher from  the <see cref="NWD.K_EditorDatabaseName"/>
        /// </summary>
        public int ClassInEditorDatabaseNumberLoaded = 0;
        /// <summary>
        /// The number of Class <see cref="NWDBasis"/> loaded in launcher from  the <see cref="NWD.K_DeviceDatabaseName"/>
        /// </summary>
        public int ClassInDeviceDatabaseNumberLoaded = 0;
        /// <summary>
        /// The number of Class <see cref="NWDBasis"/> indexed in launcher
        /// </summary>
        public int ClassNumberIndexation = 0;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// return if <see cref="NWD.K_DeviceDatabaseName"/> is connected or not
        /// </summary>
        public bool DeviceDatabaseConnected = false;
        /// <summary>
        /// return if <see cref="NWD.K_DeviceDatabaseName"/> is opening 
        /// </summary>
        public bool DeviceDatabasConnectionInProgress = false;
        /// <summary>
        /// return if <see cref="NWD.K_DeviceDatabaseName"/> is loaded
        /// </summary>
        public bool DeviceDatabaseLoaded = false;
        /// <summary>
        /// return SQLite pointer on opened database <see cref="NWD.K_DeviceDatabaseName"/>
        /// </summary>
        public IntPtr SQLiteDeviceHandle;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// return if <see cref="NWD.K_EditorDatabaseName"/> is connected or not
        /// </summary>
        public bool EditorDatabaseConnected = false;
        /// <summary>
        /// return if <see cref="NWD.K_EditorDatabaseName"/> is opening
        /// </summary>
        public bool EditorDatabaseConnectionInProgress = false;
        /// <summary>
        /// return if <see cref="NWD.K_EditorDatabaseName"/> is loaded
        /// </summary>
        public bool EditorDatabaseLoaded = false;
        /// <summary>
        /// return SQLite pointer on opened database <see cref="NWD.K_EditorDatabaseName"/>
        /// </summary>
        public IntPtr SQLiteEditorHandle;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// reccord if datas are indexed
        /// </summary>
        private bool DatasIndexed = false;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The language for the player. Used in localisation.
        /// </summary>
        public string PlayerLanguage = "en";
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The list of all classes synchronizable on cluster
        /// </summary>
        public List<Type> ClassSynchronizeList = new List<Type>();
        /// <summary>
        /// The list of all classes not synchronizable on cluster
        /// </summary>
        public List<Type> ClassUnSynchronizeList = new List<Type>();
        /// <summary>
        /// The list of all classes dependent of an account 
        /// </summary>
        public List<Type> ClassAccountDependentList = new List<Type>();
        /// <summary>
        /// The list of all classes not dependent of an account
        /// </summary>
        public List<Type> ClassNotAccountDependentList = new List<Type>();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Dictionary to associate the good trigramm with the good class. It's easily to work with trigram in reference tahn juste number.
        /// </summary>
        public Dictionary<string, Type> ClassTrigramDictionary = new Dictionary<string, Type>(new StringIndexKeyComparer());
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================