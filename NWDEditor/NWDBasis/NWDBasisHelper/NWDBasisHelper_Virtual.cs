//=====================================================================================================================
//
// ideMobi copyright 2019 
// All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
using SQLite4Unity3d;
using BasicToolBox;
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        public virtual void New_ErrorRegenerate()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string New_AddonPhpPreCalculate(NWDAppEnvironment AppEnvironment)
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string New_AddonPhpPostCalculate(NWDAppEnvironment AppEnvironment)
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string New_AddonPhpGetCalculate(NWDAppEnvironment AppEnvironment)
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string New_AddonPhpSpecialCalculate(NWDAppEnvironment AppEnvironment)
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string New_AddonPhpFunctions(NWDAppEnvironment AppEnvironment)
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif