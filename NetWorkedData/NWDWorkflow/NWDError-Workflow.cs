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
	public struct NWDErrorConnexion
	{
		[SerializeField]
		public string Reference;
		public NWDError GetObject ()
		{
			return NWDError.GetObjectWithReference (Reference);
		}
		public void SetObject (NWDError sObject)
		{
			Reference = sObject.Reference;
		}
		public NWDError NewObject ()
		{
			NWDError tObject = NWDError.NewObject ();
			Reference = tObject.Reference;
			return tObject;
		}
	}
	//-------------------- 
	// CONNEXION METHODS
	//-------------------- 
	public class NWDErrorConnexionAttribut : PropertyAttribute
	{
		public bool ShowInspector = true;
		public bool Editable = false;
		public bool EditButton = true;
		public bool NewButton = true;
		public NWDErrorConnexionAttribut ()
		{
		}
		public NWDErrorConnexionAttribut (bool sShowInspector, bool sEditable, bool sEditButton, bool sNewButton)
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
	[CustomPropertyDrawer (typeof(NWDErrorConnexion))]
	public partial class NWDErrorConnexionDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			NWDErrorConnexionAttribut tReferenceConnexion = new NWDErrorConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDErrorConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDErrorConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDErrorConnexionAttribut), true)[0];
			}
			return NWDError.ReferenceConnexionHeightSerialized(property, tReferenceConnexion.ShowInspector);
		}
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			NWDErrorConnexionAttribut tReferenceConnexion = new NWDErrorConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDErrorConnexionAttribut), true).Length > 0)
				{
					tReferenceConnexion = (NWDErrorConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDErrorConnexionAttribut), true)[0];
				}
			NWDError.ReferenceConnexionFieldSerialized (position, property.displayName, property, "", tReferenceConnexion.ShowInspector, tReferenceConnexion.Editable, tReferenceConnexion.EditButton, tReferenceConnexion.NewButton);
		}
	}
	#endif
//-------------------- 
// GENERAL METHODS
//-------------------- 
	public partial class NWDError : NWDBasis <NWDError>
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
//		public override bool IsLockedObject () // return true during the player game if this object cannot be modified by player
//		{
//			#if UNITY_EDITOR
//			return false;
//			#else
//			return true;
//			#endif
//		}
//-------------------- 
public static NWDError NewObject()
		{
			NWDError rReturn = NWDError.NewInstance () as NWDError;
			return rReturn;
		}
//-------------------- 
		public override void InstanceInit ()
		{
			LocalizedTitle = new NWDLocalizableStringType(); // normal type
			LocalizedDescription = new NWDLocalizableStringType(); // normal type
		}
//-------------------- 
    public static NWDError[] GetAllObjects()
     {
      List<NWDError> rReturn = new List<NWDError>();
     foreach (NWDError tObject in NWDError.ObjectsList)
      {
       if (tObject.Reference != null) {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 
    public static NWDError GetObjectWithReference(string sReference)
     {
     NWDError rReturn = null;
     foreach (NWDError tObject in NWDError.ObjectsList)
      {
       if (tObject.Reference == sReference) {
       rReturn = tObject;
       break;
      }
      }
      return rReturn;
    }
//-------------------- 

    public static NWDError[] GetObjectsWithReferences(string[] sReferences)
     {
      List<NWDError> rReturn = new List<NWDError>();
      foreach(string tReference in sReferences)
       {
         NWDError tObject = GetObjectWithReference(tReference);
         if (tObject!=null)
          {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 

    public static NWDError GetObjectWithInternalKey(string sInternalKey)
    {
     NWDError rReturn = null;
     foreach (NWDError tObject in NWDError.ObjectsList)
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

    public static NWDError[] GetObjectsWithInternalKeys(string[] sInternalKeys)
     {
      List<NWDError> rReturn = new List<NWDError>();
      foreach(string tInternalKey in sInternalKeys)
       {
         NWDError tObject = GetObjectWithInternalKey(tInternalKey);
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
     foreach (NWDError tObject in NWDError.ObjectsList)
      {
       tObject.ChangeUser(sOldUser, sNewUser);      }
    }
//-------------------- 
    public void ChangeUser(string sOldUser, string sNewUser)
    {
    }
//-------------------- 

//-------------------- 
// DOMAIN
//-------------------- 
//-------------------- 
// CODE
//-------------------- 
//-------------------- 
// LOCALIZEDTITLE
//-------------------- 
//-------------------- 
// LOCALIZEDDESCRIPTION
//-------------------- 
//-------------------- 
// TYPE
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
