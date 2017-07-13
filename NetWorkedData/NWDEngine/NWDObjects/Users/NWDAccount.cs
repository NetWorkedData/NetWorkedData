using System;

using UnityEngine;
using UnityEngine.UI;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (false)]
	[NWDClassTrigrammeAttribute ("ACC")]
	[NWDClassDescriptionAttribute ("Account descriptions Class")]
	[NWDClassMenuNameAttribute ("Account")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDAccount : NWDBasis <NWDAccount>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }

		/// <summary>
		/// Gets or sets the NickName.
		/// </summary>
		/// <value>The nickname.</value>
		public string NickName { get; set; }

		/// <summary>
		/// Gets or sets the Avatar Data PNG.
		/// </summary>
		/// <value>The Avatar Data PNG (limit on server).</value>
		public string Avatar { get; set; }

		/// <summary>
		/// Gets or sets the SecretKey to restaure anonymous account.
		/// </summary>
		/// <value>The login.</value>
		public string SecretKey { get; set; }

		/// <summary>
		/// Gets or sets the email.
		/// </summary>
		/// <value>The login is an email.</value>
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		public string Password { get; set; }

		/// <summary>
		/// Gets or sets the apple notification token for message.
		/// </summary>
		/// <value>The apple notification token.</value>
		public string AppleNotificationToken { get; set; }

		/// <summary>
		/// Gets or sets the google notification token for message.
		/// </summary>
		/// <value>The google notification token.</value>
		public string GoogleNotificationToken { get; set; }

		/// <summary>
		/// Gets or sets the Facebook Identifiant.
		/// </summary>
		/// <value>The facebook I.</value>
		public string FacebookID { get; set; }

		/// <summary>
		/// Gets or sets the Google Identifiant.
		/// </summary>
		/// <value>The google I.</value>
		public string GoogleID { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether this account <see cref="NWDEditor.NWDAccount"/> is banned.
		/// </summary>
		/// <value><c>true</c> if ban; otherwise, <c>false</c>.</value>
		public int Ban { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDAccount ()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		private string kInternalLogin;
		private string kInternalPassword;
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditor (Rect sInRect)
		{
			//Debug.Log ("AddonEditor");
			float tWidth = sInRect.width;
			float tX = sInRect.x;
			float tY = sInRect.y;

			GUIStyle tTextFieldStyle = new GUIStyle (EditorStyles.textField);
			tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight (new GUIContent ("A"), tWidth);

			GUIStyle tMiniButtonStyle = new GUIStyle (EditorStyles.miniButton);
			tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight (new GUIContent ("A"), tWidth);

			GUIStyle tLabelStyle = new GUIStyle (EditorStyles.boldLabel);
			tLabelStyle.fixedHeight = tLabelStyle.CalcHeight (new GUIContent ("A"), tWidth);


			EditorGUI.DrawRect (new Rect (tX, tY + NWDConstants.kFieldMarge, tWidth, 1), kRowColorLine);
			tY += NWDConstants.kFieldMarge * 2;

			EditorGUI.LabelField (new Rect (tX, tY, tWidth, tTextFieldStyle.fixedHeight), "Tools box", tLabelStyle);
			tY += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;

			kInternalLogin = EditorGUI.TextField (new Rect (tX, tY, tWidth, tTextFieldStyle.fixedHeight), "Email to hash", kInternalLogin, tTextFieldStyle);
			tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
			kInternalPassword = EditorGUI.TextField (new Rect (tX, tY, tWidth, tTextFieldStyle.fixedHeight), "Password to hash", kInternalPassword, tTextFieldStyle);
			tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

			float tWidthTiers = (tWidth - NWDConstants.kFieldMarge * 1) / 2.0f;

			EditorGUI.BeginDisabledGroup (kInternalLogin == "" || kInternalPassword == "" || kInternalLogin == null || kInternalPassword == null);

			if (GUI.Button (new Rect (tX, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "SignUp dev", tMiniButtonStyle)) {
				NWDAppEnvironment tEnvironmentDev = NWDAppConfiguration.SharedInstance.DevEnvironment;
				this.Email = BTBSecurityTools.GenerateSha (kInternalLogin + tEnvironmentDev.SaltStart, BTBSecurityShaTypeEnum.Sha1);
				this.Password = BTBSecurityTools.GenerateSha(kInternalPassword + tEnvironmentDev.SaltEnd, BTBSecurityShaTypeEnum.Sha1);
				this.InternalDescription = "Account for Dev test (" + kInternalLogin + " / " + kInternalPassword + ")";
				kInternalLogin = "";
				kInternalPassword = "";
				this.UpdateMeIfModified ();
			}

			if (GUI.Button (new Rect (tX + tWidthTiers + NWDConstants.kFieldMarge, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "SignUp preprod", tMiniButtonStyle)) {
				NWDAppEnvironment tEnvironmentPreprod = NWDAppConfiguration.SharedInstance.PreprodEnvironment;
				this.Email = BTBSecurityTools.GenerateSha(kInternalLogin + tEnvironmentPreprod.SaltStart, BTBSecurityShaTypeEnum.Sha1);
				this.Password = BTBSecurityTools.GenerateSha(kInternalPassword + tEnvironmentPreprod.SaltEnd, BTBSecurityShaTypeEnum.Sha1);
				this.InternalDescription = "Account for Preprod test (" + kInternalLogin + " / " + kInternalPassword + ")";
				kInternalLogin = "";
				kInternalPassword = "";
				this.UpdateMeIfModified ();
			}

			//			if (GUI.Button (new Rect (tX+(tWidthTiers+NWDConstants.kFieldMarge)*2, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "SignUp prod", tMiniButtonStyle)) {
//				NWDAppEnvironment tEnvironmentProd = NWDAppConfiguration.SharedInstance.ProdEnvironment;
//				this.Email = BTBSecurityTools.generateSha (kInternalLogin + tEnvironmentProd.SaltStart, BTBSecurityShaTypeEnum.Sha1);
//				this.Password = BTBSecurityTools.generateSha (kInternalPassword + tEnvironmentProd.SaltEnd, BTBSecurityShaTypeEnum.Sha1);
//				this.InternalDescription = "Account for Prod test (" + kInternalLogin + " / " + kInternalPassword + ")";
//				kInternalLogin = "";
//				kInternalPassword = "";
//				this.UpdateMeIfModified();
//			}

			tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
			EditorGUI.EndDisabledGroup ();


			EditorGUI.BeginDisabledGroup (kInternalLogin == "" || kInternalLogin == null);

			if (GUI.Button (new Rect (tX, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "Rescue dev", tMiniButtonStyle)) {
				NWDAppEnvironment tEnvironmentDev = NWDAppConfiguration.SharedInstance.DevEnvironment;
				NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create ("Editor Account Rescue", null, null, null, null, tEnvironmentDev);
				sOperation.Action = "rescue";
				sOperation.EmailRescue = kInternalLogin;
				NWDDataManager.SharedInstance.WebOperationQueue.AddOperation (sOperation, true);
			}

			if (GUI.Button (new Rect (tX + tWidthTiers + NWDConstants.kFieldMarge, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "SignUp preprod", tMiniButtonStyle)) {
				NWDAppEnvironment tEnvironmentPreprod = NWDAppConfiguration.SharedInstance.PreprodEnvironment;
				NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create ("Editor Account Rescue", null, null, null, null, tEnvironmentPreprod);
				sOperation.Action = "rescue";
				sOperation.EmailRescue = kInternalLogin;
				NWDDataManager.SharedInstance.WebOperationQueue.AddOperation (sOperation, true);
			}

//			if (GUI.Button (new Rect (tX + (tWidthTiers + NWDConstants.kFieldMarge) * 2, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "SignUp prod", tMiniButtonStyle)) {
//				NWDAppEnvironment tEnvironmentProd = NWDAppConfiguration.SharedInstance.ProdEnvironment;
//				NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create ("Editor Account Rescue", null, null, null, null, tEnvironmentProd);
//				sOperation.Action = "rescue";
//				sOperation.Email = Email;
//				NWDDataManager.SharedInstance.WebOperationQueue.AddOperation (sOperation, true);
//			}

			tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
			EditorGUI.EndDisabledGroup ();

			EditorGUI.BeginDisabledGroup (Email == "" || Password == "");
			if (GUI.Button (new Rect (tX, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "SignIn dev", tMiniButtonStyle)) {
				NWDAppEnvironment tEnvironmentDev = NWDAppConfiguration.SharedInstance.DevEnvironment;

				NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create ("Editor Account Sign-in", null, null, null, null, tEnvironmentDev);
				sOperation.Action = "signin";
				sOperation.EmailHash = Email;
				sOperation.PasswordHash = Password;
				NWDDataManager.SharedInstance.WebOperationQueue.AddOperation (sOperation, true);
			}

			if (GUI.Button (new Rect (tX + tWidthTiers + NWDConstants.kFieldMarge, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "SignIn preprod", tMiniButtonStyle)) {
				NWDAppEnvironment tEnvironmentPreprod = NWDAppConfiguration.SharedInstance.PreprodEnvironment;

				NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create ("Editor Account Sign-in", null, null, null, null, tEnvironmentPreprod);
				sOperation.Action = "signin";
				sOperation.EmailHash = Email;
				sOperation.PasswordHash = Password;
				NWDDataManager.SharedInstance.WebOperationQueue.AddOperation (sOperation, true);
			}

//			if (GUI.Button (new Rect (tX+(tWidthTiers+NWDConstants.kFieldMarge)*2, tY, tWidthTiers, tMiniButtonStyle.fixedHeight), "SignIn prod", tMiniButtonStyle)) {
//				NWDAppEnvironment tEnvironmentProd = NWDAppConfiguration.SharedInstance.ProdEnvironment;
//
//				NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create ("Editor Account Sign-in", null, null, null, null, tEnvironmentProd);
//				sOperation.Action = "signin";
//				sOperation.EmailHash = Email;
//				sOperation.PasswordHash = Password;
//				NWDDataManager.SharedInstance.WebOperationQueue.AddOperation (sOperation, true);
			//			}
			tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
			EditorGUI.EndDisabledGroup ();
			tY += NWDConstants.kFieldMarge;
			return tY;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override float AddonEditorHeight ()
		{
			//Debug.Log ("AddonEditorHeight");
			GUIStyle tTextFieldStyle = new GUIStyle (EditorStyles.textField);
			tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight (new GUIContent ("A"), 100);

			GUIStyle tMiniButtonStyle = new GUIStyle (EditorStyles.miniButton);
			tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight (new GUIContent ("A"), 100);

			GUIStyle tLabelStyle = new GUIStyle (EditorStyles.label);
			tLabelStyle.fixedHeight = tLabelStyle.CalcHeight (new GUIContent ("A"), 100);

			float tY = NWDConstants.kFieldMarge;

			tY += NWDConstants.kFieldMarge * 2;
			tY += tLabelStyle.fixedHeight + NWDConstants.kFieldMarge;
			tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
			tY += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;
			tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
			tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
			tY += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
			tY += NWDConstants.kFieldMarge;

			return tY;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================