//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections.Generic;
using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public bool IsReacheableByGameSave(NWDGameSave sGameSave)
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
        public bool VisibleByGameSave(string sGameSaveReference)
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
        public bool VisibleByAccountByEqual(string sAccountReference)
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
        public bool VisibleByAccount(string sAccountReference)
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
        public bool IsReacheableByAccount(string sAccountReference = null)
        {
            bool rReturn = false;
            if (AccountDependent())
            {
                // is account dependency : get all value and test
                if (sAccountReference == null || sAccountReference == "") // TODO : replace by  string.IsNullOrEmpty
                {
                    sAccountReference = NWDAccount.GetCurrentAccountReference();
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
}
//=====================================================================================================================