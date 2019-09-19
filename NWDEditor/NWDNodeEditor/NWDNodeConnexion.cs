//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:22:57
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using System;
using System.Reflection;
using System.IO;

using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDNodePloter
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDNodeCard Card;
        public string Reference;
        public Vector2 Point;
        //-------------------------------------------------------------------------------------------------------------
        public NWDNodePloter(NWDNodeCard sCard, string sReference, Vector2 sPoint)
        {
            Card = sCard;
            Reference = sReference;
            Point = sPoint;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
