//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

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
	[NWDClassTrigrammeAttribute ("NWDExample_Tri")]
	[NWDClassDescriptionAttribute ("NWDExample_Description")]
	[NWDClassMenuNameAttribute ("NWDExample_MenuName")]
	//[NWDInternalKeyNotEditableAttribute]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDExample : NWDBasis<NWDExample>
	{
		//#warning YOU MUST FOLLOW THIS INSTRUCTIONS
		//-------------------------------------------------------------------------------------------------------------
		// YOU MUST GENERATE AUOT CSHARP FILE FOR THIS CLASS AFTER FIELD THIS CLASS WITH YOUR PROPERTIES
		// AFTER YOU CAN EASYLY USE THIS CLASS IN YOUR PROJECT
		//-------------------------------------------------------------------------------------------------------------
		// YOU MUST GENERATE PHP FOR THIS CLASS AFTER FIELD THIS CLASS WITH YOUR PROPERTIES
		// YOU MUST GENERATE WEBSITE AND UPLOAD THE FOLDER ON YOUR SERVER
		// YOU MUST UPDATE TABLE ON THE SERVER WITH THE MENU , FOR DEV, FOR PREPROD, FOR PROD
		//-------------------------------------------------------------------------------------------------------------
		//PROPERTIES
		//-------------------------------------------------------------------------------------------------------------
		public NWDExample()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void MyPrivateMethod ()
		{
			// do something with this object
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		//Addons for Edition
		//-------------------------------------------------------------------------------------------------------------
		public override bool AddonEdited( bool sNeedBeUpdate)
		{
			if (sNeedBeUpdate == true) 
			{
				// do something
			}
			return sNeedBeUpdate;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void Updated ()
		{
			// do something
		}
		//-------------------------------------------------------------------------------------------------------------
		public override float AddonEditor (Rect sInRect)
		{
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

			EditorGUI.DrawRect (new Rect (tX, tY+tYadd, tWidth, 1), NWDVersion.kRowColorLine);
			tYadd += NWDConstants.kFieldMarge;

			GUI.Label(new Rect (tX, tY+tYadd, tWidth, tTextFieldStyle.fixedHeight), "Environement selected to build", EditorStyles.boldLabel);
			tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

			EditorGUI.LabelField (new Rect (tX, tY+tYadd, tWidth, tTextFieldStyle.fixedHeight), "Environment", NWDAppConfiguration.SharedInstance.SelectedEnvironment ().Environment);
			tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

			EditorGUI.LabelField (new Rect (tX, tY+tYadd, tWidth, tTextFieldStyle.fixedHeight), "Version", PlayerSettings.bundleVersion);
			tYadd += tTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge;

			EditorGUI.DrawRect (new Rect (tX, tY+tYadd, tWidth, 1), NWDVersion.kRowColorLine);
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