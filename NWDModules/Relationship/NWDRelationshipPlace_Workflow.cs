//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    public partial class NWDRelationshipPlace : NWDBasis<NWDRelationshipPlace>
    {
        //-------------------------------------------------------------------------------------------------------------
        const int K_CODE_LENGHT_MIN = 4; 
        const int K_CODE_LENGHT_MAX = 8;
        const int K_EXPIRE_TIME_MIN = 60;
        const int K_EXPIRE_TIME_MAX = 600;
        //-------------------------------------------------------------------------------------------------------------
        public NWDRelationshipPlace()
        {
            //Debug.Log("NWDRelationshipPlace Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDRelationshipPlace(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDRelationshipPlace Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            ExpireTime = K_EXPIRE_TIME_MIN;
            ExpireTime = K_EXPIRE_TIME_MIN;
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_OverrideClasseInThisSync)]
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDUserRelationship), typeof(NWDRelationshipPlace)};
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================