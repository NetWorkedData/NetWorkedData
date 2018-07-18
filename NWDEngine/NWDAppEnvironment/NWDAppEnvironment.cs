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
		public string AnonymousPlayerAccountReference = "";
		// reccord the first anonymous value to restaure old original account
		public string AnonymousResetPassword = "";
		// reccord the secretKey to reset token
		//-------------------------------------------------------------------------------------------------------------
		public string DataSHAPassword = "";
		public string DataSHAVector = "";
		public string SaltStart = "";
        public string SaltEnd = "";
#if UNITY_EDITOR
        public string SaltServer = "";
        public string MailHost = "";
        public int MailPort = 465;
        public string MailUserName = "";
        public string MailPassword = "";
        public string MailDomain = "";
        public string MailAuthentication = "plain";
        public string MailEnableStarttlsAuto = "true";
        public string MailOpenSSLVerifyMode = "peer";
        public string MailFrom = "";
        public string MailReplyTo = "";
        public string RescueEmail = "no-reply@my-web-site.com";
        public string ServerHost = "localhost";
        public string ServerUser = "user";
        public string ServerPassword = "";
        public string ServerBase = "myDatabase";
        public string AdminKey = "";
#endif
        public int SaltFrequency = 300;
        public string AddressPing = "8.8.8.8";
		public string ServerHTTPS = "https://www.my-web-site.com/";
		public string FacebookAppID = "";
		public string FacebookAppSecret = "";
		public string GoogleAppKey = "";
		public string UnityAppKey = "";
		public string TwitterAppKey = "";
		public int BuildTimestamp = 0;
		public int TokenHistoric = 6;
		public string AppName = "MyGameApp";
        public string PreProdTimeFormat = "_yyyy_MM_dd_HHmmss";
        public string AppProtocol = "MyGameApp://";
        public int WebTimeOut = 30;

        public float SpeedOfGameTime = 1.0F;
        //public string Version = "0.00.00";

        public string AccountsForTests = "";

        public string BuildDate = "";

        public Color CartridgeColor = new Color(1.0F,1.0F,1.0F);

        // TODO : check if working
        public bool ThreadPoolSQLActive = true;
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		#region constructor
		//-------------------------------------------------------------------------------------------------------------
		public NWDAppEnvironment ()
		{
			PlayerAccountReference = NWDToolbox.GenerateUniqueID ();
			FormatVerification ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDAppEnvironment (string sEnvironement, bool sSelected)
		{
			this.Environment = sEnvironement;
			this.Selected = sSelected;
			FormatVerification ();
		}
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAppEnvironment SelectedEnvironment()
        {
           return NWDAppConfiguration.SharedInstance().SelectedEnvironment();
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        #region instance methods
		//-------------------------------------------------------------------------------------------------------------
		public void AnonymousVerification ()
		{
			if (AnonymousPlayerAccountReference == "") {
				AnonymousPlayerAccountReference = NWDToolbox.GenerateUniqueID ();
			}
			if (AnonymousResetPassword == "") {
				AnonymousResetPassword = NWDToolbox.RandomStringUnix (36);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public void FormatVerification ()
		{
			// Debug.Log ("VerifySecurity");
			// clean the salts
			DataSHAPassword = NWDToolbox.SaltCleaner (DataSHAPassword);
			DataSHAVector = NWDToolbox.SaltCleaner (DataSHAVector);
			SaltStart = NWDToolbox.SaltCleaner (SaltStart);
            SaltEnd = NWDToolbox.SaltCleaner (SaltEnd);

#if UNITY_EDITOR
            SaltServer = NWDToolbox.SaltCleaner(SaltServer);
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
            if (SaltServer == "")
            {
                SaltServer = NWDToolbox.RandomString(16);
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
			if (TokenHistoric < 1 || TokenHistoric > 10) {
				TokenHistoric = 3;
            }
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Absolutes the date time in game time. With additional year.
        /// </summary>
        /// <returns>The date time in game time.</returns>
        public DateTime AbsoluteDateTimeInGameTime()
        {
            float tSpeedOfGameTime = SpeedOfGameTime;
            DateTime tNow = DateTime.Now;
            DateTime tNowTwo = new DateTime(tNow.Year,1,1, 0, 0, 0, DateTimeKind.Utc);
            float tTimestamp = (float)tNow.Subtract(tNowTwo).TotalSeconds;
            if (tSpeedOfGameTime > 0 && tSpeedOfGameTime < 1000)
            {
                tTimestamp = tTimestamp * tSpeedOfGameTime;
            }
            DateTime tDateInGame = new DateTime(tNow.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            tDateInGame = tDateInGame.AddSeconds(tTimestamp);
             return tDateInGame;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Date the time in game time. Without additional year (loop in the same year… paradoxal seasons)
        /// </summary>
        /// <returns>The time in game time.</returns>
        public DateTime DateTimeInGameTime()
        {
            float tSpeedOfGameTime = SpeedOfGameTime;
            DateTime tNow = DateTime.Now;
            DateTime tNowTwo = new DateTime(tNow.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            float tTimestamp = (float)tNow.Subtract(tNowTwo).TotalSeconds;
            if (tSpeedOfGameTime > 0 && tSpeedOfGameTime < 1000)
            {
                tTimestamp = tTimestamp * tSpeedOfGameTime;
            }
            DateTime tDateInGame = new DateTime(tNow.Year, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            tDateInGame = tDateInGame.AddSeconds(tTimestamp);
            DateTime tDateInGameResult = new DateTime(tNow.Year, tDateInGame.Month, tDateInGame.Day, tDateInGame.Hour, tDateInGame.Minute, tDateInGame.Second, DateTimeKind.Utc);
            return tDateInGameResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static float kSunRotationPerSeconds = 0.0014666667F; // ((360 /24)/60)/60 : 360° div by 24 hours, div by 60 minutes, div by 60 seconds : angle for one second
        //-------------------------------------------------------------------------------------------------------------
        public float RotationOfSunInGameTime()
        {
            //TODO Test this solution 
            DateTime tNow = DateTime.Now;
            int tSeconds = tNow.Hour*3600 + tNow.Minute*60 + tNow.Second;
            return kSunRotationPerSeconds * tSeconds * SpeedOfGameTime;
        }
		//-------------------------------------------------------------------------------------------------------------
		#endregion
	}
}
//=====================================================================================================================