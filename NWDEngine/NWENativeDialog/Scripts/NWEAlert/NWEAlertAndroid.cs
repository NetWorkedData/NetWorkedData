using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWEAlertAndroid : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        const string K_OK = "OK";
        //-------------------------------------------------------------------------------------------------------------
        public string Title;
        public string Message;
        public string Ok;
        public NWEAlertOnCompleteBlock CompleteBlock;
        //-------------------------------------------------------------------------------------------------------------
        public static NWEAlertAndroid Create(string sTitle, string sMessage)
        {
            return Create(sTitle, sMessage, K_OK, null);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWEAlertAndroid Create(string sTitle, string sMessage, string sOK, NWEAlertOnCompleteBlock sCompleteBlock)
        {
            NWEAlertAndroid tDialog = new GameObject("NWEAlertAndroid_GameObject").AddComponent<NWEAlertAndroid>();
            tDialog.Title = sTitle;
            tDialog.Message = sMessage;
            tDialog.Ok = sOK;
            tDialog.CompleteBlock = sCompleteBlock;
            tDialog.Initialization();
            return tDialog;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Initialization()
        {
            NWENativeDialogAndroid.ShowAlert(Title, Message, Ok);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnAlertCallback(string sButtonIndex) // call from MobileNativePopup.jar! Don't change the name
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
