﻿//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:24:52
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
//using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBundle : NWEDataTypeEnumGeneric<NWDBundle>
    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWDBundle ALL = Add(-1, "ALL");
        //-------------------------------------------------------------------------------------------------------------
        public static NWDBundle Translate = Add(1, "Splashscreen");
        public static NWDBundle Asset = Add(2, "Home");
        public static NWDBundle Description = Add(3, "Credits");
        public static NWDBundle Test = Add(4, "Licence");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================