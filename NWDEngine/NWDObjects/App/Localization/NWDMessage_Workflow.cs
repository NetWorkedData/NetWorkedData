// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:33:56
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
        public static string Enrichment(string sText, string sLanguage = null, bool sBold = true)
        {
            string rText = NWDLocalization.Enrichment(sText, sLanguage, sBold);
            rText = NWDUserNickname.Enrichment(rText, sBold);
            rText = NWDAccountNickname.Enrichment(rText, sLanguage, sBold);
            return rText;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PostNotificationError()
        {
            BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_MESSAGE, this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ShowAlert(BTBDialogOnCompleteBlock sCompleteBlock = null)
        {
            NWDMessageType tType = Type;
            // NWDErrorType set by compile environment
#if UNITY_EDITOR
            // NO CHANGE
            //tType = NWDMessageType.Alert;
#elif UNITY_IOS
            // NO CHANGE
#elif UNITY_ANDROID
            // NO CHANGE
#elif UNITY_STANDALONE_OSX
            // NO CHANGE
            //tType = NWDMessageType.InGame;
#elif UNITY_STANDALONE_WIN
            tType = NWDMessageType.InGame;
#elif UNITY_STANDALONE_LINUX
            tType = NWDMessageType.InGame;
#else
            tType = NWDMessageType.InGame;
#endif
            switch (tType)
            {
                case NWDMessageType.Alert:
                    {
                        BTBDialog.Dialog(Enrichment(Title.GetLocalString()), Enrichment(Description.GetLocalString()), Validation.GetLocalString(), Cancel.GetLocalString(), sCompleteBlock);
                    }
                    break;
                case NWDMessageType.InGame:
                    {
                        PostNotificationError();
                    }
                    break;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================