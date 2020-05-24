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
    public class NWDAlertOSX : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        const string K_OK = "OK";
        //-------------------------------------------------------------------------------------------------------------
        public string Title;
        public string Message;
        public string Ok;
        public NWDAlertOnCompleteBlock CompleteBlock;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAlertOSX Create(string sTitle, string sMessage)
        {
            return Create(sTitle, sMessage, K_OK, null);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAlertOSX Create(string sTitle, string sMessage, string sOK, NWDAlertOnCompleteBlock sCompleteBlock)
        {
            NWDAlertOSX tDialog = new GameObject("NWDAlertOSX_GameObject").AddComponent<NWDAlertOSX>();
            tDialog.Title = sTitle;
            tDialog.Message = sMessage;
            tDialog.Ok = sOK;
            tDialog.CompleteBlock = sCompleteBlock;
            return tDialog;
        }
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            int rResult = NWDNativeDialogOSX.ShowAlert(Title, Message, Ok);
            if (rResult == 0)
            {
                if (CompleteBlock != null)
                {
                    CompleteBlock(NWDMessageState.OK);
                }
            }
            else
            {
                Debug.LogWarning("NWDAlertOSX ShowAlert() error!");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
