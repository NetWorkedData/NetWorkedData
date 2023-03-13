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
        //-------------------------------------------------------------------------------------------------------------
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
            int tAdd = 0;
            if (Value != string.Empty)
            {
                tAdd = 1;
            }
            GUIStyle tObjectFieldStyle = new GUIStyle(EditorStyles.objectField);
            tObjectFieldStyle.fixedHeight = tObjectFieldStyle.CalcHeight(new GUIContent(NWEConstants.K_A), 100.0f);
            GUIStyle tLabelStyle = new GUIStyle(EditorStyles.label);
            tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent(NWEConstants.K_A), 100.0f);
            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent(NWEConstants.K_A), 100.0f);
            GUIStyle tLabelAssetStyle = new GUIStyle(EditorStyles.label);
            tLabelAssetStyle.fontSize = 12;
            tLabelAssetStyle.fixedHeight = tLabelAssetStyle.CalcHeight(new GUIContent(NWEConstants.K_A), 100.0f);
            tLabelAssetStyle.normal.textColor = Color.gray;

            return tObjectFieldStyle.fixedHeight + tAdd * (NWDGUI.kPrefabSize + NWDGUI.kFieldMarge);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = NWEConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            NWDVideoClipType tTemporary = new NWDVideoClipType();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

            tTemporary.Value = Value;

            float tWidth = sPosition.width;
            float tHeight = sPosition.height;
            float tX = sPosition.position.x;
            float tY = sPosition.position.y;

            GUIStyle tObjectFieldStyle = new GUIStyle(EditorStyles.objectField);
            tObjectFieldStyle.fixedHeight = tObjectFieldStyle.CalcHeight(new GUIContent(NWEConstants.K_A), tWidth);
            GUIStyle tLabelStyle = new GUIStyle(EditorStyles.label);
            tLabelStyle.fixedHeight = tLabelStyle.CalcHeight(new GUIContent(NWEConstants.K_A), tWidth);
            tLabelStyle.normal.textColor = Color.red;
            GUIStyle tLabelAssetStyle = new GUIStyle(EditorStyles.label);
            tLabelAssetStyle.fontSize = 12;
            tLabelAssetStyle.fixedHeight = tLabelAssetStyle.CalcHeight(new GUIContent(NWEConstants.K_A), tWidth);
            tLabelAssetStyle.normal.textColor = Color.gray;
            GUIStyle tMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
            tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight(new GUIContent(NWEConstants.K_A), tWidth);

            VideoClip tObject = null;

            bool tRessource = true;

            if (Value != null && Value != string.Empty)
            {
                string tPath = Value.Replace(NWDAssetType.kAssetDelimiter, string.Empty);
                tObject = AssetDatabase.LoadAssetAtPath(tPath, typeof(VideoClip)) as VideoClip;
                if (tObject == null)
                {
                    tRessource = false;
                }
                else
                {
                    if (kGUID == null)
                    {
                        if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth, tY + NWDGUI.kFieldMarge + tObjectFieldStyle.fixedHeight, NWDGUI.kPrefabSize, NWDGUI.kPrefabSize), " PLAY"))
                        {
                            PlayClip(tObject);
                        }
                    }
                    else
                    {
                        if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth, tY + NWDGUI.kFieldMarge + tObjectFieldStyle.fixedHeight, NWDGUI.kPrefabSize, NWDGUI.kPrefabSize), " STOP"))
                        {
                            StopClip();
                        }
                        /*if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth + NWDGUI.kPrefabSize, tY + NWDGUI.kFieldMarge + tObjectFieldStyle.fixedHeight, NWDGUI.kPrefabSize, NWDGUI.kPrefabSize), "pause"))
                        {
                            PauseClip();
                        }*/
                    }
                }
                if (Value.Contains(NWD.K_Resources) == false)
                {
                    EditorGUI.LabelField(new Rect(tX, tY + tLabelAssetStyle.fixedHeight, tWidth, tLabelAssetStyle.fixedHeight), "NOT IN \"Resources\"", tLabelStyle);
                }
            }
            EditorGUI.BeginDisabledGroup(!tRessource);
            UnityEngine.Object pObj = EditorGUI.ObjectField(new Rect(tX, tY, tWidth, tObjectFieldStyle.fixedHeight), tContent, tObject, typeof(VideoClip), false);
            tY = tY + NWDGUI.kFieldMarge + tObjectFieldStyle.fixedHeight;
            if (pObj != null)
            {
                tTemporary.Value = NWDAssetType.kAssetDelimiter + AssetDatabase.GetAssetPath(pObj) + NWDAssetType.kAssetDelimiter;
            }
            else
            {
                tTemporary.Value = string.Empty;
            }
            EditorGUI.EndDisabledGroup();
            if (tRessource == true)
            {
            }
            else
            {
                tTemporary.Value = Value;

                GUI.Label(new Rect(tX + EditorGUIUtility.labelWidth, tY, tWidth, tLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_ASSET_MUST_BE_DOWNLOAD, tLabelStyle);
                tY = tY + NWDGUI.kFieldMarge + tLabelStyle.fixedHeight;
                GUI.Label(new Rect(tX + EditorGUIUtility.labelWidth, tY, tWidth, tLabelAssetStyle.fixedHeight), Value.Replace(NWDAssetType.kAssetDelimiter, ""), tLabelAssetStyle);
                tY = tY + NWDGUI.kFieldMarge + tLabelAssetStyle.fixedHeight;
                NWDGUI.BeginRedArea();
                if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth, tY, 60.0F, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE_CLEAN, tMiniButtonStyle))
                {
                    tTemporary.Value = string.Empty;
                }
                NWDGUI.EndRedArea();
                tY = tY + NWDGUI.kFieldMarge + tMiniButtonStyle.fixedHeight;
            }
            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
        static object kGUID;
        //-------------------------------------------------------------------------------------------------------------
        public static void StopClip()
        {
            Assembly tAssembly = typeof(VideoClipImporter).Assembly;
            Type tVideoUtilType = tAssembly.GetType("UnityEditor.VideoUtil");

            Type[] tTypes = { typeof(GUID) };
            string tMethodName = "Stop";
            #if UNITY_2021
            tMethodName = "StopPreview";
            #endif
            
            MethodInfo tMethod = tVideoUtilType.GetMethod(tMethodName, tTypes);
            if (tMethod != null)
            {
                object[] tObjects = { kGUID };
                tMethod.Invoke(null, BindingFlags.Static | BindingFlags.Public, null, tObjects, null);
            }
            else
            {
                Debug.LogWarning("Method is Null!, can't " + tMethodName + " video");
            }

            kGUID = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void PlayClip(VideoClip sVideoClip)
        {
            Assembly tAssembly = typeof(VideoClipImporter).Assembly;
            Type tVideoUtilType = tAssembly.GetType("UnityEditor.VideoUtil");

            Type[] tTypesMethodA = { typeof(VideoClip) };
            string tMethodName = "Start";
            #if UNITY_2021
            tMethodName = "StartPreview";
            #endif

            MethodInfo tMethod = tVideoUtilType.GetMethod(tMethodName, tTypesMethodA);
            if (tMethod != null)
            {
                object[] tObjectsA = { sVideoClip };
                kGUID = tMethod.Invoke(null, BindingFlags.Static | BindingFlags.Public, null, tObjectsA, null);
                
                Type[] tTypesMethodB = { typeof(GUID), typeof(bool) };
                tMethod = tVideoUtilType.GetMethod("PlayPreview", tTypesMethodB);
                if (tMethod != null)
                {
                    object[] tObjectsB = { kGUID, false };
                    tMethod.Invoke(null, BindingFlags.Static | BindingFlags.Public, null, tObjectsB, null);
                }
            }
            else
            {
                Debug.LogWarning("Method is Null!, can't " + tMethodName + " video");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /*public static void PauseClip()
        {

        }*/
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
