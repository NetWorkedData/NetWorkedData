//=====================================================================================================================
//
// ideMobi copyright 2017 
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


#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //-------------------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    //-------------------------------------------------------------------------------------------------------------
    public class NWDClassTrigrammeAttribute : Attribute
    {
        public string Trigramme;
        public NWDClassTrigrammeAttribute(string sTrigramme)
        {
            this.Trigramme = sTrigramme;
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    //-------------------------------------------------------------------------------------------------------------
    public class NWDClassServerSynchronizeAttribute : Attribute
    {
        public bool ServerSynchronize;
        public NWDClassServerSynchronizeAttribute(bool sServerSynchronize)
        {
            this.ServerSynchronize = sServerSynchronize;
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    //-------------------------------------------------------------------------------------------------------------
    public class NWDClassDescriptionAttribute : Attribute
    {
        public string Description;
        public NWDClassDescriptionAttribute(string sDescription)
        {
            this.Description = sDescription;
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    //-------------------------------------------------------------------------------------------------------------
    public class NWDClassMenuNameAttribute : Attribute
    {
        public string MenuName;
        public NWDClassMenuNameAttribute(string sMenuName)
        {
            this.MenuName = sMenuName;
        }
    }
    //-------------------------------------------------------------------------------------------------------------
    public abstract class NWDTypeClass
    {
        //-------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------------

//#if UNITY_EDITOR
    public class NWDClassPhpPostCalculateAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Script;
        public NWDClassPhpPostCalculateAttribute(string sScript)
        {
            this.Script = sScript;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
//#endif
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================