using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDDialogWin : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Title;
        public string Message;
        public string OK;
        public string NOK;
        public NWDDialogOnCompleteBlock CompleteBlock;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDDialogWin CreateCreate(string sTitle, string sMessage)
        {
            return Create(sTitle, sMessage, "Yes", "No",null);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDDialogWin Create(string sTitle, string sMessage, string sOK, string sNOK, NWDDialogOnCompleteBlock sCompleteBlock)
        {
            NWDDialogWin tDialog = new GameObject("BTBDialogWin_GameObject").AddComponent<NWDDialogWin>();
            tDialog.Title = sTitle;
            tDialog.Message = sMessage;
            tDialog.OK = sOK;
            tDialog.NOK = sNOK;
            tDialog.CompleteBlock = sCompleteBlock;
            tDialog.Initialization();
            return tDialog;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Initialization()
        {
            NWDNativeDialogWin.ShowDialog(Title, Message, OK, NOK);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnDialogCallback(string sButtonIndex) // call from .mm! Don't change the name
        {
            if (CompleteBlock != null)
            {
                int tIndex = System.Convert.ToInt16(sButtonIndex);
                switch (tIndex)
                {
                    case 0:
                        CompleteBlock(NWDMessageState.OK);
                        break;
                    case 1:
                        CompleteBlock(NWDMessageState.NOK);
                        break;
                }
            }
            Destroy(gameObject);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
