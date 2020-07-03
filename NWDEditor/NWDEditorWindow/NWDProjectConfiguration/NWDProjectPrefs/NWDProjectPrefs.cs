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
#endif
//=====================================================================================================================

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDProjectPrefs
    {
        //-------------------------------------------------------------------------------------------------------------
        static string GetRandomAppID()
        {
            //string rRandomAppID = Application.productName;
            string rRandomAppID = Application.dataPath;
            return rRandomAppID;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void SetBool(string sKey, bool sValue)
        {
            EditorPrefs.SetBool(GetRandomAppID() + sKey, sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void SetString(string sKey, string sValue)
        {
            EditorPrefs.SetString(GetRandomAppID() + sKey, sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void SetFloat(string sKey, float sValue)
        {
            EditorPrefs.SetFloat(GetRandomAppID() + sKey, sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void SetInt(string sKey, int sValue)
        {
            EditorPrefs.SetInt(GetRandomAppID() + sKey, sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public bool GetBool(string sKey)
        {
            return EditorPrefs.GetBool(GetRandomAppID() + sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public string GetString(string sKey)
        {
            return EditorPrefs.GetString(GetRandomAppID() + sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public float GetFloat(string sKey)
        {
            return EditorPrefs.GetFloat(GetRandomAppID() + sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public int GetInt(string sKey)
        {
            return EditorPrefs.GetInt(GetRandomAppID() + sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public bool GetBool(string sKey, bool sDefaultValue)
        {
            return EditorPrefs.GetBool(GetRandomAppID() + sKey, sDefaultValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public string GetString(string sKey, string sDefaultValue)
        {
            return EditorPrefs.GetString(GetRandomAppID() + sKey, sDefaultValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public float GetFloat(string sKey, float sDefaultValue)
        {
            return EditorPrefs.GetFloat(GetRandomAppID() + sKey, sDefaultValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public int GetInt(string sKey, int sDefaultValue)
        {
            return EditorPrefs.GetInt(GetRandomAppID() + sKey, sDefaultValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public bool HasKey(string sKey)
        {
            return EditorPrefs.HasKey(GetRandomAppID() + sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void DeleteKey(string sKey)
        {
            EditorPrefs.DeleteKey(GetRandomAppID() + sKey);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif