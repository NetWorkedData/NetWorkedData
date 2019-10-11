//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:26:56
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
using System.Collections;
using System.IO;
using UnityEngine;
using System.Threading;
using System.Text.RegularExpressions;
using SQLite4Unity3d;
//using BasicToolBox;
#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDWritingMode in Database.
    /// </summary>
    public enum NWDWritingMode : int
    {
        /// <summary>
        /// The data is writing By default Configuration.
        /// </summary>
        ByEditorDefault = -3,
        /// <summary>
        /// The data is writing By default Configuration.
        /// </summary>
        ByDefaultWebService = -2,
        /// <summary>
        /// The data is writing By default Configuration.
        /// </summary>
        ByDefaultLocal = -1,
        /// <summary>
        /// The data is writing in main thread now.
        /// </summary>
        MainThread = 0, // Main Thread
        /// <summary>
        /// The data is writing in background thread now. 
        /// Can be concurrence by main thread an webservice!
        /// </summary>
        PoolThread = 1, // Pool Thread
        /// <summary>
        /// The data is pull in queue in main thread. 
        /// The data will be writing directly on QueueExecution.
        /// </summary>
        QueuedMainThread = 2, // Main Thread In Queue
        /// <summary>
        /// The data is pull in queue in background thread. 
        /// The data will be writing in background on QueueExecution. 
        /// Can be concurrence by main thread an webservice!
        /// </summary>
        QueuedPoolThread = 3, // Pool Thread In Queue
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDWritingModeConfig : int
    {
        MainThread = NWDWritingMode.MainThread,
        PoolThread = NWDWritingMode.PoolThread,
        QueuedMainThread = NWDWritingMode.QueuedMainThread,
        QueuedPoolThread = NWDWritingMode.QueuedPoolThread,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDWritingModeConfigSync : int
    {
        MainThread = NWDWritingMode.MainThread,
        //PoolThread = NWDWritingMode.PoolThread,
        //QueuedMainThread = NWDWritingMode.QueuedMainThread,
        QueuedPoolThread = NWDWritingMode.QueuedPoolThread,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================