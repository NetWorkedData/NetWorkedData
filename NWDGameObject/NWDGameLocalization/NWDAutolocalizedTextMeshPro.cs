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
using UnityEngine;
using UnityEngine.UI;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// Autolocalized text with an NWDLocalizationConnection reference selected by popmenu in editor.
    /// </summary>
    public class NWDAutolocalizedTextMeshPro : MonoBehaviour
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
        /// <summary>
        /// The text target (optional).
        /// </summary>
        [Header("Optional binding")]
        public bool TextMeshProUGUIBinding = true;
        public TextMeshProUGUI TextMeshProUGUITarget;
        public bool TextMeshProBinding = true;
        public TextMeshPro TextMeshProTarget;
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            LocalizeEditor();
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        public void LocalizeEditor()
        {
            if (EditorApplication.isPlaying)
            {
            }
            else
            {
                if (AutoTag != NWDAutolocalizedTag.None)
                {
                    //TextMeshProUGUI
                    if (TextMeshProUGUIBinding == true)
                    {
                        // if not pluging
                        if (TextMeshProUGUITarget == null)
                        {
                            // get root text
                            TextMeshProUGUITarget = GetComponent<TextMeshProUGUI>();
                        }
                        // if not find in root object
                        if (TextMeshProUGUITarget == null)
                        {
                            // get first children text
                            TextMeshProUGUITarget = GetComponentInChildren<TextMeshProUGUI>();
                        }
                        if (TextMeshProUGUITarget != null)
                        {
                            // set the localizable text
                            NWDLocalization tLocalization = LocalizationReference.GetReachableData();
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
                                            tTextString = NWEConstants.K_HASHTAG + tLocalization.TextValue.GetBaseString() + NWEConstants.K_HASHTAG;
                                        }
                                        break;
                                    case NWDAutolocalizedTag.KeyInternal:
                                        {
                                            tTextString = tLocalization.InternalKey;
                                        }
                                        break;
                                }
                                if (TextMeshProUGUITarget.text != tTextString)
                                {
                                    TextMeshProUGUITarget.text = tTextString;
                                    EditorUtility.SetDirty(TextMeshProUGUITarget);
                                }
                            }
                        }
                    }

                    //TextMeshPro 
                    if (TextMeshProBinding == true)
                    {
                        // if not pluging
                        if (TextMeshProTarget == null)
                        {
                            // get root text
                            TextMeshProTarget = GetComponent<TextMeshPro>();
                        }
                        // if not find in root object
                        if (TextMeshProTarget == null)
                        {
                            // get first children text
                            TextMeshProTarget = GetComponentInChildren<TextMeshPro>();
                        }
                        if (TextMeshProTarget != null)
                        {
                            // set the localizable text
                            NWDLocalization tLocalization = LocalizationReference.GetReachableData();
                            if (tLocalization != null)
                            {
                                string tTextString = "";
                                switch (AutoTag)
                                {
                                    case NWDAutolocalizedTag.BaseString:
                                        {
                                            tTextString = tLocalization.TextValue.GetBaseString();
                                        }
                                        break;
                                    case NWDAutolocalizedTag.MarkedBaseString:
                                        {
                                            tTextString = NWEConstants.K_HASHTAG + tLocalization.TextValue.GetBaseString() + NWEConstants.K_HASHTAG;
                                        }
                                        break;
                                    case NWDAutolocalizedTag.KeyInternal:
                                        {
                                            tTextString = tLocalization.InternalKey;
                                        }
                                        break;
                                }
                                if (TextMeshProTarget.text != tTextString)
                                {
                                    TextMeshProTarget.text = tTextString;
                                    EditorUtility.SetDirty(TextMeshProTarget);
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
            //TextMeshProUGUI 
            if (TextMeshProUGUIBinding == true)
            {
                // if not pluging
                if (TextMeshProUGUITarget == null)
                {
                    // get root text
                    TextMeshProUGUITarget = GetComponent<TextMeshProUGUI>();
                }

                // if not find in root object
                if (TextMeshProUGUITarget == null)
                {
                    // get first children text
                    TextMeshProUGUITarget = GetComponentInChildren<TextMeshProUGUI>();
                }

                // TextMesh found
                if (TextMeshProUGUITarget != null)
                {
                    // set the localizable text
                    NWDLocalization tLocalization = LocalizationReference.GetReachableData();
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
                            TextMeshProUGUITarget.text = Enrichment(tTextString);
                        }
                        else
                        {
                            TextMeshProUGUITarget.text = tTextString;
                        }
                    }
                }
            }

            //TextMeshPro 
            if (TextMeshProBinding == true)
            {
                // if not pluging
                if (TextMeshProTarget == null)
                {
                    // get root text
                    TextMeshProTarget = GetComponent<TextMeshPro>();
                }

                // if not find in root object
                if (TextMeshProTarget == null)
                {
                    // get first children text
                    TextMeshProTarget = GetComponentInChildren<TextMeshPro>();
                }

                // TextMesh found
                if (TextMeshProTarget != null)
                {
                    // set the localizable text
                    NWDLocalization tLocalization = LocalizationReference.GetReachableData();
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
                            TextMeshProTarget.text = Enrichment(tTextString);
                        }
                        else
                        {
                            TextMeshProTarget.text = tTextString;
                        }
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Empty(bool sUseBaseString = false)
        {
            //TextMeshProUGUI 
            if (TextMeshProUGUIBinding == true)
            {
                // if not pluging
                if (TextMeshProUGUITarget == null)
                {
                    // get root text
                    TextMeshProUGUITarget = GetComponent<TextMeshProUGUI>();
                }

                // if not find in root object
                if (TextMeshProUGUITarget == null)
                {
                    // get first children text
                    TextMeshProUGUITarget = GetComponentInChildren<TextMeshProUGUI>();
                }

                // TextMesh found
                if (TextMeshProUGUITarget != null)
                {

                    TextMeshProUGUITarget.text = string.Empty;
                }
            }

            //TextMeshPro 
            if (TextMeshProBinding == true)
            {
                // if not pluging
                if (TextMeshProTarget == null)
                {
                    // get root text
                    TextMeshProTarget = GetComponent<TextMeshPro>();
                }

                // if not find in root object
                if (TextMeshProTarget == null)
                {
                    // get first children text
                    TextMeshProTarget = GetComponentInChildren<TextMeshPro>();
                }

                // TextMesh found
                if (TextMeshProTarget != null)
                {
                    TextMeshProTarget.text = string.Empty;
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void DataIsLoaded()
        {
            //Debug.Log("NWDAutolocalized DataIsLoaded()");
            //NWENotificationManager tNotificationManager = NWENotificationManager.SharedInstance();
            //tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATAS_LOADED);
            Localize(false);
        }
        //-------------------------------------------------------------------------------------------------------------
        void Awake()
        {
            //Debug.Log("NWDAutolocalized Awake()");
            if (NWDDataManager.SharedInstance().EditorDatabaseLoaded == false)
            {
                if (EmptyIfNotLoaded == true)
                {
                    Empty();
                }
                NWENotificationManager.SharedInstance().AddObserverForAll(this, NWDNotificationConstants.K_LAUNCHER_EDITOR_READY, delegate (NWENotification sNotification)
                {
                    DataIsLoaded();
                });
            }
            else
            {
                Localize(false);
            }
            NWENotificationManager.SharedInstance().AddObserverForAll(this, NWDNotificationConstants.K_LANGUAGE_CHANGED, delegate (NWENotification sNotification)
            {
                Localize(false);
            });
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnDestroy()
        {
            //Debug.Log("NWDAutolocalized OnDestroy()");
            NWENotificationManager tNotificationManager = NWENotificationManager.SharedInstance();
            tNotificationManager.RemoveObserverEveryWhere(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        string Enrichment(string sText)
        {
            string rText = NWDLocalization.Enrichment(sText);
#if NWD_USER_IDENTITY
            rText = NWDUserNickname.Enrichment(rText);
#endif
#if NWD_ACCOUNT_IDENTITY
            rText = NWDAccountNickname.Enrichment(rText);
#endif
            return rText;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================