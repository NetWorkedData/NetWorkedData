using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWENativeDialogAndroid
    {
        //-------------------------------------------------------------------------------------------------------------
        public static void CallStatic(string methodName, params object[] args)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            string CLASS_NAME = "com.idemobi.networkeddata.NWEDialogAndroidManager";
            AndroidJavaObject bridge = new AndroidJavaObject(CLASS_NAME);

            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer"); 
            AndroidJavaObject act = jc.GetStatic<AndroidJavaObject>("currentActivity"); 
            
            act.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                bridge.CallStatic(methodName, args);
            }));

        } catch (System.Exception ex)
        {
            Debug.LogWarning(ex.Message);
        }
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static void showRateUsPopUP(string title, string message, string rate, string remind, string declined)
        //{
        //    CallStatic("ShowRatePopup", title, message, rate, remind, declined);
        //}
        //-------------------------------------------------------------------------------------------------------------
        public static void ShowDialog(string title, string message, string yes, string no)
        {
            CallStatic("ShowDialog", title, message, yes, no);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ShowAlert(string title, string message, string ok)
        {
            CallStatic("ShowAlert", title, message, ok);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================