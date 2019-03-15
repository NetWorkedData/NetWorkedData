//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
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
        public NWDWorld()
        {
            //Debug.Log("NWDWorld Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDWorld(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDWorld Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }//-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight()
        {
            return NWDConstants.kBoldLabelStyle.fixedHeight*6;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditor(Rect sInRect)
        {
            // draw separator
            //Rect tRect = NWDConstants.GUIColorLine(new Rect(sInRect));
            //// draw title
            //tRect.height = NWDConstants.kBoldLabelStyle.fixedHeight;
            //GUI.Label(tRect, "Test Mask", NWDConstants.kBoldLabelStyle);
            //tRect.y+= NWDConstants.kBoldLabelStyle.fixedHeight;

            //// prepare for label standard
            //tRect.height = NWDConstants.kLabelStyle.fixedHeight;

            //// draw Constainst test
            //if (Flag.ContainsMask(Mask))
            //{
            //    GUI.Label(tRect,"Flag containts Mask = true");
            //}
            //else
            //{
            //    GUI.Label(tRect,"Flag containts Mask = false");
            //}
            //tRect.y += NWDConstants.kLabelStyle.fixedHeight;

            //if (Flag.IntersectsMask(Mask))
            //{
            //    GUI.Label(tRect,"Flag intersects Mask = true");
            //}
            //else
            //{
            //    GUI.Label(tRect,"Flag intersects Mask = false");
            //}
            //tRect.y += NWDConstants.kLabelStyle.fixedHeight;

            //if (Flag.ExcludesMask(Mask))
            //{
            //    GUI.Label(tRect,"Flag excludesMask Mask = true");
            //}
            //else
            //{
            //    GUI.Label(tRect, "Flag excludesMask Mask = false");
            //}
            //tRect.y += NWDConstants.kLabelStyle.fixedHeight;
            return base.AddonEditor(sInRect);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
