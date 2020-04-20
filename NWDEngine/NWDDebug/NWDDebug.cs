﻿//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:3
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================


using UnityEditor;
#if UNITY_EDITOR
using UnityEngine;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDDebug it class to use enable/disable Log.
    /// </summary>
    public static class NWDDebug
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Active or not the log.
        /// </summary>
        public static bool Active = true;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Log the specified sString.
        /// </summary>
        /// <param name="sString">S string.</param>
        public static void Log(string sString, Object sThis = null)
        {
            if (Active == true)
            {
                if (sThis != null)
                {
                    Debug.Log(sString, sThis);
                }
                else
                {
#if UNITY_EDITOR
                    if (EditorPrefs.GetBool(NWDConstants.K_EDITOR_CLIPBOARD_LAST_LOG) == true)
                    {
                        NWEClipboard.CopyToClipboard(sString);
                    }
#endif
                    Debug.Log(sString);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Log Warning the specified sString.
        /// </summary>
        /// <param name="sString">S string.</param>
        public static void Warning(string sString, Object sThis = null)
        {
            if (Active == true)
            {
                if (sThis != null)
                {
                    Debug.LogWarning(sString, sThis);
                }
                else
                {
                    Debug.LogWarning(sString);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
