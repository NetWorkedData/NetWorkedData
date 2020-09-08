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
                    ToogleBinding.isOn = PreferenceKeyConnexion.GetBoolValue();
                    ToogleBinding.onValueChanged.AddListener(delegate
                    {
                        this.OnChangedBool(ToogleBinding.isOn);
                    });
                }
                if (SliderBinding != null)
                {
                    SliderBinding.value = PreferenceKeyConnexion.GetFloatValue();
                    SliderBinding.onValueChanged.AddListener(delegate
                    {
                        this.OnChangedFloat(SliderBinding.value);
                    });
                }
                if (InputFieldBinding != null)
                {
                    InputFieldBinding.text = PreferenceKeyConnexion.GetStringValue();
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
                    PreferenceKeyConnexion.SetValue(sValue);
                }
                if (SliderBinding != null)
                {
                    PreferenceKeyConnexion.SetValue(sValue);
                }
                if (InputFieldBinding != null)
                {
                    PreferenceKeyConnexion.SetValue(sValue);
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
                    PreferenceKeyConnexion.SetValue(sValue);
                }
                if (SliderBinding != null)
                {
                    PreferenceKeyConnexion.SetValue(sValue);
                }
                if (InputFieldBinding != null)
                {
                    PreferenceKeyConnexion.SetValue(sValue);
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
                    PreferenceKeyConnexion.SetValue(sValue);
                }
                if (SliderBinding != null)
                {
                    PreferenceKeyConnexion.SetValue(sValue);
                }
                if (InputFieldBinding != null)
                {
                    PreferenceKeyConnexion.SetValue(sValue);
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
