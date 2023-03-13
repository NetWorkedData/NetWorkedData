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
#if UNITY_EDITOR
using UnityEditor;
using NetWorkedData.NWDEditor;
#endif
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
//=====================================================================================================================
namespace NetWorkedData
{
    [SerializeField]
    //-----------------------------------------------------------------------------------------------------------------
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
        //-------------------------------------------------------------------------------------------------------------
        public void InitValue()
        {
            // Split Value into a List
            List<string> tValueList = new List<string>();
            if (!string.IsNullOrEmpty(Value))
            {
                tValueList = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            }

            foreach(string tLine in tValueList)
            {
                string tLangague = string.Empty;
                string tText = string.Empty;
                string tAudio = string.Empty;
                string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                if (tLineValue.Length == 2)
                {
                    tLangague = tLineValue[0];
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
                else if (tLineValue.Length == 1)
                {
                    tLangague = tLineValue[0];
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public new string GetLocalString()
        {
            string rValue = "";

            string tLine = SplitDico(NWDDataManager.SharedInstance().PlayerLanguage);
            string[] k = tLine.Split(new string[] { NWDConstants.kFieldSeparatorD }, StringSplitOptions.RemoveEmptyEntries);
            if (k.Length == 2)
            {
                rValue = k[0];
            }
            else
            {
                rValue = tLine;
            }

            return NWDToolbox.TextUnprotect(rValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public AudioClip GetLocalAudio()
		{
            string tPath = GetAudioValue();

            AudioClip rClip = null;
			if (!string.IsNullOrEmpty(tPath))
            {
				#if UNITY_EDITOR
                tPath = tPath.Replace(NWDAssetType.kAssetDelimiter, string.Empty);
				rClip = AssetDatabase.LoadAssetAtPath<AudioClip>(tPath);
                #else
                tPath = GetAbsolutePath(tPath);
                rClip = Resources.Load<AudioClip>(tPath);
                #endif
			}

			return rClip;
		}
        //-------------------------------------------------------------------------------------------------------------
        public async Task<AudioClip> GetAddressableAudio()
        {
            string tPath = GetAudioValue();

            AudioClip rClip = null;
            if(!string.IsNullOrEmpty(tPath))
            {
                string tFileNameKey = Path.GetFileName(this.GetAbsolutePath(tPath));
                Task<AudioClip> tTask = LoadAddressableAudioClip(tFileNameKey);
                rClip = await tTask;
            }

            return rClip;
        }
        //-------------------------------------------------------------------------------------------------------------
        private async Task<AudioClip> LoadAddressableAudioClip(string sKey)
        {
            AudioClip rClip = null;
            AsyncOperationHandle<AudioClip> tHandle = Addressables.LoadAssetAsync<AudioClip>(sKey);
            await tHandle.Task;
            if(tHandle.Status == AsyncOperationStatus.Succeeded)
            {
                rClip = tHandle.Result;
            }
            else
            {
                Debug.LogWarning("Addressable " + tHandle.DebugName + " load error");
            }
            return rClip;
        }
        //-------------------------------------------------------------------------------------------------------------
        private string GetAudioValue()
        {
            string rAudio = "";
            string tLine = SplitDico(NWDDataManager.SharedInstance().PlayerLanguage);
            string[] k = tLine.Split(new string[] { NWDConstants.kFieldSeparatorD }, StringSplitOptions.RemoveEmptyEntries);
            if (k.Length == 2)
            {
                rAudio = k[1];
            }

            return rAudio;
        }
        //-------------------------------------------------------------------------------------------------------------
        private string GetAbsolutePath(string sFile)
		{
			string rPath = "";
			if (!string.IsNullOrEmpty(sFile))
            {
				rPath = sFile.Replace(NWDAssetType.kAssetDelimiter, string.Empty);
				rPath = NWEPathResources.PathAbsoluteToPathDB(rPath);
			}

			return rPath;
		}
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        const float kCONT_FIELD_STYLE_SPACE = 5f;
        const int kCONST_NUMBER_OF_LINE = 6;
        AudioClip kAudioClipUsed = null;
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight()
        {
            int tRow = 0;
            if (!string.IsNullOrEmpty(Value))
            {
                tRow = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries).Count();
            }

            return (NWDGUI.kTextFieldStyle.fixedHeight * kCONST_NUMBER_OF_LINE + NWDGUI.kPopupStyle.fixedHeight + NWDGUI.kObjectFieldStyle.fixedHeight + kCONT_FIELD_STYLE_SPACE + NWDGUI.kFieldMarge * 2) * tRow + NWDGUI.kPopupStyle.fixedHeight;
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
                else if (tLineValue.Length == 1)
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
                    tText = NWDToolbox.TextProtect(tText);

                    // Audio field
                    AudioClip tAudioClipLoaded = null;
                    if (!string.IsNullOrEmpty(tAudio.Trim()))
                    {
                        string tPath = tAudio.Replace(NWDAssetType.kAssetDelimiter, string.Empty);
                        tAudioClipLoaded = AssetDatabase.LoadAssetAtPath(tPath, typeof(AudioClip)) as AudioClip;

                        Rect tRect = new Rect(tX - NWDGUI.kLangWidth * 2 + tWidth + 10, tY + NWDGUI.kTextFieldStyle.fixedHeight * kCONST_NUMBER_OF_LINE + NWDGUI.kPopupStyle.fixedHeight + NWDGUI.kFieldMarge * 2, NWDGUI.kPrefabSize, NWDGUI.kObjectFieldStyle.fixedHeight * 2);
                        if (tAudioClipLoaded != null && kAudioClipUsed != tAudioClipLoaded)
                        {
                            if (GUI.Button(tRect, "PLAY"))
                            {
                                PlayClip(tAudioClipLoaded);
                            }
                        }
                        else
                        {
                            if (GUI.Button(tRect, "STOP"))
                            {
                                StopClip();
                            }
                        }
                    }

                    UnityEngine.Object tAudioObject = EditorGUI.ObjectField(new Rect(tX - NWDGUI.kLangWidth * 2, tY + NWDGUI.kTextFieldStyle.fixedHeight * kCONST_NUMBER_OF_LINE + NWDGUI.kPopupStyle.fixedHeight + NWDGUI.kFieldMarge * 2, tWidth, NWDGUI.kObjectFieldStyle.fixedHeight), tContent, tAudioClipLoaded, typeof(AudioClip), false);
                    tY = tY + NWDGUI.kFieldMarge + NWDGUI.kObjectFieldStyle.fixedHeight;
                    if (tAudioObject != null)
                    {
                        tAudio = NWDAssetType.kAssetDelimiter + AssetDatabase.GetAssetPath(tAudioObject) + NWDAssetType.kAssetDelimiter;
                    }
                    else
                    {
                        tAudio = string.Empty;
                    }

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
        //-------------------------------------------------------------------------------------------------------------
        private void StopClip()
        {
            string tMethodName = "StopClip";
            #if UNITY_2021
            tMethodName = "StopAllPreviewClips";
            #endif

            GetMethodResult(tMethodName, typeof(AudioImporter), "UnityEditor.AudioUtil");

            kAudioClipUsed = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void PlayClip(AudioClip sAudioClip)
        {
            StopClip();

            kAudioClipUsed = sAudioClip;

            string tMethodName = "PlayClip";
            #if UNITY_2021
            tMethodName = "PlayPreviewClip";
            #endif

            GetMethodResult(tMethodName,
                            typeof(AudioImporter),
                            "UnityEditor.AudioUtil",
                            new Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
                            new object[] { sAudioClip, 0, false });
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
    }
}
//=====================================================================================================================
