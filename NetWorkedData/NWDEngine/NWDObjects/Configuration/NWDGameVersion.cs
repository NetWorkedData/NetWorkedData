using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

using SQLite4Unity3d;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("GVA")]
	[NWDClassDescriptionAttribute ("Game's Version App descriptions Class")]
	[NWDClassMenuNameAttribute ("Game's Version App")]
	[NWDInternalKeyNotEditableAttribute]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDGameVersion :NWDBasis<NWDGameVersion>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		public NWDVersionType Version { get; set; }
		[NWDSeparatorAttribute]
		public bool BuildActive { get; set; }
		[NWDSeparatorAttribute]
		public bool ActiveDev { get; set; }
		public bool ActivePreprod { get; set; }
		public bool ActiveProd { get; set; }
		[NWDSeparatorAttribute]
		public bool BlockDataUpdate { get; set; }
		public bool ForceApplicationUpdate { get; set; }
		public NWDLocalizableStringType AlertTitle { get; set; }
		public NWDLocalizableStringType AlertMessage { get; set; }
		public NWDLocalizableStringType AlertButtonOK { get; set; }
		public string AppleStoreURL { get; set; }
		public string GooglePlayURL { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDGameVersion()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public static string GetVersionForEnvironemt (NWDAppEnvironment sEnvironment)
		{
			BTBDebug.LogVerbose ("GetVersionForEnvironemt");
			// I will change th last version of my App
			string tVersionString = "0.00.00";
			int tVersionInt = 0;
			int.TryParse (tVersionString.Replace(".",""), out tVersionInt);
			foreach (NWDGameVersion tGameVersionObject in NWDGameVersion.ObjectsList)
			{
				if (tGameVersionObject.TestIntegrity () == true  && tGameVersionObject.AC == true && tGameVersionObject.BuildActive == true) 
				{
					if ((NWDAppConfiguration.SharedInstance.DevEnvironment == sEnvironment && tGameVersionObject.ActiveDev == true) || 
						(NWDAppConfiguration.SharedInstance.PreprodEnvironment == sEnvironment && tGameVersionObject.ActivePreprod == true) ||
						(NWDAppConfiguration.SharedInstance.ProdEnvironment == sEnvironment && tGameVersionObject.ActiveProd == true))
					{
						int tVersionInteger = 0;
						int.TryParse (tGameVersionObject.Version.ToString ().Replace (".", ""), out tVersionInteger);
						if (tVersionInt < tVersionInteger) {
							tVersionInt = tVersionInteger;
							tVersionString = tGameVersionObject.Version.ToString ();
						}
					}
				}
			}
			// sEnvironment.Version = tVersionString;
			return tVersionString;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void UpdateVersionBundle ()
		{
			if (NWDAppConfiguration.SharedInstance.IsDevEnvironement () == false &&
				NWDAppConfiguration.SharedInstance.IsPreprodEnvironement () == false &&
			    NWDAppConfiguration.SharedInstance.IsProdEnvironement () ==false
			) {
				// error no environnment selected 
				NWDAppConfiguration.SharedInstance.DevEnvironment.Selected = true;
			}
			// I will change th last version of my App
			string tVersionString = "0.00.00";
			int tVersionInt = 0;
			int.TryParse (tVersionString.Replace(".",""), out tVersionInt);
			NWDGameVersion tLastGameVersionObject = null;
			foreach (NWDGameVersion tGameVersionObject in NWDGameVersion.ObjectsList)
			{
				if (tGameVersionObject.TestIntegrity () == true  && tGameVersionObject.AC == true && tGameVersionObject.BuildActive == true) 
				{
					if ((NWDAppConfiguration.SharedInstance.IsDevEnvironement () && tGameVersionObject.ActiveDev == true) || 
						(NWDAppConfiguration.SharedInstance.IsPreprodEnvironement () && tGameVersionObject.ActivePreprod == true) ||
						(NWDAppConfiguration.SharedInstance.IsProdEnvironement () && tGameVersionObject.ActiveProd == true))
					{
						int tVersionInteger = 0;
						int.TryParse (tGameVersionObject.Version.ToString ().Replace (".", ""), out tVersionInteger);
						if (tVersionInt < tVersionInteger) {
							tVersionInt = tVersionInteger;
							tVersionString = tGameVersionObject.Version.ToString ();
							tLastGameVersionObject = tGameVersionObject;
						}
					}
				}
			}
			if (tLastGameVersionObject != null) {
				if (PlayerSettings.bundleVersion != tLastGameVersionObject.Version.ToString ()) {
					PlayerSettings.bundleVersion = tLastGameVersionObject.Version.ToString ();
				}
			}
			else
			{
				if (PlayerSettings.bundleVersion != tVersionString) {
					PlayerSettings.bundleVersion = tVersionString;
				}
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public override bool  AddonEdited( bool sNeedBeUpdate)
		{
			if (sNeedBeUpdate == true) 
			{
				this.InternalKey = this.Version.ToString ();
			}
			return sNeedBeUpdate;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void Updated ()
		{
			NWDGameVersion.UpdateVersionBundle ();
			NWDDataManager.SharedInstance.RepaintWindowsInManager (typeof(NWDGameVersion));
		}
		//-------------------------------------------------------------------------------------------------------------
		public override float AddonEditor (Rect sInRect)
		{
			// force update 
			NWDGameVersion.UpdateVersionBundle ();
			// show editor add-on
			float tWidth = sInRect.width - NWDConstants.kFieldMarge * 2;
			float tX = sInRect.position.x + NWDConstants.kFieldMarge;
			float tY = sInRect.position.y + NWDConstants.kFieldMarge;

			GUIStyle tTextFieldStyle = new GUIStyle (EditorStyles.textField);
			tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight (new GUIContent ("A"), tWidth);

			GUIStyle tMiniButtonStyle = new GUIStyle (EditorStyles.miniButton);
			tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight (new GUIContent ("A"), tWidth);

			float tYadd = 0.0f;
			// darw information about actual bundle 
			EditorGUI.BeginDisabledGroup (true);

			EditorGUI.DrawRect (new Rect (tX, tY+tYadd, tWidth, 1), NWDGameVersion.kRowColorLine);
			tYadd += NWDConstants.kFieldMarge;

			GUI.Label(new Rect (tX, tY+tYadd, tWidth, tTextFieldStyle.fixedHeight), "Environement selected to build", EditorStyles.boldLabel);
			tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

			EditorGUI.LabelField (new Rect (tX, tY+tYadd, tWidth, tTextFieldStyle.fixedHeight), "Environment", NWDAppConfiguration.SharedInstance.SelectedEnvironment ().Environment);
			tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

			EditorGUI.LabelField (new Rect (tX, tY+tYadd, tWidth, tTextFieldStyle.fixedHeight), "Version", PlayerSettings.bundleVersion);
			tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

			EditorGUI.DrawRect (new Rect (tX, tY+tYadd, tWidth, 1), NWDGameVersion.kRowColorLine);
			tYadd += NWDConstants.kFieldMarge;

			EditorGUI.EndDisabledGroup ();

			if (GUI.Button (new Rect (tX, tY + tYadd, tWidth, tMiniButtonStyle.fixedHeight), "Environment chooser",tMiniButtonStyle)) {
				NWDEditorMenu.EnvironementChooserShow ();
			}
			tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;

			tYadd += NWDConstants.kFieldMarge;

			return tYadd;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override float AddonEditorHeight ()
		{
			GUIStyle tTextFieldStyle = new GUIStyle (EditorStyles.textField);
			tTextFieldStyle.fixedHeight = tTextFieldStyle.CalcHeight (new GUIContent ("A"), 100);
			GUIStyle tMiniButtonStyle = new GUIStyle (EditorStyles.miniButton);
			tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight (new GUIContent ("A"), 100);

			float tYadd = 0.0f;

			tYadd += NWDConstants.kFieldMarge;

			tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

			tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

			tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

			tYadd += NWDConstants.kFieldMarge;

			tYadd += tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;

			tYadd += NWDConstants.kFieldMarge;

			return tYadd;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================