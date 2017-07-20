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
	public struct NWDCategoryConnexion
	{
		[SerializeField]
		public string Reference;
		public NWDCategory GetObject ()
		{
			return NWDCategory.GetObjectWithReference (Reference);
		}
		public void SetObject (NWDCategory sObject)
		{
			Reference = sObject.Reference;
		}
		public NWDCategory NewObject ()
		{
			NWDCategory tObject = NWDCategory.NewObject ();
			Reference = tObject.Reference;
			return tObject;
		}
	}
	//-------------------- 
	// CONNEXION METHODS
	//-------------------- 
	public class NWDCategoryConnexionAttribut : PropertyAttribute
	{
		public bool ShowInspector = true;
		public bool Editable = false;
		public bool EditButton = true;
		public bool NewButton = true;
		public NWDCategoryConnexionAttribut ()
		{
		}
		public NWDCategoryConnexionAttribut (bool sShowInspector, bool sEditable, bool sEditButton, bool sNewButton)
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
	[CustomPropertyDrawer (typeof(NWDCategoryConnexion))]
	public partial class NWDCategoryConnexionDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			NWDCategoryConnexionAttribut tReferenceConnexion = new NWDCategoryConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDCategoryConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDCategoryConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDCategoryConnexionAttribut), true)[0];
			}
			return NWDCategory.ReferenceConnexionHeightSerialized(property, tReferenceConnexion.ShowInspector);
		}
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			NWDCategoryConnexionAttribut tReferenceConnexion = new NWDCategoryConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDCategoryConnexionAttribut), true).Length > 0)
				{
					tReferenceConnexion = (NWDCategoryConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDCategoryConnexionAttribut), true)[0];
				}
			NWDCategory.ReferenceConnexionFieldSerialized (position, property.displayName, property, "", tReferenceConnexion.ShowInspector, tReferenceConnexion.Editable, tReferenceConnexion.EditButton, tReferenceConnexion.NewButton);
		}
	}
	#endif
//-------------------- 
// GENERAL METHODS
//-------------------- 
	public partial class NWDCategory : NWDBasis <NWDCategory>
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
public static NWDCategory NewObject()
		{
			NWDCategory rReturn = NWDCategory.NewInstance () as NWDCategory;
			return rReturn;
		}
//-------------------- 
		public override void InstanceInit ()
		{
			Name = new NWDLocalizableStringType(); // normal type
		}
//-------------------- 
    public static NWDCategory[] GetAllObjects()
     {
      List<NWDCategory> rReturn = new List<NWDCategory>();
     foreach (NWDCategory tObject in NWDCategory.ObjectsList)
      {
       if (tObject.Reference != null) {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 
    public static NWDCategory GetObjectWithReference(string sReference)
     {
     NWDCategory rReturn = null;
     foreach (NWDCategory tObject in NWDCategory.ObjectsList)
      {
       if (tObject.Reference == sReference) {
       rReturn = tObject;
       break;
      }
      }
      return rReturn;
    }
//-------------------- 

    public static NWDCategory[] GetObjectsWithReferences(string[] sReferences)
     {
      List<NWDCategory> rReturn = new List<NWDCategory>();
      foreach(string tReference in sReferences)
       {
         NWDCategory tObject = GetObjectWithReference(tReference);
         if (tObject!=null)
          {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 

    public static NWDCategory GetObjectWithInternalKey(string sInternalKey)
    {
     NWDCategory rReturn = null;
     foreach (NWDCategory tObject in NWDCategory.ObjectsList)
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

    public static NWDCategory[] GetObjectsWithInternalKeys(string[] sInternalKeys)
     {
      List<NWDCategory> rReturn = new List<NWDCategory>();
      foreach(string tInternalKey in sInternalKeys)
       {
         NWDCategory tObject = GetObjectWithInternalKey(tInternalKey);
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
     foreach (NWDCategory tObject in NWDCategory.ObjectsList)
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
