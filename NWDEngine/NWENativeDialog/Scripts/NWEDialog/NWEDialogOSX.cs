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
    public class NWEDialogOSX : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Title;
        public string Message;
        public string OK;
        public string NOK;
        public NWEDialogOnCompleteBlock CompleteBlock;
        //-------------------------------------------------------------------------------------------------------------
        public static NWEDialogOSX Create(string sTitle, string sMessage)
        {
            Debug.Log("NWEDialogOSX Create()");
            return Create(sTitle, sMessage, "Yes", "No", null);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWEDialogOSX Create(string sTitle, string sMessage, string sOK, string sNOK, NWEDialogOnCompleteBlock sCompleteBlock)
        {
            Debug.Log("NWEDialogOSX Create()");
            NWEDialogOSX tDialog = new GameObject("NWEDialogOSX_GameObject").AddComponent<NWEDialogOSX>();
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
            Debug.Log("NWEDialogOSX Initialization()");
            NWENativeDialogOSX.ShowDialog(Title, Message, OK, NOK);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnDialogCallback(string sButtonIndex) // call from .mm! Don't change the name
        {
            Debug.Log("NWEDialogOSX OnDialogCallback() sButtonIndex = " + sButtonIndex);
            if (CompleteBlock != null)
            {
                int tIndex = System.Convert.ToInt16(sButtonIndex);
                switch (tIndex)
                {
                    case 0:
                        {
                            Debug.Log("OK");
                            CompleteBlock(NWEMessageState.OK);
                        }
                        break;
                    case 1:
                        {
                            Debug.Log("NOK");
                            CompleteBlock(NWEMessageState.NOK);
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
