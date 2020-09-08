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

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// The relation with the Game Save <see cref="NWDGameSave"/> (<see cref="NWDBasisGameSaveDependent.GameSave"/>)
    /// </summary>
    public enum NWDTemplateGameSaveDependent : int
    {
        /// <summary>
        /// Not possible! It's nescessary to define!
        /// </summary>
        Error = -1,
        /// <summary>
        /// No dependent from Game Save.
        /// </summary>
        NoGameSaveDependent = 0,
        /// <summary>
        /// Dependent from a normal Account's Game Save
        /// </summary>
        SimpleGameSaveDependent = 1,
        /// <summary>
        /// Dependent from multi Game Save from Multi-Account
        /// </summary>
        MultiGameSaveDependent = 2,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// The relation with the account notion with <see cref="NWDAccount"/> (<see cref="NWDBasisAccountDependent.Account"/>)
    /// </summary>
    public enum NWDTemplateAccountDependent : int
    {
        /// <summary>
        /// Not possible! It's nescessary to define!
        /// </summary>
        Error = -1,
        /// <summary>
        /// Not account dependent
        /// </summary>
        NoAccountDependent = 0,
        /// <summary>
        /// Account dependent (<see cref="NWDBasisAccountDependent.Account"/>)
        /// </summary>
        SimpleAccountDependent = 1,
        /// <summary>
        /// Account dependent read and write for <see cref="NWDBasisAccountDependent.Account"/> and readeable only for the other account in List
        /// </summary>
        PublishAccountDependent = 2,
        /// <summary>
        /// Account dependent read and write for all <see cref="NWDBasisAccountDependent.Account"/>
        /// </summary>
        ShareAccountDependent = 3,
        /// <summary>
        /// Account dependent but restricted for a virtual referencing in Runtime and real referencing in Editor and in cluster: protect <see cref="NWDAccount"/> and <see cref="NWDRequestToken"/>
        /// </summary>
        RestrictedAccountDependent = 9,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// The relation with the local database writing
    /// </summary>
    public enum NWDTemplateDeviceDatabase : int
    {
        /// <summary>
        /// Not possible! It's nescessary to define!
        /// </summary>
        Error = -1,
        /// <summary>
        /// No reccord on local database
        /// </summary>
        NoReccordable = 0,
        /// <summary>
        /// Reccord on local database on the editor database
        /// </summary>
        ReccordableInDeviceDatabaseEditor = 1,
        /// <summary>
        /// Reccord on local database on the account database
        /// </summary>
        ReccordableInDeviceDatabaseAccount = 2,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDTemplateClusterDatabase : int
    {
        /// <summary>
        /// Not possible! It's nescessary to define!
        /// </summary>
        Error = -1,
        /// <summary>
        /// No Synchronization on <see cref="NWDCluster"/>
        /// </summary>
        NoSynchronizable = 0,
        /// <summary>
        /// Synchronization on <see cref="NWDCluster"/> in all node of databases (<see cref="NWDServerDatas"/>)
        /// </summary>
        SynchronizableInClusterAllDatabase = 1,
        /// <summary>
        /// Synchronization on <see cref="NWDCluster"/> only in range access database (<see cref="NWDServerDatas"/>)
        /// </summary>
        SynchronizableInClusterAccessRangeDatabase = 2,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// The relation with the bundle notion with <see cref="NWDBundle"/> (<see cref="NWDBasisBundled.Bundle"/>)
    /// </summary>
    public enum NWDTemplateBundlisable : int
    {
        /// <summary>
        /// Not possible! It's nescessary to define!
        /// </summary>
        Error = -1,
        /// <summary>
        /// No bundlisable
        /// </summary>
        NoBundlisable = 0,
        /// <summary>
        /// Bundlisable with enum <see cref="NWDBundle"/>
        /// </summary>
        Bundlisable = 1,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDTemplateHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        private Type ClassType;
        //-------------------------------------------------------------------------------------------------------------
        private Type BaseClassType;
        //-------------------------------------------------------------------------------------------------------------
        private NWDTemplateDeviceDatabase DeviceDatabase;
        private NWDTemplateClusterDatabase Synchronizable;
        private NWDTemplateBundlisable Bundlisable;
        private NWDTemplateAccountDependent AccountDependent;
        private NWDTemplateGameSaveDependent GamesaveDependent;
        //-------------------------------------------------------------------------------------------------------------
        public Type GetClass() { return ClassType; }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTemplateDeviceDatabase GetDeviceDatabase() { return DeviceDatabase; }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTemplateClusterDatabase GetSynchronizable() { return Synchronizable; }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTemplateBundlisable GetBundlisable() { return Bundlisable; }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTemplateAccountDependent GetAccountDependent() { return AccountDependent; }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTemplateGameSaveDependent GetGamesaveDependent() { return GamesaveDependent; }
        //-------------------------------------------------------------------------------------------------------------
        public string GetBaseClassToString()
        {
            string rReturn = "error";
            if (BaseClassType!=null)
            {
                rReturn = BaseClassType.Name;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetClassToString()
        {
            string rReturn = "error";
            if (ClassType != null)
            {
                rReturn = ClassType.Name;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetClassType(Type sType)
        {
            DeviceDatabase = NWDTemplateDeviceDatabase.NoReccordable;
            Synchronizable = NWDTemplateClusterDatabase.Error;
            Bundlisable = NWDTemplateBundlisable.Error;
            AccountDependent = NWDTemplateAccountDependent.Error;
            GamesaveDependent = NWDTemplateGameSaveDependent.Error;

            ClassType = sType;
            if (ClassType != null)
            {
                if (ClassType.IsSubclassOf(typeof(NWDTypeClass)))
                {
                    BaseClassType = ClassType.BaseType;

                    if (ClassType.IsAbstract)
                    {
                    }
                    /*if (ClassType.IsSubclassOf(typeof(NWDBasisGameSaveShared)) || ClassType == typeof(NWDBasisGameSaveShared))
                    {
                        DeviceDatabase = NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseAccount;
                        Synchronizable = NWDTemplateClusterDatabase.SynchronizableInClusterAccessRangeDatabase;
                        Bundlisable = NWDTemplateBundlisable.Bundlisable;
                        AccountDependent = NWDTemplateAccountDependent.SharedAccountDependent;
                        GamesaveDependent = NWDTemplateGamesaveDependent.NoGamsesaveDependent;
                    }
                    else if (ClassType.IsSubclassOf(typeof(NWDBasisAccountShared)) || ClassType == typeof(NWDBasisAccountShared))
                    {
                        DeviceDatabase = NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseAccount;
                        Synchronizable = NWDTemplateClusterDatabase.SynchronizableInClusterAccessRangeDatabase;
                        Bundlisable = NWDTemplateBundlisable.Bundlisable;
                        AccountDependent = NWDTemplateAccountDependent.SharedAccountDependent;
                        GamesaveDependent = NWDTemplateGamesaveDependent.SimpleGamesaveDependent;
                    }
                    else if (ClassType.IsSubclassOf(typeof(NWDBasisGameSavePublish)) || ClassType == typeof(NWDBasisGameSavePublish))
                    {
                        DeviceDatabase = NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseAccount;
                        Synchronizable = NWDTemplateClusterDatabase.SynchronizableInClusterAccessRangeDatabase;
                        Bundlisable = NWDTemplateBundlisable.Bundlisable;
                        AccountDependent = NWDTemplateAccountDependent.PublishAccountDependent;
                        GamesaveDependent = NWDTemplateGamesaveDependent.SimpleGamesaveDependent;
                    }
                    else if (ClassType.IsSubclassOf(typeof(NWDBasisAccountPublish)) || ClassType == typeof(NWDBasisAccountPublish))
                    {
                        DeviceDatabase = NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseAccount;
                        Synchronizable = NWDTemplateClusterDatabase.SynchronizableInClusterAccessRangeDatabase;
                        Bundlisable = NWDTemplateBundlisable.Bundlisable;
                        AccountDependent = NWDTemplateAccountDependent.PublishAccountDependent;
                        GamesaveDependent = NWDTemplateGamesaveDependent.NoGamsesaveDependent;
                    }*/
                    else if (ClassType.IsSubclassOf(typeof(NWDBasisGameSaveShared)) || ClassType == typeof(NWDBasisGameSaveShared))
                    {
                        DeviceDatabase = NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseAccount;
                        Synchronizable = NWDTemplateClusterDatabase.SynchronizableInClusterAccessRangeDatabase;
                        Bundlisable = NWDTemplateBundlisable.Bundlisable;
                        AccountDependent = NWDTemplateAccountDependent.ShareAccountDependent;
                        GamesaveDependent = NWDTemplateGameSaveDependent.MultiGameSaveDependent;
                    }
                    else if (ClassType.IsSubclassOf(typeof(NWDBasisGameSavePublish)) || ClassType == typeof(NWDBasisGameSavePublish))
                    {
                        DeviceDatabase = NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseAccount;
                        Synchronizable = NWDTemplateClusterDatabase.SynchronizableInClusterAccessRangeDatabase;
                        Bundlisable = NWDTemplateBundlisable.Bundlisable;
                        AccountDependent = NWDTemplateAccountDependent.PublishAccountDependent;
                        GamesaveDependent = NWDTemplateGameSaveDependent.MultiGameSaveDependent;
                    }
                    else if (ClassType.IsSubclassOf(typeof(NWDBasisAccountShared)) || ClassType == typeof(NWDBasisAccountShared))
                    {
                        DeviceDatabase = NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseAccount;
                        Synchronizable = NWDTemplateClusterDatabase.SynchronizableInClusterAccessRangeDatabase;
                        Bundlisable = NWDTemplateBundlisable.Bundlisable;
                        AccountDependent = NWDTemplateAccountDependent.ShareAccountDependent;
                        GamesaveDependent = NWDTemplateGameSaveDependent.NoGameSaveDependent;
                    }
                    else if (ClassType.IsSubclassOf(typeof(NWDBasisAccountPublish)) || ClassType == typeof(NWDBasisAccountPublish))
                    {
                        DeviceDatabase = NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseAccount;
                        Synchronizable = NWDTemplateClusterDatabase.SynchronizableInClusterAccessRangeDatabase;
                        Bundlisable = NWDTemplateBundlisable.Bundlisable;
                        AccountDependent = NWDTemplateAccountDependent.PublishAccountDependent;
                        GamesaveDependent = NWDTemplateGameSaveDependent.NoGameSaveDependent;
                    }
                    else if (ClassType.IsSubclassOf(typeof(NWDBasisGameSaveDependent)) || ClassType == typeof(NWDBasisGameSaveDependent))
                    {
                        DeviceDatabase = NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseAccount;
                        Synchronizable = NWDTemplateClusterDatabase.SynchronizableInClusterAccessRangeDatabase;
                        Bundlisable = NWDTemplateBundlisable.Bundlisable;
                        AccountDependent = NWDTemplateAccountDependent.SimpleAccountDependent;
                        GamesaveDependent = NWDTemplateGameSaveDependent.SimpleGameSaveDependent;
                    }
                    else if (ClassType.IsSubclassOf(typeof(NWDBasisAccountUnsynchronize)) || ClassType == typeof(NWDBasisAccountUnsynchronize))
                    {
                        DeviceDatabase = NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseAccount;
                        Synchronizable = NWDTemplateClusterDatabase.NoSynchronizable;
                        Bundlisable = NWDTemplateBundlisable.Bundlisable;
                        AccountDependent = NWDTemplateAccountDependent.SimpleAccountDependent;
                        GamesaveDependent = NWDTemplateGameSaveDependent.NoGameSaveDependent;
                    }
                    else if (ClassType.IsSubclassOf(typeof(NWDBasisAccountDependent)) || ClassType == typeof(NWDBasisAccountDependent))
                    {
                        DeviceDatabase = NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseAccount;
                        Synchronizable = NWDTemplateClusterDatabase.SynchronizableInClusterAccessRangeDatabase;
                        Bundlisable = NWDTemplateBundlisable.Bundlisable;
                        AccountDependent = NWDTemplateAccountDependent.SimpleAccountDependent;
                        GamesaveDependent = NWDTemplateGameSaveDependent.NoGameSaveDependent;
                    }
                    else if (ClassType.IsSubclassOf(typeof(NWDBasisAccountRestricted)) || ClassType == typeof(NWDBasisAccountRestricted))
                    {
                        DeviceDatabase = NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseAccount;
                        Synchronizable = NWDTemplateClusterDatabase.SynchronizableInClusterAccessRangeDatabase;
                        Bundlisable = NWDTemplateBundlisable.NoBundlisable;
                        AccountDependent = NWDTemplateAccountDependent.RestrictedAccountDependent;
                        GamesaveDependent = NWDTemplateGameSaveDependent.NoGameSaveDependent;
                    }
                    else if (ClassType.IsSubclassOf(typeof(NWDBasisUnsynchronize)) || ClassType == typeof(NWDBasisUnsynchronize))
                    {
                        DeviceDatabase = NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseAccount;
                        Synchronizable = NWDTemplateClusterDatabase.NoSynchronizable;
                        Bundlisable = NWDTemplateBundlisable.NoBundlisable;
                        AccountDependent = NWDTemplateAccountDependent.NoAccountDependent;
                        GamesaveDependent = NWDTemplateGameSaveDependent.NoGameSaveDependent;
                    }
                    else if (ClassType.IsSubclassOf(typeof(NWDBasisBundled)) || ClassType == typeof(NWDBasisBundled))
                    {
                        DeviceDatabase = NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseEditor;
                        Synchronizable = NWDTemplateClusterDatabase.SynchronizableInClusterAllDatabase;
                        Bundlisable = NWDTemplateBundlisable.Bundlisable;
                        AccountDependent = NWDTemplateAccountDependent.NoAccountDependent;
                        GamesaveDependent = NWDTemplateGameSaveDependent.NoGameSaveDependent;
                    }
                    else if (ClassType.IsSubclassOf(typeof(NWDBasis)) || ClassType == typeof(NWDBasis))
                    {
                        DeviceDatabase = NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseEditor;
                        Synchronizable = NWDTemplateClusterDatabase.SynchronizableInClusterAllDatabase;
                        Bundlisable = NWDTemplateBundlisable.NoBundlisable;
                        AccountDependent = NWDTemplateAccountDependent.NoAccountDependent;
                        GamesaveDependent = NWDTemplateGameSaveDependent.NoGameSaveDependent;
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
