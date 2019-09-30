//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:33:46
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using SQLite4Unity3d;
#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public delegate void NWDUserNotificationDelegate(NWDUserNotification sNotification);
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDUserNotificationOrigin
    {
        Error,
        Message,
        InterMessage,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDUserNotification
    {
        //-------------------------------------------------------------------------------------------------------------
        private NWDUserNotificationDelegate _ValidationDelegate;
        private NWDUserNotificationDelegate _CancelDelegate;
        private NWDUserNotificationOrigin _Origin;
        //-------------------------------------------------------------------------------------------------------------
        private NWDError _Error;
        private string _Info = string.Empty;
        //-------------------------------------------------------------------------------------------------------------
        private string ErrorEnrichment(string sString)
        {
            return sString.Replace("XXX", _Info);
        }
        //-------------------------------------------------------------------------------------------------------------
        private NWDMessage _Message;
        //-------------------------------------------------------------------------------------------------------------
        private NWDUserInterMessage _UserMessage;
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserNotification(NWDError sError, string sInfo, NWDUserNotificationDelegate sCompleteBlock = null)
        {
            _Origin = NWDUserNotificationOrigin.Error;
            _Error = sError;
            _Info = sInfo;
            _ValidationDelegate = sCompleteBlock;
            _CancelDelegate = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserNotification(NWDMessage sMessage, NWDUserNotificationDelegate sValidationbBlock = null, NWDUserNotificationDelegate sCancelBlock = null)
        {
            _Origin = NWDUserNotificationOrigin.Message;
            _Message = sMessage;
            _ValidationDelegate = sValidationbBlock;
            _CancelDelegate = sCancelBlock;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserNotification(NWDUserInterMessage sUserInterMessage, NWDUserNotificationDelegate sValidationbBlock = null, NWDUserNotificationDelegate sCancelBlock = null)
        {
            _Origin = NWDUserNotificationOrigin.InterMessage;
            _UserMessage = sUserInterMessage;
            _ValidationDelegate = sValidationbBlock;
            _CancelDelegate = sCancelBlock;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserNotification PostNotification(NWDError sError, string sInfo, NWDUserNotificationDelegate sCompleteBlock = null)
        {
            NWDUserNotification rReturn = new NWDUserNotification(sError, sInfo, sCompleteBlock);
            rReturn.Post();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserNotification PostNotification(NWDMessage sMessage, NWDUserNotificationDelegate sValidationbBlock = null, NWDUserNotificationDelegate sCancelBlock = null)
        {
            NWDUserNotification rReturn = new NWDUserNotification(sMessage, sValidationbBlock, sCancelBlock);
            rReturn.Post();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserNotification PostNotification(NWDUserInterMessage sUserInterMessage, NWDUserNotificationDelegate sValidationbBlock = null, NWDUserNotificationDelegate sCancelBlock = null)
        {
            NWDUserNotification rReturn = new NWDUserNotification(sUserInterMessage, sValidationbBlock, sCancelBlock);
            rReturn.Post();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Post()
        {
            if (_Error != null)
            {
                NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_ERROR, this);
            }
            if (_Message != null)
            {
                NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_MESSAGE, this);
            }
            if (_UserMessage != null)
            {
                NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_NEWS_NOTIFICATION, this);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void ErrorPostComplete()
        {
            if (_Origin == NWDUserNotificationOrigin.Error)
            {
                if (_Error != null)
                {
                    switch (_Error.Type)
                    {
                        case NWDErrorType.Alert:
                            {
                            }
                            break;
                        case NWDErrorType.Critical:
                            {
                                NWDDataManager.SharedInstance().DataQueueExecute();
                                Application.Quit(9);
                            }
                            break;
                        case NWDErrorType.Upgrade:
                            {
                                NWDDataManager.SharedInstance().DataQueueExecute();
                                Application.OpenURL(NWDVersion.SelectMaxDataForEnvironment(NWDAppEnvironment.SelectedEnvironment()).URLMyApp(false));
                                Application.Quit(9);
                            }
                            break;
                        default:
                            {
                            }
                            break;
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void InterMessagePostComplete()
        {
            if (_UserMessage != null)
            {
                _UserMessage.Read = true;
                _UserMessage.SaveData();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void IsShowing()
        {
            if (_Error != null)
            {
            }
            if (_Message != null)
            {
            }
            if (_UserMessage != null)
            {
                _UserMessage.Distribute = true;
                _UserMessage.SaveData();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Validate()
        {
            if (_ValidationDelegate != null)
            {
                _ValidationDelegate(this);
            }
            ErrorPostComplete();
            InterMessagePostComplete();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Complete()
        {
            if (_ValidationDelegate != null)
            {
                _ValidationDelegate(this);
            }
            ErrorPostComplete();
            InterMessagePostComplete();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Cancel()
        {
            if (_CancelDelegate != null)
            {
                _CancelDelegate(this);
            }
            ErrorPostComplete();
            InterMessagePostComplete();
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserNotificationOrigin Origin()
        {
            return _Origin;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDErrorType ErrorType()
        {
            NWDErrorType rReturn = NWDErrorType.Ignore;
            if (_Error != null)
            {
                rReturn = _Error.Type;
            }
            if (_Message != null)
            {
            }
            if (_UserMessage != null)
            {
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDMessageType MessageType()
        {
            NWDMessageType rReturn = NWDMessageType.Trashable;
            if (_Error != null)
            {
            }
            if (_Message != null)
            {
                rReturn = _Message.Type;
            }
            if (_UserMessage != null)
            {
                rReturn = _UserMessage.Message.GetData().Type;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDMessageStyle MessageStyle()
        {
            NWDMessageStyle rReturn = NWDMessageStyle.kAlert;
            if (_Error != null)
            {
            }
            if (_Message != null)
            {
            }
            if (_UserMessage != null)
            {
                rReturn = _UserMessage.Message.GetData().Style;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string Domain()
        {
            string rReturn = string.Empty;
            if (_Error != null)
            {
                rReturn = _Error.Domain;
            }
            if (_Message != null)
            {
                rReturn = _Message.Domain;
            }
            if (_UserMessage != null)
            {
                rReturn = _UserMessage.Message.GetData().Domain;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string Code()
        {
            string rReturn = string.Empty;
            if (_Error != null)
            {
                rReturn = _Error.Code;
            }
            if (_Message != null)
            {
                rReturn = _Message.Code;
            }
            if (_UserMessage != null)
            {
                rReturn = _UserMessage.Message.GetData().Code;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string Title()
        {
            string rReturn = string.Empty;
            if (_Error != null)
            {
                rReturn = ErrorEnrichment(_Error.Title.GetLocalString());
            }
            if (_Message != null)
            {
                rReturn = _Message.Title.GetLocalString();
            }
            if (_UserMessage != null)
            {
                rReturn = _UserMessage.TitleRichText(false);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string Description()
        {
            string rReturn = string.Empty;
            if (_Error != null)
            {
                rReturn = ErrorEnrichment(_Error.Description.GetLocalString());
            }
            if (_Message != null)
            {
                rReturn = _Message.Description.GetLocalString();
            }
            if (_UserMessage != null)
            {
                rReturn = _UserMessage.MessageRichText(false);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string TextValidate()
        {
            string rReturn = string.Empty;
            if (_Error != null)
            {
                rReturn = ErrorEnrichment(_Error.Validation.GetLocalString());
            }
            if (_Message != null)
            {
                rReturn = _Message.Validation.GetLocalString();
            }
            if (_UserMessage != null)
            {
                rReturn = _UserMessage.ValidationRichText(false);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string TextComplete()
        {
            return TextValidate();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string TextCancel()
        {
            string rReturn = string.Empty;
            if (_Error != null)
            {
                rReturn = ErrorEnrichment(_Error.Validation.GetLocalString());
            }
            if (_Message != null)
            {
                rReturn = _Message.Cancel.GetLocalString();
            }
            if (_UserMessage != null)
            {
                rReturn = _UserMessage.CancelRichText(false);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
