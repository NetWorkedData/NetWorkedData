//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#endif
//=====================================================================================================================

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Networking;

using NWEMiniJSON;

//=====================================================================================================================
namespace NetWorkedData
{
	public enum NWEUnityWebServiceMethod
	{
		POST,
		GET
    };

	public interface NWEInterfaceUnityWebServiceDelegate
	{
		void UWSConnectionDidFail (string error);
		void UWSConnectionDidFinish (long response, Dictionary<string, object> data);
		void UWSConnectionDidReceiveResponse (long response, string url);
	}

	public class NWEUnityWebService : MonoBehaviour
	{
        
        public const string UnSecureKey = "prm";
        public const string SecureKey = "scr";
        public const string UnSecureDigestKey = "prmdgt";
        public const string SecureDigestKey = "scrdgt";


		public bool mIsSecureURL = false;
		public NWEInterfaceUnityWebServiceDelegate mDelegate;

		protected UnityWebRequest mUnityWebRequest;
		protected Dictionary<string, object> mHeaderParams = new Dictionary<string, object> ();
		protected Dictionary<string, object> mBodyParams = new Dictionary<string, object> ();
		protected string mURL = string.Empty;
		protected NWEUnityWebServiceMethod mSendMethod = NWEUnityWebServiceMethod.POST;
		protected int mSaltFrequency = 600;
		private string mSaltStartKey = string.Empty;
		private string mSaltEndKey = string.Empty;
		private string mSecretKey = string.Empty;
		private string mVectorKey = string.Empty;

		public void setUrl (string url)
		{
			mURL = url;
		}

		public void addHeaderParam (string key, object value)
		{
			mHeaderParams.Add (key, value);
		}

		public void addBodyParam (string key, object value)
		{
			mBodyParams.Add (key, value);
		}

		public void setSendMethod (NWEUnityWebServiceMethod method)
		{
			mSendMethod = method;
		}

		public void setSaltFrequency (int value)
		{
			mSaltFrequency = value;
		}

		public void setSaltStartKey (string value)
		{
			mSaltStartKey = value;
		}

		public void setSaltEndKey (string value)
		{
			mSaltEndKey = value;
		}

		public void setSecretKey (string value)
		{
			mSecretKey = value.Substring (0, 16);
		}

		public void setVectorKey (string value)
		{
			mVectorKey = value.Substring (0, 16);
		}

		public void send ()
		{
			StartCoroutine (sendRequest ());
		}

		IEnumerator sendRequest ()
		{
			string tURL = mURL;
			WWWForm tBodyData = new WWWForm ();

			string tParamKey = UnSecureKey;
			string tDigestKey = UnSecureDigestKey;
			string tParam = string.Empty;
			string tDigest = string.Empty;

			if (mIsSecureURL) {
				tParamKey = SecureKey;
				tDigestKey = SecureDigestKey;

				tParam = NWESecurityTools.AddAes (mBodyParams, mSecretKey, mVectorKey, NWESecurityAesTypeEnum.Aes128);
				tDigest = NWESecurityTools.GenerateSha(mSaltStartKey + tParam + mSaltEndKey, NWESecurityShaTypeEnum.Sha1);
			} else {
				tParam = NWESecurityTools.Base64Encode (Json.Serialize (mBodyParams));
				tDigest = NWESecurityTools.GenerateSha(mSaltStartKey + tParam + mSaltEndKey, NWESecurityShaTypeEnum.Sha1);
			}

			Debug.Log ("tParamJSON = " + Json.Serialize (mBodyParams));
			Debug.Log ("tParam = " + tParam);

			if (mSendMethod == NWEUnityWebServiceMethod.GET) {
				tURL += "?" + tParamKey + "=" + tParam + "&" + tDigestKey + "=" + tDigest;

			} else if (mSendMethod == NWEUnityWebServiceMethod.POST) {
				tBodyData.AddField (tParamKey, tParam);
				tBodyData.AddField (tDigestKey, tDigest);
			}

			//prepare the request with header and body params
			prepareRequest (mSendMethod, tURL, mHeaderParams, tBodyData);

			//send the request
            yield return mUnityWebRequest.SendWebRequest();

			mDelegate.UWSConnectionDidReceiveResponse (mUnityWebRequest.responseCode, mURL);

			if (mUnityWebRequest.isNetworkError) {
				mDelegate.UWSConnectionDidFail (mUnityWebRequest.error);
			} else {
				string dataFromServer = mUnityWebRequest.downloadHandler.text;

				Debug.Log ("data from server:" + dataFromServer);
				Dictionary<string, object> data = new Dictionary<string, object> ();
				if (dataFromServer.Equals (string.Empty)) {
					data = new Dictionary<string, object> ();
					data.Add ("error", true);
					data.Add ("error_code", "0");
					data.Add ("error_description", "not a json object");
				} else {
					data = Json.Deserialize (dataFromServer) as Dictionary<string, object>;
				}
				mDelegate.UWSConnectionDidFinish (mUnityWebRequest.responseCode, data);
			}
		}

		public void cancel ()
		{
			mUnityWebRequest.Abort ();
		}

		protected void prepareRequest (NWEUnityWebServiceMethod type, string url, Dictionary<string, object> header, WWWForm body)
		{
			if (type == NWEUnityWebServiceMethod.POST) {
				mUnityWebRequest = UnityWebRequest.Post (url, body);
			} else if (type == NWEUnityWebServiceMethod.GET) {
				mUnityWebRequest = UnityWebRequest.Get (url);
			}

			foreach (KeyValuePair<string, object> entry in header) {
				mUnityWebRequest.SetRequestHeader (entry.Key, entry.Value.ToString ());
			}

			Debug.Log ("body = " + body.data.ToString ());
		}
	}
}
//=====================================================================================================================