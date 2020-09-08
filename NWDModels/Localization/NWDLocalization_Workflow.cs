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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDLocalization : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDLocalization() {}
        //-------------------------------------------------------------------------------------------------------------
        public NWDLocalization(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData) {}
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
                    foreach (NWDLocalization tObject in RawDatasWithKey())
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
        public string GetLocalString()
        {
            string tText = TextValue.GetLocalString();
            return tText.Replace("<br>", Environment.NewLine);
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        public static NWDLocalization EditorCreateIfNotExists(string sReferenceKey, string sBaseTextValue)
        {
            NWDLocalization rReturn = NWDBasisHelper.GetRawDataByReference<NWDLocalization>(sReferenceKey);
            if (rReturn == null)
            {
                rReturn = NWDBasisHelper.NewDataWithReference<NWDLocalization>(sReferenceKey);
                rReturn.TextValue.AddBaseString(sBaseTextValue);
                rReturn.InternalKey = sReferenceKey;
                rReturn.SaveData();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        public static NWDLocalization GetByReference (string sReferenceKey)
        {
            NWDLocalization rReturn = NWDBasisHelper.GetRawDataByReference<NWDLocalization>(sReferenceKey);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
