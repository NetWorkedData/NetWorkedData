// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:26:2
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

using System.Collections.Generic;
using System.Reflection;

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
        //TODO : use in place of ...
        public static List<K> RawDatas()
        {
            return BasisHelper().Datas as List<K>;
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : use in place of ...
        public static K RawDataByReference(string sReference)
        {
            return BasisHelper().DatasByReference[sReference] as K;
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : use in place of ...
        public static List<K> RawDatasByInternalKey(string sInternalKey)
        {
            return BasisHelper().DatasByInternalKey[sInternalKey] as List<K>;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K[] FindDatas()
        {
            return FindDatas(NWDAccount.CurrentReference(), NWDGameSave.CurrentData());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K GetFirstData(string sAccountReference = null, NWDGameSave sGameSave = null)
        {
            K rReturn = null;
            K[] rDatas = FindDatas(sAccountReference, sGameSave);
            if (rDatas.Length > 0)
            {
                rReturn = rDatas[0];
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool IsReacheableByGameSave(NWDGameSave sGameSave)
        {
            bool rReturn = false;
            if (BasisHelper().GameSaveMethod != null)
            {
                string tGameIndex = "";
                if (sGameSave != null)
                {
                    tGameIndex = sGameSave.Reference;
                }
                var tValue = BasisHelper().ClassGameDependentProperties.GetValue(this, null);
                string tSaveIndex = BasisHelper().GameSaveMethod.Invoke(tValue, null) as string;
                if (tSaveIndex == tGameIndex)
                {
                    rReturn = true;
                }
            }
            else
            {
                rReturn = true;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool VisibleByGameSave(string sGameSaveReference)
        {
            bool rReturn = false;
            if (BasisHelper().GameSaveMethod != null)
            {
                var tValue = BasisHelper().ClassGameDependentProperties.GetValue(this, null);
                if (tValue != null)
                {
                    string tSaveIndex = BasisHelper().GameSaveMethod.Invoke(tValue, null) as string;
                    if (tSaveIndex == sGameSaveReference)
                    {
                        rReturn = true;
                    }
                }
            }
            else
            {
                rReturn = true;
            }
            return rReturn;
        }
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override bool VisibleByAccountByEqual(string sAccountReference)
        {
            bool rReturn = false;
            if (AccountDependent())
            {
                foreach (KeyValuePair<PropertyInfo, MethodInfo> tInfos in BasisHelper().AccountMethodDico)
                {
                    var tValue = tInfos.Key.GetValue(this, null);
                    if (tValue != null)
                    {
                        string tAccount = tInfos.Value.Invoke(tValue, null) as string;
                        if (tAccount == sAccountReference)
                        {
                            rReturn = true;
                            break; // I fonud one solution! this user can see this informations
                        }
                    }
                }
            }
            else
            {
                // non account dependency return acces is true.
                rReturn = true;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool VisibleByAccount(string sAccountReference)
        {
            bool rReturn = false;
            if (AccountDependent())
            {
                foreach (KeyValuePair<PropertyInfo, MethodInfo> tInfos in BasisHelper().AccountMethodDico)
                {
                    var tValue = tInfos.Key.GetValue(this, null);
                    if (tValue != null)
                    {
                        string tAccount = tInfos.Value.Invoke(tValue, null) as string;
                        if (tAccount.Contains(sAccountReference))
                        {
                            rReturn = true;
                            break; // I fonud one solution! this user can see this informations
                        }
                    }
                }
            }
            else
            {
                // non account dependency return acces is true.
                rReturn = true;
            }
            return rReturn;
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        public override bool IsReacheableByAccount(string sAccountReference = null)
        {
            bool rReturn = false;
            if (AccountDependent())
            {
                // is account dependency : get all value and test
                if (sAccountReference == null || sAccountReference == "") // TODO : replace by  string.IsNullOrEmpty
                {
                    sAccountReference = NWDAccount.CurrentReference();
                }
                foreach (KeyValuePair<PropertyInfo, MethodInfo> tInfos in BasisHelper().AccountMethodDico)
                {
                    var tValue = tInfos.Key.GetValue(this, null);
                    if (tValue != null)
                    {
                        string tAccount = tInfos.Value.Invoke(tValue, null) as string;
                        if (tAccount.Contains(sAccountReference))
                        {
                            rReturn = true;
                            break; // I fonud one solution! this user can see this informations
                        }
                    }
                }
            }
            else
            {
                // non account dependency return acces is true.
                rReturn = true;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================