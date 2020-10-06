//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassTrigrammeAttribute("XYZ")]
    [NWDClassDescriptionAttribute("Basis descriptions Class")]
    [NWDClassMenuNameAttribute("Basis")]
    public partial class NWDBasis : NWDTypeClass
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        static NWDBasis()
        {
            //Debug.Log("NWDBasis Static Class Constructor()");
        }
        //-------------------------------------------------------------------------------------------------------------
        private NWDBasisHelper _BasisHelper;
        //-------------------------------------------------------------------------------------------------------------
        public NWDBasisHelper BasisHelper()
        {
            if (_BasisHelper == null)
            {
                _BasisHelper = NWDBasisHelper.TypesDictionary[GetType()];
            }
            return _BasisHelper;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Type ClassType()
        {
            return BasisHelper().ClassType;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public bool AccountDependent()
        //{
        //    return BasisHelper().kAccountDependent;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public bool GameSaveDependent()
        //{
        //    return BasisHelper().ClassGameSaveDependent;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public bool IsClassLockedObject()
        {
            return BasisHelper().kLockedObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool AssetDependent()
        {
            return BasisHelper().kAssetDependent;
        }
        //----------------------------------------------
        public List<PropertyInfo> PropertiesAssetDependent()
        {
            return BasisHelper().kAssetDependentProperties;
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
