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
using System.Reflection;
using UnityEngine;
using UnityEngine.Video;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using System.IO;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField]
    public class NWDVideoClipType : NWDAssetType
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDVideoClipType()
        {
            Value = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDVideoClipType(string sValue = NWEConstants.K_EMPTY_STRING)
        {
            if (sValue == null)
            {
                Value = string.Empty;
            }
            else
            {
                Value = sValue;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public async Task<VideoClip> ToAddressableVideoClip()
		{
            VideoClip rClip = null;
			string tFileNameKey = Path.GetFileName(this.GetAbsolutePath());
			Task<VideoClip> tTask = LoadAddressableVideoClip(tFileNameKey);
			rClip = await tTask;

			return rClip;
		}
        //-------------------------------------------------------------------------------------------------------------
        public VideoClip ToVideoClip()
        {
            VideoClip rVideoClip = null;
            if (Value != null && Value != string.Empty)
            {
                string tPath = Value.Replace(NWDAssetType.kAssetDelimiter, string.Empty);
#if UNITY_EDITOR
                rVideoClip = AssetDatabase.LoadAssetAtPath(tPath, typeof(VideoClip)) as VideoClip;
#else
                tPath = NWEPathResources.PathAbsoluteToPathDB(tPath);
                rVideoClip = Resources.Load(tPath, typeof(VideoClip)) as VideoClip;
#endif
            }
            return rVideoClip;
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
#if UNITY_EDITOR
        public override bool ErrorAnalyze()
        {
            bool rReturn = false;
            if (string.IsNullOrEmpty(Value) == false)
            {
                if (Value.Contains(NWD.K_Resources) == false)
                {
                    rReturn = true;
                }
                else
                {
                    string tPath = Value.Replace(NWDAssetType.kAssetDelimiter, string.Empty);
                    VideoClip tObject = AssetDatabase.LoadAssetAtPath(tPath, typeof(VideoClip)) as VideoClip;
                    if (tObject == null)
                    {
                        rReturn = true;
                    }
                }
            }
            InError = rReturn;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight()
        {
            int tVideoHeight = 0;
            if (kGUID != null)
            {
                tVideoHeight = 210;
            }

            return (NWDGUI.kPopupStyle.fixedHeight + NWDGUI.kFieldMarge + tVideoHeight);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = NWEConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            NWDVideoClipType tTemporary = new NWDVideoClipType();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

            tTemporary.Value = Value;

            float tWidth = sPosition.width - NWDGUI.kLangWidth * 2 + 5;
            float tHeight = sPosition.height;
            float tX = sPosition.position.x;
            float tY = sPosition.position.y;

            VideoClip tVideoClipLoaded = null;
            if (!string.IsNullOrEmpty(Value.Trim()))
            {
                string tPath = Value.Replace(NWDAssetType.kAssetDelimiter, string.Empty);
                tVideoClipLoaded = AssetDatabase.LoadAssetAtPath(tPath, typeof(VideoClip)) as VideoClip;

                Rect tRect = new Rect(tX + tWidth + 5, tY, NWDGUI.kPrefabSize, NWDGUI.kObjectFieldStyle.fixedHeight);
                if (kGUID == null)
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

            UnityEngine.Object tVideoObject = EditorGUI.ObjectField(new Rect(tX, tY, tWidth, NWDGUI.kObjectFieldStyle.fixedHeight), tContent, tVideoClipLoaded, typeof(VideoClip), false);
            if (tVideoObject != null)
            {
                tTemporary.Value = NWDAssetType.kAssetDelimiter + AssetDatabase.GetAssetPath(tVideoObject) + NWDAssetType.kAssetDelimiter;
            }
            else
            {
                tTemporary.Value = string.Empty;
            }

            tY += NWDGUI.kPopupStyle.fixedHeight + NWDGUI.kFieldMarge;

            ShowAssetPreviewTexture(tX - NWDGUI.kLangWidth + 10, tY + NWDGUI.kPopupStyle.fixedHeight, tWidth, 200);

            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
        static object kGUID;
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
        }
        //-------------------------------------------------------------------------------------------------------------
        private void PlayClip(VideoClip sVideoClip)
        {
            StopClip();

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
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
