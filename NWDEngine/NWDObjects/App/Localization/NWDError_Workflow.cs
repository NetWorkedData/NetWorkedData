//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
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
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDError : NWDBasis<NWDError>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDError()
        {
            //Debug.Log("NWDError Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDError(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDError Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDError GetErrorWithCode(string sCode)
        {
            NWDError rReturn = null;
            foreach (NWDError tObject in NWDError.FindDatas())
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
        public static NWDError GetErrorWithDomainAndCode(string sDomain, string sCode)
        {
            NWDError rReturn = null;
            foreach (NWDError tObject in NWDError.FindDatas())
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
        //public static NWDError PostNotificationErrorWithDomainAndCode(string sDomain, string sCode)
        //{
        //    NWDError rReturn = NWDError.GetErrorWithDomainAndCode(sDomain, sCode);
        //    if (rReturn != null)
        //    {
        //        rReturn.PostNotificationError();
        //    }
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public static string Enrichment(string sText, string sLanguage = null, bool sBold = true)
        {

            string rText = NWDLocalization.Enrichment(sText, sLanguage, sBold);
            rText = NWDUserNickname.Enrichment(rText, sLanguage, sBold);
            rText = NWDAccountNickname.Enrichment(rText, sLanguage, sBold);
            return rText;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PostNotificationError()
        {
            BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_ERROR, this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ShowAlert(BTBAlertOnCompleteBlock sCompleteBlock = null)
        {
            NWDErrorType tType = Type;
            // NWDErrorType set by compile environment
#if UNITY_EDITOR
            // NO CHANGE
            //tType = NWDErrorType.UnityEditor;
#elif UNITY_IOS
            // NO CHANGE
#elif UNITY_ANDROID
            // NO CHANGE
#elif UNITY_STANDALONE_OSX
            // NO CHANGE
            //tType = NWDErrorType.InGame;
#elif UNITY_STANDALONE_WIN
            tType = NWDErrorType.InGame;
#elif UNITY_STANDALONE_LINUX
            tType = NWDErrorType.InGame;
#else
            tType = NWDErrorType.InGame;
#endif
            switch (tType)
            {
                case NWDErrorType.Alert:
                    {
                        BTBAlert.Alert(Enrichment(Title.GetLocalString()), Enrichment(Description.GetLocalString()), Validation.GetLocalString(), sCompleteBlock);
                    }
                    break;
                case NWDErrorType.Critical:
                    {
                        BTBAlert.Alert(Enrichment(Title.GetLocalString()), Enrichment(Description.GetLocalString()), Validation.GetLocalString(), delegate (BTBMessageState state)
                        {
                            Application.Quit();
                        }
                        );
                    }
                    break;
                case NWDErrorType.Upgrade:
                    {
                        BTBAlert.Alert(Enrichment(Title.GetLocalString()), Enrichment(Description.GetLocalString()), Validation.GetLocalString(), delegate (BTBMessageState state)
                        {
                            string tURL = "https://www.google.fr/search?q=" + NWDAppEnvironment.SelectedEnvironment().AppName;
                            NWDVersion tVersion = NWDVersion.GetActualVersion();
#if UNITY_EDITOR
                            // NO CHANGE
#elif UNITY_IOS
                            if (string.IsNullOrEmpty(tVersion.IOSStoreURL) == false)
                            {
                                tURL = tVersion.IOSStoreURL;
                            }
#elif UNITY_ANDROID
                            if (string.IsNullOrEmpty(tVersion.GooglePlayURL) == false)
                            {
                                tURL = tVersion.GooglePlayURL;
                            }
#elif UNITY_STANDALONE_OSX
                            if (string.IsNullOrEmpty(tVersion.OSXStoreURL) == false)
                            {
                                tURL = tVersion.OSXStoreURL;
                            }
#elif UNITY_STANDALONE_WIN

#elif UNITY_STANDALONE_LINUX

#else

#endif
                            Application.OpenURL(tURL);
                            Application.Quit();
                        });
                        // TODO : redirection to Store

                    }
                    break;
                case NWDErrorType.Ignore:
                    {
                        // Do nothing
                    }
                    break;
                case NWDErrorType.InGame:
                    {
                        // Do nothing
                        PostNotificationError();
                    }
                    break;
                case NWDErrorType.LogVerbose:
                    {
                        Debug.Log("ALERT! " + Title.GetLocalString() + " : " + Description.GetLocalString());
                    }
                    break;
                case NWDErrorType.LogWarning:
                    {
                        Debug.LogWarning("WARNING! " + Title.GetLocalString() + " : " + Description.GetLocalString());
                    }
                    break;
                case NWDErrorType.UnityEditor:
                    {
                        BTBAlert.Alert(Title.GetLocalString(), Description.GetLocalString(), Validation.GetLocalString(), sCompleteBlock);
                    }
                    break;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================