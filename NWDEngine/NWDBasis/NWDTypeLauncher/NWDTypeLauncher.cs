//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

using UnityEngine;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	#if UNITY_EDITOR
	[InitializeOnLoad]
    #endif
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public class NWDTypeLauncher
	{
        private static NWDTypeLauncher InitialLaucher = new NWDTypeLauncher();
        //-------------------------------------------------------------------------------------------------------------
        public static bool IsLaunching = false;// to protect dupplicate launch editor/player
		public static bool IsLaunched = false;// to protect dupplicate launch editor/player
        //-------------------------------------------------------------------------------------------------------------
        static NWDTypeLauncher()
        {
            //Debug.Log("#### NWDTypeLauncher Class Constructor");
            Launcher();
        }
        //-------------------------------------------------------------------------------------------------------------
		public NWDTypeLauncher ()
		{
            //Debug.Log ("#### NWDTypeLauncher Instance Constructor");
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void Launcher ()
		{
			//Debug.Log ("#### NWDTypeLauncher Launcher start");
            if (IsLaunched == false && IsLaunching==false) {
                IsLaunching = true;
				// Get ShareInstance
				NWDDataManager tShareInstance = NWDDataManager.SharedInstance();
				// connect to database;
				tShareInstance.ConnectToDatabase ();
				// Find all Type of NWDType
				Type[] tAllTypes = System.Reflection.Assembly.GetExecutingAssembly ().GetTypes ();
				Type[] tAllNWDTypes = (from System.Type type in tAllTypes
				                       where type.IsSubclassOf (typeof(NWDTypeClass))
				                       select type).ToArray ();
				// Force launch and register class type
				int tTrigrammeAbstract = 111;
				int tNumberOfClasses = tAllNWDTypes.Count ();
				int tIndexOfActualClass = 0;
				foreach (Type tType in tAllNWDTypes) {
					tTrigrammeAbstract++;
					if (tType.ContainsGenericParameters == false) {
						//Debug.Log ("FIND tType = " + tType.Name);
						string tTrigramme = tTrigrammeAbstract.ToString ();
						if (tType.GetCustomAttributes (typeof(NWDClassTrigrammeAttribute), true).Length > 0) {
							NWDClassTrigrammeAttribute tTrigrammeAttribut = (NWDClassTrigrammeAttribute)tType.GetCustomAttributes (typeof(NWDClassTrigrammeAttribute), true) [0];
							tTrigramme = tTrigrammeAttribut.Trigramme;
							if (tTrigramme == null || tTrigramme == "") {
								tTrigramme = tTrigrammeAbstract.ToString ();
							}
						}
						bool tServerSynchronize = true;
						if (tType.GetCustomAttributes (typeof(NWDClassServerSynchronizeAttribute), true).Length > 0) {
							NWDClassServerSynchronizeAttribute tServerSynchronizeAttribut = (NWDClassServerSynchronizeAttribute)tType.GetCustomAttributes (typeof(NWDClassServerSynchronizeAttribute), true) [0];
							tServerSynchronize = tServerSynchronizeAttribut.ServerSynchronize;
						}
						string tDescription = "no description";
						if (tType.GetCustomAttributes (typeof(NWDClassDescriptionAttribute), true).Length > 0) {
							NWDClassDescriptionAttribute tDescriptionAttribut = (NWDClassDescriptionAttribute)tType.GetCustomAttributes (typeof(NWDClassDescriptionAttribute), true) [0];
							tDescription = tDescriptionAttribut.Description;
							if (tDescription == null || tDescription == "") {
								tDescription = "empty description";
							}
						}
						string tMenuName = tType.Name + " menu";
						if (tType.GetCustomAttributes (typeof(NWDClassMenuNameAttribute), true).Length > 0) {
							NWDClassMenuNameAttribute tMenuNameAttribut = (NWDClassMenuNameAttribute)tType.GetCustomAttributes (typeof(NWDClassMenuNameAttribute), true) [0];
							tMenuName = tMenuNameAttribut.MenuName;
							if (tMenuName == null || tMenuName == "") {
								tMenuName = tType.Name + " menu";
							}
						}

						var tMethodDeclare = tType.GetMethod ("ClassDeclare", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
						if (tMethodDeclare != null) {
							tMethodDeclare.Invoke (null, new object[]{ tServerSynchronize, tTrigramme, tMenuName, tDescription});
						}

						/* DEBUG */
//						var tMethodInfo = tType.GetMethod ("ClassInfos", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
//						if (tMethodInfo != null) {
//							tMethodInfo.Invoke (null, new object[]{ "Launcher " });
//						}
						/* DEBUG */
					}
					tIndexOfActualClass++;
				}
                IsLaunched = true;
			}
            //Debug.Log ("#### NWDTypeLauncher Launcher FINISHED");
		}
		//-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================