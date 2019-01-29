//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
using System.Collections.Generic;
using BasicToolBox;
#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDConsent : NWDBasis<NWDConsent>
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
            DefaultState = BTBSwitchState.Off;
            ExpectedState = BTBSwitchState.Unknow;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDConsent[] GetAllLastVersionObjects()
        {
            List<NWDConsent> rList = new List<NWDConsent>();
            Dictionary<string, NWDConsent> tDico = new Dictionary<string, NWDConsent>();
            NWDConsent[] tConsentList = NWDConsent.FindDatas();
            foreach (NWDConsent tConsent in tConsentList)
            {
                if (tDico.ContainsKey(tConsent.KeyOfConsent) == false)
                {
                    tDico.Add(tConsent.KeyOfConsent, tConsent);
                }
                else
                {
                    if (tDico[tConsent.KeyOfConsent].Version.ToInt() < tConsent.Version.ToInt())
                    {
                        tDico[tConsent.KeyOfConsent] = tConsent;
                    }
                }
            }
            foreach (KeyValuePair<string, NWDConsent> tConsentKeyValue in tDico)
            {
                rList.Add(tConsentKeyValue.Value);
            }
            return rList.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool ConsentsAreValid(NWDConsent[] sConsentsArray)
        {
            bool rReturn = true;
            foreach (NWDConsent tConsent in sConsentsArray)
            {
                NWDAccountConsent tUserConsent = NWDAccountConsent.ForConsent(tConsent, false);
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
            bool rReturn = ConsentsAreValid(GetAllLastVersionObjects());
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool ConsentsCheck()
        {
            bool rReturn = ConsentsAreValid(GetAllLastVersionObjects());
            if (rReturn == false)
            {
                BTBNotificationManager.SharedInstance().PostNotification(null, NWDConsent.K_APPCONSENTS_NEED_VALIDATION);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountConsent GetUserConsent(bool sCreateIfNull = false)
        {
            return NWDAccountConsent.ForConsent(this, sCreateIfNull);
        }
        //-------------------------------------------------------------------------------------------------------------
        public BTBSwitchState GetUserAuthorization(bool sCreateIfNull = false)
        {
            BTBSwitchState rReturn = BTBSwitchState.Unknow;
            NWDAccountConsent tUserConsent = NWDAccountConsent.ForConsent(this, sCreateIfNull);
            if (tUserConsent == null)
            {
                rReturn = BTBSwitchState.Unknow;
            }
            else
            {
                rReturn = tUserConsent.Authorization;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Localize(UnityEngine.UI.Text sText, string sDefault = BTBConstants.K_EMPTY_STRING)
        {
            sText.text = Localize(sText.text, sDefault);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string Localize(string sText, string sDefault = BTBConstants.K_EMPTY_STRING)
        {
            string rLocalizeText = string.Empty;

            if (NWDTypeLauncher.DataLoaded)
            {
                if (sText != null)
                {
                    if (sDefault.Equals(string.Empty))
                    {
                        sDefault = sText;
                    }

                    NWDConsent tObject = FindFirstDatasByInternalKey(sText, true) as NWDConsent;
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