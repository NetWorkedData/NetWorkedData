//NWD Autogenerate File at 2017-07-21
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
	public struct NWDConfigurationConnexion
	{
		[SerializeField]
		public string Reference;
		public NWDConfiguration GetObject ()
		{
			return NWDConfiguration.GetObjectWithReference (Reference);
		}
		public void SetObject (NWDConfiguration sObject)
		{
			Reference = sObject.Reference;
		}
		public NWDConfiguration NewObject ()
		{
			NWDConfiguration tObject = NWDConfiguration.NewObject ();
			Reference = tObject.Reference;
			return tObject;
		}
	}
	//-------------------- 
	// CONNEXION METHODS
	//-------------------- 
	public class NWDConfigurationConnexionAttribut : PropertyAttribute
	{
		public bool ShowInspector = true;
		public bool Editable = false;
		public bool EditButton = true;
		public bool NewButton = true;
		public NWDConfigurationConnexionAttribut ()
		{
		}
		public NWDConfigurationConnexionAttribut (bool sShowInspector, bool sEditable, bool sEditButton, bool sNewButton)
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
	[CustomPropertyDrawer (typeof(NWDConfigurationConnexion))]
	public partial class NWDConfigurationConnexionDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			NWDConfigurationConnexionAttribut tReferenceConnexion = new NWDConfigurationConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDConfigurationConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDConfigurationConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDConfigurationConnexionAttribut), true)[0];
			}
			return NWDConfiguration.ReferenceConnexionHeightSerialized(property, tReferenceConnexion.ShowInspector);
		}
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			NWDConfigurationConnexionAttribut tReferenceConnexion = new NWDConfigurationConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDConfigurationConnexionAttribut), true).Length > 0)
				{
					tReferenceConnexion = (NWDConfigurationConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDConfigurationConnexionAttribut), true)[0];
				}
			NWDConfiguration.ReferenceConnexionFieldSerialized (position, property.displayName, property, "", tReferenceConnexion.ShowInspector, tReferenceConnexion.Editable, tReferenceConnexion.EditButton, tReferenceConnexion.NewButton);
		}
	}
	#endif
//-------------------- 
// GENERAL METHODS
//-------------------- 
	public partial class NWDConfiguration : NWDBasis <NWDConfiguration>
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
public static NWDConfiguration NewObject()
		{
			NWDConfiguration rReturn = NWDConfiguration.NewInstance () as NWDConfiguration;
			return rReturn;
		}
//-------------------- 
		public override void InstanceInit ()
		{
			ValueString = new NWDLocalizableStringType(); // normal type
		}
//-------------------- 
    public static NWDConfiguration[] GetAllObjects()
     {
      List<NWDConfiguration> rReturn = new List<NWDConfiguration>();
     foreach (NWDConfiguration tObject in NWDConfiguration.ObjectsList)
      {
       if (tObject.Reference != null) {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 
    public static NWDConfiguration GetObjectWithReference(string sReference)
     {
     NWDConfiguration rReturn = null;
     foreach (NWDConfiguration tObject in NWDConfiguration.ObjectsList)
      {
       if (tObject.Reference == sReference) {
       rReturn = tObject;
       break;
      }
      }
      return rReturn;
    }
//-------------------- 

    public static NWDConfiguration[] GetObjectsWithReferences(string[] sReferences)
     {
      List<NWDConfiguration> rReturn = new List<NWDConfiguration>();
      foreach(string tReference in sReferences)
       {
         NWDConfiguration tObject = GetObjectWithReference(tReference);
         if (tObject!=null)
          {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 

    public static NWDConfiguration GetObjectWithInternalKey(string sInternalKey)
    {
     NWDConfiguration rReturn = null;
     foreach (NWDConfiguration tObject in NWDConfiguration.ObjectsList)
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

    public static NWDConfiguration[] GetObjectsWithInternalKeys(string[] sInternalKeys)
     {
      List<NWDConfiguration> rReturn = new List<NWDConfiguration>();
      foreach(string tInternalKey in sInternalKeys)
       {
         NWDConfiguration tObject = GetObjectWithInternalKey(tInternalKey);
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
     foreach (NWDConfiguration tObject in NWDConfiguration.ObjectsList)
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
