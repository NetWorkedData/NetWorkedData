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
	public struct NWDLocalizationConnexion
	{
		[SerializeField]
		public string Reference;
		public NWDLocalization GetObject ()
		{
			return NWDLocalization.GetObjectWithReference (Reference);
		}
		public void SetObject (NWDLocalization sObject)
		{
			Reference = sObject.Reference;
		}
		public NWDLocalization NewObject ()
		{
			NWDLocalization tObject = NWDLocalization.NewObject ();
			Reference = tObject.Reference;
			return tObject;
		}
	}
	//-------------------- 
	// CONNEXION METHODS
	//-------------------- 
	public class NWDLocalizationConnexionAttribut : PropertyAttribute
	{
		public bool ShowInspector = true;
		public bool Editable = false;
		public bool EditButton = true;
		public bool NewButton = true;
		public NWDLocalizationConnexionAttribut ()
		{
		}
		public NWDLocalizationConnexionAttribut (bool sShowInspector, bool sEditable, bool sEditButton, bool sNewButton)
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
	[CustomPropertyDrawer (typeof(NWDLocalizationConnexion))]
	public partial class NWDLocalizationConnexionDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			NWDLocalizationConnexionAttribut tReferenceConnexion = new NWDLocalizationConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDLocalizationConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDLocalizationConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDLocalizationConnexionAttribut), true)[0];
			}
			return NWDLocalization.ReferenceConnexionHeightSerialized(property, tReferenceConnexion.ShowInspector);
		}
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			NWDLocalizationConnexionAttribut tReferenceConnexion = new NWDLocalizationConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDLocalizationConnexionAttribut), true).Length > 0)
				{
					tReferenceConnexion = (NWDLocalizationConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDLocalizationConnexionAttribut), true)[0];
				}
			NWDLocalization.ReferenceConnexionFieldSerialized (position, property.displayName, property, "", tReferenceConnexion.ShowInspector, tReferenceConnexion.Editable, tReferenceConnexion.EditButton, tReferenceConnexion.NewButton);
		}
	}
	#endif
//-------------------- 
// GENERAL METHODS
//-------------------- 
	public partial class NWDLocalization : NWDBasis <NWDLocalization>
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
public static NWDLocalization NewObject()
		{
			NWDLocalization rReturn = NWDLocalization.NewInstance () as NWDLocalization;
			return rReturn;
		}
//-------------------- 
		public override void InstanceInit ()
		{
			TextValue = new NWDLocalizableTextType(); // normal type
			AnnexeValue = new NWDMultiType(); // normal type
		}
//-------------------- 
    public static NWDLocalization[] GetAllObjects()
     {
      List<NWDLocalization> rReturn = new List<NWDLocalization>();
     foreach (NWDLocalization tObject in NWDLocalization.ObjectsList)
      {
       if (tObject.Reference != null) {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 
    public static NWDLocalization GetObjectWithReference(string sReference)
     {
     NWDLocalization rReturn = null;
     foreach (NWDLocalization tObject in NWDLocalization.ObjectsList)
      {
       if (tObject.Reference == sReference) {
       rReturn = tObject;
       break;
      }
      }
      return rReturn;
    }
//-------------------- 

    public static NWDLocalization[] GetObjectsWithReferences(string[] sReferences)
     {
      List<NWDLocalization> rReturn = new List<NWDLocalization>();
      foreach(string tReference in sReferences)
       {
         NWDLocalization tObject = GetObjectWithReference(tReference);
         if (tObject!=null)
          {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 

    public static NWDLocalization GetObjectWithInternalKey(string sInternalKey)
    {
     NWDLocalization rReturn = null;
     foreach (NWDLocalization tObject in NWDLocalization.ObjectsList)
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

    public static NWDLocalization[] GetObjectsWithInternalKeys(string[] sInternalKeys)
     {
      List<NWDLocalization> rReturn = new List<NWDLocalization>();
      foreach(string tInternalKey in sInternalKeys)
       {
         NWDLocalization tObject = GetObjectWithInternalKey(tInternalKey);
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
     foreach (NWDLocalization tObject in NWDLocalization.ObjectsList)
      {
       tObject.ChangeUser(sOldUser, sNewUser);      }
    }
//-------------------- 
    public void ChangeUser(string sOldUser, string sNewUser)
    {
    }
//-------------------- 

//-------------------- 
// TEXTVALUE
//-------------------- 
//-------------------- 
// ANNEXEVALUE
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
