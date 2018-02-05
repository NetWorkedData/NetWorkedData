//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

using SQLite4Unity3d;

#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.Build;

//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public class NWDBuildPostProcess : IPostprocessBuild {
		//-------------------------------------------------------------------------------------------------------------
		public int callbackOrder { get { return 0; } }
		//-------------------------------------------------------------------------------------------------------------
		public void OnPostprocessBuild(BuildTarget target, string path)
		{
			Debug.Log ("NWDBuildPostProcess OnPostprocessBuild for target " + target + " at path " + path);

            // TODO : find ALL TARGET PATH
//            bool tNeedPatchDatabase = false;

			// must find the good database 
//			string tPath = path; // Hum not good path ...
			BuildTarget tBuildTarget = EditorUserBuildSettings.activeBuildTarget;
			switch (tBuildTarget) {
                case BuildTarget.StandaloneOSX: 
				{
//					tPath = path + "/Data/Raw/NWDmage.prp";
				}
				break;
			case BuildTarget.StandaloneWindows: 
				{
				}
				break;
			case BuildTarget.iOS: 
				{
//                    tNeedPatchDatabase = true;
//                    tPath = path + "/Data/Raw/NWDmage.prp";
				}
				break;
			case BuildTarget.Android: 
				{
                    //Can't modify BDD, APK = zip file
                }
				break;
			case BuildTarget.StandaloneLinux: 
				{
				}
				break;
			case BuildTarget.StandaloneWindows64: 
				{
				}
				break;
			case BuildTarget.WebGL: 
				{
				}
				break;
			case BuildTarget.WSAPlayer: 
				{
				}
				break;
			case BuildTarget.StandaloneLinux64: 
				{
				}
				break;
			case BuildTarget.StandaloneLinuxUniversal: 
				{
				}
				break;
			//case BuildTarget.StandaloneOSXIntel64: 
            //	{
            ////					tPath = path + "/Data/Raw/NWDmage.prp";
			//  }
			//  break;
			case BuildTarget.Tizen: 
				{
				}
				break;
			case BuildTarget.PSP2: 
				{
				}
				break;
			case BuildTarget.PS4: 
				{
				}
				break;
			case BuildTarget.XboxOne: 
				{
				}
				break;
			//case BuildTarget.SamsungTV: 
				//{
				//}
				//break;
			case BuildTarget.N3DS: 
				{
				}
				break;
			case BuildTarget.WiiU: 
				{
				}
				break;
			case BuildTarget.tvOS: 
				{
				}
				break;
			case BuildTarget.Switch: 
				{
				}
				break;
			}
			/*
            if (tNeedPatchDatabase == true)
            {
                // connect to database 
                SQLiteConnection tSQLiteConnection = new SQLiteConnection(tPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
                // Remove datas from unsync tables
                foreach (Type tType in NWDDataManager.SharedInstance.mTypeUnSynchronizedList)
                {
                    tSQLiteConnection.DropTableByType(tType);
                    tSQLiteConnection.CreateTableByType(tType);
                }
                // Remove datas from sync tables with account reference
                foreach (Type tType in NWDDataManager.SharedInstance.mTypeSynchronizedList)
                {
                    bool tAccountConnected = false;
                    foreach (var tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        Type tTypeOfThis = tProp.PropertyType;
                        if (tTypeOfThis != null)
                        {
                            if (tTypeOfThis.IsGenericType)
                            {
                                if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferenceType<>))
                                {
                                    Type tSubType = tTypeOfThis.GetGenericArguments()[0];
                                    if (tSubType == typeof(NWDAccount))
                                    {
                                        tAccountConnected = true;
                                    }
                                }
                            }
                        }
                    }
                    if (tAccountConnected == true)
                    {
                        tSQLiteConnection.DropTableByType(tType);
                        tSQLiteConnection.CreateTableByType(tType);
                    }
                }
            }
            */
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif