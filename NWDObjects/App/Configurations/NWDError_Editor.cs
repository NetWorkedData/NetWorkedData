//=====================================================================================================================
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
//=====================================================================================================================

using UnityEditor;
using UnityEngine;
//using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDError : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWDError CreateGenericError(string sDomain, string sCode, string sTitle, string sDescription, string sValidation, NWDErrorType sType = NWDErrorType.LogVerbose, NWDBasisTag sTag = NWDBasisTag.TagInternal)
        {
            NWDError tError = null;
            if (NWDBasisHelper.BasisHelper<NWDError>() != null)
            {
                //NWEBenchmark.Start();
                string tReference = NWDBasisHelper.BasisHelper<NWDError>().ClassTrigramme + "-" + sDomain + NWEConstants.K_MINUS + sCode;
                // TODO: alert if reference is too long for ereg / or substring if too long
                tError = NWDBasisHelper.GetRawDataByReference<NWDError>(tReference);
                if (tError != null && tError.IsTrashed())
                {
                    tError = null;
                }
                if (tError == null)
                {
                    tError = NWDBasisHelper.NewDataWithReference<NWDError>(tReference, true);
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

#if UNITY_EDITOR
                    // add-on edited
                    tError.AddonEdited(true);
#endif

                    // reccord
                    //tError.NotNullChecker();
                    tError.UpdateData(true, NWDWritingMode.ByEditorDefault);
                    //AddObjectInListOfEdition(tError);
                }
            }
            //NWEBenchmark.Finish();
            return tError;
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
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
                if (Application.isPlaying)
                {
                    this.ShowAlert("TEST");
                }
                else
                {
                    //NWEAlert.Alert(Title.GetLocalString(), Description.GetLocalString(), Validation.GetLocalString(), null);
                    if (EditorUtility.DisplayDialog(Title.GetLocalString(), Description.GetLocalString(), Validation.GetLocalString()) == true)
                    {
                    }
                }
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
#endif
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================