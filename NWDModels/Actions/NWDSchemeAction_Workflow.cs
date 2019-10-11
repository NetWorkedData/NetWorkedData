//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:34:15
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================



using System;
using UnityEngine;
//using BasicToolBox;
using ZXing;
using ZXing.QrCode;
using UnityEngine.Networking;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDSchemeAction : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDSchemeAction()
        {
            //Debug.Log("NWDSchemeAction Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDSchemeAction(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDSchemeAction Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public Texture2D FlashMyApp(string sProto, bool sRedirection, int sDimension)
        {
            Texture2D rTexture = new Texture2D(sDimension, sDimension);
            var color32 = Encode(URISchemePath(sProto, string.Empty), rTexture.width, rTexture.height);
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
        public string URISchemePath(string sProtocol, string sAdditional)
        {
            string tText = sProtocol;
            tText = tText.Replace(":", string.Empty);
            tText = tText.Replace("/", string.Empty);
            tText += "://do?A=" + UnityWebRequest.EscapeURL(Reference);
            if (string.IsNullOrEmpty(Message) == false)
            {
                tText += "&M=" + UnityWebRequest.EscapeURL(Message);
            }
            if (string.IsNullOrEmpty(Parameter) == false)
            {
                tText += "&P=" + UnityWebRequest.EscapeURL(Parameter);
            }
            if (string.IsNullOrEmpty(sAdditional) == false)
            {
                tText += "&A=" + UnityWebRequest.EscapeURL(sAdditional);
            }
            return tText;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================