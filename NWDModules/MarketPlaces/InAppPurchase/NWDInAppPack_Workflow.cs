﻿//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDInAppPack : NWDBasis<NWDInAppPack>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDInAppPack()
        {
            //Debug.Log("NWDInAppPack Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDInAppPack(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDInAppPack Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_ClassInitialization)]
        public static void ClassInitialization() // call by invoke
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetIAPKey()
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return GoogleID;
            }
            else if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                return AppleID;
            }

            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================