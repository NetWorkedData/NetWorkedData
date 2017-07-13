using BasicToolBox;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace NetWorkedData
{
    public class NWDOperationResult : BTBOperationResult
    {
        public int timestamp { get; private set; }
        public float perform { get; private set; }
        public bool isError { get; private set; }
        public string errorCode { get; private set; }
        public BTBError errorDesc { get; private set; }
        public string token { get; private set; }
        public NWDAppEnvironmentPlayerStatut sign { get; private set; }
        public bool isSignUpdate { get; private set; }
        public string uuid { get; private set; }
        public string signkey { get; private set; }
        public bool isNewUser { get; private set; }
        public bool isUserTransfert { get; private set; }
        public bool isReloadingData { get; private set; }

        // TODO: remove and replace by properties exhaustives (with enum)
        public Dictionary<string, object> param { get; private set; }

        public NWDOperationResult()
        {
            Init();
        }

        public NWDOperationResult(Dictionary<string, object> data)
        {
            Init();

			if (data.ContainsKey("timestamp"))
            {
				timestamp = int.Parse(data["timestamp"].ToString());
            }
			if (data.ContainsKey("perform"))
            {
				perform = float.Parse(data["perform"].ToString());
            }
			if (data.ContainsKey("token"))
            {
				token = data["token"] as string;
            }
            if (data.ContainsKey("sign"))
            {
                isSignUpdate = true;
                try
                {
                    sign = (NWDAppEnvironmentPlayerStatut)Enum.Parse(typeof(NWDAppEnvironmentPlayerStatut), data["sign"].ToString(), true);
                }
                catch (ArgumentException e)
                {
                    BTBDebug.Log(e.StackTrace);
                }
            }
            if (data.ContainsKey("signkey"))
            {
                signkey = data["signkey"] as string;
            }
            if (data.ContainsKey("error"))
            {
                isError = (bool)data["error"];
            }
            if (data.ContainsKey("newuser"))
            {
                isNewUser = (bool)data["newuser"];
            }
            if (data.ContainsKey("usertransfert"))
            {
                isUserTransfert = (bool)data["usertransfert"];
            }
            if (data.ContainsKey("reloaddatas"))
            {
                isReloadingData = (bool)data["reloaddatas"];
            }
            if (data.ContainsKey("error_code"))
            {
				errorCode = data["error_code"] as string;
            }
            if (data.ContainsKey("uuid"))
            {
                uuid = data["uuid"] as string;
            }

            if(isError)
            {
                errorDesc = BTBErrorManager.ShareInstance().FindError(errorCode);
            }

			param = new Dictionary<string, object>(data);
        }

        public NWDOperationResult(string code)
        {
            Init();

            errorDesc = BTBErrorManager.ShareInstance().FindError(code);
            isError = true;
            errorCode = code;
        }

        private void Init()
        {
            timestamp = 0;
            perform = 0.0f;
            isError = false;
            errorCode = "";
            token = "";
            signkey = "";
            sign = NWDAppEnvironmentPlayerStatut.Unknow;
            isSignUpdate = false;
            isNewUser = false;
            isUserTransfert = false;
            isReloadingData = false;
            uuid = "";

            param = new Dictionary<string, object>();
        }
    }
}
