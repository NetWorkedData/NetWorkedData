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
    /// Autolocalized text with an NWDLocalizationConnection object
    /// </summary>
    public class NWDAutolocalized : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        [Header("Connect the localizable Text")]
        public NWDLocalizationConnection LocalizationReference;
        [Header("Optional bind")]
        public Text TextTarget;
        //-------------------------------------------------------------------------------------------------------------
        void Start()
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