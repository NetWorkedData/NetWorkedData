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
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================

using System.Collections.Generic;

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
	public class NWEUnityWebServiceDataRequest : NWEInterfaceUnityWebServiceDelegate
	{
		public delegate void errorBlock(NWEError error);
		public errorBlock errorBlockDelegate;

		public delegate void successBlock(Dictionary<string, object> data);
		public successBlock successBlockDelegate;

		public delegate void destroyBlock();
		public destroyBlock destroyBlockDelegate;

		private NWEUnityWebService _webService;
		protected GameObject _gameObject;

		public void setUrl( string url )
		{
			_gameObject = new GameObject();
			_gameObject.name = "Unity Web Service";
			_webService = _gameObject.AddComponent<NWEUnityWebService>();
			_webService.setUrl(url);
		}

		public void setSecureURL(bool value)
		{
			_webService.mIsSecureURL = value;
		}

        public void setSecurity(int frequency = 600, string startSalt = NWEConstants.K_EMPTY_STRING,
         string endSalt = NWEConstants.K_EMPTY_STRING,
         string secret = NWEConstants.K_EMPTY_STRING,
         string vector = NWEConstants.K_EMPTY_STRING)
        {
            _webService.setSaltFrequency(frequency);
            _webService.setSaltStartKey(startSalt);
            _webService.setSaltEndKey(endSalt);
            _webService.setSecretKey(secret);
            _webService.setVectorKey(vector);
        }

        public void setSendMethod(NWEUnityWebServiceMethod method)
		{
			_webService.setSendMethod(method);
		}

		public void addHeaderParameter(string parameter, object value)
		{
			_webService.addHeaderParam(parameter, value);
		}

        public void addBodyParameter(string parameter, object value)
        {
            _webService.addBodyParam(parameter, value);
        }

        /*
		func addData( name:String, forValue data:NSData, withType type:String, andFilename filename:String) {
			_webService.addData(name, forValue:data, withType:type, andFilename:filename)
		}
		*/

		public void send()
		{
			_webService.mDelegate = this;
			_webService.send();
		}

		public void cancel()
		{
			_webService.cancel();
			destroy();
		}

		//MARK: - Utilities -
		protected NWEError errorFromCode(long code)
		{
            //200, 404 or 500
            NWEError tError = new NWEError();
			if (code != 200) {
				tError.domain = "com.webservice";
				tError.code = code.ToString();
				tError.localizedDescription = string.Empty;
			}
			else
			{
				tError.domain = string.Empty;
				tError.code = code.ToString();
				tError.localizedDescription = string.Empty;
			}

			return tError;
		}

		protected void callSuccess(Dictionary<string, object> data)
		{
			if (successBlockDelegate != null)
			{
				successBlockDelegate(data);
			}
		}

        protected void callError(NWEError error)
		{
			if (errorBlockDelegate != null)
			{
				errorBlockDelegate(error);
			}
		}

        protected void destroy()
		{
			if (destroyBlockDelegate != null)
			{
				destroyBlockDelegate();
			}
			#if UNITY_EDITOR
			GameObject.DestroyImmediate(_gameObject);
			#else
			GameObject.Destroy(_gameObject);
			#endif
		}

		// IWebService Delegate
		public void UWSConnectionDidFail(string error)
		{
            NWEError tError = new NWEError();
			tError.domain = "";
			tError.code = "500";
			tError.localizedDescription = error;

			callError(tError);
			destroy();
		}

		public void UWSConnectionDidFinish(long response, Dictionary<string, object> data)
		{
            NWEError tError = errorFromCode(response);
			Debug.Log("WSConnectionDidFinish: " + tError.localizedDescription);

			Debug.Log ("------ WSConnectionDidFinish finish " + Time.time.ToString ());

			// Check
			if( tError.code != "200" )
			{
				callError(tError);
			}
			else
			{
				callSuccess(data);
			}

			// Destroy
			destroy();
		}

		public void UWSConnectionDidReceiveResponse(long response, string url)
		{
			Debug.Log("WSConnectionDidReceiveResponse: " + response + " -url : " + url);
		}
	}
}
//=====================================================================================================================