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
using UnityEngine.Video;
using System.Reflection;
//=====================================================================================================================
namespace NetWorkedData
{
    [SerializeField]
    //-----------------------------------------------------------------------------------------------------------------
    public partial class NWDLocalizableVideoClipType : NWDLocalizableType
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDLocalizableVideoClipType()
        {
            Value = string.Empty;
            AddBaseString(string.Empty);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDLocalizableVideoClipType(string sValue = NWEConstants.K_EMPTY_STRING)
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
                string tAudio = string.Empty;
                string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                if (tLineValue.Length == 2)
                {
                    tLangague = tLineValue[0];
                    tAudio = tLineValue[1];
                }
                else if (tLineValue.Length == 1)
                {
                    tLangague = tLineValue[0];
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public VideoClip GetLocalVideo()
		{
            string tPath = SplitDico(NWDDataManager.SharedInstance().PlayerLanguage);

            VideoClip rClip = null;
			if (!string.IsNullOrEmpty(tPath))
            {
				#if UNITY_EDITOR
                tPath = tPath.Replace(NWDAssetType.kAssetDelimiter, string.Empty);
				rClip = AssetDatabase.LoadAssetAtPath<VideoClip>(tPath);
                #else
                tPath = GetAbsolutePath(tPath);
                rClip = Resources.Load<VideoClip>(tPath);
                #endif
			}

			return rClip;
		}
        //-------------------------------------------------------------------------------------------------------------
        public async Task<VideoClip> GetAddressableVideo()
        {
            string tPath = SplitDico(NWDDataManager.SharedInstance().PlayerLanguage);

            VideoClip rClip = null;
            if(!string.IsNullOrEmpty(tPath))
            {
                string tFileNameKey = Path.GetFileName(this.GetAbsolutePath(tPath));
                Task<VideoClip> tTask = LoadAddressableVideoClip(tFileNameKey);
                rClip = await tTask;
            }

            return rClip;
        }
        //-------------------------------------------------------------------------------------------------------------
        private async Task<VideoClip> LoadAddressableVideoClip(string sKey)
        {
            VideoClip rClip = null;
            AsyncOperationHandle<VideoClip> tHandle = Addressables.LoadAssetAsync<VideoClip>(sKey);
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
        VideoClip kVideoClipUsed = null;
        object kGUID = null;
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight()
        {
            int tRow = 1;
            if (!string.IsNullOrEmpty(Value))
            {
                tRow += Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries).Count();
            }

            int tVideoHeight = 0;
            if (kVideoClipUsed != null)
            {
                tVideoHeight = 200;
            }

            return (tRow * (NWDGUI.kPopupStyle.fixedHeight + NWDGUI.kFieldMarge) + tVideoHeight);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = NWEConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            NWDLocalizableVideoClipType tTemporary = new NWDLocalizableVideoClipType();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

            float tWidth = sPosition.width - NWDGUI.kLangWidth * 3;
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
                string tVideo = string.Empty;
                string tLine = tValueList.ElementAt(i);
                string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                if (tLineValue.Length == 2)
                {
                    tLangague = tLineValue[0];
                    tVideo = tLineValue[1];
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

                    // Video field
                    VideoClip tVideoClipLoaded = null;
                    if (!string.IsNullOrEmpty(tVideo.Trim()))
                    {
                        string tPath = tVideo.Replace(NWDAssetType.kAssetDelimiter, string.Empty);
                        tVideoClipLoaded = AssetDatabase.LoadAssetAtPath<VideoClip>(tPath);

                        Rect tRect = new Rect(tX + NWDGUI.kLangWidth + tWidth + 10, tY, NWDGUI.kPrefabSize, NWDGUI.kObjectFieldStyle.fixedHeight);
                        if (tVideoClipLoaded != null && kVideoClipUsed != tVideoClipLoaded)
                        {
                            if (GUI.Button(tRect, "PLAY"))
                            {
                                PlayClip(tVideoClipLoaded);
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

                    UnityEngine.Object tVideoObject = EditorGUI.ObjectField(new Rect(tX + NWDGUI.kLangWidth + 5, tY, tWidth, NWDGUI.kObjectFieldStyle.fixedHeight), tContent, tVideoClipLoaded, typeof(VideoClip), false);
                    if (tVideoObject != null)
                    {
                        tVideo = NWDAssetType.kAssetDelimiter + AssetDatabase.GetAssetPath(tVideoObject) + NWDAssetType.kAssetDelimiter;
                    }
                    else
                    {
                        tVideo = string.Empty;
                    }

                    EditorGUI.indentLevel = tIndentLevel;
                }
                tY += NWDGUI.kPopupStyle.fixedHeight + NWDGUI.kFieldMarge;

                tResult = UpdateDictionaryValue(tLangague, tVideo, tResult);
            }
            tResult.Remove(NWEConstants.K_MINUS); // remove default value
            tResult.Remove(string.Empty); // remove empty value

            ShowAssetPreviewTexture(tX, tY, tWidth, 200);

            if (tResult.ContainsKey(NWDDataLocalizationManager.kBaseDev) == false)
            {
                tResult = UpdateDictionaryValue(NWDDataLocalizationManager.kBaseDev, "", tResult);
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
        private Dictionary<string, string> UpdateDictionaryValue(string sValueLangue, string sValueVideo, Dictionary<string, string> sDictionary)
        {
            if (string.IsNullOrEmpty(sValueVideo))
            {
                sValueVideo = " ";
            }

            if (sDictionary.ContainsKey(sValueLangue))
            {
                sDictionary[sValueLangue] = sValueVideo;
            }
            else
            {
                sDictionary.Add(sValueLangue, sValueVideo);
            }

            return sDictionary;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void StopClip()
        {
            if (kGUID != null)
            {
                string tMethodName = "Stop";
                #if UNITY_2021
                tMethodName = "StopPreview";
                #endif

                GetMethodResult(tMethodName,
                                typeof(VideoClipImporter),
                                "UnityEditor.VideoUtil",
                                new Type[] { typeof(GUID) },
                                new object[] { kGUID });

                kGUID = null;
            }

            kVideoClipUsed = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void PlayClip(VideoClip sVideoClip)
        {
            StopClip();

            kVideoClipUsed = sVideoClip;

            string tMethodName = "Start";
            #if UNITY_2021
            tMethodName = "StartPreview";
            #endif

            kGUID = GetMethodResult(tMethodName,
                                    typeof(VideoClipImporter),
                                    "UnityEditor.VideoUtil",
                                    new Type[] { typeof(VideoClip) },
                                    new object[] { sVideoClip });

            if (kGUID != null)
            {
                GetMethodResult("PlayPreview",
                                typeof(VideoClipImporter),
                                "UnityEditor.VideoUtil",
                                new Type[] { typeof(GUID), typeof(bool) },
                                new object[] { kGUID, false });
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void ShowAssetPreviewTexture(float sX, float sY, float sWidth, float sHeight)
        {
            if (kGUID != null)
            {
                string tMethodName = "GetPreviewTexture";
                object tTexture = GetMethodResult(tMethodName,
                                                typeof(VideoClipImporter),
                                                "UnityEditor.VideoUtil",
                                                new Type[] { typeof(GUID) },
                                                new object[] { kGUID });

                if (tTexture != null)
                {
                    EditorGUI.DrawPreviewTexture(new Rect(sX + NWDGUI.kLangWidth * 3, sY - NWDGUI.kObjectFieldStyle.fixedHeight, sWidth, sHeight), tTexture as Texture, null , ScaleMode.ScaleToFit);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
    }
}
//=====================================================================================================================
