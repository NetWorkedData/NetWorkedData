//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
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
        public List<Type> mTypeLoadedList = new List<Type>();                   // TODO rename ClassLoadedList
        public List<Type> mTypeList = new List<Type>();                         // TODO rename ClassList


        public List<Type> mTypeSynchronizedList = new List<Type>();             // TODO rename ClassSynchronizedList
        public List<Type> mTypeUnSynchronizedList = new List<Type>();           // TODO rename ClassUnsynchronizedList
        public List<Type> mTypeAccountDependantList = new List<Type>();         // TODO rename ClassAccountDependentList
        public List<Type> mTypeNotAccountDependantList = new List<Type>();      // TODO rename ClassEditorDependentList
        public Dictionary<string, Type> mTrigramTypeDictionary = new Dictionary<string, Type>(new StringIndexKeyComparer());
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================