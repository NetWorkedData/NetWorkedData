// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:22:31
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using BasicToolBox;
using System.Globalization;
using UnityEditor;
using UnityEditor.SceneManagement;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWD
    {        
        //-------------------------------------------------------------------------------------------------------------
        // SQL INDEXATION
        public const string K_BASIS_INDEX = "BasisIndex";
        public const string K_INTERNAL_INDEX = "InternalIndex";
        public const string K_EDITOR_INDEX = "EditorIndex";
        //-------------------------------------------------------------------------------------------------------------
        // INSPECTOR GROUP
        public const string K_INSPECTOR_BASIS = "Basis";
        //-------------------------------------------------------------------------------------------------------------
        // FOR PHP AUTO EDITION
        public const string K_ENG = "ENG";
        public const string K_DB = "DB";
        public const string K_HTACCESS = ".htaccess";
        public const string K_DOT_HTACCESS = "dot_htaccess.txt";
        public const string K_PATH_BASE= "$PATH_BASE";
        public const string K_CONSTANTS_FILE = "constants.php";
        public const string K_MANAGEMENT_FILE = "management.php";
        public const string K_WS_SYNCHRONISATION = "synchronization.php";
        public const string K_WS_ENGINE = "engine.php";
        
        public const string K_SQL_CON = "$SQL_CON";
        public const string K_ENV = "$ENV";
        public const string K_NWD_SLT_SRV = "$NWD_SLT_SRV";
        public const string K_PHP_TIME_SYNC = "$TME_SYNC";



        public const string K_PHP_WSBUILD = "$WSBUILD";

        public const string K_WIP = "WIP_";
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
#endif
//=====================================================================================================================