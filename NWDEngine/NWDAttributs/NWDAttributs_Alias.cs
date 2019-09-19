//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:24:49
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Reflection;
using System.Collections.Generic;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class NWDAliasMethod : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Alias = string.Empty;
        //-------------------------------------------------------------------------------------------------------------
        private static Dictionary<Type, Dictionary<string, MethodInfo>> kCache = new Dictionary<Type, Dictionary<string, MethodInfo>>();
        //-------------------------------------------------------------------------------------------------------------
        public NWDAliasMethod(string sAlias)
        {
            this.Alias = sAlias;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static MethodInfo GetMethod(Type sType, string sAlias, BindingFlags sFlags)
        {
            MethodInfo rReturn = null;
            bool tAllreadyCache = false;
            if (kCache.ContainsKey(sType))
            {
                if (kCache[sType].ContainsKey(sAlias))
                {
                    tAllreadyCache = true;
                    rReturn = kCache[sType][sAlias];
                }
            }
            if (tAllreadyCache == false)
            {
                foreach (MethodInfo tProp in sType.GetMethods(sFlags))
                {
                    foreach (NWDAliasMethod tReference in tProp.GetCustomAttributes(typeof(NWDAliasMethod), true))
                    {
                        if (tReference.Alias == sAlias)
                        {
                            rReturn = tProp;
                        }
                    }
                }
                if (kCache.ContainsKey(sType) == false)
                {
                    kCache.Add(sType, new Dictionary<string, MethodInfo>());
                }
                if (kCache[sType].ContainsKey(sAlias) == false)
                {
                    kCache[sType].Add(sAlias, rReturn);
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static MethodInfo GetMethodPublicInstance(Type sType, string sAlias)
        {
            return GetMethod(sType, sAlias, BindingFlags.Public | BindingFlags.Instance);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static MethodInfo GetMethodPublicStaticFlattenHierarchy(Type sType, string sAlias)
        {
            return GetMethod(sType, sAlias, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool InvokeClassMethod(Type sType, string sAlias, object sSender = null, object[] sParameter = null)
        {
            bool rReturn = true;
            MethodInfo tMethodInfo = GetMethod(sType, sAlias, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (tMethodInfo != null)
            {
                tMethodInfo.Invoke(sSender, sParameter);
            }
            else
            {
                rReturn = false;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
    public class NWDAlias : Attribute
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string FindAliasName(Type sType, string sAlias)
        {
            string rReturn = sAlias;
            foreach (PropertyInfo tProp in sType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                foreach (NWDAlias tReference in tProp.GetCustomAttributes(typeof(NWDAlias), true))
                {
                    if (tReference.Alias == sAlias)
                    {
                        rReturn = tProp.Name;
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string FindAliasName<T>(string sAlias)
        {
            string rReturn = sAlias;
            foreach (PropertyInfo tProp in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                foreach (NWDAlias tReference in tProp.GetCustomAttributes(typeof(NWDAlias), true))
                {
                    if (tReference.Alias == sAlias)
                    {
                        rReturn = tProp.Name;
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        // use in NWDBasis with FindAliasName();
        public string Alias = string.Empty;
        public NWDAlias(string sAlias)
        {
            this.Alias = sAlias;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================