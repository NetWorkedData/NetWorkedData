using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDSwitchState : int
    {
        On,
        Off,
        Unknow,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public class NWDSwitchEvent : UnityEvent
    {
        // Event interface for unity
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [RequireComponent(typeof(Image))]
    public class NWDSwitch : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDSwitchState SwitchState = NWDSwitchState.Unknow;
        public NWDSwitchState SwitchStateDefault = NWDSwitchState.On;
        public Sprite ImageOn;
        public Sprite ImageOff;
        public Sprite ImageUnknow;
        public Sprite ImageDisable;
        public NWDSwitchEvent OnSwitch;
        //-------------------------------------------------------------------------------------------------------------
        Image ImageGraphic;
        Button ButtonComponent;
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
            //Output this to console when the Button is clicked
            Debug.Log("You have clicked the button!");
            switch (SwitchState)
            {
                case NWDSwitchState.Unknow:
                    {
                        SwitchState = SwitchStateDefault;
                    }
                    break;
                case NWDSwitchState.Off:
                    {
                        SwitchState = NWDSwitchState.On;
                    }
                    break;
                case NWDSwitchState.On:
                    {
                        SwitchState = NWDSwitchState.Off;
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
                case NWDSwitchState.Unknow:
                    {
                        if (ImageUnknow != null)
                        {
                            ImageGraphic.sprite = ImageUnknow;
                        }
                    }
                    break;
                case NWDSwitchState.Off:
                    {
                        if (ImageOff != null)
                        {
                            ImageGraphic.sprite = ImageOff;
                        }
                    }
                    break;
                case NWDSwitchState.On:
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
