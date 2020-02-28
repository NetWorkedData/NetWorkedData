//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:33:40
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
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
        [Header("Connect the localizable Text")]
        public NWDLocalizationConnection LocalizationReference;
        public NWDAutolocalizedTag AutoTag = NWDAutolocalizedTag.MarkedBaseString;
        public bool EmptyIfNotLoaded = true;
        public bool EnrichmentByReference = true;

        [Header("Optional binding")]
        public bool TextBinding = true;
        public Text TextTarget;
        public bool TextMeshBinding = true;
        public TMP_Text TextMeshTarget;
        //=============================================================================================================
        // PUBLIC METHOD
        //-------------------------------------------------------------------------------------------------------------
        public void Localize(bool sUseBaseString = false)
        {
            //Debug.Log("Localize");
            //if (LocalizationReference != null)
            //{
            //    NWDLocalization tLocalization = LocalizationReference.GetReachableData();
            //}
            
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
                        NWDLocalization tLocalization = LocalizationReference.GetReachableData();
                        if (tLocalization != null)
                        {
                            //Debug.Log("Reference "+ LocalizationReference.Reference+ " exists!");
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
                        else
                        {
                            Debug.LogWarning("Reference " + LocalizationReference.Reference + " NOT exists!");
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
                    TextMeshTarget = GetComponent<TMP_Text>();
                }
                // if not find in root object
                if (TextMeshTarget == null)
                {
                    // get first children text
                    TextMeshTarget = GetComponentInChildren<TMP_Text>();
                }
                // TextMesh found
                if (TextMeshTarget != null)
                {
                    // set the localizable text
                    if (LocalizationReference != null)
                    {
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
                    TextMeshTarget = GetComponent<TMP_Text>();
                }
                // if not find in root object
                if (TextMeshTarget == null)
                {
                    // get first children text
                    TextMeshTarget = GetComponentInChildren<TMP_Text>();
                }
                // TextMesh found
                if (TextMeshTarget != null)
                {
                    // set the localizable text
                    TextMeshTarget.text = string.Empty;
                }
            }
        }
        //=============================================================================================================
        // PRIVATE METHOD
        //-------------------------------------------------------------------------------------------------------------
        void DataIsLoaded()
        {
            //Debug.Log("Data editor just loaded");
            NWENotificationManager tNotificationManager = NWENotificationManager.SharedInstance();
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_LAUNCHER_EDITOR_READY);
            Localize(false);
        }
        //-------------------------------------------------------------------------------------------------------------
        void LanguageChanged()
        {
            Localize(false);
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnEnable()
        {
            //LocalizationReference.Log();
            if (NWDDataManager.SharedInstance().DataEditorLoaded == false)
            {
                //Debug.Log("Data editor not loaded");
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
                //Debug.Log("Data editor allready loaded");
                Localize(false);
            }

            NWENotificationManager.SharedInstance().AddObserverForAll(this, NWDNotificationConstants.K_LANGUAGE_CHANGED, delegate (NWENotification sNotification)
            {
                Localize(false);
            });
        }
        //-------------------------------------------------------------------------------------------------------------
        void OnDisable()
        {
            NWENotificationManager tNotificationManager = NWENotificationManager.SharedInstance();
            tNotificationManager.RemoveObserverEveryWhere(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        string Enrichment(string sText)
        {
            string rText = NWDLocalization.Enrichment(sText);
            rText = NWDUserNickname.Enrichment(rText);
            rText = NWDAccountNickname.Enrichment(rText);
            return rText;
        }
#if UNITY_EDITOR
        //=============================================================================================================
        // EDITOR METHOD
        //-------------------------------------------------------------------------------------------------------------
        void OnDrawGizmos()
        {
            LocalizeEditor();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void LocalizeEditor()
        {
            if (EditorApplication.isPlaying)
            {
                // Do nothing !
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
                            TextMeshTarget = GetComponent<TMP_Text>();
                        }
                        // if not find in root object
                        if (TextMeshTarget == null)
                        {
                            // get first children text
                            TextMeshTarget = GetComponentInChildren<TMP_Text>();
                        }
                        if (TextMeshTarget != null)
                        {
                            // set the localizable text
                            if (LocalizationReference != null)
                            {
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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================