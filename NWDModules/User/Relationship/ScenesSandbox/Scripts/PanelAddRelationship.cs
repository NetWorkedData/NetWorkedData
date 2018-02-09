//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NetWorkedData;

//=====================================================================================================================
namespace SpiritOfBottle
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// Visual Object in Unity Player
    /// </summary>
    public class PanelAddRelationship : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        public GameObject PanelNickName;
        public GameObject PanelChooseRelationship;
        public GameObject PanelValidategRelationship;
        public GameObject PanelWaitingRelationship;
        public GameObject PanelGenerateCodePin;
        //-------------------------------------------------------------------------------------------------------------
        public Text TextTitleNickName;
        public Text TextTitleChooseRelationship;
        public Text TextEnterCodePin;
        public Text TextOr;
        public Text TextGenerate;
        public Text TextValidate;
        public Text TextRefused;
        public Text[] TextTimer;
        //-------------------------------------------------------------------------------------------------------------
        // Panel Add Relationship delegate
        public delegate void validateRelationshipBlock(bool success);
        public validateRelationshipBlock validateRelationshipBlockDelegate;
        //-------------------------------------------------------------------------------------------------------------
        private float TimeLeft = 10f;
        private const float DefaultTimer = 10f;
        private bool IsTimerStarted = false;
        private Animator PanelAnim;
        //-------------------------------------------------------------------------------------------------------------
        private enum CodePinStat { None, Sent, Generated }
        private CodePinStat PinStat = CodePinStat.None;
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            // Localized text
            NWDLocalization.AutoLocalize(TextTitleNickName);
            NWDLocalization.AutoLocalize(TextTitleChooseRelationship);
            NWDLocalization.AutoLocalize(TextEnterCodePin);
            NWDLocalization.AutoLocalize(TextOr);
            NWDLocalization.AutoLocalize(TextGenerate);
            NWDLocalization.AutoLocalize(TextValidate);
            NWDLocalization.AutoLocalize(TextRefused);

            //TODO verify is user have a 'nickname'
            PanelNickName.SetActive(true);

            //TODO also show Panel Choose Relationship
            //PanelChooseRelationship.SetActive(true);

            PanelAnim = GetComponent<Animator>();
        }
        //-------------------------------------------------------------------------------------------------------------
        void Update()
        {
            if (IsTimerStarted)
            {
                TimeLeft -= Time.deltaTime;
                if (TimeLeft <= 0)
                {
                    EndTimer();
                }
                else
                {
                    var minutes = Mathf.Floor(TimeLeft / 60); //Divide the guiTime by sixty to get the minutes.
                    var seconds = TimeLeft % 60;//Use the euclidean division for the seconds.
                    var fraction = (TimeLeft * 100) % 100;

                    //update the label value
                    foreach(Text t in TextTimer) {
                        t.text = string.Format("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);    
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void EndTimer()
        {
            if(PinStat == CodePinStat.Generated)
            {
                PanelValidategRelationship.SetActive(true);
                PanelGenerateCodePin.SetActive(false);
            }
            else if(PinStat == CodePinStat.Sent)
            {
                //TODO send true if master validate the relationship after active user send code pin
                ValidateRelationship(false);
            }
            TimeLeft = DefaultTimer;
            IsTimerStarted = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ShowPanel()
        {
            gameObject.SetActive(true); 
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ClosePanel()
        {
            PanelAnim.SetTrigger("Hide");
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DestroyPanel()
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        //-------------------------------------------------------------------------------------------------------------
        // Panel NickName
        public void ValidateNickName(InputField NickName)
        {
            //TODO Save nickname in user account
            PanelNickName.SetActive(false);
            PanelChooseRelationship.SetActive(true);
        }
        //-------------------------------------------------------------------------------------------------------------
        // Panel Choose Relationship
        public void ValidateCodePin(InputField NickName)
        {
            //TODO Show waiting panel with timer
            PanelNickName.SetActive(false);
            PanelWaitingRelationship.SetActive(true);
            IsTimerStarted = true;
            PinStat = CodePinStat.Sent;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateCodePin()
        {
            //TODO Show generate panel with timer
            PanelChooseRelationship.SetActive(false);
            PanelGenerateCodePin.SetActive(true);
            IsTimerStarted = true;
            PinStat = CodePinStat.Generated;
        }
        //-------------------------------------------------------------------------------------------------------------
        // Panel Validate Relationship
        public void ValidateRelationship(bool value)
        {
            if(value)
            {
                //TODO Save the new relationship if true
            }

            PanelValidategRelationship.SetActive(false);
            PanelChooseRelationship.SetActive(true);
            PinStat = CodePinStat.None;

            if(validateRelationshipBlockDelegate != null)
            {
                validateRelationshipBlockDelegate(value);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================