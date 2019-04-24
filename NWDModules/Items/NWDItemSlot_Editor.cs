// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:48:38
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
using UnityEditor;
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDItemSlot : NWDBasis<NWDItemSlot>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override bool AddonEdited(bool sNeedBeUpdate)
        {
            if (sNeedBeUpdate == true)
            {
                // do something
            }
            return sNeedBeUpdate;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonEditor(Rect sRect)
        {
            float tWidth = sRect.width;
            float tX = sRect.x;
            float tY = sRect.y;

            tY += NWDGUI.Separator(NWDGUI.MargeLeftRight(sRect)).height;

            List<NWDUserItemSlot> tUserItemSlotList = NWDUserItemSlot.FindByIndex(this.Reference);
            foreach(NWDUserItemSlot tUserItemSlot in tUserItemSlotList)
            {
               if (GUI.Button(new Rect(tX, tY, tWidth, NWDGUI.kMiniButtonStyle.fixedHeight), "Select User Item Slot", NWDGUI.kMiniButtonStyle))
                {
                    NWDDataInspector.InspectNetWorkedData(tUserItemSlot);
                    tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight(float sWidth)
        {
            // Height calculate for the interface addon for editor
            float tY = 0.0f; 
            tY += NWDGUI.kFieldMarge * 2;
            List<NWDUserItemSlot> tUserItemSlotList = NWDUserItemSlot.FindByIndex(this.Reference);
            tY += tUserItemSlotList.Count*(NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge);
            return tY;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddOnNodeDrawHeight(float sCardWidth)
        {
            return 50.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddOnNodeDraw(Rect sRect)
        {
            GUI.Label(sRect, InternalDescription);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif