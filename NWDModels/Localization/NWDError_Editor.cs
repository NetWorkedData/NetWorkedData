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

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDError : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        static NWDBasisHelper KBasisHelper;
        //-------------------------------------------------------------------------------------------------------------
        private static NWDBasisHelper GetBasisHelper()
        {
            if (KBasisHelper == null)
            {
                KBasisHelper = NWDBasisHelper.BasisHelper<NWDError>();
                //Debug.Log("DatasByReference count " + KBasisHelper.DatasByReference.Count);
            }
            return KBasisHelper;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDError CreateGenericError(string sDomain, string sCode, string sTitle, string sDescription, string sValidation, NWDErrorType sType = NWDErrorType.LogVerbose, NWDBasisTag sTag = NWDBasisTag.TagInternal)
        {
            NWDError tError = null;
            if (GetBasisHelper() != null)
            {
                string tReference = GetBasisHelper().ClassTrigramme + "-" + sDomain + NWEConstants.K_MINUS + sCode;
                if (GetBasisHelper().DatasByReference.ContainsKey(tReference) == true)
                {
                    //Debug.Log("find tReference " + tReference);
                    tError = GetBasisHelper().DatasByReference[tReference] as NWDError;
                }
                //if (tError != null && tError.IsTrashed())
                //{
                //    tError = null;
                //}
                if (tError == null)
                {
                    //Debug.Log("create tReference " + tReference);
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
                    tError.UpdateData(true, NWDWritingMode.ByEditorDefault);
                }
            }
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
                ShowAlert("TEST", delegate (NWDUserNotification sUserNotification)
                {
                    Debug.Log("Completed! block is running!");
                });
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