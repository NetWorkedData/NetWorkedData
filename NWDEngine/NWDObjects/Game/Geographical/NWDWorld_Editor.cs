﻿// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:41:38
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDWorld : NWDBasis<NWDWorld>
    {
        //-------------------------------------------------------------------------------------------------------------
        //public override float AddonEditorHeight()
        //{
        //    return NWDGUI.kBoldLabelStyle.fixedHeight*6;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonEditor(Rect sRect)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
