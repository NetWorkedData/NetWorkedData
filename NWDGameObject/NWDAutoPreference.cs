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
        public NWDPreferenceKeyConnection PreferenceKey;
        //-------------------------------------------------------------------------------------------------------------
        public Toggle ToogleBinding;
        public Slider SliderBinding;
        public InputField InputFieldBinding;
        //-------------------------------------------------------------------------------------------------------------
        void Start()
        {
            NWDPreferenceKey tPreferenceKey = PreferenceKey.GetObject();
            if (tPreferenceKey != null)
            {
                if (ToogleBinding != null)
                {
                    ToogleBinding.isOn = tPreferenceKey.GetBool();
                    ToogleBinding.onValueChanged.AddListener(delegate
                    {
                        this.OnChangedBool(ToogleBinding.isOn);
                    });
                }
                if (SliderBinding != null)
                {
                    SliderBinding.value = tPreferenceKey.GetFloat();
                    SliderBinding.onValueChanged.AddListener(delegate
                    {
                        this.OnChangedFloat(SliderBinding.value);
                    });
                }
                if (InputFieldBinding != null)
                {
                    InputFieldBinding.text = tPreferenceKey.GetString();
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
            NWDPreferenceKey tPreferenceKey = PreferenceKey.GetObject();
            if (tPreferenceKey != null)
            {
                if (ToogleBinding != null)
                {
                    tPreferenceKey.SetBool(sValue);
                }
                if (SliderBinding != null)
                {
                    tPreferenceKey.SetBool(sValue);
                }
                if (InputFieldBinding != null)
                {
                    tPreferenceKey.SetBool(sValue);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnChangedFloat(float sValue)
        {
            NWDPreferenceKey tPreferenceKey = PreferenceKey.GetObject();
            if (tPreferenceKey != null)
            {
                if (ToogleBinding != null)
                {
                    tPreferenceKey.SetFloat(sValue);
                }
                if (SliderBinding != null)
                {
                    tPreferenceKey.SetFloat(sValue);
                }
                if (InputFieldBinding != null)
                {
                    tPreferenceKey.SetFloat(sValue);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnChangedString(string sValue)
        {
            NWDPreferenceKey tPreferenceKey = PreferenceKey.GetObject();
            if (tPreferenceKey != null)
            {
                if (ToogleBinding != null)
                {
                    tPreferenceKey.SetString(sValue);
                }
                if (SliderBinding != null)
                {
                    tPreferenceKey.SetString(sValue);
                }
                if (InputFieldBinding != null)
                {
                    tPreferenceKey.SetString(sValue);
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
