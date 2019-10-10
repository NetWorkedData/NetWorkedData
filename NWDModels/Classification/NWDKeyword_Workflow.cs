﻿//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:34:27
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDKeyword : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDKeyword()
        {
            //Debug.Log("NWDKeyword Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDKeyword(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDKeyword Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================