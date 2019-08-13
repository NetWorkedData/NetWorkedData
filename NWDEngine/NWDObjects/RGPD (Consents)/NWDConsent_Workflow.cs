//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:34:2
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

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
                BTBNotificationManager.SharedInstance().PostNotification(null, NWDConsent.K_APPCONSENTS_NEED_VALIDATION);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public BTBSwitchState GetUserAuthorization(bool sCreateIfNull = false)
        {
            BTBSwitchState rReturn = BTBSwitchState.Unknow;
            NWDAccountConsent tUserConsent = NWDAccountConsent.FindDataByConsent(this, sCreateIfNull);
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

            if (NWDDataManager.SharedInstance().DataEditorLoaded)
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