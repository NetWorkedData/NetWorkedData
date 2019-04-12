// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:46:5
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NetWorkedData;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDAutoPreference : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDPreferenceKeyConnection PreferenceKeyConnexion;
        //-------------------------------------------------------------------------------------------------------------
        public Toggle ToogleBinding;
        public Slider SliderBinding;
        public InputField InputFieldBinding;
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            if (PreferenceKeyConnexion != null)
            {
                if (ToogleBinding != null)
                {
                    ToogleBinding.isOn = PreferenceKeyConnexion.GetBool();
                    ToogleBinding.onValueChanged.AddListener(delegate
                    {
                        this.OnChangedBool(ToogleBinding.isOn);
                    });
                }
                if (SliderBinding != null)
                {
                    SliderBinding.value = PreferenceKeyConnexion.GetFloat();
                    SliderBinding.onValueChanged.AddListener(delegate
                    {
                        this.OnChangedFloat(SliderBinding.value);
                    });
                }
                if (InputFieldBinding != null)
                {
                    InputFieldBinding.text = PreferenceKeyConnexion.GetString();
                    InputFieldBinding.onEndEdit.AddListener(delegate
                    {
                        this.OnChangedString(InputFieldBinding.text);
                    });
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnChangedBool(bool sValue)
        {
            if (PreferenceKeyConnexion != null)
            {
                if (ToogleBinding != null)
                {
                    PreferenceKeyConnexion.SetBool(sValue);
                }
                if (SliderBinding != null)
                {
                    PreferenceKeyConnexion.SetBool(sValue);
                }
                if (InputFieldBinding != null)
                {
                    PreferenceKeyConnexion.SetBool(sValue);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnChangedFloat(float sValue)
        {
            if (PreferenceKeyConnexion != null)
            {
                if (ToogleBinding != null)
                {
                    PreferenceKeyConnexion.SetFloat(sValue);
                }
                if (SliderBinding != null)
                {
                    PreferenceKeyConnexion.SetFloat(sValue);
                }
                if (InputFieldBinding != null)
                {
                    PreferenceKeyConnexion.SetFloat(sValue);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnChangedString(string sValue)
        {
            if (PreferenceKeyConnexion != null)
            {
                if (ToogleBinding != null)
                {
                    PreferenceKeyConnexion.SetString(sValue);
                }
                if (SliderBinding != null)
                {
                    PreferenceKeyConnexion.SetString(sValue);
                }
                if (InputFieldBinding != null)
                {
                    PreferenceKeyConnexion.SetString(sValue);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        // Update is called once per frame
        void Update()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (GetComponent<Toggle>() != null)
            {
                if (ToogleBinding != GetComponent<Toggle>())
                {
                    ToogleBinding = GetComponent<Toggle>();
                    EditorUtility.SetDirty(this);
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                };
            }
            if (GetComponent<Toggle>() != null)
            {
                if (ToogleBinding != GetComponent<Toggle>())
                {
                    ToogleBinding = GetComponent<Toggle>();
                    EditorUtility.SetDirty(this);
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                };
            }
            if (GetComponent<Slider>() != null)
            {
                if (SliderBinding != GetComponent<Slider>())
                {
                    SliderBinding = GetComponent<Slider>();
                    EditorUtility.SetDirty(this);
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                };
            }
            if (GetComponent<InputField>() != null)
            {
                if (InputFieldBinding != GetComponent<InputField>())
                {
                    InputFieldBinding = GetComponent<InputField>();
                    EditorUtility.SetDirty(this);
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
                };
            }
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
