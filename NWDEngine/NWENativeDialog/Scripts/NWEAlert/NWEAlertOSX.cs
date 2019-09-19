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
    public class NWEAlertOSX : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        const string K_OK = "OK";
        //-------------------------------------------------------------------------------------------------------------
        public string Title;
        public string Message;
        public string Ok;
        public NWEAlertOnCompleteBlock CompleteBlock;
        //-------------------------------------------------------------------------------------------------------------
        public static NWEAlertOSX Create(string sTitle, string sMessage)
        {
            Debug.Log("NWEAlertOSX CreateCreate()");
            return Create(sTitle, sMessage, K_OK, null);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWEAlertOSX Create(string sTitle, string sMessage, string sOK, NWEAlertOnCompleteBlock sCompleteBlock)
        {
            Debug.Log("NWEAlertOSX Create()");
            NWEAlertOSX tDialog = new GameObject("NWEAlertOSX_GameObject").AddComponent<NWEAlertOSX>();
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
            Debug.Log("NWEAlertOSX Initialization()");
            NWENativeDialogOSX.ShowAlert(Title, Message, Ok);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnAlertCallback(string sButtonIndex) // call from .mm! Don't change the name
        {
            Debug.Log("NWEAlertOSX OnAlertCallback()");
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
