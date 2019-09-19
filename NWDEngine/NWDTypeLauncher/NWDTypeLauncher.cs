//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:48
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================



using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
//using BasicToolBox;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDTypeLauncher is the first class launch in the NetWorkedData lib. It's used to determine the class model and interaction.
    /// </summary>
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    public class NWDTypeLauncher // TODO : put in static?
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The lib is launching.
        /// </summary>
        //public static bool IsLaunching = false;
        /// <summary>
        /// The lib is launched.
        /// </summary>
        //public static bool IsLaunched = false;
        /// <summary>
        /// All Types array.
        /// </summary>
        public static Type[] AllTypes;
        //-------------------------------------------------------------------------------------------------------------
        //public static int CodePinTentative = 0;
        //public static string CodePinValue;
        //public static string CodePinValueConfirm;
        //public static bool CodePinNeeded = false;
        //public static bool CodePinCreationNeeded = false;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes the <see cref="T:NetWorkedData.NWDTypeLauncher"/> class.
        /// </summary>
        static NWDTypeLauncher()
        {
            //NWDDebug.Log("NWDTypeLauncher Static Class Constructor()");
            Launcher();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="T:NetWorkedData.NWDTypeLauncher"/> class.
        /// </summary>
        public NWDTypeLauncher()
        {
            //NWDDebug.Log("NWDTypeLauncher Instance Constructor NWDTypeLauncher()");
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Launcher this daemon lib.
        /// </summary>
        public static void Launcher()
        {
            NWDLauncher.Launch();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Runs the launcher.
        /// </summary>
#if UNITY_EDITOR
        [InitializeOnLoadMethod()]
#endif
        public static void RunLauncher()
        {
#if UNITY_EDITOR
            // to clear error ProgressBar 
            EditorUtility.ClearProgressBar();
#endif
            // FORCE TO ENGLISH FORMAT!
            Thread.CurrentThread.CurrentCulture = NWDConstants.FormatCountry;
            // this class deamon is launch at start ... Read all classes, install all classes deamon and load all datas
            //NWDDebug.Log("NWDTypeLauncher RunLauncher()");
            // not double lauch
            // not double launching!
            if (NWDLauncher.GetState() == NWDStatut.EngineLaunching)
            {
                // craeta a list to reccord all classes
                List<Type> tTypeList = new List<Type>();
                // Find all Type of NWDType
                //NWEBenchmark.Start("Launcher() reflexion");
                Type[] tAllTypes = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
                // sort and filter by NWDBasis (NWDTypeClass subclass)
                Type[] tAllNWDTypes = (from System.Type type in tAllTypes
                                       where type.IsSubclassOf(typeof(NWDTypeClass))
                                       select type).ToArray();
                //NWEBenchmark.Finish("Launcher() reflexion");
                foreach (Type tType in tAllNWDTypes)
                {
                    // not the NWDBasis because it's generic class
                    if (tType.ContainsGenericParameters == false)
                    {
                        // add type in list of class
                        tTypeList.Add(tType);
                        // invoke the ClassDeclare method!

                        NWDBasisHelper tHelper = NWDBasisHelper.Declare(tType);

                        //MethodInfo tMethodInfo = NWDAliasMethod.GetMethodPublicStaticFlattenHierarchy(tType, NWDConstants.M_ClassDeclare);

                        //if (tMethodInfo != null)
                        //{
                        //    tMethodInfo.Invoke(null, null);
                        //}
                    }
                }
                AllTypes = tTypeList.ToArray();
                NWDAppConfiguration.SharedInstance().RestaureTypesConfigurations();
                NWDDataManager.SharedInstance().ClassEditorExpected = NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Count();
                NWDDataManager.SharedInstance().ClassAccountExpected = NWDDataManager.SharedInstance().mTypeAccountDependantList.Count();
                NWDDataManager.SharedInstance().ClassExpected = NWDDataManager.SharedInstance().ClassEditorExpected + NWDDataManager.SharedInstance().ClassAccountExpected;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static void DatabaseEditorLauncher()
        //{
        //    if (IsLaunched == true && NWDDataManager.SharedInstance().DataEditorConnected == false)
        //    {
        //        if (NWDDataManager.SharedInstance().ConnectToDatabaseEditor())
        //        {
        //            NWDDataManager.SharedInstance().CreateAllTablesLocalEditor();
        //        }
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public static void DatabaseEditorLoadDatas()
        //{
        //    if (IsLaunched == true && NWDDataManager.SharedInstance().DataEditorConnected == true)
        //    {
        //        if (NWDDataManager.SharedInstance().DataEditorLoaded == false)
        //        {
        //            NWDDataManager.SharedInstance().ReloadAllObjectsEditor();
        //        }
        //        NWDDataManager.SharedInstance().DataEditorLoaded = true;
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public static void DatabaseAccountLauncher()
        //{
        //    NWEBenchmark.Start();
        //    if (IsLaunched == true && NWDDataManager.SharedInstance().DataAccountConnected == false && IsLaunching == true)
        //    {
        //        string tSurProtection = string.Empty;
        //        if (NWDAppConfiguration.SharedInstance().SurProtected == true)
        //        {
        //            NWDTypeLauncher.CodePinNeeded = true;
        //            NWDDataManager tShareInstance = NWDDataManager.SharedInstance();
        //            if (tShareInstance.DatabaseAccountExists() == false)
        //            {
        //                Debug.LogWarning("### Database NOT EXISTS");
        //                NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_NEEDED);
        //                NWDTypeLauncher.CodePinCreationNeeded = true;
        //            }
        //            else
        //            {
        //                Debug.LogWarning("### Database EXISTS NEED PINCODE");
        //                NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_REQUEST);
        //            }
        //        }
        //        else
        //        {
        //            DatabaseAccountConnection(string.Empty);
        //        }
        //    }
        //    NWEBenchmark.Finish();
        //}
        //-------------------------------------------------------------------------------------------------------------
//        public static void DatabaseAccountConnection(string sSurProtection)
//        {
//            Debug.Log("<color=orange>DatabaseAccountConnection(" + sSurProtection + ")</color>");
//            NWEBenchmark.Start();
//            //if (IsLaunched == true && DataAccountConnected == false && IsLaunching == true)
//            if (NWDDataManager.SharedInstance().DataAccountConnected == false)
//            {
//                CodePinTentative++;
//                // Get ShareInstance of datamanager instance
//                if (NWDDataManager.SharedInstance().ConnectToDatabaseAccount(sSurProtection) == false)
//                {
//                    if (CodePinTentative < NWDAppConfiguration.SharedInstance().ProtectionTentativeMax)
//                    {
//#if UNITY_EDITOR
//                        EditorUtility.DisplayDialog("ERROR", "CodePin for account database is invalid!", "OK");
//#endif
//                        Debug.Log("<color=orange>Database is not openable with this sur protected code! Tentative n°" + CodePinTentative + " : " + sSurProtection + "</color>");
//                        NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_FAIL);
//                        //DatabaseAccountLauncher();
//                    }
//                    else
//                    {
//                        Debug.Log("<color=orange>Database is not openable max tentative over! Tentative n°" + CodePinTentative + "</color>");
//                        // Kill App || Destroy Database || Call FBI || Vodoo ?
//                        NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_STOP);
//                    }
//                }
//                else
//                {
//                    CodePinTentative = 0;
//                    if (NWDAppConfiguration.SharedInstance().SurProtected == true)
//                    {
//                        Debug.Log("<color=orange>Database is opened with this sur protected code! Tentative n°" + CodePinTentative + "</color>");
//                        NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_SUCCESS);
//                    }
//                    NWDDataManager.SharedInstance().CreateAllTablesLocalAccount();
//                    Debug.Log("<color=orange>Database is connected</color>");
//                    NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DB_EDITOR_CONNECTED);
//#if UNITY_EDITOR
//                    if (Application.isEditor && Application.isPlaying == false)
//                    {
//                        DatabaseAccountLoadDatas();
//                    }
//#endif
        //        }
        //    }
        //    NWEBenchmark.Finish();
        //}
        //-------------------------------------------------------------------------------------------------------------
//        public static void DatabaseAccountLoadDatas()
//        {
//            NWEBenchmark.Start();
//            if (IsLaunched == true && NWDDataManager.SharedInstance().DataAccountConnected == true)
//            {
//                // Ok database is connected
//                NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DB_ACCOUNT_CONNECTED);
//                // start to lauch datas from database
//                // create, recreate or update all account's tables!
//                // Loaded data 
//                if (NWDDataManager.SharedInstance().DataAccountLoaded == false)
//                {
//                    bool tEditorByPass = false;
//#if UNITY_EDITOR
//                    if (Application.isEditor && Application.isPlaying == false)
//                    {
//                        tEditorByPass = true;
//                        //Debug.Log("NWD => Preload Datas bypass by editor");
//                    }
//#endif
        //            if (NWDAppConfiguration.SharedInstance().PreloadDatas == true || tEditorByPass == true)
        //            {
        //                NWEBenchmark.Start("DatabaseAccountLoadDatas() load Datas");
        //                NWDDataManager.SharedInstance().ReloadAllObjectsAccount();
        //                NWEBenchmark.Finish("DatabaseAccountLoadDatas() load Datas");
        //            }
        //        }
        //        // finish launch
        //        NWDDataManager.SharedInstance().DataAccountLoaded = true;
        //        //Debug.Log ("#### NWDTypeLauncher Launcher FINISHED");
        //    }
        //    NWEBenchmark.Finish();
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================