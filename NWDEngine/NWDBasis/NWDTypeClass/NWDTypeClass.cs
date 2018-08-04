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
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public abstract class NWDTypeClass
    {
        //-------------------------------------------------------------------------------------------------------------
        public virtual string InternalKeyValue()
        {
            return "";
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string InternalDescriptionValue()
        {
            return "";
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string ReferenceUsedValue()
        {
            return "";
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual string ClassNameUsedValue()
        {
            return "";
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool DataIntegrityState()
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool TrashState()
        {
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool EnableState()
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool ReachableState()
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool InGameSaveState()
        {
            return true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void SetCurrentGameSave()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public string DatasMenu()
        {
            string rReturn = InternalKeyValue() + " <" + ReferenceUsedValue() + ">";
            rReturn = rReturn.Replace("/", " ");
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void InsertDataProceedWithTransaction()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void InsertDataProceed()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void InsertDataFinish()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void UpdateDataProceedWithTransaction()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void UpdateDataProceed()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void UpdateDataFinish()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DeleteDataProceedWithTransaction()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DeleteDataProceed()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DeleteDataFinish()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDClassTrigrammeAttribute : Attribute
    {
        public string Trigramme;
        public NWDClassTrigrammeAttribute(string sTrigramme)
        {
            this.Trigramme = sTrigramme;
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDClassServerSynchronizeAttribute : Attribute
    {
        public bool ServerSynchronize;
        public NWDClassServerSynchronizeAttribute(bool sServerSynchronize)
        {
            this.ServerSynchronize = sServerSynchronize;
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDClassDescriptionAttribute : Attribute
    {
        public string Description;
        public NWDClassDescriptionAttribute(string sDescription)
        {
            this.Description = sDescription;
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDClassMenuNameAttribute : Attribute
    {
        public string MenuName;
        public NWDClassMenuNameAttribute(string sMenuName)
        {
            this.MenuName = sMenuName;
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
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
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property)]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDNeedAccountAvatarAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string PHPstring(string sPropertyName)
        {
            return "if ($uuid!= $tRow['" + sPropertyName + "']) {$ACC_NEEDED['NWDAccountAvatar'][$tRow['" + sPropertyName + "']]= true;}\n";
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property)]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDNeedUserAvatarAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string PHPstring(string sPropertyName)
        {
            return "if ($uuid!= $tRow['" + sPropertyName + "']) {$ACC_NEEDED['NWDUserAvatar'][$tRow['" + sPropertyName + "']]= true;}\n";
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property)]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDNeedAccountNicknameAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string PHPstring(string sPropertyName)
        {
            return "if ($uuid!= $tRow['" + sPropertyName + "']) {$ACC_NEEDED['NWDAccountNickname'][$tRow['"+sPropertyName+"']]= true;}\n";
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property)]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDNeedUserNicknameAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string PHPstring(string sPropertyName)
        {
            return "if ($uuid!= $tRow['" + sPropertyName + "']) {$ACC_NEEDED['NWDUserNickname'][$tRow['" + sPropertyName + "']]= true;}\n";
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property)]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDNeedReferenceAttribute : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public string ClassName;
        public Type ClassType;
        //-------------------------------------------------------------------------------------------------------------
        public NWDNeedReferenceAttribute(Type sClassType)
        {
            this.ClassName = sClassType.Name;
            this.ClassType = sClassType;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string PHPstring(string sPropertyName)
        {
            return "$REF_NEEDED['"+this.ClassName+"'][$tRow['" + sPropertyName + "']]= true;\n";
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================