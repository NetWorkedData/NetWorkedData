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
	public struct NWDAccountConnexion
	{
		[SerializeField]
		public string Reference;
		public NWDAccount GetObject ()
		{
			return NWDAccount.GetObjectWithReference (Reference);
		}
		public void SetObject (NWDAccount sObject)
		{
			Reference = sObject.Reference;
		}
		public NWDAccount NewObject ()
		{
			NWDAccount tObject = NWDAccount.NewObject ();
			Reference = tObject.Reference;
			return tObject;
		}
	}
	//-------------------- 
	// CONNEXION METHODS
	//-------------------- 
	public class NWDAccountConnexionAttribut : PropertyAttribute
	{
		public bool ShowInspector = true;
		public bool Editable = false;
		public bool EditButton = true;
		public bool NewButton = true;
		public NWDAccountConnexionAttribut ()
		{
		}
		public NWDAccountConnexionAttribut (bool sShowInspector, bool sEditable, bool sEditButton, bool sNewButton)
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
	[CustomPropertyDrawer (typeof(NWDAccountConnexion))]
	public partial class NWDAccountConnexionDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			NWDAccountConnexionAttribut tReferenceConnexion = new NWDAccountConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDAccountConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDAccountConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDAccountConnexionAttribut), true)[0];
			}
			return NWDAccount.ReferenceConnexionHeightSerialized(property, tReferenceConnexion.ShowInspector);
		}
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			NWDAccountConnexionAttribut tReferenceConnexion = new NWDAccountConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDAccountConnexionAttribut), true).Length > 0)
				{
					tReferenceConnexion = (NWDAccountConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDAccountConnexionAttribut), true)[0];
				}
			NWDAccount.ReferenceConnexionFieldSerialized (position, property.displayName, property, "", tReferenceConnexion.ShowInspector, tReferenceConnexion.Editable, tReferenceConnexion.EditButton, tReferenceConnexion.NewButton);
		}
	}
	#endif
//-------------------- 
// GENERAL METHODS
//-------------------- 
	public partial class NWDAccount : NWDBasis <NWDAccount>
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
public static NWDAccount NewObject()
		{
			NWDAccount rReturn = NWDAccount.NewInstance () as NWDAccount;
			return rReturn;
		}
//-------------------- 
		public override void InstanceInit ()
		{
		}
//-------------------- 
    public static NWDAccount[] GetAllObjects()
     {
      List<NWDAccount> rReturn = new List<NWDAccount>();
     foreach (NWDAccount tObject in NWDAccount.ObjectsList)
      {
       if (tObject.Reference != null) {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 
    public static NWDAccount GetObjectWithReference(string sReference)
     {
     NWDAccount rReturn = null;
     foreach (NWDAccount tObject in NWDAccount.ObjectsList)
      {
       if (tObject.Reference == sReference) {
       rReturn = tObject;
       break;
      }
      }
      return rReturn;
    }
//-------------------- 

    public static NWDAccount[] GetObjectsWithReferences(string[] sReferences)
     {
      List<NWDAccount> rReturn = new List<NWDAccount>();
      foreach(string tReference in sReferences)
       {
         NWDAccount tObject = GetObjectWithReference(tReference);
         if (tObject!=null)
          {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 

    public static NWDAccount GetObjectWithInternalKey(string sInternalKey)
    {
     NWDAccount rReturn = null;
     foreach (NWDAccount tObject in NWDAccount.ObjectsList)
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

    public static NWDAccount[] GetObjectsWithInternalKeys(string[] sInternalKeys)
     {
      List<NWDAccount> rReturn = new List<NWDAccount>();
      foreach(string tInternalKey in sInternalKeys)
       {
         NWDAccount tObject = GetObjectWithInternalKey(tInternalKey);
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
     foreach (NWDAccount tObject in NWDAccount.ObjectsList)
      {
       tObject.ChangeUser(sOldUser, sNewUser);      }
    }
//-------------------- 
    public void ChangeUser(string sOldUser, string sNewUser)
    {
    }
//-------------------- 

//-------------------- 
// NICKNAME
//-------------------- 
//-------------------- 
// AVATAR
//-------------------- 
//-------------------- 
// SECRETKEY
//-------------------- 
//-------------------- 
// EMAIL
//-------------------- 
//-------------------- 
// PASSWORD
//-------------------- 
//-------------------- 
// APPLENOTIFICATIONTOKEN
//-------------------- 
//-------------------- 
// GOOGLENOTIFICATIONTOKEN
//-------------------- 
//-------------------- 
// FACEBOOKID
//-------------------- 
//-------------------- 
// GOOGLEID
//-------------------- 
//-------------------- 
// BAN
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
