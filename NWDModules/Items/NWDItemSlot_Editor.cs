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
        public override float AddonEditor(Rect sInRect)
        {
            float tWidth = sInRect.width;
            float tX = sInRect.x;
            float tY = sInRect.y;
            EditorGUI.DrawRect(EditorGUI.IndentedRect(new Rect(tX, tY + NWDConstants.kFieldMarge, tWidth, 1)), NWDConstants.kRowColorLine);
            tY += NWDConstants.kFieldMarge * 2;

            List<NWDUserItemSlot> tUserItemSlotList = NWDUserItemSlot.FindByIndex(this.Reference);
            foreach(NWDUserItemSlot tUserItemSlot in tUserItemSlotList)
            {
               if (GUI.Button(new Rect(tX, tY, tWidth, NWDConstants.tMiniButtonStyle.fixedHeight), "Select User Item Slot", NWDConstants.tMiniButtonStyle))
                {
                    NWDDataInspector.InspectNetWorkedData(tUserItemSlot);
                    tY += NWDConstants.tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
                }
            }
            return tY;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight()
        {
            // Height calculate for the interface addon for editor
            float tY = 0.0f; 
            tY += NWDConstants.kFieldMarge * 2;
            List<NWDUserItemSlot> tUserItemSlotList = NWDUserItemSlot.FindByIndex(this.Reference);
            tY += tUserItemSlotList.Count*(NWDConstants.tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge);
            return tY;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddOnNodeDrawWidth(float sDocumentWidth)
        {
            return 200.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddOnNodeDrawHeight(float sCardWidth)
        {
            return 50.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddOnNodeDraw(Rect sRect, bool sPropertysGroup)
        {
            GUI.Label(sRect, InternalDescription);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif