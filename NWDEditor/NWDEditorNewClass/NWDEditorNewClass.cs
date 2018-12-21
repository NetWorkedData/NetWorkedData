//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using System;
using System.Reflection;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	/// <summary>
	/// NWD editor new class. Can create a new classes based on NWDExample automatically from the form generated in this editor window.
	/// </summary>
	public class NWDEditorNewClass : EditorWindow
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The futur name of the class.
		/// </summary>
		string ClassName = string.Empty;
		/// <summary>
		/// The futur trigramme of the class.
		/// </summary>
		string ClassNameTrigramme = string.Empty;
		/// <summary>
		/// The futur class description.
		/// </summary>
		string ClassNameDescription = string.Empty;
        /// <summary>
        /// The futur menu name use for this class.
        /// </summary>
        string ClassNameMenuName = string.Empty;
		/// <summary>
		/// The class name properties list.
		/// </summary>
		List<KeyValuePair<string,string>> ClassNameProperties = new List<KeyValuePair<string,string>> ();
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Generate the new class. It's not Magic, it's Sciences! (and a little bit of magic :-p )
		/// </summary>
		public void GenerateNewClass ()
		{
			GUI.FocusControl (null);
			// get the NWDExample code source
			string tClassExamplePath = NWDFindPackage.PathOfPackage () + "/NWDEngine/NWDObjects/NWDExample/NWDExample.cs";
			string tClassExample = File.ReadAllText (tClassExamplePath);
			// replace template by this params
			tClassExample = tClassExample.Replace ("NWDExample_Tri", ClassNameTrigramme);
			tClassExample = tClassExample.Replace ("NWDExample_Description", ClassNameDescription);
			tClassExample = tClassExample.Replace ("NWDExample_MenuName", ClassNameMenuName);
			tClassExample = tClassExample.Replace ("//#warning", "#warning");
			tClassExample = tClassExample.Replace ("NWDExample", ClassName);
			// prepare properties 
			Dictionary<string,string> tPropertiesDico = new Dictionary<string,string> ();
			foreach (KeyValuePair<string,string> tKeyValue in ClassNameProperties) {
				if (tKeyValue.Key != string.Empty && tKeyValue.Value != " " && tKeyValue.Value != string.Empty) {
					if (tPropertiesDico.ContainsKey (tKeyValue.Key) == false) {
						string tTypeString = tKeyValue.Value;
						if (tTypeString.Contains ("<K>/")) {
							tTypeString = tTypeString.Replace ("<K>/", "<") + ">";
						}
						tPropertiesDico.Add (tKeyValue.Key, tTypeString);
					}
				}
			}
			// place the properties
			string tPropertiesLinearize = "//PROPERTIES\n";
			foreach (KeyValuePair<string,string> tKeyValue in tPropertiesDico) {
				tPropertiesLinearize += "\t\tpublic " + tKeyValue.Value + " " + tKeyValue.Key + " {get; set;}\n";
			}
			tClassExample = tClassExample.Replace ("//PROPERTIES", tPropertiesLinearize);
            // find the owner classes folder
            string tOwnerClassesFolderPath = NWDToolbox.FindOwnerClassesFolder();
			// write file
			string tFilePath = tOwnerClassesFolderPath + "/" + ClassName + ".cs";
			File.WriteAllText (tFilePath, tClassExample);
			// flush params
			ClassName = string.Empty;
			ClassNameTrigramme = string.Empty;
			ClassNameDescription = string.Empty;
			ClassNameMenuName = string.Empty;
			ClassNameProperties = new List<KeyValuePair<string,string>> ();
			// import new script
			AssetDatabase.ImportAsset (tFilePath);
			// TODO: not working ... must be fix or remove
//			GameObject tScript = AssetDatabase.LoadAssetAtPath<GameObject> (tFilePath);
//			Selection.activeObject = tScript;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the enable event.
		/// </summary>
		public void OnEnable ()
		{
			titleContent = new GUIContent ("New NWDBasis Class generator");
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the OnGUI event. Create the interface to enter a new class.
		/// </summary>
		public void OnGUI ()
		{

			titleContent = new GUIContent ("New NWDBasis Class generator");
			//Prepare the form varaible 
			Regex tRegExpression = new Regex ("[^a-zA-Z]");
			Regex tRegExpressionProperties = new Regex ("[^a-zA-Z0-9]");
			Regex tRegExpressionEmptyType = new Regex ("[ ]+");
			// validate the form ?
			bool tCanCreate = true;
			// start Layout
			EditorGUILayout.LabelField ("Easy NWDBasis Class Generator", EditorStyles.boldLabel);
			EditorGUILayout.HelpBox ("Helper to create a new NWDBasis herited class. NWDBasis is the class of data in NetWorkedData framework.", MessageType.Info);
			// futur class infos
			EditorGUILayout.LabelField ("Class informations", EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			ClassName = EditorGUILayout.TextField ("Name ", ClassName);
			ClassName = tRegExpression.Replace (ClassName, string.Empty);
			if (ClassName.Length < 3) {
				EditorGUILayout.LabelField (" ", "name must be longer than 3 characters");
				tCanCreate = false;
			} else {
//				TODO: find if Type exists for generic
				foreach (Type tType in NWDDataManager.SharedInstance().mTypeList) {
					if (tType.Name == ClassName) {
						tCanCreate = false;
					}
				}
				if (tCanCreate == false) {
					EditorGUILayout.LabelField (" ", "this class allready exists");
				} else {
					EditorGUILayout.LabelField (" ", "class name is Ok!");
				}
			}
			ClassNameTrigramme = EditorGUILayout.TextField ("Trigramme", ClassNameTrigramme);
			ClassNameTrigramme = tRegExpression.Replace (ClassNameTrigramme, string.Empty);
			ClassNameTrigramme = ClassNameTrigramme.ToUpper ();
			if (ClassNameTrigramme.Length < 2) {
				EditorGUILayout.LabelField (" ", "trigramme must be longer than 1 characters");
				tCanCreate = false;
			} else if (ClassNameTrigramme.Length > 5) {
				EditorGUILayout.LabelField (" ", "trigramme must be shorter than 5 characters");
				tCanCreate = false;
			} else {
				//  but Trigramme already exists ?
				if (NWDDataManager.SharedInstance().mTrigramTypeDictionary.ContainsKey (ClassNameTrigramme)) {
					tCanCreate = false;
					EditorGUILayout.LabelField (" ", "trigramme allready used by '" + NWDDataManager.SharedInstance().mTrigramTypeDictionary [ClassNameTrigramme].Name + "'!");
				} else {
					EditorGUILayout.LabelField (" ", "trigramme is Ok!");
				}
			}
			EditorGUI.indentLevel--;
			// futur class description
			EditorGUILayout.LabelField ("Class description", EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			ClassNameDescription = EditorGUILayout.TextField ("Description", ClassNameDescription);
			ClassNameDescription = ClassNameDescription.Replace ("\\", string.Empty);
			EditorGUI.indentLevel--;
			EditorGUILayout.LabelField ("Menu in interface", EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			// futur class menu name
			ClassNameMenuName = EditorGUILayout.TextField ("Menu name", ClassNameMenuName);
			ClassNameMenuName = ClassNameMenuName.Replace ("\\", string.Empty);
			if (ClassNameMenuName.Length < 3) {
				EditorGUILayout.LabelField (" ", "menu name must be longer than 2 characters");
				tCanCreate = false;
			} else if (ClassNameMenuName.Length > 16) {
				EditorGUILayout.LabelField (" ", "menu name must be shorter than 16 characters");
				tCanCreate = false;
			} else {
				EditorGUILayout.LabelField (" ", "menu name is Ok!");
			}
			EditorGUI.indentLevel--;
			// the futur properties
			EditorGUILayout.LabelField ("Properties", EditorStyles.boldLabel);
			EditorGUI.indentLevel++;
			// create properties type
			List<string> tListOfType = new List<string> ();
			tListOfType.Add (" ");
			tListOfType.Add ("string");
			tListOfType.Add ("bool");
			tListOfType.Add ("int");
			tListOfType.Add ("float");
			tListOfType.Add ("  "); // use as separator remove by ereg
			tListOfType.Add ("NWDColorType");
			tListOfType.Add ("NWDGameObjectType");
			tListOfType.Add ("NWDPrefabType");
			tListOfType.Add ("NWDSpriteType");
			tListOfType.Add ("NWDTextureType");
			tListOfType.Add ("   "); // use as separator remove by ereg
			tListOfType.Add ("NWDLocalizableStringType");
			tListOfType.Add ("NWDLocalizableTextType");
			tListOfType.Add ("NWDLocalizablePrefabType");
			tListOfType.Add ("    "); // use as separator remove by ereg
			tListOfType.Add ("NWDDateTimeType");
			tListOfType.Add ("NWDDateType");
			tListOfType.Add ("NWDTimeType");
			tListOfType.Add ("     "); // use as separator remove by ereg
			tListOfType.Add ("NWDDateTimeRangeType");
			tListOfType.Add ("NWDTimeRangeType");
			tListOfType.Add ("      "); // use as separator remove by ereg
			tListOfType.Add ("NWDDateScheduleType");
			tListOfType.Add ("NWDDaysOfWeekScheduleType");
			tListOfType.Add ("NWDDaysScheduleType");
			tListOfType.Add ("NWDMonthsScheduleType");
			tListOfType.Add ("NWDHoursScheduleType");
			tListOfType.Add ("NWDMinutesScheduleType");
			tListOfType.Add ("       "); // use as separator remove by ereg
			tListOfType.Add ("NWDGeolocType");
			tListOfType.Add ("        "); // use as separator remove by ereg
            tListOfType.Add ("NWDJsonType");
            tListOfType.Add("          "); // use as separator remove by ereg
            tListOfType.Add("NWDVersionType");
            tListOfType.Add("           "); // use as separator remove by ereg
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeList) {
                tListOfType.Add("NWDReferenceType<K>/" + tType.Name);
                tListOfType.Add("NWDReferencesArrayType<K>/" + tType.Name);
				tListOfType.Add ("NWDReferencesListType<K>/"+tType.Name);
                tListOfType.Add ("NWDReferencesQuantityType<K>/"+tType.Name);
                tListOfType.Add("NWDReferencesAmountType<K>/" + tType.Name);
                tListOfType.Add("NWDReferencesAverageType<K>/" + tType.Name);
                tListOfType.Add("NWDReferencesConditionalType<K>/" + tType.Name);
                tListOfType.Add("NWDReferencesRangeType<K>/" + tType.Name);
			}
			// prepare result properties
			List<KeyValuePair<string,string>> tNextClassNameProperties = new List<KeyValuePair<string,string>> ();
			int tCounter = 0;
			foreach (KeyValuePair<string,string> tKeyValue in ClassNameProperties) {
				tCounter++;
				GUILayout.BeginHorizontal ();
				int tIndex = tListOfType.IndexOf (tKeyValue.Value);
				if (tIndex < 0 || tIndex > tListOfType.Count) {
					tIndex = 0;
				}
				tIndex = EditorGUILayout.Popup ("Property " + tCounter, tIndex, tListOfType.ToArray ());
				string tPropertyType = tListOfType [tIndex];
				tPropertyType = tRegExpressionEmptyType.Replace (tPropertyType, " ");
				string tPropertyName = tKeyValue.Key;
				tPropertyName = EditorGUILayout.TextField (tPropertyName, GUILayout.MaxWidth (160));
				tPropertyName = tRegExpressionProperties.Replace (tPropertyName, string.Empty);
				if (tPropertyType != string.Empty || tPropertyName != string.Empty) {
					KeyValuePair<string,string> tEnter = new KeyValuePair<string,string> (tPropertyName, tPropertyType);
					tNextClassNameProperties.Add (tEnter);
				}
				GUILayout.EndHorizontal ();
			}
			// add New property
			GUILayout.BeginHorizontal ();
			int tNextIndex = 0;
			tNextIndex = EditorGUILayout.Popup ("New Property", tNextIndex, tListOfType.ToArray ());
			string tNextPropertyType = tListOfType [tNextIndex];
			tNextPropertyType = tRegExpressionEmptyType.Replace (tNextPropertyType, " ");
			string tNextPropertyName = string.Empty;
			tNextPropertyName = EditorGUILayout.TextField (tNextPropertyName, GUILayout.MaxWidth (160));
			tNextPropertyName = tRegExpressionProperties.Replace (tNextPropertyName, string.Empty);
			if (tNextPropertyType != string.Empty || tNextPropertyName != string.Empty) {
				KeyValuePair<string,string> tEnter = new KeyValuePair<string,string> (tNextPropertyName, tNextPropertyType);
				tNextClassNameProperties.Add (tEnter);
			}
			GUILayout.EndHorizontal ();
			EditorGUI.indentLevel--;
			// remove empty properties
			tNextClassNameProperties.RemoveAll (RemoveAllPredicate);
			// meorize new properties list
			ClassNameProperties = tNextClassNameProperties;
			// Generate Button
			EditorGUILayout.Space ();
			// if ok continue else disable
			EditorGUILayout.LabelField ("Generate", EditorStyles.boldLabel);
			EditorGUI.BeginDisabledGroup (!tCanCreate);
			if (GUILayout.Button ("generate class")) {
				// ok generate!
				GenerateNewClass ();
			}
			EditorGUI.EndDisabledGroup ();
			EditorGUILayout.Space ();
			// calculate the good dimension for window
			if (Event.current.type == EventType.Repaint)
			{
				Rect tRect = GUILayoutUtility.GetLastRect ();
				maxSize = new Vector2 (600,tRect.height + tRect.y);
				minSize = new Vector2 (300,tRect.height + tRect.y);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Removes all predicate for the empty properties value key at the end of GUI.
		/// </summary>
		/// <returns><c>true</c>, if all predicate was removed, <c>false</c> otherwise.</returns>
		/// <param name="tObject">T object.</param>
		bool RemoveAllPredicate (KeyValuePair<string,string> tObject)
		{
			bool tReturn = false;
			if (tObject.Key == string.Empty && tObject.Value == " ") {
				tReturn = true; 
			}
			return tReturn;
		}

		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
