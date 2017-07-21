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
	public struct NWDWorldConnexion
	{
		[SerializeField]
		public string Reference;
		public NWDWorld GetObject ()
		{
			return NWDWorld.GetObjectWithReference (Reference);
		}
		public void SetObject (NWDWorld sObject)
		{
			Reference = sObject.Reference;
		}
		public NWDWorld NewObject ()
		{
			NWDWorld tObject = NWDWorld.NewObject ();
			Reference = tObject.Reference;
			return tObject;
		}
	}
	//-------------------- 
	// CONNEXION METHODS
	//-------------------- 
	public class NWDWorldConnexionAttribut : PropertyAttribute
	{
		public bool ShowInspector = true;
		public bool Editable = false;
		public bool EditButton = true;
		public bool NewButton = true;
		public NWDWorldConnexionAttribut ()
		{
		}
		public NWDWorldConnexionAttribut (bool sShowInspector, bool sEditable, bool sEditButton, bool sNewButton)
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
	[CustomPropertyDrawer (typeof(NWDWorldConnexion))]
	public partial class NWDWorldConnexionDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			NWDWorldConnexionAttribut tReferenceConnexion = new NWDWorldConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDWorldConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDWorldConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDWorldConnexionAttribut), true)[0];
			}
			return NWDWorld.ReferenceConnexionHeightSerialized(property, tReferenceConnexion.ShowInspector);
		}
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			NWDWorldConnexionAttribut tReferenceConnexion = new NWDWorldConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDWorldConnexionAttribut), true).Length > 0)
				{
					tReferenceConnexion = (NWDWorldConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDWorldConnexionAttribut), true)[0];
				}
			NWDWorld.ReferenceConnexionFieldSerialized (position, property.displayName, property, "", tReferenceConnexion.ShowInspector, tReferenceConnexion.Editable, tReferenceConnexion.EditButton, tReferenceConnexion.NewButton);
		}
	}
	#endif
//-------------------- 
// GENERAL METHODS
//-------------------- 
	public partial class NWDWorld : NWDBasis <NWDWorld>
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
public static NWDWorld NewObject()
		{
			NWDWorld rReturn = NWDWorld.NewInstance () as NWDWorld;
			return rReturn;
		}
//-------------------- 
		public override void InstanceInit ()
		{
			Name = new NWDLocalizableStringType(); // normal type
			SubName = new NWDLocalizableStringType(); // normal type
			Description = new NWDLocalizableStringType(); // normal type
			PrimaryColor = new NWDColorType(); // normal type
			SecondaryColor = new NWDColorType(); // normal type
			TertiaryColor = new NWDColorType(); // normal type
			Categories = new NWDReferencesListType<NWDCategory>(); // generic type 
			Families = new NWDReferencesListType<NWDFamily>(); // generic type 
			Keywords = new NWDReferencesListType<NWDKeyword>(); // generic type 
		}
//-------------------- 
    public static NWDWorld[] GetAllObjects()
     {
      List<NWDWorld> rReturn = new List<NWDWorld>();
     foreach (NWDWorld tObject in NWDWorld.ObjectsList)
      {
       if (tObject.Reference != null) {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 
    public static NWDWorld GetObjectWithReference(string sReference)
     {
     NWDWorld rReturn = null;
     foreach (NWDWorld tObject in NWDWorld.ObjectsList)
      {
       if (tObject.Reference == sReference) {
       rReturn = tObject;
       break;
      }
      }
      return rReturn;
    }
//-------------------- 

    public static NWDWorld[] GetObjectsWithReferences(string[] sReferences)
     {
      List<NWDWorld> rReturn = new List<NWDWorld>();
      foreach(string tReference in sReferences)
       {
         NWDWorld tObject = GetObjectWithReference(tReference);
         if (tObject!=null)
          {
           rReturn.Add(tObject);
          }
       }
      return rReturn.ToArray();
    }
//-------------------- 

    public static NWDWorld GetObjectWithInternalKey(string sInternalKey)
    {
     NWDWorld rReturn = null;
     foreach (NWDWorld tObject in NWDWorld.ObjectsList)
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

    public static NWDWorld[] GetObjectsWithInternalKeys(string[] sInternalKeys)
     {
      List<NWDWorld> rReturn = new List<NWDWorld>();
      foreach(string tInternalKey in sInternalKeys)
       {
         NWDWorld tObject = GetObjectWithInternalKey(tInternalKey);
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
     foreach (NWDWorld tObject in NWDWorld.ObjectsList)
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
// SUBNAME
//-------------------- 
//-------------------- 
// DESCRIPTION
//-------------------- 
//-------------------- 
// PRIMARYCOLOR
//-------------------- 
//-------------------- 
// SECONDARYCOLOR
//-------------------- 
//-------------------- 
// TERTIARYCOLOR
//-------------------- 
//-------------------- 
// KIND
//-------------------- 
//-------------------- 
// CATEGORIES
//-------------------- 
    public NWDCategory[] CategoriesObjects()
     {
      string[] tReferencesArray = Categories.Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
      NWDCategory[] rReturn = NWDCategory.GetObjectsWithReferences(tReferencesArray);
      return rReturn;
     }

    static public NWDWorld[] ObjectsLinkedWithCategories(NWDCategory sConnexion)
     {
      List<NWDWorld> rReturn = new List<NWDWorld>();
      // I need to find the reference in the DataBase .... or in objects ... 
     foreach (NWDWorld tObject in NWDWorld.ObjectsList)
      {
       if (tObject.Categories.Value.Contains(sConnexion.Reference)) {
       NWDWorld tTryObject = GetObjectWithReference(tObject.Reference);
       if (tTryObject!=null)
        {
         rReturn.Add(tTryObject);
        }
      }
      }
      return rReturn.ToArray();
     }

//-------------------- 
// FAMILIES
//-------------------- 
    public NWDFamily[] FamiliesObjects()
     {
      string[] tReferencesArray = Families.Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
      NWDFamily[] rReturn = NWDFamily.GetObjectsWithReferences(tReferencesArray);
      return rReturn;
     }

    static public NWDWorld[] ObjectsLinkedWithFamilies(NWDFamily sConnexion)
     {
      List<NWDWorld> rReturn = new List<NWDWorld>();
      // I need to find the reference in the DataBase .... or in objects ... 
     foreach (NWDWorld tObject in NWDWorld.ObjectsList)
      {
       if (tObject.Families.Value.Contains(sConnexion.Reference)) {
       NWDWorld tTryObject = GetObjectWithReference(tObject.Reference);
       if (tTryObject!=null)
        {
         rReturn.Add(tTryObject);
        }
      }
      }
      return rReturn.ToArray();
     }

//-------------------- 
// KEYWORDS
//-------------------- 
    public NWDKeyword[] KeywordsObjects()
     {
      string[] tReferencesArray = Keywords.Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
      NWDKeyword[] rReturn = NWDKeyword.GetObjectsWithReferences(tReferencesArray);
      return rReturn;
     }

    static public NWDWorld[] ObjectsLinkedWithKeywords(NWDKeyword sConnexion)
     {
      List<NWDWorld> rReturn = new List<NWDWorld>();
      // I need to find the reference in the DataBase .... or in objects ... 
     foreach (NWDWorld tObject in NWDWorld.ObjectsList)
      {
       if (tObject.Keywords.Value.Contains(sConnexion.Reference)) {
       NWDWorld tTryObject = GetObjectWithReference(tObject.Reference);
       if (tTryObject!=null)
        {
         rReturn.Add(tTryObject);
        }
      }
      }
      return rReturn.ToArray();
     }

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
