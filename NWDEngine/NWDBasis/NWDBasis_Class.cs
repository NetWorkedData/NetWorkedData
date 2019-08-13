//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:25:51
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
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
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        static NWDBasis()
        {
            //Debug.Log("NWDBasis Static Class Constructor()");
            NWDTypeLauncher.Launcher();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static NWDBasisHelper BasisHelper()
        //{
        //    NWDBasisHelper rHelper= NWDBasisHelper.FindTypeInfos(typeof(K));
        //    if (rHelper == null)
        //    {
        //        Debug.LogWarning("ERROR NWDBasisHelper.FindTypeInfos(typeof(K)) NOT RETURN FOR " + typeof(K).Name);
        //    }
        //    return rHelper;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public static string ClassID()
        {
            string tReturn = typeof(K).Name;
            return tReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Type ClassType()
        {
            return typeof(K);
        }
        //-------------------------------------------------------------------------------------------------------------
        //        public static void AccountDependentAnalyze()
        //        {
        //            //BTBBenchmark.Start();
        //            bool rAccountConnected = false;
        //            bool rAssetConnected = false;
        //            bool rLockedObject = true;
        //            List<PropertyInfo> tPropertyList = new List<PropertyInfo>();
        //            List<PropertyInfo> tPropertyListConnected = new List<PropertyInfo>();
        //            List<PropertyInfo> tAssetPropertyList = new List<PropertyInfo>();
        //            Dictionary<PropertyInfo, MethodInfo> tAccountMethodList = new Dictionary<PropertyInfo, MethodInfo>();
        //            Type tType = ClassType();

        //            BasisHelper().ClassGameSaveDependent = false;
        //            BasisHelper().ClassGameDependentProperties = null;
        //            BasisHelper().GameSaveMethod = null;
        //            // TODO : check 
        //            // exception for NWDAccount table
        //            if (tType == typeof(NWDAccount) || tType == typeof(NWDRequestToken))
        //            {
        //                rAccountConnected = true;
        //            }

        //            foreach (PropertyInfo tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        //            {
        //                Type tTypeOfThis = tProp.PropertyType;
        //                if (tTypeOfThis != null)
        //                {
        //                    if (tTypeOfThis.IsGenericType)
        //                    {
        //                        if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferenceType<>))
        //                        {
        //                            Type tSubType = tTypeOfThis.GetGenericArguments()[0];
        //                            if (tSubType == typeof(NWDAccount))
        //                            {
        //                                tPropertyList.Add(tProp);
        //                                tPropertyListConnected.Add(tProp);
        //                                MethodInfo tMethod = tSubType.GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance);
        //                                tAccountMethodList.Add(tProp, tMethod);
        //                                rAccountConnected = true;
        //                                rLockedObject = false;
        //                            }
        //                            if (tSubType == typeof(NWDGameSave))
        //                            {
        //                                BasisHelper().ClassGameSaveDependent = true;
        //                                BasisHelper().ClassGameDependentProperties = tProp;
        //                                MethodInfo tGameSaveMethod = tSubType.GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance);
        //                                BasisHelper().GameSaveMethod = tGameSaveMethod;
        //                            }
        //                        }
        //                        else if (
        //                            tTypeOfThis.IsSubclassOf(typeof(NWDReferenceMultiple))
        //                        //|| tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesAmountType<>)
        //                        //|| tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesArrayType<>)
        //                        //|| tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesAverageType<>)
        //                        //|| tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesConditionalType<>)
        //                        //|| tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesListType<>)
        //                        //|| tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesQuantityType<>)
        //                        //|| tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferencesRangeType<>)
        //                        )
        //                        {
        //                            Type tSubType = tTypeOfThis.GetGenericArguments()[0];
        //                            if (tSubType == typeof(NWDAccount))
        //                            {
        //                                // it's not directly a NWDAccount a dependency ....
        //                                tPropertyListConnected.Add(tProp);
        //                                MethodInfo tMethod = tSubType.GetMethod("ToString", BindingFlags.Public | BindingFlags.Instance);
        //                                tAccountMethodList.Add(tProp, tMethod);
        //                            }
        //                        }
        //                        else if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferenceHashType<>))
        //                        {
        //                            Type tSubType = tTypeOfThis.GetGenericArguments()[0];
        //                            if (tSubType == typeof(NWDAccount))
        //                            {
        //                                // it's not directly a NWDAccount a dependency ....
        //                                // I don't know what I must do with in this case..
        //                            }
        //                        }
        //                    }
        //                    else if (tTypeOfThis.IsSubclassOf(typeof(NWDAssetType)))
        //                    {
        //                        rAssetConnected = true;
        //                        tAssetPropertyList.Add(tProp);
        //                    }
        //                }
        //            }

        //            BasisHelper().kAccountDependent = rAccountConnected;
        //            // reccord class' object is account dependent properties
        //            BasisHelper().kAccountDependentProperties = tPropertyList.ToArray();

        //            // reccord class' object is account connected properties
        //            BasisHelper().kAccountConnectedProperties = tPropertyListConnected.ToArray();
        //            BasisHelper().AccountMethodDico = tAccountMethodList;

        //            // reccord if class' object is locked for editor

        //#if UNITY_EDITOR
        //            rLockedObject = false;
        //#endif
        //            BasisHelper().kLockedObject = rLockedObject;

        //            // reccord if class' object is asset dependent
        //            BasisHelper().kAssetDependent = rAssetConnected;
        //            BasisHelper().kAssetDependentProperties = tAssetPropertyList.ToArray();
        //            //BTBBenchmark.Finish();
        //        }
        //-------------------------------------------------------------------------------------------------------------
        public static PropertyInfo[] PropertiesAccountDependent()
        {
            return BasisHelper().kAccountDependentProperties;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static PropertyInfo[] PropertiesAccountConnected()
        {
            return BasisHelper().kAccountConnectedProperties;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool AccountConnected()
        {
            bool rReturn = false;
            if (BasisHelper().kAccountConnectedProperties.Length > 0)
            {
                rReturn = true;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool AccountDependent()
        {
            return BasisHelper().kAccountDependent;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool GameSaveDependent()
        {
            return BasisHelper().ClassGameSaveDependent;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool IsClassLockedObject()
        {
            return BasisHelper().kLockedObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool AssetDependent()
        {
            return BasisHelper().kAssetDependent;
        }
        //----------------------------------------------
        public static PropertyInfo[] PropertiesAssetDependent()
        {
            return BasisHelper().kAssetDependentProperties;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool DatabaseIsLoaded()
        {
            bool rLoaded = true;
            if (AccountDependent() == true && NWDDataManager.SharedInstance().DataAccountLoaded == false)
            {
                rLoaded = false;
            }
            else if (AccountDependent() == false && NWDDataManager.SharedInstance().DataEditorLoaded == false)
            {
                rLoaded = false;
            }
            return rLoaded;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public override bool IsLockedObject() // return true during the player game
        {
            return BasisHelper().kLockedObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================