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
	public struct NWDVersionConnexion
	{
		[SerializeField]
		public string Reference;
		public NWDVersion GetObject ()
		{
			return NWDVersion.GetObjectWithReference (Reference);
		}
		public void SetObject (NWDVersion sObject)
		{
			Reference = sObject.Reference;
		}
		public NWDVersion NewObject ()
		{
			NWDVersion tObject = NWDVersion.NewObject ();
			Reference = tObject.Reference;
			return tObject;
		}
	}
	//-------------------- 
	// CONNEXION METHODS
	//-------------------- 
	public class NWDVersionConnexionAttribut : PropertyAttribute
	{
		public bool ShowInspector = true;
		public bool Editable = false;
		public bool EditButton = true;
		public bool NewButton = true;
		public NWDVersionConnexionAttribut ()
		{
		}
		public NWDVersionConnexionAttribut (bool sShowInspector, bool sEditable, bool sEditButton, bool sNewButton)
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
	[CustomPropertyDrawer (typeof(NWDVersionConnexion))]
	public partial class NWDVersionConnexionDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			NWDVersionConnexionAttribut tReferenceConnexion = new NWDVersionConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDVersionConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDVersionConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDVersionConnexionAttribut), true)[0];
			}
			return NWDVersion.ReferenceConnexionHeightSerialized(property, tReferenceConnexion.ShowInspector);
		}
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			NWDVersionConnexionAttribut tReferenceConnexion = new NWDVersionConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDVersionConnexionAttribut), true).Length > 0)
				{
					tReferenceConnexion = (NWDVersionConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDVersionConnexionAttribut), true)[0];
				}
			NWDVersion.ReferenceConnexionFieldSerialized (position, property.displayName, property, "", tReferenceConnexion.ShowInspector, tReferenceConnexion.Editable, tReferenceConnexion.EditButton, tReferenceConnexion.NewButton);
		}
	}
	#endif
//-------------------- 
// GENERAL METHODS
//-------------------- 
	public partial class NWDVersion : NWDBasis <NWDVersion>
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
public static NWDVersion NewObject()
		{
			NWDVersion rReturn = NWDVersion.NewInstance () as NWDVersion;
			return rReturn;
		}
//-------------------- 
		public override void InstanceInit ()
		{
			Version = new NWDVersionType(); // normal type
			AlertTitle = new NWDLocalizableStringType(); // normal type
			AlertMessage = new NWDLocalizableStringType(); // normal type
			AlertButtonOK = new NWDLocalizableStringType(); // normal type
		}
//-------------------- 
    public static NWDVersion[] GetAllObjects()
     {
      List<NWDVersion> rReturn = new List<NWDVersion>();
     foreach (NWDVersion tObject in NWDVersion.ObjectsList)
      {
       if (tObject.Reference != null) {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 
    public static NWDVersion GetObjectWithReference(string sReference)
     {
     NWDVersion rReturn = null;
     foreach (NWDVersion tObject in NWDVersion.ObjectsList)
      {
       if (tObject.Reference == sReference) {
       rReturn = tObject;
       break;
      }
      }
      return rReturn;
    }
//-------------------- 

    public static NWDVersion[] GetObjectsWithReferences(string[] sReferences)
     {
      List<NWDVersion> rReturn = new List<NWDVersion>();
      foreach(string tReference in sReferences)
       {
         NWDVersion tObject = GetObjectWithReference(tReference);
         if (tObject!=null)
          {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 

    public static NWDVersion GetObjectWithInternalKey(string sInternalKey)
    {
     NWDVersion rReturn = null;
     foreach (NWDVersion tObject in NWDVersion.ObjectsList)
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

    public static NWDVersion[] GetObjectsWithInternalKeys(string[] sInternalKeys)
     {
      List<NWDVersion> rReturn = new List<NWDVersion>();
      foreach(string tInternalKey in sInternalKeys)
       {
         NWDVersion tObject = GetObjectWithInternalKey(tInternalKey);
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
     foreach (NWDVersion tObject in NWDVersion.ObjectsList)
      {
       tObject.ChangeUser(sOldUser, sNewUser);      }
    }
//-------------------- 
    public void ChangeUser(string sOldUser, string sNewUser)
    {
    }
//-------------------- 

//-------------------- 
// VERSION
//-------------------- 
//-------------------- 
// BUILDACTIVE
//-------------------- 
//-------------------- 
// ACTIVEDEV
//-------------------- 
//-------------------- 
// ACTIVEPREPROD
//-------------------- 
//-------------------- 
// ACTIVEPROD
//-------------------- 
//-------------------- 
// BLOCKDATAUPDATE
//-------------------- 
//-------------------- 
// FORCEAPPLICATIONUPDATE
//-------------------- 
//-------------------- 
// ALERTTITLE
//-------------------- 
//-------------------- 
// ALERTMESSAGE
//-------------------- 
//-------------------- 
// ALERTBUTTONOK
//-------------------- 
//-------------------- 
// APPLESTOREURL
//-------------------- 
//-------------------- 
// GOOGLEPLAYURL
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
