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
using UnityEditor.SceneManagement;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	public enum NWDSpotType
	{
		Laboratory_Element = 1,
        Laboratory_Ingredient = 2,
        Laboratory_Special = 4,

		Laboratory_Spirit = 3,

        Laboratory_Forge = 5,


		Shop_Store = 11,
		Shop_BonusZone = 12,
		Shop_Library = 13,
		Shop_HunterPlace = 14,
        Shop_AccessoriesStore = 15,

        Tavern = 20,

		Arena = 21,
		BarterPlace = 31,
		TradePlace = 41,
		Embassy = 51,
		Initial = 61,

        Portal = 62,
        SubPortal = 63,

        Teleport = 80,


        Arrival = 99, // arrival in the map (no return back)
	}
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("SPO")]
	[NWDClassDescriptionAttribute ("Spot descriptions Class")]
	[NWDClassMenuNameAttribute ("Spot")]
//	[NWDInternalKeyNotEditableAttribute]
	//-------------------------------------------------------------------------------------------------------------
    public partial class NWDSpot : NWDBasis<NWDSpot>
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
		[NWDGroupStart ("Spot attach to scene", true, true, true)]
		//[NWDHeaderAttribute("Position of Spot in game")]
        public string SceneName { get; set; }

		public NWDLocalizableStringType Name { get; set; }

		[NWDGroupEnd]
		[NWDGroupStart ("Position in hexagonal matrix ", true, true, true)]
		public int Column { get; set; }

		public int Line { get; set; }

		public int Level { get; set; }

		public int Rotation { get; set; }

		[NWDGroupEnd]

		[NWDSeparatorAttribute]

		[NWDGroupStart ("Type of Spot in game", true, true, true)]
//		[NWDEnumStringAttribute (new string[] {
//			"Laboratory/Element",
//			"Laboratory/Ingredient",
//			"Laboratory/Spirit",
//			"Laboratory/Special",
//			"Shop/Store",
//			"Shop/BonusZone",
//			"Shop/Library",
//			"Shop/HunterPlace",
//			"Shop/AccessoriesStore",
//			"Arena",
//			"BarterPlace",
//			"TradePlace",
//			"Embassy",
//			"Portal",
//		})]
//		public string SpotType { get; set; }

		public NWDSpotType Type { get; set; }

		[NWDEnumStringAttribute (new string[] {
			"01",
			"02",
			"03",
			"04",
			"05",
			"06",
			"07",
			"08",
			"09",
			"10",
			"11",
			"12",
			"13",
			"14",
			"15",
			"16",
			"17",
			"18",
			"19",
			"20",
			"21",
			"22",
			"23",
			"24",
			"25",
			"26",
		})]
		public string SpotTypeNumber { get; set; }
		[NWDIf ("Type", new int[] {
			(int)NWDSpotType.Shop_Store,
			(int)NWDSpotType.Shop_BonusZone,
			(int)NWDSpotType.Shop_Library,
			(int)NWDSpotType.Shop_HunterPlace,
			(int)NWDSpotType.Shop_AccessoriesStore,
		})]
		public NWDReferenceType<NWDShop> ShopReference { get; set; }
		[NWDIf ("Type", (int)NWDSpotType.BarterPlace)]
		public NWDReferenceType<NWDBarterPlace> BarterPlaceReference { get; set; }
		[NWDIf ("Type", (int)NWDSpotType.TradePlace)]
		public NWDReferenceType<NWDTradePlace> TradePlaceReference { get; set; }
		[NWDIf ("Type", (int)NWDSpotType.Arena)]
		public NWDReferenceType<NWDArena> ArenaReference { get; set; }
		[NWDIf ("Type", (int)NWDSpotType.Embassy)]
		public NWDReferenceType<NWDEmbassy> EmbassyReference { get; set; }

		[NWDHeaderAttribute ("State of spot")]
		public bool ActiveAtStart { get; set; }

		public bool SecretAtStart { get; set; }

		//public bool FinishAtStart { get; set; }

		//public bool PulseLocalisation { get; set; }

		[NWDGroupEnd]

		[NWDSeparatorAttribute]

		[NWDGroupStart ("Items to play and valide", true, true, true)]
		public NWDReferencesQuantityType<NWDItem> ItemsRequired { get; set; }

		public NWDReferencesQuantityType<NWDItem> ItemsForSuccess { get; set; }

		public NWDReferencesQuantityType<NWDItem> ItemsForSecret { get; set; }

		public NWDReferencesQuantityType<NWDItem> ItemsToFail { get; set; }

		[NWDIntSliderAttribute (0, 4)]
		public int ItemsAddFree { get; set; }

		[NWDIntSliderAttribute (0, 4)]
		public int ItemsAddPaidMax { get; set; }

		public string EngineDefinition { get; set; }

		[NWDGroupEnd]

		[NWDGroupStart ("Spots Connexion", true, true, true)]
		[NWDIf ("Type",new int[] {(int)NWDSpotType.Initial,(int)NWDSpotType.Portal})]
		public NWDReferenceType<NWDSpot> SpotDestinationReference { get; set; }

		public bool Connected { get; set; }

		[NWDIf ("Connected", true)]
		public NWDReferencesListType<NWDSpot> ConnectedSpotsList { get; set; }

		[NWDGroupEnd]

		[NWDSeparatorAttribute]

		[NWDGroupStart ("Socials networking", true, true, true)]
		public string FacebookURL { get; set; }

		public string TwitterURL { get; set; }

		[NWDGroupEnd]


		public bool inError { get; set; }

		//		private List<NWDSpot> _SpotsConnected = new List<NWDSpot>();
		//		private List<NWDSpot> _SpotsSecretConnected = new List<NWDSpot>();
		//-------------------------------------------------------------------------------------------------------------

		#endregion

		//-------------------------------------------------------------------------------------------------------------

		#region Constructors

		//-------------------------------------------------------------------------------------------------------------
		public NWDSpot ()
		{
			ActiveAtStart = false;
			SecretAtStart = false;
			//FinishAtStart = false;
			//PulseLocalisation = false;
			Type = NWDSpotType.Laboratory_Element;
			SpotTypeNumber = "01";
			ConnectedSpotsList = new NWDReferencesListType<NWDSpot> ();
			inError = false;
			Connected = true;
		}
		//-------------------------------------------------------------------------------------------------------------

		#endregion

		//-------------------------------------------------------------------------------------------------------------

		#region Class methods

		//-------------------------------------------------------------------------------------------------------------
		public static void MyClassMethod ()
		{
			// do something with this class
		}
		//-------------------------------------------------------------------------------------------------------------
//		public static NWDSpot CreateSpotAtColumnLine (string sSceneName, int sColumn, int sLine, int sLevel, int sRotation)
//		{
//			NWDSpot tSpot = NWDSpot.NewObject ();
//			Debug.Log ("sSceneName = " + sSceneName);
//			tSpot.SceneName = sSceneName;
//			tSpot.Column = sColumn;
//			tSpot.Line = sLine;
//			tSpot.Level = sLevel;
//			tSpot.Rotation = sRotation;
//			tSpot.SaveModifications ();
//			Debug.Log ("tSpot.InternalKey = " + tSpot.InternalKey);
//			Debug.Log ("tSpot.InternalDescription = " + tSpot.InternalDescription);
//			return tSpot;
//		}
		//-------------------------------------------------------------------------------------------------------------
//		public static string KeyForSpotAtColumnLine (string sSceneName, string sSpotType, int sColumn, int sLine)
//		{
//			string rKey = sSceneName + " " + sSpotType.Replace ("/", " ") + " " + sColumn.ToString ("0000") + "x" + sLine.ToString ("0000");
//			return rKey;
//		}
		//-------------------------------------------------------------------------------------------------------------
//		public static string DescriptionForSpotAtColumnLine (string sSceneName, int sColumn, int sLine)
//		{
//			string rDescription = sSceneName + "  Auto-generate NWDSpot for column " + sColumn.ToString () + " and line " + sLine.ToString ();
//			return rDescription;
//		}
		//-------------------------------------------------------------------------------------------------------------
		//        public static NWDSpot GetSpotAtColumnLine(string sSceneName,int sColumn, int sLine)
		//        {
		//            return NWDSpot.GetObjectByInternalKey(KeyForSpotAtColumnLine(sSceneName, sColumn, sLine));
		//		}
		//-------------------------------------------------------------------------------------------------------------

		#endregion

		//-------------------------------------------------------------------------------------------------------------

		#region Instance methods

		//-------------------------------------------------------------------------------------------------------------
		//        public NWDSpot ConnectWithSpotAtColumnLine(string sSceneName,int sColumn, int sLine)
		//        {
		//            NWDSpot rReturn = NWDSpot.GetObjectByInternalKey(KeyForSpotAtColumnLine(sSceneName, sColumn, sLine));
		//            if (rReturn != null)
		//            {
		//                rReturn.ConnectedSpotsList.AddObject(this);
		//                rReturn.SaveModificationsIfModified();
		//            }
		//            return rReturn;
		//        }
		//-------------------------------------------------------------------------------------------------------------
		//        public void SpotAnalyze(int sColumn, int sLine, int sLevel, int sRotation, HexagonalTileOrientation sOrientation)
		//        {
		//#if UNITY_EDITOR
		//            /*if (Connected == true)
		//            {
		//                ConnectedSpotsList = new NWDReferencesListType<NWDSpot>();
		//                if (sOrientation == HexagonalTileOrientation.Portrait)
		//                {
		//                    ConnectedSpotsList.AddObject(ConnectWithSpotAtColumnLine(EditorSceneManager.GetActiveScene().name, sColumn, sLine + 1));
		//                    ConnectedSpotsList.AddObject(ConnectWithSpotAtColumnLine(EditorSceneManager.GetActiveScene().name, sColumn, sLine - 1));
		//                    ConnectedSpotsList.AddObject(ConnectWithSpotAtColumnLine(EditorSceneManager.GetActiveScene().name, sColumn - 1, sLine));
		//                    ConnectedSpotsList.AddObject(ConnectWithSpotAtColumnLine(EditorSceneManager.GetActiveScene().name, sColumn + 1, sLine));
		//                    if (sColumn % 2 == 0)
		//                    {
		//                        ConnectedSpotsList.AddObject(ConnectWithSpotAtColumnLine(EditorSceneManager.GetActiveScene().name, sColumn - 1, sLine - 1));
		//                        ConnectedSpotsList.AddObject(ConnectWithSpotAtColumnLine(EditorSceneManager.GetActiveScene().name, sColumn + 1, sLine - 1));
		//                    }
		//                    else
		//                    {
		//                        ConnectedSpotsList.AddObject(ConnectWithSpotAtColumnLine(EditorSceneManager.GetActiveScene().name, sColumn - 1, sLine + 1));
		//                        ConnectedSpotsList.AddObject(ConnectWithSpotAtColumnLine(EditorSceneManager.GetActiveScene().name, sColumn + 1, sLine + 1));
		//                    }
		//                }
		//                else if (sOrientation == HexagonalTileOrientation.Landscape)
		//                {
		//                    ConnectedSpotsList.AddObject(ConnectWithSpotAtColumnLine(EditorSceneManager.GetActiveScene().name, sColumn, sLine + 1));
		//                    ConnectedSpotsList.AddObject(ConnectWithSpotAtColumnLine(EditorSceneManager.GetActiveScene().name, sColumn, sLine - 1));
		//                    ConnectedSpotsList.AddObject(ConnectWithSpotAtColumnLine(EditorSceneManager.GetActiveScene().name, sColumn - 1, sLine));
		//                    ConnectedSpotsList.AddObject(ConnectWithSpotAtColumnLine(EditorSceneManager.GetActiveScene().name, sColumn + 1, sLine));
		//                    if (sLine % 2 == 0)
		//                    {
		//                        ConnectedSpotsList.AddObject(ConnectWithSpotAtColumnLine(EditorSceneManager.GetActiveScene().name, sColumn - 1, sLine - 1));
		//                        ConnectedSpotsList.AddObject(ConnectWithSpotAtColumnLine(EditorSceneManager.GetActiveScene().name, sColumn - 1, sLine + 1));
		//                    }
		//                    else
		//                    {
		//                        ConnectedSpotsList.AddObject(ConnectWithSpotAtColumnLine(EditorSceneManager.GetActiveScene().name, sColumn + 1, sLine - 1));
		//                        ConnectedSpotsList.AddObject(ConnectWithSpotAtColumnLine(EditorSceneManager.GetActiveScene().name, sColumn + 1, sLine + 1));
		//                    }
		//                }
		//                SaveModificationsIfModified();
		//            }
		//#endif
		//*/
		//        }
		//-------------------------------------------------------------------------------------------------------------
		public void MyInstanceMethod ()
		{
			// do something with this object
		}
		//-------------------------------------------------------------------------------------------------------------
		//		public void AddSpotFromSpotScript(SpotScript sSpotScript)
		//		{
		//			Debug.Log ("AddSpotFromSpotScript");
		//			if (sSpotScript != null) {
		//				AddSpot(sSpotScript.SpotReference.GetObject ());
		//			}
		//		}
		//		//-------------------------------------------------------------------------------------------------------------
		//		public void AddSpot(NWDSpot sSpot)
		//		{
		//			Debug.Log ("AddSpot");
		//			if (_SpotsConnected == null) {
		//				_SpotsConnected = new List<NWDSpot>();
		//			}
		//			if (_SpotsSecretConnected == null) {
		//				_SpotsSecretConnected = new List<NWDSpot>();
		//			}
		//			if (sSpot != null) {
		//				Debug.Log ("AddSpot ADDED "+sSpot.Reference);
		//				if (sSpot.SecretAtStart == false) {
		//					_SpotsConnected.Add (sSpot);
		//				} else {
		//					_SpotsSecretConnected.Add (sSpot);
		//				}
		//			}
		//		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDSpot[] GetSpots ()
		{
			List<NWDSpot> tList = new List<NWDSpot> ();
			foreach (NWDSpot tSpot in ConnectedSpotsList.GetObjects()) {
				if (tSpot.SecretAtStart == false) {
					tList.Add (tSpot);
				}
			}
			return tList.ToArray ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDSpot[] GetSecretSpots ()
		{
			List<NWDSpot> tList = new List<NWDSpot> ();
			foreach (NWDSpot tSpot in ConnectedSpotsList.GetObjects()) {
				if (tSpot.SecretAtStart == true) {
					tList.Add (tSpot);
				}
			}
			return tList.ToArray ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public void GameStart ()
		{
			Play ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public void GameOver (NWDReferencesQuantityType<NWDItem> sItemsQuantities = null)
		{
			// I need to compare element in gameOver and elements in the conditions of win
			if (ItemsToFail.IsNotEmpty () && sItemsQuantities.ContainsReferencesQuantity (ItemsToFail)) {
				// force to fail :-(
				Fail ();
			} else if (ItemsForSecret.IsNotEmpty () && sItemsQuantities.ContainsReferencesQuantity (ItemsForSecret)) {
				// secret success GOOOOOOD :-D
				SecretSuccess ();
			} else if (ItemsForSuccess.IsNotEmpty () && sItemsQuantities.ContainsReferencesQuantity (ItemsForSuccess)) {
				// Success YESSS :-)
				Success ();
			} else {
				// ... not pertinent :-/
				// I push 'finished'? 'success'? 'fail'?
				Fail ();
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public void FakeSuccess ()
		{
			Debug.Log ("FakeSuccess");
			Success ();
		}
		//-------------------------------------------------------------------------------------------------------------
		private void Success ()
		{
			NWDSpotUser tSpotUser = NWDSpotUser.SpotUserForSpot (this);
            tSpotUser.NextState = SOBSpotState.Success;
            tSpotUser.NextColor = SOBSpotColor.Success;
			tSpotUser.FinishCounter++;
			tSpotUser.SuccessCounter++;
			tSpotUser.SaveModifications ();
			// active success spots
			foreach (NWDSpot tSpot in GetSpots()) {
                NWDSpotUser tNewSpotUser = NWDSpotUser.SpotUserForSpot (tSpot);
                if (tNewSpotUser.ActualState == SOBSpotState.Close)
                {
                    tNewSpotUser.NextState = SOBSpotState.Activation;
                    tNewSpotUser.NextColor = SOBSpotColor.Open;
                    tNewSpotUser.SaveModifications();
                }
			}
            // active destination spots
			NWDSpot tScriptDestination = SpotDestinationReference.GetObject ();
			if (tScriptDestination != null) {
                NWDSpotUser tNewSpotUser = NWDSpotUser.SpotUserForSpot (tScriptDestination);
                if (tNewSpotUser.ActualState == SOBSpotState.Close)
                {
                    tNewSpotUser.NextState = SOBSpotState.Open;
                    tNewSpotUser.NextColor = SOBSpotColor.Open;
                    tNewSpotUser.SaveModifications();
                }
				if (tScriptDestination.Type == NWDSpotType.Portal) {
					// active the spot connected to the portal
					foreach (NWDSpot tSpot in tScriptDestination.GetSpots()) {
                        NWDSpotUser tNewSubSpotUser = NWDSpotUser.SpotUserForSpot (tSpot);
                        if (tNewSubSpotUser.ActualState == SOBSpotState.Close)
                        {
                            tNewSubSpotUser.NextState = SOBSpotState.Activation;
                            tNewSubSpotUser.NextColor = SOBSpotColor.Open;
                            tNewSubSpotUser.SaveModifications();
                        }
					}
				}
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		private void Play ()
		{
			NWDSpotUser tSpotUser = NWDSpotUser.SpotUserForSpot (this);
			tSpotUser.PlayCounter++;
			tSpotUser.SaveModifications ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public void FakeSecretSuccess ()
		{
			SecretSuccess ();
		}
		//-------------------------------------------------------------------------------------------------------------
		private void SecretSuccess ()
		{
			NWDSpotUser tSpotUser = NWDSpotUser.SpotUserForSpot (this);
            tSpotUser.NextState = SOBSpotState.SecretSuccess;
            tSpotUser.NextColor = SOBSpotColor.SecretSuccess;
			tSpotUser.FinishCounter++;
			tSpotUser.SecretCounter++;
			tSpotUser.SaveModifications ();
			foreach (NWDSpot tSpot in GetSecretSpots()) {
				NWDSpotUser tNewSpotUser = NWDSpotUser.SpotUserForSpot (tSpot);
                if (tNewSpotUser.ActualState == SOBSpotState.Close)
                {
                    tNewSpotUser.NextState = SOBSpotState.SecretActivation;
                    tNewSpotUser.SaveModifications();
                }
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public void FakeFail ()
		{
			Fail ();
		}
		//-------------------------------------------------------------------------------------------------------------
		private void Fail ()
		{
			NWDSpotUser tSpotUser = NWDSpotUser.SpotUserForSpot (this);
            tSpotUser.NextState = SOBSpotState.Fail;
            tSpotUser.NextColor = SOBSpotColor.Fail;
			tSpotUser.FinishCounter++;
			tSpotUser.FailCounter++;
            tSpotUser.SaveModifications ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public void Cancel ()
		{
			NWDSpotUser tSpotUser = NWDSpotUser.SpotUserForSpot (this);
            tSpotUser.NextState = SOBSpotState.Cancel;
            tSpotUser.NextColor = SOBSpotColor.Cancel;
			tSpotUser.CancelCounter++;
			tSpotUser.SaveModifications ();
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
		public override bool AddonEdited (bool sNeedBeUpdate)
		{
			if (sNeedBeUpdate == true) {
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
	#region Connexion NWDSpot with Unity MonoBehavior
	//-------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// NWDSpot connexion.
	/// In your MonoBehaviour Script connect object with :
	/// <code>
	///	[NWDConnexionAttribut(true,true, true, true)]
	/// public NWDSpotConnexion MyNWDSpotObject;
	/// </code>
	/// </summary>
	//-------------------------------------------------------------------------------------------------------------
	// CONNEXION STRUCTURE METHODS
	//-------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class NWDSpotConnexion
	{
		//-------------------------------------------------------------------------------------------------------------
		[SerializeField]
		public string Reference;
		//-------------------------------------------------------------------------------------------------------------
		public NWDSpot GetObject ()
		{
			return NWDSpot.GetObjectByReference (Reference);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetObject (NWDSpot sObject)
		{
			if (sObject != null) {
				Reference = sObject.Reference;
			} else {
				Reference = "";
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDSpot NewObject ()
		{
			NWDSpot tObject = NWDSpot.NewObject ();
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
	[CustomPropertyDrawer (typeof(NWDSpotConnexion))]
	public class NWDSpotConnexionDrawer : PropertyDrawer
	{
		//-------------------------------------------------------------------------------------------------------------
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			NWDConnexionAttribut tReferenceConnexion = new NWDConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true).Length > 0) {
				tReferenceConnexion = (NWDConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true) [0];
			}
			return NWDSpot.ReferenceConnexionHeightSerialized (property, tReferenceConnexion.ShowInspector);
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			NWDConnexionAttribut tReferenceConnexion = new NWDConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true).Length > 0) {
				tReferenceConnexion = (NWDConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true) [0];
			}
			NWDSpot.ReferenceConnexionFieldSerialized (position, property.displayName, property, "", tReferenceConnexion.ShowInspector, tReferenceConnexion.Editable, tReferenceConnexion.EditButton, tReferenceConnexion.NewButton);
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