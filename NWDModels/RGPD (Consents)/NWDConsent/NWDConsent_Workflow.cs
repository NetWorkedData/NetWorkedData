//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
#if NWD_RGPD
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDConsent : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public const string K_APPCONSENTS_NEED_VALIDATION = "K_APPCONSENTS_NEED_VALIDATION_Je7dY5z"; // OK Need to test & verify
        public const string K_APPCONSENTS_CHANGED = "K_APPCONSENTS_CHANGED_rhjge4ez"; // OK Need to test & verify
        //-------------------------------------------------------------------------------------------------------------
        public NWDConsent()
        {
            //Debug.Log("NWDAppConsent Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDConsent(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDAppConsent Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
            DefaultState = NWESwitchState.Off;
            ExpectedState = NWESwitchState.Unknow;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool ConsentsAreValid(NWDConsent[] sConsentsArray)
        {
            bool rReturn = true;
            foreach (NWDConsent tConsent in sConsentsArray)
            {
                NWDAccountConsent tUserConsent = NWDAccountConsent.FindDataByConsent(tConsent, false);
                if (tUserConsent == null)
                {
                    rReturn = false;
                    break;
                }
                else
                {
                    if (tUserConsent.ConsentIsValid() == false)
                    {
                        rReturn = false;
                        break;
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool ConsentsAreAllValid()
        {
            bool rReturn = ConsentsAreValid(SelectCurrentDatas());
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool ConsentsCheck()
        {
            bool rReturn = ConsentsAreValid(SelectCurrentDatas());
            if (rReturn == false)
            {
                NWENotificationManager.SharedInstance().PostNotification(null, NWDConsent.K_APPCONSENTS_NEED_VALIDATION);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWESwitchState GetUserAuthorization(bool sCreateIfNull = false)
        {
            NWESwitchState rReturn = NWESwitchState.Unknow;
            NWDAccountConsent tUserConsent = NWDAccountConsent.FindDataByConsent(this, sCreateIfNull);
            if (tUserConsent == null)
            {
                rReturn = NWESwitchState.Unknow;
            }
            else
            {
                rReturn = tUserConsent.Authorization;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Localize(UnityEngine.UI.Text sText, string sDefault = NWEConstants.K_EMPTY_STRING)
        {
            sText.text = Localize(sText.text, sDefault);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string Localize(string sText, string sDefault = NWEConstants.K_EMPTY_STRING)
        {
            string rLocalizeText = string.Empty;

            if (NWDDataManager.SharedInstance().EditorDatabaseLoaded)
            {
                if (sText != null)
                {
                    if (sDefault.Equals(string.Empty))
                    {
                        sDefault = sText;
                    }

                    NWDConsent tObject = NWDBasisHelper.GetRawFirstDataByInternalKey<NWDConsent>(sText) as NWDConsent;
                    if (tObject != null)
                    {
                        // Title
                        string tText = tObject.Title.GetLocalString();
                        rLocalizeText = tText.Replace("<br>", "\n");
                        rLocalizeText += "\n";

                        // Description
                        tText = tObject.Description.GetLocalString();
                        rLocalizeText += tText.Replace("<br>", "\n");
                    }
                }
                else
                {
#if UNITY_EDITOR
                    EditorUtility.DisplayDialog("Localize", "String is null", "OK");
#endif
                }
            }
            else
            {
#if UNITY_EDITOR
                EditorUtility.DisplayDialog("Localize", "NWD engine not loaded", "OK");
#endif
            }
            return rLocalizeText;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
