//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:49:16
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDInAppPack : NWDBasis
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