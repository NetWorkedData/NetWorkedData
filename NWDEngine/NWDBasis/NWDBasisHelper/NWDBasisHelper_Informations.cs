//=====================================================================================================================
//
// ideMobi copyright 2019 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
using SQLite4Unity3d;
using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        public void New_ClassInformations(string sString)
        {

            Debug.Log("From " + sString + " real [" + ClassType.Name + "] = > " + Informations(ClassType) + "' ");
        }
        //-------------------------------------------------------------------------------------------------------------
        public string New_Informations()
        {

#if UNITY_EDITOR
            int tCount = Datas.Count;
            if (tCount == 0)
            {
                return string.Empty + ClassNamePHP + " " + NWDConstants.K_APP_BASIS_NO_OBJECT + " (sync at " + New_SynchronizationGetLastTimestamp(NWDAppEnvironment.SelectedEnvironment()) + ")\n";
            }
            else if (tCount == 1)
            {
                return string.Empty + ClassNamePHP + " : " + tCount + " " + NWDConstants.K_APP_BASIS_ONE_OBJECT + " (sync at " + New_SynchronizationGetLastTimestamp(NWDAppEnvironment.SelectedEnvironment()) + ")\n";
            }
            else
            {
                return string.Empty + ClassNamePHP + " : " + tCount + " " + NWDConstants.K_APP_BASIS_X_OBJECTS + " (sync at " + New_SynchronizationGetLastTimestamp(NWDAppEnvironment.SelectedEnvironment()) + ")\n";
            }
#else
            return string.Empty;
#endif
        }

        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================