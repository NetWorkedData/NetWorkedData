//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:26:58
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
using BasicToolBox;
#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDWritingPending : int
    {
        /// <summary>
        /// Unknow pending...
        /// </summary>
        Unknow,
        /// <summary>
        /// New Object in memory. Pending writing in database.
        /// </summary>
        InsertInMemory,
        /// <summary>
        /// Updated Object in memory. Pending writing in database.
        /// </summary>
        UpdateInMemory,
        /// <summary>
        /// Deleted Object in memory. Pending writing in database.
        /// </summary>
        DeleteInMemory,
        /// <summary>
        /// Object in database. You have the same representation.
        /// </summary>
        InDatabase,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================