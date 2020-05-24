using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDDialogIOS : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Title;
        public string Message;
        public string OK;
        public string NOK;
        public NWDDialogOnCompleteBlock CompleteBlock;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDDialogIOS CreateCreate(string sTitle, string sMessage)
        {
            return Create(sTitle, sMessage, "Yes", "No",null);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDDialogIOS Create(string sTitle, string sMessage, string sOK, string sNOK, NWDDialogOnCompleteBlock sCompleteBlock)
        {
           NWDDialogIOS tDialog = new GameObject("NWDDialogIOS_GameObject").AddComponent<NWDDialogIOS>();
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
            NWDNativeDialogIOS.ShowDialog(Title, Message, OK, NOK);
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
                        {
                        Debug.Log("OnDialogCallback call NWDMessageState.OK");
                        CompleteBlock(NWDMessageState.OK);
                        }
                        break;
                    case 1:
                        {
                        Debug.Log("OnDialogCallback call NWDMessageState.NOK");
                        CompleteBlock(NWDMessageState.NOK);
                        }
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
