//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

using SQLite4Unity3d;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{

    //-------------------------------------------------------------------------------------------------------------
    [Serializable]
    public enum SOBSpotState
    {
        Close,
        Activation,  // => go to open
        SecretActivation, // => Go to Open

        Success, // => Go to Open
        SecretSuccess, // => Go to Open
        Fail, // => Go to Open
        Cancel, // => Go to Open

        Open,
    }
    //-------------------------------------------------------------------------------------------------------------
    [Serializable]
    public enum SOBSpotColor
    {
        Close,
        Open,
        Success,
        SecretSuccess,
        Fail,
        Cancel,
    }

	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("SPU")]
	[NWDClassDescriptionAttribute ("Spot's User descriptions Class")]
	[NWDClassMenuNameAttribute ("Spot's User")]
	[NWDInternalKeyNotEditableAttribute]
	//-------------------------------------------------------------------------------------------------------------
	public partial class  NWDSpotUser :NWDBasis <NWDSpotUser>
	{
		//-------------------------------------------------------------------------------------------------------------
		//#warning YOU MUST FOLLOW THIS INSTRUCTIONS
		//-------------------------------------------------------------------------------------------------------------
		// YOU MUST GENERATE PHP FOR THIS CLASS AFTER FIELD THIS CLASS WITH YOUR PROPERTIES
		// YOU MUST GENERATE WEBSITE AND UPLOAD THE FOLDER ON YOUR SERVER
		// YOU MUST UPDATE TABLE ON THE SERVER WITH THE MENU FOR DEV, FOR PREPROD AND FOR PROD
		//-------------------------------------------------------------------------------------------------------------
		#region Properties
		//-------------------------------------------------------------------------------------------------------------
		// Your properties
		public NWDReferenceType<NWDAccount> AccountReference { get; set; }
		public NWDReferenceType<NWDSpot> SpotReference { get; set; }
		[NWDHeaderAttribute("Spot Activity")]
		//public bool SpotOpen { get; set; } // spot is playable
        public SOBSpotState ActualState { get; set; } // can use to compare with the next state to play animation and change color
        public SOBSpotState NextState { get; set; }
        public SOBSpotColor ActualColor { get; set; } // can use to compare with the next state to play animation and change color
        public SOBSpotColor NextColor { get; set; }
        public bool FirstOpening { get; set; } // spot is open for the first time
        public bool Pulse { get; set; } // pulse localisation or not ?

		[NWDHeaderAttribute("Last Activity")]
		//public bool Finish { get; set; } // game was finish last time
		//public bool Cancel { get; set; } // game was cancel last time
		//public bool Success { get; set; } // game was successed last time
		//public bool Secret { get; set; } // game was secret successed last time
		//public bool Fail { get; set; } // game was failed last time

		[NWDHeaderAttribute("Partie counter")]
		public int PlayCounter { get; set; }

		[NWDHeaderAttribute("Analytics")]
		public int FinishCounter { get; set; }
		public int CancelCounter { get; set; }
		public int SuccessCounter { get; set; }
		public int SecretCounter { get; set; }
		public int FailCounter { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Constructors
		//-------------------------------------------------------------------------------------------------------------
		public NWDSpotUser()
		{
			//Init your instance here
			FirstOpening = true;
			ActualState = SOBSpotState.Close;
            NextState = SOBSpotState.Close;
            ActualColor = SOBSpotColor.Close;
            NextColor = SOBSpotColor.Close;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Class methods
		//-------------------------------------------------------------------------------------------------------------
		public static NWDSpotUser SpotUserForSpot (NWDSpot sSpot)
		{
			NWDSpotUser rSpotUser = null;
            if (sSpot != null)
            {
                foreach (NWDSpotUser tSpotUser in NWDSpotUser.GetAllObjects())
                {
                    if (tSpotUser.SpotReference.GetReference() == sSpot.Reference)
                    {
                        rSpotUser = tSpotUser;
                        break;
                    }
                }
                if (rSpotUser == null)
                {
                    rSpotUser = NWDSpotUser.NewObject();
                    rSpotUser.SpotReference.SetObject(sSpot);
                    rSpotUser.SaveModifications();
                }
            }
			return rSpotUser;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static bool SpotUserForSpotExists (NWDSpot sSpot)
		{
			NWDSpotUser rSpotUser = null;
			if (sSpot != null) {
				foreach (NWDSpotUser tSpotUser in NWDSpotUser.GetAllObjects()) {
					if (tSpotUser.SpotReference.GetReference () == sSpot.Reference) {
						rSpotUser = tSpotUser;
						break;
					}
				}
			}
			return rSpotUser != null;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Instance methods
		//-------------------------------------------------------------------------------------------------------------
		public void MyInstanceMethod ()
		{
			// do something with this object
		}
		//-------------------------------------------------------------------------------------------------------------
		#region override of NetWorkedData addons methods
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonInsertMe ()
		{
			// do something when object will be inserted
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonUpdateMe ()
		{
			// do something when object will be updated
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonUpdatedMe ()
		{
			// do something when object finish to be updated
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonDuplicateMe ()
		{
			// do something when object will be dupplicate
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonEnableMe ()
		{
			// do something when object will be enabled
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonDisableMe ()
		{
			// do something when object will be disabled
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonTrashMe ()
		{
			// do something when object will be put in trash
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonUnTrashMe ()
		{
			// do something when object will be remove from trash
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		//Addons for Edition
		//-------------------------------------------------------------------------------------------------------------
		public override bool AddonEdited( bool sNeedBeUpdate)
		{
			if (sNeedBeUpdate == true) 
			{
				// do something
			}
			return sNeedBeUpdate;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override float AddonEditor (Rect sInRect)
		{
			// Draw the interface addon for editor
			float tYadd = 0.0f;
			return tYadd;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override float AddonEditorHeight ()
		{
			// Height calculate for the interface addon for editor
			float tYadd = 0.0f;
			return tYadd;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
	}

	//-------------------------------------------------------------------------------------------------------------
	#region Connexion NWDSpotUser with Unity MonoBehavior
	//-------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// NWDSpotUser connexion.
	/// In your MonoBehaviour Script connect object with :
	/// <code>
	///	[NWDConnexionAttribut(true,true, true, true)]
	/// public NWDSpotUserConnexion MyNWDSpotUserObject;
	/// </code>
	/// </summary>
	//-------------------------------------------------------------------------------------------------------------
	// CONNEXION STRUCTURE METHODS
	//-------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class NWDSpotUserConnexion
	{
		//-------------------------------------------------------------------------------------------------------------
		[SerializeField]
		public string Reference;
		//-------------------------------------------------------------------------------------------------------------
		public NWDSpotUser GetObject ()
		{
			return NWDSpotUser.GetObjectByReference (Reference);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetObject (NWDSpotUser sObject)
		{
			if (sObject != null) {
				Reference = sObject.Reference;
			} else {
				Reference = "";
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDSpotUser NewObject ()
		{
			NWDSpotUser tObject = NWDSpotUser.NewObject ();
			Reference = tObject.Reference;
			return tObject;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//-------------------------------------------------------------------------------------------------------------
	// CUSTOM PROPERTY DRAWER METHODS
	//-------------------------------------------------------------------------------------------------------------
	#if UNITY_EDITOR
	//-------------------------------------------------------------------------------------------------------------
	[CustomPropertyDrawer (typeof(NWDSpotUserConnexion))]
	public class NWDSpotUserConnexionDrawer : PropertyDrawer
	{
		//-------------------------------------------------------------------------------------------------------------
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			NWDConnexionAttribut tReferenceConnexion = new NWDConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true)[0];
			}
			return NWDSpotUser.ReferenceConnexionHeightSerialized(property, tReferenceConnexion.ShowInspector);
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			NWDConnexionAttribut tReferenceConnexion = new NWDConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true)[0];
			}
			NWDSpotUser.ReferenceConnexionFieldSerialized (position, property.displayName, property, "", tReferenceConnexion.ShowInspector, tReferenceConnexion.Editable, tReferenceConnexion.EditButton, tReferenceConnexion.NewButton);
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//-------------------------------------------------------------------------------------------------------------
	#endif
	//-------------------------------------------------------------------------------------------------------------
	#endregion
	//-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================