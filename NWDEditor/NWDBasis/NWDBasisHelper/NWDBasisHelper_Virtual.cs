//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:44
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
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
        public virtual string New_AddonPhpPreCalculate(NWDAppEnvironment sEnvironment)
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string New_AddonPhpPostCalculate(NWDAppEnvironment sEnvironment)
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string New_AddonPhpGetCalculate(NWDAppEnvironment sEnvironment)
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string New_AddonPhpSpecialCalculate(NWDAppEnvironment sEnvironment)
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string New_AddonPhpEngineCalculate(NWDAppEnvironment sEnvironment)
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string New_AddonPhpFunctions(NWDAppEnvironment sEnvironment)
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif