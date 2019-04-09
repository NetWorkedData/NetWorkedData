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

using ZXing;
using ZXing.QrCode;
using UnityEngine.Networking;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDVersion : NWDBasis<NWDVersion>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDVersion()
        {
            //Debug.Log("NWDVersion Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDVersion(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDVersion Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override int WebModelToUse()
        {
            return 0;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdatedMe()
        {
            // do something when object finish to be updated
#if UNITY_EDITOR
            NWDVersion.UpdateVersionBundle();
            NWDDataManager.SharedInstance().RepaintWindowsInManager(typeof(NWDVersion));
            QRCodeTexture = FlashMyApp(false, 256);
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RecommendationBy(NWDRecommendationType sType)
        {
            //Debug.Log("NWDVersion RecommendationBy()");
            NWDVersion tVersion = FindMaxVersionByEnvironment(NWDAppEnvironment.SelectedEnvironment());

            string tToFlash = tVersion.URLMyApp(false);
            switch (sType)
            {
                case NWDRecommendationType.SMS:
                    {
                        string tText = tVersion.Recommendation.GetLocalString() + "\r\n";
                        tText += " Magic link : " + tToFlash + "\r\n";
                        if (string.IsNullOrEmpty(tVersion.OSXStoreURL) == false)
                        {
                            tText += " OSX : " + tVersion.OSXStoreURL + "\r\n";
                        }
                        if (string.IsNullOrEmpty(tVersion.IOSStoreURL) == false)
                        {
                            tText += " IOS : " + tVersion.IOSStoreURL + "\r\n";
                        }
                        if (string.IsNullOrEmpty(tVersion.GooglePlayURL) == false)
                        {
                            tText += " Android : " + tVersion.GooglePlayURL + "\r\n";
                        }
                        //tText = tText.Replace(";", "");
                        tText = UnityWebRequest.EscapeURL(tText).Replace("+", "%20");

                        string tSubject = tVersion.RecommendationSubject.GetLocalString();
                        tSubject = UnityWebRequest.EscapeURL(tSubject).Replace("+", "%20");
                        string tSMS = "sms:?body=" + tText;
                        //Debug.Log("NWDVersion RecommendationBy SMS => " + tSMS);
                        Application.OpenURL(tSMS);
                    }
                    break;
                case NWDRecommendationType.Email:
                    {

                        string tText = tVersion.Recommendation.GetLocalString() + "\n\r";
                        tText += " Magic link : " + tToFlash + "\r\n";
                        if (string.IsNullOrEmpty(tVersion.OSXStoreURL) == false)
                        {
                            tText += " OSX : " + tVersion.OSXStoreURL + "\n\r";
                        }
                        if (string.IsNullOrEmpty(tVersion.IOSStoreURL) == false)
                        {
                            tText += " IOS : " + tVersion.IOSStoreURL + "\n\r";
                        }
                        if (string.IsNullOrEmpty(tVersion.GooglePlayURL) == false)
                        {
                            tText += " Android : " + tVersion.GooglePlayURL + "\n\r";
                        }
                        tText += string.Empty;
                        tText = UnityWebRequest.EscapeURL(tText).Replace("+", "%20");

                        string tSubject = tVersion.RecommendationSubject.GetLocalString();
                        tSubject = UnityWebRequest.EscapeURL(tSubject).Replace("+", "%20");
                        string tEmail = "mailto:?subject=" + tSubject + "&body=" + tText;
                        //Debug.Log("NWDVersion RecommendationBy Email => " + tEmail);
                        Application.OpenURL(tEmail);
                    }
                    break;
                case NWDRecommendationType.EmailHTML:
                    {
                        string tText = string.Empty;
                        //tText+= "<HTML><BODY>";
                        tText += tVersion.Recommendation.GetLocalString() + "</BR>";
                        tText += " Magic link : <A HREF='" + tToFlash + "'> got to store</A></BR>";
                        if (string.IsNullOrEmpty(tVersion.OSXStoreURL) == false)
                        {
                            tText += " OSX : <A HREF='" + tVersion.OSXStoreURL + "'>Apple Store</A></BR>";
                        }
                        if (string.IsNullOrEmpty(tVersion.IOSStoreURL) == false)
                        {
                            tText += " IOS : <A HREF='" + tVersion.IOSStoreURL + "'>" + tVersion.IOSStoreURL + "</A></BR>";
                        }
                        if (string.IsNullOrEmpty(tVersion.GooglePlayURL) == false)
                        {
                            tText += " Android : <A HREF='" + tVersion.GooglePlayURL + "'>" + tVersion.GooglePlayURL + "</A></BR>";
                        }
                        //tText += "</BODY></HTML>";
                        //tText = tText.Replace("<", "");
                        //tText = tText.Replace(">", "");
                        tText = UnityWebRequest.EscapeURL(tText).Replace("+", "%20");

                        string tSubject = tVersion.RecommendationSubject.GetLocalString();
                        tSubject = UnityWebRequest.EscapeURL(tSubject).Replace("+", "%20");
                        string tEmail = "mailto:?subject=" + tSubject + "&body=" + tText;
                        //Debug.Log("NWDVersion RecommendationBy Email => " + tEmail);
                        Application.OpenURL(tEmail);
                    }
                    break;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public Texture2D FlashMyApp(bool sRedirection, int sDimension)
        {
            Texture2D rTexture = new Texture2D(sDimension, sDimension);
            var color32 = Encode(URLMyApp(sRedirection), rTexture.width, rTexture.height);
            rTexture.SetPixels32(color32);
            rTexture.Apply();
            return rTexture;
        }
        //-------------------------------------------------------------------------------------------------------------
        private static Color32[] Encode(string textForEncoding, int width, int height)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width
                }
            };
            return writer.Write(textForEncoding);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string URLMyApp(bool sRedirection)
        {
            string tText = NWDAppEnvironment.SelectedEnvironment().ServerHTTPS;
            tText += NWDAppConfiguration.SharedInstance().WebServiceFolder();
            if (sRedirection == true)
            {
                tText += "/" + NWD.K_STATIC_FLASH_PHP + "?r=0";
            }
            else
            {
                tText += "/" + NWD.K_STATIC_FLASH_PHP + "?r=1";
            }
            if (string.IsNullOrEmpty(OSXStoreID) == false)
            {
                tText += "&m=" + OSXStoreID;
            }

            if (string.IsNullOrEmpty(IOSStoreID) == false)
            {
                tText += "&a=" + IOSStoreID;
            }
            if (string.IsNullOrEmpty(IPadStoreID) == false)
            {
                tText += "&b=" + IPadStoreID;
            }

            if (string.IsNullOrEmpty(GooglePlayID) == false)
            {
                tText += "&g=" + GooglePlayID;
            };
            if (string.IsNullOrEmpty(GooglePlayTabID) == false)
            {
                tText += "&h=" + GooglePlayTabID;
            };

            if (string.IsNullOrEmpty(WindowsPhoneID) == false)
            {
                tText += "&w=" + WindowsPhoneID;
            };
            if (string.IsNullOrEmpty(WindowsStoreID) == false)
            {
                tText += "&x=" + WindowsStoreID;
            };

            if (string.IsNullOrEmpty(SteamStoreID) == false)
            {
                tText += "&s=" + SteamStoreID;
            };
            return tText;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetMaxVersion()
        {
            //Debug.Log("NWDVersion GetMaxVersion()");
            return GetMaxVersionStringForEnvironemt(NWDAppEnvironment.SelectedEnvironment());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetMaxVersionStringForEnvironemt(NWDAppEnvironment sEnvironment)
        {
            //Debug.Log("NWDVersion GetMaxVersionStringForEnvironemt()");
            string tVersionString = "0.00.00";
            NWDVersion tVersion = FindMaxVersionByEnvironment(sEnvironment);
            if (tVersion != null)
            {
                tVersionString = tVersion.Version.ToString();
            }
            return tVersionString;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================