

using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	#region Structures & Enums
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum ErrorType {verbose, info, alert, warning, critical}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [System.Serializable]
    public class NWEError
    {
        public string domain;
        public string code;
        public string localizedTitle;
        public string localizedDescription;
        public ErrorType type;
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	#endregion
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWEErrorManager
	{
		//-------------------------------------------------------------------------------------------------------------
        private static NWEErrorManager _instance;
        private Dictionary<string, NWEError> _domains = new Dictionary<string, NWEError>();
		//-------------------------------------------------------------------------------------------------------------
        public static NWEErrorManager ShareInstance()
        {
            if (_instance == null)
            {
                _instance = new NWEErrorManager();
            }

            return _instance;
        }
		//-------------------------------------------------------------------------------------------------------------
        private NWEErrorManager()
        {
            NWEError tError = new NWEError();
            tError.code = "UNK";
            tError.domain = Application.identifier;
            tError.localizedTitle = "Error";
            tError.localizedDescription = "No error found";
            tError.type = ErrorType.warning;

            AddError(tError);
        }
		//-------------------------------------------------------------------------------------------------------------
        public void AddError(NWEError error)
        {
            _domains.Add(error.code, error);
        }
		//-------------------------------------------------------------------------------------------------------------
        public bool RemoveError(string code)
        {
            return _domains.Remove(code);
        }
		//-------------------------------------------------------------------------------------------------------------
        public void RemoveAllError()
        {
            _domains.Clear();
        }
		//-------------------------------------------------------------------------------------------------------------
        public NWEError FindError(string code)
        {
            NWEError tResult = _domains["UNK"];
            string title = string.Empty;
            string desc = string.Empty;

            if ( _domains.ContainsKey(code) )
            {
                tResult = _domains[code];
                LogError(tResult);

                title = tResult.localizedTitle;
                desc = tResult.localizedDescription;
                AlertWithError(title, desc);
            }

            return tResult;
        }
		//-------------------------------------------------------------------------------------------------------------
        public void LogAllEntry()
        {
            foreach (KeyValuePair<string, NWEError> pair in _domains)
            {
                string key = pair.Key;
                NWEError value = pair.Value;
                Debug.Log("key: " + key + " // code: " + value.code);
            }
        }
		//-------------------------------------------------------------------------------------------------------------
        //Private Methode
        private void LogError(NWEError error)
        {
            Debug.Log("[" + error.domain + ":" + error.code + "] " + error.localizedTitle + " " + error.localizedDescription);
        }
		//-------------------------------------------------------------------------------------------------------------
        private void AlertWithError(string title, string desc)
        {
#if UNITY_EDITOR
            EditorUtility.DisplayDialog(title, desc, "OK");
#endif
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================