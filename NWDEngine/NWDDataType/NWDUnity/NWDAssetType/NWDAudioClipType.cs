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
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.AddressableAssets;
using System.IO;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField]
    public class NWDAudioClipType : NWDAssetType
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDAudioClipType()
        {
            Value = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAudioClipType(string sValue = NWEConstants.K_EMPTY_STRING)
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
        public async Task<AudioClip> ToAddressableAudioClip()
		{
            AudioClip rClip = null;
			string tFileNameKey = Path.GetFileName(this.GetAbsolutePath());
			Task<AudioClip> tTask = LoadAddressableAudioClip(tFileNameKey);
			rClip = await tTask;

			return rClip;
		}
        //-------------------------------------------------------------------------------------------------------------
        public AudioClip ToAudioClip()
        {
            AudioClip rAudioClip = null;
            if (Value != null && Value != string.Empty)
            {
                string tPath = Value.Replace(NWDAssetType.kAssetDelimiter, string.Empty);
#if UNITY_EDITOR
                rAudioClip = AssetDatabase.LoadAssetAtPath(tPath, typeof(AudioClip)) as AudioClip;
#else
                tPath = NWEPathResources.PathAbsoluteToPathDB(tPath);
                rAudioClip = Resources.Load(tPath, typeof(AudioClip)) as AudioClip;
#endif
                /*Debug.LogWarning("rTexture at path " + tPath);
                if (rAudioClip == null)
                {
                    Debug.LogWarning("rTexture is null at path " + tPath);
                }*/
            }
            return rAudioClip;
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
#if UNITY_EDITOR
        AudioClip kAudioClipUsed = null;
        AudioClip kAudioClipLoaded = null;
        string kValue = "";
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
                    AudioClip tObject = AssetDatabase.LoadAssetAtPath(tPath, typeof(AudioClip)) as AudioClip;
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
            NWDAudioClipType tTemporary = new NWDAudioClipType();
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

            bool tRessource = true;
            //string tDuration = "";
            if (!string.IsNullOrEmpty(Value))
            {
                if (kAudioClipLoaded == null || kValue != Value)
                {
                    StopClip();

                    kValue = Value;
                    string tPath = Value.Replace(NWDAssetType.kAssetDelimiter, string.Empty);
                    kAudioClipLoaded = AssetDatabase.LoadAssetAtPath(tPath, typeof(AudioClip)) as AudioClip;
                    //tDuration = TimeSpan.FromMilliseconds(GetDuration(kAudioClipLoaded)).ToString(@"mm\:ss");
                }

                if (kAudioClipLoaded == null)
                {
                    tRessource = false;
                }
                else
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
                }
                if (Value.Contains(NWD.K_Resources) == false)
                {
                    EditorGUI.LabelField(new Rect(tX, tY + tLabelAssetStyle.fixedHeight, tWidth, tLabelAssetStyle.fixedHeight), "NOT IN \"Resources\"", tLabelStyle);
                }
            }

            EditorGUI.BeginDisabledGroup(!tRessource);
            UnityEngine.Object pObj = EditorGUI.ObjectField(new Rect(tX, tY, tWidth, tObjectFieldStyle.fixedHeight), tContent, kAudioClipLoaded, typeof(AudioClip), false);
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

            if (tRessource == false)
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
        public void StopClip()
        {
            string tMethodName = "StopClip";
            #if UNITY_2021
            tMethodName = "StopAllPreviewClips";
            #endif

            GetMethodResult(tMethodName, typeof(AudioImporter), "UnityEditor.AudioUtil");

            /*Assembly tAssembly = typeof(AudioImporter).Assembly;
            Type tAudioUtilType = tAssembly.GetType("UnityEditor.AudioUtil");

            Type[] tTypes = {};
            string tMethodName = "StopClip";
            #if UNITY_2021
            tMethodName = "StopAllPreviewClips";
            #endif

            MethodInfo tMethod = tAudioUtilType.GetMethod(tMethodName, tTypes);
            if (tMethod != null)
            {
                object[] tObjects = {};
                tMethod.Invoke(null, BindingFlags.Static | BindingFlags.Public, null, tObjects, null);
            }
            else
            {
                Debug.LogWarning("Method is Null!, can't " + tMethodName + " sound");
            }*/

            kAudioClipUsed = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PlayClip(AudioClip sAudioClip)
        {
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

            /*kAudioClipUsed = sAudioClip;
            Assembly tAssembly = typeof(AudioImporter).Assembly;
            Type tAudioUtilType = tAssembly.GetType("UnityEditor.AudioUtil");
            
            Type[] tTypes = { typeof(AudioClip), typeof(int), typeof(bool) };
            string tMethodName = "PlayClip";
            #if UNITY_2021
            tMethodName = "PlayPreviewClip";
            #endif

            MethodInfo tMethod = tAudioUtilType.GetMethod(tMethodName, tTypes);
            if (tMethod != null)
            {
                int tSample = (int)(0 * sAudioClip.frequency);
                object[] tObjects = { sAudioClip, tSample, false };
                tMethod.Invoke(null, BindingFlags.Static | BindingFlags.Public, null, tObjects, null);
            }
            else
            {
                Debug.LogWarning("Method is Null!, can't " + tMethodName + " sound");
            }*/
        }
        //-------------------------------------------------------------------------------------------------------------
        public double GetDuration(AudioClip sAudioClip)
        {
            double rReturn = 0f;

            Assembly tAssembly = typeof(AudioImporter).Assembly;
            Type tAudioUtilType = tAssembly.GetType("UnityEditor.AudioUtil");
            
            Type[] tTypes = { typeof(AudioClip) };
            string tMethodName = "GetDuration";

            MethodInfo tMethod = tAudioUtilType.GetMethod(tMethodName, tTypes);
            if (tMethod != null)
            {
                object[] tObjects = { sAudioClip };
                rReturn = (double)tMethod.Invoke(null, BindingFlags.Static | BindingFlags.Public, null, tObjects, null);
            }
            else
            {
                Debug.LogWarning("Method is Null!, can't " + tMethodName + " sound");
            }

            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
