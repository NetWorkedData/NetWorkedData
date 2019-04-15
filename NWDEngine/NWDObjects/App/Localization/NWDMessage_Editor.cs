// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:33:54
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
using BasicToolBox;
using UnityEditor;
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDMessage : NWDBasis<NWDMessage>
    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWDMessage CreateGenericMessage(string sDomain, string sCode, string sTitle, string sDescription,
             string sValid ="Ok", string sCancel = "Cancel", NWDMessageType sType = NWDMessageType.InGame, NWDBasisTag sTag = NWDBasisTag.TagInternal)
        {
            string tReference = BasisHelper().ClassTrigramme + "-" + sDomain + BTBConstants.K_MINUS + sCode;
            // TODO: alert if reference is too long for ereg / or substring if too long
            NWDMessage tMessage = NWDMessage.RawDataByReference(tReference);
            //NWDMessage tError = InstanceByReference(tReference) as NWDMessage;
            if (tMessage == null)
            {
                tMessage = NWDBasis<NWDMessage>.NewData();
                //RemoveObjectInListOfEdition(tError);
                tMessage.Reference = tReference;
                //				tError.InternalKey = Domain + " : " + sCode;
                tMessage.InternalDescription = sDescription;
                tMessage.Tag = sTag;
                // domain code
                tMessage.Domain = sDomain;
                tMessage.Code = sCode;
                // title
                NWDLocalizableStringType tTitle = new NWDLocalizableStringType();
                tTitle.AddBaseString(sTitle);
                tMessage.Title = tTitle;
                // description
                NWDLocalizableTextType tDescription = new NWDLocalizableTextType();
                tDescription.AddBaseString(sDescription);
                tMessage.Description = tDescription;
                // validation
                NWDLocalizableStringType tValid = new NWDLocalizableStringType();
                tValid.AddBaseString(sValid);
                tMessage.Validation = tValid;
                // cancel
                NWDLocalizableStringType tCancel = new NWDLocalizableStringType();
                tCancel.AddBaseString(sCancel);
                tMessage.Cancel = tCancel;
                // type of message
                tMessage.Type = sType;
                // add-on edited
                tMessage.AddonEdited(true);
                // reccord
                tMessage.UpdateData();
                //AddObjectInListOfEdition(tError);
            }
            return tMessage;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool AddonEdited(bool sNeedBeUpdate)
        {
            if (sNeedBeUpdate == true)
            {
                if (Domain == null || Domain == string.Empty)
                {
                    Domain = "Unknow";
                }
                InternalKey = Domain + " : " + Code;
            }
            return sNeedBeUpdate;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditor(Rect sInRect)
        {
            // Draw the interface addon for editor
            // Draw the interface addon for editor
            float tWidth = sInRect.width;
            float tX = sInRect.x;
            float tY = sInRect.y;

            tY += NWDGUI.Separator(NWDGUI.MargeLeftRight(sInRect)).height;
            if (GUI.Button(new Rect(tX, tY, tWidth, NWDGUI.kMiniButtonStyle.fixedHeight), "Test message", NWDGUI.kMiniButtonStyle))
            {
                BTBDialog.Dialog(Enrichment(Title.GetLocalString()), Enrichment(Description.GetLocalString()), Validation.GetLocalString(), Cancel.GetLocalString(), null);
            }
            return tY;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight()
        {
            // Height calculate for the interface addon for editor
            float tYadd = NWDGUI.kMiniButtonStyle.fixedHeight;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif