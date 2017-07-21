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
	public struct NWDFamilyConnexion
	{
		[SerializeField]
		public string Reference;
		public NWDFamily GetObject ()
		{
			return NWDFamily.GetObjectWithReference (Reference);
		}
		public void SetObject (NWDFamily sObject)
		{
			Reference = sObject.Reference;
		}
		public NWDFamily NewObject ()
		{
			NWDFamily tObject = NWDFamily.NewObject ();
			Reference = tObject.Reference;
			return tObject;
		}
	}
	//-------------------- 
	// CONNEXION METHODS
	//-------------------- 
	public class NWDFamilyConnexionAttribut : PropertyAttribute
	{
		public bool ShowInspector = true;
		public bool Editable = false;
		public bool EditButton = true;
		public bool NewButton = true;
		public NWDFamilyConnexionAttribut ()
		{
		}
		public NWDFamilyConnexionAttribut (bool sShowInspector, bool sEditable, bool sEditButton, bool sNewButton)
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
	[CustomPropertyDrawer (typeof(NWDFamilyConnexion))]
	public partial class NWDFamilyConnexionDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			NWDFamilyConnexionAttribut tReferenceConnexion = new NWDFamilyConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDFamilyConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDFamilyConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDFamilyConnexionAttribut), true)[0];
			}
			return NWDFamily.ReferenceConnexionHeightSerialized(property, tReferenceConnexion.ShowInspector);
		}
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			NWDFamilyConnexionAttribut tReferenceConnexion = new NWDFamilyConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDFamilyConnexionAttribut), true).Length > 0)
				{
					tReferenceConnexion = (NWDFamilyConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDFamilyConnexionAttribut), true)[0];
				}
			NWDFamily.ReferenceConnexionFieldSerialized (position, property.displayName, property, "", tReferenceConnexion.ShowInspector, tReferenceConnexion.Editable, tReferenceConnexion.EditButton, tReferenceConnexion.NewButton);
		}
	}
	#endif
//-------------------- 
// GENERAL METHODS
//-------------------- 
	public partial class NWDFamily : NWDBasis <NWDFamily>
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
public static NWDFamily NewObject()
		{
			NWDFamily rReturn = NWDFamily.NewInstance () as NWDFamily;
			return rReturn;
		}
//-------------------- 
		public override void InstanceInit ()
		{
			Name = new NWDLocalizableStringType(); // normal type
		}
//-------------------- 
    public static NWDFamily[] GetAllObjects()
     {
      List<NWDFamily> rReturn = new List<NWDFamily>();
     foreach (NWDFamily tObject in NWDFamily.ObjectsList)
      {
       if (tObject.Reference != null) {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 
    public static NWDFamily GetObjectWithReference(string sReference)
     {
     NWDFamily rReturn = null;
     foreach (NWDFamily tObject in NWDFamily.ObjectsList)
      {
       if (tObject.Reference == sReference) {
       rReturn = tObject;
       break;
      }
      }
      return rReturn;
    }
//-------------------- 

    public static NWDFamily[] GetObjectsWithReferences(string[] sReferences)
     {
      List<NWDFamily> rReturn = new List<NWDFamily>();
      foreach(string tReference in sReferences)
       {
         NWDFamily tObject = GetObjectWithReference(tReference);
         if (tObject!=null)
          {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 

    public static NWDFamily GetObjectWithInternalKey(string sInternalKey)
    {
     NWDFamily rReturn = null;
     foreach (NWDFamily tObject in NWDFamily.ObjectsList)
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

    public static NWDFamily[] GetObjectsWithInternalKeys(string[] sInternalKeys)
     {
      List<NWDFamily> rReturn = new List<NWDFamily>();
      foreach(string tInternalKey in sInternalKeys)
       {
         NWDFamily tObject = GetObjectWithInternalKey(tInternalKey);
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
     foreach (NWDFamily tObject in NWDFamily.ObjectsList)
      {
       tObject.ChangeUser(sOldUser, sNewUser);      }
    }
//-------------------- 
    public void ChangeUser(string sOldUser, string sNewUser)
    {
    }
//-------------------- 

//-------------------- 
// NAME
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
