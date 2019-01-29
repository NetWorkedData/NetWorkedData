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
            string tReference = "ERR-" + sDomain + BTBConstants.K_MINUS + sCode;
            // TODO: alert if reference is too long for ereg / or substring if too long
            NWDError tError = NWDError.GetDataByReference(tReference);
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
                // description
                NWDLocalizableStringType tValidation = new NWDLocalizableStringType();
                tValidation.AddBaseString(sValidation);
                tError.Validation = tValidation;
                // type of alert
                tError.Type = sType;
                // add-on edited
                tError.AddonEdited(true);
                // reccord
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
        public override float AddonEditor(Rect sInRect)
        {
            // Draw the interface addon for editor
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight()
        {
            // Height calculate for the interface addon for editor
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif