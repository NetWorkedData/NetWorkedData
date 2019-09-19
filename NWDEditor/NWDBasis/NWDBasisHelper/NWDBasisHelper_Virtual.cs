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
        public virtual void  ErrorRegenerate()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string  AddonPhpPreCalculate(NWDAppEnvironment sEnvironment)
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string  AddonPhpPostCalculate(NWDAppEnvironment sEnvironment)
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string  AddonPhpGetCalculate(NWDAppEnvironment sEnvironment)
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string  AddonPhpSpecialCalculate(NWDAppEnvironment sEnvironment)
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string  AddonPhpEngineCalculate(NWDAppEnvironment sEnvironment)
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string  AddonPhpFunctions(NWDAppEnvironment sEnvironment)
        {
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif