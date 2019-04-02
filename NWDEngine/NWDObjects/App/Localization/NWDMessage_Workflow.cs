//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
using UnityEngine;
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDMessage : NWDBasis<NWDMessage>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDMessage()
        {
            //Debug.Log("NWDMessage Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDMessage(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDMessage Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDMessage GetMessageWithCode(string sCode)
        {
            NWDMessage rReturn = null;
            foreach (NWDMessage tObject in NWDMessage.FindDatas())
            {
                if (tObject.Code == sCode)
                {
                    rReturn = tObject;
                    break;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDMessage GetMessageWithDomainAndCode(string sDomain, string sCode)
        {
            NWDMessage rReturn = null;
            foreach (NWDMessage tObject in NWDMessage.FindDatas())
            {
                if (tObject.Code == sCode && tObject.Domain == sDomain)
                {
                    rReturn = tObject;
                    break;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string Enrichment(string sText, string sLanguage = null, bool sBold = true)
        {
            string rText = NWDLocalization.Enrichment(sText, sLanguage, sBold);
            rText = NWDUserNickname.Enrichment(rText, sLanguage, sBold);
            rText = NWDAccountNickname.Enrichment(rText, sLanguage, sBold);
            return rText;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================