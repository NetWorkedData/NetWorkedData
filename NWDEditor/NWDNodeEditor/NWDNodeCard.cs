﻿using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using System;
using System.Reflection;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDNodeCard
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDTypeClass Data;
        public List<NWDNodeConnexion> ConnexionList = new List<NWDNodeConnexion>();
        public Vector2 Position;
        public float Width = 100.0F;
        public float Height = 100.0F;
        public float Margin = 40.0F;
        public float Line=0;
        public float Column=0;
        //-------------------------------------------------------------------------------------------------------------
        public void DrawLine()
        {
            foreach (NWDNodeConnexion tConnexion in ConnexionList)
            {
                tConnexion.DrawLine();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddPropertyResult(PropertyInfo sProperty , NWDTypeClass[] sObjectsArray)
        {
            NWDNodeConnexion tNewConnexion = new NWDNodeConnexion();
            tNewConnexion.Property = sProperty;
            tNewConnexion.Parent = this;
            foreach (NWDTypeClass tObject in sObjectsArray)
            {
                NWDNodeCard tCard = new NWDNodeCard();

                //tNewConnexion.ChildrenList.Add();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawCard()
        {
            GUI.Box(new Rect(Margin+Line*(Width+Margin),Margin+Column*(Height+Margin), Width, Height ),Data.GetType().AssemblyQualifiedName);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawPlot()
        {
            foreach (NWDNodeConnexion tConnexion in ConnexionList)
            {
                tConnexion.DrawPlot();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif