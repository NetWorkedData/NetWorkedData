//NWD Autogenerate File at 2017-07-20
//Copyright NetWorkedDatas ideMobi 2017
//Created by Jean-François CONTART
//-------------------- 
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

namespace NetWorkedData
 {
	//-------------------- 
	// CONNEXION STRUCTURE METHODS
	//-------------------- 
	[Serializable]
	public struct NWDGameConfigurationConnexion
	{
		[SerializeField]
		public string Reference;
		public NWDGameConfiguration GetObject ()
		{
			return NWDGameConfiguration.GetObjectWithReference (Reference);
		}
		public void SetObject (NWDGameConfiguration sObject)
		{
			Reference = sObject.Reference;
		}
		public NWDGameConfiguration NewObject ()
		{
			NWDGameConfiguration tObject = NWDGameConfiguration.NewObject ();
			Reference = tObject.Reference;
			return tObject;
		}
	}
	//-------------------- 
	// CONNEXION METHODS
	//-------------------- 
	public class NWDGameConfigurationConnexionAttribut : PropertyAttribute
	{
		public bool ShowInspector = true;
		public bool Editable = false;
		public bool EditButton = true;
		public bool NewButton = true;
		public NWDGameConfigurationConnexionAttribut ()
		{
		}
		public NWDGameConfigurationConnexionAttribut (bool sShowInspector, bool sEditable, bool sEditButton, bool sNewButton)
		{
			ShowInspector = sShowInspector;
			Editable = sEditable;
			EditButton = sEditButton;
			NewButton = sNewButton;
		}
	}

	//-------------------- 
	// CUSTOM PROPERTY DRAWER METHODS
	//-------------------- 
	#if UNITY_EDITOR
	[CustomPropertyDrawer (typeof(NWDGameConfigurationConnexion))]
	public partial class NWDGameConfigurationConnexionDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			NWDGameConfigurationConnexionAttribut tReferenceConnexion = new NWDGameConfigurationConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDGameConfigurationConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDGameConfigurationConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDGameConfigurationConnexionAttribut), true)[0];
			}
			return NWDGameConfiguration.ReferenceConnexionHeightSerialized(property, tReferenceConnexion.ShowInspector);
		}
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			NWDGameConfigurationConnexionAttribut tReferenceConnexion = new NWDGameConfigurationConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDGameConfigurationConnexionAttribut), true).Length > 0)
				{
					tReferenceConnexion = (NWDGameConfigurationConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDGameConfigurationConnexionAttribut), true)[0];
				}
			NWDGameConfiguration.ReferenceConnexionFieldSerialized (position, property.displayName, property, "", tReferenceConnexion.ShowInspector, tReferenceConnexion.Editable, tReferenceConnexion.EditButton, tReferenceConnexion.NewButton);
		}
	}
	#endif
//-------------------- 
// GENERAL METHODS
//-------------------- 
	public partial class NWDGameConfiguration : NWDBasis <NWDGameConfiguration>
	{
//-------------------- 
		public override bool IsAccountDependent ()
		{
			return false;
		}
//-------------------- 
		public override bool IsAccountConnected (string sAccountReference)
		{
			return true;
		}
		public override string NewReference ()
		{
			return NewReferenceFromUUID("");
		}
//-------------------- 
		public override bool IsLockedObject () // return true during the player game if this object cannot be modified by player
		{
			#if UNITY_EDITOR
			return false;
			#else
			return true;
			#endif
		}
//-------------------- 
public static NWDGameConfiguration NewObject()
		{
			NWDGameConfiguration rReturn = NWDGameConfiguration.NewInstance () as NWDGameConfiguration;
			return rReturn;
		}
//-------------------- 
		public override void InstanceInit ()
		{
			ValueString = new NWDLocalizableStringType(); // normal type
		}
//-------------------- 
    public static NWDGameConfiguration[] GetAllObjects()
     {
      List<NWDGameConfiguration> rReturn = new List<NWDGameConfiguration>();
     foreach (NWDGameConfiguration tObject in NWDGameConfiguration.ObjectsList)
      {
       if (tObject.Reference != null) {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 
    public static NWDGameConfiguration GetObjectWithReference(string sReference)
     {
     NWDGameConfiguration rReturn = null;
     foreach (NWDGameConfiguration tObject in NWDGameConfiguration.ObjectsList)
      {
       if (tObject.Reference == sReference) {
       rReturn = tObject;
       break;
      }
      }
      return rReturn;
    }
//-------------------- 

    public static NWDGameConfiguration[] GetObjectsWithReferences(string[] sReferences)
     {
      List<NWDGameConfiguration> rReturn = new List<NWDGameConfiguration>();
      foreach(string tReference in sReferences)
       {
         NWDGameConfiguration tObject = GetObjectWithReference(tReference);
         if (tObject!=null)
          {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 

    public static NWDGameConfiguration GetObjectWithInternalKey(string sInternalKey)
    {
     NWDGameConfiguration rReturn = null;
     foreach (NWDGameConfiguration tObject in NWDGameConfiguration.ObjectsList)
      {
       if (tObject.InternalKey == sInternalKey)
       {
         rReturn = tObject;
         break;
       }
      }
     return rReturn;
    }
//-------------------- 

    public static NWDGameConfiguration[] GetObjectsWithInternalKeys(string[] sInternalKeys)
     {
      List<NWDGameConfiguration> rReturn = new List<NWDGameConfiguration>();
      foreach(string tInternalKey in sInternalKeys)
       {
         NWDGameConfiguration tObject = GetObjectWithInternalKey(tInternalKey);
         if (tObject!=null)
          {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }

//-------------------- 
// USER UPDATE 
//-------------------- 
    public static void TryToChangeUserForAllObjects (string sOldUser, string sNewUser)
    {
     foreach (NWDGameConfiguration tObject in NWDGameConfiguration.ObjectsList)
      {
       tObject.ChangeUser(sOldUser, sNewUser);      }
    }
//-------------------- 
    public void ChangeUser(string sOldUser, string sNewUser)
    {
    }
//-------------------- 

//-------------------- 
// VALUESTRING
//-------------------- 
//-------------------- 
// VALUEINT
//-------------------- 
//-------------------- 
// VALUEBOOL
//-------------------- 
//-------------------- 
// VALUEFLOAT
//-------------------- 
//-------------------- 
// ID
//-------------------- 
//-------------------- 
// REFERENCE
//-------------------- 
//-------------------- 
// INTERNALKEY
//-------------------- 
//-------------------- 
// INTERNALDESCRIPTION
//-------------------- 
//-------------------- 
// PREVIEW
//-------------------- 
//-------------------- 
// AC
//-------------------- 
//-------------------- 
// DC
//-------------------- 
//-------------------- 
// DM
//-------------------- 
//-------------------- 
// DD
//-------------------- 
//-------------------- 
// XX
//-------------------- 
//-------------------- 
// INTEGRITY
//-------------------- 
//-------------------- 
// DS
//-------------------- 
//-------------------- 
// DEVSYNC
//-------------------- 
//-------------------- 
// PREPRODSYNC
//-------------------- 
//-------------------- 
// PRODSYNC
//-------------------- 
	}
 }
