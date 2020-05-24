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
    public class NWDDialogOSX : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Title;
        public string Message;
        public string OK;
        public string NOK;
        public NWDDialogOnCompleteBlock CompleteBlock;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDDialogOSX CreateCreate(string sTitle, string sMessage)
        {
            return Create(sTitle, sMessage, "Yes", "No", null);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDDialogOSX Create(string sTitle, string sMessage, string sOK, string sNOK, NWDDialogOnCompleteBlock sCompleteBlock)
        {
            NWDDialogOSX tDialog = new GameObject("NWDDialogOSX_GameObject").AddComponent<NWDDialogOSX>();
            tDialog.Title = sTitle;
            tDialog.Message = sMessage;
            tDialog.OK = sOK;
            tDialog.NOK = sNOK;
            tDialog.CompleteBlock = sCompleteBlock;
            return tDialog;
        }
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            int rResult = NWDNativeDialogOSX.ShowDialog(Title, Message, OK, NOK);
            if (rResult == 0)
            {
                if (CompleteBlock != null)
                {
                    CompleteBlock(NWDMessageState.OK);
                }
            }
            else if (rResult == 1)
            {
                if (CompleteBlock != null)
                {
                    CompleteBlock(NWDMessageState.NOK);
                }
            }
            else
            {
                Debug.LogWarning("NWDDialogOSX ShowDialog() error!");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
