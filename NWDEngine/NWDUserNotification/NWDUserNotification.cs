//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

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
    public partial class NWDUserNotification
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
#if NWD_INTERMESSAGE
            if (_UserMessage != null)
            {
                NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_NEWS_NOTIFICATION, this);
            }
#endif
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
                                Application.OpenURL(NWDVersion.SelectMaxRecheableDataForEnvironment(NWDAppEnvironment.SelectedEnvironment()).URLMyApp(false));
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
#if NWD_INTERMESSAGE
            if (_UserMessage != null)
            {
                _UserMessage.Read = true;
                _UserMessage.SaveData();
            }
#endif
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
#if NWD_INTERMESSAGE
            if (_UserMessage != null)
            {
                _UserMessage.Distribute = true;
                _UserMessage.SaveData();
            }
#endif
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
#if NWD_INTERMESSAGE
            if (_UserMessage != null)
            {
            }
#endif
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

#if NWD_INTERMESSAGE
            if (_UserMessage != null)
            {
                rReturn = _UserMessage.Message.GetRawData().Type;
            }
#endif
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
#if NWD_INTERMESSAGE
            if (_UserMessage != null)
            {
                rReturn = _UserMessage.Message.GetRawData().Style;
            }
#endif
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
#if NWD_INTERMESSAGE
            if (_UserMessage != null)
            {
                rReturn = _UserMessage.Message.GetRawData().Domain;
            }
#endif
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
#if NWD_INTERMESSAGE
            if (_UserMessage != null)
            {
                rReturn = _UserMessage.Message.GetRawData().Code;
            }
#endif
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
#if NWD_INTERMESSAGE
            if (_UserMessage != null)
            {
                rReturn = _UserMessage.TitleRichText(false);
            }
#endif
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
#if NWD_INTERMESSAGE
            if (_UserMessage != null)
            {
                rReturn = _UserMessage.MessageRichText(false);
            }
#endif
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
#if NWD_INTERMESSAGE
            if (_UserMessage != null)
            {
                rReturn = _UserMessage.ValidationRichText(false);
            }
#endif
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
#if NWD_INTERMESSAGE
            if (_UserMessage != null)
            {
                rReturn = _UserMessage.CancelRichText(false);
            }
#endif
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
