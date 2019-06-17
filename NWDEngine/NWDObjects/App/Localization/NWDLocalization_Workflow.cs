//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:33:49
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDLocalization : NWDBasis<NWDLocalization>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDLocalization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDLocalization(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        /*[Obsolete]
        public static NWDLocalization CreateLocalizationTextValue(string sKey, string sDefault = BTBConstants.K_EMPTY_STRING)
        {
            NWDLocalization rReturn = NewData();
            rReturn.InternalKey = sKey;
            if (sDefault != string.Empty)
            {
                rReturn.TextValue.AddBaseString(sDefault);
            }
            else
            {
                rReturn.TextValue.AddBaseString(sKey);
            }
            rReturn.UpdateData();
            return rReturn;
        }*/
        //-------------------------------------------------------------------------------------------------------------
        /*[Obsolete]
        public static string GetLocalText(string sKey, string sDefault = BTBConstants.K_EMPTY_STRING)
        {
            NWDLocalization tObject = GetRawFirstDataByInternalKey(sKey) as NWDLocalization;
            string rReturn = sDefault;
            if (tObject != null)
            {
                rReturn = tObject.TextValue.GetLocalString();
            }
            else
            {
                CreateLocalizationTextValue(sKey, sDefault);
            }
            return rReturn;
        }*/
        //-------------------------------------------------------------------------------------------------------------
        public static string Enrichment(string sText, string sLanguage = null, bool sBold = true)
        {
            string rText = sText;
            string tBstart = "<b>";
            string tBend = "</b>";
            if (sBold == false)
            {
                tBstart = string.Empty;
                tBend = string.Empty;
            }
            if (sLanguage == null)
            {
                sLanguage = NWDDataManager.SharedInstance().PlayerLanguage;
            }
            int tI = 0;
                while (tI <= 10)
                {
                    tI++;
                    int tJ = 0;
                    foreach (NWDLocalization tObject in RawDatasByIndex())
                    {
                        if (sText.Contains(tObject.KeyValue))
                        {
                            tJ++;
                            if (string.IsNullOrEmpty(tObject.InternalKey) == false)
                            {
                                rText = rText.Replace(tObject.InternalKey, tBstart + tObject.TextValue.GetLanguageString(sLanguage) + tBend);
                            }
                        }
                    }
                    if (tJ == 0)
                    {
                        break;
                    }
                }
            return rText;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetLocalString()         {             string tText = TextValue.GetLocalString();             return tText.Replace("<br>", Environment.NewLine);         }
        //-------------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================