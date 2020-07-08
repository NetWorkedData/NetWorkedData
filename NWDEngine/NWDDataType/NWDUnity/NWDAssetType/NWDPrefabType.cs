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
#endif
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField]
	public class NWDPrefabType : NWDAssetType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDPrefabType ()
		{
			Value = string.Empty;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDPrefabType (string sValue = NWEConstants.K_EMPTY_STRING)
		{
			if (sValue == null) {
				Value = string.Empty;
			} else {
				Value = sValue;
			}
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        public GameObject ToPrefabConnected(GameObject sParent = null)
        {
            GameObject rReturn = null;
            GameObject tPrefab = ToPrefab();
            if (tPrefab != null)
            {
                rReturn = PrefabUtility.InstantiatePrefab(tPrefab) as GameObject;
                if (sParent != null)
                {
                    rReturn.transform.SetParent(sParent.transform);
                    rReturn.transform.localPosition = Vector3.zero;
                }
            }
            return rReturn;
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        public GameObject ToGameObjectAsync(GameObject sInterim, NWDOperationAssetDelegate sDelegate)
        {
            string tPath = Value.Replace(NWDAssetType.kAssetDelimiter, string.Empty);
            tPath = NWEPathResources.PathAbsoluteToPathDB(tPath);
            NWDOperationAsset tOperation = NWDOperationAsset.AddOperation(tPath, sInterim, false, sDelegate);
            return tOperation.Interim;
        }
        //-------------------------------------------------------------------------------------------------------------
        public GameObject ToPrefab ()
		{
			GameObject tObject = null;
            if (Value != null && Value != string.Empty)
            {
                string tPath = Value.Replace(NWDAssetType.kAssetDelimiter, string.Empty);
#if UNITY_EDITOR
                tObject = AssetDatabase.LoadAssetAtPath(tPath, typeof(GameObject)) as GameObject;
                Resources.LoadAsync(tPath, typeof(GameObject));
#else
                tPath = NWEPathResources.PathAbsoluteToPathDB(tPath);
                Debug.Log("ToPrefab() path = " + tPath);
                tObject = Resources.Load (tPath, typeof(GameObject)) as GameObject;
#endif
                //Debug.LogWarning("tObject at path " + tPath);
            }
            return tObject;
        }
        //-------------------------------------------------------------------------------------------------------------
//        private GameObject ResultPrefab;
//        private GameObject ResultGameObject;
//        //private bool ResultGameObjectIsDone;
//        //-------------------------------------------------------------------------------------------------------------
//        public GameObject ToPrefabAsync(GameObject sProvisoire)
//        {
//            if (ResultPrefab == null)
//            {
//                ResultGameObject = UnityEngine.Object.Instantiate(sProvisoire) as GameObject;
//                StartCoroutine(LoadCharacters());
//            }
//            else
//            {
//                ResultGameObject = UnityEngine.Object.Instantiate(ResultPrefab) as GameObject;
//            }
//            return ResultGameObject;
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public ResourceRequest ToPrefabAsyncRequest()
//        {
//            ResourceRequest tResourceRequest = null;
//            if (Value != null && Value != string.Empty)
//            {
//                string tPath = Value.Replace(NWDAssetType.kAssetDelimiter, string.Empty);
//#if UNITY_EDITOR
//                tResourceRequest = Resources.LoadAsync(tPath, typeof(GameObject));
//#else
//                tPath = NWEPathResources.PathAbsoluteToPathDB(tPath);
//                tResourceRequest = Resources.LoadAsync(tPath, typeof(GameObject));
//#endif
        //        //Debug.LogWarning("tObject at path " + tPath);
        //    }
        //    return tResourceRequest;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public GameObject ToGameObject(GameObject sParent = null, bool sWorldPosition = true)
		{
			GameObject rReturn = null;
			GameObject tPrefab = ToPrefab();
			if (tPrefab != null)
            {
				rReturn = UnityEngine.Object.Instantiate(tPrefab);
				if (sParent != null)
                {
					rReturn.transform.SetParent(sParent.transform, sWorldPosition);
				}
			}
			return rReturn;
		}
        //-------------------------------------------------------------------------------------------------------------
        public bool IsEmpty()
        {
            return (Value == string.Empty);
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
                    GameObject tObject = AssetDatabase.LoadAssetAtPath(tPath, typeof(GameObject)) as GameObject;
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
        public override float ControlFieldHeight ()
		{
			int tAdd = 0;
			if (Value != string.Empty) {
				tAdd = 1;
			}
			GUIStyle tObjectFieldStyle = new GUIStyle (EditorStyles.objectField);
			tObjectFieldStyle.fixedHeight = tObjectFieldStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			GUIStyle tLabelStyle = new GUIStyle (EditorStyles.label);
			tLabelStyle.fixedHeight = tLabelStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			GUIStyle tMiniButtonStyle = new GUIStyle (EditorStyles.miniButton);
			tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			GUIStyle tLabelAssetStyle = new GUIStyle (EditorStyles.label);
			tLabelAssetStyle.fontSize = 12;
			tLabelAssetStyle.fixedHeight = tLabelAssetStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			tLabelAssetStyle.normal.textColor = Color.gray;

			return tObjectFieldStyle.fixedHeight + tAdd * (NWDGUI.kPrefabSize + NWDGUI.kFieldMarge);
		}
		//-------------------------------------------------------------------------------------------------------------
        public override object ControlField (Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = NWEConstants.K_EMPTY_STRING, object sAdditionnal = null)
		{
            NWDPrefabType tTemporary = new NWDPrefabType ();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

			tTemporary.Value = Value;

			float tWidth = sPosition.width;
			float tHeight = sPosition.height;
			float tX = sPosition.position.x;
			float tY = sPosition.position.y;

			GUIStyle tObjectFieldStyle = new GUIStyle (EditorStyles.objectField);
			tObjectFieldStyle.fixedHeight = tObjectFieldStyle.CalcHeight (new GUIContent ("A"), tWidth);
			GUIStyle tLabelStyle = new GUIStyle (EditorStyles.label);
			tLabelStyle.fixedHeight = tLabelStyle.CalcHeight (new GUIContent ("A"), tWidth);
			tLabelStyle.normal.textColor = Color.red;
			GUIStyle tLabelAssetStyle = new GUIStyle (EditorStyles.label);
			tLabelAssetStyle.fontSize = 12;
			tLabelAssetStyle.fixedHeight = tLabelAssetStyle.CalcHeight (new GUIContent ("A"), tWidth);
			tLabelAssetStyle.normal.textColor = Color.gray;
			GUIStyle tMiniButtonStyle = new GUIStyle (EditorStyles.miniButton);
			tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight (new GUIContent ("A"), tWidth);

			GameObject tObject = null;

			bool tRessource = true;

			if (Value != null && Value != string.Empty) {
				string tPath = Value.Replace (NWDAssetType.kAssetDelimiter, string.Empty);
				tObject = AssetDatabase.LoadAssetAtPath (tPath, typeof(GameObject)) as GameObject;
				if (tObject == null) {
					tRessource = false;
				} else {
					Texture2D tTexture2D = AssetPreview.GetAssetPreview (tObject);
					if (tTexture2D != null) {
						EditorGUI.DrawPreviewTexture (new Rect (tX + EditorGUIUtility.labelWidth, tY + NWDGUI.kFieldMarge + tObjectFieldStyle.fixedHeight, NWDGUI.kPrefabSize, NWDGUI.kPrefabSize)
							, tTexture2D);
					}
                }
                if (Value.Contains(NWD.K_Resources) == false)
                {
                    EditorGUI.LabelField(new Rect(tX, tY + tLabelAssetStyle.fixedHeight, tWidth, tLabelAssetStyle.fixedHeight), NWDConstants.K_NOT_IN_RESOURCES_FOLDER, tLabelStyle);
                }
			}

			EditorGUI.BeginDisabledGroup (!tRessource);
            UnityEngine.Object pObj = EditorGUI.ObjectField (new Rect (tX, tY, tWidth, tObjectFieldStyle.fixedHeight), tContent, (UnityEngine.Object)tObject, typeof(GameObject), false);
			tY = tY + NWDGUI.kFieldMarge + tObjectFieldStyle.fixedHeight;
			if (pObj != null) {
                string tPreFabGameObject = AssetDatabase.GetAssetPath(pObj);
                PrefabAssetType tAssetType = PrefabUtility.GetPrefabAssetType(pObj);
                if (tAssetType == PrefabAssetType.Model ||
                    tAssetType == PrefabAssetType.Regular ||
                    tAssetType == PrefabAssetType.Variant)
                {

                    tTemporary.Value = NWDAssetType.kAssetDelimiter + tPreFabGameObject + NWDAssetType.kAssetDelimiter;
                }

                //if (PrefabUtility.GetPrefabType (pObj) == PrefabType.Prefab) 
                //if (PrefabUtility.GetPrefabInstanceStatus(pObj) == PrefabInstanceStatus.Connected)
                //if (PrefabUtility.GetPrefabType(pObj) == PrefabType.Prefab)
                //{
                ////tTemporary.Value = NWDAssetType.kAssetDelimiter + AssetDatabase.GetAssetPath (PrefabUtility.GetPrefabInstanceHandle(pObj)) + NWDAssetType.kAssetDelimiter;
                //    tTemporary.Value = NWDAssetType.kAssetDelimiter + AssetDatabase.GetAssetPath(PrefabUtility.GetPrefabObject(pObj)) + NWDAssetType.kAssetDelimiter;
                //}
            }
            else {
				tTemporary.Value = string.Empty;
			}
			EditorGUI.EndDisabledGroup ();
			if (tRessource == true) {
			} else {
				tTemporary.Value = Value;

				GUI.Label (new Rect (tX + EditorGUIUtility.labelWidth, tY, tWidth, tLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_ASSET_MUST_BE_DOWNLOAD, tLabelStyle);
				tY = tY + NWDGUI.kFieldMarge + tLabelStyle.fixedHeight;
				GUI.Label (new Rect (tX + EditorGUIUtility.labelWidth, tY, tWidth, tLabelAssetStyle.fixedHeight), Value.Replace (NWDAssetType.kAssetDelimiter, ""), tLabelAssetStyle);
				tY = tY + NWDGUI.kFieldMarge + tLabelAssetStyle.fixedHeight;
                NWDGUI.BeginRedArea();
                if (GUI.Button (new Rect (tX + EditorGUIUtility.labelWidth, tY, 60.0F, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE_CLEAN, tMiniButtonStyle)) {
					tTemporary.Value = string.Empty;
                }
                NWDGUI.EndRedArea();
                tY = tY + NWDGUI.kFieldMarge + tMiniButtonStyle.fixedHeight;
			}
			return tTemporary;
		}
        //-------------------------------------------------------------------------------------------------------------
        #endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================