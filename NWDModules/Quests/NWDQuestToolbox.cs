//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

using BasicToolBox;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public class NWDQuestToolbox
	{
        //-------------------------------------------------------------------------------------------------------------
        public static string Enrichment(string sText,
                                        string sLanguage = null,
                                        NWDReferencesListType<NWDCharacter> sReplaceCharacters = null,
                                        NWDReferencesQuantityType<NWDItem> sReplaceItems = null,
                                        NWDReferencesQuantityType<NWDItemGroup> sReplaceItemGroups = null,
                                        NWDReferencesQuantityType<NWDPack> sReplacePacks = null,
                                        bool sBold = true)
        {
            string rText = NWDUserToolbox.Enrichment(sText, sLanguage, sBold); // add nickname, nickname id etc...  
            int tCounter = 0;
            string tBstart = "<b>";
            string tBend = "</b>";
            if (sBold == false)
            {
                tBstart = "";
                tBend = "";
            }
            if (sLanguage == null)
            {
                sLanguage = NWDDataManager.SharedInstance().PlayerLanguage;
            }
            // // replace referecen in text
            if (sReplaceCharacters != null)
            {
                tCounter = 1;
                foreach (NWDCharacter tCharacter in sReplaceCharacters.GetObjects())
                {
                    if (tCharacter.LastName != null)
                    {
                        string tLastName = tCharacter.LastName.GetLanguageString(sLanguage);
                        if (tLastName != null)
                        {
                            rText = rText.Replace("#L" + tCounter.ToString() + "#", tBstart + tLastName + tBend);
                        }
                    }
                    if (tCharacter.FirstName != null)
                    {
                        string tFirstName = tCharacter.FirstName.GetLanguageString(sLanguage);
                        if (tFirstName != null)
                        {
                            rText = rText.Replace("#F" + tCounter.ToString() + "#", tBstart + tFirstName + tBend);
                        }
                    }
                    if (tCharacter.NickName != null)
                    {
                        string tNickName = tCharacter.NickName.GetLanguageString(sLanguage);
                        if (tNickName != null)
                        {
                            rText = rText.Replace("#N" + tCounter.ToString() + "#", tBstart + tNickName + tBend);
                        }
                    }
                    tCounter++;
                }
            }
            if (sReplaceItems != null)
            {
                tCounter = 1;
                foreach (KeyValuePair<NWDItem, int> tKeyValue in sReplaceItems.GetObjectAndQuantity())
                {
                    NWDItem tItem = tKeyValue.Key;
                    if (tItem != null)
                    {
                        string tNameQuantity = "";
                        string tNameSingular = "";
                        string tNamePlural = "";
                        if (tItem.Name != null)
                        {
                            tNameSingular = tItem.Name.GetLanguageString(sLanguage);
                        }
                        if (tItem.PluralName != null)
                        {
                            tNamePlural = tItem.PluralName.GetLanguageString(sLanguage);
                        }
                        if (tKeyValue.Value == 1 && tItem.Name != null)
                        {
                            tNameQuantity = tKeyValue.Value + " " + tNameSingular;
                        }
                        else if (tKeyValue.Value > 1 && tItem.PluralName != null)
                        {
                            tNameQuantity = tKeyValue.Value + " " + tNameSingular;
                        }
                        rText = rText.Replace("#I" + tCounter.ToString() + "#", tBstart + tNameSingular + tBend);
                        rText = rText.Replace("#I" + tCounter.ToString() + "s#", tBstart + tNamePlural + tBend);
                        rText = rText.Replace("#xI" + tCounter.ToString() + "#", tBstart + tNameQuantity + tBend);
                    }
                    tCounter++;
                }
            }
            if (sReplaceItemGroups != null)
            {
                tCounter = 1;
                foreach (KeyValuePair<NWDItemGroup, int> tKeyValue in sReplaceItemGroups.GetObjectAndQuantity())
                {
                    NWDItem tItem = tKeyValue.Key.DescriptionItem.GetObject();
                    if (tItem != null)
                    {
                        string tNameQuantity = "";
                        string tNameSingular = "";
                        string tNamePlural = "";
                        if (tItem.Name != null)
                        {
                            tNameSingular = tItem.Name.GetLanguageString(sLanguage);
                        }
                        if (tItem.PluralName != null)
                        {
                            tNamePlural = tItem.PluralName.GetLanguageString(sLanguage);
                        }
                        if (tKeyValue.Value == 1 && tItem.Name != null)
                        {
                            tNameQuantity = tKeyValue.Value + " " + tNameSingular;
                        }
                        else if (tKeyValue.Value > 1 && tItem.PluralName != null)
                        {
                            tNameQuantity = tKeyValue.Value + " " + tNameSingular;
                        }
                        rText = rText.Replace("#G" + tCounter.ToString() + "#", tBstart + tNameSingular + tBend);
                        rText = rText.Replace("#G" + tCounter.ToString() + "s#", tBstart + tNamePlural + tBend);
                        rText = rText.Replace("#xG" + tCounter.ToString() + "#", tBstart + tNameQuantity + tBend);
                    }
                    tCounter++;
                }
            }
            if (sReplacePacks != null)
            {
                tCounter = 1;
                foreach (KeyValuePair<NWDPack, int> tKeyValue in sReplacePacks.GetObjectAndQuantity())
                {
                    NWDItem tItem = tKeyValue.Key.ItemToDescribe.GetObject();
                    if (tItem != null)
                    {
                        string tNameQuantity = "";
                        string tNameSingular = "";
                        string tNamePlural = "";
                        if (tItem.Name != null)
                        {
                            tNameSingular = tItem.Name.GetLanguageString(sLanguage);
                        }
                        if (tItem.PluralName != null)
                        {
                            tNamePlural = tItem.PluralName.GetLanguageString(sLanguage);
                        }
                        if (tKeyValue.Value == 1 && tItem.Name != null)
                        {
                            tNameQuantity = tKeyValue.Value + " " + tNameSingular;
                        }
                        else if (tKeyValue.Value > 1 && tItem.PluralName != null)
                        {
                            tNameQuantity = tKeyValue.Value + " " + tNameSingular;
                        }
                        rText = rText.Replace("#P" + tCounter.ToString() + "#", tBstart + tNameSingular + tBend);
                        rText = rText.Replace("#P" + tCounter.ToString() + "s#", tBstart + tNamePlural + tBend);
                        rText = rText.Replace("#xP" + tCounter.ToString() + "#", tBstart + tNameQuantity + tBend);
                    }
                    tCounter++;
                }
            }
            return rText;
        }
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
