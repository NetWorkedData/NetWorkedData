//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
	public partial class NWDAppEnvironment
	{
		#region properties
		//-------------------------------------------------------------------------------------------------------------
		public bool Selected = false;
		public string Environment = NWDConstants.K_PRODUCTION_NAME;
		//-------------------------------------------------------------------------------------------------------------
		public NWDAppEnvironmentPlayerStatut PlayerStatut = NWDAppEnvironmentPlayerStatut.Temporary;
		public string PlayerAccountReference = "";
		public string RequesToken = "";
		//-------------------------------------------------------------------------------------------------------------
		public string AnonymousPlayerAccountReference = ""; // reccord the first anonymous value to restaure old original account
//		public string AnonymousRequesToken = ""; // reccord the last anonymous value to restaure original account
		public string AnonymousResetPassword = ""; // reccord the secretKey to reset token
		//-------------------------------------------------------------------------------------------------------------
		public string DataSHAPassword = "";
		public string DataSHAVector = "";
		public string SaltStart = "";
		public string SaltEnd = "";
		public int SaltFrequency = 300;
		public string ServerHTTPS = "https://www.my-web-site.com/";
		public string ServerHost = "localhost";
		public string ServerUser = "user";
		public string ServerPassword = "";
		public string ServerBase = "myDatabase";
		public string AdminKey = "";
		public string FacebookAppID = "";
		public string FacebookAppSecret = "";
		public string GoogleAppKey = "";
		public string UnityAppKey = "";
		public string TwitterAppKey = "";
		public int BuildTimestamp = 0;
		public int TokenHistoric = 6;
		public string AppName = "MyGameApp";
		public string RescueEmail = "no-reply@my-web-site.com";
//		public string Version = "0.00.00";
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		#region constructor
		//-------------------------------------------------------------------------------------------------------------
		public NWDAppEnvironment () {
			PlayerAccountReference = NWDToolbox.GenerateUniqueID ();
			FormatVerification ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDAppEnvironment (string sEnvironement, bool sSelected) {
			this.Environment = sEnvironement;
			this.Selected = sSelected;
			FormatVerification ();
		}
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		#region instance methods
		//-------------------------------------------------------------------------------------------------------------
		public void AnonymousVerification () {
			if (AnonymousPlayerAccountReference == "") {
				AnonymousPlayerAccountReference = NWDToolbox.GenerateUniqueID ();
			}
			if (AnonymousResetPassword == "") {
				AnonymousResetPassword = NWDToolbox.RandomStringUnix (36);
			}
	}
		//-------------------------------------------------------------------------------------------------------------
		public void FormatVerification () {
            // BTBDebug.Log ("VerifySecurity");
            // clean the salts
            DataSHAPassword = NWDToolbox.SaltCleaner (DataSHAPassword);
			DataSHAVector = NWDToolbox.SaltCleaner (DataSHAVector);
			SaltStart = NWDToolbox.SaltCleaner (SaltStart);
			SaltEnd = NWDToolbox.SaltCleaner (SaltEnd);
			// ServerPassword = NWDToolbox.SaltCleaner (ServerPassword);
			AdminKey = NWDToolbox.SaltCleaner (AdminKey);
			//check salts are not mull
			if (DataSHAPassword == "") {
				DataSHAPassword = NWDToolbox.RandomString (16);
			}
			if (DataSHAVector == "") {
				DataSHAVector = NWDToolbox.RandomString (16);
			}
			if (SaltStart == "") {
				SaltStart = NWDToolbox.RandomString (16);
			}
			if (SaltEnd == "") {
				SaltEnd = NWDToolbox.RandomString (16);
			}
			if (SaltFrequency <= 400) {
				SaltFrequency = UnityEngine.Random.Range (400, 800);
			}
			if (ServerPassword == "") {
				ServerPassword = NWDToolbox.RandomString (16);
			}
			if (AdminKey == "") {
				AdminKey = NWDToolbox.RandomString (16);
			}
			if (TokenHistoric <1 || TokenHistoric>10) {
				TokenHistoric = 3;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		#endregion
	}
}
//=====================================================================================================================