//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDEditorPrefs
    {
        //-------------------------------------------------------------------------------------------------------------
        static public void SetBool(string sKey, bool sValue)
        {
            EditorPrefs.SetBool(sKey, sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void SetString(string sKey, string sValue)
        {
            EditorPrefs.SetString(sKey, sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void SetFloat(string sKey, float sValue)
        {
            EditorPrefs.SetFloat(sKey, sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void SetInt(string sKey, int sValue)
        {
            EditorPrefs.SetInt(sKey, sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public bool GetBool(string sKey)
        {
            return EditorPrefs.GetBool(sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public string GetString(string sKey)
        {
            return EditorPrefs.GetString(sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public float GetFloat(string sKey)
        {
            return EditorPrefs.GetFloat(sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public int GetInt(string sKey)
        {
            return EditorPrefs.GetInt(sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public bool GetBool(string sKey, bool sDefaultValue)
        {
            return EditorPrefs.GetBool(sKey, sDefaultValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public string GetString(string sKey, string sDefaultValue)
        {
            return EditorPrefs.GetString(sKey, sDefaultValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public float GetFloat(string sKey, float sDefaultValue)
        {
            return EditorPrefs.GetFloat(sKey, sDefaultValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public int GetInt(string sKey, int sDefaultValue)
        {
            return EditorPrefs.GetInt(sKey, sDefaultValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public bool HasKey(string sKey)
        {
            return EditorPrefs.HasKey(sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void DeleteKey(string sKey)
        {
            EditorPrefs.DeleteKey(sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif