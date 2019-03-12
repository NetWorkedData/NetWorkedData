//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BasicToolBox;
#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField]
    public enum NWDAutolocalizedTag : int
    {
        None,
        BaseString,
        MarkedBaseString,
        KeyInternal,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// Autolocalized text with an NWDLocalizationConnection reference selected by popmenu in editor.
    /// </summary>
    public class NWDAutolocalized : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The localization reference.
        /// </summary>
        [Header("Connect the localizable Text")]
        public NWDLocalizationConnection LocalizationReference;
        public NWDAutolocalizedTag AutoTag = NWDAutolocalizedTag.MarkedBaseString;
        public bool EmptyIfNotLoaded = true;
        public bool EnrichmentByReference = true;

        [Header("Optional binding")]
        public bool TextBinding = true;
        public Text TextTarget;
        public bool TextMeshBinding = true;
        public TextMesh TextMeshTarget;
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            //Debug.Log("NWDAutolocalized OnDrawGizmos()");
            LocalizeEditor();
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        public void LocalizeEditor()
        {
           //Debug.Log("NWDAutolocalized LocalizeEditor()");
            if (EditorApplication.isPlaying)
            {
            }
            else
            {
                if (AutoTag != NWDAutolocalizedTag.None)
                {

                    // text
                    if (TextBinding == true)
                    {
                        // if not pluging
                        if (TextTarget == null)
                        {
                            // get root text
                            TextTarget = GetComponent<Text>();
                        }
                        // if not find in root object
                        if (TextTarget == null)
                        {
                            // get first children text
                            TextTarget = GetComponentInChildren<Text>();
                        }
                        if (TextTarget != null)
                        {
                            // set the localizable text
                            if (LocalizationReference != null)
                            {
                                NWDLocalization tLocalization = LocalizationReference.GetObject();
                                if (tLocalization != null)
                                {
                                    string tTextString = string.Empty;
                                    switch (AutoTag)
                                    {
                                        case NWDAutolocalizedTag.BaseString:
                                            {
                                                tTextString = tLocalization.TextValue.GetBaseString();
                                            }
                                            break;
                                        case NWDAutolocalizedTag.MarkedBaseString:
                                            {
                                                tTextString = BTBConstants.K_HASHTAG + tLocalization.TextValue.GetBaseString() + BTBConstants.K_HASHTAG;
                                            }
                                            break;
                                        case NWDAutolocalizedTag.KeyInternal:
                                            {
                                                tTextString = tLocalization.InternalKeyValue();
                                            }
                                            break;
                                    }
                                    if (TextTarget.text != tTextString)
                                    {
                                        TextTarget.text = tTextString;
                                        EditorUtility.SetDirty(TextTarget);
                                    }
                                }
                            }
                        }
                    }

                    //TextMesh
                    if (TextMeshBinding == true)
                    {
                        // if not pluging
                        if (TextMeshTarget == null)
                        {
                            // get root text
                            TextMeshTarget = GetComponent<TextMesh>();
                        }
                        // if not find in root object
                        if (TextMeshTarget == null)
                        {
                            // get first children text
                            TextMeshTarget = GetComponentInChildren<TextMesh>();
                        }
                        if (TextMeshTarget != null)
                        {
                            // set the localizable text
                            if (LocalizationReference != null)
                            {
                                NWDLocalization tLocalization = LocalizationReference.GetObject();
                                if (tLocalization != null)
                                {
                                    string tTextString = string.Empty;
                                    switch (AutoTag)
                                    {
                                        case NWDAutolocalizedTag.BaseString:
                                            {
                                                tTextString = tLocalization.TextValue.GetBaseString();
                                            }
                                            break;
                                        case NWDAutolocalizedTag.MarkedBaseString:
                                            {
                                                tTextString = BTBConstants.K_HASHTAG + tLocalization.TextValue.GetBaseString() + BTBConstants.K_HASHTAG;
                                            }
                                            break;
                                        case NWDAutolocalizedTag.KeyInternal:
                                            {
                                                tTextString = tLocalization.InternalKeyValue();
                                            }
                                            break;
                                    }
                                    if (TextMeshTarget.text != tTextString)
                                    {
                                        TextMeshTarget.text = tTextString;
                                        EditorUtility.SetDirty(TextMeshTarget);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        public void Localize(bool sUseBaseString = false)
        {
            //Debug.Log("NWDAutolocalized Localize()");
            //Text
            if (TextBinding == true)
            {
                // if not pluging
                if (TextTarget == null)
                {
                    // get root text
                    TextTarget = GetComponent<Text>();
                }
                // if not find in root object
                if (TextTarget == null)
                {
                    // get first children text
                    TextTarget = GetComponentInChildren<Text>();
                }
                // if Text found
                if (TextTarget != null)
                {
                    // set the localizable text
                    if (LocalizationReference != null)
                    {
                        NWDLocalization tLocalization = LocalizationReference.GetObject();
                        if (tLocalization != null)
                        {
                            string tTextString = string.Empty;
                            if (sUseBaseString == true)
                            {
                                tTextString = tLocalization.TextValue.GetBaseString();
                            }
                            else
                            {
                                tTextString = tLocalization.GetLocalString();
                            }

                            // Enrich text
                            if (EnrichmentByReference == true)
                            {
                                TextTarget.text = Enrichment(tTextString);
                            }
                            else
                            {
                                TextTarget.text = tTextString;
                            }
                        }
                    }
                }
            }
            //TextMesh 
            if (TextMeshBinding == true)
            {
                // if not pluging
                if (TextMeshTarget == null)
                {
                    // get root text
                    TextMeshTarget = GetComponent<TextMesh>();
                }
                // if not find in root object
                if (TextMeshTarget == null)
                {
                    // get first children text
                    TextMeshTarget = GetComponentInChildren<TextMesh>();
                }
                // TextMesh found
                if (TextMeshTarget != null)
                {
                    // set the localizable text
                    if (LocalizationReference != null)
                    {
                        NWDLocalization tLocalization = LocalizationReference.GetObject();
                        if (tLocalization != null)
                        {
                            string tTextString = string.Empty;
                            if (sUseBaseString == true)
                            {
                                tTextString = tLocalization.TextValue.GetBaseString();
                            }
                            else
                            {
                                tTextString = tLocalization.GetLocalString();
                            }
                            if (EnrichmentByReference == true)
                            {
                                TextMeshTarget.text = Enrichment(tTextString);
                            }
                            else
                            {
                                TextMeshTarget.text = tTextString;
                            }
                        }
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Empty()
        {
            //Debug.Log("NWDAutolocalized Empty()");
            //Text
            if (TextBinding == true)
            {
                // if not pluging
                if (TextTarget == null)
                {
                    // get root text
                    TextTarget = GetComponent<Text>();
                }
                // if not find in root object
                if (TextTarget == null)
                {
                    // get first children text
                    TextTarget = GetComponentInChildren<Text>();
                }
                // if Text found
                if (TextTarget != null)
                {
                    TextTarget.text = string.Empty;
                }
            }
            //TextMesh 
            if (TextMeshBinding == true)
            {
                // if not pluging
                if (TextMeshTarget == null)
                {
                    // get root text
                    TextMeshTarget = GetComponent<TextMesh>();
                }
                // if not find in root object
                if (TextMeshTarget == null)
                {
                    // get first children text
                    TextMeshTarget = GetComponentInChildren<TextMesh>();
                }
                // TextMesh found
                if (TextMeshTarget != null)
                {
                    // set the localizable text
                    TextMeshTarget.text = string.Empty;
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void DataIsLoaded()
        {
            //Debug.Log("NWDAutolocalized DataIsLoaded()");
            BTBNotificationManager tNotificationManager = BTBNotificationManager.SharedInstance();
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATAS_LOADED);
            Localize(false);
        }
        //-------------------------------------------------------------------------------------------------------------
        void Awake()
        {
            //Debug.Log("NWDAutolocalized Awake()");
            if (NWDTypeLauncher.DataLoaded == false)
            {
                if (EmptyIfNotLoaded == true)
                {
                    Empty();
                }
                BTBNotificationManager tNotificationManager = BTBNotificationManager.SharedInstance();
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATAS_LOADED, delegate (BTBNotification sNotification)
                {
                    DataIsLoaded();
                });
            }
            else
            {
                Localize(false);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnDestroy()
        {
            //Debug.Log("NWDAutolocalized OnDestroy()");
            BTBNotificationManager tNotificationManager = BTBNotificationManager.SharedInstance();
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATAS_LOADED);
        }
        //-------------------------------------------------------------------------------------------------------------
        string Enrichment(string sText)
        {
            return NWDUserNickname.Enrichment(NWDLocalization.Enrichment(sText));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================