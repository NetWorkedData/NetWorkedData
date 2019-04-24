// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:33:44
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
using UnityEngine;
using BasicToolBox;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDError : NWDBasis<NWDError>
    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWDError CreateGenericError(string sDomain, string sCode, string sTitle, string sDescription, string sValidation, NWDErrorType sType = NWDErrorType.LogVerbose, NWDBasisTag sTag = NWDBasisTag.TagInternal)
        {
            //BTBBenchmark.Start();
            string tReference = BasisHelper().ClassTrigramme + "-" + sDomain + BTBConstants.K_MINUS + sCode;
            // TODO: alert if reference is too long for ereg / or substring if too long
            NWDError tError = NWDError.GetRawDataByReference(tReference);
            if (tError != null && tError.IsTrashed())
            {
                tError = null;
            }
            if (tError == null)
            {
                tError = NWDBasis<NWDError>.NewDataWithReference(tReference, true);
                //RemoveObjectInListOfEdition(tError);
                tError.Reference = tReference;
                // tError.InternalKey = Domain + " : " + sCode;
                tError.InternalDescription = sDescription;
                tError.Tag = sTag;
                // domain code
                tError.Domain = sDomain;
                tError.Code = sCode;
                // title
                NWDLocalizableStringType tTitle = new NWDLocalizableStringType();
                tTitle.AddBaseString(sTitle);
                tError.Title = tTitle;
                // description
                NWDLocalizableTextType tDescription = new NWDLocalizableTextType();
                tDescription.AddBaseString(sDescription);
                tError.Description = tDescription;
                // validation
                NWDLocalizableStringType tValidation = new NWDLocalizableStringType();
                tValidation.AddBaseString(sValidation);
                tError.Validation = tValidation;
                // type of alert
                tError.Type = sType;
                // add-on edited
                tError.AddonEdited(true);
                // reccord
                //tError.NotNullChecker();
                tError.UpdateData(true, NWDWritingMode.ByEditorDefault);
                //AddObjectInListOfEdition(tError);
            }
            //BTBBenchmark.Finish();
            return tError;
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
        public override void AddonEditor(Rect sRect)
        {
            // Draw the interface addon for editor
            float tWidth = sRect.width;
            float tX = sRect.x;
            float tY = sRect.y;

            tY += NWDGUI.Separator(NWDGUI.MargeLeftRight(sRect)).height;
            if (GUI.Button(new Rect(tX, tY, tWidth, NWDGUI.kMiniButtonStyle.fixedHeight), "Test error", NWDGUI.kMiniButtonStyle))
            {
                BTBAlert.Alert(Title.GetLocalString(), Description.GetLocalString(), Validation.GetLocalString(), null);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight(float sWidth)
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