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
using System.Linq;
using System.Reflection;
using System.IO;
using UnityEngine;
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEditor;
using NetWorkedData.NWDEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    [SerializeField]
    //-------------------------------------------------------------------------------------------------------------
    public class NWDLocalizableLongTextAndAudioType : NWDLocalizableType
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDLocalizableLongTextAndAudioType()
        {
            Value = string.Empty;
            AddBaseString(string.Empty);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDLocalizableLongTextAndAudioType(string sValue = NWEConstants.K_EMPTY_STRING)
        {
            if (string.IsNullOrEmpty(sValue))
            {
                Value = string.Empty;
                AddBaseString(string.Empty);
            }
            else
            {
                Value = sValue;
            }
            kSplitDico = new Dictionary<string, string>();
            DicoPopulate();
        }
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        const float kCONT_FIELD_STYLE_SPACE = 5f;
        const int kCONST_NUMBER_OF_LINE = 6;
        AudioClip kAudioClipUsed = null;
        AudioClip kAudioClipLoaded = null;
        string kAudio = "";
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight()
        {
            int tRow = 0;
            if (Value != null && Value != string.Empty)
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                tRow += tValueArray.Count();
            }

            float rReturn = (NWDGUI.kTextFieldStyle.fixedHeight * kCONST_NUMBER_OF_LINE + NWDGUI.kPopupStyle.fixedHeight + NWDGUI.kObjectFieldStyle.fixedHeight + kCONT_FIELD_STYLE_SPACE + NWDGUI.kFieldMarge * 2) * tRow + NWDGUI.kPopupStyle.fixedHeight;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = NWEConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            NWDLocalizableLongTextAndAudioType tTemporary = new NWDLocalizableLongTextAndAudioType();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

            float tWidth = sPosition.width;
            float tHeight = sPosition.height;
            float tX = sPosition.position.x;
            float tY = sPosition.position.y;
            float tLangWidth = EditorGUIUtility.labelWidth + NWDGUI.kLangWidth;

            // Get Languages
            string tLanguage = NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguagesString;
            string[] tLanguageArray = tLanguage.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);

            // Set Language & order the list
            List<string> tLocalizationList = new List<string>();
            tLocalizationList.Add(NWEConstants.K_MINUS);
            tLocalizationList.AddRange(tLanguageArray);
            tLocalizationList.Sort();

            // Split Value into a List
            List<string> tValueList = new List<string>();
            if (!string.IsNullOrEmpty(Value))
            {
                tValueList = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            }

            // split each subValue in Value (new List)
            // and remove from Language List subValue with already text set
            foreach(string k in tValueList)
            {
                //string tLine = k;
                string[] tLineValue = k.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                if (tLineValue.Length > 0)
                {
                    tLocalizationList.Remove(tLineValue[0]);
                }
            }
            tValueList.Add("");

            Dictionary<string, string> tResult = new Dictionary<string, string>();
            for (int i = 0; i < tValueList.Count; i++)
            {
                if (i > 0)
                {
                    tContent = new GUIContent("   ");
                }

                string tLangague = string.Empty;
                string tText = string.Empty;
                string tAudio = string.Empty;
                string tLine = tValueList.ElementAt(i);
                string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                if (tLineValue.Length == 2)
                {
                    tLangague = tLineValue[0];
                    //tText = tLineValue[1];

                    string[] k = tLineValue[1].Split(new string[] { NWDConstants.kFieldSeparatorD }, StringSplitOptions.RemoveEmptyEntries);
                    if (k.Length == 2)
                    {
                        tText = k[0];
                        tAudio = k[1];
                    }
                    else
                    {
                        tText = tLineValue[1];
                    }
                }
                if (tLineValue.Length == 1)
                {
                    tLangague = tLineValue[0];
                }

                List<string> tValueFuturList = new List<string>();
                tValueFuturList.AddRange(tLocalizationList.ToArray());
                tValueFuturList.Add(tLangague);
                tValueFuturList.Sort();

                List<GUIContent> tContentFuturList = new List<GUIContent>();
                foreach (string tS in tValueFuturList.ToArray())
                {
                    tContentFuturList.Add(new GUIContent(tS));
                }

                // Draw the dropdown language list
                int tIndex = tValueFuturList.IndexOf(tLangague);
                tIndex = EditorGUI.Popup(new Rect(tX, tY, tLangWidth, NWDGUI.kPopupStyle.fixedHeight), tContent, tIndex, tContentFuturList.ToArray(), NWDGUI.kPopupStyle);
                if (tIndex < 0 || tIndex >= tValueFuturList.Count)
                {
                    tIndex = 0;
                }
                tLangague = tValueFuturList[tIndex];

                // Draw textbox
                if (tLangague != string.Empty)
                {
                    int tIndentLevel = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 0;

                    // Text field
                    tText = EditorGUI.TextArea(new Rect(tX + NWDGUI.kLangWidth, tY + NWDGUI.kFieldMarge + NWDGUI.kPopupStyle.fixedHeight, tWidth - NWDGUI.kLangWidth, NWDGUI.kTextFieldStyle.fixedHeight * kCONST_NUMBER_OF_LINE), NWDToolbox.TextUnprotect(tText), NWDGUI.kTextAreaStyle);
                    //tText = NWDToolbox.TextProtect(tText);

                    // Audio field
                    //bool tRessource = true;
                    if (!string.IsNullOrEmpty(tAudio))
                    {
                        if (kAudioClipLoaded == null || kAudio != tAudio)
                        {
                            //StopClip();

                            kAudio = tAudio;
                            string tPath = tAudio.Replace(NWDAssetType.kAssetDelimiter, string.Empty);
                            kAudioClipLoaded = AssetDatabase.LoadAssetAtPath(tPath, typeof(AudioClip)) as AudioClip;
                            //tDuration = TimeSpan.FromMilliseconds(GetDuration(kAudioClipLoaded)).ToString(@"mm\:ss");
                        }

                        /*if (kAudioClipLoaded == null)
                        {
                            tRessource = false;
                        }*/
                        /*else
                        {
                            if (kAudioClipUsed == null)
                            {
                                if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth, tY + NWDGUI.kFieldMarge + tObjectFieldStyle.fixedHeight, NWDGUI.kPrefabSize, NWDGUI.kPrefabSize), " PLAY"))
                                {
                                    PlayClip(kAudioClipLoaded);
                                }
                            }
                            else
                            {
                                if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth, tY + NWDGUI.kFieldMarge + tObjectFieldStyle.fixedHeight, NWDGUI.kPrefabSize, NWDGUI.kPrefabSize), " STOP"))
                                {
                                    StopClip();
                                }
                            }
                        }*/
                        /*if (Value.Contains(NWD.K_Resources) == false)
                        {
                            EditorGUI.LabelField(new Rect(tX, tY + tLabelAssetStyle.fixedHeight, tWidth, tLabelAssetStyle.fixedHeight), "NOT IN \"Resources\"", tLabelStyle);
                        }*/
                    }

                    //EditorGUI.BeginDisabledGroup(!tRessource);
                    UnityEngine.Object tAudioObject = EditorGUI.ObjectField(new Rect(tX - NWDGUI.kLangWidth * 2, tY + NWDGUI.kTextFieldStyle.fixedHeight * kCONST_NUMBER_OF_LINE + NWDGUI.kPopupStyle.fixedHeight + NWDGUI.kFieldMarge*2, tWidth, NWDGUI.kObjectFieldStyle.fixedHeight), tContent, kAudioClipLoaded, typeof(AudioClip), false);
                    tY = tY + NWDGUI.kFieldMarge + NWDGUI.kObjectFieldStyle.fixedHeight;
                    if (tAudioObject != null)
                    {
                        tAudio = NWDAssetType.kAssetDelimiter + AssetDatabase.GetAssetPath(tAudioObject) + NWDAssetType.kAssetDelimiter;
                    }
                    else
                    {
                        tAudio = string.Empty;
                    }
                    //EditorGUI.EndDisabledGroup();

                    EditorGUI.indentLevel = tIndentLevel;
                }
                tY += NWDGUI.kTextFieldStyle.fixedHeight * kCONST_NUMBER_OF_LINE + NWDGUI.kPopupStyle.fixedHeight + NWDGUI.kFieldMarge*2;

                tResult = UpdateDictionaryValue(tLangague, tText, tAudio, tResult);
            }
            tResult.Remove(NWEConstants.K_MINUS); // remove default value
            tResult.Remove(string.Empty); // remove empty value

            if (tResult.ContainsKey(NWDDataLocalizationManager.kBaseDev) == false)
            {
                tResult = UpdateDictionaryValue(NWDDataLocalizationManager.kBaseDev, "", "", tResult);
            }

            tValueList = new List<string>();
            foreach (KeyValuePair<string, string> tKeyValue in tResult)
            {
                tValueList.Add(tKeyValue.Key + NWDConstants.kFieldSeparatorB + tKeyValue.Value);
            }

            string[] tValueArray = tValueList.Distinct().ToArray();
            string tNewValue = string.Join(NWDConstants.kFieldSeparatorA, tValueArray);
            tNewValue = tNewValue.Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
            if (tNewValue == NWDConstants.kFieldSeparatorB)
            {
                tNewValue = string.Empty;
            }
            tTemporary.Value = tNewValue;
            //Debug.LogWarning(tNewValue);

            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
        private Dictionary<string, string> UpdateDictionaryValue(string sValueLangue, string sValueText, string sValueAudio, Dictionary<string, string> sDictionary)
        {
            if (string.IsNullOrEmpty(sValueText))
            {
                sValueText = " ";
            }
            if (string.IsNullOrEmpty(sValueAudio))
            {
                sValueAudio = " ";
            }
            string tValue = sValueText + NWDConstants.kFieldSeparatorD + sValueAudio;

            /*string tValue = "";
            if (string.IsNullOrEmpty(sValueText) && string.IsNullOrEmpty(sValueAudio))
            {
                // do nothing
            }
            else
            {
                if (string.IsNullOrEmpty(sValueText))
                {
                    sValueText = " ";
                }
                if (string.IsNullOrEmpty(sValueAudio))
                {
                    sValueAudio = " ";
                }
                tValue = sValueText + NWDConstants.kFieldSeparatorD + sValueAudio;
            }*/

            if (sDictionary.ContainsKey(sValueLangue))
            {
                sDictionary[sValueLangue] = tValue;
            }
            else
            {
                sDictionary.Add(sValueLangue, tValue);
            }

            return sDictionary;
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================
