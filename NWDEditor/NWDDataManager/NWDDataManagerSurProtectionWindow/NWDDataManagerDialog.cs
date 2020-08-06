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
using UnityEngine;
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public delegate void NWDDataManagerDialogDelegate(string sValue);
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDDataManagerDialog : EditorWindow
    {
        //------------------------------------------------------------------------------------------------------------- 
        private string Value;
        private string ValueVerif;
        private string Title;
        private string Message;
        private MessageType DialogType = MessageType.Info;
        private bool Verif = false;
        private bool Closable = false;
        //------------------------------------------------------------------------------------------------------------- 
        NWDDataManagerDialogDelegate Delegate = null;
        //-------------------------------------------------------------------------------------------------------------
        static public void ShowDialog(string sTitle, string sMessage, MessageType sDialogType, NWDDataManagerDialogDelegate sDelegate)
        {
            ShowDialog(sTitle, sMessage, sDialogType, false, sDelegate);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void ShowDialogConfirm(string sTitle, string sMessage, MessageType sDialogType, NWDDataManagerDialogDelegate sDelegate)
        {
            ShowDialog(sTitle, sMessage, sDialogType, true, sDelegate);
        }
        //-------------------------------------------------------------------------------------------------------------
        static private void ShowDialog(string sTitle, string sMessage, MessageType sDialogType, bool sVerif, NWDDataManagerDialogDelegate sDelegate)
        {
            NWDDataManagerDialog tWindow = ScriptableObject.CreateInstance(typeof(NWDDataManagerDialog)) as NWDDataManagerDialog;
            tWindow.Show(sTitle, sMessage, sDialogType, sVerif, sDelegate);
        }
        //-------------------------------------------------------------------------------------------------------------
        private void Show(string sTitle, string sMessage, MessageType sDialogType, bool sVerif, NWDDataManagerDialogDelegate sDelegate)
        {
            Title = sTitle;
            Message = sMessage;
            DialogType = sDialogType;
            Delegate = sDelegate;
            Verif = sVerif;
            ShowUtility();
            Focus();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnLostFocus()
        {
            // force focus on this window!
            if (Closable == false)
            {
                Focus();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnEnable()
        {
            titleContent = new GUIContent(Title);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnDestroy()
        {
            if (Closable == false)
            {
            };
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetValue()
        {
            return Value;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnGUI()
        {
            //NWDBenchmark.Start();
            EditorGUILayout.HelpBox(Message, DialogType);
            bool tValid = true;
            Value = EditorGUILayout.TextField("Code", Value);
            if (Verif == true)
            {
                ValueVerif = EditorGUILayout.TextField("Code verif", ValueVerif);
                if (ValueVerif != Value)
                {
                    tValid = false;
                }
            }
            EditorGUI.BeginDisabledGroup(!tValid);
            if (GUILayout.Button("Valid"))
            {
                Delegate("" + Value);
                Closable = true;
                Close();
            }
            EditorGUI.EndDisabledGroup();
            Rect tLastRect = GUILayoutUtility.GetLastRect();
            maxSize = new Vector2(NWDGUI.KTableSearchWidth * 2, tLastRect.y + tLastRect.height + NWDGUI.kFieldMarge);
            minSize = new Vector2(NWDGUI.KTableSearchWidth * 2, tLastRect.y + tLastRect.height + NWDGUI.kFieldMarge);
            //NWDBenchmark.Finish();
        }
        //------------------------------------------------------------------------------------------------------------- 
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif