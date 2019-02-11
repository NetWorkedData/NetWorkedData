//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Reflection;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDBasisChooserMode : int
    {
        Add,
        Replace,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDBasisChooserDialog : EditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        static NWDBasisChooserDialog kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        public Type ClassToShow;
        public object ObjectToUse;
        public PropertyInfo ObjectProperty;
        public NWDBasisChooserMode Mode = NWDBasisChooserMode.Replace;
        public string ActualValue;
        public int IndexValue;
        public string ReplaceValue;
        public bool Loop = false;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDBasisChooserDialog SharedInstance()
        {
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDBasisChooserDialog)) as NWDBasisChooserDialog;
            }
            kSharedInstance.Show();
            kSharedInstance.Focus();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string SelectObject (Type sClassToShow,
                                         object sObjectToUse,
                                         PropertyInfo sObjectProperty, 
                                         NWDBasisChooserMode sMode, 
                                         string sActualValue, 
                                         int sIndexValue)
        {
            //Debug.Log("NWDBasisChooserDialog SelectObject()");
            string rReturn = null;
            NWDBasisChooserDialog tWindow = SharedInstance();
            tWindow.ClassToShow = sClassToShow;
            tWindow.ObjectToUse = sObjectToUse;
            tWindow.ObjectProperty = sObjectProperty;
            tWindow.Mode = sMode;
            tWindow.ActualValue = sActualValue;
            tWindow.IndexValue = sIndexValue;
            if (tWindow.Mode == NWDBasisChooserMode.Add)
            {
                tWindow.ReplaceValue = string.Empty;
            }
            else
            {
                tWindow.ReplaceValue = sActualValue;
            }
            //tWindow.Loop = true;
            //{
            //    while (tWindow.Loop == true)
            //    {
            //        // waiting final
            //    }
            //}
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        void SelectBasis(NWDTypeClass tObject)
        {
            //Debug.Log("NWDBasisChooserDialog SelectBasis()");
            if (Mode == NWDBasisChooserMode.Add)
            {
                // Add object in reference list
            }
            else
            {
                // Replace Object reference by this reference at Index 
            }
            Loop = true;
        }
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            //Debug.Log("NWDBasisChooserDialog Start()");
        }
        //-------------------------------------------------------------------------------------------------------------
        void Update()
        {
            //Debug.Log("NWDBasisChooserDialog Update()");
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnGUI()
        {
            //Debug.Log("NWDBasisChooserDialog OnGUI()");
            // Draw filer zone

            // Draw table with data filter

        }
        //-------------------------------------------------------------------------------------------------------------
        void OnFocus()
        {
            //Debug.Log("NWDBasisChooserDialog OnFocus()");
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnLostFocus()
        {
            //Debug.Log("NWDBasisChooserDialog OnLostFocus()");
            Loop = true;
            Close();
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnClose()
        {
            //Debug.Log("NWDBasisChooserDialog OnClose()");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif