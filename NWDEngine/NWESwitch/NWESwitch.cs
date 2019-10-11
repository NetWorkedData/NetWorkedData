//=====================================================================================================================
//
// ideMobi copyright 2018
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWESwitchState : int
    {
        On,
        Off,
        Unknow,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public class NWESwitchEvent : UnityEvent
    {
        // Event interface for unity
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [RequireComponent(typeof(Image))]
    public class NWESwitch : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        //-------------------------------------------------------------------------------------------------------------
        [Header("Value of switch")]
        public NWESwitchState SwitchState = NWESwitchState.Unknow;
        [Header("First default value")]
        public NWESwitchState SwitchStateDefault = NWESwitchState.On;
        [Header("Sprites for state")]
        public Sprite ImageOn;
        public Sprite ImageOff;
        public Sprite ImageUnknow;
        public Sprite ImageDisable;
        [Header("Color on event handler")]
        public Color NormalColor = Color.white;
        public Color HighlightedColor = Color.white;
        public Color PressedColor = Color.gray;
        public Color DisabledColor = Color.gray;
        [Header("Event on click")]
        public NWESwitchEvent OnSwitch;
        //-------------------------------------------------------------------------------------------------------------
        Image ImageGraphic;
        Button ButtonComponent;
        //-------------------------------------------------------------------------------------------------------------
        public void OnPointerClick(PointerEventData eventData)
        {
            //Debug.Log("OnPointerClick");
            TaskOnPointerDown();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnPointerDown(PointerEventData eventData)
        {
            //Debug.Log("OnPointerDown");
            ImageGraphic.color = PressedColor;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnPointerUp(PointerEventData eventData)
        {
            //Debug.Log("OnPointerUp");
            ImageGraphic.color = NormalColor;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void Start()
        {
            // get sprite
            ImageGraphic = GetComponent<Image>();
            SpriteChanged();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void TaskOnPointerDown()
        {
            switch (SwitchState)
            {
                case NWESwitchState.Unknow:
                    {
                        SwitchState = SwitchStateDefault;
                    }
                    break;
                case NWESwitchState.Off:
                    {
                        SwitchState = NWESwitchState.On;
                    }
                    break;
                case NWESwitchState.On:
                    {
                        SwitchState = NWESwitchState.Off;
                    }
                    break;
            }
            SpriteChanged();
            if (OnSwitch != null)
            {
               OnSwitch.Invoke();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void SpriteChanged()
        {
            switch (SwitchState)
            {
                case NWESwitchState.Unknow:
                    {
                        if (ImageUnknow != null)
                        {
                            ImageGraphic.sprite = ImageUnknow;
                        }
                    }
                    break;
                case NWESwitchState.Off:
                    {
                        if (ImageOff != null)
                        {
                            ImageGraphic.sprite = ImageOff;
                        }
                    }
                    break;
                case NWESwitchState.On:
                    {
                        if (ImageOn != null)
                        {
                            ImageGraphic.sprite = ImageOn;
                        }
                    }
                    break;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
