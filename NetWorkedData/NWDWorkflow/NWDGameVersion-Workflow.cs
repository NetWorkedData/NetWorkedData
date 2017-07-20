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
	public struct NWDGameVersionConnexion
	{
		[SerializeField]
		public string Reference;
		public NWDGameVersion GetObject ()
		{
			return NWDGameVersion.GetObjectWithReference (Reference);
		}
		public void SetObject (NWDGameVersion sObject)
		{
			Reference = sObject.Reference;
		}
		public NWDGameVersion NewObject ()
		{
			NWDGameVersion tObject = NWDGameVersion.NewObject ();
			Reference = tObject.Reference;
			return tObject;
		}
	}
	//-------------------- 
	// CONNEXION METHODS
	//-------------------- 
	public class NWDGameVersionConnexionAttribut : PropertyAttribute
	{
		public bool ShowInspector = true;
		public bool Editable = false;
		public bool EditButton = true;
		public bool NewButton = true;
		public NWDGameVersionConnexionAttribut ()
		{
		}
		public NWDGameVersionConnexionAttribut (bool sShowInspector, bool sEditable, bool sEditButton, bool sNewButton)
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
	[CustomPropertyDrawer (typeof(NWDGameVersionConnexion))]
	public partial class NWDGameVersionConnexionDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			NWDGameVersionConnexionAttribut tReferenceConnexion = new NWDGameVersionConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDGameVersionConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDGameVersionConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDGameVersionConnexionAttribut), true)[0];
			}
			return NWDGameVersion.ReferenceConnexionHeightSerialized(property, tReferenceConnexion.ShowInspector);
		}
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			NWDGameVersionConnexionAttribut tReferenceConnexion = new NWDGameVersionConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDGameVersionConnexionAttribut), true).Length > 0)
				{
					tReferenceConnexion = (NWDGameVersionConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDGameVersionConnexionAttribut), true)[0];
				}
			NWDGameVersion.ReferenceConnexionFieldSerialized (position, property.displayName, property, "", tReferenceConnexion.ShowInspector, tReferenceConnexion.Editable, tReferenceConnexion.EditButton, tReferenceConnexion.NewButton);
		}
	}
	#endif
//-------------------- 
// GENERAL METHODS
//-------------------- 
	public partial class NWDGameVersion : NWDBasis <NWDGameVersion>
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
public static NWDGameVersion NewObject()
		{
			NWDGameVersion rReturn = NWDGameVersion.NewInstance () as NWDGameVersion;
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
    public static NWDGameVersion[] GetAllObjects()
     {
      List<NWDGameVersion> rReturn = new List<NWDGameVersion>();
     foreach (NWDGameVersion tObject in NWDGameVersion.ObjectsList)
      {
       if (tObject.Reference != null) {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 
    public static NWDGameVersion GetObjectWithReference(string sReference)
     {
     NWDGameVersion rReturn = null;
     foreach (NWDGameVersion tObject in NWDGameVersion.ObjectsList)
      {
       if (tObject.Reference == sReference) {
       rReturn = tObject;
       break;
      }
      }
      return rReturn;
    }
//-------------------- 

    public static NWDGameVersion[] GetObjectsWithReferences(string[] sReferences)
     {
      List<NWDGameVersion> rReturn = new List<NWDGameVersion>();
      foreach(string tReference in sReferences)
       {
         NWDGameVersion tObject = GetObjectWithReference(tReference);
         if (tObject!=null)
          {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 

    public static NWDGameVersion GetObjectWithInternalKey(string sInternalKey)
    {
     NWDGameVersion rReturn = null;
     foreach (NWDGameVersion tObject in NWDGameVersion.ObjectsList)
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

    public static NWDGameVersion[] GetObjectsWithInternalKeys(string[] sInternalKeys)
     {
      List<NWDGameVersion> rReturn = new List<NWDGameVersion>();
      foreach(string tInternalKey in sInternalKeys)
       {
         NWDGameVersion tObject = GetObjectWithInternalKey(tInternalKey);
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
     foreach (NWDGameVersion tObject in NWDGameVersion.ObjectsList)
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
