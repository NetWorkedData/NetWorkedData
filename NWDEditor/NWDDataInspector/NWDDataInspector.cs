// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:22:33
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using System.IO;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDDataInspector : EditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDTypeClass mObjectInEdition;
        public List<NWDTypeClass> mObjectsList = new List<NWDTypeClass>();
        public int ActualIndex = 0;
        public bool RemoveActualFocus = true;
        //-------------------------------------------------------------------------------------------------------------
        private GUIContent IconAndTitle;
        //-------------------------------------------------------------------------------------------------------------
        static NWDDataInspector kShareInstance;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDDataInspector ShareInstance()
        {
            //BTBBenchmark.Start();
            if (kShareInstance == null)
            {
                EditorWindow tWindow = EditorWindow.GetWindow(typeof(NWDDataInspector));
                tWindow.Show();
                kShareInstance = (NWDDataInspector)tWindow;
                kShareInstance.minSize = new Vector2(300, 500);
                kShareInstance.maxSize = new Vector2(600, 2048);
            }
            //BTBBenchmark.Finish();
            return kShareInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ActiveInspector()
        {
            ShareInstance().Show();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ActiveRepaint()
        {
            if (kShareInstance != null)
            {
                kShareInstance.Repaint();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void InspectNetWorkedDataPreview()
        {
            ShareInstance().DataPreview();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DataPreview()
        {
            //BTBBenchmark.Start();
            ActualIndex--;
            if (ActualIndex < 0)
            {
                ActualIndex = 0;
            }
            NWDTypeClass tTarget = mObjectsList[ActualIndex];
            mObjectInEdition = tTarget;
            Repaint();
            RemoveActualFocus = true;
            Focus();
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void InspectNetWorkedDataNext()
        {
            ShareInstance().DataNext();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DataNext()
        {
            //BTBBenchmark.Start();
            ActualIndex++;
            if (ActualIndex >= mObjectsList.Count)
            {
                ActualIndex = 0;
            }
            NWDTypeClass tTarget = mObjectsList[ActualIndex];
            mObjectInEdition = tTarget;
            Repaint();
            RemoveActualFocus = true;
            Focus();
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool InspectNetWorkedPreview()
        {
            return ShareInstance().Preview();
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool Preview()
        {
            return (ActualIndex > 0);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool InspectNetWorkedNext()
        {
            return ShareInstance().Next();
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool Next()
        {
            return (ActualIndex < mObjectsList.Count - 1);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void InspectNetWorkedData(NWDTypeClass sTarget, bool sResetStack = true, bool sFocus = true)
        {
            //BTBBenchmark.Start();
            if (sTarget != null)
            {
                if (NWDBasisHelper.FindTypeInfos(sTarget.GetType()).DatabaseIsLoaded())
                {
                    if (ShareInstance().mObjectInEdition != sTarget)
                    {
                        ShareInstance().Data(sTarget, sResetStack, sFocus);
                        NWDDataManager.SharedInstance().RepaintWindowsInManager(sTarget.GetType());
                    }
                }
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Data(NWDTypeClass sTarget, bool sResetStack = true, bool sFocus = true)
        {
            //BTBBenchmark.Start();
            if (sTarget != null)
            {
                if (NWDBasisHelper.FindTypeInfos(sTarget.GetType()).DatabaseIsLoaded())
                {
                    //MethodBase tMethodInfo = NWDAliasMethod.GetMethodPublicInstance(sTarget.GetType(), NWDConstants.M_CheckError);
                    //if (tMethodInfo != null)
                    //{
                    //    tMethodInfo.Invoke(sTarget, null);
                    //}
                    NWDTypeClass tTarget = sTarget as NWDTypeClass;
                    tTarget.ErrorCheck();

                    if (sResetStack == true)
                    {
                        mObjectsList = new List<NWDTypeClass>();
                    }
                    else
                    {
                        if (mObjectsList.Count > ActualIndex)
                        {
                            mObjectsList.RemoveRange(ActualIndex + 1, mObjectsList.Count - ActualIndex - 1);
                        }
                    }
                    ActualIndex = mObjectsList.Count;
                    mObjectsList.Add(sTarget);
                    mObjectInEdition = sTarget;
                    Repaint();
                    RemoveActualFocus = sFocus;
                    if (sFocus == true)
                    {
                        Focus();
                    }
                    NWDNodeEditor.ReDraw();
                }
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDTypeClass ObjectInEdition()
        {
            return ShareInstance().mObjectInEdition;
        }
        //-------------------------------------------------------------------------------------------------------------
        // Use this for initialization
        void Start()
        {
            //Debug.Log ("Start");
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnEnable()
        {
            //Debug.Log ("OnEnable");

        }
        //-------------------------------------------------------------------------------------------------------------
        //		public void Update ()
        //		{
        //			Debug.Log ("Update");
        //		}
        //-------------------------------------------------------------------------------------------------------------
        void OnDestroy()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnGUI()
        {
            //BTBBenchmark.Start();
            if (RemoveActualFocus == true)
            {
                GUI.FocusControl(null);
                RemoveActualFocus = false;
            }
            if (IconAndTitle == null)
            {
                IconAndTitle = new GUIContent();
                IconAndTitle.text = NWDConstants.K_APP_SYNC_INSPECTOR_TITLE;
                if (IconAndTitle.image == null)
                {
                    string[] sGUIDs = AssetDatabase.FindAssets("NWDDataInspector t:texture");
                    foreach (string tGUID in sGUIDs)
                    {
                        //Debug.Log("TextureOfClass GUID " + tGUID);
                        string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                        string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                        //Debug.Log("tPathFilename = " + tPathFilename);
                        if (tPathFilename.Equals("NWDDataInspector"))
                        {
                            //Debug.Log("TextureOfClass " + tPath);
                            IconAndTitle.image = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                        }
                    }
                }
                titleContent = IconAndTitle;
            }
            //			ScrollPosition = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height), ScrollPosition, new Rect(0, 0, position.width, position.height*2));
            if (mObjectInEdition == null)
            {
            }
            else
            {
                //Type tType = mObjectInEdition.GetType();
                //MethodInfo tMethodInfo = NWDAliasMethod.GetMethodPublicInstance(tType, NWDConstants.M_DrawObjectEditor);
                //if (tMethodInfo != null)
                //{
                //    tMethodInfo.Invoke(mObjectInEdition, new object[] { position, true });
                //}
                //mObjectInEdition.New_DrawObjectEditor(position, true);
                mObjectInEdition.New_DrawObjectEditor(new Rect (0,0,position.width, position.height), true, null);
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif