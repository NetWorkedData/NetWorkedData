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
    //public partial class NWDBasisBundle : NWEDataTypeMaskGeneric<NWDBasisBundle>
            public partial class NWDBasisBundle : NWEDataTypeEnumGeneric<NWDBasisBundle>

    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWDBasisBundle Translate = Add(1, "Splashscreen");
        public static NWDBasisBundle Asset = Add(2, "Home");
        public static NWDBasisBundle Description = Add(3, "Credits");
        public static NWDBasisBundle Test = Add(4, "Licence");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
