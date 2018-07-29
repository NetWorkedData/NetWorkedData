//=====================================================================================================================
//
// ideMobi copyright 2018
// All rights reserved by ideMobi
//
//=====================================================================================================================

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
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
    public class NWDAutoFill : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        [Header("Connect the localizable Text")]
        public NWDLocalizationConnection LocalizationReference;
        public NWDAutolocalizedTag AutoTag = NWDAutolocalizedTag.MarkedBaseString;
        private string StringToUse;
        [Header("Optional bind")]
        public Text TextTarget;
        [Header("Fill speed")]
        public float DelayBeforeStart = 0.5F;
        public float CharPerSecond = 12.0F;
        public float FastAccelerator = 4.0F;
        private bool FastChar = false;
        [Tooltip("Delay when read Dot '.' '!' '?' ';' '…' ")]
        public float DotPause = 1.0F;
        [Tooltip("Delay when read commat ',' ")]
        public float CommaPause = 0.4F;
        private float PauseCounter;
        private float TimeCounter;
        private int CharTotal;
        private int CharVisible;
        private bool Running = false;
        [Header("Mask color")]
        public bool UseMaskColor = true;
        public Color ColorToMask;
        private string ColorString;
        [Header("Optional finished event")]
        public UnityEvent FinishedToRead;
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
#endif
        }
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
        private void Awake()
        {
            Localize(false);
        }
        //-------------------------------------------------------------------------------------------------------------
        private void Start()
        {
            StringToUse = string.Copy(TextTarget.text);
            StartFilling();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void StartFilling()
        {
            FastChar = false;
            Running = true;
            TimeCounter = 0;
            PauseCounter = DelayBeforeStart;
            CharVisible = 0;
            CharTotal = StringToUse.Length;
            ColorString = ColorUtility.ToHtmlStringRGBA(ColorToMask);
            TextShow(CharVisible);
            if (UseMaskColor)
            {
                TextTarget.supportRichText = true; 
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void FastFilling()
        {
            FastChar = true;
            PauseCounter = 0;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool IsRunning()
        {
            return Running; 
        }
        //-------------------------------------------------------------------------------------------------------------
        private void TextShow(int sIndex)
        {
            //TextTarget.text = StringToUse.Substring(0,sIndex) + "<color=" + ColorString + ">" + StringToUse.Substring(sIndex, CharTotal-sIndex)  + "</color>";
            if (UseMaskColor)
            {
                TextTarget.text = string.Copy(StringToUse).Insert(sIndex, "<color=#" + ColorString + ">") + "</color>";
            }
            else
            {
                TextTarget.text = string.Copy(StringToUse).Substring(0, sIndex);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void Update()
        {
            if (Running == true)
            {
                PauseCounter -= Time.deltaTime;
                if (PauseCounter > 0)
                {
                    //pause
                }
                else
                {
                    if (FastChar == true)
                    {
                        TimeCounter += Time.deltaTime * FastAccelerator;;
                        CharVisible = (int)(TimeCounter * CharPerSecond);
                    }
                    else
                    {

                        TimeCounter += Time.deltaTime;
                        int extCharVisible = CharVisible;
                        CharVisible = (int)(TimeCounter * CharPerSecond);
                        if (CharVisible > CharTotal)
                        {
                            CharVisible = CharTotal;
                        }
                        //int tDelta = CharVisible - extCharVisible;
                        for (int ti = extCharVisible; ti < CharVisible; ti++)
                        {
                            if (StringToUse.Substring(ti, 1) == ",")
                            {
                                // find commat
                                if (CommaPause > 0.0F)
                                {
                                    CharVisible = ti + 1;
                                    PauseCounter = CommaPause;
                                    break;
                                }
                            }
                            if (StringToUse.Substring(ti, 1) == "." ||
                                StringToUse.Substring(ti, 1) == "!" ||
                                StringToUse.Substring(ti, 1) == "?" ||
                                StringToUse.Substring(ti, 1) == "…" ||
                                StringToUse.Substring(ti, 1) == ";"
                               )
                            {
                                // find commat
                                if (DotPause > 0.0F)
                                {
                                    CharVisible = ti + 1;
                                    PauseCounter = DotPause;
                                    break;
                                }
                            }
                        }
                    }
                    if (CharVisible >= 0)
                    {
                        if (CharVisible < CharTotal)
                        {
                            TextShow(CharVisible);
                        }
                        else
                        {
                            TextTarget.text = string.Copy(StringToUse);
                            Running = false;
                            if (FinishedToRead!=null)
                            {
                                FinishedToRead.Invoke();
                            }
                        }
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        //private void OnGUI()
        //{
        //    TextShow(CharVisible);
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================