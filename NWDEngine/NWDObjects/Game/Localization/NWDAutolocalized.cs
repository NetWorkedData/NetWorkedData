//=====================================================================================================================
//
// ideMobi copyright 2018
// All rights reserved by ideMobi
//
//=====================================================================================================================

using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
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
        /// <summary>
        /// The text target (optional).
        /// </summary>
        [Header("Optional bind")]
        public Text TextTarget;
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
                        NWDLocalization tLocalization = LocalizationReference.GetObject();
                        if (tLocalization != null)
                        {
                            switch (AutoTag)
                            {
                                case NWDAutolocalizedTag.BaseString:
                                    {
                                        string tTextString = tLocalization.TextValue.GetBaseString();
                                        TextTarget.text = tTextString;
                                    }
                                    break;
                                case NWDAutolocalizedTag.MarkedBaseString:
                                    {
                                        string tTextString = tLocalization.TextValue.GetBaseString();
                                        TextTarget.text = "#" + tTextString + "#";
                                    }
                                    break;
                                case NWDAutolocalizedTag.KeyInternal:
                                    {
                                        string tTextString = tLocalization.InternalKeyValue();
                                        TextTarget.text = tTextString;
                                    }
                                    break;
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
                NWDLocalization tLocalization = LocalizationReference.GetObject();
                if (tLocalization != null)
                {
                    if (sUseBaseString == true)
                    {
                        string tTextString = tLocalization.TextValue.GetBaseString();
                        TextTarget.text = tTextString;
                    }
                    else
                    {
                        string tTextString = tLocalization.GetLocalString();
                        TextTarget.text = tTextString;
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void Awake()
        {
            Localize(false);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================