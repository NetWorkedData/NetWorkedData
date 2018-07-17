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

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// <para>Connection is used in MonBehaviour script to connect an object by its reference from popmenu list.</para>
    /// <para>The GameObject can use the object referenced by binding in game. </para>
    /// <example>
    /// Example :
    /// <code>
    /// public class MyScriptInGame : MonoBehaviour<br/>
    ///     {
    ///         NWDConnectionAttribut (true, true, true, true)] // optional
    ///         public NWDConsentConnection MyNetWorkedData;
    ///         public void UseData()
    ///             {
    ///                 NWDConsent tObject = MyNetWorkedData.GetObject();
    ///                 // Use tObject
    ///             }
    ///     }
    /// </code>
    /// </example>
    /// </summary>
    [Serializable]
    public class NWDConsentConnection : NWDConnection<NWDConsent>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDConsent class. This class is used to reccord the consent available in the game. 
    /// </summary>
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("ACS")]
    [NWDClassDescriptionAttribute("NWDConsent class. This class is used to reccord the consent available in the game for RGPD")]
    [NWDClassMenuNameAttribute("Consent")]
    public partial class NWDConsent : NWDBasis<NWDConsent>
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Instance Properties
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Consent description")]
        public NWDLocalizableStringType Title
        {
            get; set;
        }
        public NWDLocalizableTextType Description
        {
            get; set;
        }
        public string LawReferences
        {
            get; set;
        }
        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Consent version")]
        public string KeyOfConsent
        {
            get; set;
        }
        public NWDVersionType Version
        {
            get; set;
        }
        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Consent default state proposition")]
        public BTBSwitchState DefaultState
        {
            get; set;
        }
        [NWDTooltips("Expected state to continue the game. If 'Unknow' any value is ok")]
        public BTBSwitchState ExpectedState
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
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
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public static void ClassInitialization() // call by invoke
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exampel of implement for class method.
        /// </summary>
        public static NWDConsent[] GetAllLastVersionObjects()
        {
            List<NWDConsent> rList = new List<NWDConsent>();
            Dictionary<string, NWDConsent> tDico = new Dictionary<string, NWDConsent>();
            NWDConsent[] tConsentList = NWDConsent.GetAllObjects();
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
                NWDAccountConsent tUserConsent = NWDAccountConsent.UserConsentForAppConsent(tConsent, false);
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
        // Load datas
        public const string K_APPCONSENTS_NEED_VALIDATION = "K_APPCONSENTS_NEED_VALIDATION_Je7dY5z"; // OK Need to test & verify
        public const string K_APPCONSENTS_CHANGED = "K_APPCONSENTS_CHANGED_rhjge4ez"; // OK Need to test & verify
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
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exampel of implement for instance method.
        /// </summary>
        public void MyInstanceMethod()
        {
            // do something with this object
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountConsent GetUserConsent(bool sCreateIfNull = false)
        {
            return NWDAccountConsent.UserConsentForAppConsent(this, sCreateIfNull);
        }
        //-------------------------------------------------------------------------------------------------------------
        public BTBSwitchState GetUserAuthorization(bool sCreateIfNull = false)
        {
            BTBSwitchState rReturn = BTBSwitchState.Unknow;
            NWDAccountConsent tUserConsent = NWDAccountConsent.UserConsentForAppConsent(this, sCreateIfNull);
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
        public static void Localize(UnityEngine.UI.Text sText, string sDefault = "")
        {
            /*if (NWDTypeLauncher.DataLoaded)
            {
                if (sText != null)
                {
                    if (sDefault.Equals(""))
                    {
                        sDefault = sText.text;
                    }

                    NWDAppConsent tObject = GetObjectByInternalKey(sText.text, true) as NWDAppConsent;
                    if (tObject != null)
                    {
                        // Title
                        string tText = tObject.Title.GetLocalString();
                        sText.text = tText.Replace("<br>", "\n");
                        sText.text += "\n";

                        // Description
                        tText = tObject.Description.GetLocalString();
                        sText.text += tText.Replace("<br>", "\n");
                    }
                }
                else
                {
                    #if UNITY_EDITOR
                    EditorUtility.DisplayDialog("Localize", "Text component is null", "OK");
                    #endif
                }
            }
            else
            {
                #if UNITY_EDITOR
                EditorUtility.DisplayDialog("Localize", "NWD engine not loaded", "OK");
                #endif
            }*/

            sText.text = Localize(sText.text, sDefault);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string Localize(string sText, string sDefault = "")
        {
            string rLocalizeText = "";

            if (NWDTypeLauncher.DataLoaded)
            {
                if (sText != null)
                {
                    if (sDefault.Equals(""))
                    {
                        sDefault = sText;
                    }

                    NWDConsent tObject = GetObjectByInternalKey(sText, true) as NWDConsent;
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
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region NetWorkedData addons methods
        //-------------------------------------------------------------------------------------------------------------
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDConsent)/*, typeof(NWDUserNickname), etc*/ };
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just after loaded from database.
        /// </summary>
        public override void AddonLoadedMe()
        {
            // do something when object was loaded
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before unload from memory.
        /// </summary>
        public override void AddonUnloadMe()
        {
            // do something when object will be unload
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before insert.
        /// </summary>
        public override void AddonInsertMe()
        {
            // do something when object will be inserted
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before update.
        /// </summary>
        public override void AddonUpdateMe()
        {
            // do something when object will be updated
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method when updated.
        /// </summary>
        public override void AddonUpdatedMe()
        {
            // do something when object finish to be updated
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method when updated me from Web.
        /// </summary>
        public override void AddonUpdatedMeFromWeb()
        {
            // do something when object finish to be updated from CSV from WebService response
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before dupplicate.
        /// </summary>
        public override void AddonDuplicateMe()
        {
            // do something when object will be dupplicate
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before enable.
        /// </summary>
        public override void AddonEnableMe()
        {
            // do something when object will be enabled
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before disable.
        /// </summary>
        public override void AddonDisableMe()
        {
            // do something when object will be disabled
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before put in trash.
        /// </summary>
        public override void AddonTrashMe()
        {
            // do something when object will be put in trash
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before remove from trash.
        /// </summary>
        public override void AddonUnTrashMe()
        {
            // do something when object will be remove from trash
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons the delete me.
        /// </summary>
        public override void AddonDeleteMe()
        {
            // do something when object will be delete from local base
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonWebversionUpgradeMe(int sOldWebversion, int sNewWebVersion)
        {
            // do something when object will be web service upgrade
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Editor
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        //Addons for Edition
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons in edition state of object.
        /// </summary>
        /// <returns><c>true</c>, if object need to be update, <c>false</c> or not not to be update.</returns>
        /// <param name="sNeedBeUpdate">If set to <c>true</c> need be update in enter.</param>
        public override bool AddonEdited(bool sNeedBeUpdate)
        {
            if (sNeedBeUpdate == true)
            {
                // do something
            }
            return sNeedBeUpdate;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons editor interface.
        /// </summary>
        /// <returns>The editor height addon.</returns>
        /// <param name="sInRect">S in rect.</param>
        public override float AddonEditor(Rect sInRect)
        {
            // Draw the interface addon for editor
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons editor intreface expected height.
        /// </summary>
        /// <returns>The editor expected height.</returns>
        public override float AddonEditorHeight()
        {
            // Height calculate for the interface addon for editor
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the width of node draw.
        /// </summary>
        /// <returns>The on node draw width.</returns>
        /// <param name="sDocumentWidth">S document width.</param>
        public override float AddOnNodeDrawWidth(float sDocumentWidth)
        {
            return 250.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the height of node draw.
        /// </summary>
        /// <returns>The on node draw height.</returns>
        public override float AddOnNodeDrawHeight(float sCardWidth)
        {
            return 130.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds node draw.
        /// </summary>
        /// <param name="sRect">S rect.</param>
        public override void AddOnNodeDraw(Rect sRect, bool sPropertysGroup)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds color on node.
        /// </summary>
        /// <returns>The on node color.</returns>
        public override Color AddOnNodeColor()
        {
            return Color.gray;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool AddonErrorFound()
        {
            bool rReturnErrorFound = false;
            // check if you found error in Data values.
            // normal way is return false!
            return rReturnErrorFound;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================