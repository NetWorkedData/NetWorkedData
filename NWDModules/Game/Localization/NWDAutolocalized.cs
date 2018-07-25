//=====================================================================================================================
//
// ideMobi copyright 2018
// All rights reserved by ideMobi
//
//=====================================================================================================================

using UnityEngine;
using UnityEngine.UI;

//=====================================================================================================================
namespace NetWorkedData
{
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
        /// <summary>
        /// The text target (optional).
        /// </summary>
        [Header("Optional bind")]
        public Text TextTarget;
        //-------------------------------------------------------------------------------------------------------------
        void Awake()
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
            // set the localizable text
            NWDLocalization tLocalization = LocalizationReference.GetObject();
            if (tLocalization != null)
            {
                string tTextString = tLocalization.GetLocalString();
                TextTarget.text = tTextString;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================