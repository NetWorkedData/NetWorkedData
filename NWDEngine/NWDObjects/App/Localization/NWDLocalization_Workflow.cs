//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
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
        private static List<NWDLocalization> GlobalLocalizerKeys;
        //-------------------------------------------------------------------------------------------------------------
        public NWDLocalization()
        {
            //Debug.Log("NWDLocalization Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDLocalization(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDLocalization Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
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
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDLocalization CreateLocalizationAnnexe(string sKey, string sDefault)
        {
            NWDLocalization rReturn = NewData();
            rReturn.InternalKey = sKey;
            rReturn.AnnexeValue = new NWDMultiType(sDefault);
            rReturn.UpdateData();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetLocalText(string sKey, string sDefault = BTBConstants.K_EMPTY_STRING)
        {
            NWDLocalization tObject = FindFirstDatasByInternalKey(sKey, true) as NWDLocalization;
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
        }
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
                foreach (NWDLocalization tObject in GlobalLocalizerKeys)
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
        private void UpdateInEnrichment()
        {
            if (GlobalLocalizerKeys == null)
            {
                GlobalLocalizerKeys = new List<NWDLocalization>();
            }
            if (XX > 0)
            {
                RemoveEnrichment();
            }
            else
            {
                if (string.IsNullOrEmpty(KeyValue) == false && GlobalLocalizerKeys.Contains(this) == false)
                {
                    GlobalLocalizerKeys.Add(this);
                }
                else if (string.IsNullOrEmpty(KeyValue) == true && GlobalLocalizerKeys.Contains(this) == true)
                {
                    GlobalLocalizerKeys.Remove(this);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void RemoveEnrichment()
        {
            if (GlobalLocalizerKeys == null)
            {
                GlobalLocalizerKeys = new List<NWDLocalization>();
            }
            if (GlobalLocalizerKeys.Contains(this) == true)
            {
                GlobalLocalizerKeys.Remove(this);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonLoadedMe()
        {
            UpdateInEnrichment();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUnloadMe()
        {
            RemoveEnrichment();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonInsertMe()
        {
            UpdateInEnrichment();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            UpdateInEnrichment();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonTrashMe()
        {
            UpdateInEnrichment();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUnTrashMe()
        {
            UpdateInEnrichment();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================