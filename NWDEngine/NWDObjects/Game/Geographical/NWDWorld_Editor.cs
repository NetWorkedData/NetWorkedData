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
        public override float AddonEditorHeight()
        {
            return NWDGUI.kBoldLabelStyle.fixedHeight*6;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditor(Rect sInRect)
        {
            // draw separator
            //Rect tRect = NWDConstants.GUIColorLine(new Rect(sInRect));
            //// draw title
            //tRect.height = NWDGUI.tBoldLabelStyle.fixedHeight;
            //GUI.Label(tRect, "Test Mask", NWDGUI.tBoldLabelStyle);
            //tRect.y+= NWDGUI.tBoldLabelStyle.fixedHeight;

            //// prepare for label standard
            //tRect.height = NWDGUI.tLabelStyle.fixedHeight;

            //// draw Constainst test
            //if (Flag.ContainsMask(Mask))
            //{
            //    GUI.Label(tRect,"Flag containts Mask = true");
            //}
            //else
            //{
            //    GUI.Label(tRect,"Flag containts Mask = false");
            //}
            //tRect.y += NWDGUI.tLabelStyle.fixedHeight;

            //if (Flag.IntersectsMask(Mask))
            //{
            //    GUI.Label(tRect,"Flag intersects Mask = true");
            //}
            //else
            //{
            //    GUI.Label(tRect,"Flag intersects Mask = false");
            //}
            //tRect.y += NWDGUI.tLabelStyle.fixedHeight;

            //if (Flag.ExcludesMask(Mask))
            //{
            //    GUI.Label(tRect,"Flag excludesMask Mask = true");
            //}
            //else
            //{
            //    GUI.Label(tRect, "Flag excludesMask Mask = false");
            //}
            //tRect.y += NWDGUI.tLabelStyle.fixedHeight;
            return base.AddonEditor(sInRect);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
