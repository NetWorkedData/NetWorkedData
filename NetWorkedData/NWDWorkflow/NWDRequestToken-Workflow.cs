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
	public struct NWDRequestTokenConnexion
	{
		[SerializeField]
		public string Reference;
		public NWDRequestToken GetObject ()
		{
			return NWDRequestToken.GetObjectWithReference (Reference);
		}
		public void SetObject (NWDRequestToken sObject)
		{
			Reference = sObject.Reference;
		}
		public NWDRequestToken NewObject ()
		{
			NWDRequestToken tObject = NWDRequestToken.NewObject ();
			Reference = tObject.Reference;
			return tObject;
		}
	}
	//-------------------- 
	// CONNEXION METHODS
	//-------------------- 
	public class NWDRequestTokenConnexionAttribut : PropertyAttribute
	{
		public bool ShowInspector = true;
		public bool Editable = false;
		public bool EditButton = true;
		public bool NewButton = true;
		public NWDRequestTokenConnexionAttribut ()
		{
		}
		public NWDRequestTokenConnexionAttribut (bool sShowInspector, bool sEditable, bool sEditButton, bool sNewButton)
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
	[CustomPropertyDrawer (typeof(NWDRequestTokenConnexion))]
	public partial class NWDRequestTokenConnexionDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			NWDRequestTokenConnexionAttribut tReferenceConnexion = new NWDRequestTokenConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDRequestTokenConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDRequestTokenConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDRequestTokenConnexionAttribut), true)[0];
			}
			return NWDRequestToken.ReferenceConnexionHeightSerialized(property, tReferenceConnexion.ShowInspector);
		}
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			NWDRequestTokenConnexionAttribut tReferenceConnexion = new NWDRequestTokenConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDRequestTokenConnexionAttribut), true).Length > 0)
				{
					tReferenceConnexion = (NWDRequestTokenConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDRequestTokenConnexionAttribut), true)[0];
				}
			NWDRequestToken.ReferenceConnexionFieldSerialized (position, property.displayName, property, "", tReferenceConnexion.ShowInspector, tReferenceConnexion.Editable, tReferenceConnexion.EditButton, tReferenceConnexion.NewButton);
		}
	}
	#endif
//-------------------- 
// GENERAL METHODS
//-------------------- 
	public partial class NWDRequestToken : NWDBasis <NWDRequestToken>
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
public static NWDRequestToken NewObject()
		{
			NWDRequestToken rReturn = NWDRequestToken.NewInstance () as NWDRequestToken;
			return rReturn;
		}
//-------------------- 
		public override void InstanceInit ()
		{
			UUIDHash = new NWDReferenceHashType<NWDAccount>(); // generic type 
			AccountReferenceHash = new NWDReferenceHashType<NWDAccount>(); // generic type 
		}
//-------------------- 
    public static NWDRequestToken[] GetAllObjects()
     {
      List<NWDRequestToken> rReturn = new List<NWDRequestToken>();
     foreach (NWDRequestToken tObject in NWDRequestToken.ObjectsList)
      {
       if (tObject.Reference != null) {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 
    public static NWDRequestToken GetObjectWithReference(string sReference)
     {
     NWDRequestToken rReturn = null;
     foreach (NWDRequestToken tObject in NWDRequestToken.ObjectsList)
      {
       if (tObject.Reference == sReference) {
       rReturn = tObject;
       break;
      }
      }
      return rReturn;
    }
//-------------------- 

    public static NWDRequestToken[] GetObjectsWithReferences(string[] sReferences)
     {
      List<NWDRequestToken> rReturn = new List<NWDRequestToken>();
      foreach(string tReference in sReferences)
       {
         NWDRequestToken tObject = GetObjectWithReference(tReference);
         if (tObject!=null)
          {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 

    public static NWDRequestToken GetObjectWithInternalKey(string sInternalKey)
    {
     NWDRequestToken rReturn = null;
     foreach (NWDRequestToken tObject in NWDRequestToken.ObjectsList)
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

    public static NWDRequestToken[] GetObjectsWithInternalKeys(string[] sInternalKeys)
     {
      List<NWDRequestToken> rReturn = new List<NWDRequestToken>();
      foreach(string tInternalKey in sInternalKeys)
       {
         NWDRequestToken tObject = GetObjectWithInternalKey(tInternalKey);
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
     foreach (NWDRequestToken tObject in NWDRequestToken.ObjectsList)
      {
       tObject.ChangeUser(sOldUser, sNewUser);      }
    }
//-------------------- 
    public void ChangeUser(string sOldUser, string sNewUser)
    {
    }
//-------------------- 

//-------------------- 
// UUIDHASH
//-------------------- 
//-------------------- 
// ACCOUNTREFERENCEHASH
//-------------------- 
//-------------------- 
// TOKEN
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
