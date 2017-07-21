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
	public struct NWDKeywordConnexion
	{
		[SerializeField]
		public string Reference;
		public NWDKeyword GetObject ()
		{
			return NWDKeyword.GetObjectWithReference (Reference);
		}
		public void SetObject (NWDKeyword sObject)
		{
			Reference = sObject.Reference;
		}
		public NWDKeyword NewObject ()
		{
			NWDKeyword tObject = NWDKeyword.NewObject ();
			Reference = tObject.Reference;
			return tObject;
		}
	}
	//-------------------- 
	// CONNEXION METHODS
	//-------------------- 
	public class NWDKeywordConnexionAttribut : PropertyAttribute
	{
		public bool ShowInspector = true;
		public bool Editable = false;
		public bool EditButton = true;
		public bool NewButton = true;
		public NWDKeywordConnexionAttribut ()
		{
		}
		public NWDKeywordConnexionAttribut (bool sShowInspector, bool sEditable, bool sEditButton, bool sNewButton)
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
	[CustomPropertyDrawer (typeof(NWDKeywordConnexion))]
	public partial class NWDKeywordConnexionDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			NWDKeywordConnexionAttribut tReferenceConnexion = new NWDKeywordConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDKeywordConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDKeywordConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDKeywordConnexionAttribut), true)[0];
			}
			return NWDKeyword.ReferenceConnexionHeightSerialized(property, tReferenceConnexion.ShowInspector);
		}
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			NWDKeywordConnexionAttribut tReferenceConnexion = new NWDKeywordConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDKeywordConnexionAttribut), true).Length > 0)
				{
					tReferenceConnexion = (NWDKeywordConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDKeywordConnexionAttribut), true)[0];
				}
			NWDKeyword.ReferenceConnexionFieldSerialized (position, property.displayName, property, "", tReferenceConnexion.ShowInspector, tReferenceConnexion.Editable, tReferenceConnexion.EditButton, tReferenceConnexion.NewButton);
		}
	}
	#endif
//-------------------- 
// GENERAL METHODS
//-------------------- 
	public partial class NWDKeyword : NWDBasis <NWDKeyword>
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
public static NWDKeyword NewObject()
		{
			NWDKeyword rReturn = NWDKeyword.NewInstance () as NWDKeyword;
			return rReturn;
		}
//-------------------- 
		public override void InstanceInit ()
		{
			Name = new NWDLocalizableStringType(); // normal type
		}
//-------------------- 
    public static NWDKeyword[] GetAllObjects()
     {
      List<NWDKeyword> rReturn = new List<NWDKeyword>();
     foreach (NWDKeyword tObject in NWDKeyword.ObjectsList)
      {
       if (tObject.Reference != null) {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 
    public static NWDKeyword GetObjectWithReference(string sReference)
     {
     NWDKeyword rReturn = null;
     foreach (NWDKeyword tObject in NWDKeyword.ObjectsList)
      {
       if (tObject.Reference == sReference) {
       rReturn = tObject;
       break;
      }
      }
      return rReturn;
    }
//-------------------- 

    public static NWDKeyword[] GetObjectsWithReferences(string[] sReferences)
     {
      List<NWDKeyword> rReturn = new List<NWDKeyword>();
      foreach(string tReference in sReferences)
       {
         NWDKeyword tObject = GetObjectWithReference(tReference);
         if (tObject!=null)
          {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 

    public static NWDKeyword GetObjectWithInternalKey(string sInternalKey)
    {
     NWDKeyword rReturn = null;
     foreach (NWDKeyword tObject in NWDKeyword.ObjectsList)
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

    public static NWDKeyword[] GetObjectsWithInternalKeys(string[] sInternalKeys)
     {
      List<NWDKeyword> rReturn = new List<NWDKeyword>();
      foreach(string tInternalKey in sInternalKeys)
       {
         NWDKeyword tObject = GetObjectWithInternalKey(tInternalKey);
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
     foreach (NWDKeyword tObject in NWDKeyword.ObjectsList)
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
