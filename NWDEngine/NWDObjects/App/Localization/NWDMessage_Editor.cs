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
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDMessage : NWDBasis<NWDMessage>
    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWDMessage CreateGenericMessage(string sDomain, string sCode, string sTitle, string sDescription)
        {
            string tReference = "MES-" + sDomain + BTBConstants.K_MINUS + sCode;
            // TODO: alert if reference is too long for ereg / or substring if too long
            NWDMessage tError = NWDMessage.GetDataByReference(tReference);
            //NWDMessage tError = InstanceByReference(tReference) as NWDMessage;
            if (tError == null)
            {
                tError = NWDBasis<NWDMessage>.NewData();
                //RemoveObjectInListOfEdition(tError);
                tError.Reference = tReference;
                //				tError.InternalKey = Domain + " : " + sCode;
                tError.InternalDescription = sDescription;
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
                tError.Message = tDescription;
                // add-on edited
                tError.AddonEdited(true);
                // reccord
                tError.UpdateData();
                //AddObjectInListOfEdition(tError);
            }
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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif