using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWEAlertIOS : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        const string K_OK = "OK";
        //-------------------------------------------------------------------------------------------------------------
        public string Title;
        public string Message;
        public string Ok;
        public NWEAlertOnCompleteBlock CompleteBlock;
        //-------------------------------------------------------------------------------------------------------------
        public static NWEAlertIOS Create(string sTitle, string sMessage)
        {
            return Create(sTitle, sMessage, K_OK, null);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWEAlertIOS Create(string sTitle, string sMessage, string sOK, NWEAlertOnCompleteBlock sCompleteBlock)
        {
            NWEAlertIOS tDialog = new GameObject("NWEAlertIOS_GameObject").AddComponent<NWEAlertIOS>();
            tDialog.Title = sTitle;
            tDialog.Message = sMessage;
            tDialog.Ok = sOK;
            tDialog.CompleteBlock = sCompleteBlock;
            tDialog.Initialization();
            return tDialog;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Exec()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Initialization()
        {
            NWENativeDialogIOS.ShowAlert(Title, Message, Ok);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnAlertCallback(string sButtonIndex) // call from .mm! Don't change the name
        {
            if (CompleteBlock != null)
            {
                CompleteBlock(NWEMessageState.OK);
            }
            Destroy(gameObject);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
