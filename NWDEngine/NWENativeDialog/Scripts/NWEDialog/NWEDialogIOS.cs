using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWEDialogIOS : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Title;
        public string Message;
        public string OK;
        public string NOK;
        public NWEDialogOnCompleteBlock CompleteBlock;
        //-------------------------------------------------------------------------------------------------------------
        public static NWEDialogIOS Create(string sTitle, string sMessage)
        {
            return Create(sTitle, sMessage, "Yes", "No",null);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWEDialogIOS Create(string sTitle, string sMessage, string sOK, string sNOK, NWEDialogOnCompleteBlock sCompleteBlock)
        {
            NWEDialogIOS tDialog = new GameObject("NWEDialogIOS_GameObject").AddComponent<NWEDialogIOS>();
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
            NWENativeDialogIOS.ShowDialog(Title, Message, OK, NOK);
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
                        CompleteBlock(NWEMessageState.OK);
                        break;
                    case 1:
                        CompleteBlock(NWEMessageState.NOK);
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
