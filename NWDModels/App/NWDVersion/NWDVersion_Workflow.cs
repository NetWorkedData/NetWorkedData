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
using UnityEngine.Networking;
#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDVersionHelper : NWDHelper<NWDVersion>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override void ClassDatasAreLoaded()
        {
            base.ClassDatasAreLoaded();
#if UNITY_EDITOR
            // Add version by default, the version 0.00.00 of application
            NWDVersion.CheckDefaultVersion();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDVersion : NWDBasis
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
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
            //NWDBenchmark.Start();
            base.Initialization();
            if (Version != null)
            {
                NWDVersion tMaxVersion = SelectMaxRecheableData();
                if (tMaxVersion != null)
                {
                    Version.SetIntValue(tMaxVersion.Version.ToInt() + 1);
                }
                InternalKey = Version.ToString();
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override int WebModelToUse()
        {
            return 0;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonTrashMe()
        {
            //NWDBenchmark.Start();
#if UNITY_EDITOR
            //PreventDefaultVersion(this);
#endif
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUnTrashMe()
        {
            //NWDBenchmark.Start();
#if UNITY_EDITOR
            //PreventDefaultVersion(this);
#endif
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonEnableMe()
        {
            //NWDBenchmark.Start();
#if UNITY_EDITOR
            //PreventDefaultVersion(this);
#endif
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDisableMe()
        {
            //NWDBenchmark.Start();
#if UNITY_EDITOR
            //PreventDefaultVersion(this);
#endif
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDeletedMe()
        {
            //NWDBenchmark.Start();
#if UNITY_EDITOR
            GetDefaultVersion();
#endif
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            //NWDBenchmark.Start();
            InternalKey = Version.ToString();
#if UNITY_EDITOR
            // Add QRCodeTexture
            //QRCodeTexture = FlashMyApp(false, 256);
            // Prevent Default Version if it's default value
            PreventDefaultVersion(this);
#endif
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdatedMe()
        {
            //NWDBenchmark.Start();
            // do something when object finish to be updated
#if UNITY_EDITOR
            // update bundle version
            NWDVersion.UpdateVersionBundle();
            // refresh window
            NWDDataManager.SharedInstance().RepaintWindowsInManager(typeof(NWDVersion));
#endif
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RecommendationBy(NWDRecommendationType sType)
        {
            //Debug.Log("NWDVersion RecommendationBy()");
            NWDVersion tVersion = SelectMaxRecheableDataForEnvironment(NWDAppEnvironment.SelectedEnvironment());

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
        //public Texture2D FlashMyApp(bool sRedirection, int sDimension)
        //{
        //    Texture2D rTexture = new Texture2D(sDimension, sDimension);
        //    var color32 = Encode(URLMyApp(sRedirection), rTexture.width, rTexture.height);
        //    rTexture.SetPixels32(color32);
        //    rTexture.Apply();
        //    return rTexture;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //private static Color32[] Encode(string textForEncoding, int width, int height)
        //{
        //    var writer = new BarcodeWriter
        //    {
        //        Format = BarcodeFormat.QR_CODE,
        //        Options = new QrCodeEncodingOptions
        //        {
        //            Height = height,
        //            Width = width
        //        }
        //    };
        //    return writer.Write(textForEncoding);
        //}
        //-------------------------------------------------------------------------------------------------------------
        public string URLMyApp(bool sRedirection)
        {
            //string tText = NWDAppEnvironment.SelectedEnvironment().ServerHTTPS;
            //tText += NWDAppConfiguration.SharedInstance().WebServiceFolder();
            string tText = null;
            if (sRedirection == true)
            {
                tText = NWD.K_STATIC_FLASH_PHP + "?r=0";
            }
            else
            {
                tText = NWD.K_STATIC_FLASH_PHP + "?r=1";
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
            return GetMaxVersionStringForEnvironemt(NWDAppEnvironment.SelectedEnvironment());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetMaxVersionStringForEnvironemt(NWDAppEnvironment sEnvironment)
        {
            string tVersionString = "0.00.00";
            NWDVersion tVersion = SelectMaxRecheableDataForEnvironment(sEnvironment);
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
