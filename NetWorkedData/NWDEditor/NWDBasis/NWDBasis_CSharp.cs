using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

using BasicToolBox;

using SQLite4Unity3d;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
	public partial class NWDBasis <K> where K : NWDBasis <K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
		public static void CreateCSharp ()
		{
			Type tType = ClassType ();
			TableMapping tTableMapping = new TableMapping (tType);
			string tClassName = tTableMapping.TableName;
			//string tTrigramme = ClassTrigramme ();
			DateTime tTime = DateTime.UtcNow;
			string tDateTimeString = tTime.ToString ("yyyy-MM-dd");
			string tYearString = tTime.ToString ("yyyy");

			string tEngineRootFolder = "Assets/NetWorkedDataWorkflow";

			if (AssetDatabase.IsValidFolder (tEngineRootFolder) == false) {
				AssetDatabase.CreateFolder ("Assets", "NetWorkedDataWorkflow");
				AssetDatabase.ImportAsset (tEngineRootFolder);
			}

			bool tAccountUsed = false;
			string tAccountConnected = "false";
			string tAccountPropertyName = "AccRef";
			string tLockedObject = "true";

			List<string> tAccountReferenceList = new List<string> ();
			List<string> tAccountAnalzeList = new List<string> ();
			List<string> tAccountAnalzeANDList = new List<string> ();
			foreach (var tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
				Type tTypeOfThis = tProp.PropertyType;
//				BTBDebug.LogVerbose ("tTypeOfThis = " + tTypeOfThis.Name);	
				if (tTypeOfThis != null) {
					//BTBDebug.LogVerbose ("tTypeOfThis not null = " + tTypeOfThis.Name + " for " + tProp.Name);
					if (tTypeOfThis.IsGenericType) {
						//BTBDebug.LogVerbose ("tTypeOfThis IsGenericTypeDefinition");
						if (tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferenceType<>)) {
							Type tSubType = tTypeOfThis.GetGenericArguments () [0];
							//BTBDebug.LogVerbose ("tTypeOfThis = " + tTypeOfThis.Name + " tSubType = " + tSubType.Name);
							if (tSubType == typeof(NWDAccount)) {
								//BTBDebug.LogVerbose ("tAccountAnalzeList ADDDDDDD");
								tAccountReferenceList.Add (tProp.Name);
								tAccountAnalzeANDList.Add ("tObject." + tProp.Name + ".Value == NWDAppConfiguration.SharedInstance.SelectedEnvironment().PlayerAccountReference");
								tAccountAnalzeList.Add ("" + tProp.Name + ".Value == NWDAppConfiguration.SharedInstance.SelectedEnvironment().PlayerAccountReference");
								tAccountConnected = "true";
								tAccountUsed = true;
								tLockedObject = "false";
								tAccountPropertyName = tProp.Name;
							}
						} 
					}
				}

			}
			string tAccountAnalzeListString = "";
			string tAccountAnalzeListANDString = "";
			if (tAccountAnalzeList.Count > 0) {
				tAccountAnalzeListString = "" + string.Join (" || ", tAccountAnalzeList.ToArray ()) + "";
				tAccountAnalzeListANDString = " && (" + string.Join (" || ", tAccountAnalzeANDList.ToArray ()) + ") ";
			}

			string tWorkFlowFile = "" +
			                       "//NWD Autogenerate File at " + tDateTimeString + "\n" +
			                       "//Copyright NetWorkedDatas ideMobi " + tYearString + "\n" +
			                       "//Created by Jean-François CONTART\n" +
			                       "//-------------------- \n";
			tWorkFlowFile += "using System;\n" +
			"using System.Collections;\n" +
			"using System.Collections.Generic;\n" +
			"using System.Linq;\n" +
			"using System.Reflection;\n" +
			"\n" +
			"using UnityEngine;\n" +
			"\n" +
			"#if UNITY_EDITOR\n" +
			"using UnityEditor;\n" +
			"using UnityEditorInternal;\n" +
			"#endif\n" +
			"\n" +
			"namespace NetWorkedData\n " +
			"{\n";

			// Add new struct defined
			tWorkFlowFile += "\t//-------------------- \n" +
			"\t// CONNEXION STRUCTURE METHODS\n" +
			"\t//-------------------- \n" +
			"\t[Serializable]\n" +
			"\tpublic struct " + tClassName + "Connexion\n" +
			"\t{\n" +
			"\t\t[SerializeField]\n" +
			"\t\tpublic string Reference;\n" +
			"\t\tpublic " + tClassName + " GetObject ()\n" +
			"\t\t{\n" +
			"\t\t\treturn " + tClassName + ".GetObjectWithReference (Reference);\n" +
			"\t\t}\n" +
			"\t\tpublic void SetObject (" + tClassName + " sObject)\n" +
			"\t\t{\n" +
			"\t\t\tReference = sObject.Reference;\n" +
			"\t\t}\n" +
			"\t\tpublic " + tClassName + " NewObject ()\n" +
			"\t\t{\n" +
			"\t\t\t" + tClassName + " tObject = " + tClassName + ".NewObject ();\n" +
			"\t\t\tReference = tObject.Reference;\n" +
			"\t\t\treturn tObject;\n" +
			"\t\t}\n" +
			"\t}\n";
			if (tAccountUsed == false) {
				tWorkFlowFile += "\t//-------------------- \n" +
				"\t// CONNEXION METHODS\n" +
				"\t//-------------------- \n" +
				"\tpublic class " + tClassName + "ConnexionAttribut : PropertyAttribute\n" +
				"\t{\n" +
				"\t\tpublic bool ShowInspector = true;\n" +
				"\t\tpublic bool Editable = false;\n" +
				"\t\tpublic bool EditButton = true;\n" +
				"\t\tpublic bool NewButton = true;\n" +
				"\t\tpublic " + tClassName + "ConnexionAttribut ()\n" +
				"\t\t{\n" +
				"\t\t}\n" +
				"\t\tpublic " + tClassName + "ConnexionAttribut (bool sShowInspector, bool sEditable, bool sEditButton, bool sNewButton)\n" +
				"\t\t{\n" +
				"\t\t\tShowInspector = sShowInspector;\n" +
				"\t\t\tEditable = sEditable;\n" +
				"\t\t\tEditButton = sEditButton;\n" +
				"\t\t\tNewButton = sNewButton;\n" +
				"\t\t}\n" +
				"\t}\n" +
				"\n";
				tWorkFlowFile += "\t//-------------------- \n" +
				"\t// CUSTOM PROPERTY DRAWER METHODS\n" +
				"\t//-------------------- \n" +
				"\t#if UNITY_EDITOR\n" +
				"\t[CustomPropertyDrawer (typeof(" + tClassName + "Connexion))]\n" +
				"\tpublic partial class " + tClassName + "ConnexionDrawer : PropertyDrawer\n" +
				"\t{\n" +
				"\t\tpublic override float GetPropertyHeight (SerializedProperty property, GUIContent label)\n" +
				"\t\t{\n" +
//				"\t\t\tfloat tHeight = 80.0f;\n" +
				"\t\t\t" + tClassName + "ConnexionAttribut tReferenceConnexion = new " + tClassName + "ConnexionAttribut ();\n" +
				"\t\t\tif (fieldInfo.GetCustomAttributes (typeof(" + tClassName + "ConnexionAttribut), true).Length > 0)\n" +
				"\t\t\t{\n" +
				"\t\t\t\ttReferenceConnexion = (" + tClassName + "ConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(" + tClassName + "ConnexionAttribut), true)[0];\n" +
				"\t\t\t}\n" +
//				"\t\t\tType tClassType = typeof(" + tClassName + ");\n" +
//				"\t\t\tvar tMethodInfo = tClassType.GetMethod (\"ReferenceConnexionHeight\", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);\n" +
//				"\t\t\tstring tTargetReference = property.FindPropertyRelative (\"Reference\").stringValue;\n" +
//				"\t\t\tif (tMethodInfo != null)\n" +
//				"\t\t\t\t{\n" +
//				"\t\t\t\t\tstring tHeightString = tMethodInfo.Invoke (null, new object[]\n" +
//				"\t\t\t\t\t\t{\n" +
//				"\t\t\t\t\t\t\ttTargetReference,\n" +
//				"\t\t\t\t\t\t\ttReferenceConnexion.ShowInspector\n" +
//				"\t\t\t\t\t\t}) as string;\n" +
//				"\t\t\t\t\tfloat.TryParse (tHeightString, out tHeight);\n" +
//				"\t\t\t\t}\n" +
//				"\t\t\treturn tHeight;\n" +
				"\t\t\treturn " + tClassName + ".ReferenceConnexionHeightSerialized(property, tReferenceConnexion.ShowInspector);\n" +
				"\t\t}\n" +
				"\t\tpublic override void OnGUI (Rect position, SerializedProperty property, GUIContent label)\n" +
				"\t\t{\n" +
				"\t\t\t" + tClassName + "ConnexionAttribut tReferenceConnexion = new " + tClassName + "ConnexionAttribut ();\n" +
				"\t\t\tif (fieldInfo.GetCustomAttributes (typeof(" + tClassName + "ConnexionAttribut), true).Length > 0)\n" +
				"\t\t\t\t{\n" +
				"\t\t\t\t\ttReferenceConnexion = (" + tClassName + "ConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(" + tClassName + "ConnexionAttribut), true)[0];\n" +
				"\t\t\t\t}\n" +
//				"\t\t\tType tClassType = typeof(" + tClassName + ");\n" +
//				"\t\t\tvar tMethodInfo = tClassType.GetMethod (\"ReferenceConnexionField\", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);\n" +
//				"\t\t\tstring tTargetReference = property.FindPropertyRelative (\"Reference\").stringValue;\n" +
//				"\t\t\tif (tMethodInfo != null)\n" +
//				"\t\t\t{\n" +
//				"\t\t\t\tstring tNewValue = tMethodInfo.Invoke (null,\n" +
//				"\t\t\t\tnew object[] {position,\n" +
//				"\t\t\t\t\tproperty.displayName,\n" +
//				"\t\t\t\t\ttTargetReference,\n" +
//				"\t\t\t\t\t\"\",\n" +
//				"\t\t\t\t\ttReferenceConnexion.ShowInspector,\n" +
//				"\t\t\t\t\ttReferenceConnexion.Editable,\n" +
//				"\t\t\t\t\ttReferenceConnexion.EditButton,\n" +
//				"\t\t\t\t\ttReferenceConnexion.NewButton\n" +
//				"\t\t\t\t\t}) as string;\n" +
//				"\t\t\t\tproperty.FindPropertyRelative (\"Reference\").stringValue = tNewValue;\n" +
//				"\t\t\t}\n" +
				"\t\t\t" + tClassName + ".ReferenceConnexionFieldSerialized (position, property.displayName, property, \"\", tReferenceConnexion.ShowInspector, tReferenceConnexion.Editable, tReferenceConnexion.EditButton, tReferenceConnexion.NewButton);\n" +
				"\t\t}\n" +
				"\t}\n" +
				"\t#endif\n";
			}
			tWorkFlowFile += "" +
			"//-------------------- \n" +
			"// GENERAL METHODS\n" +
			"//-------------------- \n" +
			"\tpublic partial class " + tClassName + " : NWDBasis <" + tClassName + ">\n" +
			"\t{\n" +
			"//-------------------- \n" +
			"\t\tpublic override bool IsAccountDependent ()\n" +
			"\t\t{\n" +
			"\t\t\treturn " + tAccountConnected + ";\n" +
			"\t\t}\n" +
			"//-------------------- \n" +
			"\t\tpublic override bool IsAccountConnected (string sAccountReference)\n" +
			"\t\t{\n";
			if (tAccountUsed == true) {
				tWorkFlowFile += "" +
				"\t\t\tbool rReturn = false;\n" +
				"\t\t\tif (" + tAccountAnalzeListString + ") {\n" +
				"\t\t\t\t\trReturn = true;\n" +
				"\t\t\t}\n" +
				"\t\t\treturn rReturn;\n";
			} else {
				tWorkFlowFile += "\t\t\treturn true;\n";
			}
			tWorkFlowFile += "\t\t}\n" +
				"\t\tpublic override string NewReference ()\n" +
				"\t\t{\n";
			if (tAccountUsed == true) {
				tWorkFlowFile += "\t\t\treturn NewReferenceFromUUID(NWDAppConfiguration.SharedInstance.SelectedEnvironment().PlayerAccountReference);\n";
			} else {
				tWorkFlowFile += "\t\t\treturn NewReferenceFromUUID(\"\");\n";
			}
			tWorkFlowFile += "\t\t}\n" +
			"//-------------------- \n" +
			"\t\tpublic override bool IsLockedObject () // return true during the player game if this object cannot be modified by player\n" +
			"\t\t{\n" +
			"\t\t\t#" + "if UNITY_EDITOR\n" +
			"\t\t\treturn false;\n" +
			"\t\t\t#" + "else\n" +
			"\t\t\treturn " + tLockedObject + ";\n" +
			"\t\t\t#" + "endif\n" +
			"\t\t}\n" +
			"//-------------------- \n" +
			"public static " + tClassName + " NewObject()\n" +
			"\t\t{\n" +
				"\t\t\t" + tClassName + " rReturn = " + tClassName + ".NewInstance () as " + tClassName + ";\n";

			if (tAccountUsed == true) {
				tWorkFlowFile += "" +
					"\t\t\tNWDReferenceType<NWDAccount> tAccountReference = new NWDReferenceType<NWDAccount> ();\n" +
					"\t\t\ttAccountReference.SetReference (NWDAppConfiguration.SharedInstance.SelectedEnvironment ().PlayerAccountReference);\n" +
					"\t\t\trReturn."+tAccountPropertyName+" = tAccountReference;\n" +
					"\t\t\trReturn.UpdateMeLater ();" +
//					"\t\t\trReturn."+tAccountPropertyName+".SetReference(NWDAppConfiguration.SharedInstance.SelectedEnvironment ().PlayerAccountReference);\n" +
//					"\t\t\trReturn.UpdateMe();\n" +
					"";
			}

			tWorkFlowFile += "\t\t\treturn rReturn;\n" +
			"\t\t}\n" +
			"//-------------------- \n" +
			"\t\tpublic override void InstanceInit ()\n" +
			"\t\t{\n";
			foreach (var tPropertyInfo in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
				Type tTypeOfThis = tPropertyInfo.PropertyType;
				if (tTypeOfThis.IsSubclassOf (typeof(BTBDataType))) {
					if (tTypeOfThis.IsGenericType) {
						int tBacktick = tTypeOfThis.GetGenericTypeDefinition ().Name.IndexOf('`');
						Type tSubType = tTypeOfThis.GetGenericArguments () [0];
						tWorkFlowFile += "\t\t\t" + tPropertyInfo.Name + " = new " + tTypeOfThis.GetGenericTypeDefinition ().Name.Remove(tBacktick) + "<"+tSubType.Name+">(); // generic type \n";
					} else {
						tWorkFlowFile += "\t\t\t" + tPropertyInfo.Name + " = new " + tTypeOfThis.Name + "(); // normal type\n";
					}
				}
			}
			tWorkFlowFile += "" +
				"\t\t}\n" +
				"//-------------------- \n" +
				"    public static " + tClassName + "[] GetAllObjects()\n" +
				"     {\n" +
				"      List<" + tClassName + "> rReturn = new List<" + tClassName + ">();\n" +
				"     foreach (" + tClassName + " tObject in " + tClassName + ".ObjectsList)\n" +
				"      {\n" +
				"       if (tObject.Reference != null" + tAccountAnalzeListANDString + ") {\n" +
				"           rReturn.Add(tObject);\n" +
				"          }\n" +
				"       }\n" +
				"      return rReturn.ToArray();\n" +
				"    }\n" +
				"//-------------------- \n" +
			"    public static " + tClassName + " GetObjectWithReference(string sReference)\n" +
			"     {\n" +
			"     " + tClassName + " rReturn = null;\n" +
			"     foreach (" + tClassName + " tObject in " + tClassName + ".ObjectsList)\n" +
			"      {\n" +
			"       if (tObject.Reference == sReference" + tAccountAnalzeListANDString + ") {\n" +
			"       rReturn = tObject;\n" +
			"       break;\n" +
			"      }\n" +
			"      }\n" +
			"      return rReturn;\n" +
			"    }\n" +
			"//-------------------- \n" +
			"\n" +
			"    public static " + tClassName + "[] GetObjectsWithReferences(string[] sReferences)\n" +
			"     {\n" +
			"      List<" + tClassName + "> rReturn = new List<" + tClassName + ">();\n" +
			"      foreach(string tReference in sReferences)\n" +
			"       {\n" +
			"         " + tClassName + " tObject = GetObjectWithReference(tReference);\n" +
			"         if (tObject!=null)\n" +
			"          {\n" +
			"           rReturn.Add(tObject);\n" +
			"          }\n" +
			"       }\n" +
			"      return rReturn.ToArray();\n" +
			"    }\n" +
			"//-------------------- \n" +
			"\n" +
			"    public static " + tClassName + " GetObjectWithInternalKey(string sInternalKey)\n" +
			"    {\n" +
			"     " + tClassName + " rReturn = null;\n" +
			"     foreach (" + tClassName + " tObject in " + tClassName + ".ObjectsList)\n" +
			"      {\n" +
			"       if (tObject.InternalKey == sInternalKey" + tAccountAnalzeListANDString + ")\n" +
			"       {\n" +
			"         rReturn = tObject;\n" +
			"         break;\n" +
			"       }\n" +
			"      }\n" +
			"     return rReturn;\n" +
			"    }\n" +
			"//-------------------- \n" +
			"\n" +
			"    public static " + tClassName + "[] GetObjectsWithInternalKeys(string[] sInternalKeys)\n" +
			"     {\n" +
			"      List<" + tClassName + "> rReturn = new List<" + tClassName + ">();\n" +
			"      foreach(string tInternalKey in sInternalKeys)\n" +
			"       {\n" +
			"         " + tClassName + " tObject = GetObjectWithInternalKey(tInternalKey);\n" +
			"         if (tObject!=null)\n" +
			"          {\n" +
			"           rReturn.Add(tObject);\n" +
			"          }\n" +
			"       }\n" +
			"      return rReturn.ToArray();\n" +
			"    }\n" +
			"\n" +
			"//-------------------- \n" +
			"// USER UPDATE \n" +
			"//-------------------- \n" +
			"    public static void TryToChangeUserForAllObjects (string sOldUser, string sNewUser)\n" +
			"    {\n" +
			"     foreach (" + tClassName + " tObject in " + tClassName + ".ObjectsList)\n" +
			"      {\n" +
			"       tObject.ChangeUser(sOldUser, sNewUser);" +
			"      }\n" +
			"    }\n" +
			"//-------------------- \n";

			// write methods to upadte the user account reference UUID when definitive UUID is receipt
			tWorkFlowFile += "" +
			"    public void ChangeUser(string sOldUser, string sNewUser)\n" +
			"    {\n";

			if (tAccountAnalzeList.Count > 0) {
				tWorkFlowFile += "" +
				"     // TO DO CHANGE USER IF USER EXISTS IN \n";
				foreach (string tProperty in tAccountReferenceList) {
					tWorkFlowFile += "     if (" + tProperty + ".Value == sOldUser)\n" +
					"      {\n" +
					"        " + tProperty + ".SetReference(sNewUser);\n" +
					"      }\n";
				}
				tWorkFlowFile += "" +
				"     // If user exist In need to change the Reference To by simple replace the usersequence by the new user sequence (the xxxxxxT by xxxxxxxS)\n" +
				"     UpdateReference(sOldUser, sNewUser);\n" +
				"     UpdateMe();\n";
			}
			tWorkFlowFile += "" +
			"    }\n" +
			"//-------------------- \n" +
			"\n";

			foreach (var tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {

				tWorkFlowFile += "//-------------------- \n";
				tWorkFlowFile += "// " + tProp.Name.ToUpper () + "\n";
				tWorkFlowFile += "//-------------------- \n";


				Type tTypeOfThis = tProp.PropertyType;

				if (tTypeOfThis.IsGenericType) {
					if (tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferenceType<>)) {
						Type tSubType = tTypeOfThis.GetGenericArguments () [0];

						tWorkFlowFile += "" +
						"    public " + tSubType.Name + " " + tProp.Name + "Object()\n" +
						"     {\n" +
						"      " + tSubType.Name + " rReturn = " + tSubType.Name + ".GetObjectByReference(this." + tProp.Name + ".Value) as " + tSubType.Name + ";\n" +
						"      return rReturn;\n" +
						"     }\n" +
						"\n" +
						"    static public " + tClassName + "[] ObjectLinkedWith" + tProp.Name + "(" + tSubType.Name + " sConnexion)\n" +
						"     {\n" +
						"      List<" + tClassName + "> rReturn = new List<" + tClassName + ">();\n" +
						"      // I need to find the reference in the DataBase .... or in objects ... \n" +
						"     foreach (" + tClassName + " tObject in " + tClassName + ".ObjectsList)\n" +
						"      {\n" +
						"       if (tObject." + tProp.Name + ".ContainsReference(sConnexion.Reference)) {\n" +
						"       " + tClassName + " tTryObject = GetObjectWithReference(tObject.Reference);\n" +
						"       if (tTryObject!=null)\n" +
						"        {\n" +
						"         rReturn.Add(tTryObject);\n" +
						"        }\n" +
						"      }\n" +
						"      }\n" +
						"      return rReturn.ToArray();\n" +
						"     }\n" +
						"\n";
					}

					if (tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferencesListType<>)) {
						Type tSubType = tTypeOfThis.GetGenericArguments () [0];
						tWorkFlowFile += "    public " + tSubType.Name + "[] " + tProp.Name + "Objects()\n" +
						"     {\n" +
						"      string[] tReferencesArray = " + tProp.Name + ".Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);\n" +
						"      " + tSubType.Name + "[] rReturn = " + tSubType.Name + ".GetObjectsWithReferences(tReferencesArray);\n" +
						"      return rReturn;\n" +
						"     }\n" +
						"\n" +
						"    static public " + tClassName + "[] ObjectsLinkedWith" + tProp.Name + "(" + tSubType.Name + " sConnexion)\n" +
						"     {\n" +
						"      List<" + tClassName + "> rReturn = new List<" + tClassName + ">();\n" +
						"      // I need to find the reference in the DataBase .... or in objects ... \n" +
						"     foreach (" + tClassName + " tObject in " + tClassName + ".ObjectsList)\n" +
						"      {\n" +
						"       if (tObject." + tProp.Name + ".Value.Contains(sConnexion.Reference)) {\n" +
						"       " + tClassName + " tTryObject = GetObjectWithReference(tObject.Reference);\n" +
						"       if (tTryObject!=null)\n" +
						"        {\n" +
						"         rReturn.Add(tTryObject);\n" +
						"        }\n" +
						"      }\n" +
						"      }\n" +
						"      return rReturn.ToArray();\n" +
						"     }\n" +
						"\n";
					}
				}
			}
			tWorkFlowFile += 
				"	}\n" +
			" }\n";
			// write the workflow file in project
			File.WriteAllText (tEngineRootFolder + "/" + tClassName + "-Workflow.cs", tWorkFlowFile);
			// force to import this file by Unity3D
			AssetDatabase.ImportAsset (tEngineRootFolder + "/" + tClassName + "-Workflow.cs");
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif
