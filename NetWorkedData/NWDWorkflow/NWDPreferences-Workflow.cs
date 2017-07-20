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
	public struct NWDPreferencesConnexion
	{
		[SerializeField]
		public string Reference;
		public NWDPreferences GetObject ()
		{
			return NWDPreferences.GetObjectWithReference (Reference);
		}
		public void SetObject (NWDPreferences sObject)
		{
			Reference = sObject.Reference;
		}
		public NWDPreferences NewObject ()
		{
			NWDPreferences tObject = NWDPreferences.NewObject ();
			Reference = tObject.Reference;
			return tObject;
		}
	}
//-------------------- 
// GENERAL METHODS
//-------------------- 
	public partial class NWDPreferences : NWDBasis <NWDPreferences>
	{
//-------------------- 
		public override bool IsAccountDependent ()
		{
			return true;
		}
//-------------------- 
		public override bool IsAccountConnected (string sAccountReference)
		{
			bool rReturn = false;
			if (AccountReference.Value == NWDAppConfiguration.SharedInstance.SelectedEnvironment().PlayerAccountReference) {
					rReturn = true;
			}
			return rReturn;
		}
		public override string NewReference ()
		{
			return NewReferenceFromUUID(NWDAppConfiguration.SharedInstance.SelectedEnvironment().PlayerAccountReference);
		}
//-------------------- 
		public override bool IsLockedObject () // return true during the player game if this object cannot be modified by player
		{
			#if UNITY_EDITOR
			return false;
			#else
			return false;
			#endif
		}
//-------------------- 
public static NWDPreferences NewObject()
		{
			NWDPreferences rReturn = NWDPreferences.NewInstance () as NWDPreferences;
			NWDReferenceType<NWDAccount> tAccountReference = new NWDReferenceType<NWDAccount> ();
			tAccountReference.SetReference (NWDAppConfiguration.SharedInstance.SelectedEnvironment ().PlayerAccountReference);
			rReturn.AccountReference = tAccountReference;
			rReturn.UpdateMeLater ();			return rReturn;
		}
//-------------------- 
		public override void InstanceInit ()
		{
			AccountReference = new NWDReferenceType<NWDAccount>(); // generic type 
			Value = new NWDMultiType(); // normal type
		}
//-------------------- 
    public static NWDPreferences[] GetAllObjects()
     {
      List<NWDPreferences> rReturn = new List<NWDPreferences>();
     foreach (NWDPreferences tObject in NWDPreferences.ObjectsList)
      {
       if (tObject.Reference != null && (tObject.AccountReference.Value == NWDAppConfiguration.SharedInstance.SelectedEnvironment().PlayerAccountReference) ) {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 
    public static NWDPreferences GetObjectWithReference(string sReference)
     {
     NWDPreferences rReturn = null;
     foreach (NWDPreferences tObject in NWDPreferences.ObjectsList)
      {
       if (tObject.Reference == sReference && (tObject.AccountReference.Value == NWDAppConfiguration.SharedInstance.SelectedEnvironment().PlayerAccountReference) ) {
       rReturn = tObject;
       break;
      }
      }
      return rReturn;
    }
//-------------------- 

    public static NWDPreferences[] GetObjectsWithReferences(string[] sReferences)
     {
      List<NWDPreferences> rReturn = new List<NWDPreferences>();
      foreach(string tReference in sReferences)
       {
         NWDPreferences tObject = GetObjectWithReference(tReference);
         if (tObject!=null)
          {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 

    public static NWDPreferences GetObjectWithInternalKey(string sInternalKey)
    {
     NWDPreferences rReturn = null;
     foreach (NWDPreferences tObject in NWDPreferences.ObjectsList)
      {
       if (tObject.InternalKey == sInternalKey && (tObject.AccountReference.Value == NWDAppConfiguration.SharedInstance.SelectedEnvironment().PlayerAccountReference) )
       {
         rReturn = tObject;
         break;
       }
      }
     return rReturn;
    }
//-------------------- 

    public static NWDPreferences[] GetObjectsWithInternalKeys(string[] sInternalKeys)
     {
      List<NWDPreferences> rReturn = new List<NWDPreferences>();
      foreach(string tInternalKey in sInternalKeys)
       {
         NWDPreferences tObject = GetObjectWithInternalKey(tInternalKey);
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
     foreach (NWDPreferences tObject in NWDPreferences.ObjectsList)
      {
       tObject.ChangeUser(sOldUser, sNewUser);      }
    }
//-------------------- 
    public void ChangeUser(string sOldUser, string sNewUser)
    {
     // TO DO CHANGE USER IF USER EXISTS IN 
     if (AccountReference.Value == sOldUser)
      {
        AccountReference.SetReference(sNewUser);
      }
     // If user exist In need to change the Reference To by simple replace the usersequence by the new user sequence (the xxxxxxT by xxxxxxxS)
     UpdateReference(sOldUser, sNewUser);
     UpdateMe();
    }
//-------------------- 

//-------------------- 
// ACCOUNTREFERENCE
//-------------------- 
    public NWDAccount AccountReferenceObject()
     {
      NWDAccount rReturn = NWDAccount.GetObjectByReference(this.AccountReference.Value) as NWDAccount;
      return rReturn;
     }

    static public NWDPreferences[] ObjectLinkedWithAccountReference(NWDAccount sConnexion)
     {
      List<NWDPreferences> rReturn = new List<NWDPreferences>();
      // I need to find the reference in the DataBase .... or in objects ... 
     foreach (NWDPreferences tObject in NWDPreferences.ObjectsList)
      {
       if (tObject.AccountReference.ContainsReference(sConnexion.Reference)) {
       NWDPreferences tTryObject = GetObjectWithReference(tObject.Reference);
       if (tTryObject!=null)
        {
         rReturn.Add(tTryObject);
        }
      }
      }
      return rReturn.ToArray();
     }

//-------------------- 
// VALUE
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
